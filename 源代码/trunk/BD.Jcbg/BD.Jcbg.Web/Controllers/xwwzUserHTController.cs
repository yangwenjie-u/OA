using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD.Jcbg.Common;
using System.Web.Security;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.Web.xwwz;
using System.Web.Script.Serialization;
using System.Text;

namespace BD.Jcbg.Web.Controllers
{
    public class xwwzUserHTController : Controller
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
        /*
        private static IXUserService _xUserService = null;
        private static IXUserService XUserService
        {
            get
            {
                if (_xUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _xUserService = webApplicationContext.GetObject("XUserService") as IXUserService;
                }
                return _xUserService;
            }
        }
        */
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

        #endregion

        /// <summary>
        /// 登陆后主页
        /// </summary>
        /// <returns></returns>
         [Authorize]
        public ActionResult Index()
        {
           // 
            //XwwzSessionUser sessionuser = XwwzCurrentUser.CurUser;
            //ViewData["username"] = sessionuser.UserName;
            ViewData["username"] = CurrentUser.RealName;
            return View();
        }
        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// 修改密码界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePsw()
        {
            //XwwzSessionUser sessionuser = XwwzCurrentUser.CurUser;
            //ViewData["username"] = sessionuser.UserName;

            return View("ChangePsw");
        }


        /*
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string doChangePsw() {

            string msg;
            string oldpassword = Request["oldpassword"].GetSafeString();
            string newpassword = Request["newpassword"].GetSafeString();
            string renewpassword = Request["renewpassword"].GetSafeString();
            XwwzSessionUser sessionuser = XwwzCurrentUser.CurUser;

            if (!renewpassword.Equals(newpassword))
            {
                return "{\"success\":false,\"msg\":\"前后二次密码不一致!\"}";
            }

            bool flag = XUserService.DoChangePsw(sessionuser.UserCode, oldpassword, renewpassword,out msg);
            string ret = "{\"success\":" + flag.ToString().ToLower() + ",\"msg\":\"" + msg + "\"}";
            return ret;
        }  


        
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <returns></returns>
        public string CheckUser()
        {

            
             
            string userName = Request["userName"].GetSafeString();
            string passWord = Request["passWord"].GetSafeString();
            XUser xUser = XUserService.CheckLogin(userName, passWord);
            if (xUser!=null)
            {
                XwwzSessionUser sessionuser = new XwwzSessionUser()
                {
                    UserCode = xUser.UUsercode,
                    UserName = xUser.UUsername,
                    UpDate = xUser.UUpdate,
                    Company = xUser.UCompany,
                    Dep = xUser.UDep,
                    SupAdmin = xUser.SupAdmin,
                };
                XwwzCurrentUser.SetLoginUser(sessionuser);
                return "{\"Code\":\"1\",\"Msg\":\"登录成功\"}";
            }
            else {
                return "{\"Code\":\"0\",\"Msg\":\"用户名或密码错误\"}";
            }
        }
        public string LoginOut()
        {
            if (XwwzCurrentUser.IsLogin)
            {
                System.Web.HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                return "{\"Code\":\"1\",\"Msg\":\"退出成功\"}";
            }
            else {
                return "{\"Code\":\"0\",\"Msg\":\"退出失败!\"}";
            }
             
        }

        */

