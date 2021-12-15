using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 用户各种帮助文档页面
    /// </summary>
    public class HelpController : Controller
    {
        #region 页面
        /// <summary>
        /// 浏览器安装说明
        /// </summary>
        /// <returns></returns>
        public ActionResult IeReadme()
        {
            return View();
        }
        #endregion
    }
}