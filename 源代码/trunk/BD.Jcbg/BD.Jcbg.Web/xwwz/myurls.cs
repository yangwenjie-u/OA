using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.xwwz
{
    public class myurls
    {
        #region 登录后首页

        #endregion

        #region 无权限页

        #endregion

        #region 图片，css，js路径

        #endregion
        #region 系统管理路径
        #region 部门管理

        #endregion
        #region 角色管理

        #endregion
        #region 用户管理

        #endregion
        #endregion
        #region 新闻管理路径
        #region 新闻栏目

        #endregion
        #region 新闻
        public static string GetNewsCategoryViewUrl(NewsMenu newsMenu, NewsCategory newsCategory)
        {//cid-categoryid,rid-recid,fid-fatherid
            return "/xwwzUser/newlb?cid=" + newsCategory.Categoryid + "&rid=" + newsMenu.Recid + "&fid=" + newsMenu.CategoryId;
        }
        public static string GetNewsMenu2ViewUrl(NewsMenu newsMenu)
        {//rid-recid,fid-fatherid
            return "/xwwzUser/newlb?fid=" + newsMenu.CategoryId+"&rid="+newsMenu.Recid;
        }
        public static string GetNewsMenuViewUrl(NewsMenu newsMenu)
        {//rid-recid 
            string returl = "/";
            if (newsMenu.LinkUrl.GetSafeString().IndexOf("?") > -1)
            {
                returl += newsMenu.LinkUrl + "&rid=" + newsMenu.Recid;
            }
            else if (newsMenu.LinkUrl != "")
            {
                returl = newsMenu.LinkUrl;
            }
            return    returl;
        }


        public static string GetNewsLbUrl(int fid, int rid, int cid, IDictionary<string, string> dic)
        {//rid-recid 
            string returl = "";
            if (dic["isfile"].GetSafeBool())
            {
                returl = "'/xwwzUser/getAttachFile',{id:'" + dic["articleid"].GetSafeString() + "',name:'" + dic["filename"].GetSafeString() + "'}";
            }
            else if (dic["islink"].GetSafeBool())
            {
                returl = "'" + dic["articlelink"].GetSafeString() + "',{},'_blank'";

            }
            else 
            {
                returl = "'/xwwzUser/newsc',{id:'" + dic["articleid"].GetSafeString() + "',fid:'" + fid + "',rid:'" + rid + "'},'_blank'";
        
            }
            return returl;
        }



        public static string downNewsAttachUrl(IDictionary<string, string> dic)
        {//rid-recid 
            string returl = "";

            returl = "'/xwwzUser/getAttachFile',{aid:'" + dic["attachid"].GetSafeString() + "'}";

            return returl;
        }

        #endregion
        #region 菜单


        #endregion
        #endregion

    }
}