using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 查询统计
    /// </summary>
    public class CxtjController:Controller
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

        #endregion

        #region 页面
        /// <summary>
        /// 统计时某工程的企业、时间选择
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gcqysjxz()
        {
            string gcbh = Request["gcbh"].GetSafeRequest();
            try
            {
                ViewBag.url = Request["nexturl"].GetSafeRequest();
                IList<IDictionary<string, string>> qys = CommonService.GetDataTable("select gcqybh,qymc,qylxmc from View_GC_QY where gcbh='" + gcbh + "'");
                ViewBag.jsonstr = new JavaScriptSerializer().Serialize(qys);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);

            }
            return View();
        }
        #endregion

        #region 获取数据
        
        #endregion

        #region 更新数据
        
        #endregion

    }
}