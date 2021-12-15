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
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.IBll;

namespace BD.Jcbg.Web.Controllers
{
    public class Info_YJController : Controller
    {
        #region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                }
                return _commonService;
            }
        }
        private IOaService _oaService = null;
        private IOaService OaService
        {
            get
            {
                if (_oaService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _oaService = webApplicationContext.GetObject("OaService") as IOaService;
                }
                return _oaService;
            }
        }
        private IWorkFlowService _workFlowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workFlowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workFlowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workFlowService;
            }
        }

        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                try
                {
                    if (_jcService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcService;
            }
        }

        #endregion

        /// <summary>
        /// 获取人员预警数量
        /// </summary>
        [Authorize]
        public void getRYYJSum()
        {
            int count = 0;
            try
            {
                string sql = "select count(*) as ryyjnum from ViewKqjUserMonthPay where (TX_YJZT=1 or FF_YJZT=1) and jdzch in (select g.gcbh from i_m_gc g, View_H_ZFZH_XQ b where g.SZSF=b.SZSF AND (g.SZCS=b.SZCS or b.SZCS is null or b.SZCS='') AND (g.SZXQ=b.SZXQ or b.SZXQ is null or b.SZXQ='')  and  b.usercode='"+CurrentUser.UserName+"' AND (g.szjd=b.szjd or b.szjd is null or b.szjd='')) ";
                IList<IDictionary<string, string>> yjdata=CommonService.GetDataTable(sql);
                if(yjdata.Count!=0)
                {
                    count = yjdata[0]["ryyjnum"].GetSafeInt();
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"count\":{0}}}", count));
            }
        }
        /// <summary>
        /// 薪资预警数量
        /// </summary>
        [Authorize]
        public void getXZYJSum()
        {
            int count = 0;
            try
            {
                string sql = "select count(*) as xzyjnum from INFO_YJ_XZ where (xzdw=0 or xzze=0) and gcbh in (select g.gcbh from i_m_gc g, View_H_ZFZH_XQ b where g.SZSF=b.SZSF AND (g.SZCS=b.SZCS or b.SZCS is null or b.SZCS='') AND (g.SZXQ=b.SZXQ or b.SZXQ is null or b.SZXQ='')  and  b.usercode='"+CurrentUser.UserName+"' AND (g.szjd=b.szjd or b.szjd is null or b.szjd='')) ";               
                IList<IDictionary<string, string>> yjdata=CommonService.GetDataTable(sql);
                if(yjdata.Count!=0)
                {
                    count = yjdata[0]["xzyjnum"].GetSafeInt();
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"count\":{0}}}", count));
            }
        }
        
    }
}