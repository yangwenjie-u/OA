using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.Bll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BD.WorkFlow.DataModal.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Text.RegularExpressions;
using BD.Jcbg.Web.Func;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using NPOI.SS.Util;

namespace BD.Jcbg.Web.Controllers
{
    public class ZhwxController : Controller
    {

        #region 定义
        //   private const int pagsize_ = 5;
        //  private const string appid_ = "wxc7e4da3abafb7bde";
        //   private const string secret_ = "65e160b471effda6a0847bce39e11718";
        //   private const string template_id_1_ = "G54Q6I540w34uK5oKYAPoopqjWjktytqU39c9ioCdkU";


        private const string wx_pagesize_ = "WX_PAGESIZE";
        private const string wx_appid_ = "WX_APPID";
        private const string wx_secret_ = "WX_SECRET";
        private const string wx_message_push_get_access_token_url_ = "WX_MESSAGE_PUSH_GET_ACCESS_TOKEN_URL";
        private const string wx_message_push_get_access_token_grant_type_ = "WX_MESSAGE_PUSH_GET_ACCESS_TOKEN_GRANT_TYPE";
        private const string wx_message_push_url_ = "WX_MESSAGE_PUSH_URL";
        private const string wx_template_id_1_ = "WX_TEMPLATE_ID_1";
        private const string wx_login_url_ = "WX_LOGIN_URL";
        private const string wx_login_grant_type_ = "WX_LOGIN_GRANT_TYPE";
        private const string bd_wx_login_verifycode_length_ = "BD_WX_LOGIN_VERIFYCODE_LENGTH";
        private const string bd_wx_login_verifycode_time_ = "BD_WX_LOGIN_VERIFYCODE_TIME";
        private const string bd_wx_login_verifycode_template_ = "BD_WX_LOGIN_VERIFYCODE_TEMPLATE";
        private const string bd_wx_equip_status_template_ = "BD_WX_EQUIP_STATUS_TEMPLATE";
        private const string wx_push_unitname_ = "WX_PUSH_UNITNAME";
        private const string bd_wx_login_verifycode_count_ = "BD_WX_LOGIN_VERIFYCODE_COUNT";
        private const string zhwx_bd_sms_base_appid_ = "ZHWX_BD_SMS_BASE_APPId";

        private const string bd_wx_universal_template_ = "BD_WX_UNIVERSAL_TEMPLATE";



        private static Dictionary<string, string> dic_ = new Dictionary<string, string>();

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


