using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Web;
using System.Web.Caching;
using System.Reflection;
using ReportPrint.Common;

using Spring.Context;
using Spring.Context.Support;

namespace BD.Jcbg.Bll
{
    public class UpBgsdscService : IUpBgsdscService
    {
        #region 数据库对象
        public ICommonDao CommonDao { get; set; }

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

        private IDataFileService _dataFileService = null;
        private IDataFileService DataFileService
        {
            get
            {
                if (_dataFileService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataFileService = webApplicationContext.GetObject("DataFileService") as IDataFileService;
                }
                return _dataFileService;
            }
        }

        #endregion

        /// <summary>
        /// 生成二维码的pdf文件
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="pdf"></param>
        /// <param name="dwbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool UpReport(int recid, string pdf, string dwbh, out string barcodepdf, out string msg)
        {
            bool ret = false;
            msg = "";
            barcodepdf = "";
            string repeatField = "";
            string barcode = "";
            bool setBarcode = false;
            string filetext = "";
            StringBuilder logs = new StringBuilder();
            try
            {
                lock (this)
                {
                    string sql = "";
                    IList<IDictionary<string, string>> dttmp = CommonDao.GetDataTable("select hqewm from i_m_qy where qybh='" + dwbh + "'");
                    if (dttmp.Count == 0)
                        setBarcode = true;
                    else
                    {
                        int hqewm = dttmp[0]["hqewm"].GetSafeInt();
                        if (hqewm == 0)
                            setBarcode = true;
                    }
                    // 二维码条件
                    BarcodeOptionReport barcodeOption = GetBarCodeOption(dwbh, "");
                    // 报告明细json

                    // 报告pdf json

                    string bgewm = Guid.NewGuid().ToString();
                    int index = 1;
                    string filename = "";

                    string tmpExt = "";
                    string fileid = Guid.NewGuid().ToString("N");
                    filename = fileid + ".pdf";
                    if (filename.IndexOf(".") > 0)
                    {
                        tmpExt = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
                    }
                    byte[] pdffile = Convert.FromBase64String(pdf);
                    byte[] barimage = Barcode.GetBarcode2(GetRealContent(bgewm), barcodeOption.Width, barcodeOption.Width);

                    if (setBarcode)
                    {
                        pdffile = PdfWaterMark.SetWaterMark(pdffile, barimage, barcodeOption.PageModule,
                            barcodeOption.PositionModule, barcodeOption.HSpan, barcodeOption.VSpan);

                        barcodepdf = Convert.ToBase64String(pdffile);
                    }
                    logs.Append(sql + "\r\n" + recid + "," + index + "\r\n");
                    ret = DataFileService.SaveDataFile(fileid, filename, pdffile, tmpExt, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                    if (ret)
                    {
                        filetext = filetext + fileid + "," + filename + "|";
                        sql = "update UP_BGSDSC set PDFID='" + filetext + "',EWM='" + bgewm + "' where recid=" + recid.ToString();
                        CommonDao.ExecCommand(sql, CommandType.Text);
                        ret = true;
                    }
                    //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                    //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                    //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILENAME", filename);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILECONTENT", pdffile);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILEEXT", tmpExt);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //sqlparams.Add(sqlparam);

                    //logs.Append(sql + "\r\n" + recid + "," + index + "\r\n");
                    //if (CommonDao.ExecCommand(sqlstr, CommandType.Text, sqlparams))
                    //{
                    //    filetext = filetext + fileid + "," + filename + "|";
                    //}
                    //sql = "update UP_BGSDSC set PDFID='" + filetext + "',EWM='" + bgewm + "' where recid=" + recid.ToString();
                    //CommonDao.ExecCommand(sql, CommandType.Text);

                    //ret = true;

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteError("插入文件sql：" + logs.ToString());
                SysLog4.WriteLog("最后字段：" + repeatField, ex);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        static IDictionary<string, BarcodeOptionReport> BarcodeOptions = new Dictionary<string, BarcodeOptionReport>();
        private BarcodeOptionReport GetBarCodeOption(string dwbh, string syxmbh)
        {
            BarcodeOptionReport ret = new BarcodeOptionReport();
            try
            {
                string key = dwbh + "_" + syxmbh;
                if (!BarcodeOptions.TryGetValue(key, out ret))
                {
                    ret = new BarcodeOptionReport();
                    string sql = "select top 1 * from syspdfwatermark where (dwbh='' or dwbh='" + dwbh + "') and (syxmbh='' or syxmbh='" + syxmbh + "') order by len(dwbh) desc, len(syxmbh) desc";
                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        ret.PageModule = dt[0]["ymlx"].GetSafeInt();
                        ret.PositionModule = dt[0]["wzlx"].GetSafeInt();
                        ret.Width = dt[0]["bc"].GetSafeInt();
                        ret.HSpan = dt[0]["hxbj"].GetSafeInt();
                        ret.VSpan = dt[0]["zxbj"].GetSafeInt();
                        BarcodeOptions.Add(key, ret);
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }


        [Transaction(ReadOnly =false)]
        public bool GetReportFile(string pdfid, out byte[] file, out string msg)
        {
            bool ret = false;
            msg = "";
            file = null;
            try
            {
                IList<IDictionary<string, object>> dtf = CommonDao.GetBinaryDataTable("select filecontent,storagetype, fileurl from datafile where fileid='" + pdfid + "'");
                if (dtf.Count == 0)
                {
                    msg = "找不到对应的报告文件";
                    return ret;
                }

                file = dtf[0]["filecontent"] as byte[];
                string storagetype = dtf[0]["storagetype"].GetSafeString();
                string fileurl = dtf[0]["fileurl"].GetSafeString();
                if (file == null || file.Length == 0 )
                {
                    if (storagetype.Equals("oss",StringComparison.OrdinalIgnoreCase) && (fileurl!=""))
                    {
                        file = OssCdnUtil.DownFile(fileurl);
                    }
                }

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }


        [Transaction(ReadOnly = false)]
        public bool GetReportFileByEWM(string ewm, out string thumbfileid, out string pdfid, out string msg)
        {
            bool ret = false;
            msg = "";
            thumbfileid = "";
            pdfid = "";
            string pdfinfo = "";
            try
            {
                IList<IDictionary<string, object>> dtf = CommonDao.GetBinaryDataTable("select pdfid,thumbfileid from up_bgsdsc where ewm='" + ewm + "'");
                if (dtf.Count == 0)
                {
                    msg = "找不到对应的报告文件";
                    return ret;
                }

                thumbfileid = dtf[0]["thumbfileid"].GetSafeString();
                pdfinfo = dtf[0]["pdfid"].GetSafeString();
                if (pdfinfo !="")
                {
                    pdfid = pdfinfo.Split(',')[0];
                }

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public bool GetReportFileBySlt(string strSaveName, byte[] postcontent, string ext, Dictionary<string, object> dt)
        {
            bool ret = false;
            string msg = "";

            #region 保存上传的附件
            string fileid = Guid.NewGuid().ToString("N");
            ret = DataFileService.SaveDataFile(fileid, strSaveName, postcontent, ext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
            //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
            //IList<IDataParameter> sqlparams = new List<IDataParameter>();
            //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
            //sqlparams.Add(sqlparam);
            //sqlparam = new SqlParameter("@FILENAME", strSaveName);
            //sqlparams.Add(sqlparam);
            //sqlparam = new SqlParameter("@FILECONTENT", postcontent);
            //sqlparams.Add(sqlparam);
            //sqlparam = new SqlParameter("@FILEEXT", ext);
            //sqlparams.Add(sqlparam);
            //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //sqlparams.Add(sqlparam);
            #endregion

            #region 插入手动上传报告的记录
            // 文件保存成功，插入报告信息
            if (ret)
            {
                object bgbh = "";
                object wtdw = "";
                object gcmc = "";
                object xmmc = "";
                object lbmc = "";
                object bglx = "";
                object bgscfs = "";
                dt.TryGetValue("bgbh", out bgbh);
                dt.TryGetValue("wtdw", out wtdw);
                dt.TryGetValue("gcmc", out gcmc);
                dt.TryGetValue("syxm", out xmmc);
                dt.TryGetValue("jclb", out lbmc);
                dt.TryGetValue("bglx", out bglx);
                dt.TryGetValue("bgscfs", out bgscfs);
                if (bgbh.GetSafeString() == "")
                {
                    throw new Exception("无法获取报告编号");
                }
                if (CommonDao.GetDataTableSameTrans("select * from UP_BGSDSC where BGBH = '" + bgbh.GetSafeString() + "'").Count > 0)
                {
                    if (!CommonService.ExecSql("update UP_BGSDSC set IsDel='1' where BGBH='" + bgbh.GetSafeString() + "'", out msg))
                    {
                        throw new Exception(msg);
                    }                  
                }
                string sqlstr = string.Format(
                    "insert into up_bgsdsc(CompanyId,CompanyName,DepartmentId,DepartmentName,BGBH,WTDW,GCMC,XMMC,LBMC,BGLX,Bgscfs,LRRZH,LRRXM,FILEID,LRSJ,IsDel) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',getdate(),'0')",
                    CurrentUser.CurUser.CompanyId,
                    CurrentUser.CurUser.CompanyName,
                    CurrentUser.CurUser.DepartmentId,
                    CurrentUser.CurUser.DepartmentName,
                    bgbh.GetSafeString(),
                    wtdw.GetSafeString(),
                    gcmc.GetSafeString(),
                    xmmc.GetSafeString(),
                    lbmc.GetSafeString(),
                    bglx.GetSafeString(),
                    bgscfs.GetSafeString(),
                    CurrentUser.UserName,
                    CurrentUser.RealName,
                    fileid
                    );
                if (CommonService.ExecSql(sqlstr, out msg))
                {
                    int recid = 0;
                    sqlstr = string.Format("select recid from up_bgsdsc where fileid='{0}'", fileid);
                    IList<IDictionary<string, string>> dtt = CommonDao.GetDataTableSameTrans(sqlstr);
                    if (dtt.Count > 0)
                    {
                        recid = dtt[0]["recid"].GetSafeInt();

                    }
                    if (recid > 0)
                    {
                        string filebase64 = Convert.ToBase64String(postcontent);
                        string outfilestring = "";
                        string barcodepdf = "";
                        bool code = new OfficeConvert().ConvertWordToPdfStr(filebase64, out outfilestring, out msg);
                        if (code)
                        {
                            //生成二维码
                            if (UpReport(recid, outfilestring, "", out barcodepdf, out msg))
                            {
                                #region 生成缩略图
                                string notsts = "";
                                if (new OfficeConvert().ConvertPdfToPicStr(barcodepdf, out notsts, out msg))
                                {
                                    byte[] a = Convert.FromBase64String(notsts.Split('|')[0]);
                                    fileid = Guid.NewGuid().ToString("N");
                                    ret = DataFileService.SaveDataFile(fileid, fileid + ".jpg", a, ".jpg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);

                                    //sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                                    //sqlparams = new List<IDataParameter>();
                                    //sqlparam = new SqlParameter("@FILEID", fileid);
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILENAME", fileid + ".jpg");
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILECONTENT", a);
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILEEXT", ".jpg");
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //sqlparams.Add(sqlparam);
                                    if (ret)
                                    {
                                        sqlstr = string.Format("update up_bgsdsc set thumbfileid='{0}' where recid={1}", fileid, recid.ToString());
                                        if (!CommonService.ExecSql(sqlstr, out msg))
                                        {
                                            throw new Exception(msg);
                                        }
                                        ret = true;
                                    }
                                    else
                                    {
                                        throw new Exception(msg);
                                    }
                                }
                                else
                                {
                                    throw new Exception(msg);
                                }
                                #endregion
                            }
                            else
                            {
                                throw new Exception(msg);
                            }

                        }
                        else
                        {
                            throw new Exception(msg);
                        }
                    }
                    else
                    {
                        throw new Exception("无法获取fileid");
                    }

                }
                else
                {
                    throw new Exception(msg);
                }

            }
            else
            {
                throw new Exception(msg);
            }
            return ret;
            #endregion
        }

        [Transaction(ReadOnly = false)]
        public bool ReGenerateReport(string id, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string sql = string.Format("select * from UP_BGSDSC where 1=1 ");
                if (id != "")
                {
                    sql += string.Format(" and fileid='{0}'", id); 
                }

                IList<IDictionary<string, string>> bglist = CommonDao.GetDataTable(sql);
                foreach (var bg in bglist)
                {
                    int recid = bg["recid"].GetSafeInt();
                    string fileid = bg["fileid"];
                    string pdfinfo = bg["pdfid"];
                    string ewm = bg["ewm"];
                    string thumbfileid = bg["thumbfileid"];
                    if (recid > 0)
                    {
                        // 获取原始文件
                        byte[] postcontent = null;
                        sql = string.Format("select filecontent,storagetype,fileurl from datafile where fileid='{0}'", fileid);
                        IList <IDictionary < string, object>> orignalFileList = CommonDao.GetBinaryDataTable(sql);
                        if (orignalFileList.Count >0)
                        {
                            postcontent = orignalFileList[0]["filecontent"] as byte[];
                            if (postcontent == null || postcontent.Length == 0)
                            {
                                string storagetype = orignalFileList[0]["storagetype"].GetSafeString();
                                string fileurl = orignalFileList[0]["fileurl"].GetSafeString();
                                if (storagetype.Equals("oss", StringComparison.OrdinalIgnoreCase) && (fileurl!=""))
                                {
                                    postcontent = OssCdnUtil.DownFile(fileurl);
                                }
                            }
                        }
                        
                        // 原始文件存在
                        if (postcontent != null && postcontent.Length > 0)
                        {
                            string filebase64 = Convert.ToBase64String(postcontent);
                            string outfilestring = "";
                            string barcodepdf = "";
                            bool code = new OfficeConvert().ConvertWordToPdfStr(filebase64, out outfilestring, out msg);
                            if (code)
                            {
                                //生成二维码
                                if (UpReport(recid, outfilestring, "", out barcodepdf, out msg))
                                {
                                    #region 生成缩略图
                                    string notsts = "";
                                    if (new OfficeConvert().ConvertPdfToPicStr(barcodepdf, out notsts, out msg))
                                    {
                                        byte[] a = Convert.FromBase64String(notsts.Split('|')[0]);
                                        fileid = Guid.NewGuid().ToString("N");
                                        ret = DataFileService.SaveDataFile(fileid, fileid + ".jpg", a, ".jpg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                                        if (ret)
                                        {
                                            string sqlstr = string.Format("update up_bgsdsc set thumbfileid='{0}' where recid={1}", fileid, recid.ToString());
                                            if (!CommonService.ExecSql(sqlstr, out msg))
                                            {
                                                throw new Exception(msg);
                                            }
                                            ret = true;
                                        }
                                        else
                                        {
                                            throw new Exception(msg);
                                        }
                                        //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                        //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                                        //sqlparams = new List<IDataParameter>();
                                        //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                                        //sqlparams.Add(sqlparam);
                                        //sqlparam = new SqlParameter("@FILENAME", fileid + ".jpg");
                                        //sqlparams.Add(sqlparam);
                                        //sqlparam = new SqlParameter("@FILECONTENT", a);
                                        //sqlparams.Add(sqlparam);
                                        //sqlparam = new SqlParameter("@FILEEXT", ".jpg");
                                        //sqlparams.Add(sqlparam);
                                        //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        //sqlparams.Add(sqlparam);
                                        //if (CommonService.ExecTrans(sqlstr, sqlparams, out msg))
                                        //{
                                        //    sqlstr = string.Format("update up_bgsdsc set thumbfileid='{0}' where recid={1}", fileid, recid.ToString());
                                        //    if (!CommonService.ExecSql(sqlstr, out msg))
                                        //    {
                                        //        throw new Exception(msg);
                                        //    }
                                        //    ret = true;
                                        //}
                                        //else
                                        //{
                                        //    throw new Exception(msg);
                                        //}
                                    }
                                    else
                                    {
                                        throw new Exception(msg);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    throw new Exception(msg);
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
                
        }

        private string GetRealContent(string ewm)
        {
            string ret = ewm;
            string prefix = Configs.GetConfigItem("WeixinUrlPrefix");
            if (prefix !="")
            {
                ret = prefix.Replace("&amp;","&") + ewm;
            }
            return ret;

        }
    }
}
