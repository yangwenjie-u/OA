using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BD.Jcbg.Web.xwwz
{
    public static class XwwzInit
    {
        public static string MnusStr;
        public static IList<NewsMenu> newsMenuList = null;
        public static IDictionary<int, IList<NewsCategory>> newsCategoryDList = new Dictionary<int, IList<NewsCategory>>();
        #region 服务
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
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
        #endregion

        public static void initMnus()
        {

            
            try
            {
                newsMenuList = NewsMenuService.GetAllUseByDisp();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            newsCategoryDList.Clear();
            if (newsMenuList != null)
            {
                foreach (NewsMenu itm in newsMenuList)
                {
                    // 栏目
                    if (itm.MenuType == 2)
                    {
                        IList<NewsCategory> NewsCategoryList = NewsCategoryService.GetByFatherId(itm.CategoryId.Value.ToString());
                        if (NewsCategoryList.Count > 0)
                        {
                            newsCategoryDList.Add(itm.CategoryId.Value, NewsCategoryList);

                        }
                    }


                }
            }

             

            /*
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"navigation\" >");

            foreach (NewsMenu itm in newsMenuList)
            {
                // 链接
                if (itm.MenuType == 1)
                {
                    sb.Append("<li><a href=\"" + itm.LinkUrl + "\">" + itm.MenuName + "</a></li>");
                }
                // 栏目
                else if (itm.MenuType == 2)
                {
                    sb.Append("<li><a href=\"" + myurls.GetNewsCategoryViewUrl(itm.CategoryId.Value) + "\">" + itm.MenuName + "</a>");
                    IList<NewsCategory> NewsCategoryList = NewsCategoryService.GetByFatherId(itm.CategoryId.Value.ToString());
                    if (NewsCategoryList.Count > 0)
                    {
                        sb.Append("<ul>");
                        foreach (NewsCategory category in NewsCategoryList)
                        {
                            sb.Append("<li><a href=\"" + myurls.GetNewsCategoryViewUrl(category.Categoryid) + "\">" + category.Name + "</a></li>");
                        }
                        sb.Append("</ul>");
                        sb.Append("<div class=\"clear\"></div>");

                    }
                    sb.Append("</li>");
                }
            }
            sb.Append("</ul>");
            sb.Append("<div class=\"clear\"></div>");
            MnusStr = sb.ToString();
            */
        }

        public static void init()
        {
            initMnus();
        }
    }
}