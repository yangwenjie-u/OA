using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace BD.Jcbg.Web.Controllers
{
    public class SXJTController : Controller
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

        public ActionResult kqmonth()
        {
            ViewData["jdzch"] = Request["jdzch"].GetSafeInt();
            return View();
        }

        #endregion

        #region 各种操作
        [Authorize]
        public void AddUserLWGS()
        {
            bool code = false;
            string msg = "";

            string zh = Request["usercode"].GetSafeString();
            string qymc = Request["qymc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号

                do
                {
                    // 查找人员信息，获取人员类型、代码、姓名
                    sql = "select lwgsbh from i_m_lwgs where lwgsbh='" + zh + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        msg = "该账户创建失败";
                        break;
                    }
                    username = zh;
                    if (username == "")
                        username = zh;
                    realname = qymc;

                    // 查找人员类型信息，获取默认单位、部门、角色                        

                    string companycode = Configs.GetConfigItem("lwgscompanycode");
                    string depcode = Configs.GetConfigItem("lwgsdepcode");
                    string rolecode = Configs.GetConfigItem("lwgsrole");
                    postcode = Configs.GetConfigItem("lwgsbmbh");
                    // 判断账号是否已创建
                    //sql = "select * from i_m_lwgs where yhzh='" + username + "'";
                    //dt = CommonService.GetDataTable(sql);
                    //if (dt.Count > 0)
                    //{
                    //    msg = "账号已经存在";
                    //    code = false;
                    //    break;
                    //}

                    string password = GlobalVariable.GetDefaultUserPass();
                    if (password == "")
                        password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                    code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                    if (!code)
                        break;
                    string yhzh = msg;
                    //code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                    //if (!code)
                    //    break;

                    sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + zh + "','" + yhzh + "',1,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                    IList<string> sqls = new List<string>();
                    sqls.Add(sql);
                    code = CommonService.ExecTrans(sqls, out msg);
                    if (code)
                    {
                        Session["USER_INFO_USERNAME"] = username;
                        Session["USER_INFO_PASSWORD"] = password;
                    }
                } while (false);

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
        /// 添加劳务员
        /// </summary>
        [Authorize]
        public void AddUserLWY()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select zh,qybh,lwgsbh from i_m_lzzgy_zh where zh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "该账户创建失败";
                            break;
                        }
                        username = usercode;
                        if (username == "")
                            username = usercode;
                        realname = gcmc + "-劳务员";
                        string lwgsbh = dt[0]["lwgsbh"];
                        // 查找劳务人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_rylx where lxbh='06'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        //sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        //dt = CommonService.GetDataTable(sql);
                        //if (dt.Count > 0)
                        //{
                        //    msg = "账号已经存在";
                        //    code = false;
                        //    break;
                        //}

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
                        //code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                        //if (!code)
                        //    break;

                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + lwgsbh + "','" + yhzh + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                        string sql2 = "update I_M_LZZGY_ZH set usercode='" + yhzh + "' where zh='" + usercode + "'";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        sqls.Add(sql2);
                        code = CommonService.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            Session["USER_INFO_USERNAME"] = username;
                            Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);


                }
                else
                {
                    code = false;
                    msg = "无效的用户类型";
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
        public void DeleteUser()
        {
            bool code = false;
            string msg = "";
            string usercode = Request["usercode"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {

            }
            catch (Exception e)
            {

            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }



        #region 导入Excel

        /// <summary>
        /// 将excel导入到datatable
        /// </summary>
        /// <param name="filePath">excel路径</param>
        /// <param name="isColumnName">第一行是否是列名</param>
        public DataTable ExcelToDataTable(HttpPostedFileBase postfile, bool isColumnName)
        {
            DataTable dataTable = null;
            Stream fs = null;
            DataColumn column;
            DataRow dataRow;
            IWorkbook workbook = null;
            ISheet sheet;
            IRow row;
            ICell cell;
            int startRow = 0;
            string filename = postfile.FileName;
            try
            {
                //using (fs = System.IO.File.OpenRead(filePath))
                using (fs = postfile.InputStream)
                {
                    // 2007版本  
                    if (filename.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (filename.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);
                    if (workbook != null)
                    {
                        dataTable = new DataTable();
                        for (int m = 0; m < workbook.NumberOfSheets; m++)
                        {
                            sheet = workbook.GetSheetAt(m); //读取sheet 
                            if (sheet != null)
                            {
                                int rowCount = sheet.PhysicalNumberOfRows; //总行数
                                if (rowCount > 0)
                                {
                                    IRow firstRow = sheet.GetRow(0); //第一行
                                    int cellCount = firstRow.LastCellNum; //列数
                                    //构建datatable的列
                                    if (isColumnName)
                                    {
                                        startRow = 1; //如果第一行是列名，则从第二行开始读取  
                                        if (m == 0) //多个sheet的时候,只需要第一个sheet的第一行就可以了
                                        {
                                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                            {
                                                cell = firstRow.GetCell(i);
                                                if (cell != null)
                                                {
                                                    if (cell.StringCellValue != null)
                                                    {
                                                        column = new DataColumn(cell.StringCellValue);
                                                        dataTable.Columns.Add(column);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                        {
                                            column = new DataColumn("column" + (i + 1));
                                            dataTable.Columns.Add(column);
                                        }
                                    }
                                    //填充行  
                                    for (int i = startRow; i <= rowCount; ++i)
                                    {
                                        row = sheet.GetRow(i);
                                        if (row == null) continue;
                                        dataRow = dataTable.NewRow();
                                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                                        {
                                            cell = row.GetCell(j);
                                            if (cell == null)
                                            {
                                                dataRow[j] = "";
                                            }
                                            else
                                            {
                                                //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                                switch (cell.CellType)
                                                {
                                                    case CellType.Blank:
                                                        dataRow[j] = "";
                                                        break;
                                                    case CellType.Numeric:
                                                    case CellType.Formula:
                                                        short format = cell.CellStyle.DataFormat;
                                                        //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                        if (format == 14 || format == 31 || format == 57 ||
                                                            format == 58)
                                                            dataRow[j] = cell.DateCellValue;
                                                        else
                                                            dataRow[j] = cell.NumericCellValue;
                                                        break;
                                                    case CellType.String:
                                                        dataRow[j] = cell.StringCellValue;
                                                        break;
                                                }
                                            }
                                        }
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception exception)
            {
                SysLog4.WriteLog(exception);
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        /// <summary>
        /// 导入经纬度
        /// </summary>
        /// Author : Napoleon
        /// Created : 2017-04-08 9:57
        public int ExcelToJwd(string serverPath, string projectId)
        {
            return 1;
            HttpPostedFileBase postfile = null;
            if (Request.Files.Count > 0)
            {
                postfile = Request.Files[0];
            }

           
            //将Excel文件内容写入到数据库中
            DataTable dt = ExcelToDataTable(postfile, true);
           
            foreach (DataRow row in dt.Rows)
            {
                /*
                projectInfos.Add(new ProjectInfo
                {
                    Id = GuidFunc.CreateGuid(),
                    ProjectId = projectId,
                    Lon = decimal.Parse(row["经度"].ToString()),
                    Lat = decimal.Parse(row["纬度"].ToString()),
                    OrderBy = decimal.Parse(row["顺序"].ToString())
                });*/
            }
            
        }

        #endregion


        #endregion
    }
}