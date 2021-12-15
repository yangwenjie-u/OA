using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.IBll;

namespace BD.Jcbg.Web.Controllers
{
    public class HkwsController : Controller
    {

        #region 页面
        public ActionResult Play()
        {
            ViewBag.url = Request["url"].GetSafeString();
            return View();
        }
        #endregion

    }
}