        private BD.WorkFlow.Bll.RemoteUserService _remoteUserService = null;
        private BD.WorkFlow.Bll.RemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as BD.WorkFlow.Bll.RemoteUserService;
                }
                return _remoteUserService;
            }
        }


        private ISmsService _smsService = null;
        private ISmsService SmsService
        {
            get
            {
                if (_smsService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                }
                return _smsService;
            }
        }

        #endregion








        #region 页面生成二维码



        /// <summary>
        /// 生成设备二维码图片（作废）
        /// </summary>
        public void getvcode()
        {
            try
            {
                string equipid = R("equipid").GetSafeString();
                byte[] fileContent = Barcode.GetBarcode2(equipid, 150, 150);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + equipid + ".jpg");
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
        /// 转向设备二维码地址(作废)
        /// </summary>
        /// <returns></returns>
        public ActionResult createvcode()
        {
            //  try {
            //     string equipid = R("equipid").GetSafeString();
            //       byte[] fileContent = Barcode.GetBarcode2(equipid, 150, 150);
            //        Response.ContentEncoding = System.Text.Encoding.UTF8;
            //       Response.HeaderEncoding = System.Text.Encoding.UTF8;
            //       Response.AddHeader("Content-Disposition", "attachment;filename=ewm.jpg");
            //        Response.Charset = "UTF-8";
            // Response.ContentType = "application/octet-stream";
            //        Response.BinaryWrite(fileContent);
            //   }
            //    catch (Exception e)
            //     {
            //         SysLog4.WriteLog(e);
            //     }


            try
            {
                ViewData["param"] = "";
                Boolean flag = true;
                string equipid = R("equipid").GetSafeString();
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                IList<string> sqls = new List<string>();
                string sql = "select * from zhwx_sbewm where id = '" + equipid + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count <= 0)
                {
                    sql = "insert into zhwx_sbewm(id,value) values('" + equipid + "','" + equipid + "') ";
                    sqls.Add(sql);
                    flag = CommonService.ExecTrans(sqls);
                }
                if (flag)
                {
                    ViewData["param"] = equipid;
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ViewData["param"] = "";
            }


            return View("Index");
        }



        /// <summary>
        /// 生成设备二维码
        /// </summary>
        public void createidentifycode()
        {
            string retMsg = "{\"success\":false}";
            string mb = "SBEWM";
            try
            {
                Boolean flag = true;
                string equipid = R("equipid").GetSafeString();

                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "SELECT   *  FROM  zhwx_excel_mb where type='SBEWM' and cpcode='" + CurrentUser.CompanyCode + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count > 0)
                {
                    mb = list[0]["mb"].GetSafeString();
                }

                 
                IList<string> sqls = new List<string>();
                sql = "select * from zhwx_sbewm where id = '" + equipid + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count <= 0)
                {
                    sql = "insert into zhwx_sbewm(id,value) values('" + equipid + "','" + equipid + "') ";
                    sqls.Add(sql);
                    flag = CommonService.ExecTrans(sqls);
                }
                if (flag)
                {
                    retMsg = "{\"success\":true,\"mb\":\"" + mb + "\"}";
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }


        /// <summary>
        ///  
        /// </summary>
        public void getpower()
        {
            string retMsg = "{\"success\":false}";
            try
            {
                string recid = R("recid").GetSafeString();
                Boolean flag = true;
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.VUserrole[] vUserrole = srv.GetRoleListByProcodeAndUsername("JCXT_ZJZH", CurrentUser.UserName);
                RemoteUserService.SUser user = srv.GetUserInfo("", CurrentUser.UserName);
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string code = CurrentUser.UserRights.GetSafeString();
                flag = ("," + code + ",").Contains(",QTQX_SBXG,");
                

                if (flag)
                {
                    IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                    string sql = "select depcode from i_m_sb where recid='" + recid + "' ";
                    list = CommonService.GetDataTable(sql);
                    if (list.Count > 0)
                    {
                        string depcode = list[0]["depcode"].GetSafeString();
                        sql = "select * from  zhwx_excel_no_depcode where sqlcode like '%" + user.DEPCODE + "%' and sort=1 and cpcode='"+user.CPCODE+"'  ";
                        list = CommonService.GetDataTable(sql);
                        if (list.Count > 0)
                        {
                            flag = jss.Serialize(user.DEPCODE).Contains(depcode);
                            if(flag)
                                retMsg = "{\"success\":true}";
                            else 
                                retMsg = "{\"success\":false}";
                        }
                        else {
                            retMsg = "{\"success\":true}";
                        }

                    }
                    else {
                        retMsg = "{\"success\":false}";
                    }
                    
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }


        public void ExportExcel1()
        {

            //组装JSON
            StringBuilder html = new StringBuilder();
            //标题 
            string[] columnCaption = new string[] { "单位代码", "单位名称", "用户代码", "用户名称", "姓名", "备注" };
            for (int i = 0; i < columnCaption.Length; i++)
            {
                html.AppendFormat("{0},", columnCaption[i]);
            }
            if (html.Length > 0 && html[html.Length - 1] == ',')
                html.Remove(html.Length - 1, 1);
            html.AppendLine("");
            for (int i = 0; i < 2; i++)
            {
                html.AppendFormat("{0},", "2345");
                html.AppendFormat("{0},", "2345");
                html.AppendFormat("{0},", "2345");
                html.AppendFormat("{0},", "2345");
                html.AppendFormat("{0},", "2345");
                html.AppendFormat("{0}", "2345");
                html.AppendLine("");
            }



            //HttpResponse resp = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("用户信息", System.Text.Encoding.UTF8).ToString() + ".csv");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            Response.Write(html.ToString());
            Response.End();
        }


        /// <summary>
        /// 测量设备管理台账
        /// </summary>
        /// <returns></returns>
        public ActionResult celiangexcel() {
            ViewData["zdzd"] = "";
            IList<string[]> zdzd = new List<string[]>();
            IList<string[]> deplist = new List<string[]>();
            string template = "CELIANG.xls";
            string outzdmc = "SBBH,SBMC,SBXH,CCBH,CLFW,ZQDDJ,SYFS,SCCJ,LRRXM,BZ";
            try
            {
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "SELECT   *  FROM  zhwx_excel_mb where type='CELIANG' and cpcode='" + CurrentUser.CompanyCode + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count>0)
                {
                    template = list[0]["mb"].GetSafeString();
                    outzdmc = list[0]["sqlzd"].GetSafeString();
                }
                //IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                 
                list.Clear();
                sql = "SELECT     zdmc,zdlx,sy FROM  ZDZD_JC WHERE  (SJBMC = 'I_M_SB') and zdmc in ('" + outzdmc.Replace(",", "','") + "') order by  xssx asc"; 
                list = CommonService.GetDataTable(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    zdzd.Add(new string[] { list[i]["zdmc"].GetSafeString(), list[i]["sy"].GetSafeString(), list[i]["zdlx"].GetSafeString() });
                }
                IList<IDictionary<string, string>> list2 = new List<IDictionary<string, string>>();
                sql = "SELECT    recid,sqlcode, itemname  FROM  zhwx_excel_no_depcode  where   cpcode='" + CurrentUser.CompanyCode + "'  ORDER BY sort ";
                list2 = CommonService.GetDataTable(sql);
               
                for (int i = 0; i < list2.Count; i++)
                {
                    deplist.Add(new string[] { list2[i]["recid"].GetSafeString(), list2[i]["itemname"].GetSafeString() });
                }

                ViewData["zdzd"] = zdzd;
                ViewData["outzdmc"] = outzdmc;
                ViewData["template"] = template;
                ViewData["deplist"] = deplist;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }

            return View("QTExcel");
        }

        /// <summary>
        /// 周期溯源计划以及实施记录表
        /// </summary>
        /// <returns></returns>
        public ActionResult suyuanexcel()
        {
            ViewData["zdzd"] = "";
            IList<string[]> zdzd = new List<string[]>();
            IList<string[]> deplist = new List<string[]>();
            string template = "ZQSYJH.xls";
            string outzdmc = "SBBH,SBMC,SBXH,SYDW,JNJHSYSJ,QYRQ,ZSLX,ZSBH,XCBDRQ,BZ";
            try
            {
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "SELECT   *  FROM  zhwx_excel_mb where type='ZQSYJH' and cpcode='" + CurrentUser.CompanyCode + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count > 0)
                {
                    template = list[0]["mb"].GetSafeString();
                    outzdmc = list[0]["sqlzd"].GetSafeString();
                }
                //IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                
                list.Clear();
                sql = "SELECT     zdmc,zdlx,sy FROM  ZDZD_JC WHERE  (SJBMC = 'I_M_SB') and zdmc in ('" + outzdmc.Replace(",", "','") + "') order by  xssx asc";
                list = CommonService.GetDataTable(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    zdzd.Add(new string[] { list[i]["zdmc"].GetSafeString(), list[i]["sy"].GetSafeString(), list[i]["zdlx"].GetSafeString() });
                }
                IList<IDictionary<string, string>> list2 = new List<IDictionary<string, string>>();
                sql = "SELECT   recid, sqlcode, itemname  FROM  zhwx_excel_no_depcode  where   cpcode='" + CurrentUser.CompanyCode + "' ORDER BY sort ";
                list2 = CommonService.GetDataTable(sql);

                for (int i = 0; i < list2.Count; i++)
                {
                    deplist.Add(new string[] { list2[i]["recid"].GetSafeString(), list2[i]["itemname"].GetSafeString() });
                }

                ViewData["zdzd"] = zdzd;
                ViewData["outzdmc"] = outzdmc;
                ViewData["template"] = template;
                ViewData["deplist"] = deplist;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }

            return View("QTExcel");
        }


        public void doqtexcel()
        {

            string sql = "";
            string usercode = CurrentUser.UserName;

            string szWhere = "";

            string zdmc = R("zdmc").GetSafeString();
            string sy = R("sy").GetSafeString();
            string zdlx = R("zdlx").GetSafeString();

            string outzdmc = R("outzdmc").GetSafeString().ToLower();
            string template = R("template").GetSafeString();

            string sqlcode = " not in ('') ";
            int recid = RI("sqlcode");
            string itemname = R("itemname").GetSafeString();

            string[] zdmcArray = zdmc.Split(',');
            string[] syArray = sy.Split(',');
            string[] zdlxArray = zdlx.Split(',');

            string[] outzdmcArray = outzdmc.Split(',');

            string outzdmc_tmp = "," + outzdmc + ",";

            try
            {

                IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                ret = CommonService.GetDataTable("SELECT   recid, sqlcode, itemname  FROM  zhwx_excel_no_depcode  where recid = " + recid + "   ");
                if(ret.Count>0){
                    sqlcode = ret[0]["sqlcode"].GetSafeString();
                }

                for (int i = 0; i < zdmcArray.Length; i++)
                {
                    ;
                    string t = R(zdmcArray[i] + "t").GetSafeString();//条件
                    if (zdlxArray[i].Equals("datetime"))
                    {
                        outzdmc_tmp = outzdmc_tmp.Replace("," + zdmcArray[i].GetSafeString().ToLower() + ",", ",CONVERT(varchar, t1." + zdmcArray[i].GetSafeString().ToLower() + ", 23 ) as " + zdmcArray[i].GetSafeString().ToLower() + ",");
                        string[] tArray = t.Split(',');
                        if (tArray.Length == 2)
                        {
                            if (!tArray[0].GetSafeString().Equals(""))
                            {
                                szWhere += " and CONVERT(varchar, t1." + zdmcArray[i] + ", 23 ) >= '" + tArray[0].GetSafeString() + "'";
                            }

                            if (!tArray[1].GetSafeString().Equals(""))
                            {
                                szWhere += " and CONVERT(varchar, t1." + zdmcArray[i] + ", 23 ) <= '" + tArray[1].GetSafeString() + "'";
                            }
                        }

                    }
                    else
                    {

                        string v = R(zdmcArray[i] + "v").GetSafeString();//值
                        switch (t)
                        {
                            case "like":
                                szWhere += " and t1." + zdmcArray[i] + " like '%" + v + "%'";
                                break;
                            case "":
                                break;
                            default:
                                szWhere += " and t1." + zdmcArray[i] + " " + t + " '" + v + "'";
                                break;

                        }

                    }



                }
                outzdmc_tmp = outzdmc_tmp.Substring(1, outzdmc_tmp.Length - 2);
                szWhere = szWhere.ToLower().Replace("t1.status", "t2.statusname");
                sql = "select " + outzdmc_tmp + " from    I_M_SB AS t1 LEFT OUTER JOIN zhwx_equipstatus AS t2 ON t1.STATUS = t2.status"
                    + " where 1=1  " + szWhere + " and t1.sfjl='是' and  t1.depcode " + sqlcode + "  and ssdwbh='" + CurrentUser.CompanyCode + "' order by sbbh,depcode";

                

                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                list = CommonService.GetDataTable(sql);

                QtTableToExcel(itemname,template, outzdmcArray, list);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
           
        }


          /// <summary>
        /// Datable导出成Excel(测量设备管理台账和周期溯源计划以及实施记录表)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public void QtTableToExcel(string filename,string template, string[] outzdmcArray, IList<IDictionary<string, string>> list)
        {
            string TempletFileName = Server.MapPath("~/xlstemplate/" + template);      //模板文件  
            string ReportFileName = Server.MapPath("~/xlstemplate/Restlt.xls");    //导出文件  
            FileStream file = null;
            try
            {
                file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                Response.Write("<script>alert('模板文件不存在或正在打开');</script>");
                return;
            }
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet ws = (HSSFSheet)hssfworkbook.GetSheet("Sheet1");
            if (ws == null)//工作薄中没有工作表
            {
                Response.Write("<script>alert('工作薄中没有Sheet1工作表');</script>");
                return;
            }

            int startrownumber = 0;
            int rowNum = ws.LastRowNum;
            for (int i = 0; i < rowNum + 1; i++) {
                HSSFRow tprow = (HSSFRow)ws.GetRow(i);
                if (tprow != null) {
                    int colNum = tprow.LastCellNum;
                    for (int j = 0; j < colNum; j++) {
                        HSSFCell tmpcell = (HSSFCell)tprow.GetCell(j);
                        if (tmpcell == null)
                            continue;
                        HSSFComment comment = (HSSFComment)tmpcell.CellComment;
                        if (comment!=null)
                        {
                            startrownumber = i;
                            tmpcell.CellComment = null;
                            break;
                        }
                    }
                }
            }
            if (startrownumber == 0) {
                startrownumber = rowNum;
            }

            ICellStyle cellStyleBody = hssfworkbook.CreateCellStyle();
            cellStyleBody.BorderBottom = BorderStyle.Thin;
            cellStyleBody.BorderLeft = BorderStyle.Thin;
            cellStyleBody.BorderRight = BorderStyle.Thin;
            cellStyleBody.BorderTop = BorderStyle.Thin;

            cellStyleBody.VerticalAlignment = VerticalAlignment.Center;
            cellStyleBody.Alignment = HorizontalAlignment.Center;

            int count = list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    int _row = i + startrownumber;
                    HSSFRow row = (HSSFRow)ws.CreateRow(_row);
                    row.Height = 500;
                    ICell cell = row.CreateCell(1);
                    cell.CellStyle = cellStyleBody;
                    // row.CreateCell(j).SetCellValue(j);
                    cell.SetCellValue((i+1));
                    for (int j = 0; j < outzdmcArray.Length; j++)
                    {
                        cell = row.CreateCell(j+2);
                        cell.CellStyle = cellStyleBody;
                        // row.CreateCell(j).SetCellValue(j);
                        cell.SetCellValue(list[i][outzdmcArray[j]].GetSafeString());
                    }

                }


                //合并单元格
                int xh = 1;
                int row_start = startrownumber;
                int row_end = startrownumber;
                ICell pxcell = null;
                string temp_ = "";
                for (int i = 0; i < count; i++) {
                    int row_ = i + startrownumber;
                    HSSFRow row = (HSSFRow)ws.GetRow(row_);
                    pxcell = row.GetCell(2);
                    string temp = pxcell.StringCellValue;
                    if (i==0 || !temp_.Equals(temp.GetSafeString()))
                    {
                        if (i != count - 1)
                            temp_ = temp;
                        row_end = row_;

                    }
                    else if (temp_.Equals(temp.GetSafeString()))
                    {
                        if (i == count - 1)
                        {
                            row_end = row_;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    pxcell = row.GetCell(1);
                    pxcell.SetCellValue(xh);
                    xh++;
                    if(i==0)
                        continue;
                    //合并的是前一个的单元格
                    if (i > 0&&i<count) {
                         row_end--;
                    }
                    //最后一个单元格的内容和前一个相同时合并
                    if ((i == count - 1) && temp_.Equals(temp.GetSafeString()))
                    {
                         row_end++;
                    }

                    ws.AddMergedRegion(new CellRangeAddress(row_start, row_end, 1, 1));
                    ws.AddMergedRegion(new CellRangeAddress(row_start, row_end, 2, 2));
                    
                    row_start = row_;
                    
                    
                }

                
            }




            ws.ForceFormulaRecalculation = true;

            using (FileStream filess = System.IO.File.OpenWrite(ReportFileName))
            {
                hssfworkbook.Write(filess);
            }
            System.IO.FileInfo filet = new System.IO.FileInfo(ReportFileName);
            Response.Clear();
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(filename+".xls"));
            Response.AddHeader("Content-Length", filet.Length.ToString());
            Response.ContentType = "application/ms-excel";
            Response.WriteFile(filet.FullName);
            Response.End();
        }
       



        /// <summary>
        /// 转向导出Excel界面
        /// </summary>
        /// <returns></returns>
        public ActionResult exportexcel()
        {
            ViewData["zdzd"] = "";
            IList<string[]> zdzd = new List<string[]>();
            try
            {
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "SELECT     zdmc,zdlx,sy FROM  ZDZD_JC WHERE  (SJBMC = 'I_M_SB') and sfxs='true' order by  xssx asc"; ;
                list = CommonService.GetDataTable(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    zdzd.Add(new string[] { list[i]["zdmc"].GetSafeString(), list[i]["sy"].GetSafeString(), list[i]["zdlx"].GetSafeString() });
                }
                ViewData["zdzd"] = zdzd;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }

            return View("ExportExcel");
        }

        public void doexportexcel()
        {
            string sql = "";
            string usercode = CurrentUser.UserName;
            string zdzd = "";
            string szWhere = "";

            string zdmc = R("zdmc").GetSafeString();
            string sy = R("sy").GetSafeString();
            string zdlx = R("zdlx").GetSafeString();

            string[] zdmcArray = zdmc.Split(',');
            string[] syArray = sy.Split(',');
            string[] zdlxArray = zdlx.Split(',');
            string outzdmc = "";
            string outsy = "";
            for (int i = 0; i < zdmcArray.Length; i++)
            {
                string b = R(zdmcArray[i] + "b").GetSafeString();//是否包含
                if (!b.Equals(""))
                {
                    outzdmc += zdmcArray[i].ToLower() + ",";
                    outsy += syArray[i].ToLower() + ",";
                    string t = R(zdmcArray[i] + "t").GetSafeString();//条件
                    if (zdlxArray[i].Equals("datetime"))
                    {
                        zdzd += " CONVERT(varchar, t1." + zdmcArray[i] + ", 120 ) as " + zdmcArray[i] + ",";
                        string[] tArray = t.Split(',');
                        if (tArray.Length == 2)
                        {
                            if (!tArray[0].GetSafeString().Equals(""))
                            {
                                szWhere += " and CONVERT(varchar, t1." + zdmcArray[i] + ", 23 ) >= '" + tArray[0].GetSafeString() + "'";
                            }

                            if (!tArray[1].GetSafeString().Equals(""))
                            {
                                szWhere += " and CONVERT(varchar, t1." + zdmcArray[i] + ", 23 ) <= '" + tArray[1].GetSafeString() + "'";
                            }
                        }

                    }
                    else
                    {
                        zdzd += " t1." + zdmcArray[i] + ",";
                        string v = R(zdmcArray[i] + "v").GetSafeString();//值
                        switch (t)
                        {
                            case "like":
                                szWhere += " and t1." + zdmcArray[i] + " like '%" + v + "%'";
                                break;
                            case "":
                                break;
                            default:
                                szWhere += " and t1." + zdmcArray[i] + " " + t + " '" + v + "'";
                                break;

                        }

                    }


                }




            }
            zdzd = zdzd.Substring(0, zdzd.Length - 1);
            zdzd = zdzd.ToLower().Replace("t1.status", "t2.statusname as status");
            szWhere = szWhere.ToLower().Replace("t1.status", "t2.statusname");
            sql = "select " + zdzd + " from    I_M_SB AS t1 LEFT OUTER JOIN zhwx_equipstatus AS t2 ON t1.STATUS = t2.status"
                + " where (t1.LRRZH = '" + usercode + "' or 1=1 )  " + szWhere;

            sql += " and ssdwbh='" + CurrentUser.CompanyCode + "' ";

            IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            list = CommonService.GetDataTable(sql);


            string[] outzdmcArray = outzdmc.Substring(0, outzdmc.Length - 1).Split(',');
            string[] outsyArray = outsy.Substring(0, outsy.Length - 1).Split(',');
            TableToExcel(outsyArray, outzdmcArray, list);

        }


        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public void TableToExcel(string[] outsyArray, string[] outzdmcArray, IList<IDictionary<string, string>> list)
        {
            IWorkbook workbook;
            string fileExt = ".xls";
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            if (workbook == null) { return; }
            ISheet sheet = string.IsNullOrEmpty("Sheet1") ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet("Sheet1");

            //表头  
            IRow row = sheet.CreateRow(0);

            row.Height = 500;

            ICellStyle cellStyleHead = workbook.CreateCellStyle();
            IFont font12 = workbook.CreateFont();
            font12.FontHeightInPoints = 11;
            font12.FontName = "微软雅黑";
            cellStyleHead.SetFont(font12);
            cellStyleHead.VerticalAlignment = VerticalAlignment.Center;
            cellStyleHead.Alignment = HorizontalAlignment.Center;

            ICellStyle cellStyleBody = workbook.CreateCellStyle();
            cellStyleBody.VerticalAlignment = VerticalAlignment.Center;
            cellStyleBody.Alignment = HorizontalAlignment.Center;


            for (int i = 0; i < outsyArray.Length; i++)
            {
                sheet.SetColumnWidth(i, 5000);//设置宽度
                ICell cell = row.CreateCell(i);
                cell.CellStyle = cellStyleHead;
                cell.SetCellValue(outsyArray[i]);
            }


            //数据  
            for (int i = 0; i < list.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < list[i].Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.CellStyle = cellStyleBody;
                    cell.SetCellValue(list[i][outzdmcArray[j]].GetSafeString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            //  using (FileStream fs = new FileStream("123.xls", FileMode.Create, FileAccess.Write))
            //   {
            //       fs.Write(buf, 0, buf.Length);
            //       fs.Flush();
            //  }

            Response.Clear();
            Response.Buffer = true;
            // Response.Charset = Encoding.UTF8.BodyName;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=DeviceList" + fileExt);
            // Response.ContentEncoding = Encoding.UTF8;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/vnd.ms-excel;";
            workbook.Write(Response.OutputStream);
            Response.End();
        }





        #endregion







        #region 手机操作


        /// <summary>
        /// 百度短信得到验证码
        /// </summary>
        public void getidentifycode()
        {
            string retMsg = "";
            string sql = "";
            Boolean flag = false;
           // Boolean ret = false;
           // string err = "";
            try
            {
                string realphone = HUR("realphone").GetSafeString();
                //string usercode = HUR("usercode").ToLower();
                //string password = HUR("password");
                realphone = realphone.Substring(0, (realphone.Length >= 11 ? 11 : realphone.Length));


                sql = "select sum(yzmtotal) as zs from  zhwx_bd_message_total where phone= '" + realphone + "'";
                IList<IDictionary<string, string>> listyzmtotal = new List<IDictionary<string, string>>();
                listyzmtotal = CommonService.GetDataTable(sql);
                if (listyzmtotal[0]["zs"].GetSafeInt() <= 4)
                {
                    //string yzphonegsd = getPost("https://tcc.taobao.com/cc/json/mobile_tel_segment.htm?tel=" + realphone, "");
                    //string[] yzphonegsd_sz = yzphonegsd.Split('=');
                    //if (yzphonegsd_sz.Length == 2)
                    //{
                    //    JObject yzphonegsdjo = JObject.Parse(yzphonegsd_sz[1]);
                    //    if (yzphonegsdjo["province"].GetSafeString().Equals("浙江"))
                    //    {
                    //        flag = true;
                    //    }

                    //}
                    flag = true;
                }
                //ret = Remote.UserService.Login(usercode, password, out err);
                if (flag)
                {
                    flag = false;
                    int messages_count = getValue(bd_wx_login_verifycode_count_).GetSafeInt();
                    if (IsMobilePhone(realphone))
                    {
                        IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                        IList<string> sqls = new List<string>();

                        sql = "select * from  zhwx_bd_message_total where phone= '" + realphone + "'";
                        list = CommonService.GetDataTable(sql);
                        if (list.Count <= 0)
                        {
                            sql = "insert into  zhwx_bd_message_total(phone, yzmday, total, yzmtotal) values ('" + realphone + "',0,0,0)";
                            sqls.Add(sql);
                            flag = CommonService.ExecTrans(sqls);
                        }
                        if (flag || list[0]["yzmday"].GetSafeInt() < messages_count)
                        {
                            sql = "select count(*) as num from  zhwx_user where phone= '" + realphone + "'";
                            // IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                            list = CommonService.GetDataTable(sql);
                            if (list[0]["num"].GetSafeInt() == 0)
                            {
                                sqls.Clear();
                                string code = "";
                                Random rd = new Random();
                                int codelength = int.Parse(getValue(bd_wx_login_verifycode_length_));
                                code = rd.Next((int)Math.Pow(10, codelength - 1), (int)Math.Pow(10, codelength)).GetSafeString();
                                code = code.GetSafeString().PadLeft(codelength, '0');
                                sql = "delete from zhwx_yzm where phone = '" + realphone + "'";
                                sqls.Add(sql);
                                sql = "insert into  zhwx_yzm(phone,yzm,datetime) values ( '" + realphone + "','" + code + "', CONVERT(varchar,  getdate(), 120 )) ";
                                sqls.Add(sql);
                                sql = "update zhwx_bd_message_total set yzmday = yzmday+1, total = total+1, yzmtotal = yzmtotal+1 where phone = '" + realphone + "'";
                                sqls.Add(sql);
                                flag = false;

                                ///////////////////////临时使用//////////////////////////////////////////////
                               // string unitname = getValue(wx_push_unitname_).GetSafeString();
                              //  string time = getValue(bd_wx_login_verifycode_time_).GetSafeString().GetSafeString();
                              //  string info = "验证码是" + code + ",有效期是" + time + "分钟。";
                              //  flag = bduniversalpush(realphone, unitname + "注册", info);
                                
                                /////////////////////////////////////////////////////////////////////
                                flag = bdverifycodepush(realphone, code,"设备管理");
                                if (flag)
                                {
                                    flag = CommonService.ExecTrans(sqls);
                                    if (flag)
                                    {
                                        //推送消息
                                        retMsg = "{\"success\":true,\"code\":\"" + code + "\"}";
                                    }
                                    else
                                    {
                                        retMsg = "{\"success\":false,\"msg\":\"验证码发送失败error1!\"}";
                                    }
                                }
                                else
                                {
                                    retMsg = "{\"success\":false,\"msg\":\"验证码发送失败error2!\"}";
                                }


                            }
                            else
                            {
                                retMsg = "{\"success\":false,\"msg\":\"该手机号已被绑定!!\"}";
                            }
                        }
                        else
                        {
                            retMsg = "{\"success\":false,\"msg\":\"" + ((list.Count > 0 && list[0]["yzmday"].GetSafeInt() < messages_count) ? "手机验证码获取失败!" : "您今天验证码发送已到" + messages_count + "条，请明天再试!") + "\"}";
                        }



                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"手机号码错误!\"}";
                    }

                }
                else
                {
                   // retMsg = "{\"success\":false,\"msg\":\"请先输入正确的用户名和密码!\"}";
                    retMsg = "{\"success\":false,\"msg\":\"无法获取验证码!\"}";
                    SysLog4.WriteError("无法获取验证码,非法号码:" + realphone);

                }
            }

            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"验证码获取失败!\"}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }


        }


        /// <summary>
        /// 验证短信二维码
        /// </summary>
        public JObject checkidentifycode(string identifycode, string realphone)
        {
            string retMsg = "";
            JObject jo = null;
            try
            {
                //  string identifying = HUR("identifying").GetSafeString();
                //string realphone = HUR("realphone").GetSafeString();

                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "";
                sql = "select DATEDIFF(n,datetime, getdate() ) as jg  from  zhwx_yzm where phone= '" + realphone + "' and yzm = '" + identifycode + "'";
                list = CommonService.GetDataTable(sql);
                int bd_wx_login_verifycode_time = int.Parse(getValue(bd_wx_login_verifycode_time_));

                if (list.Count > 0)
                {
                    if (list[0]["jg"].GetSafeInt() <= bd_wx_login_verifycode_time)
                    {
                        IList<string> sqls = new List<string>();
                        sql = "delete from zhwx_yzm where phone = '" + realphone + "'";
                        sqls.Add(sql);
                        CommonService.ExecTrans(sqls);
                        retMsg = "{\"success\":true,\"msg\":\"验证码正确\"}";

                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"验证码已失效!\"}";
                    }

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"验证码不存在!\"}";
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"注册异常，注册失败!\"}";
                jo = JObject.Parse(retMsg.GetSafeString());
                return jo;
            }

            finally
            {
                jo = JObject.Parse(retMsg.GetSafeString());
            }

            return jo;
        }


        /// <summary>
        /// 绑定微信
        /// </summary>
        public void bindwx()
        {
            string err = "";
            bool ret = false;
            string retMsg = "";
            JObject openidjb = null;
            JObject yzmjo = null;
            string url = getValue(wx_login_url_);
            string appid = getValue(wx_appid_);
            string secret = getValue(wx_secret_);
            string grant_type = getValue(wx_login_grant_type_);
            string data = "";
            try
            {
                string usercode = HUR("usercode").ToLower();
                string password = HUR("password");
                string code = HUR("code");

                string identifycode = HUR("identifycode");
                string realphone = HUR("realphone");

                string isreturnopenid = HUR("isreturnopenid");

                yzmjo = checkidentifycode(identifycode, realphone);
                if (yzmjo["success"].GetSafeBool())
                {

                    data = "appid=" + appid + "&secret=" + secret
                       + "&js_code=" + code + "&grant_type=" + grant_type;
                    ret = Remote.UserService.Login(usercode, password, out err);
                    if (ret)
                    {
                        string sql = "";
                        sql = "select openid from  zhwx_user where usercode= '" + usercode + "'";
                        IList<IDictionary<string, string>> userlist = new List<IDictionary<string, string>>();
                        userlist = CommonService.GetDataTable(sql);
                        if (userlist.Count > 0)
                        {
                            retMsg = "{\"success\":false,\"msg\":\"该用户已经绑定微信账户!\"}";
                        }
                        else
                        {
                            openidjb = JObject.Parse(getPost(url, data));
                            string openid = openidjb["openid"].GetSafeString();
                            string session_key = openidjb["session_key"].GetSafeString();
                            if (openid == null || openid.Equals(""))
                            {
                                retMsg = "{\"success\":false,\"msg\":\"绑定失败,找不到对应的openid，请稍后重试!\"}";
                            }
                            else
                            {
                                sql = "select openid from  zhwx_user where usercode= '" + usercode + "' or openid = '" + openid + "'";

                                userlist = CommonService.GetDataTable(sql);
                                if (userlist.Count > 0)
                                {
                                    retMsg = "{\"success\":false,\"msg\":\"该微信账户已经绑定用户!\"}";
                                }
                                else
                                {
                                    RemoteUserService.Services srv = new RemoteUserService.Services();
                                    srv.CookieContainer = CurrentUser.CurContainer;
                                    RemoteUserService.SUser user = srv.GetUserInfo("", usercode);
                                     
                                    Boolean flag = false;
                                    string uuid = Guid.NewGuid().ToString("N");
                                    IList<string> sqls = new List<string>();
                                    sqls.Add("insert into  zhwx_user (uuid,usercode,openid,session_key,phone,cpcode) values ('" + uuid + "','" + usercode + "','" + openid + "','" + session_key + "','" + realphone + "','" + user.CPCODE + "') ");
                                    flag = CommonService.ExecTrans(sqls);
                                    if (flag)
                                    {
                                        if (!isreturnopenid.Equals("1"))
                                            openid = "";
                                        retMsg = "{\"success\":true,\"code\":\"" + usercode + "\",\"openid\":\"" + openid + "\"}";
                                    }
                                    else
                                    {
                                        retMsg = "{\"success\":false,\"msg\":\"服务器操作失败，请稍后重试!\"}";
                                    }
                                }
                            }

                        }


                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"该用户不存在或密码错误，绑定失败!\"}";
                    }

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"" + yzmjo["msg"].GetSafeString() + "\"}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"注册异常，注册失败!\"}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }


        }






        /// <summary>
        /// 解绑微信
        /// </summary>
        public void unbindwx()
        {

            string retMsg = "";

            try
            {
                IList<string> sqls = new List<string>();
                string loginsession = HUR("loginsession");
                Boolean flag;
                string sql = "delete from zhwx_user where usercode = '" + loginsession + "'";
                sqls.Add(sql);
                flag = CommonService.ExecTrans(sqls);
                if (flag)
                {
                    retMsg = "{\"success\":true,\"msg\":\"解绑成功!\"}";
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"解绑失败!\"}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"解绑异常，解绑失败!\"}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }


        }


        /// <summary>
        /// 登录
        /// </summary>
        public void login()
        {
            string retMsg = "";
            string url = getValue(wx_login_url_);
            string appid = getValue(wx_appid_);
            string secret = getValue(wx_secret_);
            string grant_type = getValue(wx_login_grant_type_);
            String data = "";
            String openid = "";
            String session_key = "";
            JObject openidjb = null;
            String sql = "";

            try
            {
                IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                string code = HUR("code");
                data = "appid=" + appid + "&secret=" + secret
                   + "&js_code=" + code + "&grant_type=" + grant_type;
                openidjb = JObject.Parse(getPost(url, data));
                openid = openidjb["openid"].GetSafeString();
                session_key = openidjb["session_key"].GetSafeString();
                if (openid == null || openid.Equals(""))
                {
                    retMsg = "{\"success\":false,\"msg\":\"" + openidjb["errcode"] + "\"}";
                }
                else
                {
                    sql = "select usercode from zhwx_user where openid ='" + openid + "'";
                    ret = CommonService.GetDataTable(sql);
                    if (ret.Count > 0)
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("update zhwx_user set session_key = '" + session_key + "' where  openid = '" + openid + "' ");
                        CommonService.ExecTrans(sqls);
                        retMsg = "{\"success\":true,\"code\":\"" + ret[0]["usercode"].GetSafeString() + "\"}";
                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"404\"}";
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"登录异常，登录失败!\"}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }


        /// <summary>
        /// 设备台账列表
        /// </summary>
        public void equipsearch()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                string loginsession = HUR("loginsession");

                string syfs = HUR("syfs");
                string depname = HUR("depname");
                string sbbh = HUR("sbbh");
                string sbmc = HUR("sbmc");
                string sbxh = HUR("sbxh");
                string sfjl = HUR("sfjl");
                int status = HURI("status");


                string date1 = HUR("date1");
                string time1 = HUR("time1");
                string date2 = HUR("date2");
                string time2 = HUR("time2");


                int page = HURI("page");
                flag = isRegister(loginsession);
                if (flag)
                {

                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);

                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    string sql = "";
                    string sqlWhere = "";
                    int totalCount;
                    /*
                    flag = isSbgly(loginsession);
                    if (flag)
                    {
                        sqlWhere += "and 1=1 ";
                    }
                    else
                    {
                        sqlWhere += "and t1.lrrzh = '" + loginsession + "'";
                    }
                    */

                    if (syfs != null && syfs != "")
                    {
                        sqlWhere += " and t1.syfs like '%" + syfs + "%' ";
                    }
                    if (depname != null && depname != "")
                    {
                        sqlWhere += " and t1.depname like '%" + depname + "%' ";
                    }
                    if (sbbh != null && sbbh != "")
                    {
                        sqlWhere += " and t1.sbbh like '%" + sbbh + "%' ";
                    }
                    if (sbmc != null && sbmc != "")
                    {
                        sqlWhere += " and t1.sbmc like '%" + sbmc + "%' ";
                    }
                    if (sbxh != null && sbxh != "")
                    {
                        sqlWhere += " and t1.sbxh like '%" + sbxh + "%' ";
                    }
                    if (sfjl != null && sfjl != "")
                    {
                        sqlWhere += " and t1.sfjl = '" + sfjl + "' ";
                    }
                    if (status != 0)
                    {
                        sqlWhere += " and t1.status = " + status + " ";
                    }
                    else
                    {
                        sqlWhere += " and t1.status <> 4 ";
                    }

                    if (date1 != null && date1 != "")
                    {
                        string datetime1 = "";
                        if (time1 != null && time1 != "")
                        {
                            datetime1 = date1 + " " + time1 + ":00";
                        }
                        else
                        {
                            datetime1 = date1 + " " + "00:00:00";
                        }
                        sqlWhere += " and t1.XCBDRQ >= '" + datetime1 + "' ";
                    }
                    if (date2 != null && date2 != "")
                    {
                        string datetime2 = "";
                        if (time2 != null && time2 != "")
                        {
                            datetime2 = date2 + " " + time2 + ":00";
                        }
                        else
                        {
                            datetime2 = date2 + " " + "23:59:59";
                        }
                        sqlWhere += " and t1.XCBDRQ <= '" + datetime2 + "' ";
                    }

                    sql = "select t1.recid as id ,sbmc,ssdwmc,t2.statusname,t1.status,depname  from I_M_SB t1 left join  zhwx_equipstatus t2 "
                        + " on t1.status = t2.status where t1.ssdwbh='" + user.CPCODE + "'  " + sqlWhere + " order by t1.recid  ";
                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }

        public void getequipdetails()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                string equipid = HUR("id");

                flag = isRegister(loginsession);
                if (flag)
                {

                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);

                    IList<IDictionary<string, string>> zdzdret = new List<IDictionary<string, string>>();
                    IList<string[]> zdzdlist = new List<string[]>();
                    string sql = "SELECT     zdmc,zdlx,sy FROM  ZDZD_JC WHERE  (SJBMC = 'I_M_SB') and wsfxs='true' order by  wxssx asc";
                    string zd = "";
                    zdzdret = CommonService.GetDataTable(sql);
                    for (int i = 0; i < zdzdret.Count; i++)
                    {
                        zdzdlist.Add(new string[] { zdzdret[i]["zdmc"].GetSafeString().ToLower(), zdzdret[i]["zdlx"].GetSafeString().ToLower(), zdzdret[i]["sy"].GetSafeString() });
                        zd += "t1." + zdzdret[i]["zdmc"].GetSafeString().ToLower() + ",";
                    }
                    // zd = zd.Substring(0, zd.Length - 1);
                    sql = "select top 1 " + zd + " t2.statusname from I_M_SB t1 left join  zhwx_equipstatus t2 "
                        + " on t1.status = t2.status where t1.ssdwbh='" + user.CPCODE + "'  and t1.recid = '" + equipid + "'";
                    IList<IDictionary<string, string>> equipret = new List<IDictionary<string, string>>();
                    equipret = CommonService.GetDataTable(sql);
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    for (int i = 0; i < equipret.Count; i++)
                    {

                        for (int j = 0; j < zdzdlist.Count; j++)
                        {
                            Dictionary<string, string> myDic = new Dictionary<string, string>();
                            if (zdzdlist[j][0].GetSafeString().Equals("status"))
                            {

                                myDic.Add("id", System.Guid.NewGuid().ToString("N"));
                                myDic.Add("name", zdzdlist[j][2].GetSafeString());
                                myDic.Add("value", equipret[i]["statusname"].GetSafeString());
                                ret.Add(myDic);
                            }
                            else
                            {

                                myDic.Add("id", System.Guid.NewGuid().ToString("N"));
                                myDic.Add("name", zdzdlist[j][2].GetSafeString());
                                myDic.Add("value", equipret[i][zdzdlist[j][0].GetSafeString()].GetSafeString());
                                ret.Add(myDic);
                            }

                        }
                    }
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"data\":" + jss.Serialize(ret) + "}";
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }



        /// <summary>
        /// 扫描二维码，并上传照片，获取items列表
        /// </summary>
        public void scancode()
        {
            string retMsg = "";
            IList<IList<string>> itemslist = new List<IList<string>>();
            // IList<string> items = new List<string>();
            // IList<string> itemssname = new List<string>();
            string err = "";
            string sql = "";
            string uuid = Guid.NewGuid().ToString("N");
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                string scancode = HUR("scancode");

                flag = isRegister(loginsession);
                if (flag)
                {
                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);
                    RemoteUserService.VCompany vcompany = srv.GetCompanyByCpcode(user.CPCODE);
                    RemoteUserService.VDep vdep = srv.GetDepByCode(user.DEPCODE);

                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    sql = "select * from I_M_SB where  ssdwbh = '" + user.CPCODE + "'  and  recid = '" + scancode + "'";
                    ret = CommonService.GetDataTable(sql);
                    if(ret.Count<=0){
                        retMsg = "{\"success\":false,\"msg\":\"设备二维码无效!\"}";
                        return;
                    }

                    IList<IDictionary<string, string>> waitrecordret = new List<IDictionary<string, string>>();
                    sql = "select * from zhwx_waitreturn where equipid = '" + scancode + "'";
                    waitrecordret = CommonService.GetDataTable(sql);
                    if (waitrecordret.Count > 0)
                    {
                        retMsg = "{\"success\":false,\"msg\":\"该设备正在等待确认中,无法进行其他操作!\"}";
                    }
                    else
                    {


                        retMsg = "{\"success\":false,\"msg\":\"上传文件为空\"}";
                        for (int i = 0; i < Request.Files.Count;i++ )
                        {
                            HttpPostedFileBase postfile = Request.Files[i];
                            // postfile.SaveAs("F:/1/c/" + postfile.FileName);
                            // 读取文件
                            byte[] attach = new byte[postfile.ContentLength];
                            int readlength = 0;
                            while (readlength < postfile.ContentLength)
                            {
                                int tmplen = postfile.InputStream.Read(attach, readlength, postfile.ContentLength - readlength);
                                readlength += tmplen;
                            }

                            string width = "0";
                            string height = "0";
                            byte[] thumbattach = null;
                            string attachtype = "";
                            string attachname = uuid + "." + postfile.FileName.Substring(postfile.FileName.LastIndexOf(".") + 1);

                            MyImage img = new MyImage(attach);

                            if (img.IsImage())
                            {
                                System.Drawing.Image tempimage = System.Drawing.Image.FromStream(postfile.InputStream, true);
                                width = tempimage.Width.GetSafeString();//宽
                                height = tempimage.Height.GetSafeString();//高
                                thumbattach = img.ConvertToJpg(100, 100);
                                attachtype = "img";
                            }

                            string fqz_ompanycode = "";
                            string fqz_ompanyname = "";
                            string fqz_username = "";
                            string fqz_depcode = "";
                            string fqz_depname = "";

                            //IList<BD.WorkFlow.DataModal.VitrualEntities.VCompany> fqzusers1 = new List<BD.WorkFlow.DataModal.VitrualEntities.VCompany>();
                            //fqzusers1 = RemoteUserService.GetFlowCompanys(loginsession);
                            //fqz_ompanycode = fqzusers1[0].CompanyId;
                            //fqz_ompanyname = fqzusers1[0].CompanyName;

                            //BD.WorkFlow.DataModal.VitrualEntities.VUser fqzusers2 = new BD.WorkFlow.DataModal.VitrualEntities.VUser();
                            //fqzusers2 = RemoteUserService.GetUser(loginsession);
                            //fqz_username = fqzusers2.MyGetObjectProperty("UserRealName");



                            fqz_ompanycode = user.CPCODE;
                            fqz_ompanyname = vcompany.CPNAME;
                            fqz_username = user.REALNAME;

                            fqz_depcode = vdep.DEPCODE;
                            fqz_depname = vdep.DEPNAME;


                            IList<IDictionary<string, string>> sbret = new List<IDictionary<string, string>>();
                            string ssdwbh = "";
                            string lrrzh = "";
                            string status = "";
                            string statusname = "";

                            sql = "select t1.ssdwbh, t1.lrrzh,t1.status,t2.statusname  from  I_M_SB t1 left join zhwx_equipstatus t2 on t1.status=t2.status where t1.recid = '" + scancode + "'";
                            sbret = CommonService.GetDataTable(sql);
                            if (sbret.Count <= 0)
                            {
                                retMsg = "{\"success\":false,\"msg\":\"设备不存在!\"}";
                                break;
                            }
                            else
                            {
                                ssdwbh = sbret[0]["ssdwbh"].GetSafeString();
                                lrrzh = sbret[0]["lrrzh"].GetSafeString();
                                status = sbret[0]["status"].GetSafeString();
                                statusname = sbret[0]["statusname"].GetSafeString();
                            }

                            IList<IDictionary<string, string>> statusret = new List<IDictionary<string, string>>();
                            IList<string[]> statusarray = new List<string[]>();
                            sql = "select status,statusname,toact from  zhwx_equipstatus ";
                            statusret = CommonService.GetDataTable(sql);
                            for (int j = 0; j < statusret.Count; j++)
                            {
                                statusarray.Add(new string[] { statusret[j]["status"].GetSafeString(), statusret[j]["statusname"].GetSafeString(), statusret[j]["toact"].GetSafeString() });
                            }

                            flag = isSbgly(loginsession);
                            if (loginsession.Equals(lrrzh) || flag)
                            {

                                //发起人就是设备保管员
                                itemslist = getItemsList(itemslist, statusarray, status, 0, 2, 0);

                            }
                            else
                            {
                                //发起人非设备保管员
                                if (ssdwbh.Equals(fqz_ompanycode))
                                {
                                    //同一个单位
                                    itemslist = getItemsList(itemslist, statusarray, status, 0, 2, 1);
                                }
                                else
                                {
                                    //不同单位
                                    itemslist = getItemsList(itemslist, statusarray, status, 0, 2, 2);
                                }
                            }

                            if (itemslist.Count != 2 || itemslist[0].Count == 0)
                            {
                                retMsg = "{\"success\":false,\"msg\":\"设备" + statusname + "中!\"}";
                                break;
                            }


                            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string sqlstr = "INSERT INTO zhwx_equiprecord_tmp (uuid,equipid,handler,name,unitid,unitname,attachname,attachwidth,attachheight,attach,thumbattach,attachtype,datetime,depcode,depname)VALUES(@uuid,@equipid,@handler,@name,@unitid,@unitname,@attachname,@attachwidth,@attachheight,@attach,@thumbattach,@attachtype,@datetime,@depcode,@depname)";
                            IList<IDataParameter> sqlparams = new List<IDataParameter>();
                            IDataParameter sqlparam = new SqlParameter("@uuid", uuid);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@equipid", scancode);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@handler", loginsession);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@name", fqz_username);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@unitid", fqz_ompanycode);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@unitname", fqz_ompanyname);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@attachname", attachname);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@attachwidth", width);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@attachheight", height);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@attach", attach);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@thumbattach", thumbattach);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@attachtype", attachtype);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@datetime", datetime);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@depcode", fqz_depcode);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@depname", fqz_depname);
                            sqlparams.Add(sqlparam);

                            flag = CommonService.ExecTrans(sqlstr, sqlparams, out err);
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            retMsg = "{\"success\":true,\"uuid\":\"" + uuid + "\",\"equipid\":\"" + scancode + "\",\"status\":" + jss.Serialize(itemslist[0]) + ",\"act\":" + jss.Serialize(itemslist[1]) + "}";

                            break;
                        }


                    }
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }



        /// <summary>
        /// 操作设备
        /// </summary>
        public void doEquipAct()
        {

            //insert into zhwx_equiprecord_tmp (attach,thumbattach)  ( select attach,thumbattach from zhwx_equiprecord )
            string retMsg = "";
            string sql = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");

                string status = HUR("act");
                string act = HUR("actname");
                string uuid = HUR("recordid");//tmp表中的uuid
                string equipid = HUR("recordequipid");//扫描二维码的值，i_m_sb中的recid

                status = status.Substring(0, 1);//只取第一个动作作为设备状态

                flag = isRegister(loginsession);
                if (flag)
                {


                    IList<IDictionary<string, string>> statusret = new List<IDictionary<string, string>>();
                    sql = "select sfqr,sfmylendequips from zhwx_equipstatus where status = '" + status + "'";
                    statusret = CommonService.GetDataTable(sql);
                    string isqr = statusret[0]["sfqr"].GetSafeString();
                    string sfmylendequips = statusret[0]["sfmylendequips"].GetSafeString();

                    string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    IList<string> sqls = new List<string>();

                    sql = "insert into zhwx_equiprecord (equipid,handler,name,unitid,unitname,datetime,attachname,attachwidth,attachheight,attach,thumbattach,attachtype,act,status,depcode,depname) "
                               + " ( select equipid,handler,name,unitid,unitname,CONVERT(varchar, getdate(), 120 ),attachname,attachwidth,attachheight,attach,thumbattach,attachtype,'" + act + "','" + status + "',depcode,depname from zhwx_equiprecord_tmp where uuid = '" + uuid + "')";
                    sqls.Add(sql);
                    sql = "update i_m_sb set status ='" + status + "' where recid = '" + equipid + "'";
                    sqls.Add(sql);
                    sql = "delete zhwx_equiprecord_tmp  where uuid = '" + uuid + "'";
                    sqls.Add(sql);
                    sql = "delete zhwx_mylendequips   where equipid = '" + equipid + "'";
                    sqls.Add(sql);
                    if (sfmylendequips.Equals("1"))//是否加入我的设备列表
                    {

                        RemoteUserService.Services srv = new RemoteUserService.Services();
                        srv.CookieContainer = CurrentUser.CurContainer;
                        RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);
                        RemoteUserService.VCompany vcompany = srv.GetCompanyByCpcode(user.CPCODE);

                        sql = "insert zhwx_mylendequips (usercode,equipid,username,datetime) values ('" + loginsession + "','" + equipid + "','" + user.REALNAME + "',CONVERT(varchar, getdate(), 120 ))";
                        sqls.Add(sql);
                    }


                    string pusher = "";
                    string pushername = "";
                    string presfqr = "";
                    string phone = "";
                    string pushstatusname = "";
                    string pushsbmc = "";
                    IList<IDictionary<string, string>> lastuser = new List<IDictionary<string, string>>();
                    sql = "select top 1 t1.handler,t1.name,t2.presfqr,t3.phone,t2.statusname,t4.sbmc "
                        + "  from zhwx_equiprecord t1 left join zhwx_equipstatus t2 "
                        + " on t1.status = t2.status left join zhwx_user t3  on t1.handler = t3.usercode "
                        + " left join i_m_sb t4 on t1.equipid = t4.recid "
                        + " where t1.equipid = '" + equipid + "'  order by t1.datetime desc";
                    lastuser = CommonService.GetDataTable(sql);
                    if (lastuser.Count == 1)
                    {
                        pusher = lastuser[0]["handler"];
                        pushername = lastuser[0]["name"];
                        presfqr = lastuser[0]["presfqr"];
                        phone = lastuser[0]["phone"];
                        pushstatusname = lastuser[0]["statusname"];
                        pushsbmc = lastuser[0]["sbmc"];
                    }
                    else
                    {
                        pusher = "null";
                        pushername = "null";
                    }

                    if (isqr.Equals("1") && presfqr.Equals("1"))//需要确认
                    {

                        IList<string> wait_sqls = new List<string>();
                        for (int j = 0; j < sqls.Count; j++)
                        {
                            sql = "insert into zhwx_waitreturn(recid,equipid,sql,sort) values ('" + Guid.NewGuid().ToString("N") + "','" + equipid + "','" + sqls[j].Replace("'", "''") + "'," + (j + 1) + ")";
                            wait_sqls.Add(sql);
                        }
                        sql = "insert into zhwx_pushmessages(title,context,pusher,pushername,datetime,type,issure,isneedsure,isread,ispush,equipid)"
                            + " values ('" + act + "确认','已归还设备,需要您的确认!','" + pusher + "','" + pushername + "',CONVERT(varchar, getdate(), 120 ),'1','0','1','0','1','" + equipid + "')";
                        wait_sqls.Add(sql);
                        flag = CommonService.ExecTrans(wait_sqls);
                        if (flag)
                        {
                            //////////////////////////临时使用////////////////////////////////
                            string unitname = getValue(wx_push_unitname_).GetSafeString();
                            string time = getValue(bd_wx_login_verifycode_time_).GetSafeString().GetSafeString();
                            string info = pushsbmc + "归还需要您的确认，请尽快";
                        //    flag = bduniversalpush(phone, unitname + "设备确认", info);
                            //////////////////////////////////////////////////////////

                            flag = bdequipstatuspush(phone, pushsbmc, pushstatusname, "管理员已归还设备");

                            if (flag)
                            {
                                sqls.Clear();
                                sql = "update zhwx_bd_message_total set  total = total+1  where phone = '" + phone + "'";
                                sqls.Add(sql);
                                CommonService.ExecTrans(sqls);
                                retMsg = "{\"success\":true,\"msg\":\"操作成功,等待确认即可完成" + act + "\"}";
                            }
                            else
                            {
                                retMsg = "{\"success\":false,\"msg\":\"消息推送失败,请尽快通知相关人员" + act + "设备\"}";
                            }

                        }
                        else
                        {
                            retMsg = "{\"success\":false,\"msg\":\"" + act + "失败!\"}";
                        }
                    }
                    else
                    {
                        flag = CommonService.ExecTrans(sqls);
                        if (flag)
                        {
                            if (presfqr.Equals("1"))
                            {
                               // string unitname = getValue(wx_push_unitname_).GetSafeString();
                              //  string info = pushsbmc + "已" + act;
                              //  flag = bduniversalpush(phone, unitname + "设备确认", info);
                                //////////////////////////////////////////////////////////

                                flag = bdequipstatuspush(phone, pushsbmc, pushstatusname, "管理员已归还设备");

                                if (flag)
                                {

                                    sqls.Clear();
                                    sql = "update zhwx_bd_message_total set  total = total+1  where phone = '" + phone + "'";
                                    sqls.Add(sql);
                                    CommonService.ExecTrans(sqls);
                                    retMsg = "{\"success\":true,\"msg\":\"" + act + "成功!\"}";
                                }
                                else
                                {
                                    retMsg = "{\"success\":false,\"msg\":\"消息推送失败,请尽快通知相关人员已" + act + "设备\"}";
                                }
                            }
                            else
                            {
                                retMsg = "{\"success\":true,\"msg\":\"" + act + "成功!\"}";
                            }

                        }
                        else
                        {
                            retMsg = "{\"success\":false,\"msg\":\"" + act + "失败!\"}";
                        }
                    }


                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }

        /// <summary>
        /// 获取图片二进制流
        /// </summary>
        public void getPic()
        {
            byte[] ret = null;
            string zd = "";
            string err = "";
            string attachname = "";

            try
            {
                /*
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);*/
                string id = HUR("id");
                string thumb = HUR("thumb");

                if (thumb.Equals("1"))
                {
                    zd = "thumbattach";
                }
                else
                {
                    zd = "attach";
                }
                //int fileid = 0;
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("select attachname," + zd + " as attach from zhwx_equiprecord where recid =" + id);
                if (dt.Count > 0)
                {
                    ret = dt[0]["attach"] as byte[];
                    attachname = dt[0]["attachname"] as string;

                    Response.Clear();
                    Response.ContentType = "image/jpeg jpeg jpg jpe";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(attachname));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
            }
        }



        /// <summary>
        /// 获取设备状态列表
        /// </summary>
        public void statusTracking()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                string loginsession = HUR("loginsession");

                string equipid = HUR("equipid");
                string datetime = HUR("datetime");


                int page = HURI("page");
                flag = isRegister(loginsession);
                if (flag)
                {

                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);


                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    string sql = "";
                    int totalCount;

                    if (datetime == null || datetime == "")
                    {
                        datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (page <= 1)
                    {
                        page = 1;
                    }

                  
                    sql = "select t1.recid as id,t1.name,t1.unitname,CONVERT(varchar, t1.datetime, 120 ) as datetime,t1.act,t2.statusname,t3.sbmc,t1.depname "
                        + " from  zhwx_equiprecord t1 left join zhwx_equipstatus t2 on t1.status = t2.status left join i_m_sb t3 on t1.equipid = t3.recid "
                        + " where t3.ssdwbh='" + user.CPCODE + "' and  equipid = '" + equipid + "' and t1.datetime<='" + datetime + "' order by t1.datetime desc  ";
                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"datetime\":\"" + datetime + "\",\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }




        /// <summary>
        /// 获取我的设备列表
        /// </summary>
        public void getMyEquipList()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                int page = HURI("page");
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                int totalCount;

                flag = isRegister(loginsession);
                if (flag)
                {

                    if (page <= 1)
                    {
                        page = 1;
                    }

                    string sql = "";
                    sql = "select t1.recid as id,t1.equipid,t2.sbmc,t2.ssdwmc,t2.depname from zhwx_mylendequips t1 "
                        + " left join i_m_sb t2 on t1.equipid = t2.recid where t1.usercode = '" + loginsession + "' order by t1.recid";
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    // ret = CommonService.GetDataTable(sql);
                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";


                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }


        /*由于小程序上面多页显示有问题，暂时停用
        /// <summary>
        /// 获取我的借出设备列表
        /// </summary>
        public void getMyLoanEquipList()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                int page = HURI("page");
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                int totalCount;

                flag = isRegister(loginsession);
                if (flag)
                {

                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);

                    if (page <= 1)
                    {
                        page = 1;
                    }

                    string sqlWhere = "";
                    flag = isSbgly(loginsession);
                    if (flag)
                    {
                        sqlWhere += " 1=1 ";
                    }
                    else
                    {
                        sqlWhere += " lrrzh = '" + loginsession + "'";
                    }

                    string sql = "";
                    sql = "select usercode,username,count(usercode) as num from  zhwx_mylendequips  "
                        + " where equipid in (select recid from i_m_sb where ssdwbh='" + user.CPCODE + "'  " + sqlWhere + " )  group by usercode,username ";
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    // ret = CommonService.GetDataTable(sql);
                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";


                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }
        */


        /// <summary>
        /// 获取我的借出设备列表
        /// </summary>
        public void getMyLoanEquipList()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                int page = HURI("page");
            //    int pagsize = getValue(wx_pagesize_).GetSafeInt();
                int totalCount;

                flag = isRegister(loginsession);
                if (flag)
                {

                    RemoteUserService.Services srv = new RemoteUserService.Services();
                    srv.CookieContainer = CurrentUser.CurContainer;
                    RemoteUserService.SUser user = srv.GetUserInfo("", loginsession);


                    if (page <= 1)
                    {
                        page = 1;


                        string sqlWhere = "";
                        flag = isSbgly(loginsession);
                        if (flag)
                        {
                            sqlWhere += " 1=1 ";
                        }
                        else
                        {
                            sqlWhere += " lrrzh = '" + loginsession + "'";
                        }

                        string sql = "";
                        sql = "select usercode,username,count(usercode) as num from  zhwx_mylendequips  "
                            + " where equipid in (select recid from i_m_sb where ssdwbh='" + user.CPCODE + "'  " + sqlWhere + " )  group by usercode,username ";
                        IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                        ret = CommonService.GetDataTable(sql);
                        //ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";
                    }
                    else {
                        retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":[]}";
                    
                    }

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }



        /// <summary>
        /// 获取我的借出设备列表详情,按租用人
        /// </summary>
        public void getMyLoanEquipListDetails()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                string borrower = HUR("borrower");
                int page = HURI("page");
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                int totalCount;

                flag = isRegister(loginsession);
                if (flag)
                {

                    if (page <= 1)
                    {
                        page = 1;
                    }

                    string sql = "";
                    string szwhere = "";
                    string sqlWhere = "";

                    flag = isSbgly(loginsession);
                    if (flag)
                    {
                        sqlWhere += " 1=1 ";
                    }
                    else
                    {
                        sqlWhere += " t1.lrrzh = '" + loginsession + "'";
                    }


                    szwhere = " and t1.usercode = '" + borrower + "'";
                    sql = "select t1.equipid,t2.sbmc,t2.ssdwmc,t2.depname  from  zhwx_mylendequips t1 "
                        + " left join  i_m_sb t2 on t1.equipid = t2.recid   "
                        + "where t1.equipid in (select recid from i_m_sb where " + sqlWhere + " ) " + szwhere;
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    // ret = CommonService.GetDataTable(sql);
                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";


                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }


        /// <summary>
        /// 得到消息列表
        /// </summary>
        public void getMyMessagesList()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {
                int pagsize = getValue(wx_pagesize_).GetSafeInt();
                string loginsession = HUR("loginsession");
                string type = HUR("type");
                string datetime = HUR("datetime");

                int page = HURI("page");
                int totalCount;

                if (datetime == null || datetime == "")
                {
                    datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (page <= 1)
                {
                    page = 1;
                }

                flag = isRegister(loginsession);
                if (flag)
                {
                    string sql = "";
                    // sql = "select     recid as id, title, context, pusher, pushername, datetime, type,issure, isneedsure, isread, ispush, equipid "
                    //     + " from  zhwx_pushmessages where     type = '" + type + "' and pusher = '" + loginsession + "' and datetime<='" + datetime + "' order by datetime desc ";
                    sql = "select     recid as id, title, CONVERT(varchar, datetime, 120 ) as  datetime,  isread ,issure, isneedsure"
                        + " from  zhwx_pushmessages where     type = '" + type + "' and pusher = '" + loginsession + "' and datetime<='" + datetime + "' order by isread asc,datetime desc ";
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

                    ret = CommonService.GetPageData(sql, pagsize, page, out  totalCount);
                    for (int i = 0; i < ret.Count; i++)
                    {
                        if (ret[i]["isneedsure"].GetSafeString().Equals("1"))
                        {
                            if (ret[i]["issure"].GetSafeString().Equals("1"))
                            {
                                ret[i]["needmsg"] = "【已确认】";
                            }
                            else
                            {
                                ret[i]["needmsg"] = "【未确认】";
                            }
                        }
                        else
                        {
                            ret[i]["needmsg"] = "";
                        }
                    }

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    retMsg = "{\"success\":true,\"datetime\":\"" + datetime + "\",\"page\":\"" + (page + 1) + "\",\"data\":" + jss.Serialize(ret) + "}";

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }




        /// <summary>
        /// 信息详情
        /// </summary>
        public void getMessageDetail()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                int id = HURI("id");

                flag = isRegister(loginsession);
                if (flag)
                {
                    IList<string> sqls = new List<string>();
                    string sql = "update zhwx_pushmessages set isread = '1' where recid = " + id;
                    sqls.Add(sql);
                    flag = CommonService.ExecTrans(sqls);
                    if (flag)
                    {
                        IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                        sql = "select top 1 t1.title, t1.context,t1.pushername, CONVERT(varchar, t1.datetime, 120 ) as datetime,t1.issure,"
                            + " t1.isneedsure,t1.equipid,t2.sbmc,t2.ssdwmc,t2.sbbh,t2.depname from zhwx_pushmessages t1 "
                            + " left join i_m_sb t2 on t1.equipid = t2.recid where t1.recid = " + id;

                        ret = CommonService.GetDataTable(sql);
                        if (ret.Count == 0)
                        {
                            retMsg = "{\"success\":false,\"msg\":\"没有找到该条信息!\"}";
                        }
                        else
                        {
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            retMsg = "{\"success\":true,\"data\":" + jss.Serialize(ret) + "}";
                        }
                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"读取失败!\"}";
                    }


                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }



        /// <summary>
        /// 确认信息（执行zhwx_waitreturn中的数据）
        /// </summary>
        public void messagesure()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");
                string equipid = HUR("equipid");
                int messageid = HURI("id");

                flag = isRegister(loginsession);
                if (flag)
                {
                    IList<string> sqls = new List<string>();
                    string sql = "";
                    sql = "update zhwx_pushmessages set issure = '1' where recid = '" + messageid + "'";
                    sqls.Add(sql);
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    sql = "select recid,equipid,sql,sort from zhwx_waitreturn where equipid = '" + equipid + "' order by sort asc";
                    ret = CommonService.GetDataTable(sql);
                    for (int i = 0; i < ret.Count; i++)
                    {
                        sql = ret[i]["sql"].GetSafeString();
                        sqls.Add(sql);
                    }
                    sql = "delete from zhwx_waitreturn where equipid = '" + equipid + "' ";
                    sqls.Add(sql);
                    flag = CommonService.ExecTrans(sqls);
                    if (flag)
                    {
                        retMsg = "{\"success\":true,\"msg\":\"确认成功!\"}";
                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"msg\":\"确认失败，请重试!\"}";
                    }
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }




        /// <summary>
        /// 获取信息数目和租借设备、借出设备数目
        /// </summary>
        public void getMessagesAndEquip()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");

                int txxx = 0;
                int gzxx = 0;
                int qrxx = 0;
                int jcsb = 0;
                int zjsb = 0;

                flag = isRegister(loginsession);
                if (flag)
                {
                    string sqlWhere = "";
                    flag = isSbgly(loginsession);
                    if (flag)
                    {
                        sqlWhere = " 1=1 ";
                    }
                    else
                    {
                        sqlWhere = " lrrzh ='" + loginsession + "' ";
                    }

                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                    string sql = "";
                    sql = "select 'zjsb' as type, count(*) as num from  zhwx_mylendequips "
                        + " where equipid in (select recid from i_m_sb where " + sqlWhere + " )"
                        + " union all select 'jcsb' as type,count(*) as num from  zhwx_mylendequips "
                        + " where usercode ='" + loginsession + "' union all"
                        + " select type,count(*) as num from zhwx_pushmessages where isread = 0 and pusher = '" + loginsession + "' group by type";
                    ret = CommonService.GetDataTable(sql);
                    for (int i = 0; i < ret.Count; i++)
                    {
                        switch (ret[i]["type"].GetSafeString())
                        {
                            case "2":
                                txxx = ret[i]["num"].GetSafeInt();
                                break;
                            case "3":
                                gzxx = ret[i]["num"].GetSafeInt();
                                break;
                            case "1":
                                qrxx = ret[i]["num"].GetSafeInt();
                                break;
                            case "zjsb":
                                zjsb = ret[i]["num"].GetSafeInt();
                                break;
                            case "jcsb":
                                jcsb = ret[i]["num"].GetSafeInt();
                                break;
                        }
                    }

                    retMsg = "{\"success\":true,\"txxx\":\"" + txxx + "\",\"gzxx\":\"" + gzxx + "\",\"qrxx\":\"" + qrxx + "\",\"zjsb\":\"" + zjsb + "\",\"jcsb\":\"" + jcsb + "\"}";

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }




        public void searchStatus()
        {
            string retMsg = "";
            Boolean flag = false;
            int j = 0;
            try
            {

                string loginsession = HUR("loginsession");
                string status = HUR("status");
                int page = HURI("page");
                flag = isRegister(loginsession);
                if (flag)
                {
                    if (page < 10)
                    {
                        retMsg = "{\"success\":true,\"page\":\"" + (page + 1) + "\""
                        + ",\"data\":[";

                        for (int z = 0; z < 10; z++)
                        {
                            retMsg += "{\"id\":\"id" + (page) + "-" + j + "\",\"value\":\"value" + (page) + "-" + j + status + "\"},";
                            j++;
                        }
                        retMsg = retMsg.Substring(0, retMsg.Length - 1);
                        retMsg += "]}";
                    }
                    else
                    {
                        retMsg = "{\"success\":false,\"page\":\"" + (page + 1) + "\",\"data\":[]}";
                    }
                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }


        /// <summary>
        /// 列子
        /// </summary>
        public void equipsearch2()
        {
            string retMsg = "";
            Boolean flag = false;
            try
            {

                string loginsession = HUR("loginsession");

                flag = isRegister(loginsession);
                if (flag)
                {
                    IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

                }
                else
                {
                    retMsg = "{\"success\":false,\"msg\":\"没有绑定微信\",\"unbind\":true}";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"msg\":\"数据异常!\"}";
            }


            finally
            {
                Response.Write(retMsg);
                Response.End();
            }

        }

        #endregion

        #region 列表获取

        /// <summary>
        /// 获取状态列表
        /// </summary>
        public void getStatus()
        {
            string retMsg = "";
            try
            {
                IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                string sql = "select status,statusname from zhwx_equipstatus";
                ret = CommonService.GetDataTable(sql);
                IList<string> status = new List<string>();
                IList<string> statusname = new List<string>();
                for (int i = 0; i < ret.Count; i++)
                {
                    status.Add(ret[i]["status"]);
                    statusname.Add(ret[i]["statusname"]);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                retMsg = "{\"success\":true,\"status\":" + jss.Serialize(status) + ",\"statusname\":" + jss.Serialize(statusname) + "}";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                retMsg = "{\"success\":false,\"data\":[]}";
            }

            finally
            {
                Response.Write(retMsg);
                Response.End();
            }
        }

        #endregion


        #region 其他公共方法



        /// <summary>
        /// 手机短信推送
        /// </summary>
        /// <returns></returns>
        public bool phoneShortMessagePush()
        {

            return false;
        }


        /// <summary>
        /// 微信消息推送
        /// </summary>
        public bool wxpushmessage()
        {
           // Boolean ret = false;
            try
            {
                string template_id_1 = getValue(wx_template_id_1_);
                string appid = getValue(wx_appid_);
                string secret = getValue(wx_secret_);
                string access_token_url = getValue(wx_message_push_get_access_token_url_);
                string access_token_grant_type = getValue(wx_message_push_get_access_token_grant_type_);
                string access_token = getPost(access_token_url + "?grant_type=" + access_token_grant_type + "&appid=" + appid + "&secret=" + secret, "");
                string wx_message_push_url = getValue(wx_message_push_url_);

                JObject access_token_jo = JObject.Parse(access_token);

                string http = wx_message_push_url + "?access_token=" + access_token_jo["access_token"];

                JObject jb = new JObject();
                jb.Add("touser", "oFAv70Cxp-8YCUSHWI6MNVK62TOM");
                jb.Add("template_id", template_id_1);
                jb.Add("form_id", "c790aae5ecfc414ccd41cf50df8efc98");
                JObject jb1 = new JObject();
                JObject keyword1 = new JObject();
                keyword1.Add("value", "大门");
                jb1.Add("keyword1", keyword1);
                JObject keyword2 = new JObject();
                keyword2.Add("value", "2017-02-20 10:42:21");
                jb1.Add("keyword2", keyword2);
                JObject keyword3 = new JObject();
                keyword3.Add("value", "设备撤防");
                jb1.Add("keyword3", keyword3);
                JObject keyword4 = new JObject();
                keyword4.Add("value", "设备状态变化，请注意");
                jb1.Add("keyword4", keyword4);
                jb.Add("data", jb1);

                JavaScriptSerializer jss = new JavaScriptSerializer();

                string pushret = getPost(http, jb.GetSafeString());
                //Response.Write(jb.ToString());
                // Response.Write(a.ToString());
                JObject pushret_jo = JObject.Parse(pushret);
                if (pushret_jo["errcode"].GetSafeString().Equals("0"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                return false;
            }


        }










        /// <summary>
        /// 获取网络数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string getPost(string url, string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);// 要发放的数据 

            HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(url);
            objWebRequest.Method = "POST";
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            objWebRequest.ContentLength = byteArray.Length;
            Stream newStream = objWebRequest.GetRequestStream();
            // Send the data. 
            newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string ret = sr.ReadToEnd(); // 返回的数据

            return ret;
        }


        /// <summary>
        /// 是否已经注册
        /// </summary>
        /// <param name="uesrcode"></param>
        /// <returns></returns>
        public Boolean isRegister(string uesrcode)
        {
            Boolean ret = false;

            try
            {
                IList<IDictionary<string, string>> userlist = new List<IDictionary<string, string>>();
                string sql = "select * from zhwx_user where usercode = '" + uesrcode + "'";
                userlist = CommonService.GetDataTable(sql);
                if (userlist.Count > 0)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
            }

            return ret;
        }


        /// <summary>
        /// 得到items菜单
        /// statusarray中status
        /// statusarray中toact
        /// toactArray中的第几个
        /// </summary>
        /// <param name="itemslist"></param>
        /// <param name="statusarray"></param>
        /// <param name="status"></param>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        public IList<IList<string>> getItemsList(IList<IList<string>> itemslist, IList<string[]> statusarray, string status, int i1, int i2, int ii)
        {

            IList<string> items = new List<string>();
            IList<string> itemssname = new List<string>();

            if (itemslist.Count != 2)
            {
                itemslist = new List<IList<string>>();
                itemslist.Add(items);
                itemslist.Add(itemssname);
            }


            for (int j = 0; j < statusarray.Count; j++)
            {
                if (!status.Equals("") && statusarray[j][i1].Equals(status))
                {
                    string toact = statusarray[j][i2].GetSafeString();
                    string[] toactArray = toact.Split('|');
                    if (toactArray.Length >= ii)
                    {
                        string[] iArray = toactArray[ii].Split('&');
                        for (int z = 0; z < iArray.Length; z++)
                        {

                            string[] iiArray = iArray[z].Split(',');
                            string items_value = "";
                            string itemssname_value = "";
                            for (int m = 0; m < iiArray.Length; m++)
                            {
                                string[] iiiArray = iiArray[m].Split('-');
                                if (iiiArray.Length == 2)
                                {
                                    items_value += iiiArray[0] + "|";
                                    itemssname_value += iiiArray[1] + "、";
                                }


                            }
                            if (items_value.Length > 0)
                            {
                                items_value = items_value.Substring(0, items_value.Length - 1);
                                itemssname_value = itemssname_value.Substring(0, itemssname_value.Length - 1);
                                //items.Add(items_value);
                                //itemssname.Add(itemssname_value);
                                itemslist[0].Add(items_value);
                                itemslist[1].Add(itemssname_value);
                            }

                        }
                    }
                }
            }



            return itemslist;

        }


        /// <summary>
        /// 是否是设备管理员
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public bool isSbgly(string usercode)
        {
            bool flag = false;
            RemoteUserService.Services srv = new RemoteUserService.Services();
            srv.CookieContainer = CurrentUser.CurContainer;
         //   RemoteUserService.VUserrole[] vUserrole = srv.UMS_GetUserPower("","","JCXT_ZJZH", usercode);

            string timestring = GetTimeStamp();
            string json = srv.UMS_GetUserPower(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), "JCXT_ZJZH", usercode);
               
            //string  a =  srv.UMS_GetUserPower("", "", "JCXT_ZJZH", usercode);
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //flag = jss.Serialize(vUserrole).Contains("CR201702000002");
            flag = json.Contains("\"QTQX_SBGL\"");
            return flag;
        }


        /// <summary>
        /// 发送百度验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public bool bdverifycodepush(string phone, string verifycode, string unitname)
        {

            bool code = false;
            string msg = "";
            try
            {
                //  string receiver = Request["receiver"].GetSafeString();
                string zhwx_bd_sms_base_appid = getValue(zhwx_bd_sms_base_appid_).GetSafeString();
                string time = getValue(bd_wx_login_verifycode_time_).GetSafeString().GetSafeString();
                //string unitname = getValue(wx_push_unitname_).GetSafeString().GetSafeString();
                if (phone == "")
                    code = false;
                else if (!phone.IsMobile())
                    code = false;
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = getValue(bd_wx_login_verifycode_template_).GetSafeString();

                    ZhwxWxBindBdMessage zhwxWxBindBdMessage = new ZhwxWxBindBdMessage()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = phone,
                        templateCode = vctemplate,
                        contentVar = new BindBdMessage()
                        {
                            unitname = unitname.GetSafeString()+"注册通知:",
                            code = verifycode.GetSafeString(),
                            time = time.GetSafeString()
                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(zhwxWxBindBdMessage);

                    code = SmsService.SendMessage(zhwx_bd_sms_base_appid, Guid.NewGuid().GetSafeString(), phone, contents, out msg);

                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                return code;
            }

            return code;

        }




        /// <summary>
        /// 发送设备状态提醒
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="shebei"></param>
        /// <param name="status"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public bool bdequipstatuspush(string phone, string shebei, string status, string act)
        {

            bool code = false;
            string msg = "";
            try
            {
                //  string receiver = Request["receiver"].GetSafeString();
                string zhwx_bd_sms_base_appid = getValue(zhwx_bd_sms_base_appid_);
                string time = getValue(bd_wx_login_verifycode_time_).GetSafeString();
                string unitname = getValue(wx_push_unitname_).GetSafeString();
                if (phone == "")
                    code = false;
                else if (!phone.IsMobile())
                    code = false;
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = getValue(bd_wx_equip_status_template_).GetSafeString();

                    ZhwxWxEquipStatusBdMessage zhwxWxEquipStatusBdMessage = new ZhwxWxEquipStatusBdMessage()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = phone,
                        templateCode = vctemplate,
                        contentVar = new EquipStatusBdMessage()
                        {
                            unitname = unitname,
                            shebei = shebei,
                            status = status,
                            act = act

                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(zhwxWxEquipStatusBdMessage);

                    code = SmsService.SendMessage(zhwx_bd_sms_base_appid, Guid.NewGuid().GetSafeString(), phone, contents, out msg);

                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                return code;
            }

            return code;

        }




        /// <summary>
        /// 百度通用短信推送模板
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="client"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool bduniversalpush(string phone, string client, string info)
        {

            bool code = false;
            string msg = "";
            try
            {
                //  string receiver = Request["receiver"].GetSafeString();
                string zhwx_bd_sms_base_appid = getValue(zhwx_bd_sms_base_appid_);
                if (phone == "")
                    code = false;
                else if (!phone.IsMobile())
                    code = false;
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = getValue(bd_wx_universal_template_);

                    ZhwxUniversalMessage zhwxUniversalMessage = new ZhwxUniversalMessage()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = phone,
                        templateCode = vctemplate,
                        contentVar = new UniversalMessage()
                        {
                            client = client,
                            info = info

                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(zhwxUniversalMessage);

                    code = SmsService.SendMessage(zhwx_bd_sms_base_appid, Guid.NewGuid().GetSafeString(), phone, contents, out msg);

                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                return code;
            }

            return code;

        }


        public string HUR(string name)
        {
            string ret = "";
            ret = HttpUtility.UrlDecode(Request[name].GetSafeString());
            return ret;
        }

        public string R(string name)
        {
            string ret = "";
            ret = Request[name].GetSafeString();
            return ret;
        }

        public int HURI(string name)
        {
            int ret = 0;
            ret = int.Parse(HttpUtility.UrlDecode(Request[name].GetSafeInt() + ""));
            return ret;
        }

        public int RI(string name)
        {
            int ret = 0;
            ret = Request[name].GetSafeInt();
            return ret;
        }



        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);

        }


        public string getValue(string key)
        {
            Boolean flag = dic_.ContainsKey(key);
            string ret = "";
            if (flag)
            {
                ret = dic_[key].GetSafeString();
            }

            string sql = "";
            if (ret.Equals(""))
            {
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                sql = "select * from   zhwx_syssetting where SettingCode='" + key + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count > 0)
                {
                    ret = list[0]["settingvalue"].GetSafeString();
                    dic_.Add(key, ret);
                }
            }
            return ret;
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        #endregion
    }
}