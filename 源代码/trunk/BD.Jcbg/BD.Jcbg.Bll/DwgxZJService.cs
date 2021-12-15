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
using System.Text.RegularExpressions;

namespace BD.Jcbg.Bll
{
    public class DwgxZJService : IDwgxZJService
    {
        #region 数据库对象

        public ICommonDao CommonDao { get; set; }

        #endregion
        public IList<IDictionary<string, object>> GetSbcqbaByWybh(string wybh)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            try
            {
                string sql = string.Format("select * from view_info_cqba where wybh='{0}'", wybh);
                ret = CommonDao.GetBinaryDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        public bool GetSbReportFile(string serial, string reporttype, out byte[] file, out string msg)
        {
            bool ret = true;
            msg = "";
            file = null;
            try
            {
                string sql = string.Format("select * from view_sb_reportsbsy where workserial='{0}' ", serial);
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                {
                    int recid = dt[0]["recid"].GetSafeInt();
                    string reportfile = "";
                    switch (reporttype)
                    {
                        case "SBAZGZ":
                            reportfile = "安装告知表V1";
                            break;
                        case "SBSYDJ":
                            reportfile = "使用登记表V1";
                            break;
                        case "SBCXGZ":
                            reportfile = "拆卸告知表V1";
                            break;
                        default:
                            break;
                    }
                    int formid = 0;
                    sql = string.Format("select * from stform where serialno='{0}' ", serial);
                    IList<IDictionary<string, object>> dt1 = CommonDao.GetBinaryDataTable(sql);
                    if (dt1.Count > 0)
                    {
                        formid = dt1[0]["formid"].GetSafeInt();
                    }
                    // 生成二进制word
                    if (recid > 0)
                    {
                        string tables = "";
                        string wheres = "";

                        #region 获取配置的table和where
                        // 需要替换的字典
                        Dictionary<string, object> rd = new Dictionary<string, object>()
                    {
                        { "reporttype", reporttype},
                        { "reportfile", reportfile},
                        { "recid", recid},
                        { "serial", serial},
                        { "formid", formid},
                    };

                        IList<IDictionary<string, string>> dtt = CommonDao.GetDataTable(string.Format("select * from help_reporttype where reporttype='{0}'", reporttype));
                        if (dtt.Count > 0)
                        {
                            //构造数据库中配置的数据源字典
                            List<string> tlist = new List<string>();
                            List<string> wlist = new List<string>();
                            foreach (var row in dtt)
                            {
                                string t = row["tablename"].GetSafeString();
                                string w = row["wherestr"].GetSafeString();
                                // 如果表名不为空（这里包含了where条件为空的情况，按需求可能需要修改）
                                if (t != "")
                                {
                                    // 替换数据库配置项的值
                                    foreach (var r in rd)
                                    {
                                        Regex reg = new Regex("\\{" + r.Key + "\\}", RegexOptions.IgnoreCase);
                                        w = reg.Replace(w, r.Value.GetSafeString());
                                    }
                                    tlist.Add(t);
                                    wlist.Add(w);
                                }
                            }
                            if (tlist.Count > 0 && tlist.Count == wlist.Count)
                            {
                                tables = string.Join("|", tlist.ToArray());
                                wheres = string.Join("|", wlist.ToArray());
                            }
                        }

                        SysLog4.WriteError(tables);
                        SysLog4.WriteError(wheres);
                        #endregion


                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        //c.openType = ReportPrint.OpenType.Print;
                        c.openType = ReportPrint.OpenType.PDF;
                        c.fileindex = "0";
                        c.filename = reportfile;
                        c.table = tables;
                        c.where = wheres;

                        ret = g.GetFile(c, out file, out msg);
                    }
                }
                else
                {
                    ret = false;
                    msg = "无法找到相应的设备安装告知记录！";
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
    }
}
