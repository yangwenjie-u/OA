using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using BD.DataInputCommon;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using SysLog4 = BD.Jcbg.Common.SysLog4;
using System.Web;

namespace BD.Jcbg.Web.Controllers
{
    public class PayController : Controller
    {

        #region 服务
        private IPayService _payService = null;
        private IPayService PayService
        {
            get
            {
                try
                {
                    if (_payService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _payService = webApplicationContext.GetObject("PayService") as IPayService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _payService;
            }
        }
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                try
                {
                    if (_commonService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _commonService;
            }
        }
        private IExcelService _excelService = null;
        private IExcelService ExcelService
        {
            get
            {
                try
                {
                    if (_excelService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _excelService = webApplicationContext.GetObject("ExcelService") as IExcelService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _excelService;
            }
        }
        #endregion

        #region 页面
        /// <summary>
        /// 登录成功后主界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Main()
        {
            // 查询上次登陆时间
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select top 1 logtime from syslog where username='" + CurrentUser.UserName + "' and operation='用户登录' and result=1 order by logtime desc");
            if (dt.Count > 0)
            {
                CurrentUser.CurUser.LastLoginTime = dt[0]["logtime"].GetSafeDate();
            }

            return View();
        }
        /// <summary>
        /// 项目管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ProjectList()
        {
            return View();
        }
        /// <summary>
        /// 工人管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EmployeeList()
        {
            return View();
        }
        /// <summary>
        /// 额度划拨
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MoneyTrans()
        {
            return View();
        }
        /// <summary>
        /// 工资发放
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MoneyPay()
        {
            return View();
        }
        /// <summary>
        /// 企业管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CompanyList()
        {
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult PassModify()
        {
            return View();
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult InfoModify()
        {
            return View();
        }
        /// <summary>
        /// 账单查询
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult BillSearch()
        {
            return View();
        }
        #endregion

        #region 服务
        /// <summary>
        /// 创建虚拟账号
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoCreateVirtualAccount(string lx, string bh)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = PayService.SetAccountSign(lx, bh, CurrentUser.UserCode, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg});
        }
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetQyAccountInfo()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string,string>> datas = null;
            try
            {
                code = PayService.GetAccountInfo("q", CurrentUser.UserCode, out datas, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records=datas});
        }
        [Authorize]
        public JsonResult GetQySubAccounts(string gcbh, string qybh, int pagesize, int pageindex)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            int totalcount = 0;
            try
            {
                datas = PayService.GetSubAccounts(gcbh, qybh, CurrentUser.UserCode,pagesize, pageindex, out totalcount, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas, totalcount = totalcount });
        }
        /// <summary>
        /// 删除发放单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoDeleteGcffdw(string id)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = PayService.DeleteGcFfdw(id, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 额度划出，从企业账号到工程账户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoMoneyOut(string tozhid, decimal ed, string bz)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = PayService.MoneyOut(CurrentUser.UserCode, tozhid, ed, bz, CurrentUser.UserCode, CurrentUser.RealName, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 额度划入，从企业账号到工程账户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoMoneyIn(string fromzhid, decimal ed, string bz)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = PayService.MoneyIn(fromzhid, CurrentUser.UserCode, ed, bz, CurrentUser.UserCode, CurrentUser.RealName, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 获取划拨记录
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetMoneyTransList(string hbzh, int pagesize, int pageindex)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            int totalcount = 0;
            try
            {
                hbzh = hbzh.GetSafeString();
                if (pagesize.GetSafeInt() == 0)
                    pagesize = 10;
                if (pageindex.GetSafeInt() < 1)
                    pageindex = 1;
                datas = PayService.GetMoneyTransList(CurrentUser.UserCode, hbzh, pagesize, pageindex, out totalcount, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas, totalcount=totalcount });
        }
        /// <summary>
        /// 获取当前人员各个工地发放摘要
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetProjectPayHistorySummary(string gcmc, string sgdw, string ffzh, int pagesize, int pageindex)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            int totalcount = 0;
            try
            {
                if (pagesize.GetSafeInt() == 0)
                    pagesize = 10;
                if (pageindex.GetSafeInt() < 1)
                    pageindex = 1;
                datas = PayService.GetProjectPayHistorySummary(CurrentUser.UserCode, gcmc, sgdw, ffzh, pagesize, pageindex, out totalcount, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas, totalcount = totalcount });
        }
        /// <summary>
        /// 获取工资发放类型
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetFflx()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            try
            {
                datas = PayService.GetFflx(out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas });
        }
        /// <summary>
        /// 上传工资册
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoUploadPayList()
        {
            string msg = "";
            bool code = false;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string yhyhid = Request["key"].GetSafeString();
                
                if (Request.Files.Count == 0)
                    msg = "工资文件不能为空";
                else
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (!file.FileName.EndsWith(".xlsx"))
                        msg = "文件格式不对";
                    else
                    {
                        IList<IDictionary<string, string>>  tmpdatas = ExcelService.ParseExcel(file.InputStream, out msg);
                        tmpdatas = tmpdatas.Where(e => e.ElementAt(0).Key == "0").ToList();
                        tmpdatas.RemoveAt(0);
                        foreach(IDictionary<string,string> tmprow in tmpdatas)
                        {
                            IDictionary<string, string> row = new Dictionary<string, string>();
                            row.Add("姓名", tmprow.ContainsKey("0")?tmprow["0"]:"");
                            row.Add("电话", tmprow.ContainsKey("1") ? tmprow["1"] : "");
                            row.Add("身份证号码", tmprow.ContainsKey("2") ? tmprow["2"] : "");
                            row.Add("银行卡号", tmprow.ContainsKey("3") ? tmprow["3"] : "");
                            row.Add("实发工资", tmprow.ContainsKey("4") ? tmprow["4"] : "");
                            row.Add("备注", tmprow.ContainsKey("5") ? tmprow["5"] : "");
                            datas.Add(row);
                        }
                        code = msg == "";
                    }
                }
                    
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas });
        }
        /// <summary>
        /// 提交发工资申请
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoSavePayListApply(string yhyhid, string fflx, string ffny, string paylist)
        {
            string msg = "";
            bool code = false;
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                IList<IDictionary<string, string>> datas = jsonSerializer.Deserialize<IList<IDictionary<string, string>>>(paylist);

                code = PayService.SavePayListApply(datas, CurrentUser.UserCode, CurrentUser.RealName, yhyhid, fflx, ffny, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg});
        }
        /// <summary>
        /// 获取工资发放记录
        /// </summary>
        /// <param name="zt"></param>
        /// <param name="gcmc"></param>
        /// <param name="sgdw"></param>
        /// <param name="ffzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetPayHistory(string zt, string gcmc, string sgdw, string ffzh, string gcbh, string sgdwbh, string gzzq1, string gzzq2, string spsj1, string spsj2, int pagesize, int pageindex)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            int totalcount = 0;
            try
            {
                if (pagesize.GetSafeInt() == 0)
                    pagesize = 10;
                if (pageindex.GetSafeInt() < 1)
                    pageindex = 1;
                datas = PayService.GetPayHistory(CurrentUser.UserCode, zt, gcmc, sgdw, ffzh, gcbh, sgdwbh, gzzq1, gzzq2, spsj1, spsj2, pagesize, pageindex, out totalcount, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas, totalcount = totalcount });
        }
        /// <summary>
        /// 获取发放详情
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetPayDetail(string recid, string ffwc, string xm, int pagesize, int pageindex)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            int totalcount = 0;
            try
            {
                if (pagesize.GetSafeInt() == 0)
                    pagesize = 10;
                if (pageindex.GetSafeInt() < 1)
                    pageindex = 1;
                datas = PayService.GetPayDetail(recid, ffwc, xm, pagesize, pageindex, out totalcount, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas, totalcount = totalcount });
        }
        /// <summary>
        /// 设置工资发放状态
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoSetPayStatus(string recid, int status)
        {
            string msg = "";
            bool code = false;
            try
            {
                code = PayService.SetPayStatus(recid, status, CurrentUser.UserCode, CurrentUser.RealName, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 获取账号关联的工程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetRelateProjects()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            try
            {
                datas = PayService.GetRelateProjects(CurrentUser.UserCode, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas });
        }
        /// <summary>
        /// 获取账号关联的施工企业
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetRelateCompanys(string gcbh)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> datas = null;
            try
            {
                datas = PayService.GetRelateCompanys(CurrentUser.UserCode, gcbh, out msg);
                code = msg == "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = datas });
        }
        #endregion

    }
}