        /// <summary>
        /// 新闻发布界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult NewRelease(){
            return View("NewReleaseView");
        }



        /// <summary>
        /// 上传图片管理界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ImageManger(){

            //string url = XWConfigs.TpUrl;
            string url = "/xwwzUser/getAttachFile?aid=";
            ViewData["url"] = url;
            return View("ImageMangerView");
        }

        /// <summary>
        /// 图片新增界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult addPicAttach()
        {
            return View("addPicAttachView");
        }


        /// <summary>
        /// 菜单管理界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult  MenuManger()
        {
            return View("menuMangerView");
        }



        /// <summary>
        /// 获取菜单类型json
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string getMenuTypeList()
        {

            return "[ {\"menutype\":\"1\",\"menutypename\":\"一级菜单\"},{\"menutype\":\"2\",\"menutypename\":\"二级菜单\"}]";
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string getAllMenuList() {

            string sql = "SELECT     RECID as id , MenuName, case MenuType when 2 then 2 else 1 end as MenuType, case MenuType when 2 then '二级菜单' else '一级菜单' end as MenuTypename, LinkUrl, CategoryId, InUse, DispOrder   FROM    News_Menu ORDER BY DispOrder ";
            int pageCount = 0;
            try
            {
                IList<IDictionary<string, string>> newsArtcleList = CommonService.GetPageData(sql, 1000, 1, out pageCount);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                return "{\"success\":true,\"msg\":\"\",\"data\":{\"total\":" + pageCount + ",\"rows\":" + jss.Serialize(newsArtcleList) + "}}";

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }



        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string saveMenu()
        {
            string id = Request["id"].GetSafeString();
            string menuname = Request["menuname"].GetSafeString();
            string menutype = Request["menutype"].GetSafeString();
            string categoryid = Request["categoryid"].GetSafeString();
            string disporder = Request["disporder"].GetSafeString();
            string linkurl = Request["linkurl"].GetSafeString();
            string inuse = Request["inuse"].GetSafeString();

            try
            {
                NewsMenu newsMenu = new NewsMenu();

                newsMenu.LinkUrl = linkurl;

                if (menuname.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"菜单名称不能为空!\"}";
                }
                else 
                {
                    newsMenu.MenuName = menuname;
                }


                if (menutype.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"菜单类型不能为空!\"}";
                }
                else
                {
                    if (menutype.Equals("2"))
                    {
                        newsMenu.MenuType = 2;
                    }
                    else 
                    {
                        newsMenu.MenuType = 1;
                    }
                   
                }


                if (categoryid.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"菜单节点不能为空!\"}";
                }
                else
                {
                    newsMenu.CategoryId = categoryid.GetSafeInt();
                }


                if (disporder.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"排序不能为空!\"}";
                }
                else
                {
                    newsMenu.DispOrder = disporder.GetSafeInt();
                }

                if (inuse.ToLower().Equals("true"))
                {
                    newsMenu.InUse = true;
                }
                else
                {
                    newsMenu.InUse = false;
                }

                if (!id.Equals("")){
                    newsMenu.Recid = id.GetSafeInt();
                }

                NewsMenuService.SaveOrUpdate(newsMenu);
                XwwzInit.init();

                return "{\"success\":true,\"msg\":\"保存成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string deleteMenu()
        {

            string id = Request["id"].GetSafeString();


            try {

                if(id.Equals(""))
                    return "{\"success\":false,\"msg\":\"该菜单不存在!\"}";

                NewsMenu newsMenu = NewsMenuService.Get(id.GetSafeInt());
                if (newsMenu == null)
                    return "{\"success\":false,\"msg\":\"该菜单不存在!\"}";

                NewsMenuService.Delete(newsMenu);
                XwwzInit.init();
                return "{\"success\":true,\"msg\":\"删除成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }


        /// <summary>
        /// 栏目管理界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ColumnManger()
        {
            return View("columnMangerView");
        }



        /// <summary>
        /// 获取栏目类型json
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string getColumnTypeList()
        {
            string sql = "SELECT   CategoryId as fatherid,  MenuName  FROM  News_Menu WHERE  CategoryId<>0 and MenuType='2' ORDER BY DispOrder";

            try
            {
                int pageCount;
                IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> list = CommonService.GetPageData(sql, 1000, 1, out pageCount);

                IDictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("fatherid", "0");
                dic.Add("menuname", "空");
                ret.Add(dic);
                foreach (Dictionary<string, string> itm in list)
                {
                    itm.Remove("rowstat");
                    ret.Add(itm);
                }


                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(ret);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "[{\"fatherid\":\"0\",\"menuname\":\"空\"}]";

            }
           
        }



        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string getAllColumnList()
        {


            string name = Request["name"].GetSafeString();

            string sql = " ";
            sql = "SELECT   t1.categoryID, t1.fatherID, t1.categoryID AS id, t1.name,"
                + " case  when t2.MenuName is null then '空' else t2.MenuName end as MenuName"
                + " FROM  News_Category t1 left join News_Menu t2 on t1.fatherID = t2.categoryID where 1=1";
            int pageCount = 0;
            try
            {
                if (!name.Equals(""))
                {
                    sql += " and t1.name like '%" + name + "%'";
                }

                IList<IDictionary<string, string>> newsArtcleList = CommonService.GetPageData(sql, 1000, 1, out pageCount);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                return "{\"success\":true,\"msg\":\"\",\"data\":{\"total\":" + pageCount + ",\"rows\":" + jss.Serialize(newsArtcleList) + "}}";

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }




        /// <summary>
        /// 保存栏目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string saveColumn()
        {
            string categoryid = Request["categoryid"].GetSafeString();
            string name = Request["name"].GetSafeString();
            string fatherid = Request["fatherid"].GetSafeString();
 
            try
            {
                NewsCategory newsCategory = new NewsCategory();


                if (name.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"栏目名称不能为空!\"}";
                }
                else
                {
                    newsCategory.Name = name;
                }


                if (fatherid.Equals(""))
                {
                    return "{\"success\":false,\"msg\":\"菜单节点不能为空!\"}";
                }
                else
                {
                    newsCategory.Fatherid = fatherid.GetSafeInt();

                }



                if (!categoryid.Equals(""))
                {
                    newsCategory.Categoryid = categoryid.GetSafeInt();
                }

                NewsCategoryService.SaveOrUpdate(newsCategory);
                XwwzInit.init();

                return "{\"success\":true,\"msg\":\"保存成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }


        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string deleteColumn()
        {

            string id = Request["id"].GetSafeString();

            try
            {

                if (id.Equals(""))
                    return "{\"success\":false,\"msg\":\"该栏目不存在!\"}";

                NewsCategory newsCategory = NewsCategoryService.Get(id.GetSafeInt());
                if (newsCategory == null)
                    return "{\"success\":false,\"msg\":\"该栏目不存在!\"}";

                NewsCategoryService.Delete(newsCategory);
                XwwzInit.init();
                return "{\"success\":true,\"msg\":\"删除成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";

            }
        }





        /// <summary>
        /// 新闻列表界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string DoSearch() {
            int page = Request["page"].GetSafeInt();
            int size = Request["rows"].GetSafeInt();

            string xwbt = Request["xwbt"].GetSafeString();
            string sfsh = Request["sfsh"].GetSafeString();
            string xwlm = Request["xwlm"].GetSafeString();
            string zdxw = Request["zdxw"].GetSafeString();

            if (page <= 0)
                page = 1;
            if (size <= 0)
                size = 20;
            int pageCount;

            string sqlWhere = "";

            if (!xwbt.Equals(""))
            {
                sqlWhere += " and t1.articleTitle like '%" + xwbt + "%' ";
            }
            switch (sfsh)
            {
                case "YSH":
                    sqlWhere += " and t1.isAudited ='true' ";
                    break;
                case "WSH":
                    sqlWhere += " and t1.isAudited ='false' ";
                    break;
            }

            if (!xwlm.Equals("ALL") && !xwlm.Equals(""))
            {
                sqlWhere += " and t2.categoryID=" + xwlm + " ";
            }

            if (zdxw.Equals("1"))
            {
                sqlWhere += "  and t1.IsImportant ='true' ";
            }

            string sql = "SELECT   articleID, articleTitle as title,CONVERT(varchar(20) , articleDate, 120 ) as articleDate,hits,t2.name,IsImportant,isAudited,isAudited as isbtn  "
                       + "  FROM    News_artcle t1 left join  News_Category t2 on t1.categoryID = t2.categoryID  "
                       + " where 1=1 " + sqlWhere + " order by articleDate desc ";

            try {
                IList<IDictionary<string, string>> newsArtcleList = CommonService.GetPageData(sql, size, page, out pageCount);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                return "{\"success\":true,\"msg\":\"\",\"data\":{\"total\":" + pageCount + ",\"rows\":" + jss.Serialize(newsArtcleList) + "}}";
              
            }catch(Exception ex){
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex .Message+ "\"}";
              
            }

        }


        

        /// <summary>
        /// 上传图片查询
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string DoSearchPicAttach()
        {
            int page = Request["page"].GetSafeInt();
            int size = Request["rows"].GetSafeInt();

            string dname = Request["dname"].GetSafeString();

            if (page <= 0)
                page = 1;
            if (size <= 0)
                size = 10;
            int pageCount;

            string sqlWhere = "";

            if (!dname.Equals(""))
            {
                sqlWhere += " and docName like '%" + dname + "%' ";
            }
            

            string sql = "SELECT    attachID, articleID, docName, saveName FROM   News_Attach "
                       + " where articleID = -1  " + sqlWhere + " order by attachID desc ";

            try {
                IList<IDictionary<string, string>> newsArtcleList = CommonService.GetPageData(sql, size, page, out pageCount);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                return "{\"success\":true,\"msg\":\"\",\"data\":{\"total\":" + pageCount + ",\"rows\":" + jss.Serialize(newsArtcleList) + "}}";
              
            }catch(Exception ex){
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex .Message+ "\"}";
              
            }

        }



        /// <summary>
        /// 禁止发布
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string jzfb()
        {

            int articleid = Request["id"].GetSafeInt();
            try
            {
                NewsArtcle newsArtcle = NewsArtcleService.Get(articleid);
                if (newsArtcle == null)
                {
                    return "{\"success\":false,\"msg\":\"找不到对应新闻!\"}";
                }
                newsArtcle.IsAudited = false;
                NewsArtcleService.Update(newsArtcle);
                return "{\"success\":true,\"msg\":\"禁止发布成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }


        }

        /// <summary>
        /// 审核签发
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string shqf()
        {

            int articleid = Request["id"].GetSafeInt();
            try
            {
                NewsArtcle newsArtcle = NewsArtcleService.Get(articleid);
                if (newsArtcle == null)
                {
                    return "{\"success\":false,\"msg\":\"找不到对应新闻!\"}";
                }
                newsArtcle.IsAudited = true;
                //newsArtcle.ArticleDate = DateTime.Now;
                NewsArtcleService.Update(newsArtcle);
                return "{\"success\":true,\"msg\":\"审核签发成功!\"}";
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }


        }




        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string deleteNewsArtcle()
        {
            string msg;
            bool flag;
            int articleid = Request["id"].GetSafeInt();
            try
            {
                NewsArtcle newsArtcle = NewsArtcleService.Get(articleid);
                if (newsArtcle == null)
                {
                    return "{\"success\":false,\"msg\":\"找不到对应新闻!\"}";
                }

                flag = NewsArtcleService.deleteNewsArtcle(newsArtcle, out msg);

                
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }

            return "{\"success\":" + flag.GetSafeString().ToLower() + ",\"msg\":\"" + msg + "\"}";
        }



        /// <summary>
        /// 删除新闻附件
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string deleteNewsAttach()
        {
            string msg;
            bool flag;
            int attachid = Request["id"].GetSafeInt();
            try
            {
                flag = NewsAttachService.deleteNewsAttach(attachid,out msg);
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }

            return "{\"success\":" + flag.GetSafeString().ToLower() + ",\"msg\":\"" + msg + "\"}";
        }

        


        [Authorize]
        public ActionResult doNewsArtcle()
        {

            string type = Request["type"].GetSafeString();
            string articleid = Request["id"].GetSafeString();


            IList<IDictionary<string, string>> tpxwret = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> fjxwret = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> fjret = new List<IDictionary<string, string>>();

            ViewData["tpxw"] = tpxwret;
            ViewData["fjxw"] = fjxwret;
            ViewData["fj"] = fjret;


            NewsArtcle newsArtcle = new NewsArtcle();
            ViewData["buttonshow"] = "1";
            if (type.Equals("edit"))//修改时
            {
                newsArtcle = NewsArtcleService.Get(articleid.GetSafeInt());
                if (newsArtcle==null)
                {
                    ViewData["buttonshow"] = "0";
                }
                ViewData["type"] = "edit";
                string sql = "";



                //查询图片新闻附件
                sql = "select  articleid,attachid,savename,docname from  News_Attach where articleID = " + newsArtcle.Articleid + " "
                    + " and attachid in (select imageUrl from News_artcle where articleID = " + newsArtcle.Articleid + " ) ";
                tpxwret = CommonService.GetDataTable(sql);
                ViewData["tpxw"] = tpxwret;
                //查询文件新闻附件
                sql = "select  articleid,attachid,savename,docname from  News_Attach where articleID = " + newsArtcle.Articleid + " "
                    + " and attachid in (select fileName from News_artcle where articleID = " + newsArtcle.Articleid + " ) ";
                fjxwret = CommonService.GetDataTable(sql);
                ViewData["fjxw"] = fjxwret;
                //查询附件
                sql = "select   attachID, articleID, docName, saveName FROM  News_Attach where  attachid not in (" + newsArtcle.FileName.GetSafeInt() + ") "
                    + " and attachid not in (" + newsArtcle.ImageUrl.GetSafeInt() + ") and  articleID=" + newsArtcle.Articleid;
                //sql = "select t1.articleid,t1.attachid,t1.savename,t1.docname from  News_Attach t1 left join  News_artcle t2 on t1.articleID = t2.articleID "
              //      + " where   t2.articleID = " + newsArtcle.Articleid + " and   (t1.attachid <> t2.fileName and t1.attachid <> t2.imageUrl)";
                fjret = CommonService.GetDataTable(sql);
                ViewData["fj"] = fjret;


            }
            else {//新增时
                newsArtcle.Articleid = -1;
                newsArtcle.ArticleDate = DateTime.Now;
                newsArtcle.ArticleContent = "";

                ViewData["type"] = "add";
            }

            newsArtcle.ArticleContent = Convert.ToBase64String(Encoding.Default.GetBytes(newsArtcle.ArticleContent));

            ViewData["newsArtcle"] = newsArtcle;
            return View("NewsArtcle");
        }

        


        /// <summary>
        /// 保存新闻信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string saveNewsArtcle()
        {

            string ret;
            string msg;

            string islink = Request["islink"].GetSafeString();//是否为链接新闻
            string articlelink = Request["articlelink"].GetSafeString();//链接新闻地址
            string isimage = Request["isimage"].GetSafeString();//是否图片新闻
            string isimportant = Request["isimportant"].GetSafeString();//是否重点新闻
            string articletitle = Request["articletitle"].GetSafeString();//新闻标题
            string categoryid = Request["categoryid"].GetSafeString();//栏目id
            string articledate = Request["articledate"].GetSafeString();//日期
            string articlekey = Request["articlekey"].GetSafeString();//关键字
            string articlefrom = Request["articlefrom"].GetSafeString();//来源
            string editor = Request["context"].GetSafeString();//新闻内容
            string articleid = Request["id"].GetSafeString();//新闻id
            string type = Request["type"].GetSafeString();//类型 edit or add 新增还是修改
            string isfile = Request["isfile"].GetSafeString();//是否文件

            bool delimageUrl = false;
            bool delfileName = false;

            try {
              //  editor = HttpUtility.UrlDecode(editor, Encoding.UTF8);

                //string strPath = "aHR0cDovLzIwMy44MS4yOS40Njo1NTU3L19iYWlkdS9yaW5ncy9taWRpLzIwMDA3MzgwLTE2Lm1pZA==";
              //  byte[] outputb = Convert.FromBase64String(strPath);

                //XwwzSessionUser sessionuser = XwwzCurrentUser.CurUser;

                editor = Encoding.Default.GetString(Convert.FromBase64String(editor));
                /*
                if (editor.EndsWith("\n"))
                {
                    editor = editor.Substring(0, editor.Length - 2);
                }
                  */

                NewsArtcle newsArtcle = new NewsArtcle();
                //附件列表
                IList<NewsAttach> newsAttachList = new List<NewsAttach>();

                //newsArtcle新增时的初始值
                newsArtcle.CreatedOn = DateTime.Now;
                newsArtcle.CreatedBy = CurrentUser.UserName; //sessionuser.UserName;
                newsArtcle.Hits = 0;
                newsArtcle.Templateid = 0;
                 

                if (type.Equals("edit"))
                {
                    //查询原先在的新闻
                    newsArtcle = NewsArtcleService.Get(articleid.GetSafeInt());
                    if (newsArtcle == null)
                    {
                        return  "{\"success\":false,\"msg\":\"找不到该新闻信息!\"}";
                    }
                }
                /********************文本字段赋值***************************************/
                if (islink.Equals("on"))
                {
                    newsArtcle.IsLink = true;
                    newsArtcle.ArticleLink = articlelink;
                }
                else {
                    newsArtcle.IsLink = false;
                    newsArtcle.ArticleLink = "";
                }

                if (isimage.Equals("on"))
                {
                    newsArtcle.IsImage = true;
                }
                else
                {
                    newsArtcle.IsImage = false;
                    //newsArtcle.ImageUrl = "";
                    //需要删除图片新闻
                    delimageUrl = true;
                }

                if (isimportant.Equals("on"))
                {
                    newsArtcle.IsImportant = true;
                }
                else
                {
                    newsArtcle.IsImportant = false;
                }

                if (isfile.Equals("on"))
                {
                    newsArtcle.IsFile = true;
                }
                else
                {
                    newsArtcle.IsFile = false;
                    //newsArtcle.FileName = "";
                    //需要删除文件新闻
                    delfileName = true;
                }

                newsArtcle.ArticleTitle = articletitle;
                newsArtcle.Categoryid = categoryid.GetSafeInt();
                
                DateTime dt = DateTime.Now;
                if (!articledate.Equals(""))
                {
                    dt = DateTime.ParseExact(articledate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }
                newsArtcle.ArticleDate = dt;

                newsArtcle.ArticleKey = articlekey;
                newsArtcle.ArticleFrom = articlefrom;
                newsArtcle.ArticleContent = editor; 

                /********************文本字段赋值***************************************/

                /**附件*************************/
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string filename = "";
                        string zdname = "";
                        HttpPostedFileBase postfile = Request.Files[i];
                        NewsAttach newsAttach = new NewsAttach();
                        if (postfile.ContentLength>0)
                        {
                            zdname = Request.Files.AllKeys[i].GetSafeString();
                            if (filename == "")
                                filename = postfile.FileName.Substring(postfile.FileName.LastIndexOf("\\") + 1); 
                            // 读取文件
                            byte[] postcontent = new byte[postfile.ContentLength];
                            int readlength = 0;
                            while (readlength < postfile.ContentLength)
                            {
                                int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                                readlength += tmplen;
                            }

                            newsAttach.DocName = filename;

                            if (zdname.StartsWith("imagelink"))
                            {
                                filename = "Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwtp_" + filename;
                                //newsArtcle.ImageUrl = filename;
                                //需要删除图片新闻
                                delimageUrl = true;
                            }
                            if (zdname.StartsWith("filelink"))
                            {
                                filename = "Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwwj_" + filename;
                                //newsArtcle.FileName = filename;
                                //需要删除文件新闻
                                delfileName = true;
                            }

                            newsAttach.SaveName = filename;
                            newsAttach.Filecontent = postcontent;
                            newsAttachList.Add(newsAttach);
                        }
                    }
                }

                /**附件*************************/


                bool flag = NewsArtcleService.saveNewsArtcle(newsArtcle, newsAttachList, type,delimageUrl,delfileName, out msg);

                ret = flag.GetSafeString().ToLower();


            }
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
                return "{\"success\":false,\"msg\":\""+e.Message+"\"}";
			}



            return "{\"success\":" + ret + ",\"msg\":\"" + msg + "\"}";
        }


        /// <summary>
        /// 保存上传图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string savePicAttachByCkeditor()
        {

            string ret;
            string msg;

            string savename = "页面文件";//图片标题
            string callback = Request["CKEditorFuncNum"].GetSafeString();//图片标题
            string path = "";

            try
            {
                string url = "/xwwzUser/getAttachFile?aid=";
                ViewData["url"] = url;


                NewsAttach newsAttach = new NewsAttach();
                newsAttach.Articleid = -1;
                newsAttach.SaveName = savename;

                /**附件*************************/
                if (Request.Files.Count > 0)
                {
                    string filename = "";
                    string zdname = "";
                    HttpPostedFileBase postfile = Request.Files[0];
                    if (postfile.ContentLength > 0)
                    {

                        zdname = Request.Files.AllKeys[0].GetSafeString();
                        if (filename == "")
                        {
                            filename = postfile.FileName.Substring(postfile.FileName.LastIndexOf("\\") + 1);
                            filename = filename.Substring(0,filename.LastIndexOf("."));
                        }
                        if (filename != "") {
                            newsAttach.SaveName = filename;
                        }
                        // 读取文件
                        byte[] postcontent = new byte[postfile.ContentLength];
                        int readlength = 0;
                        while (readlength < postfile.ContentLength)
                        {
                            int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                            readlength += tmplen;
                        }
                        newsAttach.DocName = filename;
                        newsAttach.Filecontent = postcontent;
                    }
                    else
                    {
                        return "{\"success\":false,\"msg\":\"上传文件为空!\"}";
                    }
                }
                else
                {
                    return "{\"success\":false,\"msg\":\"没有上传文件!\"}";
                }

                /**附件*************************/


                bool flag = NewsAttachService.saveNewsAttach(newsAttach, out msg);

                ret = flag.GetSafeString().ToLower();

                path = url+newsAttach.Attachid.GetSafeString();
                Response.Write("<script type=\"text/javascript\">window.parent.CKEDITOR.tools.callFunction(" + callback + ",'" + path + "',''" + ")</script>");

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                return "{\"success\":false,\"msg\":\"" + e.Message + "\"}";
            }

            return "{\"success\":" + ret + ",\"msg\":\"" + msg + "\"}";

        }
        

        /// <summary>
        /// 保存上传图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string savePicAttach() {

            string ret;
            string msg;

            string savename = Request["savename"].GetSafeString();//图片标题


            try
            {



                NewsAttach newsAttach = new NewsAttach();
                newsAttach.Articleid = -1;
                newsAttach.SaveName = savename;

                /**附件*************************/
                if (Request.Files.Count > 0)
                {
                    string filename = "";
                    string zdname = "";
                    HttpPostedFileBase postfile = Request.Files[0];
                    if (postfile.ContentLength > 0)
                    {

                        zdname = Request.Files.AllKeys[0].GetSafeString();
                        if (filename == "")
                            filename = postfile.FileName.Substring(postfile.FileName.LastIndexOf("\\") + 1);
                        // 读取文件
                        byte[] postcontent = new byte[postfile.ContentLength];
                        int readlength = 0;
                        while (readlength < postfile.ContentLength)
                        {
                            int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                            readlength += tmplen;
                        }
                        newsAttach.DocName = filename;
                        newsAttach.Filecontent = postcontent;
                    }
                    else {
                        return "{\"success\":false,\"msg\":\"上传文件为空!\"}";
                    }
                }
                else
                {
                    return "{\"success\":false,\"msg\":\"没有上传文件!\"}";
                }

                /**附件*************************/


                bool flag = NewsAttachService.saveNewsAttach(newsAttach, out msg);

                ret = flag.GetSafeString().ToLower();


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                return "{\"success\":false,\"msg\":\"" + e.Message + "\"}";
            }



            return "{\"success\":" + ret + ",\"msg\":\"" + msg + "\"}";

        }
        


        /// <summary>
        /// 获取附件
        /// </summary>
        [Authorize]
        public void getAttachFile()
        {
            byte[] ret = null;

            string err = "";
            string attachname = "";
            string attachID = Request["aid"].GetSafeString();
            try
            {
                NewsAttach newsAttach = new NewsAttach();
                // string sql = "";
                newsAttach = NewsAttachService.Get(attachID.GetSafeInt());
                //IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (newsAttach.Filecontent != null)//dt.Count > 0
                {
                    // ret = dt[0]["filecontent"] as byte[];
                    //attachname = dt[0]["docname"] as string;
                    ret = newsAttach.Filecontent;
                    attachname = newsAttach.DocName;

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



        /// <summary>
        /// 获取新闻栏目列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string xwlmcom() {

            try {

                string needall = Request["needall"].GetSafeString();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                IList<NewsCategory> newsCategoryList = NewsCategoryService.GetByLeafTrue();
                IList<IDictionary<string, string>>  dics = new   List<IDictionary<string, string>>();
                IDictionary<string, string> dic = new Dictionary<string, string>();

                if (!needall.Equals("0"))//需要添加全部
                {
                    dic.Add("id", "ALL");
                    dic.Add("text", "所有栏目");
                    dic.Add("selected", "true");
                    dics.Add(dic);
                }

                foreach (NewsCategory item in newsCategoryList)
                {
                    dic = new Dictionary<string, string>();
                    dic.Add("id", item.Categoryid.GetSafeString());
                    dic.Add("text", item.Name);
                    dics.Add(dic);
                }

                return "{\"success\":true,\"msg\":\"\",\"data\":" + jss.Serialize(dics) + "}";

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                return "{\"success\":false,\"msg\":\"" + ex.Message + "\"}";
            }
            
          

        }


        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string GetMenus() {

            string ret = "";

            // ret = "[{\"IsGroup\":true,\"MenuCode\":\"code\",\"MenuName\":\"菜单\",\"MenuUrl\":\"\"},{\"IsGroup\":false,\"MenuCode\":\"ChangePsw\",\"MenuName\":\"修改密码\",\"MenuUrl\":\"ChangePsw\"},{\"IsGroup\":false,\"MenuCode\":\"NewRelease\",\"MenuName\":\"新闻发布\",\"MenuUrl\":\"NewRelease\"},{\"IsGroup\":false,\"MenuCode\":\"ImageManger\",\"MenuName\":\"上传图片管理\",\"MenuUrl\":\"ImageManger\"}]";
            ret = "[{\"IsGroup\":true,\"MenuCode\":\"code\",\"MenuName\":\"菜单\",\"MenuUrl\":\"\"},{\"IsGroup\":false,\"MenuCode\":\"ChangePsw\",\"MenuName\":\"修改密码\",\"MenuUrl\":\"ChangePsw\"},{\"IsGroup\":false,\"MenuCode\":\"NewRelease\",\"MenuName\":\"新闻发布\",\"MenuUrl\":\"NewRelease\"},{\"IsGroup\":false,\"MenuCode\":\"ImageManger\",\"MenuName\":\"上传图片管理\",\"MenuUrl\":\"ImageManger\"},{\"IsGroup\":false,\"MenuCode\":\"MenuManger\",\"MenuName\":\"菜单管理\",\"MenuUrl\":\"MenuManger\"},{\"IsGroup\":false,\"MenuCode\":\"ColumnManger\",\"MenuName\":\"栏目管理\",\"MenuUrl\":\"ColumnManger\"}]";

            return ret;
        }

        /// <summary>
        /// 保存办公系统生成的曝光台新闻
        /// </summary>
        /// <returns></returns>
        public string saveNewsArtcleFromBG()
        {

            string ret;
            string msg;

            string islink = Request["islink"].GetSafeString();//是否为链接新闻
            string articlelink = Request["articlelink"].GetSafeString();//链接新闻地址
            string isimage = Request["isimage"].GetSafeString();//是否图片新闻
            string isimportant = Request["isimportant"].GetSafeString();//是否重点新闻
            string articletitle = Request["articletitle"].GetSafeString();//新闻标题
            string categoryid = Request["categoryid"].GetSafeString();//栏目id
            string articledate = Request["articledate"].GetSafeString();//日期
            string articlekey = Request["articlekey"].GetSafeString();//关键字
            string articlefrom = Request["articlefrom"].GetSafeString();//来源
            string editor = Request["context"].GetSafeString();//新闻内容
            string articleid = Request["id"].GetSafeString();//新闻id
            string type = Request["type"].GetSafeString();//类型 edit or add 新增还是修改
            string isfile = Request["isfile"].GetSafeString();//是否文件
            string createdby = Request["createdby"].GetSafeString();//创建者

            bool delimageUrl = false;
            bool delfileName = false;

            try
            {
                //  editor = HttpUtility.UrlDecode(editor, Encoding.UTF8);

                //string strPath = "aHR0cDovLzIwMy44MS4yOS40Njo1NTU3L19iYWlkdS9yaW5ncy9taWRpLzIwMDA3MzgwLTE2Lm1pZA==";
                //  byte[] outputb = Convert.FromBase64String(strPath);

                //XwwzSessionUser sessionuser = XwwzCurrentUser.CurUser;

                editor = editor.DecodeBase64();
                /*
                if (editor.EndsWith("\n"))
                {
                    editor = editor.Substring(0, editor.Length - 2);
                }
                  */

                NewsArtcle newsArtcle = new NewsArtcle();
                //附件列表
                IList<NewsAttach> newsAttachList = new List<NewsAttach>();

                //newsArtcle新增时的初始值
                newsArtcle.CreatedOn = DateTime.Now;
                newsArtcle.CreatedBy = createdby; //sessionuser.UserName;
                newsArtcle.Hits = 0;
                newsArtcle.Templateid = 0;


                
                /********************文本字段赋值***************************************/
                if (islink.Equals("on"))
                {
                    newsArtcle.IsLink = true;
                    newsArtcle.ArticleLink = articlelink;
                }
                else
                {
                    newsArtcle.IsLink = false;
                    newsArtcle.ArticleLink = "";
                }

                if (isimage.Equals("on"))
                {
                    newsArtcle.IsImage = true;
                }
                else
                {
                    newsArtcle.IsImage = false;
                    //newsArtcle.ImageUrl = "";
                    //需要删除图片新闻
                    delimageUrl = true;
                }

                if (isimportant.Equals("on"))
                {
                    newsArtcle.IsImportant = true;
                }
                else
                {
                    newsArtcle.IsImportant = false;
                }

                if (isfile.Equals("on"))
                {
                    newsArtcle.IsFile = true;
                }
                else
                {
                    newsArtcle.IsFile = false;
                    //newsArtcle.FileName = "";
                    //需要删除文件新闻
                    delfileName = true;
                }

                newsArtcle.ArticleTitle = articletitle;
                newsArtcle.Categoryid = categoryid.GetSafeInt();

                DateTime dt = DateTime.Now;
                if (!articledate.Equals(""))
                {
                    dt = DateTime.ParseExact(articledate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }
                newsArtcle.ArticleDate = dt;

                newsArtcle.ArticleKey = articlekey;
                newsArtcle.ArticleFrom = articlefrom;
                newsArtcle.ArticleContent = editor;

                /********************文本字段赋值***************************************/

                /**附件*************************/
                if (Request.Files.Count > 0)
                {
                    //SysLog4.WriteError("新闻网站接口，获取到的文件数量：" + Request.Files.Count.ToString());
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string filename = "";
                        string zdname = "";
                        HttpPostedFileBase postfile = Request.Files[i];
                        NewsAttach newsAttach = new NewsAttach();
                        if (postfile.ContentLength > 0)
                        {
                            zdname = Request.Files.AllKeys[i].GetSafeString();
                            if (filename == "")
                                filename = postfile.FileName.Substring(postfile.FileName.LastIndexOf("\\") + 1);
                            // 读取文件
                            byte[] postcontent = new byte[postfile.ContentLength];
                            int readlength = 0;
                            while (readlength < postfile.ContentLength)
                            {
                                int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                                readlength += tmplen;
                            }

                            newsAttach.DocName = filename;

                            if (zdname.StartsWith("imagelink"))
                            {
                                filename = "Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwtp_" + filename;
                                //newsArtcle.ImageUrl = filename;
                                //需要删除图片新闻
                                delimageUrl = true;
                            }
                            if (zdname.StartsWith("filelink"))
                            {
                                filename = "Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwwj_" + filename;
                                //newsArtcle.FileName = filename;
                                //需要删除文件新闻
                                delfileName = true;
                            }

                            newsAttach.SaveName = filename;
                            newsAttach.Filecontent = postcontent;
                            newsAttachList.Add(newsAttach);
                        }
                    }
                }

                /**附件*************************/


                bool flag = NewsArtcleService.saveNewsArtcle(newsArtcle, newsAttachList, type, delimageUrl, delfileName, out msg);
                //SysLog4.WriteError(newsAttachList.Count.ToString());
                //SysLog4.WriteError(flag.ToString());
                if (flag)
                {
                    if (newsAttachList.Count > 1)
                    {
                        for (int i = 1; i < newsAttachList.Count; i++)
                        {
                            newsArtcle.ArticleContent = newsArtcle.ArticleContent.Replace("{fileid" + (i - 1).ToString() + "}", newsAttachList[i].Attachid.ToString());
                            newsArtcle.Articleid = newsAttachList[i].Articleid.Value;
                        }
                        NewsArtcleService.Update(newsArtcle);
                    }

                }

                ret = flag.GetSafeString().ToLower();


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                return "{\"success\":false,\"msg\":\"" + e.Message + "\"}";
            }



            return "{\"success\":" + ret + ",\"msg\":\"" + msg + "\"}";
        }




    }
}