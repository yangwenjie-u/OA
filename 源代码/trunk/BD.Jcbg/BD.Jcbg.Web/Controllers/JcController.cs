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
using BD.IDataInputBll;
using System.Web;
using ReportPrint.Common;
using SysLog4 = BD.Jcbg.Common.SysLog4;
using Ionic.Zip;
using NHibernate;
using System.Data;
using BD.Jcbg.Service.Jc;
using BD.Jcbg.DataModal.VirutalEntity.Jc;
using Spring.Web.Support;
using Spring.Web.UI.Controls;
using BD.Jcbg.DataModal.Entities;
using TimeUtil = BD.Jcbg.Common.TimeUtil;


namespace BD.Jcbg.Web.Controllers
{
    public class JcController : Controller
    {

        #region 用户校验

        /// <summary>
        /// 用户校验并跳转链接
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/6/6 11:55
        public ActionResult CheckUser(string data)
        {
            string url = "";
            try
            {
                //用户名,密码,url
                string decodedata = BD.Jcbg.Common.CryptFun.Decode(data);
                string[] datas = decodedata.Split(',');
                if (datas.Length == 3)
                {
                    string error;
                    if (UserService.Login(datas[0], datas[1], out error))
                    {
                        return Redirect(datas[2]);
                    }
                }
                url = ClientInfo.GetCurDns(Request);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Redirect(url);

        }

        #endregion

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
        private IExcelPrintService _excelPrintService = null;
        private IExcelPrintService ExcelPrintService
        {
            get
            {
                if (_excelPrintService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _excelPrintService = webApplicationContext.GetObject("ExcelPrintService") as IExcelPrintService;
                }
                return _excelPrintService;
            }
        }
        private ISystemService _systemService = null;
        private ISystemService SystemService
        {
            get
            {
                try
                {
                    if (_systemService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _systemService;
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
        private static ISxtptService _sxtptService = null;
        private static ISxtptService SxtptService
        {
            get
            {
                if (_sxtptService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _sxtptService = webApplicationContext.GetObject("SxtptService") as ISxtptService;
                }
                return _sxtptService;
            }
        }

        private static IDataInputBll.IDataInputService _dataInputService = null;
        private static IDataInputBll.IDataInputService DataInputService
        {
            get
            {
                if (_dataInputService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataInputService = webApplicationContext.GetObject("DataInputService") as IDataInputBll.IDataInputService;
                }
                return _dataInputService;
            }
        }

        private static IApiSessionService _apiSessionService = null;
        private static IApiSessionService ApiSessionService
        {
            get
            {
                if (_apiSessionService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _apiSessionService = webApplicationContext.GetObject("ApiSessionService") as IApiSessionService;
                }
                return _apiSessionService;
            }
        }
        #endregion

        #region 页面
        /// <summary>
        /// 设置检测单位项目指标
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult JcdwXmzb()
        {
            ViewBag.dwbh = Request["dwbh"].GetSafeString();
            return View();
        }


        /// <summary>
        /// 地图标注
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Map()
        {
            ViewBag.pos = Request["pos"].GetSafeString();
            ViewBag.title = Request["title"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 委托时选择试验项目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Syxmxz()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gcmc = Request["gcmc"].GetSafeString();
            ViewBag.dwbh = Request["dwbh"].GetSafeString();
            ViewBag.jchtrecid = Request["jchtrecid"].GetSafeString();
            ViewBag.lsht = Request["lsht"].GetSafeInt(0);

            CurrentUser.Wtdytqy = ViewBag.dwbh;
            SystemService.SetUserSetting(CurrentUser.UserName, UserSettingItem.LastWtdw, ViewBag.dwbh);

            IList<IDictionary<string, string>> hts = CommonService.GetDataTable("select syxmbh from i_m_ht where (jcjgbh='" + ViewBag.dwbh + "' and fbjcjgbh ='' or fbjcjgbh='" + ViewBag.dwbh + "') and gcbh='" + ViewBag.gcbh + "' and sfyx=1");
            string limitxmbh = "";
            foreach (IDictionary<string, string> row in hts)
            {
                limitxmbh += row["syxmbh"] + ",";
            }
            if (!GlobalVariable.GetHtxmIsXm())
            {
                IList<IDictionary<string, string>> syxmbhs = CommonService.GetDataTable("select syxmbh from PR_M_SYXM a where a.SSDWBH='" + ViewBag.dwbh + "' and a.xsflbh1 in (" + limitxmbh.FormatSQLInStr() + ") and a.sfyx=1 and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh and b.yxqs<=getdate() and b.yxqz>=convert(datetime,'" + DateTime.Now.ToString("yyyy-MM-dd") + "'))");
                StringBuilder sb = new StringBuilder();
                foreach (IDictionary<string, string> rowData in syxmbhs)
                    sb.Append(rowData["syxmbh"] + ",");
                limitxmbh = sb.ToString();

            }
            limitxmbh = limitxmbh.Trim(new char[] { ',' });
            ViewBag.limitxmbh = limitxmbh;

            return View();
        }

        /// <summary>
        /// 监督员委托时选择试验项目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SyxmxzJdy()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gcmc = Request["gcmc"].GetSafeString();
            ViewBag.dwbh = Request["dwbh"].GetSafeString();

            CurrentUser.Wtdytqy = ViewBag.dwbh;
            SystemService.SetUserSetting(CurrentUser.UserName, UserSettingItem.LastWtdw, ViewBag.dwbh);


            return View();
        }

        /// <summary>
        /// 委托组数设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Wtzssz()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 实际委托组数查看
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Wtzsck()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 填单时选择委托单位
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Jcjgxz()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            return View();
        }
        /// <summary>
        /// 监督员填单时选择委托单位
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult JcjgxzJdy()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            return View();
        }

        /// <summary>
        /// 临时工程填单时选择委托单位
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult JcjgxzLsgc()
        {
            return View();
        }
        /// <summary>
        /// 单位项目设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Dwxmsz()
        {
            ViewBag.dwbh = Request["dwbh"].GetSafeString();
            return View();
        }


        public ActionResult ViewReport()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }

        public ActionResult ViewReportByBgwyh()
        {
            ViewBag.sdsc = Request["sdsc"].GetSafeString();
            ViewBag.bgwyh = Request["bgwyh"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult ViewXcjcDetail()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeString();
            //获取委托单的所有试验数据编号
            ResultParam ret = JcService.XcjcGetCjsybhs(wtdwyh);
            IDictionary<string, string> datas = (Dictionary<string, string>) ret.data;
            ViewBag.ids = datas["base"];
            ViewBag.others = datas["other"];
            return View();
        }

        [Authorize]
        public ActionResult ViewXcjcPic()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeString();
            //获取委托单的所有试验数据编号
            ResultParam ret = JcService.XcjcGetCjsybhs(wtdwyh);
            IDictionary<string, string> datas = (Dictionary<string, string>)ret.data;
            ViewBag.ids = datas["base"];
            ViewBag.others = datas["other"];
            return View();
        }

        /*[Authorize]
        public ActionResult ViewQx()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }*/

        [Authorize]
        public ActionResult ViewSysj()
        {
            ViewBag.sywyh = Request["sywyh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 查看一个委托单的所有数据
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdSysjs()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            ViewBag.wbc = Request["wbc"].GetSafeInt();
            ViewBag.cz = Request["cz"].GetSafeInt();
            ViewBag.cfsy = Request["cfsy"].GetSafeInt();
            ViewBag.viewpath = Configs.FileOssViewVideoPath;
            return View();
        }

        /// <summary>
        /// 查看某个委托单的所有报告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdReport()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 查看某个委托单实验数据跟报告数据比较结果
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdSysjbj()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 查看委托单异详情
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdYc()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            ViewBag.yczt = Request["yczt"].GetSafeInt();
            return View();
        }
        /// <summary>
        /// 查看委托单所有内容
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdAllDatas()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 检测项目选择页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Jcxmxz()
        {
            ViewBag.dwbh = Request["dwbh"].GetSafeString();
            ViewBag.fbxm = Request["fbxm"].GetSafeBool(false).ToString().ToLower();
            ViewBag.limitxmbh = Request["limitxmbh"].GetSafeString();
            ViewBag.view = Request["view"].GetSafeBool(false).ToString().ToLower();
            ViewBag.selectxmbh = Request["selectxmbh"].GetSafeString();
            ViewBag.global = Request["global"].GetSafeBool().ToString();
            return View();
        }
        /// <summary>
        /// 查看合同信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewHt()
        {
            ViewBag.recid = Request["recid"].GetSafeInt();
            return View();
        }
        /// <summary>
        /// 查看委托单视频
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdSp()
        {
            ViewBag.viewpath = Configs.FileOssViewVideoPath;
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            ViewBag.issc = Request["xc"].GetSafeInt();
            return View();
        }
        /// <summary>
        /// 查看委托单异常详情（包含委托单，试验数据，报告）
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdXq()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeRequest();
            ViewBag.yczt = Request["yczt"].GetSafeInt();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select syxmbh from m_by where recid='" + ViewBag.wtdwyh + "'");
                if (dt.Count > 0)
                {
                    ViewBag.syxmbh = dt[0]["syxmbh"];
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return View();
        }
        /// <summary>
        /// 检测资质项目设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult JczzSyxmSz()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }
        /// <summary>
        /// 通用查看图片也没 图片1||图片2
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewImages()
        {
            ViewBag.images = Request["images"].GetSafeRequest();
            return View();
        }
        /// <summary>
        /// 查看见证取样图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewJzqytp()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeRequest();
            return View();
        }

        /// <summary>
        /// 查看现场图片链
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctpl()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取图片链
            ResultParam ret = JcService.XcjcGetCjtpl(wtdwyh);
            ViewBag.wtdwyh = wtdwyh;
            ViewBag.tpls = ret.data;
            return View();
        }


        /// <summary>
        /// 查看现场检测视频
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjcsp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取视频组
            ResultParam ret = JcService.XcjcGetCjsp(wtdwyh);
            ViewBag.sps = ret.data;
            return View();
        }

        /// <summary>
        /// 查看现场检测视明细
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjcsps()
        {
            string id = Request["id"].GetSafeRequest();
            //string id = JcService.GetWtdXcjcSxt(wtdwyh);

            return new RedirectResult("/jc/playerysy?id=" + id);
        }

