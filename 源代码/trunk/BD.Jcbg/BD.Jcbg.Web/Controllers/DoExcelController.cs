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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BD.Jcbg.Web.Controllers
{
    public class DoExcelController : Controller
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

        private IExcelService _ExcelService = null;
        private IExcelService ExcelService
        {
            get
            {
                try
                {
                    if (_ExcelService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _ExcelService = webApplicationContext.GetObject("ExcelService") as IExcelService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _ExcelService;
            }
        }
		#endregion

		#region 页面
        /// <summary>
        /// 银行端更新账号余额
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult uploadYHZH_YE()
        {
            return View();
        }

        [Authorize]
        public ActionResult uploadFile()
        {
            return View();
        }
        [Authorize]
        public ActionResult uploadFileGZFF()
        {
            string guid = Request["rguid"].GetSafeString();
            ViewData["guid"] = guid;        
            return View();
        }
        [Authorize]
        public ActionResult uploadFilePLKHDR()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            ViewData["gcbh"] = gcbh;
            return View();
        }
        #endregion

        /// <summary>
        /// 解析银行账户余额
        /// </summary>
        [Authorize]
        public void jxExcel()
        {
            bool code = true;
            string msg = "";
            try
            {
                HttpPostedFileBase file = Request.Files[0];

                Stream stream = file.InputStream;//new MemoryStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                //设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
            
              
                ///
                IWorkbook workbook = null;
                ISheet sheet = null;
                string filename = file.FileName;
                string fileExt = Path.GetExtension(filename).ToLower();
                Stream fstream = new MemoryStream(bytes);
                if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(fstream); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(fstream); } else { workbook = null; }
                sheet = workbook.GetSheetAt(0);
                IList<string> sqls = new List<string>();
                for (int i = sheet.FirstRowNum + 2; i <= sheet.LastRowNum+1; i++)
                {
                    string qymc = sheet.GetCellValue(i, 1);
                    string yhzh = sheet.GetCellValue(i, 2);
                    string zhye = sheet.GetCellValue(i, 3);
                    string sj = sheet.GetCellValue(i, 4);
                    string gxsj="";
                    if( sheet.GetRow(i-1).GetCell(3)!=null)
                         gxsj = sheet.GetRow(i-1).GetCell(3).DateCellValue.ToString();
                    string sql = "update I_S_GC_YH set ZHYE='" + zhye + "',qymc='" + qymc + "',gxsj='" + gxsj + "' where yhzh='" + yhzh + "'";
                    sqls.Add(sql);
                }
                if(sqls.Count>0)
                {
                    CommonService.ExecSqls(sqls);
                    saveExcel(file.FileName, bytes); //保存excel
                }
                else
                {
                    msg = "没有需导入信息";
                    code = false;
                }
             
            }
           catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            Response.Write(JsonFormat.GetRetString(code, msg));
        }
        
        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }
        private static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        private  string getCellValue( ISheet sheet ,int row,int col)
        {
            IRow firstRow = sheet.GetRow(row);//第一行  
            string pch = firstRow.GetCell(col).ToString(); //第一行第一列
            string []list = pch.Split(':');
            if (list.Count() != 2)
            {
                return "";
            }
            return list[1].Trim();
        }
        /// <summary>
        /// 解析银行实际工资发放
        /// </summary>
        [Authorize]
        public void jxExcelGzff()
        {
            bool code = true;
            string msg = "";
            string guid = Request["guid"].GetSafeString();
            string recids = "";
            try
            {
                string sql_g = "select payrecids from view_i_m_xzff where rguid='" + guid + "'";
                IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql_g);
                if(dt.Count>0)
                {
                    recids = dt[0]["payrecids"];
                    string [] recidlist=recids.Split(',');
                    string t_guid = Guid.NewGuid().ToString();//作为发放日志的流水号
                    HttpPostedFileBase file = Request.Files[0];

                    Stream stream = file.InputStream;//new MemoryStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    //设置当前流的位置为流的开始
                    stream.Seek(0, SeekOrigin.Begin);

                    IWorkbook workbook = null;
                    ISheet sheet = null;
                    string filename = file.FileName;
                    string fileExt = Path.GetExtension(filename).ToLower();
                    Stream fstream = new MemoryStream(bytes);
                    if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(fstream); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(fstream); } else { workbook = null; }
                    sheet = workbook.GetSheetAt(0);
                    IRow firstRow = sheet.GetRow(0);//第一行  
                    string pch = firstRow.GetCell(0).ToString(); //第一行第一列
                    string []listpch = pch.Split(':');
                    if(listpch.Count()!=2)
                    {
                        code = false;
                        msg = "批次号获取失败;";
                    }
                    pch = listpch[1].Trim();
                    string fkzh = sheet.GetRow(1).GetCell(0).ToString(); //第二行第一列
                    string[] listfkzh = fkzh.Split(':');
                    if (listfkzh.Count() != 2)
                    {
                        code = false;
                        msg += "付款人账号获取失败";
                    }
                    fkzh = listfkzh[1].Trim();
                    if( pch == "" || fkzh=="")
                    {
                        code = false;
                        msg = "批次号获取失败或者付款人账号获取失败";
                    }
                    string ffzje = getCellValue(sheet, 3, 0).Replace(",", "");
                    if(ffzje=="")
                    {
                        code = false;
                        msg = "获取批总金额失败";
                    }
                    string pzbs = getCellValue(sheet, 3, 1) ;
                    if (pzbs == "")
                    {
                        code = false;
                        msg += "获取批总笔数失败";
                    }
                    if (recidlist.Length != pzbs.GetSafeInt())
                    {
                        code = false;
                        msg += "总笔数与应发总笔数不一致";
                    }
                    if (fkzh!="")
                    {
                        string sql = "select * from View_I_M_XZFF where rguid='" + guid + "'";
                        IList<IDictionary<string, string>> gcyhzhdatas =CommonService.GetDataTable(sql);
                        if(gcyhzhdatas.Count>0)
                        {
                            if(gcyhzhdatas[0]["yhzh"]!=fkzh)
                            {
                                code = false;
                                msg = "付款账号与登记的银行项目账户不匹配";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "查不到该发放信息";
                        }
                    }
                    //判断账号余额与发放总金额对比 相减
                    string yesql = "select zhye from I_S_GC_YH where yhzh='" + fkzh + "'";
                    IList<IDictionary<string, string>> yhyedata = CommonService.GetDataTable(yesql);
                    if (yhyedata.Count > 0)
                    {
                        if(yhyedata[0]["zhye"].GetSafeDecimal()<ffzje.GetSafeDecimal())
                        {
                            msg = "银行余款小于批总金额，请先更新银行余款";
                            code = false;
                        }
                    }
                    if (msg == "")
                    {
                        string str_ffrz = "";
                        IList<string> sqls = new List<string>();
                        List<string> yhzhs = new List<string>();
                        List<string> ryxms = new List<string>();
                        for (int i = sheet.FirstRowNum + 11; i <= sheet.LastRowNum + 1; i++)
                        {
                            string yhzh = sheet.GetCellValue(i, 4).Trim();
                            yhzhs.Add(yhzh);
                            string ryxm = sheet.GetCellValue(i, 5).Trim();
                            ryxms.Add(ryxm); 
                            string ffzt = sheet.GetCellValue(i, 8).Trim();
                            string ffje = GetValueType(sheet.GetRow(i-1).GetCell(2)).ToString(); //sheet.GetCellValue(i, 3);
                            ffje = ffje.Replace(",", "").Trim();

                            //string sql = "update kqjusermonthpay set bankpay='" + ffje + "',notpay=shouldpay-'" + ffje + "' where userid=(select sfzhm from i_s_gc_yhkh where yhkh='" + yhzh + "') and recid in (select item from dbo.splitstring((select top 1 payrecids from INFO_XZFF where rguid='" + guid + "'),',',1))";
                            string sql = "update kqjusermonthpay set bankpay='" + ffje + "',notpay=shouldpay-'" + ffje + "' where userid=(select sfzhm from i_s_gc_yhkh where yhkh='" + yhzh + "') and recid in (" + recids + ")";

                            sqls.Add(sql);
                            if (i == sheet.LastRowNum + 1)
                                str_ffrz += "select '" + ryxm + "','" + yhzh + "','" + t_guid.ToString() + "','" + pch + "','" + ffje + "','" + ffzt +"' ";
                            else
                                str_ffrz += "select '" + ryxm + "','" + yhzh + "','" + t_guid.ToString() + "','" + pch + "','" + ffje + "','" + ffzt + "' union all ";
                        }
                        string sql_yhk = "select ryxm,yhkh from View_KQJUSERMONTHPAY where recid in (select item from dbo.splitstring((select top 1 payrecids from INFO_XZFF where rguid='" + guid + "'),',',1))";
                        IList<IDictionary<string, string>> yhkslist = CommonService.GetDataTable(sql_yhk);
                        for (int h = 0; h < yhkslist.Count;h++ )
                        {
                            if (!yhzhs.Contains(yhkslist[h]["yhkh"]))
                                msg += "导入的发放信息不包含" + yhkslist[h]["ryxm"] + "的" + yhkslist[h]["yhkh"] + "</br>";
                        }
                        if (sqls.Count > 0 && msg=="")
                        {
                            //更新薪资发放审批表
                            string gsql = "update INFO_M_XZFF set ffcg=1,ffcgsj=getdate() where rguid='" + guid + "'";
                            sqls.Add(gsql);

                            code = CommonService.ExecSqls(sqls);
                            saveExcel(file.FileName, bytes);//保存


                            //更新薪资发放流水日志表 I_M_XZFF_LSRZ I_S_XZFF_LSRZ
                            if (code)
                            {
                                string sql = "select * from i_m_xzff_lsrz where pch='" + pch + "' and guid='" + guid + "'";
                                IList<IDictionary<string, string>> lsrzdatas = CommonService.GetDataTable(sql);
                                if (lsrzdatas.Count == 0) //第一次导入
                                {
                                    sql = "insert into i_m_xzff_lsrz ([LSH],[PCH],[YHZH],[GUID],[GXSJ],[FFZJE]) values('" + t_guid.ToString() + "','" + pch + "','" + fkzh + "','" + guid + "',getdate(),'" + ffzje + "')";
                                    IList<string> t_sqls = new List<string>();
                                    t_sqls.Add(sql); //更新流水表主表

                                    sql = "INSERT INTO I_S_XZFF_LSRZ ([RYXM] ,[YHKH],[LSH],[PCH] ,[FFJE],[FFZT]) " + str_ffrz;
                                    t_sqls.Add(sql); //更新流水表主表

                                    //更新银行账户余额
                                    string syye = (yhyedata[0]["zhye"].GetSafeDecimal() - ffzje.GetSafeDecimal()).ToString();
                                    sql = "update I_S_GC_YH set zhye='" + syye + "' where yhzh='" + fkzh + "'";
                                    t_sqls.Add(sql);
                                    code = CommonService.ExecSqls(t_sqls);
                                }
                                else //同批次号再次导入
                                {
                                    sql = "update  i_m_xzff_lsrz set [LSH]='" + t_guid + "', [GXSJ]=getdate(),ffzje='" + ffzje + "' where pch='" + pch + "' and yhzh='" + fkzh + "'";
                                    IList<string> t_sqls = new List<string>();
                                    t_sqls.Add(sql); //更新流水表主表

                                    sql = "INSERT INTO I_S_XZFF_LSRZ ([RYXM] ,[YHKH],[LSH],[PCH] ,[FFJE],[FFZT]) " + str_ffrz;
                                    t_sqls.Add(sql); //更新流水表主表
                                    code = CommonService.ExecSqls(t_sqls);
                                }
                            }


                        }
                        else
                        {
                            code = false;
                            if(msg=="")
                                 msg = "excel表解析失败";
                        }
                    }
                    
                             
                }
                else
                {
                    code = false;
                    msg = "记录为空";
                }

                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            Response.Write(JsonFormat.GetRetString(code, msg));
        }
        private void saveExcel(string filename, byte[] bytes)
        {
            try
            {
                string path = Server.MapPath("~/excelFile/") + DateTime.Now.ToString("yyyyMMddHHmmssfff")+"_"+CurrentUser.RealName+"_" + filename;//设定上传的文件路径
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch(Exception e)
            { }
        }

        /// <summary>
        /// 解析银行导入开卡卡号
        /// </summary>
        [Authorize]
        public void jxExcelPLKHDR()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string yhmc = "";
                string sql = "select * from INFO_YH where ZH=(select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')";
                IList<IDictionary<string, string>> yhdata=CommonService.GetDataTable(sql);
                if(yhdata.Count>0)
                {
                    yhmc = yhdata[0]["yhmc"];
                    HttpPostedFileBase file = Request.Files[0];

                    Stream stream = file.InputStream;//new MemoryStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    //设置当前流的位置为流的开始
                    stream.Seek(0, SeekOrigin.Begin);

                    IWorkbook workbook = null;
                    ISheet sheet = null;
                    string filename = file.FileName;
                    string fileExt = Path.GetExtension(filename).ToLower();
                    Stream fstream = new MemoryStream(bytes);
                    if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(fstream); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(fstream); } else { workbook = null; }
                    sheet = workbook.GetSheetAt(0);
                    IRow hssfRow = sheet.GetRow(4);
                    int columnNum = hssfRow.PhysicalNumberOfCells; //获取列数

                    IList<string> sqls = new List<string>();
                    string yhkhs = "";
                    for (int i = sheet.FirstRowNum + 4; i <= sheet.LastRowNum+1; i++)
                    {
                        string ryxm = sheet.GetCellValue(i, 2);
                        string sfzhm = sheet.GetCellValue(i, 4);
                        string yhzh = sheet.GetCellValue(i, 8);
                        yhkhs += "'"+yhzh+ "',";
                        sql = "update i_m_wgry set yhkyh='" + yhmc + "',yhkh='" + yhzh + "' where sfzhm='" + sfzhm + "' and jdzch='" + gcbh + "' and (yhkh='' or yhkh is null)";
                        string sql_k = "insert into i_s_gc_yhkh (jdzch,sfzhm,yhkyh,yhkh,ryxm,sjhm) select '" +
                            gcbh + "','" + sfzhm + "','" + yhmc + "','" + yhzh + "','" + ryxm + "', sjhm from i_m_wgry where jdzch='" + gcbh + "' and sfzhm='" + sfzhm + "'";
                        sqls.Add(sql);
                        sqls.Add(sql_k);
                    }
                    yhkhs = yhkhs.Substring(0,yhkhs.Length - 1);
                    string gsql = "select * from i_s_gc_yhkh where yhkh in ("+yhkhs+")"; //判断导入的表格中银行卡号是不是已经存在
                    IList<IDictionary<string, string>> yhkdatas=CommonService.GetDataTable(gsql);
                    for (int i = 0; i < yhkdatas.Count;i++ )
                    {
                        msg += yhkdatas[i]["yhkh"] + "卡号已经存在,不能重复导入\r\n";
                    }
                    if(msg=="")
                    {
                        if (sqls.Count > 0)
                        {
                            CommonService.ExecSqls(sqls);
                            //保存excel
                            saveExcel(file.FileName, bytes);
                        }                    
                    }
                    else
                    {
                        code = false;
                    }                
                }
                else
                {
                    code = false;
                    msg = "该账号没有与所属银行绑定";
                }

               
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            Response.Write(JsonFormat.GetRetString(code, msg));
        }

        /// <summary>
        /// 导出银行发放表
        /// </summary>
        [Authorize]
        public  void exportTXTGZFF()
        {
            string str_txt = "PAYOFFSTART\r\n";
            string guid = Request["rguid"].GetSafeString();       
            string gcbh = Request["gcbh"].GetSafeString();     
            try
            {
                IList<IDictionary<string, string>> yhdatas =CommonService.GetDataTable("select * from i_s_gc_yh where gcbh='" + gcbh + "'");
                if(yhdatas.Count>0)
                {
                    string yhzh = yhdatas[0]["yhzh"];
                    string sql = "select * from View_KQJUSERMONTHPAY where recid in (select item from dbo.splitstring((select top 1 payrecids from INFO_XZFF where rguid='" + guid + "'),',',1))";
                    IList<IDictionary<string, string>> paydatas = CommonService.GetDataTable(sql);
                    decimal totalpay = 0;
                    string rypaydetail = "";
                    for (int i = 0; i < paydatas.Count; i++)
                    {
                        if (paydatas[i]["yhkh"] == "")
                            continue;
                        totalpay += paydatas[i]["havepay"].GetSafeDecimal();
                        rypaydetail += "33|" + paydatas[i]["havepay"] + "|" + paydatas[i]["yhkh"] + "|" + paydatas[i]["ryxm"]+"|||\r\n";
                    }
                    str_txt += "|45156|" + yhzh + "|1|CNY|" + totalpay + "|3|EV|"+DateTime.Now.ToString("yyyyMMdd")+"||\r\n";
                    str_txt += rypaydetail + "PAYOFFEND";
                }
                 
            }
            catch(Exception e)
            {

            }
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //设置输出流
            Response.ContentType = "application/octet-stream";
            //防止中文乱码
            string fileName = HttpUtility.UrlEncode("网银发放文本");
            //设置输出文件名
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".txt");
            //输出
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetBytes(str_txt));
        }

    }
}
