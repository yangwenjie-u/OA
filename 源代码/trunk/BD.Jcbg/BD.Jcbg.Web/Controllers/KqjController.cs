using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BD.DataInputCommon;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using CryptFun = BD.Jcbg.Common.CryptFun;
using SysLog4 = BD.Jcbg.Common.SysLog4;

namespace BD.Jcbg.Web.Controllers
{
    public class KqjController : Controller
    {

        #region 服务
        private IKqjCmdService _kqjService;
        private IKqjCmdService KqjService
        {
            get
            {
                if (_kqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _kqjService = webApplicationContext.GetObject("KqjService") as IKqjCmdService;
                }
                return _kqjService;
            }
        }
		private IWgryKqjService _wgrykqjService;
        private IWgryKqjService WgryKqjService
        {
            get
            {
                if (_wgrykqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _wgrykqjService = webApplicationContext.GetObject("WgryKqjService") as IWgryKqjService;
                }
                return _wgrykqjService;
            }
        }

		private BD.Jcbg.IBll.ICommonService _commonService = null;
		private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                try
                {
                    if (_commonService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _commonService;
            }
        }
        private ISelfService _selfService = null;
        private ISelfService SelfService
        {
            get
            {
                try
                {
                    if (_selfService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _selfService = webApplicationContext.GetObject("SelfService") as ISelfService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _selfService;
            }
        }
        #endregion

		#region 页面
        [Authorize]
		public ActionResult UserMonthLog()
		{			
			return View();
		}
        [Authorize]
        public ActionResult UserMonthLog_gc()
        {
            return View();
        }
        [Authorize]
        public ActionResult hmc_gc()
        {
            ViewData["jdzch"] = CurrentUser.Jdzch;
            return View();
        }
        [Authorize]
        public ActionResult currentworker()
        {
            string gcbh = Request["gcbh"].GetSafeString() ;
            if (gcbh=="")
                gcbh = CurrentUser.Jdzch;
            ViewData["jdzch"] = gcbh;
            ViewData["dt"] = DateTime.Now;
            return View();
        }
        [Authorize]
        public ActionResult setmonthpay_gc()
        {       
             ViewData["year"]=Request["year"].GetSafeInt();
             ViewData["month"]=Request["month"].GetSafeInt();
             ViewData["jdzch"]=Request["jdzch"].GetSafeString();
             ViewData["gz"]=Request["gz"].GetSafeString();
             ViewData["gw"] = Request["gw"].GetSafeString();
             ViewData["bzfzr"]=Request["bzfzr"].GetSafeString();
            return View();
        }
        [Authorize]
        public ActionResult UserMonthdetail()
        {
            ViewData["jdzch"] = CurrentUser.Jdzch;
            return View();
        }
        [Authorize]
        public ActionResult BankMonthPay()
        {
            ViewData["gcbh"] = Request["gcbh"].GetSafeString();
            return View();
        }
        [Authorize]
        public ActionResult usermonthpay_gc()
        {
            ViewData["jdzch"] = CurrentUser.Jdzch;
            return View();
        }

        [Authorize]
        public ActionResult tz_ygzsz_gc()
        {     
            return View();
        }
        #endregion
        #region 操作
        /// <summary>
        /// 下载考勤机模板
        /// </summary>
        [Authorize]
        public void DownKqjIris()
        {
            bool code = true;
            string msg = "";
            try
            {
                string kqjbh = Request["kqjbh"].GetSafeString();

                code = KqjService.DownKqjIris(kqjbh, out msg);

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
        /// 下发务工人员模板到考勤机
        /// </summary>
        [Authorize]
        public void DownWgryKqjIris()
        {
            bool code = true;
            string msg = "";
            try
            {
                string kqjbh = Request["kqjbh"].GetSafeString();

                code = KqjService.DownWgryKqjIris(kqjbh, out msg);

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
        /// 下载人员模板
        /// </summary>
        [Authorize]
        public void DownRyIris()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();

                code = KqjService.DownRyIris(rybh, out msg);

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
        public void DownWGRyIris()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();

                code = KqjService.DownWGRyIris(rybh, out msg);

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
        /// 初始化考勤机
        /// </summary>
        [Authorize]
        public void InitKqj()
        {
            bool code = true;
            string msg = "";
            try
            {
                string kqjbh = Request["kqjbh"].GetSafeString();

                code = KqjService.InitKqj(kqjbh, out msg);

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
        /// 重启考勤机
        /// </summary>
        [Authorize]
        public void RestartKqj()
        {
            bool code = true;
            string msg = "";
            try
            {
                string kqjbh = Request["kqjbh"].GetSafeString();

                code = KqjService.RestartKqj(kqjbh, out msg);

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
        #endregion
        #region 页面

         /// <summary>
        /// 考勤月报告统计
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult KqExport()
        {
            return View();
        }

        #endregion

        #region 统计图表

        /// <summary>
        /// 外地企业统计
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult QyKqCharts()
        {
            ViewData["KqTime"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 根据时间获取外地企业考勤统计信息
        /// </summary>
        /// <param name="datetime">考勤时间</param>
        [Authorize]
        public ActionResult GetWdqyKq(string datetime)
        {
            DateTime nowTime;
            if (!DateTime.TryParse(datetime, out nowTime))
            {
                nowTime = DateTime.Now;
            }
            string startTime = nowTime.ToString("yyyy-MM-dd") + " 00:00:00";
            string endTime = nowTime.ToString("yyyy-MM-dd") + " 23:59:59";
            var list = KqjService.GetQyKq(startTime, endTime);
            string xAxis = "", series = "";
            list.ForEach(dic =>
            {
                xAxis += "\"" + dic["qymc"] + "\",";
                if (dic["ryzs"].Equals("0"))
                {
                    series += "\"" + 0 + "\",";
                }
                else
                {
                    series += "\"" + Math.Round(Decimal.Parse(dic["rykq"]) / Decimal.Parse(dic["ryzs"]), 2) + "\",";
                }
            });
            string json = "{\"title\":\"外来企业考勤统计\",\"legend\":[\"企业人员出勤率\"],\"xAxis\":[" + xAxis.Trim(',') + "],\"series\":[" + series.Trim(',') + "]}";
            return Content(json);
        }

        public ActionResult GcKqCharts()
        {
            ViewData["KqTime"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 工程信息下拉框
        /// </summary>
        [Authorize]
        public ActionResult GetGcName()
        {
            var list = KqjService.GetGcName();
            return Content(JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 根据工程编号和考勤时间,查询企业人员考勤信息
        /// </summary>
        /// <param name="gcbh">工程编号</param>
        /// <param name="kqTime">考勤时间</param>
        [Authorize]
        public ActionResult GetGcQyKq(string gcbh, string kqTime)
        {
            DateTime nowTime;
            if (!DateTime.TryParse(kqTime, out nowTime))
            {
                nowTime = DateTime.Now;
            }
            var list = KqjService.GetGcKq(gcbh, nowTime.ToString("yyyy-MM-dd"));
            string xAxis = "", series = "";
            list.ForEach(dic =>
            {
                xAxis += "\"" + dic["qymc"] + "\",";
                if (dic["ryzs"].Equals("0"))
                {
                    series += "\"" + 0 + "\",";
                }
                else
                {
                    series += "\"" + Math.Round(Decimal.Parse(dic["rykq"]) / Decimal.Parse(dic["ryzs"]), 2) + "\",";
                }
            });
            string json = "{\"title\":\"工程标段的企业考勤统计\",\"legend\":[\"企业人员出勤率\"],\"xAxis\":[" + xAxis.Trim(',') + "],\"series\":[" + series.Trim(',') + "]}";
            return Content(json);
        }

        /// <summary>
        /// 地图统计管理
        /// </summary>
        [Authorize]
        public ActionResult Maps()
        {
            ViewData["wdqy"] = CurrentUser.Wdqy;
            return View();
        }

        /// <summary>
        ///  获取工程信息,工程企业考勤信息,工程经纬度信息
        /// </summary>
        [Authorize]
        public ActionResult GetGcInfos(string gczt, string qymc)
        {
            string gcJson = "", kqJson = "", kqjJson = "";
            gczt = HttpUtility.UrlDecode(gczt);
            qymc = HttpUtility.UrlDecode(qymc);
            if (string.IsNullOrEmpty(qymc))
            {
                qymc = "";
            }
            if (string.IsNullOrEmpty(gczt))
            {
                gczt = "";
            }
            DataSet ds = KqjService.GetGcInfos(gczt, qymc);
            foreach (DataRow gcRow in ds.Tables[0].Rows)
            {
                //根据工程编号获取企业考勤信息
                var kqRow = (from a in ds.Tables[1].AsEnumerable()
                             where a.Field<string>("JDZCH").Equals(gcRow["JDZCH"])
                             select a).ToList();
                if (kqRow.Count == 0)
                {
                    continue;
                }
                var kq = kqRow.CopyToDataTable();
                //根据工程编号获取工程图片和工程标段的中间经纬度
                var jwrRow = (from a in ds.Tables[2].AsEnumerable()
                              where a.Field<string>("JDZCH").Equals(gcRow["JDZCH"])
                              select a).ToList();
                //没有填写经纬度的则不显示在地图上
                if (jwrRow.Count == 0)
                {
                    continue;
                }
                var jwd = jwrRow.CopyToDataTable();
                string infoImg = "";
                int center = jwd.Rows.Count / 2;
                foreach (DataRow jwdRow in jwd.Rows)
                {
                    if (!string.IsNullOrEmpty(jwdRow["InfoImg"].ToString()))
                    {
                        infoImg = jwdRow["InfoImg"].ToString();
                        break;
                    }
                }
                gcJson += "{\"gcmc\":\"" + gcRow["GCMC"] + "\",\"qybh\":\"" + gcRow["QYBH"] + "\",\"qymc\":\"" + gcRow["QYMC"] + "\",\"gczt\":\"" + gcRow["GCZT"] + "\",\"gctp\":\"" + infoImg + "\",\"Lon\":\"" + jwd.Rows[center]["Lon"] + "\",\"Lat\":\"" + jwd.Rows[center]["Lat"] + "\"},";
                kqJson += "{\"gcmc\":\"" + gcRow["GCMC"] + "\",\"gczt\":\"" + gcRow["GCZT"] + "\",\"kqxx\":" + JsonConvert.SerializeObject(kq) + ",\"jwd\":" + JsonConvert.SerializeObject(jwd) + "},";
            }
            string json = "{\"gcxx\":[" + gcJson.Trim(',') + "],\"gckq\":[" + kqJson.Trim(',') + "],\"kqj\":[" + kqjJson.Trim(',') + "]}";
            return Content(json);
        }

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/31 8:51
        [Authorize]
        public ActionResult GetSwqyInfos()
        {
            string kqjJson = "";
            DataSet ds = KqjService.GetSwqyInfos(CurrentUser.Wdqy);
            foreach (DataRow qyRow in ds.Tables[0].Rows)
            {
                //根据企业编号获取企业图片和企业经纬度
                var jwrRow = (from a in ds.Tables[1].AsEnumerable()
                              where a.Field<string>("QYBH").Equals(qyRow["QYBH"])
                              select a).ToList();
                //没有经纬度的则不显示在地图上
                if (jwrRow.Count == 0)
                {
                    continue;
                }
                kqjJson += "{\"qymc\":\"" + qyRow["QYMC"] + "\",\"qylx\":\"" + qyRow["LXMC"] + "\",\"lon\":\"" + jwrRow[0]["Lon"] + "\",\"lat\":\"" + jwrRow[0]["Lat"] + "\",\"ryzs\":\"" + qyRow["RYZS"] + "\",\"kqs\":\"" + qyRow["KQS"] + "\",\"cql\":\"" + qyRow["CQL"] + "\"},";
            }
            string json = "{\"qy\":[" + kqjJson.Trim(',') + "]}";
            return Content(json);
        }

        /// <summary>
        /// 考勤机信息,考勤机断线间隔
        /// </summary>
        public ActionResult GetKqjInfos(string kqjzt)
        {
            string kqjJson = "";
            if (string.IsNullOrEmpty(kqjzt))
            {
                kqjzt = "";
            }
            DataSet ds = KqjService.GetKqjInfos(CurrentUser.Wdqy);
            //考勤机断线间隔时间
            int time = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
            foreach (DataRow kqjRow in ds.Tables[0].Rows)
            {
                var zt = "离线";
                if (DateTime.Parse(kqjRow["LastUpdate"].ToString()).AddMinutes(time) > DateTime.Now)
                {
                    zt = "在线";
                }
                if (string.IsNullOrEmpty(kqjzt) || kqjzt == zt)
                {
                    kqjJson += "{\"qymc\":\"" + kqjRow["QYMC"] + "\",\"gcmc\":\"" + kqjRow["GCMC"] + "\",\"kqjbh\":\"" + kqjRow["KQJBH"] + "\",\"lon\":\"" + kqjRow["Lon"] + "\",\"lat\":\"" + kqjRow["Lat"] + "\",\"kqjbz\":\"" + kqjRow["KQJBZ"] + "\",\"kqjzt\":\"" + zt + "\"},";
                }
            }
            string json = "{\"kqj\":[" + kqjJson.Trim(',') + "]}";
            return Content(json);
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 14:52
        public ActionResult GetRyInfos(string qylx, string qybh, string ryxm)
        {
            IList<IDictionary<string, string>> ry = KqjService.GetRyInfos(qylx, qybh, ryxm.Replace(" ", ""), CurrentUser.Wdqy);
            string json = JsonConvert.SerializeObject(ry);
            return Content(json);
        }

        #endregion

        #region 下拉框

        /// <summary>
        /// 单位类型
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:26
        public ActionResult GetDwlx()
        {
            List<IDictionary<string, string>> dwlx = KqjService.GetDwlx().ToList();
            return Content(JsonConvert.SerializeObject(dwlx));
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:55
        public ActionResult GetDwmc(string lxbh)
        {
            List<IDictionary<string, string>> dwmc = KqjService.GetDwmc(lxbh, CurrentUser.Wdqy).ToList();
            return Content(JsonConvert.SerializeObject(dwmc));
        }

        /// <summary>
        /// 获取项目企业列表
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/7/25 13:16
        public ActionResult GetGcQyList()
        {
            List<IDictionary<string, string>> dwmc = KqjService.GetGcQyList("0").ToList();
            return Content(JsonConvert.SerializeObject(dwmc));
        }

        #endregion

        #region 项目人员考勤统计

        public ActionResult XmRyKqTj()
        {
            ViewData["KqTime"] = DateTime.Now.ToString("yyyy-MM");
            return View();
        }

        #endregion

        #region 导出Excel

        /// <summary>
        /// 导出单位月考勤统计报表
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/7/19 11:03
        public string ExportKqj(FormCollection form)
        {
            ResExportJson json = new ResExportJson { Status = "failure", Msg = "导出失败,请稍后重试!" };
            try
            {
                List<EasyParam> easyParams = JsonConvert.DeserializeObject<List<EasyParam>>(Request.Form["exportfilter"]);
                string qymc = "", rq = "";
                foreach (var easyParam in easyParams)
                {
                    switch (easyParam.field)
                    {
                        case "QYMC":
                            qymc = easyParam.value;
                            break;
                        case "RQ":
                            rq = easyParam.value;
                            break;
                    }
                }
                if (string.IsNullOrEmpty(qymc) || string.IsNullOrEmpty(rq))
                {
                    json.Msg = "请选择企业并输入考勤日期!";
                }
                else
                {
                    DataTable kqxx = KqjService.GetDwKqInfos(qymc, rq);
                    DataTable kqtj = KqjService.GetDwKqTotal(qymc, rq);
                    string fileName = "考勤统计报表" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + ".xls";
                    string path = Path.Combine(HttpRuntime.AppDomainAppPath, "DownFiles\\") + fileName;
                    using (MemoryStream ms = CreateSheet(kqxx, kqtj, qymc, rq))
                    {
                        byte[] bytes = ms.ToArray();
                        FileStream fs = new FileStream(path, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(bytes);
                        bw.Close();
                        fs.Close();
                    }
                    json.Status = "success";
                    json.Msg = "导出成功!";
                    json.FileName = Base64Util.EncodeBase64(Encoding.UTF8, fileName);
                    json.Url = CryptFun.Encode(path);
                }
            }
            catch (Exception exception)
            {
                SysLog4.WriteError(exception.Message);
                json.Status = "error";
                json.Msg = "导出报错,请联系管理员!";
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        ///  创建Excel
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2015-01-15 13:59:30
        public static MemoryStream CreateSheet(DataTable kqxx, DataTable kqtj, string qymc, string rq)
        {
            //获取当前月份的天数
            DateTime startTime = DateTime.Parse(rq + "-01");
            DateTime endTime = startTime.AddMonths(1);
            int monthNumber = (endTime - startTime).Days;
            MemoryStream ms = new MemoryStream();
            HSSFWorkbook workbook = new HSSFWorkbook();
            //合同号
            string ht = "第先行开工标段";
            switch (qymc)
            {
                case "浙江省隧道工程公司":
                    ht = "第先行开工标段";
                    break;
                case "中交第三公路工程局有限公司":
                    ht = "第TJ01标段";
                    break;
                case "中交第三航务工程局有限公司":
                    ht = "第TJ02标段";
                    break;
                case "中交二公局第一工程有限公司":
                    ht = "第TJ03标段";
                    break;
                case "浙江浙中建设工程管理有限公司":
                    ht = "第JL01监理标段";
                    break;
                case "台州市公路水运工程监理咨询有限公司":
                    ht = "第JL02监理标段";
                    break;
            }
            int count = 0;//第一列
            int rowIndex = 0; //从第一行开始

            #region 样式设置

            #region 头部大标题样式

            ICellStyle topTitleStyle = workbook.CreateCellStyle(); //单元格样式
            topTitleStyle.Alignment = HorizontalAlignment.Center; //水平居中
            topTitleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直居中
            topTitleStyle.WrapText = true;
            IFont topTitleFont = workbook.CreateFont(); //字体样式
            topTitleFont.FontName = "黑体";
            topTitleFont.FontHeightInPoints = 16;
            topTitleStyle.SetFont(topTitleFont);

            #endregion

            #region 头部左小标题样式

            ICellStyle lTitleStyle = workbook.CreateCellStyle(); //单元格样式
            lTitleStyle.Alignment = HorizontalAlignment.Left; //水平
            lTitleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直
            lTitleStyle.WrapText = true;
            IFont lTitleFont = workbook.CreateFont(); //字体样式
            lTitleFont.FontName = "黑体";
            lTitleFont.FontHeightInPoints = 13;
            lTitleStyle.SetFont(lTitleFont);

            #endregion

            #region 头部右小标题样式

            ICellStyle rTitleStyle = workbook.CreateCellStyle(); //单元格样式
            rTitleStyle.Alignment = HorizontalAlignment.Right; //水平
            rTitleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直
            rTitleStyle.WrapText = true;
            IFont rTitleFont = workbook.CreateFont(); //字体样式
            rTitleFont.FontName = "黑体";
            rTitleFont.FontHeightInPoints = 13;
            rTitleStyle.SetFont(rTitleFont);

            #endregion

            #region 头部中小标题样式

            ICellStyle cTitleStyle = workbook.CreateCellStyle(); //单元格样式
            cTitleStyle.Alignment = HorizontalAlignment.Center; //水平
            cTitleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直
            cTitleStyle.WrapText = true;
            IFont cTitleFont = workbook.CreateFont(); //字体样式
            cTitleFont.FontName = "黑体";
            cTitleFont.FontHeightInPoints = 13;
            cTitleStyle.SetFont(cTitleFont);

            #endregion

            #region 尾部标题样式

            ICellStyle bottomTitleStyle = workbook.CreateCellStyle(); //单元格样式
            bottomTitleStyle.Alignment = HorizontalAlignment.Left; //水平
            bottomTitleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直
            bottomTitleStyle.WrapText = true;
            IFont bottomTitleFont = workbook.CreateFont(); //字体样式
            bottomTitleFont.FontName = "黑体";
            bottomTitleFont.FontHeightInPoints = 14;
            bottomTitleStyle.SetFont(topTitleFont);

            #endregion

            #region 标题样式

            ICellStyle titleStyle = workbook.CreateCellStyle(); //单元格样式
            titleStyle.BorderBottom = BorderStyle.Thin;
            titleStyle.BorderLeft = BorderStyle.Thin;
            titleStyle.BorderRight = BorderStyle.Thin;
            titleStyle.BorderTop = BorderStyle.Thin;
            titleStyle.BottomBorderColor = HSSFColor.Black.Index;
            titleStyle.LeftBorderColor = HSSFColor.Black.Index;
            titleStyle.RightBorderColor = HSSFColor.Black.Index;
            titleStyle.TopBorderColor = HSSFColor.Black.Index;
            titleStyle.Alignment = HorizontalAlignment.Center; //水平居中
            titleStyle.VerticalAlignment = VerticalAlignment.Center; //垂直居中
            IFont titleFont = workbook.CreateFont(); //字体样式
            titleFont.FontName = "黑体";
            titleFont.FontHeightInPoints = 12;
            titleFont.Boldweight = 10;
            titleStyle.SetFont(titleFont);

            #endregion

            #region 内容样式

            ICellStyle contentStyle = workbook.CreateCellStyle(); //单元格样式
            contentStyle.BorderBottom = BorderStyle.Thin;
            contentStyle.BorderLeft = BorderStyle.Thin;
            contentStyle.BorderRight = BorderStyle.Thin;
            contentStyle.BorderTop = BorderStyle.Thin;
            contentStyle.BottomBorderColor = HSSFColor.Black.Index;
            contentStyle.LeftBorderColor = HSSFColor.Black.Index;
            contentStyle.RightBorderColor = HSSFColor.Black.Index;
            contentStyle.TopBorderColor = HSSFColor.Black.Index;
            contentStyle.Alignment = HorizontalAlignment.Center; //水平居中
            contentStyle.VerticalAlignment = VerticalAlignment.Center; //垂直居中
            contentStyle.WrapText = true;//换行
            IFont contentFont = workbook.CreateFont(); //字体样式
            contentFont.FontName = "宋体";
            contentFont.FontHeightInPoints = 12;
            contentStyle.SetFont(contentFont);

            #endregion

            #region 内容符号样式

            ICellStyle symbolStyle = workbook.CreateCellStyle(); //单元格样式
            symbolStyle.BorderBottom = BorderStyle.Thin;
            symbolStyle.BorderLeft = BorderStyle.Thin;
            symbolStyle.BorderRight = BorderStyle.Thin;
            symbolStyle.BorderTop = BorderStyle.Thin;
            symbolStyle.BottomBorderColor = HSSFColor.Black.Index;
            symbolStyle.LeftBorderColor = HSSFColor.Black.Index;
            symbolStyle.RightBorderColor = HSSFColor.Black.Index;
            symbolStyle.TopBorderColor = HSSFColor.Black.Index;
            symbolStyle.Alignment = HorizontalAlignment.Center; //水平居中
            symbolStyle.VerticalAlignment = VerticalAlignment.Center; //垂直居中
            symbolStyle.WrapText = true;//换行
            IFont symbolFont = workbook.CreateFont(); //字体样式
            symbolFont.FontName = "黑体";
            symbolFont.FontHeightInPoints = 12;
            symbolStyle.SetFont(symbolFont);

            #endregion

            #endregion

            #region 考勤统计详情

            ISheet sheet1 = workbook.CreateSheet("sheet1");
            sheet1.PrintSetup.PaperSize = 9;//A4纸张
            sheet1.PrintSetup.Landscape = true;//横向打印
            sheet1.SetColumnWidth(0, 12 * 256);
            sheet1.SetColumnWidth(1, 10 * 256);
            //大标题
            IRow topRow1 = sheet1.CreateRow(rowIndex);
            topRow1.Height = 1000;
            ICell topCell1 = topRow1.CreateCell(0);
            topCell1.CellStyle = topTitleStyle;
            topCell1.SetCellValue("鄞州至玉环公路椒江洪家至温岭城东段公路工程\n" + rq.Replace("-", "年") + "月份考勤表");
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, monthNumber + 1));
            rowIndex++;
            IRow headerRow1 = sheet1.CreateRow(rowIndex);
            headerRow1.Height = 600;
            ICell cell01 = headerRow1.CreateCell(count);
            cell01.CellStyle = lTitleStyle;
            cell01.SetCellValue("单位：" + qymc);
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 20));
            ICell cell02 = headerRow1.CreateCell(21);
            cell02.CellStyle = rTitleStyle;
            cell02.SetCellValue("合同号：" + ht);
            CellRangeAddress cellAdd1 = new CellRangeAddress(rowIndex, rowIndex, 21, monthNumber + 1);
            sheet1.AddMergedRegion(cellAdd1);
            count = 0;
            rowIndex++;
            //循环所有表格行列,并添加边框
            for (int i = 2; i < kqxx.Rows.Count * 2 + 4; i++)
            {
                IRow rows = sheet1.CreateRow(i);
                rows.Height = 300;
                for (int j = 0; j < monthNumber + 2; j++)
                {
                    ICell cells = rows.CreateCell(j);
                    cells.CellStyle = contentStyle;
                }
            }
            //表格标题
            IRow titleRow1 = sheet1.GetRow(rowIndex);
            ICell cell011 = titleRow1.GetCell(count);
            cell011.SetCellValue("姓名");
            CellRangeAddress cellAdd2 = new CellRangeAddress(rowIndex, rowIndex + 1, count, count);
            sheet1.AddMergedRegion(cellAdd2);
            count++;
            ICell cell012 = titleRow1.GetCell(count);
            cell012.SetCellValue("日期");
            count++;
            for (int i = 0; i < monthNumber; i++)
            {
                sheet1.SetColumnWidth(count, 8 * 256);
                ICell cell = titleRow1.GetCell(count);
                cell.SetCellValue(i + 1);
                CellRangeAddress cellAdd3 = new CellRangeAddress(rowIndex, rowIndex + 1, count, count);
                sheet1.AddMergedRegion(cellAdd3);
                count++;
            }
            count = 1;
            rowIndex++;
            IRow titleRow2 = sheet1.GetRow(rowIndex);
            ICell cell021 = titleRow2.GetCell(count);
            cell021.SetCellValue("项目");
            count = 0;
            rowIndex++;
            //内容
            for (int m = 0; m < kqxx.Rows.Count; m++)
            {
                string value;
                IRow titleRow3 = sheet1.GetRow(rowIndex);
                ICell cell031 = titleRow3.GetCell(count);
                cell031.SetCellValue(kqxx.Rows[m][0].ToString());
                CellRangeAddress cellAdd4 = new CellRangeAddress(rowIndex, rowIndex + 1, count, count);
                sheet1.AddMergedRegion(cellAdd4);
                count++;
                ICell cell032 = titleRow3.GetCell(count);
                cell032.SetCellValue("上午");
                count++;
                for (int n = 0; n < monthNumber; n++)
                {
                    value = kqxx.Rows[m][n + 1].Equals(1) ? "|" : "√";
                    ICell cell = titleRow3.GetCell(count);
                    cell.CellStyle = symbolStyle;
                    cell.SetCellValue(value);
                    CellRangeAddress cellAdd5 = new CellRangeAddress(rowIndex, rowIndex + 1, count, count);
                    sheet1.AddMergedRegion(cellAdd5);
                    count++;
                }
                count = 1;
                rowIndex++;
                IRow titleRow4 = sheet1.GetRow(rowIndex);
                ICell cell041 = titleRow4.GetCell(count);
                cell041.SetCellValue("下午");
                count = 0;
                rowIndex++;
            }
            //尾部
            IRow topRow5 = sheet1.CreateRow(rowIndex);
            topRow5.Height = 600;
            ICell topCell5 = topRow5.CreateCell(0);
            topCell5.CellStyle = lTitleStyle;
            topCell5.SetCellValue("            符号:出勤:∣ 休假:√                                                                                                                     负责人（签字、盖章）：             日期：         ");
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, monthNumber + 1));
            #endregion

            #region 考勤统计汇总信息

            //创建一个名称为sheet1的工作表
            ISheet sheet2 = workbook.CreateSheet("sheet2");
            sheet2.PrintSetup.PaperSize = 9;//A4纸张
            sheet2.PrintSetup.Landscape = false;//纵向
            count = 0;
            rowIndex = 0;
            sheet2.SetColumnWidth(0, 12 * 256);
            sheet2.SetColumnWidth(1, 30 * 256);
            sheet2.SetColumnWidth(2, 15 * 256);
            sheet2.SetColumnWidth(3, 30 * 256);
            sheet2.SetColumnWidth(4, 20 * 256);
            //标题
            IRow topRow = sheet2.CreateRow(rowIndex);
            topRow.Height = 1000;
            ICell topCell = topRow.CreateCell(0);
            topCell.CellStyle = topTitleStyle;
            topCell.SetCellValue("鄞州至玉环公路椒江洪家至温岭城东段公路工程\n主要人员考勤月度统计表");
            sheet2.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4));
            rowIndex++;
            IRow headerRow = sheet2.CreateRow(rowIndex);
            headerRow.Height = 600;
            ICell cell1 = headerRow.CreateCell(count);
            cell1.CellStyle = cTitleStyle;
            cell1.SetCellValue("单位：" + qymc);
            sheet2.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, count, count + 2));
            count = count + 3;
            ICell cell4 = headerRow.CreateCell(count);
            cell4.CellStyle = lTitleStyle;
            cell4.SetCellValue("合同号：" + ht);
            count++;
            ICell cell5 = headerRow.CreateCell(count);
            cell5.CellStyle = cTitleStyle;
            cell5.SetCellValue(rq.Replace("-", "年") + "月");
            rowIndex++;
            count = 0;
            IRow titleRow = sheet2.CreateRow(rowIndex);
            titleRow.Height = 600;
            ICell cell11 = titleRow.CreateCell(count);
            cell11.CellStyle = titleStyle;
            cell11.SetCellValue("姓名");
            count++;
            ICell cell12 = titleRow.CreateCell(count);
            cell12.CellStyle = titleStyle;
            cell12.SetCellValue("岗位");
            count++;
            ICell cell14 = titleRow.CreateCell(count);
            cell14.CellStyle = titleStyle;
            cell14.SetCellValue("应勤天数");
            count++;
            ICell cell13 = titleRow.CreateCell(count);
            cell13.CellStyle = titleStyle;
            cell13.SetCellValue("出勤天数");
            count++;
            ICell cell15 = titleRow.CreateCell(count);
            cell15.CellStyle = titleStyle;
            cell15.SetCellValue("备注");
            rowIndex++;
            count = 0;
            //内容
            foreach (DataRow row in kqtj.Rows)
            {
                IRow newRow = sheet2.CreateRow(rowIndex);
                newRow.Height = 600;
                ICell cell21 = newRow.CreateCell(count);
                cell21.CellStyle = contentStyle;
                cell21.SetCellValue(row["RYXM"].ToString());
                count++;
                ICell cell22 = newRow.CreateCell(count);
                cell22.CellStyle = contentStyle;
                cell22.SetCellValue(row["gw"].ToString());
                count++;
                ICell cell24 = newRow.CreateCell(count);
                cell24.CellStyle = contentStyle;
                cell24.SetCellValue(row["yql"].ToString());
                count++;
                ICell cell23 = newRow.CreateCell(count);
                cell23.CellStyle = contentStyle;
                cell23.SetCellValue(row["cql"].ToString());
                count++;
                ICell cell25 = newRow.CreateCell(count);
                cell25.CellStyle = contentStyle;
                cell25.SetCellValue("");
                rowIndex++;
                count = 0;
            }
            //尾部
            IRow bottomRow = sheet2.CreateRow(rowIndex);
            bottomRow.Height = 600;
            ICell bottomCell = bottomRow.CreateCell(0);
            bottomCell.CellStyle = lTitleStyle;
            bottomCell.SetCellValue("                        负责人（签字、盖章）：                日期：         ");
            sheet2.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 4));

            #endregion

            //将表内容写入流 通知浏览器下载
            workbook.Write(ms);
            return ms;
        }

        #endregion

        #region 修改考勤时间
        public ActionResult ModifyKQ()
        {
            string recid = Request["recid"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            string s1 = Request["s1"].GetSafeString();
            string s4 = Request["s4"].GetSafeString();

            ViewBag.recid = recid;
            ViewBag.usercode = usercode;
            ViewBag.s1 = s1;
            ViewBag.s4 = s4;

            return View();
        }

        public void DoModifyKQ()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string usercode = Request["usercode"].GetSafeString();
                string s1 = Request["s1"].GetSafeString();
                string s4 = Request["s4"].GetSafeString();
                if (recid == "" || usercode == "")
                {
                    code = false;
                    msg = "用户不能为空！";
                }
                else
                {
                    DateTime dt = new DateTime();
                    if (s1!="" &&  !DateTime.TryParse(s1, out dt))
                    {
                        code = false;
                        msg = "上班考勤时间格式错误！";
                    }
                    else if (s4 != "" && !DateTime.TryParse(s4, out dt))
                    {
                        code = false;
                        msg = "下班考勤时间格式错误！";
                    }
                    else
                    {
                        string procstr = string.Format("SetKQSJ({0}, '{1}', '{2}', '{3}')", recid, usercode, s1, s4);
                        code = CommonService.ExecProc(procstr, out msg);
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
        #endregion
		#region 月工资设置
        /// <summary>
        /// 获取用户月工资册列表
        /// </summary>
        [Authorize]
        public void GetUserMonthPay()
        {
            string id = DataFormat.GetSafeString(RouteData.Values["id"]);
            int pageIndex = DataFormat.GetSafeInt(Request["page"], 1);
            int pageSize = DataFormat.GetSafeInt(Request["rows"], 10);

            int totalCount = 0;
            string ret = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            string jdzch = DataFormat.GetSafeString(Request["jdzch"]);
            string xm1 = DataFormat.GetSafeString(Request["xm"]);
            string bzfzr1 = DataFormat.GetSafeString(Request["bzfzr"]);
            string sfz1 = DataFormat.GetSafeString(Request["sfz"]);
            string year = DataFormat.GetSafeString(Request["year"]);
            string month = DataFormat.GetSafeString(Request["month"]);
            string gz = DataFormat.GetSafeString(Request["gz"]);
            string gw = DataFormat.GetSafeString(Request["gw"]);
            datas = KqjService.GetUserMonthPay(jdzch, xm1, sfz1, year, month, gz, gw, bzfzr1, pageSize, pageIndex, out totalCount);
            ret = jss.Serialize(datas);


            Response.ContentType = "text/plain";

            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalCount, ret));
            Response.End();
        }
        [Authorize]
        public void GetUserWGRYMonthPay()
        {
            //string id = DataFormat.GetSafeString(RouteData.Values["id"]);
            //int pageIndex = DataFormat.GetSafeInt(Request["page"], 1);
            //int pageSize = DataFormat.GetSafeInt(Request["rows"], 10);

            //int totalCount = 0;
            //string ret = "";
            //JavaScriptSerializer jss = new JavaScriptSerializer();

            //IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            //string jdzch = DataFormat.GetSafeString(Request["jdzch"]);
            //string xm1 = DataFormat.GetSafeString(Request["xm"]);
            //string bzfzr1 = DataFormat.GetSafeString(Request["bzfzr"]);
            //string sfz1 = DataFormat.GetSafeString(Request["sfz"]);
            //string year = DataFormat.GetSafeString(Request["year"]);
            //string month = DataFormat.GetSafeString(Request["month"]);
            //string gz = DataFormat.GetSafeString(Request["gz"]);
            //string gw = DataFormat.GetSafeString(Request["gw"]);
            //datas = WgryKqjService.GetUserMonthPay(jdzch, xm1, sfz1, year, month, gz, gw, bzfzr1, pageSize, pageIndex, out totalCount);
            //ret = jss.Serialize(datas);


            //Response.ContentType = "text/plain";

            //Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalCount, ret));
            //Response.End();
        }

        /// <summary>
        /// 更新月工资册
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="realpay"></param>
        /// <param name="advance"></param>
        /// <param name="paid"></param>
        /// <returns></returns>
        [Authorize]
        public void UpdateUserMonthPay(string jdzch, string userid, string ryxm, string year, string month, string gzgz,
                   string shouldpay, string havepay, string workday, string yzpay)
        {
            bool code = false;
            string msg = "";
            if (msg == "")
            {
				if (KqjService.UpdateUserMonthPay(jdzch, userid, ryxm, year, month, gzgz, shouldpay, havepay, yzpay))
                {
                    code = true;
                }
                else
                {
                    code = false;
                    msg = "保存金额出错";
                }
            }

            Response.ContentType = "text/plain";
            Response.Write(JsonFormat.GetRetString(code, msg));
            Response.End();

        }
        /// <summary>
        /// 根据json  更新月工资册
        /// </summary>
        [Authorize]
        public void UpdateUserMonthPayByJson()
        {
            bool code = false;
            string msg = "";
            string jsonstr = Request["jsonstr"].GetSafeString();
            try
            {
                if (jsonstr != "")
                {
                    JObject jsons = JObject.Parse(jsonstr);
                 
                    if(jsons!=null)
                    {
                        string jdzch = jsons.Value<string>("jdzch");
                        string userid = jsons.Value<string>("userid");
                        string ryxm = jsons.Value<string>("ryxm");
                        string year = jsons.Value<string>("year");
                        string month = jsons.Value<string>("month");
                        string gzgz = jsons.Value<string>("gzgz");
                        string shouldpay = jsons.Value<string>("shouldpay");
                        string havepay = jsons.Value<string>("havepay");
                        string yzpay = jsons.Value<string>("yzpay");
                        if (KqjService.UpdateUserMonthPay(jdzch, userid, ryxm, year, month, gzgz, shouldpay, havepay, yzpay))
                        {
                            code = true;
                        }
                        else
                        {
                            code = false;
                            msg = "保存金额出错";
                        } 
                    }
                }
            }
            catch(Exception e)
            {
                code = false;
                msg = e.Message;
            }
          

            Response.ContentType = "text/plain";
            Response.Write(JsonFormat.GetRetString(code, msg));
            Response.End();

        }

		/// <summary>
        /// 根据json  批量更新月工资册
        /// </summary>
        [Authorize]
        public void UpdateUserMonthPayByRowsJson()
        {
            bool code = false;
            string msg = "";
            string jsonstr = Request["jsonstr"].GetSafeString();
            try
            {
                if (jsonstr != "")
                {
                    JToken jsons = JToken.Parse(jsonstr);  //[]表示数组，{} 表示对象
                    foreach (JToken baseJ in jsons)//遍历数组
                    {
                        string jdzch = baseJ.Value<string>("jdzch");
                        string userid = baseJ.Value<string>("userid");
                        string ryxm = baseJ.Value<string>("ryxm");
                        string year = baseJ.Value<string>("year");
                        string month = baseJ.Value<string>("month");
                        string gzgz = baseJ.Value<string>("gzgz");
                        string shouldpay = baseJ.Value<string>("shouldpay");
                        string havepay = baseJ.Value<string>("havepay");
                        string yzpay = baseJ.Value<string>("yzpay");
                        if (KqjService.UpdateUserMonthPay(jdzch, userid, ryxm, year, month, gzgz, shouldpay, havepay, yzpay))
                        {
                            code = true;
                        }
                        else
                        {
                            code = false;
                            msg = "保存金额出错";
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }


            Response.ContentType = "text/plain";
            Response.Write(JsonFormat.GetRetString(code, msg));
            Response.End();

        }

        /// <summary>
        /// led液晶屏显示
        /// </summary>
        public void GetLedData()
        {
            bool error = false;

            string Person = "";

            VLedInfo info = new VLedInfo();
            try
            {
                //info.Weather = WeatherService.GetTemperatureForecast();
                string gcid = DataFormat.GetSafeString(Request["gcid"]);

                string curcity =  SelfService.getCity(gcid);
                info.Weather = "";// WeatherService.GetCityWeatherByName(curcity);


                string sql = "select * from i_m_gc where gcbh='" + gcid + "'";
                IList<IDictionary<string, string>> gcdata = CommonService.GetDataTable(sql);
                if (gcdata == null || gcdata.Count == 0)
                {
                    //error = true;
                    //ErrorMsg = "找不到工程信息";
                    info.Error = true;
                    info.ErrorMsg = "找不到工程信息";
                }
                else
                {                
                    IList<IDictionary<string, string>> datas = WgryKqjService.GetCurrrentInWgryStatistic(gcid, "");
                    StringBuilder sb = new StringBuilder();
                    int sum = 0;
                    foreach (IDictionary<string, string> row in datas)
                    {
                        if (row.ContainsKey("gzname") && row.ContainsKey("num"))
                        {
                            sb.Append(row["gzname"] + row["num"] + "人，");
                            sum += DataFormat.GetSafeInt(row["num"]);
                        }
                    }
                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    Person = "当前工地共有" + sum + "人";
                    if (sb.Length > 0)
                        Person += "，其中：" + sb;
                    info.Person = Person;
                }
            }
            catch (Exception e)
            {
                info.Error = true;
                info.ErrorMsg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentType = "text/plain";
                Response.Write(jss.Serialize(info));
                Response.End();
            }

        }
        #endregion

		#region 微信考勤

        /// <summary>
        /// 根据工程坐标签到
        /// </summary>
        public void WXkqIn_ByGCZB()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            double lon = Request["lon"].GetSafeDouble();
            double lat = Request["lat"].GetSafeDouble();
            bool code=false;
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else if (lon == 0 || lat == 0)
                {
                    wxjson.Msg = "获取坐标失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        double c_Distance=Configs.GetConfigItem("Distance").GetSafeDouble();
                        sql = "select lon,lat from I_M_GC_JWD where jdzch='" + gcbh + "'";
                        IList<IDictionary<string, string>> jwdlist = CommonService.GetDataTable(sql);
                        double Distance = 0;
                        double minDistance = 0;
                        for(int i=0;i<jwdlist.Count;i++)
                        {
                            double g_lon = jwdlist[i]["lon"].GetSafeDouble();
                            double g_lat = jwdlist[i]["lat"].GetSafeDouble();
                            Distance = JwdDistance.GetDistance(g_lat, g_lon, lat, lon);
                            if (i == 0)
                                minDistance = Distance;
                            else if (Distance < minDistance)
                            {
                                minDistance = Distance;
                                if (minDistance < c_Distance)
                                {
                                    code = true;
                                    break;
                                }                             
                            }                     
                        }
                        if(code)
                        {
                            wxjson.Msg = "";
                            wxjson.Status = "success";
                        }
                        else
                        {
                            wxjson.Status = "failure";
                            wxjson.Msg = "当前位置没在有效区域内";
                        }

                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
              //  Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }

        /// <summary>
        /// 根据工程坐标签退
        /// </summary>
        public void WXkqOut_ByGCZB()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            double lon = Request["lon"].GetSafeDouble();
            double lat = Request["lat"].GetSafeDouble();
            bool code = false;
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else if (lon == 0 || lat == 0)
                {
                    wxjson.Msg = "获取坐标失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        double c_Distance = Configs.GetConfigItem("Distance").GetSafeDouble();
                        sql = "select lon,lat from I_M_GC_JWD where jdzch='" + gcbh + "'";
                        IList<IDictionary<string, string>> jwdlist = CommonService.GetDataTable(sql);
                        double Distance = 0;
                        double minDistance = 0;
                        for (int i = 0; i < jwdlist.Count; i++)
                        {
                            double g_lon = jwdlist[i]["lon"].GetSafeDouble();
                            double g_lat = jwdlist[i]["lat"].GetSafeDouble();
                            Distance = JwdDistance.GetDistance(g_lat, g_lon, lat, lon);
                            if (i == 0)
                                minDistance = Distance;
                            else if (Distance < minDistance)
                            {
                                minDistance = Distance;
                                if (minDistance < c_Distance)
                                {
                                    code = true;
                                    break;
                                }
                            }
                        }
                        if (code)
                        {
                            wxjson.Msg = "";
                            wxjson.Status = "success";
                        }
                        else
                        {
                            wxjson.Status = "failure";
                            wxjson.Msg = "当前位置没在有效区域内";
                        }

                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }


            //string wxkey = Request["wxkey"].GetSafeString();
            //string gcbh = Request["gcbh"].GetSafeString();
      
            //bool code = false;
            //WXJSON wxjson = new WXJSON();
            //try
            //{
            //    double Distance = 0;
            //    double g_lat = 120.619299;
            //    double g_lon = 29.987557;
            //    double lat = 120.62446;
            //    double lon = 29.987397;

            //    Distance = JwdDistance.GetDistance(g_lat, g_lon, lat, lon);
            //    wxjson.Msg = Distance.ToString();
            //}
            //catch (Exception e)
            //{
            
            //    SysLog4.WriteLog(e);
            //}
            //finally
            //{
            //    JavaScriptSerializer jss = new JavaScriptSerializer();
            //   // Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //    Response.ContentEncoding = System.Text.Encoding.UTF8;
            //    Response.Write(jss.Serialize(wxjson));
            //    Response.End();
            //}
        }

        /// <summary>
        /// 根据二维码坐标签到
        /// </summary>
        public void WXkqIn()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            double lon = Request["lon"].GetSafeDouble();
            double lat = Request["lat"].GetSafeDouble();
            string xlh = Request["guid"].GetSafeString();
            bool code = false;
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else if (lon == 0 || lat == 0)
                {
                    wxjson.Msg = "获取坐标失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        double c_Distance = Configs.GetConfigItem("Distance").GetSafeDouble();
                        sql = "select lon,lat from I_M_QRCODE where gcbh='" + gcbh + "' and xlh='" + xlh + "'";
                        IList<IDictionary<string, string>> jwdlist = CommonService.GetDataTable(sql);
                        double Distance = 0;
                        double minDistance = 0;
                        if (jwdlist.Count>0)
                        {
                            double g_lon = jwdlist[0]["lon"].GetSafeDouble();
                            double g_lat = jwdlist[0]["lat"].GetSafeDouble();
                            Distance = JwdDistance.GetDistance(g_lat, g_lon, lat, lon);

                            minDistance = Distance;
                            if (minDistance < c_Distance)
                            {
                                code = true;     
                            }
                            if (code)
                            {
                                sql = "INSERT INTO kqjuserlog (userid,serial,LogDate,placeid,hasdeal,logtype,ExtraInfo2,Longitude,Latitude) values ('" + sfzhm + "','" + xlh + "',CONVERT(varchar(100), GETDATE(), 120),'" + gcbh + "',0,'1','qrcode','" + lon + "','" + lat + "')";
                                CommonService.Execsql(sql);
                                wxjson.Msg = "";
                                wxjson.Status = "success";
                            }
                            else
                            {
                                wxjson.Status = "failure";
                                wxjson.Msg = "当前位置没在有效区域内";
                            }
                        }
                        else
                        {
                            code = false;
                            wxjson.Status = "failure";
                            wxjson.Msg = "没有查到该二维码信息或该二维码还未定位";
                        }                      
                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                //  Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }
        /// <summary>
        /// 根据二维码签退
        /// </summary>
        public void WXkqOut()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            double lon = Request["lon"].GetSafeDouble();
            double lat = Request["lat"].GetSafeDouble();
            string xlh = Request["guid"].GetSafeString();
            bool code = false;
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else if (lon == 0 || lat == 0)
                {
                    wxjson.Msg = "获取坐标失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        double c_Distance = Configs.GetConfigItem("Distance").GetSafeDouble();
                        sql = "select lon,lat from I_M_QRCODE where gcbh='" + gcbh + "' and xlh='" + xlh + "'";
                        IList<IDictionary<string, string>> jwdlist = CommonService.GetDataTable(sql);
                        double Distance = 0;
                        double minDistance = 0;
                        if (jwdlist.Count > 0)
                        {
                            double g_lon = jwdlist[0]["lon"].GetSafeDouble();
                            double g_lat = jwdlist[0]["lat"].GetSafeDouble();
                            Distance = JwdDistance.GetDistance(g_lat, g_lon, lat, lon);

                            minDistance = Distance;
                            if (minDistance < c_Distance)
                            {
                                code = true;
                            }
                            if (code)
                            {
                                sql = "INSERT INTO kqjuserlog (userid,serial,LogDate,placeid,hasdeal,logtype,ExtraInfo2,Longitude,Latitude) values ('" + sfzhm + "','" + xlh + "',CONVERT(varchar(100), GETDATE(), 120),'" + gcbh + "',0,'2','qrcode','" + lon + "','" + lat + "')";
                                CommonService.Execsql(sql);
                                wxjson.Msg = "";
                                wxjson.Status = "success";
                            }
                            else
                            {
                                wxjson.Status = "failure";
                                wxjson.Msg = "当前位置没在有效区域内";
                            }
                        }
                        else
                        {
                            code = false;
                            wxjson.Status = "failure";
                            wxjson.Msg = "没有查到该二维码信息";
                        }      

                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }
        #endregion
    }
}