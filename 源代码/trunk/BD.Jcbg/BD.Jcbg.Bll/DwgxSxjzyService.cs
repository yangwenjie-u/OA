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
    public class DwgxSxjzyService:IDwgxSxjzyService
    {
        #region 数据库对象

        public ICommonDao CommonDao { get; set; }

        #endregion

        #region 服务
        /// <summary>
        /// 删除技术负责人
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelJSFZR(string rybh, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYJSFZR('{0}')", rybh);
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        /// <summary>
        /// 删除注册建造师
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelZCJZS(string rybh, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYZCJZS('{0}')", rybh);
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        /// <summary>
        /// 删除中级以上职称人员
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelZJYSZCRY(string rybh, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYZJYSZCRY('{0}')", rybh);
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        /// <summary>
        /// 删除现场管理人员
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelXCGLRY(string rybh, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYXCGLRY('{0}')", rybh);
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        /// <summary>
        /// 删除技术工人
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelJSGR(string rybh, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYJSGR('{0}')", rybh);
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        /// <summary>
        /// 删除机械设备
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelJXSB(int recid, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYJXSB('{0}')", recid.ToString());
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        /// <summary>
        /// 删除工程业绩
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelGCYJ(int recid, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("DelQYGCYJ('{0}')", recid.ToString());
                ret = CommonDao.ExecProc(proc, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        /// <summary>
        /// 获取企业资质申报类型
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, object>> GetQyzzSblx()
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select * from h_zzsblx where sfyx=1 order by xssx";
                ret = CommonDao.GetBinaryDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        /// <summary>
        /// 获取企业资质申请表，包括：初次申请和变更表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reporttype"></param>
        /// <param name="msg"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool GetQyzzSqb(string id, string reporttype, string reportfile, out string msg, out byte[] file)
        {
            bool code = true;
            msg = "";
            file = null;
            try
            {
                // 获取记录ID
                int recid = 0;
                string sql = string.Format("select * from view_jdbg_qyzzsb where id='{0}'", id);
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                {
                    recid = dt[0]["recid"].GetSafeInt();
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

                    code = g.GetFile(c, out file, out msg);
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return code;
                
        }


        #endregion

        #region 个人社保查询
        public bool GetAreaAK(string areaKey, out string msg, out string areaAK)
        {
            bool ret = true;
            msg = "";
            areaAK = "";
            try
            {
                string sql = string.Format("select top 1 areaak from h_areaak where areakey='{0}'", areaKey);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    areaAK = dt[0]["areaak"];
                }
                else
                {
                    areaAK = "ZjE0LTkxMDQtMmZjYzgxNzQ3Njhi";
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

        public IList<IDictionary<string, string>> GetAreaList()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select areakey, areaname from h_areaak";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }
        #endregion

        #region 专家会签信息
        [Transaction(ReadOnly =false)]
        public bool WriteZjhq(string idlist, string userlist, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string proc = string.Format("WriteZjhq('{0}','{1}')", idlist, userlist);
                IList<IDictionary<string,string>>  dt = CommonDao.ExecDataTableProc(proc, out msg);
                if (dt.Count > 0)
                {
                    ret = dt[0]["ret"] == "1";
                    msg = dt[0]["msg"];
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

        public bool Zjqrhq(int recid, string sfty, string zjqryj, out string msg)
        {

            bool ret = true;
            try
            {
                string sql = $"update qyzzsb_zjhq set zt=1, sfty='{sfty}', zjqryj='{zjqryj}' where recid={recid.ToString()}" ;
                SysLog4.WriteError(sql);
                ret = CommonDao.ExecSql(sql);
                if (!ret)
                {
                    msg = "更新失败！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            msg = "";
            return ret;
        }

        #endregion


        public IList<IDictionary<string, object>> GetQyzzsbYCRY(string id)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            try
            {
                string sql = string.Format("select * from view_qyzzsb_xq_ycry where parentid in (select recid from jdbg_qyzzsb where id='{0}')", id);
                ret = CommonDao.GetBinaryDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }


        public bool GetQyQGCXPTID(string qybh, out string msg, out string ptid)
        {
            bool ret = true;
            msg = "";
            ptid = "";
            try
            {
                
                string sql = string.Format("select qgcxptid from i_m_qy where qybh='{0}'", qybh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    ptid = dt[0]["qgcxptid"].GetSafeString();
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
        [Transaction(ReadOnly =false)]
        public bool DelQYZZ(string id, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {

                string procstr = string.Format("DelQYZZ('{0}')", id);
                ret = CommonDao.ExecProc(procstr, out msg);
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return ret;
        }


        public bool GetQyzzReportFile(string id, string reporttype, out byte[] file, out string msg)
        {
            bool ret = true;
            file = null;
            msg = "";
            try
            {
                string sql = string.Format("select * from jdbg_qyzzsb where id='{0}'", id);
                IList<IDictionary<string, object>> dtt = CommonDao.GetBinaryDataTable(sql);
                if (dtt.Count > 0)
                {
                    int recid = dtt[0]["recid"].GetSafeInt();
                    string reportFile = "";
                    if (reporttype == "ZZSQB")
                    {
                        reportFile = "ZZSQB";
                    }
                    else if (reporttype == "ZZBGB")
                    {
                        reportFile = "ZZBGB";
                    }


                    if (reportFile == "")
                    {
                        ret = false;
                        msg = "参数错误！";
                    }
                    else
                    {
                        Dictionary<string, object> rd = new Dictionary<string, object>()
                        {
                            { "recid", recid },
                            { "reporttype", reporttype},
                            { "reportfile", reportFile},
                            { "isprint", 1}
                        };
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        c.openType = ReportPrint.OpenType.PDF;
                        c.fileindex = "0";
                        c.filename = reportFile;
                        c.signindex = 0;
                        c.table = "";
                        c.where = "";
                        c.customtools = "1,|2,|12,下载";
                        c.AllowVisitNum = 1;
                        if (reporttype != "")
                        {
                            string tables = "";
                            string wheres = "";
                            try
                            {
                                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(string.Format("select * from help_reporttype where reporttype='{0}'", reporttype));
                                if (dt.Count > 0)
                                {
                                    //构造数据库中配置的数据源字典
                                    List<string> tlist = new List<string>();
                                    List<string> wlist = new List<string>();
                                    foreach (var row in dt)
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
                            }
                            catch (Exception e)
                            {
                                SysLog4.WriteLog(e);
                            }


                            if (tables != "" && wheres != "")
                            {
                                c.table = tables;
                                c.where = wheres;
                            }


                        }

                        if (!g.GetFile(c, out file, out msg))
                        {
                            ret = false;
                        }
                    }
                    
                }
                else
                {
                    ret = false;
                    msg = "无法获取数据！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(msg);
            }

            return ret;
        }

        public IList<IDictionary<string, object>> GetYcQyzzInfo(string id)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            try
            {
                string sql = string.Format("select * from view_jdbg_qyzz_zzfw where zzid='{0}'", id);
                ret = CommonDao.GetBinaryDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        public bool GetHCQKFile(string id, string reporttype, out byte[] file, out string msg)
        {
            bool ret = true;
            file = null;
            msg = "";
            try
            {
                string sql = string.Format("select * from jdbg_qyzz_hcqkjl where id='{0}'", id);
                IList<IDictionary<string, object>> dtt = CommonDao.GetBinaryDataTable(sql);
                if (dtt.Count > 0)
                {
                    int recid = dtt[0]["recid"].GetSafeInt();
                    string reportFile = "HCQKJLB";
                    if (reportFile == "")
                    {
                        ret = false;
                        msg = "参数错误！";
                    }
                    else
                    {
                        Dictionary<string, object> rd = new Dictionary<string, object>()
                        {
                            { "recid", recid },
                            { "reporttype", reporttype},
                            { "reportfile", reportFile},
                            { "isprint", 1}
                        };
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        c.openType = ReportPrint.OpenType.PDF;
                        c.fileindex = "0";
                        c.filename = reportFile;
                        c.signindex = 0;
                        c.table = "";
                        c.where = "";
                        c.customtools = "1,|2,|12,下载";
                        c.AllowVisitNum = 1;
                        if (reporttype != "")
                        {
                            string tables = "";
                            string wheres = "";
                            try
                            {
                                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(string.Format("select * from help_reporttype where reporttype='{0}'", reporttype));
                                if (dt.Count > 0)
                                {
                                    //构造数据库中配置的数据源字典
                                    List<string> tlist = new List<string>();
                                    List<string> wlist = new List<string>();
                                    foreach (var row in dt)
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
                            }
                            catch (Exception e)
                            {
                                SysLog4.WriteLog(e);
                            }


                            if (tables != "" && wheres != "")
                            {
                                c.table = tables;
                                c.where = wheres;
                            }


                        }

                        if (!g.GetFile(c, out file, out msg))
                        {
                            ret = false;
                        }
                    }

                }
                else
                {
                    ret = false;
                    msg = "无法获取数据！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(msg);
            }

            return ret;
        }
    }
}