        /// <summary>
        /// 查看现场检测图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取图片数组
            ResultParam ret = JcService.XcjcGetCjtp(wtdwyh);
            ViewBag.tps = ret.data;
            return View();
        }

        /// <summary>
        /// 查看现场检测图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctps()
        {
            //委托单唯一号
            ViewBag.id = Request["id"].GetSafeRequest();
            //组号
            ViewBag.zh = Request["zh"].GetSafeRequest();
            return View();
        }

        /// <summary>
        /// 查看图片链
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdTpl()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeRequest();
            ViewBag.zh = Request["id"].GetSafeRequest();
            return View();
        }
        /// <summary>
        /// 现场检测地图展示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MapXcjc()
        {
            return View();
        }
        /// <summary>
        /// 现场检测视频地图展示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MapXcjcsp()
        {
            ViewBag.viewpath = Configs.FileOssViewVideoPath;
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            ViewBag.issc = Request["xc"].GetSafeInt();
            return View();
        }
        /// <summary>
        /// 手动上传报告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadReport()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            ViewBag.syxmbh = Request["syxmbh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 无见证二维码申请
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult NoQrcodeReq()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 委托单关联见证信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ContactWtdJz()
        {
            string msg = string.Empty;
            string wtdwyhs = string.Empty;

            JcService.GetContactWtd(out msg, out wtdwyhs);

            if (string.IsNullOrEmpty(wtdwyhs))
            {
                wtdwyhs = "暂无";
            }

            ViewBag.wtdwyhs = wtdwyhs;
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 配置个性化表时选择试验项目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SyxmIndividual()
        {
            ViewBag.dwbh = CommonService.GetSingleData("select top 1 qybh from i_m_qy where zh='" + CurrentUser.RealUserName + "'").GetSafeString();

            return View();
        }

        /// <summary>
        /// 工程坐标地图展示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MapGc()
        {
            ViewBag.Gcbh = Request["gcbh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 查看委托部位清单
        /// </summary>
        /// <returns></returns>
        public ActionResult SyxmGcbw()
        {
            var dwbh = Request["dwbh"].GetSafeString();
            var status = Request["status"].GetSafeString();

            if(status == "2")
                dwbh = CommonService.GetSingleData("select top 1 qybh from i_m_qy where zh='" + CurrentUser.RealUserName + "'").GetSafeString();

            ViewBag.dwbh = dwbh;
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gcmc = Request["gcmc"].GetSafeString();
            ViewBag.status = status;
            return View();
        }

        /// <summary>
        /// 非监督工程审核项目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult FjdGcAudit()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.zjzbh = Request["zjzbh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 委托单修改申请
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WtdModifyApply()
        {
            var dwbh = Request["dwbh"].GetSafeString();
            var sql = string.Format("select lxdh from i_m_qy where qybh = '{0}'", dwbh);
            ViewBag.phone = CommonService.GetSingleData(sql).GetSafeString();
            ViewBag.applyreason = Request["applyreason"].GetSafeString();
            return View();
        }

        #endregion

        #region 各种处理函数
        /// <summary>
        /// 初始化检测机构项目
        /// </summary>
        [Authorize]
        public void InitXm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["dwbh"].GetSafeString();
                string dwmc = "";
                string sql = "select qymc from i_m_qy where qybh='" + recid + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    dwmc = dt[0]["qymc"];
                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_SYXMXSFL where SSDWBH='" + recid + "' ");
                sqls.Add("delete from PR_M_SYXM where SSDWBH='" + recid + "'");
                sqls.Add("insert into PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[XSFLBH],[XSFLMC],[SFYX],[XSSX],[SJXSFLBH]) select '" + recid + "','" + dwmc + "',[XSFLBH],[XSFLMC],[SFYX],[XSSX],[SJXSFLBH] from PR_M_SYXMXSFL  where  SSDWBH=''");
                sqls.Add("insert into PR_M_SYXM([SSDWBH],[SSDWMC],[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFYX],[XSSX],[XSFLBH1],[SFDCBD],[WTDLRBJ],[YXFB],XCXM,WTDDYFS,XMLX,JGBGTXSJ,EWMGL,SCJZQYTP, SCBG) select '" + recid + "','" + dwmc + "',[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFYX],[XSSX],[XSFLBH1],[SFDCBD],[WTDLRBJ],[YXFB],XCXM,WTDDYFS,XMLX,JGBGTXSJ,EWMGL,SCJZQYTP,SCBG from PR_M_SYXM where SSDWBH=''");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 更新检测机构项目
        /// </summary>
        [Authorize]
        public void SyncXm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["dwbh"].GetSafeString();
                string dwmc = "";
                string sql = "select qymc from i_m_qy where qybh='" + recid + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    dwmc = dt[0]["qymc"];
                IList<string> sqls = new List<string>();
                sqls.Add("insert into PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) select '" + recid + "','" + dwmc + "',[XSFLBH],[XSFLMC],[SFYX],[XSSX] from PR_M_SYXMXSFL  where  SSDWBH='' and XSFLBH not in (select XSFLBH from PR_M_SYXMXSFL where SSDWBH='" + recid + "')");
                sqls.Add("insert into PR_M_SYXM([SSDWBH],[SSDWMC],[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFYX],[XSSX],[XSFLBH1],[SFDCBD],[WTDLRBJ],[YXFB],XCXM,WTDDYFS,XMLX,JGBGTXSJ,EWMGL,SCJZQYTP) select '" + recid + "','" + dwmc + "',[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],0,[XSSX],[XSFLBH1],[SFDCBD],[WTDLRBJ],[YXFB],XCXM,WTDDYFS,XMLX,JGBGTXSJ,EWMGL,SCJZQYTP from PR_M_SYXM where SSDWBH='' and SYXMBH not in (select SYXMBH from PR_M_SYXM where SSDWBH='" + recid + "')");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 设置项目资质
        /// </summary>
        [Authorize]
        public void SetJcdwxmzz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string dwbh = Request["dwbh"].GetSafeString();
                string zbbh = Request["zbbh"].GetSafeString();
                string dt1 = Request["dt1"].GetSafeString();
                string dt2 = Request["dt2"].GetSafeString();
                if (dt1 == "")
                {
                    code = false;
                    msg = "资质有效期无效";
                }
                else if (dt2 == "")
                {
                    code = false;
                    msg = "资质有效期无效";
                }
                else if (zbbh == "")
                {
                    code = false;
                    msg = "未选择指标";
                }
                else
                {
                    string[] arr = zbbh.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    IList<string> sqls = new List<string>();
                    //sqls.Add("delete from pr_m_qyzb where qybh='" + dwbh + "'");
                    if (arr != null)
                    {
                        foreach (string str in arr)
                        {
                            sqls.Add("insert into pr_m_qyzb(qybh,zbbh,yxqs,yxqz) values('" + dwbh + "','" + str + "',convert(datetime,'" + dt1 + "'), convert(datetime,'" + dt2 + "'))");
                        }
                    }
                    code = CommonService.ExecTrans(sqls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 单位项目禁用启用
        /// </summary>
        [Authorize]
        public void SetJcdwxm()
        {
            bool code = true;
            string msg = "";
            try
            {
                IDictionary<int, int> dicIdValue = new Dictionary<int, int>();
                string ids = Request["ids"].GetSafeString();
                string[] arr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    string[] itm = str.Split(new char[] { ':' });
                    if (itm.Length < 2)
                        continue;
                    dicIdValue.Add(itm[0].GetSafeInt(), itm[1].GetSafeInt());
                }
                code = JcService.EnableJcdwXm(dicIdValue, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 添加检测机构到所有质监站中
        /// </summary>
        [Authorize]
        public void SetJcjgToZjz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string dwbh = Request["dwbh"].GetSafeString();

                if(string.IsNullOrEmpty(dwbh))
                {
                    code = false;
                    msg = "单位编号不能为空";
                    Response.Write(JsonFormat.GetRetString(code, msg));
                }

                string sql = string.Format(@"select a.zjzbh 
                                               from h_zjz a 
                                              where not exists 
                                            (select 1 from i_s_zjz_jczx b where a.ZJZBH = b.ZJZBH and b.JCJGBH = '{0}')", dwbh);

                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                var startDate = DateTime.Parse("2019-03-01");
                var endDate = DateTime.Parse("2026-03-31");

                //初始化数据库信息
                ISession session = null;
                ITransaction transaction = null;

                try
                {
                    if (zjzbhs.Count > 0)
                    {
                        session = DataInputService.GetDBSession();
                        IDbCommand cmd = session.Connection.CreateCommand();
                        transaction = session.BeginTransaction();
                        transaction.Enlist(cmd);

                        foreach (var zjzbh in zjzbhs)
                        {
                            var recid = DataInputService.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_ZJZ_JCZX__RECID'|maxbhfield-ZDBH", "I_S_ZJZ_JCZX", "RECID", null, cmd);

                            if (recid == "")
                            {
                                transaction.Rollback();
                                code = false;
                                msg = "获取编号失败！";
                            }
                            else
                            {
                                var insertSql = string.Format(@"insert into I_S_ZJZ_JCZX
                                        ([RECID],[JCJGBH],[ZJZBH],[YXQS],[YXQZ],[SFYX]) 
                                         values ('{0}', '{1}', '{2}', '{3}', '{4}', 1)", recid, dwbh, zjzbh["zjzbh"], startDate, endDate);
                                DataInputService.ExecSql(insertSql, cmd);
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    code = false;
                    msg = ex.Message;
                    SysLog4.WriteLog(ex);
                }
                finally
                {
                    if (session != null)
                    {
                        session.Close();
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 获取工程坐标
        /// </summary>
        [Authorize]
        public void GetGcbz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();

                string sql = "select jdzch,lon,lat from I_M_GC_JWD where jdzch='" + jdzch + "'";
                IList<IDictionary<string, string>> sqllist = CommonService.GetDataTable(sql);
                IList<string> sqls = new List<string>();
                if (sqllist.Count != 0)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    msg = jss.Serialize(sqllist);

                }
                else
                {
                    code = false;
                    msg = "没有设置坐标";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 工程标注
        /// </summary>
        [Authorize]
        public void SetGcbz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                if (jdzch == "")
                    jdzch = gcbh;
                string pos = Request["pos"].GetSafeString();
                string lon = "";
                string lat = "";
                if (pos != "")
                {
                    string[] list = pos.Split(',');
                    if (list.Count() == 2)
                    {
                        lon = list[0];
                        lat = list[1];
                    }
                    else
                    {
                        code = false;
                        msg = "设置经纬度失败";
                    }
                }
                if (msg == "")
                {
                    string sql = "select * from I_M_GC_JWD where jdzch='" + jdzch + "'";
                    IList<IDictionary<string, string>> sqllist = CommonService.GetDataTable(sql);
                    IList<string> sqls = new List<string>();
                    sqls.Add("update i_m_gc set gczb='" + pos + "' where gcbh='" + jdzch + "'");
                    if (sqllist.Count != 0)
                    {
                        sqls.Add("update i_m_gc_jwd set lon='" + lon + "',lat='" + lat + "' where jdzch='" + jdzch + "'");

                    }
                    else
                    {
                        sqls.Add("insert into i_m_gc_jwd (jdzch,lon,lat,orderby) values('"
                            + jdzch + "','"
                            + lon + "','"
                            + lat + "',1)");
                    }
                    code = CommonService.ExecTrans(sqls);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 工程委托组数
        /// </summary>
        [Authorize]
        public void SetGcWtzs()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string wtzs = Request["wtzs"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_S_GC_WTZS where gcbh='" + gcbh + "'");
                string[] arr = wtzs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    if (str.Length == 0)
                        continue;
                    string[] arr1 = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr1.Length < 2)
                        continue;
                    int nwtzs = arr1[1].GetSafeInt();
                    if (nwtzs <= 0)
                        continue;
                    sqls.Add("insert into I_S_GC_WTZS(GCBH,YQWTZS,SYXMBH) values('" + gcbh + "'," + nwtzs.ToString() + ", '" + arr1[0] + "')");
                }
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 设置资质有效期
        /// </summary>
        [Authorize]
        public void SetZzyxq()
        {
            bool code = false;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                string datetype = Request["datetype"].GetSafeString();
                DateTime datevalue = Request["datevalue"].GetSafeDate(DateTime.MinValue);

                code = JcService.SetZzyxq(recid, datetype, datevalue, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 设置委托单为提交状态
        /// </summary>
        [Authorize]
        public void SetWtdSubmit()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();

                // 组合项目添加其他记录
                //JcService.CopyCombinationInfos(recid, out msg);

                code = JcService.SetWtdStatusTj(recid, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 设置委托单为打印
        /// </summary>
        [Authorize]
        public void SetWtdPrint()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recids = Request["recid"].GetSafeString();

                code = JcService.SetWtdStatusDy(recids, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 设置session的单位编号
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void SetDwbh()
        {
            bool code = true;
            string msg = "";
            try
            {
                string dwbh = Request["dwbh"].GetSafeString();

                CurrentUser.Wtdytqy = dwbh;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }
        /// <summary>
        /// 调用该接口，重新设置所有委托单数据和异常状态
        /// </summary>
        public void SetAllWtdzt()
        {
            bool code = true;
            string msg = "";
            string recid = "";
            try
            {

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select recid from m_by  where recid>='00001974' order by recid");

                foreach (IDictionary<string, string> row in dt)
                {
                    recid = row["recid"];
                    code = JcService.SetWtdyczt(recid, out msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg + "/" + recid));
            }

        }
        /// <summary>
        /// 把所有非基础项目的委托单录入布局设置成基础项目的委托单录入布局
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult BatchUpdateWtdlrbj()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sql = "update pr_m_syxm set wtdlrbj=(select wtdlrbj from pr_m_syxm b where b.syxmbh=pr_m_syxm.syxmbh and b.ssdwbh='') where ssdwbh<>''";
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
                CommonService.ExecTrans(sqls);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 保存检测资质-试验项目对应关系
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult SaveJczzSyxm(string id, string values)
        {
            string msg = "";
            bool code = true;
            try
            {
                IList<string> sqls = new List<string>();
                string sql = "delete from pr_s_zzsyxm where parentid='" + id + "'";
                sqls.Add(sql);
                if (values != "")
                {
                    string[] arr = values.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arr)
                    {
                        string[] arrItem = str.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrItem.Length < 2)
                            continue;
                        string zznrbh = arrItem[0];
                        if (arrItem[1].Length == 0)
                            continue;
                        string[] arrIds = arrItem[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strId in arrIds)
                            sqls.Add("insert into pr_s_zzsyxm(RECID,ParentId,ZZNRBH,SYXMBH) values('" + Guid.NewGuid().ToString() + "','" + id + "','" + zznrbh + "','" + strId + "')");

                    }
                }
                CommonService.ExecTrans(sqls);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 设置设备标定信息，把以前表单信息设置为无效，设置设备的备注信息和下次标定时间
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult SetBdxx(string recid)
        {
            string msg = "";
            bool code = true;
            try
            {
                IList<string> sqls = new List<string>();
                string sql = "update i_s_sb_bd set sfyx=0 where recid<>'" + recid + "' and sbid in (select sbid from i_s_sb_bd where recid='" + recid + "')";
                sqls.Add(sql);
                sql = "update i_m_sb set xcbdrq=dateadd(day,bdzq,(select bdrq from i_s_sb_bd where recid='" + recid + "')),bz=bz+char(10)+char(13)+(select bz from i_s_sb_bd where recid='" + recid + "') where recid=(select sbid from i_s_sb_bd where recid='" + recid + "')";

                sqls.Add(sql);
                CommonService.ExecTrans(sqls);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取检测单位的项目指标
        /// </summary>
        [Authorize]
        public void GetJcdwXmzb()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            string dwbh = Request["dwbh"].GetSafeString();
            string xsflbh = Request["xsflbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> xmfls = CommonService.GetDataTable("select xsflbh,xsflmc from pr_m_syxmxsfl where ssdwbh='" + dwbh + "' and xsflbh='" + xsflbh + "'  order by xssx");
                IList<IDictionary<string, string>> xms = CommonService.GetDataTable("select xsflbh,syxmbh,syxmmc from pr_m_syxm where ssdwbh='" + dwbh + "' and xsflbh in (select xsflbh from pr_m_syxmxsfl where ssdwbh='" + dwbh + "' and xsflbh='" + xsflbh + "') order by xsflbh,xssx");
                IList<IDictionary<string, string>> zbs = CommonService.GetDataTable("select syxmbh,recid,zbmc from pr_m_zb where syxmbh in (select syxmbh from pr_m_syxm where ssdwbh='" + dwbh + "' and xsflbh in (select xsflbh from pr_m_syxmxsfl where ssdwbh='" + dwbh + "' and xsflbh='" + xsflbh + "') ) order by syxmbh,xssx");
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select zbbh from pr_m_qyzb where qybh='" + dwbh + "'");

                ret.Append("[");

                foreach (IDictionary<string, string> xmfl in xmfls)
                {
                    if (ret.Length > 1)
                        ret.Append(",");
                    ret.Append(new VCheckItem()
                    {
                        id = "F_" + xmfl["xsflbh"],
                        name = xmfl["xsflmc"],
                        chkDisabled = false,
                        ischeckecd = false,
                        open = false,
                        pId = ""
                    }.GetJsonStr());

                }
                foreach (IDictionary<string, string> xm in xms)
                {
                    if (ret.Length > 1)
                        ret.Append(",");
                    ret.Append(new VCheckItem()
                    {
                        id = "X_" + xm["syxmbh"],
                        name = xm["syxmmc"],
                        chkDisabled = false,
                        ischeckecd = false,
                        open = false,
                        pId = "F_" + xm["xsflbh"]
                    }.GetJsonStr());
                }
                foreach (IDictionary<string, string> zb in zbs)
                {
                    if (ret.Length > 1)
                        ret.Append(",");
                    bool ischecked = false;
                    var q = from e in qyzbs where e["zbbh"].GetSafeString().Equals(zb["recid"]) select e;
                    ischecked = q.Count() > 0;
                    ret.Append(new VCheckItem()
                    {
                        id = "Z_" + zb["recid"],
                        name = zb["zbmc"],
                        chkDisabled = false,
                        ischeckecd = ischecked,
                        open = false,
                        pId = "X_" + zb["syxmbh"]
                    }.GetJsonStr());
                }
                ret.Append("]");
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write("{\"code\":\"" + (code ? "0" : "1") + "\", \"msg\":\"" + msg + "\", \"data\":" + ret.ToString() + "}");
            }
        }
        /// <summary>
        /// 获取检测单位项目分组
        /// </summary>
        //[Authorize]
        public void GetJcdwXmfz()
        {
            string ret = "[]";
            string dwbh = Request["dwbh"].GetSafeString();
            bool global = Request["global"].GetSafeBool();
            //if (dwbh == "" && !global)
            //    dwbh = CurrentUser.Wtdytqy;
            try
            {
                if (global || dwbh != "")
                {
                    IList<IDictionary<string, string>> xmfls = CommonService.GetDataTable("select sjxsflbh,xsflbh,xsflmc from pr_m_syxmxsfl where ssdwbh='" + dwbh + "' order by xssx");
                    if (xmfls.Count > 0)
                        ret = new JavaScriptSerializer().Serialize(xmfls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取检测单位的项目
        /// </summary>
        //  [Authorize]
        public void GetJcdwXm()
        {
            string ret = "[]";
            string dwbh = Request["dwbh"].GetSafeString();
            int yx = Request["yx"].GetSafeInt(1);
            int yzb = Request["yzb"].GetSafeInt(1);
            bool global = Request["global"].GetSafeBool();
            bool fbxm = Request["fbxm"].GetSafeBool(false);
            string limitxmbh = Request["limitxmbh"].GetSafeString();

            //if (dwbh == "" && !global)
            //    dwbh = CurrentUser.Wtdytqy; 
            try
            {
                if (global || dwbh != "")
                {
                    string where = " and a.xmlx<>'3' ";
                    if (yx == 1)
                        where += " and a.sfyx=1 ";
                    if (yzb == 1)
                        where += " and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh ) ";
                    if (fbxm)
                        where += " and a.yxfb=1 ";
                    if (limitxmbh != "" && !limitxmbh.Equals("all", StringComparison.OrdinalIgnoreCase))
                        where += " and a.syxmbh in (" + limitxmbh.FormatSQLInStr() + ") ";
                    string sql = "select a.xsflbh,a.syxmbh,a.syxmmc,a.sfyx,a.recid,a.wtdlrbj,a.xmlx,(select top 1 b.xmdh from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdh,(select count(*) from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdhsl from pr_m_syxm a where a.ssdwbh='" + dwbh + "' " + where + "  order by a.xsflbh,a.xssx";

                    IList<IDictionary<string, string>> xms = CommonService.GetDataTable(sql);
                    foreach (IDictionary<string, string> row in xms)
                    {
                        int xmdhsl = row["xmdhsl"].GetSafeInt();
                        if (xmdhsl == 0)
                            row["xmdh"] = row["syxmbh"];
                        row.Remove("xmdhsl");
                    }
                    if (xms.Count > 0)
                        ret = new JavaScriptSerializer().Serialize(xms);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取分包合同编号
        /// </summary>
        /// <param name="htbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetFbhtbh(string htbh)
        {
            bool code = false;
            string msg = "";
            try
            {
                string sql = "select top 1 fbhtbh from i_m_ht where htbh='" + htbh + "' and fbhtbh<>'' order by fbhtbh desc";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    msg = htbh + "-01";
                }
                else
                {
                    msg = htbh + "-" + (dt[0]["fbhtbh"].Split(new char[] { '-' })[1].GetSafeInt() + 1).ToString("00");
                }
                code = true;

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 获取内部合同编号
        /// </summary>
        /// <param name="htbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetNbhtbh(string htbh)
        {
            bool code = false;
            string msg = "";
            try
            {
                string sql = "select top 1 jchtbh from i_m_jcht where htbh='" + htbh + "' order by jchtbh desc";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    msg = htbh + "-01";
                }
                else
                {
                    msg = htbh + "-" + (dt[0]["jchtbh"].Split(new char[] { '-' })[1].GetSafeInt() + 1).ToString("00");
                }
                code = true;

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 获取指标
        /// </summary>
        //[Authorize]
        public void GetZb()
        {
            string ret = "[]";
            try
            {
                IList<IDictionary<string, string>> zbs = CommonService.GetDataTable("select syxmbh,recid,zbmc from pr_m_zb order by syxmbh,xssx");

                if (zbs.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(zbs);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取单位指标
        /// </summary>
        //[Authorize]
        public void GetValidDwzb()
        {
            string ret = "[]";
            string dwbh = Request["dwbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select recid,qybh,zbbh,yxqs,yxqz from pr_m_qyzb where qybh='" + dwbh + "' and YXQS<=getdate() and dateadd(d,1,YXQZ)>getdate() order by YXQZ desc");
                foreach (IDictionary<string, string> row in qyzbs)
                {
                    row["yxqs"] = row["yxqs"].GetSafeDate().ToString("yyyy-MM-dd");
                    row["yxqz"] = row["yxqz"].GetSafeDate().ToString("yyyy-MM-dd");
                }

                if (qyzbs.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(qyzbs);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取某个工程的要求委托组数
        /// </summary>
        [Authorize]
        public void GetGcWtzs()
        {
            string ret = "[]";
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select SYXMBH,YQWTZS from I_S_GC_WTZS where GCBH='" + gcbh + "' and yqwtzs>0");

                if (qyzbs.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(qyzbs);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取某个工程的实际委托组数
        /// </summary>
        [Authorize]
        public void GetGcSjyxwtzs()
        {
            string ret = "[]";
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select syxmbh, count(*) as wtsl from m_by where GCBH='" + gcbh + "' and dbo.IsValidWtd(ZT)=1 group by syxmbh");

                if (qyzbs.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(qyzbs);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取某个工程实际委托组数
        /// </summary>
        [Authorize]
        public void GetGcdwSjwtzs()
        {
            string ret = "[]";
            string gcbh = Request["gcbh"].GetSafeString();
            string dwbh = Request["dwbh"].GetSafeString();
            string where = " and gcbh='" + gcbh + "' ";
            try
            {
                // 当前人是送样人
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select syxmbh,count(*) as wtzs from m_by where 1=1 " + where + " and (ytdwbh='" + dwbh + "') group by syxmbh");

                if (qyzbs.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(qyzbs);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取工程信息
        /// </summary>
        [Authorize]
        public void GetGcInfo()
        {
            string ret = "{}";
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> gcinfos = CommonService.GetDataTable("select * from i_m_gc where GCBH='" + gcbh + "'");

                if (gcinfos.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(gcinfos[0]);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取委托单录入时工程信息
        /// </summary>
        [Authorize]
        public void GetWtdGcInfo()
        {
            string ret = "{}";
            string gcbh = Request["gcbh"].GetSafeString();
            string dwbh = Request["dwbh"].GetSafeString();
            string jchtrecid = Request["jchtrecid"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();
                if (GlobalVariable.UseNbht())
                {
                    if (jchtrecid == "")
                        gcinfos = CommonService.GetDataTable("select top 1 * from View_I_M_JCHT_WTD where jcjgbh = '" + dwbh + "' and (GCBH='" + gcbh + "' or gcbh in (select gcbh from i_m_gc where sjgcbh='" + gcbh + "')) and syrbh in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserCode + "') order by lrsj desc");
                    else
                        gcinfos = CommonService.GetDataTable("select top 1 * from View_I_M_JCHT_WTD where recid='" + jchtrecid + "'");
                }
                else
                    gcinfos = CommonService.GetDataTable("select * from View_I_M_GC_WTD where GCBH='" + gcbh + "'");

                if (gcinfos.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(gcinfos[0]);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 监督员获取委托单录入时工程信息
        /// </summary>
        [Authorize]
        public void GetWtdGcInfoJdy()
        {
            string ret = "{}";
            string gcbh = Request["gcbh"].GetSafeString();
            string dwbh = Request["dwbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                gcinfos = CommonService.GetDataTable("select * from View_I_M_GC_WTD where SJGCBH='" + gcbh + "' and SSJCJGBH='" + dwbh + "'");

                if (gcinfos.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(gcinfos[0]);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 监督员获取委托单录入时合同信息
        /// </summary>
        [Authorize]
        public void GetWtdHtInfoJdy()
        {
            string ret = "{}";
            string jcjgbh = Request["dwbh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                gcinfos = CommonService.GetDataTable("select top 1 * from i_m_jcht where jcjgbh='" + jcjgbh + "' and  khdwbh in (select zjzbh from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')) order by lrsj desc");

                if (gcinfos.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(gcinfos[0]);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取检测机构
        /// </summary>
        [Authorize]
        public void GetJcjg()
        {
            string ret = "[]";
            try
            {
                string filtergc = Request["filtergc"].GetSafeRequest();
                bool jdywt = Request["jdywt"].GetSafeInt() == 1;

                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                if (jdywt)
                {
                    gcinfos = CommonService.GetDataTable("select distinct jcjgbh as qybh,jcjgmc as qymc from i_m_jcht where khdwbh=(select zjzbh from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'))");
                }
                else
                {

                    if (!GlobalVariable.UseHtjg() && !GlobalVariable.UseNbht())
                        gcinfos = CommonService.GetDataTable("select qybh,qymc from i_m_qy where lxbh='01' or exists (select * from I_S_QY_QYZZ where qylxbh='01' and I_S_QY_QYZZ.qybh=i_m_qy.qybh)");
                    else if (GlobalVariable.UseNbht())
                    {
                        IList<IDictionary<string, string>> qyrys = CommonService.GetDataTable("select count(*) as c1 from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                        string appendWhere = "";
                        if (qyrys[0]["c1"].GetSafeInt() > 0)
                            appendWhere += " and syrbh=(select rybh from i_m_ry where zh='" + CurrentUser.RealUserName + "')";
                        gcinfos = CommonService.GetDataTable("select distinct jcjgbh as qybh,jcjgmc as qymc from i_m_jcht where sfyx=1 and (gcbh='" + filtergc + "' or gcbh in (select gcbh from i_m_gc where sjgcbh='" + filtergc + "')) " + appendWhere);

                    }
                    else
                    {
                        gcinfos = CommonService.GetDataTable("select qybh,qymc from i_m_qy where lxbh='01' or exists (select * from I_S_QY_QYZZ where qylxbh='01' and I_S_QY_QYZZ.qybh=i_m_qy.qybh)");

                        if (filtergc != "")
                        {
                            IList<IDictionary<string, string>> hts = CommonService.GetDataTable("select jcjgbh,fbjcjgbh from i_m_ht where sfyx=1 and gcbh='" + filtergc + "'");
                            if (hts.Count > 0)
                            {
                                IList<string> allqybhs = new List<string>();
                                foreach (IDictionary<string, string> row in hts)
                                {
                                    allqybhs.Add(row["jcjgbh"]);
                                    allqybhs.Add(row["fbjcjgbh"]);
                                }
                                for (int i = gcinfos.Count - 1; i >= 0; i--)
                                {
                                    string qybh = gcinfos[i]["qybh"];
                                    var q = from e in allqybhs where e == qybh select e;
                                    if (q.Count() == 0)
                                        gcinfos.RemoveAt(i);
                                }
                            }
                        }
                    }
                }

                ret = new JavaScriptSerializer().Serialize(gcinfos);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 监督员填单时获取检测机构
        /// </summary>
        [Authorize]
        public void GetJcjgJdy()
        {
            string ret = "[]";
            try
            {
                string filtergc = Request["filtergc"].GetSafeRequest();

                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                gcinfos = CommonService.GetDataTable("select distinct jcjgbh as qybh,jcjgmc as qymc from i_m_jcht where khdwbh=(select zjzbh from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'))");


                ret = new JavaScriptSerializer().Serialize(gcinfos);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }

        /// <summary>
        /// 临时工程填单时获取检测机构
        /// </summary>
        [Authorize]
        public void GetJcjgLsgc()
        {
            string ret = "[]";
            try
            {
                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                gcinfos = CommonService.GetDataTable("select jcjgbh as qybh,jcjgmc as qymc,max(recid) as htbh from i_m_jcht where sfyx=1 and htlx='LSHT' and glrybh=(select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "') group by jcjgbh,jcjgmc");

                ret = new JavaScriptSerializer().Serialize(gcinfos);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取企业信息
        /// </summary>
        [Authorize]
        public void GetQyInfo()
        {
            string ret = "{}";
            string qybh = Request["qybh"].GetSafeString();
            try
            {
                IList<IDictionary<string, string>> gcinfos = CommonService.GetDataTable("select * from i_m_qy where qybh='" + qybh + "'");

                if (gcinfos.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(gcinfos[0]);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }


        }
        /// <summary>
        /// 根据帮助表类型获取帮助表
        /// </summary>
        [Authorize]
        public void GetHelpTables()
        {
            string ret = "[]";
            try
            {
                string blx = Request["blx"].GetSafeString();
                if (blx != "")
                {
                    IList<IDictionary<string, string>> helps = CommonService.GetDataTable("select * from pr_m_sjbsm where blx='" + blx + "'");
                    ret = new JavaScriptSerializer().Serialize(helps);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取报告摘要：唯一号，顺序号
        /// </summary>

        public void GetReportAbs()
        {
            bool code = false;
            string msg = "";
            IList<IDictionary<string, string>> bgabs = new List<IDictionary<string, string>>();
            try
            {
                string wtdwyh = Request["wtdwyh"].GetSafeString();

                if (wtdwyh != "")
                    code = JcService.GetReportAbs(wtdwyh, out bgabs, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code ? "0" : "1");
                row.Add("msg", msg);
                row.Add("bgabs", bgabs);
                Response.Write(jss.Serialize(row));
            }
        }
        public void GetReportAbsByBgwyh()
        {
            bool code = false;
            string msg = "";
            string bgabs = "";
            try
            {
                string sdsc = Request["sdsc"].GetSafeString();
                string bgwyh = Request["bgwyh"].GetSafeString();

                if (bgwyh != "")
                    code = JcService.GetReportAbsByBgwyh(bgwyh, sdsc, out bgabs, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code ? "0" : "1");
                row.Add("msg", msg);
                row.Add("bgabs", bgabs);
                Response.Write(jss.Serialize(row));
            }
        }
        
        /// <summary>
        /// 根据委托单唯一号获聂报告文件
        /// </summary>
        [Authorize]
        public void GetReportFileByWtdwyh()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string wtdwyh = Request["wtdwyh"].GetSafeString();
                if (wtdwyh == "")
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "application/json";
                    Response.Write(String.Format("唯一号：{0}不存在！", wtdwyh));
                    return;
                }
                //根据委托单
                string bgwyh = "";
                string sxh = "";
                code = JcService.GetReportAbs(wtdwyh,out bgwyh,out sxh,out msg);
                if (!code)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "application/json";
                    Response.Write(String.Format("唯一号：{0}获取报告信息失败，原因：{1}", wtdwyh, msg));
                    return;
                }

                //获取下载文件内容
                if (bgwyh != "")
                {
                    code = JcService.GetReportFile(bgwyh, sxh, out file, out msg);
                    if (code && file != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + String.Format("{0}_{1}.pdf", bgwyh, sxh));
                        Response.BinaryWrite(file);
                    }
                }
                else
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "application/json";
                    Response.Write(String.Format("唯一号：{0}所对应的报告文件信息不存在！", wtdwyh));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }

        /// <summary>
        /// 获取某页报告文件
        /// </summary>

        public void GetReportFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string key = RouteData.Values["id"].GetSafeString();
                string[] arr = key.Split(new char[] { '_' });
                string bgwyh = arr[0];
                string sxh = arr[1];
                if (bgwyh != "")
                {
                    code = JcService.GetReportFile(bgwyh, sxh, out file, out msg);
                    if (code && file != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "application/pdf";
                        Response.BinaryWrite(file);
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 获取一个试验的数据详情
        /// </summary>
        [Authorize]
        public void GetSysjxqList()
        {
            string ret = "[]";
            string sywyh = Request["sywyh"].GetSafeString();
            try
            {
                string msg = "";
                IList<IDictionary<string, string>> dt = JcService.GetSysjxq(sywyh, out msg);

                if (dt.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(dt);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取试验曲线图片
        /// </summary>
        [Authorize]
        public void GetSysjqx()
        {
            byte[] file = null;
            string sywyh = Request["sywyh"].GetSafeString();
            try
            {

                string msg = "";
                file = JcService.GetSysjqx(sywyh, out msg);
                if (file != null)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "image/jpeg";
                    Response.BinaryWrite(file);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 获取某个委托单的所有试验数据和详情
        /// </summary>
        [Authorize]
        public JsonResult GetWtdSysjList(string wtdwyh, string wbc, string cz, string cfsy)
        {
            IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
            try
            {
                string msg = "";
                dt1 = JcService.GetWtdSysjs(wtdwyh, out msg);
                dt2 = JcService.GetWtdSysjxqs(wtdwyh, out msg);
                // 仅显示未保存的
                if (wbc.GetSafeInt() == 1)
                {
                    var q = from e in dt1 where e["sfbc"].Equals("false", StringComparison.OrdinalIgnoreCase) select e;
                    dt1 = q.ToList();

                }
                // 仅显示重做的
                else if (cz.GetSafeInt() == 1)
                {
                    var q = from e in dt1 where e["sfcz"].Equals("true", StringComparison.OrdinalIgnoreCase) select e;
                    dt1 = q.ToList();
                }
                // 仅显示重复试验的
                else if (cfsy.GetSafeInt() == 1)
                {
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    dt1 = dt1.Where(p => p["sfbc"].Equals("true", StringComparison.OrdinalIgnoreCase)).ToList();
                    dt1 = dt1.Where(p => p["sfcz"].Equals("false", StringComparison.OrdinalIgnoreCase)).ToList();
                    for (int i = 0; i < dt1.Count; i++)
                    {
                        string symc = dt1[i]["symc"];
                        string ypbh = dt1[i]["ypbh"];
                        string zh = dt1[i]["zh"];
                        bool find = false;
                        for (int j = 0; j < dt1.Count; j++)
                        {
                            if (i == j)
                                continue;
                            if (symc == dt1[j]["symc"] && ypbh == dt1[j]["ypbh"] && zh == dt1[j]["zh"])
                            {
                                find = true;
                                break;
                            }
                        }
                        if (find)
                            dt.Add(dt1[i]);

                    }
                    dt1 = dt;
                }


                foreach (IDictionary<string, string> row in dt1)
                {
                    row["sykssj"] = row["sykssj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    row["syjssj"] = row["syjssj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    row["scsj"] = row["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    TimeSpan ts = row["scsj"].GetSafeDate().Subtract(row["syjssj"].GetSafeDate());
                    int d = ts.Days;
                    int h = ts.Hours;
                    int m = ts.Minutes;
                    string str = "";
                    if (d > 0)
                        str += d + "天";
                    if (h > 0)
                        str += h + "小时";
                    if (m > 0)
                        str += m + "分钟";
                    row.Add("scsc", str);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(new { sjs = dt1, xqs = dt2 });
        }
        /// <summary>
        /// 获取委托单所有报告唯一号和对应的顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetWtdReports(string wtdwyh)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            string msg = "";
            try
            {

                ret = JcService.GetWtdReports(wtdwyh, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(new { code = (msg == "" ? "0" : "1"), msg = msg, reports = ret });
        }
        /// <summary>
        /// 获取委托单录入布局和前缀编号
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="syxmbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetWtdlrbj(string qybh, string syxmbh)
        {
            bool code = true;
            string msg = "";
            string xmdh = "";
            try
            {
                string sql = string.Empty;
                sql = "select wtdlrbj from pr_m_syxm where ssdwbh='" + qybh + "' and syxmbh='" + syxmbh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    msg = dt[0]["wtdlrbj"];

                sql = string.Format("select top 1 xmdh from PR_M_WTDWJH where ssdwbh = '{0}' and syxmbh = '{1}'", qybh, syxmbh);
                xmdh = CommonService.GetSingleData(sql).GetSafeString();

                if (string.IsNullOrEmpty(xmdh))
                {
                    xmdh = syxmbh;
                }
            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, xmdh });
        }
        /// <summary>
        /// 获取某个委托单报告和实验数据比较结果，报告取最后一份，试验数据取最后
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns>[{'zh':''}]</returns>

        //[Authorize]
        public JsonResult GetWtdSysjbj(string wtdwyh)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                // 报告唯一号
                string sql = "select top 1 bgwyh from up_bgsj where wtdbh='" + wtdwyh + "'order by scsj desc";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "报告数据还未上传";
                }
                else
                {
                    string bgwyh = dt[0]["bgwyh"];
                    // 报告从表数据
                    sql = "select * from up_bgxqs where bgwyh='" + bgwyh + "'";
                    IList<IDictionary<string, string>> dtBgs = CommonService.GetDataTable(sql);
                    // 所有试验主表
                    sql = "select sywyh,symc,zh from up_sysj where wtdbh='" + wtdwyh + "' and sfbc=1 order by scsj asc";
                    dt = CommonService.GetDataTable(sql);
                    // 获取不同项目，组的最后一个试验
                    IList<IDictionary<string, string>> sywyhs = new List<IDictionary<string, string>>();
                    string strSywyhs = "";
                    IList<string> zhs = new List<string>();
                    for (int i = 0; i < dt.Count; i++)
                    {
                        IDictionary<string, string> currow = dt[i];
                        bool isvalid = true;
                        for (int j = i + 1; j < dt.Count; j++)
                        {
                            IDictionary<string, string> nextrow = dt[j];
                            if (currow["symc"] == nextrow["symc"] && currow["zh"] == nextrow["zh"])
                            {
                                isvalid = false;
                                break;
                            }
                        }
                        if (isvalid)
                        {
                            sywyhs.Add(currow);
                            strSywyhs += currow["sywyh"] + ",";
                            string zh = currow["zh"];
                            var q = from e in zhs where e == zh select e;
                            if (q.Count() == 0)
                                zhs.Add(zh);
                        }
                    }
                    // 获取试验详细数据
                    sql = "select * from up_syxq where sywyh in (" + strSywyhs.FormatSQLInStr() + ") order by zdhy";
                    IList<IDictionary<string, string>> dtSysjs = CommonService.GetDataTable(sql);
                    // 获取采集记录所有字段含义
                    IList<MHeader> heads = new List<MHeader>();
                    MHeader head;
                    foreach (IDictionary<string, string> row1 in dtSysjs)
                    {
                        head = new MHeader();
                        head.zdhy = row1["zdhy"];
                        head.zdsy = row1["zdsy"];
                        //为兼容旧采集软件
                        if (head.zdsy.GetSafeString() == "")
                            head.zdsy = head.zdhy;
                        var q = from e in heads where e.zdhy == head.zdhy select e;
                        if (q.Count() == 0)
                            heads.Add(head);
                    }
                    // 添加到记录集
                    var qz = from e in zhs orderby e.GetSafeInt() select e;
                    foreach (var zh in qz)
                    {
                        IDictionary<string, string> row1 = new Dictionary<string, string>();
                        IDictionary<string, string> row2 = new Dictionary<string, string>();
                        row1.Add("组号", zh);
                        row1.Add("类型", "采集数据");
                        row2.Add("组号", zh);
                        row2.Add("类型", "报告数据");
                        // 获取该组号的试验唯一号
                        foreach (var item in heads)
                        {
                            // 试验数据
                            var q1 = from e in sywyhs where e["zh"] == zh select e["sywyh"];
                            string sysj = "";
                            foreach (var sywyh in q1)
                            {
                                var q2 = from e in dtSysjs where e["sywyh"] == sywyh && e["zdhy"] == item.zdhy select e;
                                if (q2.Count() > 0)
                                {
                                    sysj = q2.First()["zdz"];
                                    break;
                                }
                            }
                            row1.Add(item.zdsy, sysj);
                            // 报告数据
                            string bgsj = "";
                            var q3 = from e in dtBgs where e["zh"] == zh && e["zdhy"] == item.zdhy select e;
                            if (q3.Count() > 0)
                            {
                                bgsj = q3.First()["zdz"];
                            }
                            row2.Add(item.zdsy, bgsj);
                        }
                        result.Add(row1);
                        result.Add(row2);
                    }
                }

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 获取合同信息
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJchtInfo(int recid)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from i_m_ht where recid=" + recid;
                result = CommonService.GetDataTable(sql);
                if (result.Count == 0)
                {
                    code = false;
                    msg = "无效的记录号";
                }
            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }

        /// <summary>
        /// 获取有视频的试验数据
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns>[{'zh':''}]</returns>

        [Authorize]
        public JsonResult GetWtdSysjHasSp(string wtdwyh, int xc)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                if (xc != 1)
                {
                    string sql = "select a.*,convert(varchar(50),a.syjssj,120) as sy_syjssj,convert(varchar(50),a.scsj,120) as sy_scsj,(select top 1 b.uploadfileid from companyfileoss b where b.srcfilename=a.spwj order by uploadtime desc) as uploadfileid from up_sysj a where a.spwj is not null and a.spwj<>'' and a.wtdbh='" + wtdwyh + "' order by a.scsj desc";
                    result = CommonService.GetDataTable(sql);
                }
                else
                {
                    string sql = "select a.*,convert(varchar(50),a.jssj,120) as sy_syjssj,convert(varchar(50),a.scsj,120) as sy_scsj,(select top 1 b.uploadfileid from companyfileoss b where substring(b.srcfilename,1,len(b.srcfilename)-4)=substring(a.spwjm,1,len(a.spwjm)-4)) as uploadfileid from up_cjsp a where a.wtdbh='" + wtdwyh + "' order by a.scsj desc";
                    result = CommonService.GetDataTable(sql);
                }

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 获取有效的检测资质分类
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJczzZzfl()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from H_QYZZLX where qylxbh='01' and sfyx=1 order by xssx";
                result = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 获取有效的检测资质
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJczzZznr()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from H_QYZZNR where qylxbh='01' and sfyx=1 order by zzlxbh,xssx";
                result = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 根据pr_m_zzsyxm表中recid获取pr_s_zzsyxm中的记录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJczzxmById(string id)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from pr_s_zzsyxm where parentid='" + id + "'";
                result = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 获取某个委托单见证取样图片列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJzqytps(string id)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> result = new List<IDictionary<string, object>>();
            try
            {

                string url = ClientInfo.GetXcjcImageUrl(HttpContext.Request, "ShowWtdImage?id={0}");
                string viewWtdUrl = ClientInfo.GetCurDnsWithPort(HttpContext.Request) + "/jc/viewwtd?id=" + id;
                result = JcService.JzqyGetWtdJzxq(url, id, viewWtdUrl, out msg);

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });
        }
        /// <summary>
        /// 获取某个委托单现场图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetXcjctps(string id,string zh)
        {
            bool code = true;
            string msg = "";
            IDictionary<string, object> result = new Dictionary<string, object>();
            try
            {

                string url = ClientInfo.GetXcjcImageUrl(HttpContext.Request, "ShowWtdXcjcImage?id={0}");
                result = JcService.XcjcGetImages(url, id, zh, out msg);

            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = result });


        }
        /// <summary>
        /// 获取地图默认标注点
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public JsonResult GetDefaultMap()
        {
            return Json(new { title = GlobalVariable.DefaultMapTitle, pos = GlobalVariable.DefaultMapPos });
        }
        /// <summary>
        /// 获取正在进行的现场检测
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetOnXcjcInfos()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = JcService.XcjcGetOnExperments(CurrentUser.UserName);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, result = ret });
        }

        /// <summary>
        /// 根据检测机构账号获取编号
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSelfJcjgbh()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonService.GetDataTable("select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "' and qybh in (select qybh from view_i_m_qy_jczx)");
                if (ret.Count > 0)
                    msg = ret[0]["qybh"];
                else
                {
                    code = false;
                    msg = "获取检测机构失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        public JsonResult GetXcjcsjId(string id)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonService.GetDataTable("select top 1 cjsybh from UP_CJJL where wtdbh='" + id + "' and cjsybh<>''");
                if (ret.Count > 0)
                    msg = ret[0]["cjsybh"].EncodeBase64();
                else
                {
                    code = false;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 获取组合项目的子项目
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="syxmbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetZxms(string qybh, string syxmbh)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select *,(select top 1 xmdh from PR_M_WTDWJH where ssdwbh = pr_m_syxm.ssdwbh and syxmbh = pr_m_syxm.syxmbh) xmdh from PR_M_SYXM where syxmbh in (" +
                    "SELECT zxmbh from PR_M_ZB where recid in (" +
                    "select zbbh from PR_M_QYZB where zbbh in (select zbbh from PR_S_CP_ZB where syxmbh='" + syxmbh + "') and qybh='" + qybh + "'" +
                    "and YXQS<=getdate() and YXQZ>=convert(datetime,convert(varchar(50),getdate(),23))))  and ssdwbh='" + qybh + "'";
                ret = CommonService.GetDataTable(sql);
                if (ret.Count == 0)
                {
                    msg = "找不到子项目";
                    code = false;
                }

                foreach (var item in ret)
                {
                    if (string.IsNullOrEmpty(item["xmdh"]))
                    {
                        item["xmdh"] = item["syxmbh"];
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, result = ret });
        }
        /// <summary>
        /// 获取某个单位某个项目的有效期
        /// </summary>
        [Authorize]
        public JsonResult GetOneValidDwzb(string dwbh, string zbbh)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            string msg = "";
            bool code = false;
            try
            {
                IList<IDictionary<string, string>> qyzbs = CommonService.GetDataTable("select recid,qybh,zbbh,yxqs,yxqz from pr_m_qyzb where qybh='" + dwbh + "' and zbbh='" + zbbh + "' and YXQS<=getdate() and dateadd(d,1,YXQZ)>getdate() order by YXQZ desc");
                if (qyzbs.Count > 0)
                {
                    ret = qyzbs[0];
                    ret["yxqs"] = ret["yxqs"].GetSafeDate().ToString("yyyy-MM-dd");
                    ret["yxqz"] = ret["yxqz"].GetSafeDate().ToString("yyyy-MM-dd");
                    code = true;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
            }

            return Json(new { code = code ? "0" : "1", msg = msg, result = ret });
        }
        /// <summary>
        /// 删除某个企业指标
        /// </summary>
        [Authorize]
        public JsonResult DeleteQyzb(string id)
        {
            string msg = "";
            bool code = false;
            try
            {
                code = CommonService.Execsql("delete from pr_m_qyzb where recid=" + id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region 打印
        /// <summary>
        /// 委托单打印页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult PrintWts()
        {
            /*
            string syxmbh = Request["syxmbh"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            string url = "/jc/getexcelwts?syxmbh=" + syxmbh + "&recid=" + recid;

            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ID = "PageOfficeCtrl1";
            pc.SaveFilePage = "/jc/saveexcel?syxmbh=" + syxmbh + "&recid=" + recid;
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Titlebar = false; //隐藏标题栏
            pc.Menubar = false; //隐藏菜单栏

            pc.OfficeToolbars = false; //隐藏Office工具栏
            pc.CustomToolbar = false; //隐藏自定义工具栏

            PageOffice.ExcelWriter.Workbook workBook = new PageOffice.ExcelWriter.Workbook();
            workBook.DisableSheetRightClick = true;//禁止当前工作表的鼠标右键
            pc.SetWriter(workBook);

            System.Web.UI.Page page = new System.Web.UI.Page();
            pc.WebOpen(url, PageOffice.OpenModeType.xlsReadOnly, CurrentUser.UserName);

            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false);
                    ViewBag.EditorHtml = sb.ToString();
                }
            }

            return View();*/
            string url = "";

            string syxmbh = Request["syxmbh"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            string reportFile = Request["wtsmb"].GetSafeString();

            string upurl = JcService.GetUploadWtdUrl(recid);

            if (string.IsNullOrEmpty(upurl))
            {
                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                c.type = ReportPrint.EnumType.Excel;
                c.fileindex = "1";
                c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                c.filename = reportFile.Replace(",", "|");

                c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print;
                c.openType = ReportPrint.OpenType.PDFPrint;
                c.AllowVisitNum = 1;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                var guid = g.Add(c);

                url = "/reportPrint/Index?" + guid + "&c=1";
            }
            else
            {
                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                c.type = ReportPrint.EnumType.Excel;
                c.fileindex = "1";
                c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                c.filename = reportFile.Replace(",", "|");

                c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print;
                c.AllowVisitNum = 1;
                c.libType = ReportPrint.LibType.OpenXmlSdk;

                c.openType = ReportPrint.OpenType.PDFFilePrint;
                c.pdfurl = upurl;

                var guid = g.Add(c);

                url = "/reportPrint/Index?" + guid + "&c=1";
            }
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }
        [Authorize]//更新打印份数 2019-6-05
        public ActionResult BatchPrintWts()
        {
            string url = "";
            int clientdyfs =Request["dyfs"].GetSafeInt(0); 
            string reqParam = Request["reqparam"].GetSafeString();
            string[] items = reqParam.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            items = items.Distinct().ToArray();
            StringBuilder sbGuids = new StringBuilder();
            // syxmbh|recid|wtdmb
            IList<string> wtdwyhs = new List<string>();
            foreach (string item in items)
            {
                string[] arr = item.Split(new char[] { '|' });
                if (arr.Length < 2)
                    continue;
                wtdwyhs.Add(arr[1]);
            }
            string msg = "";
            IList<IDictionary<string, string>> wtddyfss = JcService.GetSyxmWtddyfs(wtdwyhs, out msg);

            IDictionary<string, IDictionary<string, IList<IDictionary<string, object>>>> datas = JcService.GetWtdPrintInfos(items, out msg);

            foreach (string item in items)
            {
                string[] arr = item.Split(new char[] { '|' });
                string syxmbh = arr[0];
                string recid = arr[1];
                string reportFile = "";
                if (arr.Length > 2)
                    reportFile = arr[2];
                // 获取打印份数
                int dyfs = 1;
                IList<IDictionary<string, string>> wtddyfs = wtddyfss.Where(e => e["wtdwyh"].Equals(recid)).ToList();
                if (clientdyfs == 0 && wtddyfs.Count > 0)
                    dyfs = wtddyfs[0]["wtddyfs"].GetSafeInt();
                else
                    dyfs = clientdyfs;
                if (dyfs < 1)
                    dyfs = 1;    
                IDictionary<string, IList<IDictionary<string, object>>> findDatas = datas[recid];
                if (findDatas == null)
                    continue;

                string upurl = JcService.GetUploadWtdUrl(recid);

                if (string.IsNullOrEmpty(upurl))
                {
                    for (int i = 0; i < dyfs; i++)
                    {

                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Excel;
                        c.fileindex = "1";
                        c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                        c.filename = reportFile.Replace(",", "|");

                        c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                        c.signindex = 0;
                        //c.openType = ReportPrint.OpenType.Print;
                        c.openType = ReportPrint.OpenType.PDFPrint;
                        c.AllowVisitNum = 1;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        c.data = findDatas;
                        var guid = g.Add(c);
                        sbGuids.Append(guid + "|");
                    }
                }
                else
                {
                    for (int i = 0; i < dyfs; i++)
                    {

                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Excel;
                        c.fileindex = "1";
                        c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                        c.filename = reportFile.Replace(",", "|");

                        c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                        c.signindex = 0;
                        //c.openType = ReportPrint.OpenType.Print;
                        c.openType = ReportPrint.OpenType.PDFFilePrint;
                        c.pdfurl = upurl;
                        c.AllowVisitNum = 1;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        c.data = findDatas;
                        var guid = g.Add(c);
                        sbGuids.Append(guid + "|");
                    }
                }
            }

            string strGuid = sbGuids.ToString().Trim(new char[] { '|' });


            url = "/ReportPrint/BatchPrinting?id=" + strGuid + "&c=1";
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }
        /// <summary>
        /// 获取模板内容
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetExcelWts()
        {
            try
            {
                string syxmbh = Request["syxmbh"].GetSafeString().ToLower();
                string recid = Request["recid"].GetSafeString();

                IDictionary<string, string> wheres = new Dictionary<string, string>();
                wheres.Add("m_by", "recid='" + recid + "'");
                wheres.Add("s_by", "byzbrecid='" + recid + "'");
                wheres.Add("m_d_" + syxmbh, "recid='" + recid + "'");
                wheres.Add("s_d_" + syxmbh, "byzbrecid='" + recid + "'");
                wheres.Add("m_" + syxmbh, "recid='" + recid + "'");
                wheres.Add("s_" + syxmbh, "byzbrecid='" + recid + "'");
                byte[] fileContent = ExcelPrintService.FormatWts(Server.MapPath("/report/jc/wts/" + syxmbh + ".xls"), wheres, Remote.UserService.GetUserSign);
                //SysLog4.WriteError(DateTime.Now.ToString());
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office.xls");
                Response.Charset = "UTF-8";
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(fileContent);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 委托单查看页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWts()
        {
            /*
            string syxmbh = Request["syxmbh"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            string url = "/jc/getexcelwts?syxmbh=" + syxmbh + "&recid=" + recid;

            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ID = "PageOfficeCtrl1";
            pc.SaveFilePage = "/jc/saveexcel?syxmbh=" + syxmbh + "&recid=" + recid;
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Titlebar = false; //隐藏标题栏
            pc.Menubar = false; //隐藏菜单栏

            pc.OfficeToolbars = false; //隐藏Office工具栏
            pc.CustomToolbar = true; //隐藏自定义工具栏

            pc.AddCustomToolButton("打印预览", "printView()", 7);

            PageOffice.ExcelWriter.Workbook workBook = new PageOffice.ExcelWriter.Workbook();
            workBook.DisableSheetRightClick = true;//禁止当前工作表的鼠标右键
            pc.SetWriter(workBook);

            System.Web.UI.Page page = new System.Web.UI.Page();
            pc.WebOpen(url, PageOffice.OpenModeType.xlsReadOnly, CurrentUser.UserName);

            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false);
                    ViewBag.EditorHtml = sb.ToString();
                }
            }

            return View();*/
            string syxmbh = Request["syxmbh"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            string reportfile = Request["wtsmb"].GetSafeString();
            if (reportfile == "")
                reportfile = syxmbh;
            string url = "";
            string upurl = JcService.GetUploadWtdUrl(recid);

            if (string.IsNullOrEmpty(upurl))
            {
                string msg = "";
                string[] items = new string[] { syxmbh + "|" + recid };
                IDictionary<string, IDictionary<string, IList<IDictionary<string, object>>>> datas = JcService.GetWtdPrintInfos(items, out msg);
                IDictionary<string, IList<IDictionary<string, object>>> data = datas[recid];

                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                c.type = ReportPrint.EnumType.Excel;
                c.openType = ReportPrint.OpenType.PDF;
                //c.field = reportFile;
                c.fileindex = "1";
                c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                c.filename = reportfile.Replace(",", "|");
                //c.field = "formid";
                c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print ;
                c.AllowVisitNum = 1;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                c.data = data;
                c.customtools = "-1";
                var guid = g.Add(c);

                url = "/reportPrint/Index?" + guid;
            }
            else
            {
                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();

                c.type = ReportPrint.EnumType.Excel;
                c.fileindex = "1";
                c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                c.filename = reportfile.Replace(",", "|");
                c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                c.signindex = 0;
                c.AllowVisitNum = 1;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                //c.data = data;

                c.openType = ReportPrint.OpenType.PDFFile;
                c.pdfurl = upurl;
                c.customtools = "-1";
                var guid = g.Add(c);

                url = "/reportPrint/Index?" + guid;
            }
            //string url = "/reportPrint/Index?type=excel&filename="+syxmbh+"&table="+c.table+"&field=&where="+c.where+"&fileindex=1";
            return new RedirectResult(url);
        }
        /// <summary>
        /// 查看检测登记表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewJcdjb()
        {
            string recid = Request["id"].GetSafeString();
            string reportfile = Request["file"].GetSafeString();

            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            c.openType = ReportPrint.OpenType.PDF;
            //c.field = reportFile;
            c.fileindex = "2";
            c.table = "view_i_m_ht_djb";
            c.filename = reportfile;
            //c.field = "formid";
            c.where = "recid=" + recid;
            c.signindex = 0;
            c.customtools = "1,|2,";
            //c.openType = ReportPrint.OpenType.Print ;
            c.AllowVisitNum = 1;
            var guid = g.Add(c);

            string url = "/reportPrint/Index?" + guid;
            //string url = "/reportPrint/Index?type=excel&filename="+syxmbh+"&table="+c.table+"&field=&where="+c.where+"&fileindex=1";
            return new RedirectResult(url);
        }
        #endregion

        #region 汇总数据
        [Authorize]
        public void GetGcs()
        {
            bool code = false;
            string msg = "";
            try
            {
                string sql = "select count(*) from i_m_gc where (not exists (select * from i_m_qyzh where yhzh='" + CurrentUser.UserName + "') or gcbh in (select gcbh from i_s_gc_syry where rybh='" + CurrentUser.UserName + "'))";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 手机查询
        /// <summary>
        /// 状态列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MStatusList()
        {
            ViewBag.username = Request["username"].GetSafeRequest();
            ViewBag.company = Server.UrlDecode(Request["company"].GetSafeRequest());
            return View();
        }
        /// <summary>
        /// 获取委托单列表，跟当前人员挂钩
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWtdList(string syxmbh, string zt, string lrrzh, string gcbh, int pagesize, int pageindex)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            string msg = "";
            bool code = false;
            int totalcount = 0;
            try
            {
                code = JcService.GetWtds(syxmbh, zt, lrrzh, gcbh, pagesize, pageindex, out totalcount, out ret, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, totalcount = totalcount, records = ret });
        }
        /// <summary>
        /// 获取工程列表，跟当前人员挂钩
        /// </summary>
        /// <param name="ryzh"></param>
        /// <returns></returns>
        public JsonResult GetGcList(string ryzh)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            string msg = "";
            bool code = false;
            try
            {
                code = JcService.GetGcs(ryzh, out ret, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        #endregion

        #region 摄像头平台
        /// <summary>
        /// 往摄像头平台添加摄像头
        /// </summary>
        [Authorize]
        public JsonResult DoAddSxtToPlatform()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeString();
                code = SxtptService.Register(recid, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 获取摄像头查看地址
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSxtptViewUrl()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeString();
                code = SxtptService.GetPlayUrl(recid, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 萤石云播放
        /// </summary>
        /// <returns></returns>

        public ActionResult PlayerYsy()
        {
            try
            {
                string recid = Request["id"].GetSafeString();
                string msg = "";
                bool code = SxtptService.GetPlayUrl(recid, out msg);
                //if (code)
                ViewBag.playurl = msg;
                //else
                //    ViewBag.playurl = "";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return View();
        }
        /// <summary>
        /// 摄像头抓拍
        /// </summary>
        /// <param name="sxtid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult CaptureSxtImage(string sxtid)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = SxtptService.CaptuerImage(sxtid, CurrentUser.UserName, CurrentUser.RealName, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 获取抓拍图片
        /// </summary>
        [Authorize]
        public void GetSxtCaptureImage()
        {
            byte[] file = null;
            string tpwyh = Request["tpwyh"].GetSafeString();
            try
            {
                string msg = "";
                file = SxtptService.GetCaptureImageContent(tpwyh, out msg);
                if (file != null)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "image/jpeg";
                    Response.BinaryWrite(file);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 获取抓拍图片
        /// </summary>
        [Authorize]
        public JsonResult GetSxtCaptureImages(string sxtid, string from)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                dt = SxtptService.GetCaptureImages(sxtid, from, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = dt });
        }
        #endregion

        #region 监管首页数据获取
        /// <summary>
        /// 获取监管人员省市区
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgAreaList()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase))
                {
                    ret = JcService.GetJgAreaList(CurrentUser.UserCode, out msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        /// <summary>
        /// 获取监管人员检测机构
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgJcjgList()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase))
                {
                    ret = JcService.GetJgJcjgList(CurrentUser.UserCode, out msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }

        /// <summary>
        /// 获取监管人员工程列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgGcList(string sfid, string csid, string xqid, string jdid)
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase))
                {
                    ret = JcService.GetJgGcList(CurrentUser.UserCode, sfid, csid, xqid, jdid, out msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        /// <summary>
        /// 获取监管工程统计
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgStatistic(string sfid, string csid, string xqid, string jdid, string jcjgid, string gcid)
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase) && jcjgid != "null")
                {
                    ret = JcService.GetJgStatistic(CurrentUser.UserCode, sfid, csid, xqid, jdid, jcjgid, gcid, out msg);
                    code = string.IsNullOrEmpty(msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }
        /// <summary>
        /// 获取报告合格不合格统计
        /// </summary>
        /// <param name="sfid"></param>
        /// <param name="csid"></param>
        /// <param name="xqid"></param>
        /// <param name="jdid"></param>
        /// <param name="jcjgid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgBgStatistic(string sfid, string csid, string xqid, string jdid, string jcjgid, string gcid)
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase) && jcjgid != "null")
                {
                    ret = JcService.GetJgBgStatistic(CurrentUser.UserCode, sfid, csid, xqid, jdid, jcjgid, gcid, out msg);
                    code = string.IsNullOrEmpty(msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }
        /// <summary>
        /// 获取委托单各个状态统计
        /// </summary>
        /// <param name="sfid"></param>
        /// <param name="csid"></param>
        /// <param name="xqid"></param>
        /// <param name="jdid"></param>
        /// <param name="jcjgid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgWtdztStatistic(string sfid, string csid, string xqid, string jdid, string jcjgid, string gcid)
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase) && jcjgid != "null")
                {
                    ret = JcService.GetJgWtdztStatistic(CurrentUser.UserCode, sfid, csid, xqid, jdid, jcjgid, gcid, out msg);
                    code = string.IsNullOrEmpty(msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }
        /// <summary>
        /// 获取委托单异常状态统计
        /// </summary>
        /// <param name="sfid"></param>
        /// <param name="csid"></param>
        /// <param name="xqid"></param>
        /// <param name="jdid"></param>
        /// <param name="jcjgid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgWtdycztStatistic(string sfid, string csid, string xqid, string jdid, string jcjgid, string gcid)
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase) && jcjgid != "null")
                {
                    ret = JcService.GetJgWtdycztStatistic(CurrentUser.UserCode, sfid, csid, xqid, jdid, jcjgid, gcid, out msg);
                    code = string.IsNullOrEmpty(msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }
        /// <summary>
        /// 获取检测结构委托单状态统计
        /// </summary>
        /// <param name="sfid"></param>
        /// <param name="csid"></param>
        /// <param name="xqid"></param>
        /// <param name="jdid"></param>
        /// <param name="jcjgid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJgJcjgWtdztStatistic(string sfid, string csid, string xqid, string jdid, string jcjgid, string gcid)
        {
            string msg = "";
            bool code = true;
            IDictionary<string, IDictionary<string, string>> ret = new Dictionary<string, IDictionary<string, string>>();
            try
            {
                // 非管理人员账号，不能获取
                if (CurrentUser.CurUser.UrlJumpType.StartsWith("N", StringComparison.OrdinalIgnoreCase) && jcjgid != "null")
                {
                    ret = JcService.GetJgJcjgWtdztStatistic(CurrentUser.UserCode, sfid, csid, xqid, jdid, jcjgid, gcid, out msg);
                    code = string.IsNullOrEmpty(msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }

        #endregion

        #region 组合项目处理
        /// <summary>
        /// 组合项目，把记录复制到其他项目
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult CopyCombinationInfos(string wtdwyh)
        {
            string msg = "";
            bool code = true;
            try
            {
                code = JcService.CopyCombinationInfos(wtdwyh, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region 手动上传报告
        static IDictionary<string, IList<KeyValuePair<string, string>>> m_ReportFiles = new Dictionary<string, IList<KeyValuePair<string, string>>>();
        /// <summary>
        /// 上传报告文件
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoUploadReport()
        {
            bool code = true;
            string msg = "";
            try
            {
                string fileid = Request["id"].GetSafeString();
                if (Request.Files.Count == 0)
                {
                    code = false;
                    msg = "没有需要上传的文件";
                }
                else
                {
                    HttpPostedFileBase postfile = Request.Files[0];
                    string filename = postfile.FileName;
                    byte[] postcontent = new byte[postfile.ContentLength];
                    int readlength = 0;
                    while (readlength < postfile.ContentLength)
                    {
                        int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    string filebase64 = Convert.ToBase64String(postcontent);

                    string filekey = "REPORT_" + fileid;
                    if (filename.EndsWith("docx", StringComparison.OrdinalIgnoreCase))
                    {
                        string outfilestring = "";
                        code = new OfficeConvert().ConvertWordToPdfStr(filebase64, out outfilestring, out msg);
                        if (code)
                            filebase64 = outfilestring;
                    }
                    else if (filename.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        string outfilestring = "";
                        code = new OfficeConvert().ConvertExcelToPdfStr(filebase64, out outfilestring, out msg);
                        if (code)
                            filebase64 = outfilestring;
                    }
                    else if (!filename.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        code = false;
                        msg = "无效的文件类型";
                    }

                    if (code)
                    {
                        IList<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
                        if (m_ReportFiles.ContainsKey(filekey))
                        {
                            files = m_ReportFiles[filekey];
                            m_ReportFiles.Remove(filekey);
                        }
                        files.Add(new KeyValuePair<string, string>(filename, filebase64));
                        m_ReportFiles.Add(filekey, files);
                    }

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 删除session中的报告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoDeleteReport(string id, string filename)
        {
            bool code = true;
            string msg = "";
            try
            {
                string filekey = "REPORT_" + id;

                IList<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
                if (m_ReportFiles.ContainsKey(filekey))
                {
                    files = m_ReportFiles[filekey];
                    m_ReportFiles.Remove(filekey);

                    foreach (KeyValuePair<string, string> file in files)
                    {
                        if (file.Key.Equals(filename))
                        {
                            files.Remove(file);
                            break;
                        }
                    }
                    m_ReportFiles.Add(filekey, files);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 保存报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="id"></param>
        /// <param name="bgbh"></param>
        /// <param name="jcjg"></param>
        /// <param name="jcjgms"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoSaveReportInfo(string wtdwyh, string id, string bgbh, int jcjg, string jcjgms)
        {
            bool code = true;
            string msg = "";
            try
            {
                //获取协会需要上传字段
                string dataJson = Request["datajson"].GetSafeString();

                var ret = JcService.XsxhUploadBgData(wtdwyh, dataJson);

                if (ret.success)
                {
                    dataJson = ret.data as string;
                }
                else 
                {
                    code = ret.success;
                    msg = ret.msg;
                    return Json(new { code = code ? "0" : "1", msg = msg });
                }
                //

                string filekey = "REPORT_" + id;
                if (!m_ReportFiles.ContainsKey(filekey))
                {
                    code = false;
                    msg = "文件不能为空";
                }
                else
                {
                    IList<KeyValuePair<string, string>> datas = m_ReportFiles[filekey];
                    if (datas == null || datas.Count == 0)
                    {
                        code = false;
                        msg = "文件不能为空";
                    }
                    else
                    {
                        IList<VTransUpBgwj> files = new List<VTransUpBgwj>();
                        foreach (KeyValuePair<string, string> data in datas)
                            files.Add(new VTransUpBgwj() { bgwj = data.Value });
                        JavaScriptSerializer serial = new JavaScriptSerializer();
                        serial.MaxJsonLength = int.MaxValue;
                        string strfiles = serial.Serialize(files);
                        string qybh = JcService.GetQybh(CurrentUser.UserCode);
                        code = JcService.UpReport(qybh, wtdwyh, bgbh, "", "", "", DateTime.Now, DateTime.Now, jcjg, jcjgms, "[]", "[]", strfiles, true, true, dataJson, out msg);
                        if (code)
                            m_ReportFiles.Remove(filekey);
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 删除报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="id"></param>
        /// <param name="bgbh"></param>
        /// <param name="jcjg"></param>
        /// <param name="jcjgms"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoDeleteReportInfo(string bgwyh)
        {
            bool code = true;
            string msg = "";
            try
            {
                code = JcService.DeleteReportSdsc(bgwyh, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 下载委托单最后一次上传的报告，压缩成zip
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [Authorize]
        public void DoDownLastReportFile(string wtdwyh)
        {
            ZipFile ret = new ZipFile(System.Text.Encoding.UTF8);

            try
            {
                string msg = "";

                IList<byte[]> files = JcService.GetLastReportFile(wtdwyh, out msg);
                int index = 1;
                foreach (byte[] file in files)
                {
                    string filename = wtdwyh + "_" + (index++).ToString() + ".pdf";
                    ret.AddEntry(filename, file);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.HeaderEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + wtdwyh + ".zip");
            ret.Save(Response.OutputStream);
            Response.End();
        }
        #endregion

        #region 无二维码见证信息
        /// <summary>
        /// 无二维码申请
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sqms"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DoSaveNoQrcodeReq(string wtdwyh, string sqms)
        {
            bool code = true;
            string msg = "";
            try
            {
                code = JcService.NoQrcodeReq(wtdwyh, sqms, CurrentUser.UserName, CurrentUser.RealName, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region 页面-见证取样(标点)
        /// <summary>
        /// 见证取样
        /// </summary>
        /// <returns></returns>
        public ActionResult JzqyLogin()
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult JzqySearch()
        {
            //获取微信信息
            string timestamp = WeiXinHelper.CreateTimestamp();
            string noncestr = WeiXinHelper.CreateNonceStr();
            string url = Request.Url.ToString();
            string signature = WeiXinHelper.GetSignature(timestamp, noncestr, url);
            IDictionary<string, string> dt = new Dictionary<string, string>();
            //AppID
            ViewData["appId"] = Configs.WxAppid;
            //时间戳
            ViewData["timestamp"] = timestamp;
            //随机数
            ViewData["nonceStr"] = noncestr;
            //验证码
            ViewData["signature"] = signature;
            return View();
        }

        public ActionResult JzqyList()
        {
            //获取微信信息
            string timestamp = WeiXinHelper.CreateTimestamp();
            string noncestr = WeiXinHelper.CreateNonceStr();
            string url = Request.Url.ToString();
            string signature = WeiXinHelper.GetSignature(timestamp, noncestr, url);
            IDictionary<string, string> dt = new Dictionary<string, string>();
            //AppID
            ViewData["appId"] = Configs.WxAppid;
            //时间戳
            ViewData["timestamp"] = timestamp;
            //随机数
            ViewData["nonceStr"] = noncestr;
            //验证码
            ViewData["signature"] = signature;
            return View();
        }

        public ActionResult Jzqy()
        {
            return View();
        }

        public ActionResult qrcode()
        {
            //获取微信信息
            string timestamp = WeiXinHelper.CreateTimestamp();
            string noncestr = WeiXinHelper.CreateNonceStr();
            string url = Request.Url.ToString();
            string signature = WeiXinHelper.GetSignature(timestamp, noncestr, url);
            IDictionary<string, string> dt = new Dictionary<string, string>();
            //AppID
            ViewData["appId"] = Configs.WxAppid;
            //时间戳
            ViewData["timestamp"] = timestamp;
            //随机数
            ViewData["nonceStr"] = noncestr;
            //验证码
            ViewData["signature"] = signature;
            return View();
        }

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult JzqyImage()
        {
            ResultParam ret = new ResultParam();
            try
            {
                string mediaId = Request["id"].GetSafeString();
                ret = JcService.DownloadWeiXinImage(mediaId);
            }
            catch (Exception e)
            {
                ret.msg = e.Message;
            }
            return Content((new JavaScriptSerializer()).Serialize(ret));
        }

        #endregion

        #region 确认委托单可以下载到检测系统中
        /// <summary>
        /// 确认委托单可以下载到检测系统中
        /// </summary>
        /// <param name="recids"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ConfirmDownload(string recids)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(recids))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.UpdateConfirmDownload(recids, 1, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        } 
        #endregion

        #region 委托单不可以下载到检测系统中
        /// <summary>
        /// 委托单不可以下载到检测系统中
        /// </summary>
        /// <param name="recids"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult UnConfirmDownload(string recids)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(recids))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.UpdateConfirmDownload(recids, 0, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion

        #region 获取委托单填单类型
        /// <summary>
        /// 获取委托单填单类型 0- 送样人填单 1- 监督员填单 2- 内部填单 
        /// </summary>
        /// <param name="recids"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetWtdType()
        {
            bool code = true;
            string msg = string.Empty;
            int wtdType = 0;

            try
            {
                var roles = Session["ROLES"] as string;

                if (string.IsNullOrEmpty(roles))
                {
                    roles = UserService.GetRoles(Configs.AppId, CurrentUser.RealUserName);
                }

                //监督员 监督员填单
                if (roles.IndexOf("CR201903000001") != -1)
                {
                    wtdType = 1;
                }
                //检测机构 内部填单
                else if (roles.IndexOf("CR201709000003") != -1)
                {
                    wtdType = 2;
                }
               
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, wtdType });
        }
        #endregion

        #region 委托单取消见证
        /// <summary>
        /// 委托单取消见证
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CancelWtdJz(string wtdwyh)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(wtdwyh))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.CancelWtdJz(wtdwyh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion

        #region 委托单关联见证
        /// <summary>
        /// 委托单关联见证
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="oldwtdwyh"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SaveContactWtdJz(string wtdwyh, string oldwtdwyh)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(wtdwyh) || string.IsNullOrEmpty(oldwtdwyh))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.ContactWtdJz(wtdwyh, oldwtdwyh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion

        #region 微信二维码扫描
        /// <summary>
        /// 扫二维码防伪
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult QRAntiFake(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                ret = JcService.QRAntiFake(wtdwyh);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return Content(JsonFormat.GetString(ret));
        }

        #endregion

        #region 非监督工程关联上级监督工程
        /// <summary>
        /// 非监督工程关联上级监督工程
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="sjgcbh"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ContactJdgc(string gcbh, string ssjcjgbh, string sjgcbh)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(gcbh)
                    || string.IsNullOrEmpty(ssjcjgbh)
                    || string.IsNullOrEmpty(sjgcbh))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.ContactJdgc(gcbh, ssjcjgbh, sjgcbh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion

        #region 判断上级监督工程是否被检测机构重复引用

        /// <summary>
        /// 判断上级监督工程是否被检测机构重复引用
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult IsJdgcUsed(string gcbh)
        {
            bool code = true;
            string msg = string.Empty;
            string qybh = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(gcbh))
                {
                    code = false;
                    msg = "传入的参数为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.IsJdgcUsed(gcbh, out qybh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, qybh });
        } 
        #endregion

        #region 获取下一页委托单，所有委托单信息
        /// <summary>
        /// 获取下一页委托单，所有委托单信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetNext()
        {

            string current = Request["current"].GetSafeString();
            string all = Request["all"].GetSafeString();
            ViewBag.current = current;
            ViewBag.all = all;
            return View();
        } 
        #endregion

        #region 分页获取建研未委托的见证取样记录
        public ActionResult JzqyUpLoadDataListPage()
        {
            var ret = new ResultParam
            {
                success = true,
                data = new
                {
                    total = 0,
                    rows = new List<Dictionary<string, string>>()
                }
            };

            try
            {
                string msg = string.Empty;
                var data = Request.Form["data"].GetSafeString();
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return Json(ret);
                }

                if (!dataDict.ContainsKey("fieldname")
                 || !dataDict.ContainsKey("wherefield")
                 || !dataDict.ContainsKey("wherectrl")
                 || !dataDict.ContainsKey("col")
                 || !dataDict.ContainsKey("val"))
                {
                    ret.msg = "data不包含fieldname, wherefield, wherectrl, col, val 字段";
                    return Json(ret);
                }

                var wherefield = dataDict["wherefield"];
                var wherectrl = dataDict["wherectrl"];
                var col = dataDict["col"];
                var val = dataDict["val"];

                var fields = wherefield.Split(',');
                var values = wherectrl.Split(',');

                if (fields.Length != values.Length)
                {
                    ret.msg = "传入条件和条件参数数量不匹配";
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }

                var dict = new Dictionary<string, string>();

                for (int i = 0; i < fields.Length; i++)
                {
                    dict.Add(fields[i], values[i]);
                }

                string gcbw = string.Empty;
                string qrCode = string.Empty;
                string recordId = string.Empty;

                if (col == "GCBW")
                    gcbw = val;
                else if (col == "QRCODE")
                    qrCode = val;
                else if (col == "JZRECID")
                    recordId = val;

                string projectNum = DictionaryHelper.GetValue(dict, "GCBH");
                string itemCode = DictionaryHelper.GetValue(dict, "SYXMBH");
                string startTime = "2019-01-01";
                string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                string jstartTime = string.Empty;
                string jendTime = string.Empty;
                string syyNum = string.Empty;
                string jzyNum = string.Empty;
                string syyName = string.Empty;
                string jzyName = string.Empty;
                int orderStatus = 0;
                int page = dataDict["page"].GetSafeInt();
                int limit = dataDict["rows"].GetSafeInt();
                int total = 0;

                var jzqyRet = JyJzqyService.UpLoadDataListPage(projectNum, gcbw, itemCode, startTime, endTime, jstartTime, jendTime, syyNum
                                , jzyNum, syyName, jzyName, recordId, qrCode, orderStatus, page, limit, out total);

                if (!jzqyRet.success)
                {
                    msg = jzqyRet.msg;
                    return Json(ret);
                }

                var rows = new List<Dictionary<string, string>>();
                var jzqyRetDatas = jzqyRet.data as List<Dictionary<string, string>>;
                var userNames = jzqyRetDatas.Select(x => x["SPNID"]).Distinct();
                IList<IDictionary<string,string>> rys = new List<IDictionary<string, string>>();
                JcService.GetRybhByZh(string.Join(",", userNames), out rys, out msg);

                var fieldnames = dataDict["fieldname"].Split(',').ToList();

                foreach (var jzqyRetData in jzqyRetDatas)
                {
                    var item = JsonSerializer.Deserialize<Dictionary<string, string>>(jzqyRetData["ITEMJSON"]);

                    var row = new Dictionary<string, string>();

                    var rybh = string.Empty;
                    if(!string.IsNullOrEmpty(jzqyRetData["SPNID"]))
                    {
                        var info = rys.FirstOrDefault(x => x["zh"].Trim().ToLower() == jzqyRetData["SPNID"].Trim().ToLower());
                        rybh = DictionaryHelper.GetValue(info, "rybh");
                    }

                    //固定字段
                    row.Add("JZRECID", jzqyRetData["GUID"]);
                    row.Add("QRCODE", jzqyRetData["QRINFO"]);
                    row.Add("JZRBH", rybh);
                    row.Add("JZRXM", jzqyRetData["SPNNAME"]);
                    row.Add("ISJZ", !string.IsNullOrEmpty(jzqyRetData["SPNID"]) ? "是" : "否");
                    row.Add("EDITSTATUS", GetEditStatusName(jzqyRetData["EDITSTATUS"]));

                    //扩展字段
                    foreach (var fieldname in fieldnames)
                    {
                        //去除固定字段包含
                        if (row.ContainsKey(fieldname.ToUpper()))
                            continue;

                        row.Add(fieldname, DictionaryHelper.GetValueIgnoreCase(item, fieldname));
                    }

                    rows.Add(row);
                }

                ret.success = jzqyRet.success;
                ret.msg = jzqyRet.msg;
                ret.data = new
                {
                    total,
                    rows
                };
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return Json(ret);
        } 
        #endregion

        #region 分页获取建研未委托的取样清单记录
        public ActionResult JyJzqyListPage()
        {
            var ret = new ResultParam
            {
                success = true,
                data = new
                {
                    total = 0,
                    rows = new List<Dictionary<string, string>>()
                }
            };

            try
            {
                string msg = string.Empty;
                var gcbh = Request["gcbh"].GetSafeString();
                var page = Request["page"].GetSafeInt(1);
                var pageSize = Request["pageSize"].GetSafeInt(20);

                //过滤条件
                var filterRules = Request["filterRules"].GetSafeString();
                var filters = new List<Dictionary<string, string>>();

                if(!string.IsNullOrEmpty(filterRules))
                    filters = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(filterRules);

                string gcbw = string.Empty;
                string syxmbh = string.Empty;
                string recordId = string.Empty;
                string qrCode = string.Empty;
                string startTime = "2019-01-01";
                string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                string jstartTime = string.Empty;
                string jendTime = string.Empty;
                string syyNum = string.Empty;
                string jzyNum = string.Empty;
                string syyName = string.Empty;
                string jzyName = string.Empty;
                int orderStatus = 0;
                int limit = pageSize;
                int total = 0;

                foreach (var filter in filters)
                {
                    var fieldName = filter["fieldname"];
                    var fieldValue = filter["fieldvalue"];

                    if (fieldName == "GCBW")
                        gcbw = fieldValue;
                    else if (fieldName == "SYXMMC")
                        syxmbh = CommonService.GetSingleData(string.Format("select syxmbh from pr_m_syxm where ssdwbh='' and syxmmc like '%{0}%'", fieldValue)).GetSafeString();
                    else if (fieldName == "QYRXM")
                        syyName = fieldValue;
                    else if (fieldName == "QYSJ")
                        startTime = fieldValue;
                    else if (fieldName == "JZRXM")
                        jzyName = fieldValue;
                    else if (fieldName == "JZSJ")
                    {
                        jstartTime = fieldValue;
                        jendTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    else if (fieldName == "QRCODE")
                        qrCode = fieldValue;
                }

                var jzqyRet = JyJzqyService.UpLoadDataListPage(gcbh, gcbw, syxmbh, startTime, endTime, jstartTime, jendTime, syyNum
                                , jzyNum, syyName, jzyName, recordId, qrCode, orderStatus, page, limit, out total);

                if (!jzqyRet.success)
                {
                    msg = jzqyRet.msg;
                    return Json(ret);
                }

                var rows = new List<Dictionary<string, string>>();
                var jzqyRetDatas = jzqyRet.data as List<Dictionary<string, string>>;
                var userNames = jzqyRetDatas.Select(x => x["SPNID"]).Distinct();
                var syxmbhs = jzqyRetDatas.Select(x => x["ITEMCODE"]).Distinct();

                IList<IDictionary<string, string>> rys = new List<IDictionary<string, string>>();
                JcService.GetRybhByZh(string.Join(",", userNames), out rys, out msg);
                var helpLinks = JcService.GetJyJzqyHelpLink(string.Join(",", syxmbhs));

                foreach (var jzqyRetData in jzqyRetDatas)
                {
                    var item = JsonSerializer.Deserialize<Dictionary<string, string>>(jzqyRetData["ITEMJSON"]);

                    var row = new Dictionary<string, string>();

                    var rybh = string.Empty;
                    if (!string.IsNullOrEmpty(jzqyRetData["SPNID"]))
                    {
                        var info = rys.FirstOrDefault(x => x["zh"].Trim().ToLower() == jzqyRetData["SPNID"].Trim().ToLower());
                        rybh = DictionaryHelper.GetValue(info, "rybh");
                    }

                    //固定字段
                    row.Add("jzrecid", jzqyRetData["GUID"]);
                    row.Add("qrcode", jzqyRetData["QRINFO"]);
                    row.Add("jzrbh", rybh);
                    row.Add("jzrxm", jzqyRetData["SPNNAME"]);
                    row.Add("isjz", !string.IsNullOrEmpty(jzqyRetData["SPNID"]) ? "是" : "否");
                    row.Add("gcbw", DictionaryHelper.GetValueIgnoreCase(item, "GCBW"));
                    row.Add("editstatus", GetEditStatusName(jzqyRetData["EDITSTATUS"]));

                    string itemString = string.Empty;
                    string itemJson = string.Empty;
                    var itemJsonDict = new Dictionary<string, object>();
                    string itemCode = jzqyRetData["ITEMCODE"];

                    //处理helpLink,返回fieldnames,fieldmcs,targetfields,targetctrls,readonlyctrls
                    string helpLink = GetHelpLinkValue(helpLinks, itemCode, "helplnk");
                    string syxmmc = GetHelpLinkValue(helpLinks, itemCode, "syxmmc");
                    var helpLinkDict = HelpLinkHelper.HandleHelpLink(helpLink);

                    //字段显示
                    List<string> fieldnames = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "fieldname");
                    List<string> fieldmcs = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "fieldmc");
                    List<string> targetfields = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "targetfield");
                    List<string> targetctrls = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "targetctrl");
                    List<string> readonlyctrls = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "readonlyctrl");
                    List<Dictionary<string, string>> fieldList = new List<Dictionary<string, string>>();
                    Dictionary<string, string> fieldDict = new Dictionary<string, string>();

                    //扩展字段
                    for (int i = 0; i < fieldnames.Count(); i++)
                    {
                        //去除固定字段包含
                        if (row.ContainsKey(fieldnames[i].ToLower()))
                            continue;

                        fieldDict = new Dictionary<string, string>();
                        fieldDict.Add("fieldName", fieldmcs[i]);
                        fieldDict.Add("fieldValue", DictionaryHelper.GetValueIgnoreCase(item, fieldnames[i]));
                        fieldList.Add(fieldDict);
                    }

                    var targetList = new List<Dictionary<string, string>>();
                    var targetDict = new Dictionary<string,string>();

                    for (int i = 0; i < targetfields.Count(); i++)
                    {
                        targetDict = new Dictionary<string, string>();
                        //targetDict.Add("targetfield", targetfield);
                        targetDict.Add("targetctrl", targetctrls[i]);

                        if (row.ContainsKey(targetfields[i].ToLower()))
                            targetDict.Add("targetvalue", row[targetfields[i].ToLower()]);
                        else
                            targetDict.Add("targetvalue", DictionaryHelper.GetValueIgnoreCase(item, targetfields[i]));

                        targetList.Add(targetDict);
                    }

                    itemJsonDict.Add("targetlist", targetList);
                    itemJsonDict.Add("readonlyctrls", readonlyctrls);

                    row.Add("syxmbh", itemCode);
                    row.Add("syxmmc", syxmmc);
                    row.Add("qyrxm", jzqyRetData["SLNAME"]);
                    row.Add("qysj", jzqyRetData["SLDATE"]);
                    row.Add("jzsj", jzqyRetData["SPNDATE"]);
                    row.Add("itemstring", JsonSerializer.Serialize(fieldList));
                    row.Add("itemjson", JsonSerializer.Serialize(itemJsonDict));

                    rows.Add(row);
                }

                ret.success = jzqyRet.success;
                ret.msg = jzqyRet.msg;
                ret.data = new
                {
                    total,
                    rows
                };
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return Json(ret);
        }

        private string GetHelpLinkValue(IList<IDictionary<string, string>> helpLinks, string syxmbh, string field)
        {
            var dict = helpLinks.FirstOrDefault(x => x["syxmbh"].ToUpper() == syxmbh.ToUpper());
            return DictionaryHelper.GetValue(dict, field);
        }

        private string GetEditStatusName(string editStatus)
        {
            string editStatusName = "未修改";

            if (editStatus == "0")
            {
                editStatusName = "未确认";
            }
            else if (editStatus == "1")
            {
                editStatusName = "已确认";
            }

            return editStatusName;
        }

        #endregion

        #region 其他接口数据包
        [Authorize]
        /// <summary>
        /// 获取图片链数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult GetTplData()
        {
            ResultParam ret = new ResultParam();
            try
            {
                string wtdwyh = Request["wtdwyh"].GetSafeString();
                string zh = Request["zh"].GetSafeString();
                int pagesize = Request["pagesize"].GetSafeInt(20);
                int pageindex = Request["pageindex"].GetSafeInt(1);
                ret = JcService.XcjcTpl(wtdwyh, zh, pagesize, pageindex);
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Content((new JavaScriptSerializer()).Serialize(ret));
        }

        #endregion

        #region 获取工程坐标
        /// <summary>
        /// 获取工程坐标
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetGczb(string gcbh)
        {
            bool code = true;
            string msg = string.Empty;
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                code = JcService.GetGczb(gcbh, out result, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, result });
        }
        #endregion

        #region 更新i_m_gc表的GCMCNEW字段
        [Authorize]
        public JsonResult UpdateGCMCNEW()
        {
            bool code = true;
            string msg = "";
            try
            {
                code = JcService.UpdateGCMCNEW();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg },JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取单个工程的坐标
        [Authorize]
        public ActionResult GetGcPos(string gcbh)
        {
            bool code = true;
            string msg = string.Empty;
            object data = null;

            try
            {
                if (string.IsNullOrEmpty(gcbh))
                {
                    code = false;
                    msg = "传入的工程编号不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.GetGcPos(gcbh, out data, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        } 
        #endregion

        #region 获取所有工程的坐标
        [Authorize]
        public ActionResult GetAllGcPos()
        {
            bool code = true;
            string msg = string.Empty;
            List<Dictionary<string, object>> data = null;

            try
            {
                string qybh = Session["QYZHBH"].GetSafeString();
                code = JcService.GetAllGcPos(qybh, out data, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }
        #endregion


        #region 萧山接口设置获取
        [Authorize]
        public void getSysYcjkLx()
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try{
                 datas = JcService.getSysYcjkLx();
            }
            catch(Exception e)
            {

            }      
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.ContentType = "text/plain";
            Response.Write(jss.Serialize(datas));
            Response.End();
        }
        [Authorize]
        public void getSysYcjkLxQY()
        {
            string msg = "";
            bool code = false;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                code = JcService.getSysYcjkLx(qybh, out msg);
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 手动设置收样(受理按钮)
        [Authorize]
        public ActionResult SetWtdStatusXf(string wtdwyhs)
        {
            bool code = true;
            string msg = string.Empty;
            string infoMsg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(wtdwyhs))
                {
                    code = false;
                    msg = "传入的委托单唯一号不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                string qybh = JcService.GetQybh(CurrentUser.UserCode);

                if (string.IsNullOrEmpty(qybh))
                {
                    code = false;
                    msg = "检测机构编号不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                //判断是否使用标点检测系统
                var ret = JcService.JudgeBDJcxt(qybh);

                if (!ret.success)
                {
                    msg = ret.msg;
                    return Json(new { code = code ? "0" : "1", msg, infoMsg });
                }

                var wtdwyhArr = wtdwyhs.Trim(',').Split(',');
                string errMsg = string.Empty;

                foreach (var wtdwyh in wtdwyhArr)
                {
                    var result = JcService.SetWtdStatusXf(wtdwyh, qybh, out errMsg, true);

                    if (!result)
                    {
                        msg += string.Format("委托单唯一号[{0}]不能收样,原因:{1}", wtdwyh, errMsg);
                    }
                }

                JcService.JudgeXcjgjc(qybh, wtdwyhs, out infoMsg, out errMsg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, infoMsg });
        }
        #endregion 

        #region 获取所有试验项目
        /// <summary>
        /// 获取所有项目分组
        /// </summary>
        [Authorize]
        public void GetAllXmfz()
        {
            string ret = "[]";

            try
            {
                IList<IDictionary<string, string>> xmfls = CommonService.GetDataTable("select sjxsflbh,xsflbh,xsflmc from pr_m_syxmxsfl where ssdwbh='' order by xssx");
                if (xmfls.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(xmfls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取基础单位的项目
        /// </summary>
        [Authorize]
        public void GetAllXm()
        {
            string ret = "[]";
            int yx = Request["yx"].GetSafeInt(1);
            int yzb = Request["yzb"].GetSafeInt(1);
            bool fbxm = Request["fbxm"].GetSafeBool(false);
            string limitxmbh = Request["limitxmbh"].GetSafeString();

            try
            {
                string where = " and a.xmlx<>'3' ";
                if (yx == 1)
                    where += " and a.sfyx=1 ";
                //if (yzb == 1)
                //    where += " and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh ) ";
                if (fbxm)
                    where += " and a.yxfb=1 ";

                if (limitxmbh != "" && !limitxmbh.Equals("all", StringComparison.OrdinalIgnoreCase))
                    where += " and a.syxmbh in (" + limitxmbh.FormatSQLInStr() + ") ";

                string sql = "select a.xsflbh,a.syxmbh,a.syxmmc,a.sfyx,a.recid,a.wtdlrbj,a.xmlx,(select top 1 b.xmdh from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdh,(select count(*) from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdhsl from pr_m_syxm a where a.ssdwbh='' " + where + "  order by a.xsflbh,a.xssx";

                IList<IDictionary<string, string>> xms = CommonService.GetDataTable(sql);
                foreach (IDictionary<string, string> row in xms)
                {
                    int xmdhsl = row["xmdhsl"].GetSafeInt();
                    if (xmdhsl == 0)
                        row["xmdh"] = row["syxmbh"];
                    row.Remove("xmdhsl");
                }
                if (xms.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(xms);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        } 
        #endregion

        #region 无二维码申请审核
        [Authorize]
        public ActionResult NoQrCodeAudit(string data)
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                code = JcService.NoQrCodeAudit(data, CurrentUser.UserName, CurrentUser.RealName, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion 

        #region 获取非监督工程检测审核项目
        /// <summary>
        /// 获取非监督工程检测审核项目分组
        /// </summary>
        [Authorize]
        public void GetFjdGcAuditXmfz()
        {
            string ret = "[]";
            string zjzbh = Request["zjzbh"].GetSafeString();

            try
            {
                if (!string.IsNullOrEmpty(zjzbh))
                {
                    IList<IDictionary<string, string>> xmfls = CommonService.GetDataTable
                        (string.Format(@"select a.sjxsflbh,a.xsflbh,a.xsflmc
                                           from pr_m_syxmxsfl a
                                          inner join h_zjz b on b.zjzbh = '{0}'
                                          where a.ssdwbh=''
                                            and (b.FJDGCAudit like '%' + a.xsflbh + '%' or a.SJXSFLBH > '')
                                        order by a.xssx", zjzbh));

                    if (xmfls.Count > 0)
                        ret = new JavaScriptSerializer().Serialize(xmfls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }

        /// <summary>
        /// 获取非监督工程检测审核项目
        /// </summary>
        [Authorize]
        public void GetFjdGcAuditXm()
        {
            string ret = "[]";

            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string zjzbh = Request["zjzbh"].GetSafeString();
                string sql = string.Format(@"SELECT a.xsflbh,a.syxmbh,a.syxmmc,a.recid,a.wtdlrbj,a.xmlx,isnull(b.IsAudit, 0) sfyx
                                               FROM pr_m_syxm a
                                          LEFT JOIN I_S_FJDGC_Audit b on a.syxmbh = b.syxmbh and b.gcbh = '{0}' and b.zjzbh = '{1}'
                                            WHERE a.ssdwbh=''
                                              AND a.xmlx<>'3'
                                            ORDER BY  a.xsflbh,a.xssx", gcbh, zjzbh);
                IList<IDictionary<string, string>> xms = CommonService.GetDataTable(sql);
                if (xms.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(xms);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }

        /// <summary>
        /// 设置非监督工程检测审核项目
        /// </summary>
        [Authorize]
        public void SetFjdGcAuditXm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string zjzbh = Request["zjzbh"].GetSafeString();
                var dict = new Dictionary<string, int>();
                string ids = Request["ids"].GetSafeString();
                string[] arr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    string[] itm = str.Split(new char[] { ':' });
                    if (itm.Length < 2)
                        continue;

                    if (!dict.ContainsKey(itm[0].GetSafeString()))
                        dict.Add(itm[0].GetSafeString(), itm[1].GetSafeInt());
                }
                code = JcService.SetFjdGcAuditXm(gcbh, zjzbh, dict, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        [Authorize]
        public ActionResult JudgeFjdGcAuditXm()
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                string dwbh = Request["dwbh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string zjzbh = Request["zjzbh"].GetSafeString();
                string syxmbh = Request["syxmbh"].GetSafeString();

                code = JcService.JudgeFjdGcAuditXm(dwbh, gcbh, zjzbh, syxmbh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }

        #endregion

        #region 判断温州监管建研新见证流程是否启用
        public ActionResult GetWzJgJyNewJzqy()
        {
            bool code = false;
            string msg = string.Empty;

            if (JcService.GetSysWzJgJyNewJzqy() == (int)SysWzJgJyNewJzqyEnum.Enabled)
            {
                code = true;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion 

        #region 判断委托单修改申请
        /// <summary>
        /// 判断委托单修改申请
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="applyreason"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult JudgeWtdModifyApply(string wtdwyh)
        {
            bool code = true;
            string msg = string.Empty;
            string data = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(wtdwyh))
                {
                    code = false;
                    msg = "传入的委托单唯一号不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                var result = JcService.JudgeWtdModifyApply(wtdwyh);
                code = result.success;
                msg = result.msg;
                data = result.data as string;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }
        #endregion

        #region 保存委托单修改申请
        /// <summary>
        /// 保存委托单修改申请
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="applyreason"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SaveWtdModifyApply(string wtdwyh, string applyReason)
        {
            bool code = true;
            string msg = string.Empty;
            string data = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(wtdwyh))
                {
                    code = false;
                    msg = "传入的委托单唯一号不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                if (string.IsNullOrEmpty(applyReason))
                {
                    code = false;
                    msg = "申请原因不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                var result = JcService.SaveWtdModifyApply(wtdwyh, applyReason, CurrentUser.UserCode, CurrentUser.RealName);
                code = result.success;
                msg = result.msg;
                data = result.data as string;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }
        #endregion

        #region 委托单修改申请显示
        /// <summary>
        /// 委托单修改申请显示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewWtdModifyApply()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取修改申请
            ResultParam ret = JcService.GetViewWtdModifyApply(wtdwyh);
            ViewBag.WtdModifyApplys = ret.data;
            return View();
        }

        [Authorize]
        public ActionResult ViewWtdModifyDetail()
        {
            //修改申请号
            string applyId = Request["applyid"].GetSafeRequest();
            //获取修改申请
            ResultParam ret = JcService.GetViewWtdModifyDetail(applyId);

            var data = ret.data as Dictionary<string, object>;
            var mdata = data["mdata"] as List<IDictionary<string, string>>;
            var smdata = data["smdata"] as List<IDictionary<string, string>>;
            var sdeletedata = data["sdeletedata"] as List<IDictionary<string, string>>;
            var sadddata = data["sadddata"] as List<IDictionary<string, string>>;
            var supdatedata = data["supdatedata"] as List<IDictionary<string, string>>;

            ViewBag.mdata = mdata;
            ViewBag.smdata = smdata;
            ViewBag.sdeletedata = sdeletedata;
            ViewBag.sadddata = sadddata;
            ViewBag.supdatedata = supdatedata;

            return View();
        } 
        #endregion

        #region 检测轨迹
        /// <summary>
        /// 检测轨迹页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Jcgj()
        {
            ViewBag.wtdwyh = Request["wtdwyh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 获取检测轨迹数据
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJcgjs(string wtdwyh)
        {
            bool code = true;
            string msg = string.Empty;
            string wtdbh = "";
            string syxmmc = "";
            string gcmc = "";
            string khdwmc = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (string.IsNullOrEmpty(wtdwyh))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                datas = CommonService.GetDataTable
                        (string.Format(@"select * from view_up_jcgj where WTDBH='{0}' order by FSSJ desc", wtdwyh));

                IList<IDictionary<string, string>> wtdDatas = new List<IDictionary<string, string>>();
                wtdDatas = CommonService.GetDataTable
                    (string.Format(@"select khdwmc,gcmc,syxmmc,wtdbh from m_by where recid='{0}'", wtdwyh));

                if (wtdDatas.Count > 0)
                {
                    wtdbh = wtdDatas[0]["wtdbh"];
                    syxmmc = wtdDatas[0]["syxmmc"];
                    gcmc = wtdDatas[0]["gcmc"];
                    khdwmc = wtdDatas[0]["khdwmc"];
                }

                foreach (IDictionary<string, string> row in datas)
                {
                    string fssj = row["fssj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
                    string zt = row["fszt"].GetSafeString();
                    string sy_zt = "";
                    switch (zt)
                    {
                        case "10":
                            sy_zt = "委托";
                            break;
                        case "11":
                            sy_zt = "见证";
                            break;

                        case "12":
                            sy_zt = "收样";
                            break;
                        case "20":
                            sy_zt = "试验";
                            break;
                        case "21":
                            sy_zt = "试验";
                            break;
                        case "30":
                            sy_zt = "报告审核";
                            break;
                        case "31":
                            sy_zt = "报告签发";
                            break;
                        default:
                            sy_zt = "zt";
                            break;
                    }
                    row["mark"] = "[" + fssj + "][" + sy_zt + "]" + row["fsryxm"] + " " + row["fsdd"];
                }
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, records = datas ,wtdbh=wtdbh, syxmmc=syxmmc,gcmc=gcmc,khdwmc=khdwmc});
        }

        #endregion

        #region 数据展现 现场项目
        [Authorize]
        public ActionResult Xcxmzs()
        {
            return View();
        }

        [Authorize]
        public ActionResult Xcxmzslb()
        {
            ViewBag.Syxmbhs = Request["syxmbhs"].GetSafeString();
            //默认分页20
            ViewBag.Rows = Request["rows"].GetSafeInt(20);
            //默认第一页
            ViewBag.Page = Request["page"].GetSafeInt(1);

            return View();
        }

        [Authorize]
        public ActionResult GetXcxmlb()
        {
            bool code = true;
            string msg = string.Empty;
            Dictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                var records = JcService.GetXcxmlb();
                data.Add("records", records);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg,  data}, JsonRequestBehavior.AllowGet);
        }

        //获取现场项目数据
        [Authorize]
        public ActionResult GetXcxmData()
        {
            bool code = true;
            string msg = string.Empty;
            Dictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                var syxmbhs = Request["syxmbhs"].GetSafeString();
                //默认分页20
                var pageSize = Request["rows"].GetSafeInt(20);
                //默认第一页
                var pageIndex = Request["page"].GetSafeInt(1);

                if (string.IsNullOrEmpty(syxmbhs))
                {
                    code = false;
                    msg = "试验项目编号不能为空";
                    return Json(new { code = code ? "0" : "1", msg, data }, JsonRequestBehavior.AllowGet);
                }

                int totalCount = 0;
                var records = JcService.GetXcxmData(syxmbhs, pageSize, pageIndex, out totalCount);

                data.Add("total", totalCount);
                data.Add("rows", records);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 设置建研委托清单传递参数
        [Authorize]
        public ActionResult SetJyWtqd()
        {
            bool code = true;
            string msg = string.Empty;
            string data = string.Empty;

            try
            {
                string wtqdContent = Request["wtqdcontent"].GetSafeString();

                if (string.IsNullOrEmpty(wtqdContent))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                var result = JcService.InsertJyWtqd(wtqdContent);
                msg = result.msg;
                data = result.data as string;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        } 
        #endregion

        #region 获取建研委托清单传递参数
        [Authorize]
        public ActionResult GetJyWtqd()
        {
            bool code = true;
            string msg = string.Empty;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            try
            {
                string recid = Request["jyWtqdId"].GetSafeString();

                if (string.IsNullOrEmpty(recid))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg, data });
                }

                var wtqdContent = JcService.GetJyWtqd(recid);
                var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(wtqdContent);
                Dictionary<string, object> dict = new Dictionary<string, object>();

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        dict = new Dictionary<string, object>();
                        dict.Add("targetlist", JsonSerializer.Deserialize<List<Dictionary<string, string>>>(item["targetlist"].GetSafeString()));
                        dict.Add("readonlyctrls", JsonSerializer.Deserialize<List<string>>(item["readonlyctrls"].GetSafeString()));
                        data.Add(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }  
        #endregion

        #region 判断温州监管建研新见证流程是否启用，是否存在温州建研见证取样记录
        public ActionResult ExistJyJzqy(string wtdwyh)
        {
            bool code = true;
            string msg = string.Empty;
            //是否启用
            int enabled = 0;
            //是否存在见证取样记录
            int exist = 0;

            try
            {
                if (JcService.GetSysWzJgJyNewJzqy() != (int)SysWzJgJyNewJzqyEnum.Enabled)
                {
                    code = true;
                    return Json(new { code = code ? "0" : "1", msg, enabled, exist });
                }

                enabled = 1;

                if (string.IsNullOrEmpty(wtdwyh))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg, enabled, exist});
                }

                string sql = string.Format("select count(1) from s_by where byzbrecid='{0}' and jzrecid > ''", wtdwyh);
                var count = CommonService.GetSingleData(sql).GetSafeInt();
                if (count > 0)
                {
                    exist = 1;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, enabled, exist });
        } 
        #endregion  

        #region 获取建研见证取样readonly配置信息
        [Authorize]
        public ActionResult GetJyJzqyReadonly(string syxmbh)
        {
            bool code = true;
            string msg = string.Empty;
            List<string> data = new List<string>();

            try
            {
                if (JcService.GetSysWzJgJyNewJzqy() != (int)SysWzJgJyNewJzqyEnum.Enabled)
                {
                    code = true;
                    return Json(new { code = code ? "0" : "1", msg, data });
                }

                if (string.IsNullOrEmpty(syxmbh))
                {
                    code = false;
                    msg = "传入参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg, data });
                }

                var helpLinks = JcService.GetJyJzqyHelpLink(syxmbh);
                string helpLink = GetHelpLinkValue(helpLinks, syxmbh, "helplnk");
                var helpLinkDict = HelpLinkHelper.HandleHelpLink(helpLink);
                data = HelpLinkHelper.GetHelpLinkValue(helpLinkDict, "readonlyctrl");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }
        #endregion

        #region 判断是否一个见证人员只允许添加一个工程
        [Authorize]
        public ActionResult GetSysJzryZyxOneGc()
        {
            bool code = false;
            string msg = string.Empty;

            if (JcService.GetSysJzryZyxOneGc() == (int)SysWzJgJyNewJzqyEnum.Enabled)
            {
                code = true;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion 

        #region 人员录用申请审核
        [Authorize]
        public ActionResult RylySqSh()
        {
            bool code = false;
            string msg = string.Empty;

            try
            {
                string recids = Request["recids"].GetSafeString();

                if (string.IsNullOrEmpty(recids))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                var result = JcService.RylySqSh(recids, CurrentUser.UserCode, CurrentUser.RealName);
                code = result.success;
                msg = result.msg;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion 

        #region 判断是否是现场项目
        [Authorize]
        public ActionResult JudgeXcxm()
        {
            bool code = false;
            string msg = string.Empty;
            int data = 0;

            try
            {
                string syxmbh = Request["syxmbh"].GetSafeRequest();

                if (string.IsNullOrEmpty(syxmbh))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg, data });
                }

                string sql = string.Format("select IsNull(xcxm,0) from pr_m_syxm where ssdwbh = '' and syxmbh = '{0}'", syxmbh);
                data = CommonService.GetSingleData(sql).GetSafeInt();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg, data });
        }
        #endregion 

        #region 修改温州建研见证取样配置记录
        [Authorize]
        public ActionResult EditJyJzqyHelpLink()
        {
            try
            {
                string msg = string.Empty;
                string recid = Request["recid"].GetSafeRequest();
                ViewBag.recid = recid;

                string helpLink = CommonService.GetSingleData(string.Format("select helplnk from DATAZDZD_INDIVIDUALPROJECT where sjgj_id = '{0}'", recid)).GetSafeString();

                //处理helpLink,返回fieldnames,fieldmcs,targetfields,targetctrls,readonlyctrls
                var helpLinkDict = HelpLinkHelper.HandleHelpLink(helpLink);

                //字段显示
                string fieldname = string.Empty;
                string fieldmc = string.Empty;
                string targetfield = string.Empty;
                string targetctrl = string.Empty;
                string readonlyctrl = string.Empty;

                if (helpLink != null)
                {
                    fieldname = DictionaryHelper.GetValue(helpLinkDict,"fieldname");
                    fieldmc = DictionaryHelper.GetValue(helpLinkDict,"fieldmc");
                    targetfield = DictionaryHelper.GetValue(helpLinkDict,"targetfield");
                    targetctrl = DictionaryHelper.GetValue(helpLinkDict,"targetctrl");
                    readonlyctrl = DictionaryHelper.GetValue(helpLinkDict,"readonlyctrl");
                }

                ViewBag.fieldname = fieldname;
                ViewBag.fieldmc = fieldmc;
                ViewBag.targetfield = targetfield;
                ViewBag.targetctrl = targetctrl;
                ViewBag.readonlyctrl = readonlyctrl;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return View();
        }

        [Authorize]
        public ActionResult SaveEditJyJzqyHelpLink()
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                string recid = Request["recid"].GetSafeString();
                string fieldname = Request["fieldname"].GetSafeString();
                string fieldmc = Request["fieldmc"].GetSafeString();
                string targetfield = Request["targetfield"].GetSafeString();
                string targetctrl = Request["targetctrl"].GetSafeString();
                string readonlyctrl = Request["readonlyctrl"].GetSafeString();

                if (string.IsNullOrEmpty(recid) || string.IsNullOrEmpty(fieldname) || string.IsNullOrEmpty(fieldmc)
                   || string.IsNullOrEmpty(targetfield) || string.IsNullOrEmpty(targetctrl) || string.IsNullOrEmpty(readonlyctrl))
                {
                    code = false;
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                if (fieldname.Split(',').Count() != fieldmc.Split(',').Count())
                {
                    code = false;
                    msg = "显示字段和显示字段名称对应字段数量不匹配";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                if (targetfield.Split(',').Count() != targetctrl.Split(',').Count())
                {
                    code = false;
                    msg = "赋值字段和赋值字段控件对应字段数量不匹配";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                string helpLink = CommonService.GetSingleData(string.Format("select helplnk from DATAZDZD_INDIVIDUALPROJECT where sjgj_id = '{0}'", recid)).GetSafeString();
                var helpLinkDict = HelpLinkHelper.HandleHelpLink(helpLink);

                var url = DictionaryHelper.GetValue(helpLinkDict, "url");
                var whereCtrlCustom = DictionaryHelper.GetValue(helpLinkDict, "wherectrl_custom");
                var whereField = DictionaryHelper.GetValue(helpLinkDict, "wherefield");
                var js = DictionaryHelper.GetValue(helpLinkDict, "js");
                var checkfun = DictionaryHelper.GetValue(helpLinkDict, "checkfun");

                string newhelpLink = string.Format("helplink--url-{0}|fieldname-{1}|fieldmc-{2}|targetfield-{3}|targetctrl-{4}|wherectrl_custom-{5}|wherefield-{6}|readonlyctrl-{7}|js-{8}|checkfun-{9}|datatype-serviceRow",
                    url, fieldname, fieldmc, targetfield, targetctrl, whereCtrlCustom, whereField, readonlyctrl, js, checkfun);

                string sql = string.Format("update DATAZDZD_INDIVIDUALPROJECT set helplnk = '{0}' where sjgj_id = '{1}'", newhelpLink, recid);
                CommonService.ExecSql(sql, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }

        [Authorize]
        public ActionResult InitJyJzqyHelpLink()
        {
            bool code = true;
            string msg = string.Empty;

            try
            {
                var url = "http://wzjcjg.jzyglxt.com/JC/JzqyUpLoadDataListPage";
                var ret = JcService.InitJyJzqyHelpLink(url);
                code = ret.success;
                msg = ret.msg;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }  
        #endregion

        #region 设置用户单位
        [Authorize]
        public ActionResult SetUserDwbh()
        {
            var code = true;
            var msg = string.Empty;

            try
            {
                var dwbh = Request["dwbh"].GetSafeRequest();
                CurrentUser.Wtdytqy = dwbh;
                SystemService.SetUserSetting(CurrentUser.UserName, UserSettingItem.LastWtdw, dwbh);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        } 
        #endregion

        #region 获取现场项目
        /// <summary>
        /// 获取分页的试验列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetXcjcSyPageList()
        {
            var ret = new ResultParam
            {
                success = false,
                data = new
                {
                    total = 0,
                    rows = new List<Dictionary<string, string>>()
                }
            };

            try
            {
                string msg = string.Empty;
                var page = Request["page"].GetSafeInt(1);
                var pageSize = Request["pageSize"].GetSafeInt(20);

                //过滤条件
                var filterRules = Request["filterRules"].GetSafeString();
                var filters = new List<Dictionary<string, string>>();

                if (!string.IsNullOrEmpty(filterRules))
                    filters = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(filterRules);

                string ptbh = string.Empty;
                string zh = string.Empty;

                foreach (var filter in filters)
                {
                    var fieldName = filter["fieldname"];
                    var fieldValue = filter["fieldvalue"];

                    if (fieldName == "PTBH")
                        ptbh = fieldValue;
                    else if (fieldName == "ZH")
                        zh = fieldValue;
                }

                var url = JcService.GetXcjcUrl();

                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("key", Configs.XcjcKey);
                queryParams.Add("pagesize", pageSize.GetSafeString());
                queryParams.Add("pageindex", page.GetSafeString());
                queryParams.Add("busynessid", "");
                queryParams.Add("isvalid", "0");
                queryParams.Add("ptbh", ptbh);
                queryParams.Add("zh", zh);

                var resp = new XcjcRespSyPageList() {
                    issuccess = false,
                    message = "",
                    totalcount = 0,
                    records = new List<Dictionary<string,string>>()
                };

                var code = MyHttp.Post(url + "/xcjc/GetPageSyList", queryParams, out msg);

                if (code)
                {
                    resp = JsonSerializer.Deserialize<XcjcRespSyPageList>(msg);
                }

                var syids = resp.records.Select(x => x["syid"]).ToList();

                //申请审核标记
                var dts = JcService.GetXcjcModifyApply(string.Join(",", syids));
                string status = "未申请";
               
                foreach(var record in resp.records)
                {
                    var dt = dts.FirstOrDefault(x => x["busynessid"] == record["busynessid"] && x["syid"] == record["syid"]);

                    if (dt != null)
                    {
                        if (dt["audit"].GetSafeInt() == 1)
                            status = "审批通过";
                        else
                            status = "已申请";
                    }
                    else {
                        status = "未申请";
                    }

                    record.Add("status", status);
                }

                ret.success = resp.issuccess;
                ret.msg = resp.message;
                ret.data = new
                {
                    total = resp.totalcount,
                    rows = resp.records
                };
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return Json(ret);
        }

        public ActionResult SetXcjcSyPtbh()
        {
            bool code = false;
            string msg = string.Empty;

            try
            {
                string busynessid = Request["busynessid"].GetSafeString();
                string syid = Request["syid"].GetSafeString();
                var result = JcService.GetXcjcModifyContent(busynessid, syid);
                string applyId = string.Empty;
                string ptbh = string.Empty;
                string zh = string.Empty;

                if (result.success) {
                    var data = result.data as Dictionary<string, string>;
                    applyId = data["applyid"];
                    ptbh = data["ptbh"];
                    zh = data["zh"];
                }
                else
                {
                    return Json(new { code = code ? "0" : "1", msg = result.msg });
                }

                var url = JcService.GetXcjcUrl();

                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("key", Configs.XcjcKey);
                queryParams.Add("busynessid", busynessid);
                queryParams.Add("syid", syid);
                queryParams.Add("ptbh", ptbh);
                queryParams.Add("zh", zh);

                var resp = new XcjcRespSyPath()
                {
                    issuccess = false,
                    message = ""
                };

                var ret = MyHttp.Post(url + "/xcjc/SetSyPtbh", queryParams, out msg);

                if (ret)
                {
                    resp = JsonSerializer.Deserialize<XcjcRespSyPath>(msg);

                    if (resp.issuccess)
                    {
                        //更新审核标记
                        JcService.AuditXcjcModifyApply(applyId, CurrentUser.UserCode, CurrentUser.RealName, 1);
                    }

                    code = resp.issuccess;
                    msg = resp.message;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 现场检测修改申请
        /// </summary>
        /// <returns></returns>
        public ActionResult XcjcModifyApply()
        {
            ViewBag.BusynessId = Request["busynessid"].GetSafeString();
            ViewBag.SyId = Request["syid"].GetSafeString();
            ViewBag.OldPtbh = Request["ptbh"].GetSafeString();
            ViewBag.OldZh = Request["zh"].GetSafeString();
            return View();
        }

        public ActionResult XcjcModifyContent()
        {
            var busynessid = Request["busynessid"].GetSafeString();
            var syid = Request["syid"].GetSafeString();

            var ret = JcService.GetAllXcjcModifyContent(busynessid, syid);
            ViewBag.XcjcModifyContent = ret.data as List<Dictionary<string, string>>;

            return View();
        }

        public ActionResult SaveXcjcModifyApply()
        {
            bool code = true;
            string msg = "";
            try
            {
                string busynessid = Request["busynessid"].GetSafeString();
                string syid = Request["syid"].GetSafeString();
                string newptbh = Request["newptbh"].GetSafeString();
                string newzh = Request["newzh"].GetSafeString();
                string oldptbh = Request["oldptbh"].GetSafeString();
                string oldzh = Request["oldzh"].GetSafeString();
                string applyReason = Request["applyreason"].GetSafeString();

                var res = JcService.SaveXcjcModifyApply(busynessid, syid, newptbh, newzh, oldptbh, oldzh, CurrentUser.UserCode, CurrentUser.RealName, applyReason);
                code = res.success;
                msg = res.msg;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region 校验检测机构区域内外是否已经审批
        [Authorize]
        public ActionResult CheckJcjgQySp()
        {
            var code = true;
            var msg = string.Empty;

            try
            {
                var dwbh = Request["dwbh"].GetSafeRequest();
                var htbh = Request["htbh"].GetSafeRequest();
                var ret = JcService.CheckJcjgQySp(dwbh, htbh);
                code = ret.success;
                msg = ret.msg;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }
        #endregion

        #region 检测机构区域内审批
        [Authorize]
        public ActionResult JcJgQynSp()
        {
            var code = true;
            var msg = string.Empty;

            try
            {
                var recids = Request["recids"].GetSafeRequest();
                var expiryDate = DateTime.Now.AddYears(1).ToString();
                var ret = JcService.JcJgQynSp(recids, expiryDate, CurrentUser.UserCode, CurrentUser.RealName);
                code = ret.success;
                msg = ret.msg;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        } 
        #endregion

        #region 采集软件接口
        /// <summary>
        /// 上传试验原始数据
        /// </summary>
        /// <param name="dwbh">单位编号</param>
        /// <param name="wtdwyh">委托单唯一</param>
        /// <param name="syxmmc">试验项目名称</param>
        /// <param name="ypbh">样品编号</param>
        /// <param name="zh">获取委托单从表里的组号（报告数据对应）</param>
        /// <param name="syr">试验人</param>
        /// <param name="sysb">试验设备</param>
        /// <param name="sykssj">试验开始时间</param>
        /// <param name="syjssj">试验结束时间</param>
        /// <param name="syqx">采集曲线jpg格式再base64编码</param>
        /// <param name="videofiles">视频文件名（多个逗号分隔）</param>
        /// <param name="recordfiles">录屏文件名（多个逗号分隔</param>
        /// <param name="datajson">采集数据（单条）</param>
        /// <param name="czdatajson">重做记录json</param>
        /// <param name="sfbc">试验是否保存(1保存,其他都是没保存)</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UpData(string dwbh, string wtdwyh, string syxmmc, string ypbh, string zh, string syr, string sysb,
            string sykssj, string syjssj, string syqx, string videofiles, string recordfiles,
            string datajson, string czdatajson, string sfbc, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                syxmmc = syxmmc.GetSafeRequest();
                ypbh = ypbh.GetSafeRequest();
                zh = zh.GetSafeRequest();
                syr = syr.GetSafeRequest();
                sysb = sysb.GetSafeRequest();


                bool bsfbc = (sfbc == "1" || sfbc.ToLower() == "true") ? true : false;

                DateTime dtsykssj = sykssj.GetSafeDate(DateTime.MinValue);
                DateTime dtsyjssj = syjssj.GetSafeDate(DateTime.MinValue);

                if (dtsykssj.Year == DateTime.MinValue.Year)
                {
                    msg = "试验开始时间无效";
                }
                else if (dtsyjssj.Year == DateTime.MinValue.Year)
                {
                    msg = "试验结束时间无效";

                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                            code = JcService.UpData(dwbh, wtdwyh, syxmmc, ypbh, zh, syr, sysb, dtsykssj, dtsyjssj, syqx, videofiles, recordfiles, datajson, czdatajson, bsfbc, out msg);
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                if (!code)
                {
                    SysLog4.WriteError("上传试验数据失败，单位编号：" + dwbh + "，委托单唯一号：" + wtdwyh + ",样品编号：" + ypbh + ",组号：" + zh + ",试验曲线：" + (syqx.Length > 0 ? "不为空" : "为空"));
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return Content(ret);
        }

        /// <summary>
        /// 采集系统用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CheckCjxtUser()
        {
            ResultParam ret = new ResultParam();
            try
            {
                //时间戳
                string timestring = Request.Form["timestring"].GetSafeString();
                //检验码
                string sign = Request.Form["sign"].GetSafeString();
                //数据包
                string data = Request.Form["data"].GetSafeString();
                //判断数据包
                if (data == "")
                {
                    ret.msg = "数据不能为空！";
                    return GetContent(ret);
                }
                //判断有效性
                ret = CheckTimeSign(timestring, sign);
                //检验有效
                if (!ret.success)
                {
                    return GetContent(ret);
                }
                //判断数据包
                ret = JcService.CjxtCheckUser(data);
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("调用JcController函数CheckCjxtUser出错，原因：{0}", ex.Message));
                ret.msg = ex.Message;
            }
            return GetContent(ret);
        }


        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetCjxtUserInfo()
        {
            ResultParam ret = new ResultParam();
            try
            {
                //时间戳
                string timestring = Request.Form["timestring"].GetSafeString();
                //检验码
                string sign = Request.Form["sign"].GetSafeString();
                //数据包
                string data = Request.Form["data"].GetSafeString();
                //判断数据包
                if (data == "")
                {
                    ret.msg = "数据不能为空！";
                    return GetContent(ret);
                }
                //判断有效性
                ret = CheckTimeSign(timestring, sign);
                //检验有效
                if (!ret.success)
                {
                    return GetContent(ret);
                }
                //判断数据包
                ret = JcService.CjxtGetUserInfo(data);
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("调用JcController函数GetCjxtUserInfo出错，原因：{0}", ex.Message));
                ret.msg = ex.Message;
            }
            return GetContent(ret);
        }

        /// <summary>
        /// 根据用户名获取权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetCjxtUserPower()
        {
            ResultParam ret = new ResultParam();
            try
            {
                //时间戳
                string timestring = Request.Form["timestring"].GetSafeString();
                //检验码
                string sign = Request.Form["sign"].GetSafeString();
                //数据包
                string data = Request.Form["data"].GetSafeString();
                //判断数据包
                if (data == "")
                {
                    ret.msg = "数据不能为空！";
                    return GetContent(ret);
                }
                //判断有效性
                ret = CheckTimeSign(timestring, sign);
                //检验有效
                if (!ret.success)
                {
                    return GetContent(ret);
                }
                //判断数据包
                ret = JcService.CjxtUserPower(data);
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("调用JcController函数GetCjxtUserPower出错，原因：{0}", ex.Message));
                ret.msg = ex.Message;
            }
            return GetContent(ret);
        }
        #endregion

        #region 现场检测分析
        /// <summary>
        /// 现场检测异常分析
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Xcjckyfx(string id)
        {
            //获取时间KEY
            string timestamp = TimeUtil.GetTimeStamp();
            string secretkey = MD5Util.StringToMD5Hash("null" + timestamp);

            string url = String.Format("{0}query/jykeyipilesinfo.aspx?serialno=null&timestamp={1}&secretkey={2}", GlobalVariable.GetSysSettingValue(true, "OTHER_SETTING_WHYH"), timestamp, secretkey);
            return new RedirectResult(url);
        }

        #endregion

        #region APP

        #region APP现场检测功能
        /// <summary>
        /// App现场检测地图展示
        /// </summary>
        /// <returns></returns>
        public ActionResult MapXcjcApp()
        {
            //用户APP端openid
            string openid = Request["openid"].GetSafeString();
            //判断用户是不在
            bool ret = false;
            string msg = "";
            SysSession session = ApiSessionService.GetSessionUser(openid, out msg);
            //判断APP用户是否已经登录
            if (session != null)
            {
                string appMapStatus = Session["AppMapStatus"].GetSafeString();
                //重新登录
                if (appMapStatus != "1")
                {
                    string realname = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qymc from I_M_QY where ZH='" + session.UserName + "'");
                    if (dt.Count > 0)
                    {
                        realname = dt[0]["qymc"];
                    }

                    string err = "";
                    ret = Remote.UserService.LoginWithOutPassWord(session.UserName, "A01", out err);
                    // 登录成功
                    if (ret)
                    {
                        // 设置日志系统用户
                        BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                        // 设置流程模块用户
                        BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                            new WorkFlow.Common.SessionUser()
                            {
                                CompanyId = CurrentUser.CurUser.CompanyId,
                                CompanyName = CurrentUser.CurUser.CompanyName,
                                DepartmentId = CurrentUser.CurUser.DepartmentId,
                                DepartmentName = CurrentUser.CurUser.DepartmentName,
                                DutyLevel = CurrentUser.CurUser.DutyLevel,
                                RealName = CurrentUser.CurUser.RealName,
                                UserName = CurrentUser.CurUser.UserName
                            }, null);
                        Session["UserPowerList"] = null;
                        // 设置录入界面用户
                        Session["USERCODE"] = CurrentUser.UserCode;
                        Session["USERNAME"] = CurrentUser.UserName;
                        Session["REALNAME"] = CurrentUser.RealName;
                        Session["CPCODE"] = CurrentUser.CompanyCode;
                        Session["CPNAME"] = CurrentUser.CompanyName;
                        Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                        Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                        Session["MANAGEDEP"] = CurrentUser.CurUser.ManageDep;
                        Session["SJHM"] = SystemService.GetUserMobile(CurrentUser.UserCode);
                        // 企业及个人用户所属企业编号
                        Session["USERQYBH"] = JcService.GetQybh(CurrentUser.UserCode);
                        // 登录的账号
                        Session["USERBH"] = JcService.GetUserbh(CurrentUser.UserCode);
                        //Session["MenuCode"] = "QYGL_QYBA";
                        //设置当前登录劳资账号所在工程的jdzch
                        //SetJDZCH(CurrentUser.UserName);
                        CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                        // 设置用户桌面项
                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                        if (!status)
                            SysLog4.WriteLog(err);

                        // 获取页面跳转类型
                        dt = CommonService.GetDataTable("select zhlx,qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");

                        var qyzhbh = string.Empty;

                        if (dt.Count > 0)
                        {
                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                            qyzhbh = dt[0]["qybh"];
                            //SetCurQybh(dt[0]["zhlx"], dt[0]["qybh"]);
                            //判断是否为企业账号
                            if (dt[0]["zhlx"].GetSafeString().ToUpper() == "Q")
                            {
                                //企业类型编号
                                Session["LXBH"] = JcService.GetLxbh(qyzhbh);
                            }
                        }
                        else
                            CurrentUser.CurUser.UrlJumpType = "SYS";

                        //账号对应的业务系统编号
                        Session["QYZHBH"] = qyzhbh;
                        dt = CommonService.GetDataTable("select qymc from I_M_QY where qybh = '" + Session["USERQYBH"] + "' ");
                        if (dt.Count > 0)
                            Session["USERQYMC"] = dt[0]["qymc"].GetSafeString();
                        //所属质监站编号
                        dt = CommonService.GetDataTable("select top 1 zjzbh from i_m_nbry where zh='" + CurrentUser.RealUserName + "'");
                        if (dt.Count > 0)
                            Session["ZJZBH"] = dt[0]["zjzbh"];
                        else
                            Session["ZJZBH"] = "";

                        //用户所属角色
                        Session["ROLES"] = UserService.GetRoles(Configs.AppId, CurrentUser.RealUserName);
                    }

                    // 记录登陆日志
                    BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                    {
                        ClientType = LogConst.ClientType,
                        Ip = ClientInfo.Ip,
                        LogTime = DateTime.Now,
                        ModuleName = LogConst.ModuleUser,
                        Operation = LogConst.UserOpLogin,
                        UserName = CurrentUser.UserName,
                        RealName = ret ? CurrentUser.RealName : "",
                        Remark = "",
                        Result = ret
                    };
                }
                return View();
            }
            else
            {
                return View("mapxcjcapperror");
            }
        }

        /// <summary>
        /// 查看现场检测图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctpApp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取图片数组
            ResultParam ret = JcService.XcjcGetCjtp(wtdwyh);
            ViewBag.tps = ret.data;
            return View();
        }

        /// <summary>
        /// 查看现场检测图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctpsApp()
        {
            //委托单唯一号
            ViewBag.id = Request["id"].GetSafeRequest();
            //组号
            ViewBag.zh = Request["zh"].GetSafeRequest();
            return View();
        }

        [Authorize]
        public ActionResult ViewXcjcDetailApp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeString();
            //获取委托单的所有试验数据编号
            ResultParam ret = JcService.XcjcGetCjsybhs(wtdwyh);
            IDictionary<string, string> datas = (Dictionary<string, string>)ret.data;
            ViewBag.ids = datas["base"];
            ViewBag.others = datas["other"];
            return View();
        }

        /// <summary>
        /// 查看现场检测视频
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjcspApp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取视频组
            ResultParam ret = JcService.XcjcGetCjsp(wtdwyh);
            ViewBag.sps = ret.data;
            return View();
        }

        /// <summary>
        /// 查看现场图片链
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewXcjctplApp()
        {
            //委托单唯一号
            string wtdwyh = Request["wtdwyh"].GetSafeRequest();
            //获取图片链
            ResultParam ret = JcService.XcjcGetCjtpl(wtdwyh);
            ViewBag.wtdwyh = wtdwyh;
            ViewBag.tpls = ret.data;
            return View();
        }

        /// <summary>
        /// App播放
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult playerysyapp()
        {
            try
            {
                string recid = Request["id"].GetSafeString();
                string msg = "";
                bool code = SxtptService.GetPlayUrl(recid, out msg);
                //if (code)
                ViewBag.playurl = msg;
                //else
                //    ViewBag.playurl = "";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return View();
        }
        #endregion
        #endregion

        #region 统一检验
        /// <summary>
        /// 检验上传数据包
        /// </summary>
        /// <param name="dwbh">单位编号</param>
        /// <param name="key">检验码</param>
        /// <returns></returns>
        public ResultParam CheckSign(string dwbh, string key)
        {
            ResultParam ret = new ResultParam();
            string msg = "";
            try
            {
                //判断单位编号是否为空
                if (dwbh == "")
                {
                    ret.msg = "单位编号不能为空！";
                    return ret;
                }
                //判断KEY是否为空
                if (key == "")
                {
                    ret.msg = "检验码不能为空！";
                    return ret;
                }
                //获取单位的密钥
                if (!JcService.GetJcjgmy(dwbh, out msg))
                {
                    ret.msg = "单位编号无效！";
                    return ret;
                }
                //判断检验码
                if (key != dwbh.EncodeDesJk(msg))
                {
                    ret.msg = "数据校验错误，请正确配置数据上传密钥";
                    return ret;
                }
                //返回
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 检验时间戳
        /// </summary>
        /// <param name="timestring"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public ResultParam CheckTimeSign(string timestring, string sign)
        {
            ResultParam ret = new ResultParam();
            string msg = "";
            try
            {
                //判断时间戳
                if (timestring == "")
                {
                    ret.msg = "检验参数不能为空！";
                    return ret;
                }
                //判断KEY是否为空
                if (sign == "")
                {
                    ret.msg = "检验码不能为空！";
                    return ret;
                }

                string signstr = String.Format("timestring={0}&secret={1}", timestring, Configs.CjxtSecret);
                //判断检验码是否一致
                if (MD5Util.StringToMD5Hash(signstr, false) != sign)
                {
                    ret.msg = "数据校验错误，请正确配置数据上传密钥";
                    return ret;
                }
                //返回
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 返回数据包
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private ContentResult GetContent(Object obj)
        {
            return Content(JsonSerializer.Serialize(obj));
        }
        #endregion

        #region 数据操作
        /// <summary>
        /// 数据处理包
        /// </summary>
        [Authorize]
        public ContentResult DataOpt()
        {
            ResultParam ret = new ResultParam();
            try
            {
                ret = JcService.Service();
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Content(JsonFormat.GetString(ret));
        }
        #endregion

        #region 同步给建研工程信息
        public ActionResult JySyncProjectInfo()
        {
            var dt = CommonService.GetDataTable(@"select gcbh from i_m_gc where ssjcjgbh > '' order by gcbh desc");
            var ret = new ResultParam();

            foreach (var item in dt)
            {
                var gcbh = item["gcbh"];
                var dict = new Dictionary<string, string>();
                var msg = string.Empty;
                var code = JcService.SyncGcInfo(gcbh, out dict, out msg);

                if (code)
                {
                    string projectNum = dict["projectnum"];
                    string projectName = dict["projectname"];
                    string constractionUnit = dict["constractionunit"];
                    string inspectUnit = dict["inspectunit"];
                    string slpeopleJson = dict["slpeoplejson"];
                    string spnpeopleJson = dict["spnpeoplejson"];
                    string createunit = dict["createunit"];

                    ret = JyJzqyService.SynProjectInfo(projectNum, projectName, constractionUnit, inspectUnit, slpeopleJson, spnpeopleJson, createunit);

                    if (!ret.success)
                    {
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ret.success = false;
                    ret.msg = msg;
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 分页查询建研的见证取样记录(管理员用)
        public ActionResult QueryJyJzqyJl()
        {
            var ret = new ResultParam
            {
                success = true,
                data = new
                {
                    total = 0,
                    rows = new List<Dictionary<string, string>>()
                }
            };

            try
            {
                string msg = string.Empty;
                var gcbh = Request["gcbh"].GetSafeString();
                var page = Request["page"].GetSafeInt(1);
                var pageSize = Request["pageSize"].GetSafeInt(20);

                //过滤条件
                var filterRules = Request["filterRules"].GetSafeString();
                var filters = new List<Dictionary<string, string>>();

                if (!string.IsNullOrEmpty(filterRules))
                    filters = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(filterRules);

                string gcbw = string.Empty;
                string syxmbh = string.Empty;
                string recordId = string.Empty;
                string qrCode = string.Empty;
                string startTime = "2019-01-01";
                string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                string jstartTime = string.Empty;
                string jendTime = string.Empty;
                string syyNum = string.Empty;
                string jzyNum = string.Empty;
                string syyName = string.Empty;
                string jzyName = string.Empty;
                int orderStatus = 0;
                int limit = pageSize;
                int total = 0;

                foreach (var filter in filters)
                {
                    var fieldName = filter["fieldname"];
                    var fieldValue = filter["fieldvalue"];

                    if (fieldName == "JZRECID")
                        recordId = fieldValue;
                    if (fieldName == "GCBW")
                        gcbw = fieldValue;
                    else if (fieldName == "SYXMMC")
                        syxmbh = CommonService.GetSingleData(string.Format("select syxmbh from pr_m_syxm where ssdwbh='' and syxmmc like '%{0}%'", fieldValue)).GetSafeString();
                    else if (fieldName == "QYRXM")
                        syyName = fieldValue;
                    else if (fieldName == "QYSJ")
                        startTime = fieldValue;
                    else if (fieldName == "JZRXM")
                        jzyName = fieldValue;
                    else if (fieldName == "JZSJ")
                    {
                        jstartTime = fieldValue;
                        jendTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    else if (fieldName == "GCBH")
                        gcbh = fieldValue;
                    else if (fieldName == "QRCODE")
                        qrCode = fieldValue;
                    else if (fieldName == "ISORDER")
                        orderStatus = fieldValue.GetSafeInt();
                    else if (fieldName == "QYRZH")
                        syyNum = fieldValue;
                    else if (fieldName == "JZRZH")
                        jzyNum = fieldValue;  
                }

                var jzqyRet = JyJzqyService.UpLoadDataListPage(gcbh, gcbw, syxmbh, startTime, endTime, jstartTime, jendTime, syyNum
                                , jzyNum, syyName, jzyName, recordId, qrCode, orderStatus, page, limit, out total);

                if (!jzqyRet.success)
                {
                    msg = jzqyRet.msg;
                    return Json(ret);
                }

                var rows = new List<Dictionary<string, string>>();
                var jzqyRetDatas = jzqyRet.data as List<Dictionary<string, string>>;
                var syxmbhs = jzqyRetDatas.Select(x => x["ITEMCODE"]).Distinct();
                var helpLinks = JcService.GetJyJzqyHelpLink(string.Join(",", syxmbhs));

                foreach (var jzqyRetData in jzqyRetDatas)
                {
                    var item = JsonSerializer.Deserialize<Dictionary<string, string>>(jzqyRetData["ITEMJSON"]);

                    var row = new Dictionary<string, string>();

                    row.Add("jzrecid", jzqyRetData["GUID"]);
                    row.Add("gcbh", jzqyRetData["PROJECTNUM"]);
                    row.Add("gcmc", jzqyRetData["PROJECTNAME"]);
                    row.Add("qrcode", jzqyRetData["QRINFO"]);
                    row.Add("jzrzh", jzqyRetData["SPNID"]);
                    row.Add("jzrxm", jzqyRetData["SPNNAME"]);
                    row.Add("jzsj", jzqyRetData["SPNDATE"]);
                    row.Add("isjz", !string.IsNullOrEmpty(jzqyRetData["SPNID"]) ? "是" : "否");
                    row.Add("gcbw", DictionaryHelper.GetValueIgnoreCase(item, "GCBW"));
                    row.Add("editstatus", GetEditStatusName(jzqyRetData["EDITSTATUS"]));
                    row.Add("syxmbh", jzqyRetData["ITEMCODE"]);
                    row.Add("syxmmc", GetHelpLinkValue(helpLinks, jzqyRetData["ITEMCODE"], "syxmmc"));
                    row.Add("qyrzh", jzqyRetData["SLID"]);
                    row.Add("qyrxm", jzqyRetData["SLNAME"]);
                    row.Add("qysj", jzqyRetData["SLDATE"]);
                    row.Add("isorder", jzqyRetData["ISORDER"] == "1" ? "已委托" : "未委托");

                    rows.Add(row);
                }

                ret.success = jzqyRet.success;
                ret.msg = jzqyRet.msg;
                ret.data = new
                {
                    total,
                    rows
                };
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return Json(ret);
        }
        #endregion

        #region 在监管系统中审核修改申请
        [Authorize]
        public ActionResult AuditWtdModifyApply()
        {
            bool code = false;
            string msg = string.Empty;

            try
            {
                string dwbh = Request["dwbh"].GetSafeRequest();
                string wtdwyh = Request["wtdwyh"].GetSafeRequest();
                string auditType = Request["audittype"].GetSafeRequest();

                if (string.IsNullOrEmpty(dwbh) || string.IsNullOrEmpty(wtdwyh) || string.IsNullOrEmpty(auditType))
                {
                    msg = "传入的参数不能为空";
                    return Json(new { code = code ? "0" : "1", msg });
                }

                //判断是否使用标点检测系统
                var result = JcService.JudgeBDJcxt(dwbh);

                if (!result.success)
                {
                    msg = result.msg;
                    return Json(new { code = code ? "0" : "1", msg });
                }

                //获取修改申请Id
                result = JcService.GetWtdModifyApplyId(wtdwyh);

                if (!result.success)
                {
                    msg = result.msg;
                    return Json(new { code = code ? "0" : "1", msg });
                }

                var applyId = result.data as string;

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("applyid", applyId);
                data.Add("auditusername", CurrentUser.RealName);
                data.Add("audittype", auditType);

                result = JcService.AuditWtdModifyApply(dwbh, data);

                //重新计算费用
                if (result.success)
                {
                    InvokeDllHelper.InvokeCalculate(wtdwyh);
                }

                code = result.success;
                msg = result.msg;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return Json(new { code = code ? "0" : "1", msg });
        }

        [Authorize]
        public ActionResult AuditWtdModifyApplyView()
        {
            return View();
        }
        #endregion

        #region 企业申报

        /// <summary>
        /// 根据申报唯一号获取企业申报信息
        /// </summary>
        /// <param name="sbwyh"></param>
        [Authorize]
        public void GetQysbInfo(string sbwyh)
        {
            string msg = "";
            bool code = false;
            try
            {
                var dt = CommonService.GetDataTable("select * from I_M_QYSB where SBWYH='" + sbwyh + "'");
                if (dt.Count <= 0)
                    throw new Exception("记录不存在");

                code = true;
                msg = "成功";
                Response.Write(JsonFormat.GetRetString(code, msg, dt[0]));
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);

                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        /// <summary>
        /// 删除企业申报
        /// </summary>
        /// <param name="sbwyh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult DelQysb(string sbwyh)
        {
            string code = "";
            string msg = "";

            try
            {
                var dt = CommonService.GetDataTable("select * from I_M_QYSB where SBWYH='" + sbwyh + "'");
                if (dt.Count <= 0)
                    throw new Exception("记录不存在");

                if (dt[0]["SHZT"] == "1" || dt[0]["SHZT"] == "2")
                    throw new Exception("申报已审核，不能删除");

                CommonService.Delete("I_M_QYSB", "SBWYH", sbwyh);

                code = "0";
                msg = "成功";
            }
            catch (Exception ex)
            {
                code = "-1";
                msg = ex.Message;

                SysLog4.WriteLog(ex);
            }

            return Json(new { code = code, msg = msg });
        }

        #endregion

        #region 根据试验项目获取协会需要上传字段
        public ActionResult GetXsxhUploadField()
        {
            var result = new ResultParam();

            try
            {
                string wtdwyh = Request["wtdwyh"].GetSafeRequest();
                string syxmbh = Request["syxmbh"].GetSafeRequest();

                result = JcService.XsxhUploadBgField(wtdwyh, syxmbh, 1);

                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 不合格报告的回复内容
        [Authorize]
        public ActionResult SetBhgBgHf()
        {
            var result = new ResultParam();

            try
            {
                string bgwyh = Request["bgwyh"].GetSafeRequest();
                string hfnr = Request["hfnr"].GetSafeRequest();
                // 1- 监督员回复， 2- 监理单位回复
                int hflx = Request["hflx"].GetSafeInt();

                result = JcService.InsertBhgBgHf(bgwyh, CurrentUser.UserCode, CurrentUser.RealName, hfnr, hflx);

                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ViewBhgBgHf()
        {
            var bgwyh = Request["bgwyh"].GetSafeRequest();
            var hflx = Request["hflx"].GetSafeRequest();
            var sql = string.Format("select hfnr from I_S_BG_BHGHF where bgwyh = '{0}' and hflx = '{1}'", bgwyh, hflx);

            ViewBag.bgwyh = bgwyh;
            ViewBag.hflx = hflx;
            ViewBag.hfnr = CommonService.GetSingleData(sql).GetSafeString();

            return View();
        }
        #endregion 

        #region 是否存在form配置
        public ActionResult ExistFormDm()
        {
            var ret = new ResultParam();

            try
            {
                string formDm = Request["formDm"].GetSafeRequest();
                string formStatus = Request["formStatus"].GetSafeRequest();

                if (string.IsNullOrEmpty(formDm) || string.IsNullOrEmpty(formStatus))
                {
                    ret.msg = "传入参数不能为空";
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }

                ret = JcService.ExistFormDm(formDm, formStatus);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        public ActionResult QyApplyView()
        {
            ViewBag.recid = Request["recid"].GetSafeString();
            return View();
        }

        public ActionResult QyApplyAuditView()
        {
            ViewBag.recid = Request["recid"].GetSafeString();
            return View();
        }

        public ActionResult GetQyApplyQyInfo()
        {
            var result = new ResultParam();

            try
            {
                var qybh = "Q005233";
                //var qybh = Session["QYZHBH"];
                result = JcService.GetQyApplyQyInfo(qybh);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQyApplyRyInfo()
        {
            var result = new ResultParam();

            try
            {
                var qybh = Request["qybh"].GetSafeRequest();
                var pageIndex = Request["pageindex"].GetSafeInt(1);
                var pageSize = Request["pagesize"].GetSafeInt(20);

                result = JcService.GetQyApplyRyInfo(qybh, pageIndex, pageSize);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQyApplySbInfo()
        {
            var result = new ResultParam();

            try
            {
                var qybh = Request["qybh"].GetSafeRequest();
                var pageIndex = Request["pageindex"].GetSafeInt(1);
                var pageSize = Request["pagesize"].GetSafeInt(20);

                result = JcService.GetQyApplySbInfo(qybh, pageIndex, pageSize);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQyApplyModifyInfo()
        {
            var result = new ResultParam();

            try
            {
                string recid = Request["recid"].GetSafeRequest();
                result = JcService.GetQyApplyModifyInfo(recid);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateQyApply()
        {
            var result = new ResultParam();

            try
            {
                //企业信息
                string qyJson = Request["qyjson"].GetSafeRequest();
                //人员信息
                string ryJson = Request["ryjson"].GetSafeRequest();
                //设备信息
                string sbJson = Request["sbjson"].GetSafeRequest();
                //资质信息
                string zzJson = Request["zzjson"].GetSafeRequest();
                //承诺书
                string cnsJson = Request["cnsjson"].GetSafeRequest();
                //保存类型 暂存-0, 报备-1
                string saveType = Request["savetype"].GetSafeRequest();

                result = JcService.UpdateQyApply(qyJson, ryJson, sbJson, zzJson, cnsJson, saveType, CurrentUser.UserCode, CurrentUser.RealName);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuditQyApply()
        {
            var result = new ResultParam();

            try
            {
                string recid = Request["recid"].GetSafeRequest();
                string shsm = Request["shsm"].GetSafeRequest();
                string zt = Request["zt"].GetSafeString();

                var shr = string.Empty;
                var shrxm = string.Empty;

                //审核
                result = JcService.AuditQyApply(recid, zt, CurrentUser.UserCode, CurrentUser.RealName, shsm);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelQyApply()
        {
            var result = new ResultParam();

            try
            {
                string recid = Request["recid"].GetSafeRequest();

                //删除
                result = JcService.DelQyApply(recid);
                result.code = result.success ? "0" : "1";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
