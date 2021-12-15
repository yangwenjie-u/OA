using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web;
using BD.IDataInputDao;
using System.Diagnostics;
using System.Net.Sockets;
using ReportPrint.Common;
using BD.Jcbg.Service;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace BD.Jcbg.Bll
{
    public class JcjtService : IJcjtService
    {
        #region 数据库对象

        public ICommonDao CommonDao { get; set; }

        #endregion
        public IList<JcjtJcjgZZ> GetJcjgZZ()
        {
            string sql = "select * from PR_M_SYXMXSFL where ssdwbh='' order by xsflbh";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            string sql_xm = "select * from pr_m_syxm where ssdwbh='' order by xsflbh1,xsflbh ";
            IList<IDictionary<string, string>> dt_xm = CommonDao.GetDataTable(sql_xm);

            IList<JcjtJcjgZZ> listob = new List<JcjtJcjgZZ>();
            for (int i = 0; i < dt.Count; i++)
            {
                if (dt[i]["sjxsflbh"] == "")
                {
                    JcjtJcjgZZ ob = new JcjtJcjgZZ();
                    ob.xsflbh = dt[i]["xsflbh"];
                    ob.xsflmc = dt[i]["xsflmc"];
                    ob.sfyx = dt[i]["sfyx"];
                    List<JcjtJcjgZZFL> listzzfl = new List<JcjtJcjgZZFL>();
                    for (int j = 0; j < dt.Count; j++)
                    {
                        if (dt[j]["sjxsflbh"] == dt[i]["xsflbh"])
                        {
                            JcjtJcjgZZFL t_ob = new JcjtJcjgZZFL();
                            t_ob.sjxsflbh = dt[j]["sjxsflbh"];
                            t_ob.xsflbh = dt[j]["xsflbh"];
                            t_ob.xsflmc = dt[j]["xsflmc"];
                            t_ob.sfyx = dt[j]["sfyx"];
                            List<JcjtJcjgXM> listxm = new List<JcjtJcjgXM>();
                            for (int k = 0; k < dt_xm.Count; k++)
                            {
                                if (dt_xm[k]["xsflbh1"] == dt[i]["xsflbh"] && dt_xm[k]["xsflbh"] == dt[j]["xsflbh"])
                                {
                                    JcjtJcjgXM oxm = new JcjtJcjgXM();
                                    oxm.syxmbh = dt_xm[k]["syxmbh"];
                                    oxm.syxmmc = dt_xm[k]["syxmmc"];
                                    oxm.sfyx = dt_xm[k]["sfyx"];
                                    listxm.Add(oxm);
                                }
                            }
                            t_ob.xmrows = listxm;
                            if (listxm.Count > 0)
                                listzzfl.Add(t_ob);
                        }
                    }
                    ob.rows = listzzfl;
                    if (listzzfl.Count > 0)
                        listob.Add(ob);
                }
            }

            return listob;
        }

        /// <summary>
        /// 获取企业的资质类型 status=0显示所以包括有的和没哟的，status=1 只显示有的
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<JcjtJcjgZZ> GetJcjgZZxx_Byqybh(string qybh, int status = 0)
        {
            string where1 = "";
            string where2 = "";
            if (status == 1)
            {
                where1 += " and ((sjxsflbh='' and sfyx=1) or sjxsflbh<>'')";
                where2 += " and sfyx=1";
            }

            string sql = "select * from PR_M_SYXMXSFL where ssdwbh='" + qybh + "'" + where1 + " order by xsflbh";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            string sql_xm = "select * from pr_m_syxm where ssdwbh='" + qybh + "'" + where2 + " order by xsflbh1,xsflbh ";
            IList<IDictionary<string, string>> dt_xm = CommonDao.GetDataTable(sql_xm);

            IList<JcjtJcjgZZ> listob = new List<JcjtJcjgZZ>();
            for (int i = 0; i < dt.Count; i++)
            {
                if ((dt[i]["sjxsflbh"] == "" && status == 0) || (dt[i]["sjxsflbh"] == "" && status == 1 && dt[i]["sfyx"].ToLower() == "true"))
                {
                    JcjtJcjgZZ ob = new JcjtJcjgZZ();
                    ob.xsflbh = dt[i]["xsflbh"];
                    ob.xsflmc = dt[i]["xsflmc"];
                    ob.sfyx = dt[i]["sfyx"];
                    List<JcjtJcjgZZFL> listzzfl = new List<JcjtJcjgZZFL>();
                    for (int j = 0; j < dt.Count; j++)
                    {
                        if (dt[j]["sjxsflbh"] == dt[i]["xsflbh"])
                        {
                            JcjtJcjgZZFL t_ob = new JcjtJcjgZZFL();
                            t_ob.sjxsflbh = dt[j]["sjxsflbh"];
                            t_ob.xsflbh = dt[j]["xsflbh"];
                            t_ob.xsflmc = dt[j]["xsflmc"];
                            t_ob.sfyx = dt[j]["sfyx"];
                            List<JcjtJcjgXM> listxm = new List<JcjtJcjgXM>();
                            for (int k = 0; k < dt_xm.Count; k++)
                            {
                                if (dt_xm[k]["xsflbh1"] == dt[i]["xsflbh"] && dt_xm[k]["xsflbh"] == dt[j]["xsflbh"])
                                {
                                    JcjtJcjgXM oxm = new JcjtJcjgXM();
                                    oxm.syxmbh = dt_xm[k]["syxmbh"];
                                    oxm.syxmmc = dt_xm[k]["syxmmc"];
                                    oxm.sfyx = dt_xm[k]["sfyx"];
                                    listxm.Add(oxm);
                                }
                            }
                            t_ob.xmrows = listxm;
                            if (listxm.Count > 0)
                                listzzfl.Add(t_ob);
                        }
                    }
                    ob.rows = listzzfl;
                    if (listzzfl.Count > 0)
                        listob.Add(ob);
                }
            }

            return listob;
        }

        /// <summary>
        /// 保存检测机构信息，资质，试验项目
        /// </summary>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveJcjgXX(string qybh, JcjtInitJcjg obj_jg, string Json)
        {
            string sql = string.Format("update i_m_qy set QYMC='{0}',LXDH='{1}',QYFZR='{2}',ZCD4='{3}',ZCDYB='{4}',LXSJ='{5}',JGSX='{6}',ZZJGDM='{7}',WTDBHMS='{8}',DJDBHMS='{9}',SYBHMS='{10}',SYBGBHMS='{11}' where QYBH='{12}", obj_jg.qymc, obj_jg.lxdh, obj_jg.qyfzr, obj_jg.qydd, obj_jg.zcdyb, obj_jg.lxsj, obj_jg.jgsx, obj_jg.zzjgdm, obj_jg.wtdbhms, obj_jg.djdbhms, obj_jg.sybhms, obj_jg.sybgbhms, qybh);
            CommonDao.ExecSqlTran(sql);

            IList<JcjtJcjgZZ> listob = new List<JcjtJcjgZZ>();
            listob = JsonConvert.DeserializeObject<List<JcjtJcjgZZ>>(Json);

            for (int i = 0; i < listob.Count; i++)
            {
                JcjtJcjgZZ zz = listob[i];
                if (zz.sfyx == "1")  //一级资质
                {
                    string xsflbh = zz.xsflbh;
                    sql = string.Format("update PR_M_SYXMXSFL set sfyx=1 where ssdwbh='{0}' and xsflbh='{1}'", qybh, xsflbh);
                    CommonDao.ExecSqlTran(sql);

                    List<JcjtJcjgZZFL> listzzfl = zz.rows; //二级分类
                    for (int j = 0; j < listzzfl.Count; j++)
                    {
                        List<JcjtJcjgXM> xmrows = listzzfl[j].xmrows;  //三级试验项目
                        for (int k = 0; k < xmrows.Count; k++)
                        {
                            if (xmrows[k].sfyx == "1")
                            {
                                string syxmbh = xmrows[k].syxmbh;
                                sql = string.Format("update pr_m_syxm set sfyx=1 where ssdwbh='{0}' and syxmbh='{1}' and xsflbh1='{2}'", qybh, syxmbh, xsflbh);
                                CommonDao.ExecSqlTran(sql);
                            }
                        }
                    }
                }

            }
            return true;

        }

        public IList<JcjtJcjgZZ> GetJcjgZZFL()
        {
            string sql = "select * from PR_M_SYXMXSFL where ssdwbh='' order by xsflbh";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            IList<JcjtJcjgZZ> listob = new List<JcjtJcjgZZ>();
            for (int i = 0; i < dt.Count; i++)
            {
                if (dt[i]["sjxsflbh"] == "")
                {
                    JcjtJcjgZZ ob = new JcjtJcjgZZ();
                    ob.xsflbh = dt[i]["xsflbh"];
                    ob.xsflmc = dt[i]["xsflmc"];
                    ob.sfyx = dt[i]["sfyx"];
                    List<JcjtJcjgZZFL> listzzfl = new List<JcjtJcjgZZFL>();
                    for (int j = 0; j < dt.Count; j++)
                    {
                        if (dt[j]["sjxsflbh"] == dt[i]["xsflbh"])
                        {
                            JcjtJcjgZZFL t_ob = new JcjtJcjgZZFL();
                            t_ob.sjxsflbh = dt[j]["sjxsflbh"];
                            t_ob.xsflbh = dt[j]["xsflbh"];
                            t_ob.xsflmc = dt[j]["xsflmc"];
                            t_ob.sfyx = dt[j]["sfyx"];
                            List<JcjtJcjgXM> listxm = new List<JcjtJcjgXM>();

                            t_ob.xmrows = listxm;
                            listzzfl.Add(t_ob);
                        }
                    }
                    ob.rows = listzzfl;
                    if (listzzfl.Count > 0)
                        listob.Add(ob);
                }
            }

            return listob;
        }
        public bool InitJcjg(JcjtInitJcjg obj, out CreateZh obj_zh)
        {
            bool code = true;
            string zh = obj.qymc;// Guid.NewGuid().ToString("N");
            string qymc = obj.qymc;
            string sql = "", username = "", realname = "", postcode = "", companycode = "", depcode = "", rolecode = "", msg = "";
            IList<IDictionary<string, string>> dt = null;
            obj_zh = new CreateZh();
            // 查找企业信息
            sql = "select qybh from i_m_qy where qymc='" + qymc + "'";
            dt = CommonDao.GetDataTable(sql);
            if (dt.Count > 0)
            {
                msg = "该检测机构已经创建";
                code = false;
            }
            username = zh;
            if (username == "")
                username = zh;
            realname = qymc;

            string sqlrole = "select * from h_qylx where lxbh='01'";
            IList<IDictionary<string, string>> dtrole = CommonDao.GetDataTable(sqlrole);
            if (dtrole.Count > 0)
            {
                companycode = dtrole[0]["zhdwbh"].GetSafeString();
                depcode = dtrole[0]["zhbmbh"].GetSafeString();
                rolecode = dtrole[0]["zhjsbh"];
                postcode = dtrole[0]["gwbh"].GetSafeString();
            }

            string password = Configs.GetConfigItem("defpass");
            if (password == "")
                password = RandomNumber.GetNew(RandomType.NumberAndChar, 6);
            obj_zh.companycode = companycode;
            obj_zh.depcode = depcode;
            obj_zh.username = username;
            obj_zh.realname = realname;
            obj_zh.rolecode = rolecode;
            obj_zh.postcode = postcode;
            obj_zh.password = password;
            obj_zh.msg = msg;
            return code;
        }

        [Transaction(ReadOnly = false)]
        public bool CreateJcjg(JcjtInitJcjg obj_jg, string yhzh)
        {
            string qybh = Guid.NewGuid().ToString("N");
            string sql_qy = string.Format("INSERT INTO I_M_QY([LXBH],[QYBH],[QYMC],[LXDH],[QYFZR],[LRRZH],[LRRXM],[LRSJ],[ZH],[ZCD4],[ZCDYB],[LXSJ],[JGSX],[ZZJGDM],WTDBHMS,DJDBHMS,SYBHMS,SYBGBHMS) values('01','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", qybh, obj_jg.qymc, obj_jg.lxdh, obj_jg.qyfzr, CurrentUser.UserName, CurrentUser.RealName, DateTime.Now.ToString("yyyy-MM-dd"), obj_jg.qymc, obj_jg.qydd, obj_jg.zcdyb, obj_jg.lxsj, obj_jg.jgsx, obj_jg.zzjgdm, obj_jg.wtdbhms, obj_jg.djdbhms, obj_jg.sybhms, obj_jg.sybgbhms);
            bool code = CommonDao.ExecSqlTran(sql_qy);
            if (code)
            {
                string sql = string.Format("insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('{0}','{1}',1,'{2}','{3}',getdate(),'Q')", qybh, yhzh, CurrentUser.UserName, CurrentUser.RealName);
                code = CommonDao.ExecSqlTran(sql);
                InitQyzz(qybh, obj_jg.qymc);
            }
            return code;
        }

        /// <summary>
        /// 初始化检测机构资质,试验项目,SFYX 设置为0
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="qymc"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool InitQyzz(string qybh, string qymc)
        {
            string sql = string.Format("INSERT INTO PR_M_SYXMXSFL ([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) SELECT '{0}','{1}',[SJXSFLBH],[XSFLBH],[XSFLMC],0,[XSSX]FROM [dbo].[PR_M_SYXMXSFL] where ssdwbh='' ", qybh, qymc);
            bool code = CommonDao.ExecSqlTran(sql);
            if (code)
            {
                string sql_xm = string.Format("INSERT INTO PR_M_SYXM([SSDWBH],[SSDWMC],[XSFLBH1],[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFDCBD],[SFYX],[XSSX],[WTDLRBJ],[YXFB],[XCXM],[WTDDYFS],[XMLX],[JGBGTXSJ],[EWMGL],[SCJZQYTP],[SCBG]) select '{0}','{1}',[XSFLBH1],[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFDCBD],0,[XSSX],[WTDLRBJ],[YXFB],[XCXM],[WTDDYFS],[XMLX],[JGBGTXSJ],[EWMGL],[SCJZQYTP],[SCBG] from PR_M_SYXM where ssdwbh=''", qybh, qymc);
                code = CommonDao.ExecSqlTran(sql_xm);
            }
            return code;
        }

        public IList<IDictionary<string, string>> GetJcbhmsfs()
        {
            string sql = "select bhmsmc,bhmsdm from h_jc_bhmsfs";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }

        public bool SetSyxmbhms(string qybh, string syxmbh, string syxmmc, string bh1, string bh2, string bh3, string bh4)
        {
            string sql = "";
            bool code = true;
            if (bh1 != "")  //登记单编号代码
            {
                string s = string.Format("select * from PR_M_WTDWJH where ssdwbh='{0}' and syxmbh='{1}' and lx='DJ'", qybh, syxmbh);
                if (CommonDao.GetDataTable(s).Count == 0)
                    sql = string.Format("INSERT INTO PR_M_WTDWJH([SSDWBH],[SSDWMC],[SYXMBH],[SYXMMC],[SFYX],[LRSJ],[XMDH],[LX]) select '{0}',qymc,'{1}','{2}',1,getdate(),'{3}','DJ' from i_m_qy where qybh='{4}'", qybh, syxmbh, syxmmc, bh1, qybh);
                else
                    sql = string.Format("update PR_M_WTDWJH set xmdh='{0}' where ssdwbh='{1}' and syxmbh='{2}' and lx='DJ'", bh1, qybh, syxmbh);
                code = CommonDao.ExecSql(sql);
                if (!code)
                    return false;
            }
            if (bh2 != "")  //委托单编号代码
            {
                string s = string.Format("select * from PR_M_WTDWJH where ssdwbh='{0}' and syxmbh='{1}' and lx='WT'", qybh, syxmbh);
                if (CommonDao.GetDataTable(s).Count == 0)
                    sql = string.Format("INSERT INTO PR_M_WTDWJH([SSDWBH],[SSDWMC],[SYXMBH],[SYXMMC],[SFYX],[LRSJ],[XMDH],[LX]) select '{0}',qymc,'{1}','{2}',1,getdate(),'{3}','WT' from i_m_qy where qybh='{4}'", qybh, syxmbh, syxmmc, bh2, qybh);
                else
                    sql = string.Format("update PR_M_WTDWJH set xmdh='{0}' where ssdwbh='{1}' and syxmbh='{2}' and lx='WT'", bh2, qybh, syxmbh);
                code = CommonDao.ExecSql(sql);
                if (!code)
                    return false;
            }
            if (bh3 != "")  //试验编号代码
            {
                string s = string.Format("select * from PR_M_WTDWJH where ssdwbh='{0}' and syxmbh='{1}' and lx='SY'", qybh, syxmbh);
                if (CommonDao.GetDataTable(s).Count == 0)
                    sql = string.Format("INSERT INTO PR_M_WTDWJH([SSDWBH],[SSDWMC],[SYXMBH],[SYXMMC],[SFYX],[LRSJ],[XMDH],[LX]) select '{0}',qymc,'{1}','{2}',1,getdate(),'{3}','SY' from i_m_qy where qybh='{4}'", qybh, syxmbh, syxmmc, bh3, qybh);
                else
                    sql = string.Format("update PR_M_WTDWJH set xmdh='{0}' where ssdwbh='{1}' and syxmbh='{2}' and lx='SY'", bh3, qybh, syxmbh);
                code = CommonDao.ExecSql(sql);
                if (!code)
                    return false;
            }
            if (bh4 != "")  //实验报告编号代码
            {
                string s = string.Format("select * from PR_M_WTDWJH where ssdwbh='{0}' and syxmbh='{1}' and lx='BG'", qybh, syxmbh);
                if (CommonDao.GetDataTable(s).Count == 0)
                    sql = string.Format("INSERT INTO PR_M_WTDWJH([SSDWBH],[SSDWMC],[SYXMBH],[SYXMMC],[SFYX],[LRSJ],[XMDH],[LX]) select '{0}',qymc,'{1}','{2}',1,getdate(),'{3}','BG' from i_m_qy where qybh='{4}'", qybh, syxmbh, syxmmc, bh4, qybh);
                else
                    sql = string.Format("update PR_M_WTDWJH set xmdh='{0}' where ssdwbh='{1}' and syxmbh='{2}' and lx='BG'", bh4, qybh, syxmbh);
                code = CommonDao.ExecSql(sql);
                if (!code)
                    return false;
            }
            return true;
        }

        public IList<IDictionary<string, string>> GetSyxmbhms(string qybh, string syxmbh)
        {
            string sql = string.Format("select SYXMBH,SYXMMC,XMDH,LX from PR_M_WTDWJH where ssdwbh='{0}' and syxmbh='{1}'", qybh, syxmbh);
            return CommonDao.GetDataTable(sql);
        }

        public IList<IDictionary<string, IList<IDictionary<string, string>>>> GetJcjgSyxmbz(string qybh, string syxmbh)
        {
            IList<IDictionary<string, IList<IDictionary<string, string>>>> list = new List<IDictionary<string, IList<IDictionary<string, string>>>>();
            string sql_cp = string.Format("select distinct SYXMBH,CPMC from PR_M_CP where  syxmbh='{0}'", syxmbh);
            IList<IDictionary<string, string>> dt_cp = CommonDao.GetDataTable(sql_cp);
            string sql_qy_cp = string.Format("select * from pr_s_xm_cp where  syxmbh='{0}' and  ssdwbh='{1}'", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_qy_cp = CommonDao.GetDataTable(sql_qy_cp);
            for (int i = 0; i < dt_cp.Count; i++)
            {
                IDictionary<string, string> t = dt_cp[i];
                for (int j = 0; j < dt_qy_cp.Count; j++)
                {
                    if (dt_cp[i]["cpmc"] == dt_qy_cp[j]["cpmc"])
                    {
                        t.Add("check", "1");
                        break;
                    }
                    else if (j == dt_qy_cp.Count - 1)
                    {
                        t.Add("check", "0");
                    }
                }
            }
            IDictionary<string, IList<IDictionary<string, string>>> dt1 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt1.Add("cp", dt_cp);
            list.Add(dt1);


            string sql_zb = string.Format("select RECID,SYXMBH,ZBMC from PR_M_ZB where syxmbh='{0}'", syxmbh);
            IList<IDictionary<string, string>> dt_zb = CommonDao.GetDataTable(sql_zb);
            string sql_qy_zb = string.Format("select * from pr_s_xm_zb where syxmbh='{0}' and  ssdwbh='{1}'", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_qy_zb = CommonDao.GetDataTable(sql_qy_zb);
            for (int i = 0; i < dt_zb.Count; i++)
            {
                IDictionary<string, string> t = dt_zb[i];
                for (int j = 0; j < dt_qy_zb.Count; j++)
                {
                    if (dt_zb[i]["recid"] == dt_qy_zb[j]["zbbh"])
                    {
                        t.Add("check", "1");
                        break;
                    }
                    else if (j == dt_qy_zb.Count - 1)
                    {
                        t.Add("check", "0");
                    }
                }
            }

            IDictionary<string, IList<IDictionary<string, string>>> dt2 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt2.Add("zb", dt_zb);
            list.Add(dt2);



            string sql_bz = string.Format("select RECID,SYXMBH,BZMC from PR_M_BZ where syxmbh='{0}'", syxmbh);
            IList<IDictionary<string, string>> dt_bz = CommonDao.GetDataTable(sql_bz);
            string sql_qy_bz = string.Format("select * from pr_s_xm_bz from PR_M_BZ where syxmbh='{0}' and  ssdwbh='{1}' ", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_qy_bz = CommonDao.GetDataTable(sql_qy_bz);
            for (int i = 0; i < dt_bz.Count; i++)
            {
                IDictionary<string, string> t = dt_bz[i];
                for (int j = 0; j < dt_qy_bz.Count; j++)
                {
                    if (dt_bz[i]["recid"] == dt_qy_bz[j]["bzbh"])
                    {
                        t.Add("check", "1");
                        break;
                    }
                    else if (j == dt_qy_bz.Count - 1)
                    {
                        t.Add("check", "0");
                    }
                }
            }

            IDictionary<string, IList<IDictionary<string, string>>> dt3 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt3.Add("bz", dt_bz);
            list.Add(dt3);
            return list;
        }

        public IList<IDictionary<string, IList<IDictionary<string, string>>>> GetJcjgSyxmbz_Byqybh(string qybh, string syxmbh)
        {
            IList<IDictionary<string, IList<IDictionary<string, string>>>> list = new List<IDictionary<string, IList<IDictionary<string, string>>>>();
            string sql_cp = string.Format("select * from pr_s_xm_cp where  syxmbh='{0}' and  ssdwbh='{1}'", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_cp = CommonDao.GetDataTable(sql_cp);
            IDictionary<string, IList<IDictionary<string, string>>> dt1 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt1.Add("cp", dt_cp);
            list.Add(dt1);
            string sql_zb = string.Format("select * from pr_s_xm_zb where syxmbh='{0}' and  ssdwbh='{1}'", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_zb = CommonDao.GetDataTable(sql_zb);
            IDictionary<string, IList<IDictionary<string, string>>> dt2 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt2.Add("zb", dt_zb);
            list.Add(dt2);
            string sql_bz = string.Format("select * from pr_s_xm_bz from PR_M_BZ where syxmbh='{0}' and  ssdwbh='{1}' ", syxmbh, qybh);
            IList<IDictionary<string, string>> dt_bz = CommonDao.GetDataTable(sql_bz);
            IDictionary<string, IList<IDictionary<string, string>>> dt3 = new Dictionary<string, IList<IDictionary<string, string>>>();
            dt3.Add("bz", dt_bz);
            list.Add(dt3);
            return list;
        }

        [Transaction(ReadOnly = false)]
        public bool SetJcjgSyxmsj(string qybh, string syxmbh, string cprows, string zbrows, string bzrows)
        {
            List<string> sqls = new List<string>();
            if (cprows != "")
            {
                string sql_del = string.Format("delete from pr_s_xm_cp where ssdwbh='{0}' and syxmbh='{1}'", qybh, syxmbh);
                CommonDao.ExecCommand(sql_del, CommandType.Text);
                JToken jsons = JToken.Parse(cprows);  //[]表示数组，{} 表示对象
                foreach (JToken baseJ in jsons)//遍历数组
                {
                    string cpmc = baseJ.Value<string>("cpmc");
                    string sql_in = string.Format("Insert into pr_s_xm_cp (ssdwbh,syxmbh,cpmc) values ('{0}','{1}','{2}')", qybh, syxmbh, cpmc);
                    CommonDao.ExecCommand(sql_in, CommandType.Text);
                }
            }
            if (zbrows != "")
            {
                string sql_del = string.Format("delete from pr_s_xm_zb where ssdwbh='{0}' and syxmbh='{1}'", qybh, syxmbh);
                CommonDao.ExecCommand(sql_del, CommandType.Text);
                JToken jsons = JToken.Parse(zbrows);  //[]表示数组，{} 表示对象
                foreach (JToken baseJ in jsons)//遍历数组
                {
                    string zbbh = baseJ.Value<string>("zbbh");
                    string zbmc = baseJ.Value<string>("zbmc");

                    string sql_in = string.Format("Insert into pr_s_xm_zb (ssdwbh,syxmbh,zbbh,zbmc) values ('{0}','{1}','{2}','{3}')", qybh, syxmbh, zbbh, zbmc);
                    CommonDao.ExecCommand(sql_in, CommandType.Text);

                }
            }
            if (bzrows != "")
            {
                string sql_del = string.Format("delete from pr_s_xm_bz where ssdwbh='{0}' and syxmbh='{1}'", qybh, syxmbh);
                CommonDao.ExecCommand(sql_del, CommandType.Text);
                JToken jsons = JToken.Parse(bzrows);  //[]表示数组，{} 表示对象
                foreach (JToken baseJ in jsons)//遍历数组
                {
                    string bzbh = baseJ.Value<string>("bzbh");
                    string bzmc = baseJ.Value<string>("bzmc");
                    string sql_in = string.Format("Insert into pr_s_xm_bz (ssdwbh,syxmbh,bzbh,bzmc) values ('{0}','{1}','{2}','{3}')", qybh, syxmbh, bzbh, bzmc);
                    CommonDao.ExecCommand(sql_in, CommandType.Text);
                }
            }
            return true;
        }

        public IList<IDictionary<string, string>> GetJcjgxx(string qybh)
        {
            string sql = "select [LXBH],[QYBH],[QYMC],[LXDH],[QYFZR],[LRRZH],[LRRXM],[LRSJ],[ZH],[ZCD4],[ZCDYB],[LXSJ],[JGSX],[ZZJGDM],WTDBHMS,DJDBHMS,SYBHMS,SYBGBHMS from i_m_qy where qybh='" + qybh + "' ";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }

        public IList<IDictionary<string, string>> GetJcjglb(string key, int pagesize, int pageindex, out int totalCount)
        {
            totalCount = 0;
            string where = "";
            if (key != "")
                where += " and qymc like '%" + key + "%'";
            string sql = "select [LXBH],[QYBH],[QYMC],[LXDH],[QYFZR],[LRRZH],[LRRXM],[LRSJ],[ZH],[ZCD4],[ZCDYB],[LXSJ],[JGSX],[ZZJGDM],WTDBHMS,DJDBHMS,SYBHMS,SYBGBHMS from i_m_qy " + where;
            IList<IDictionary<string, string>> dt = CommonDao.GetPageData(sql, pagesize, pageindex, out totalCount);
            return dt;
        }

        [Transaction(ReadOnly = false)]
        public bool CreateJcZZ(string ZZname, string fl1, string fl2, string fl3, string fl4, string xssx)
        {
            string sql = "select max(SJXSFLBH) as num from PR_M_SYXMXSFL";
            int m_SJXSFLBH = CommonDao.GetDataTable(sql)[0]["num"].GetSafeInt() + 1;

            if (xssx == "")
                xssx = "1";
            sql = string.Format("INSERT INTO PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) Values ('','','','{0}','{1}',1,{2})", m_SJXSFLBH, ZZname, xssx);
            CommonDao.ExecCommand(sql, CommandType.Text);
            if (fl1 != "")
            {
                string XSFLBH = m_SJXSFLBH.ToString() + "01";
                sql = string.Format("INSERT INTO PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) Values ('','','{0}','{1}','{2}',1,1)", m_SJXSFLBH, XSFLBH, fl1);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            if (fl2 != "")
            {
                string XSFLBH = m_SJXSFLBH.ToString() + "02";
                sql = string.Format("INSERT INTO PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) Values ('','','{0}','{1}','{2}',1,2)", m_SJXSFLBH, XSFLBH, fl2);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            if (fl3 != "")
            {
                string XSFLBH = m_SJXSFLBH.ToString() + "03";
                sql = string.Format("INSERT INTO PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) Values ('','','{0}','{1}','{2}',1,3)", m_SJXSFLBH, XSFLBH, fl3);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            if (fl4 != "")
            {
                string XSFLBH = m_SJXSFLBH.ToString() + "04";
                sql = string.Format("INSERT INTO PR_M_SYXMXSFL([SSDWBH],[SSDWMC],[SJXSFLBH],[XSFLBH],[XSFLMC],[SFYX],[XSSX]) Values ('','','{0}','{1}','{2}',1,4)", m_SJXSFLBH, XSFLBH, fl4);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            return true;
        }
        /// <summary>
        /// 创建试验项目
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateSyxm(jcjtSyxm obj, out string msg)
        {
            bool code = true;
            msg = "";
            string sql = string.Format("select * from PR_M_SYXM where SYXMBH='{0}' and ssdwbh=''", obj.syxmbh);
            if (CommonDao.GetDataTable(sql).Count == 0)
            {
                sql = string.Format("INSERT INTO PR_M_SYXM([SSDWBH],[SSDWMC],[XSFLBH1],[XSFLBH],[SYXMBH],[SYXMMC],[FXBH],[FXMC],[CNQX],[JLGS],[CBZDZS],[SFXYYPJJ],[SFDCBD],[SFYX],[XSSX],[WTDLRBJ],[YXFB],[XCXM],[WTDDYFS],[XMLX],[JGBGTXSJ],[EWMGL],[SCJZQYTP],[SCBG],[SFXYTJRY]) select [SSDWBH],[SSDWMC],'{0}','{1}','{2}','{3}','{4}','{5}',5,'',1,0,{6},{7},{8},'leftright',0,{9},{10},'{11}','{12}',1,{13},{14},{15}from PR_M_SYXM group by [SSDWBH],[SSDWMC]", obj.xsflbh1, obj.xsflbh, obj.syxmbh, obj.syxmmc, obj.fxbh, obj.fxmc, obj.sfxyfb, obj.sfyx, obj.xssx, obj.sfxcxm, obj.wtddyfs, obj.xmlx, obj.jgbgtxsj, obj.scjzqytp, obj.scbg, obj.sfxytjry);
                code = CommonDao.ExecSql(sql);
            }
            else
            {
                code = false;
                msg = "该试验项目编号已存在，不能重复创建";
            }
            return code;
        }

        public IList<IDictionary<string, string>> GetH_JCJG(string usercode)
        {
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            dt = CommonDao.GetDataTable("select cpcode,depcode from H_JCJG where JCJGBH=(select qybh from i_m_qyzh where yhzh='" + usercode + "')");
            return dt;
        }


        #region 用户系统操作
        private string umsurl = Configs.GetConfigItem("umsurl");

        public string AddUser(string username, string realname, string password, string sfzh, string xb, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string rolecodelist, string syxmjson)
        {

            string err = "";
            string ret = "";
            string timestring = GetTimeStamp();
            string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
            string url = umsurl + "/Api/Service";
            string dates = "method=User&opt=AddUser&username=" + username + "&realname=" + realname + "&sfzh=" + sfzh + "&password=" + password + "&cpcode=" + cpcode + "&depcode=" + depcode + "&rolecodelist=" + rolecodelist + "&postdm=" + postdm + "&timestring=" + timestring + "&sign=" + sign;
            ret = SendDataByPost(url, dates);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            UmsRet umsret = jss.Deserialize<UmsRet>(ret);
            if (umsret.success)
            {
                Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                string rybh = Guid.NewGuid().ToString("N");
                string sql = "INSERT INTO I_M_NBRY_JC([ZH],[USERCODE],CPCODE,[JCJGBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX],LRSJ,SSKSBH) SELECT '" + username + "','" + param["usercode"] + "','" + cpcode + "',qybh,'" + rybh + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1,getdate(),'" + ksbh + "' from View_I_M_JCZH where yhzh='" + CurrentUser.UserCode + "'";  //'UR201907000002'";  //
                CommonDao.ExecSql(sql);

                sql = "INSERT INTO [I_M_QYZH]([QYBH],[YHZH],[SFQYZZH],[LRRZH],[LRRXM],[LRSJ],[ZHLX]) select '" + rybh + "','" + param["usercode"] + "',0,'" + CurrentUser.UserName + "','" + CurrentUser.RealName + "',getdate(),'R' from i_m_qyzh where yhzh='UR201907000002'";//='" + CurrentUser.UserCode + "'";
                CommonDao.ExecSql(sql);
                if (!string.IsNullOrEmpty(syxmjson))
                    SaveJCRYSYXM_INIT(rybh, syxmjson);
            }
            return ret;
        }
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }
        public string SendDataByPost(string Url, string datas)
        {
            string retString = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                byte[] data = Encoding.UTF8.GetBytes(datas);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    retString = reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }

        public string CheckUserBySfzh(string sfzhm)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=CheckUserBySfzh&sfzh=" + sfzhm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string ModifyUserStatusByUsercode(string usercode, string userstatus)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=ModifyUserStatusByUsercode&usercode=" + usercode + "&userstatus=" + userstatus + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    CommonDao.ExecSql("update i_m_nbry_jc set SFYX=" + userstatus + " where usercode='" + usercode + "'");

                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        //根据角色代码获取所有的用户及已经有此角色的用户标志
        public string GetOwnerRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetOwnerRoleListByUsercode&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }
        public string GetRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleListByUsercode&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string GetProcodeAndMenuByUsercode(string usercode)
        {
            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetProcodeAndMenuByUsercode&usercode=" + usercode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string GetRoleList(string page, string rows, string usercode, string cpcode, string cpname, string procode, string proname, string rolename, string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleList&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&cpname=" + cpname + "&proname=" + proname + "&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string AddRoleInfo(string cpcode, string procode, string rolename, string memo)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=AddRoleInfo&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&memo=" + memo + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetOwnerUserListByRolecode(string page, string rows, string rolecode, string cpcode, string username, string realname)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetOwnerUserListByRolecode&page=" + page + "&rows=" + rows + "&rolecode=" + rolecode + "&cpcode=" + cpcode + "&username=" + username + "&realname=" + realname + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string procode, string rolecodelist, string clearrole, string sfzhm)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=ModifyUserInfoByUsercode&username=" + username + "&realname=" + realname + "&usercode=" + usercode + "&procode=" + procode + "&cpcode=" + cpcode + "&depcode=" + depcode + "&postdm=" + postdm + "&rolecodelist=" + rolecodelist + "&clearrole=" + clearrole + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    // string sql = "update I_M_NBRY_JC set zh='" + username + "',zjzbh='" + cpcode + "',ryxm='" + realname + "',sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "' where rybh='" + usercode + "'";

                    // string sql = "INSERT INTO I_M_NBRY_JC([ZH],[USERCODE],CPCODE,[JCJGBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX],LRSJ,SSKSBH) SELECT '" + username + "','" + param["usercode"] + "','" + cpcode + "',qybh,'" + rybh + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1,getdate(),'" + ksbh + "' from View_I_M_JCZH where yhzh='" + CurrentUser.UserCode + "'";  //'UR201907000002'";  //
                    string sql = "update I_M_NBRY_JC set sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "', SSKSBH ='" + ksbh + "' where usercode='" + usercode + "'";
                    CommonDao.ExecSql(sql);
                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }
        public string ModifyUserRoleByRolecodeAndUsercodeList(string rolecode, string usercodelist)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=UserRole&opt=ModifyUserRoleByRolecodeAndUsercodeList&rolecode=" + rolecode + "&usercodelist=" + usercodelist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        //
        public string ModifyUserRoleByUsercodeAndRolecodeList(string usercode, string rolecodelist)
        {
            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=UserRole&opt=ModifyUserRoleByUsercodeAndRolecodeList&usercode=" + usercode + "&rolecodelist=" + rolecodelist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string GetOwnerPowerListByRolecode(string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=GetOwnerPowerListByRolecode&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string SavePowerByRolecode(string rolecode, string menulist)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=SavePowerByRolecode&rolecode=" + rolecode + "&menulist=" + menulist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public string SavePowerByRolecode(string rolecode, string menulist, string butlist)
        {

            string err = "";
            string ret = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                List<string> sqls = new List<string>();
                string sqlStr = "";
                var dt = CommonDao.GetDataTable($"select * from  M_MENU_BUTTON_ROLE where rolecode = '{rolecode}' and DWBH = '{CurrentUser.Qybh}'");
                if (!string.IsNullOrEmpty(butlist))
                {
                    var btnRoleList = jss.Deserialize<List<Dictionary<string, string>>>(butlist);
                    IDictionary<string, string> dtItem = null;
                    foreach (var item in btnRoleList.Where(u => u.Keys.Contains("btncode") && !string.IsNullOrEmpty(u["btncode"])).Distinct())
                    {
                        dtItem = dt.FirstOrDefault(u => u["menucode"] == item["menucode"] && u["btncode"] == item["btncode"]);
                        if (dtItem == null)
                            sqlStr += $"('{item["menucode"]}','{item["btncode"]}','{rolecode}','{CurrentUser.Qybh}'),";
                        else
                            dt.Remove(dtItem);
                    }
                    if (sqlStr.Length > 0)
                        sqls.Add("INSERT INTO M_MENU_BUTTON_ROLE(MENUCODE,BTNCODE,ROLECODE,DWBH) VALUES " + sqlStr.TrimEnd(','));
                }
                if (dt.Count > 0)
                {
                    sqlStr = $"delete from M_MENU_BUTTON_ROLE where RECID IN ({string.Join(",", dt.Select(u => u["recid"]).ToList()).FormatSQLInStr()})";
                    sqls.Add(sqlStr);
                }
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=SavePowerByRolecode&rolecode=" + rolecode + "&menulist=" + menulist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";
                var resdata = jss.Deserialize<Dictionary<string, object>>(ret);
                if (resdata == null || !resdata["success"].GetSafeBool() || sqls.Count == 0)
                    return ret;
                else
                {
                    sqls.ForEach(u => CommonDao.ExecSqlTran(u));
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }
        public string GetPowerListByRolecode(string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=GetPowerListByRolecode&rows=1000&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }



        public string GetUserListByMenucode(string page, string rows, string procode, string cpcode, string menucode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListByMenucode&page=" + page + "&rows=" + rows + "&procode=" + procode + "&menucode=" + menucode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetRoleListByMenucode(string page, string rows, string procode, string menucode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleListByMenucode&page=" + page + "&rows=" + rows + "&procode=" + procode + "&menucode=" + menucode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetUserListByRolecode(string page, string rows, string rolecode, string cpcode, string realname)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListByRolecode&page=" + page + "&rows=" + rows + "&rolecode=" + rolecode + "&cpcode=" + cpcode + "&realname=" + realname + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }






        public string GetUserListBySfzh(string sfzhm)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListBySfzh&sfzh=" + sfzhm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    //Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(umsret.data);
                    if (param.Count > 0)
                    {
                        ret = param[0]["username"].GetSafeString();
                    }
                }
                else
                {
                    ret = "";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        /// <summary>
        /// 获取检测人员已有的试验项目
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<JcjtJcjgZZ> GetJCRYZZxx_Byrybh(string rybh, int status = 1)
        {
            string where1 = "";
            string where2 = "";
            if (status == 1)
            {
                where1 += " and ((sjxsflbh='' and sfyx=1) or sjxsflbh<>'')";
                where2 += " and sfyx=1";
            }
            string sql = "select * from PR_M_SYXMXSFL where ssdwbh='" + CurrentUser.Qybh + "'" + where1 + " order by xsflbh";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            string sql_xm = "select * from pr_m_syxm where ssdwbh='" + CurrentUser.Qybh + "'" + where2 + " order by xsflbh1,xsflbh ";
            IList<IDictionary<string, string>> dt_xm = CommonDao.GetDataTable(sql_xm);
            string sql_ry = "select * from I_S_NBRY_SYXM where rybh='" + rybh + "'";
            IList<IDictionary<string, string>> dt_ry = CommonDao.GetDataTable(sql_ry);

            IList<JcjtJcjgZZ> listob = new List<JcjtJcjgZZ>();
            for (int i = 0; i < dt.Count; i++)
            {
                if ((dt[i]["sjxsflbh"] == "" && status == 0) || (dt[i]["sjxsflbh"] == "" && status == 1 && dt[i]["sfyx"].ToLower() == "true"))
                {
                    JcjtJcjgZZ ob = new JcjtJcjgZZ();
                    ob.xsflbh = dt[i]["xsflbh"];
                    ob.xsflmc = dt[i]["xsflmc"];
                    ob.sfyx = dt[i]["sfyx"];
                    List<JcjtJcjgZZFL> listzzfl = new List<JcjtJcjgZZFL>();
                    for (int j = 0; j < dt.Count; j++)
                    {
                        if (dt[j]["sjxsflbh"] == dt[i]["xsflbh"])
                        {
                            JcjtJcjgZZFL t_ob = new JcjtJcjgZZFL();
                            t_ob.sjxsflbh = dt[j]["sjxsflbh"];
                            t_ob.xsflbh = dt[j]["xsflbh"];
                            t_ob.xsflmc = dt[j]["xsflmc"];
                            t_ob.sfyx = dt[j]["sfyx"];
                            List<JcjtJcjgXM> listxm = new List<JcjtJcjgXM>();
                            for (int k = 0; k < dt_xm.Count; k++)
                            {
                                if (dt_xm[k]["xsflbh1"] == dt[i]["xsflbh"] && dt_xm[k]["xsflbh"] == dt[j]["xsflbh"])
                                {
                                    JcjtJcjgXM oxm = new JcjtJcjgXM();
                                    oxm.syxmbh = dt_xm[k]["syxmbh"];
                                    oxm.syxmmc = dt_xm[k]["syxmmc"];
                                    for (int h = 0; h < dt_ry.Count; h++)
                                    {
                                        if (dt_xm[k]["syxmbh"] == dt_ry[h]["syxmbh"])
                                        {
                                            oxm.sfyx = "True";
                                            break;
                                        }
                                        else
                                            oxm.sfyx = "False";
                                    }
                                    listxm.Add(oxm);
                                }
                            }
                            t_ob.xmrows = listxm;
                            if (listxm.Count > 0)
                                listzzfl.Add(t_ob);
                        }
                    }
                    ob.rows = listzzfl;
                    if (listzzfl.Count > 0)
                        listob.Add(ob);
                }
            }

            return listob;
        }

        /// <summary>
        /// 创建用户时 设置检测人员试验项目
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool SaveJCRYSYXM_INIT(string rybh, string json)
        {
            bool code = true;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<JcjtJcjgXM> list = jss.Deserialize<List<JcjtJcjgXM>>(json);
            for (int i = 0; i < list.Count; i++)
            {
                string syxmbh = list[i].syxmbh;
                string syxmmc = list[i].syxmmc;
                string sql = string.Format("INSERT INTO I_S_NBRY_SYXM ([RYBH],[SYXMBH],[SYXMMC]) values ('{0}','{1}','{2}')", rybh, syxmbh, syxmmc);
                CommonDao.ExecSql(sql);
            }
            return true;
        }
        /// <summary>
        /// 变更检测人员试验项目
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveJCRYSYXM(string usercode, string json)
        {
            bool code = true;
            string sql_del = "delete from I_S_NBRY_SYXM where usercode='" + usercode + "'";
            CommonDao.ExecCommand(sql_del, CommandType.Text);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<JcjtJcryRoleSYXM> list = jss.Deserialize<List<JcjtJcryRoleSYXM>>(json);
            for (int i = 0; i < list.Count; i++)
            {
                string rolecode = list[i].rolecode;
                string syxmbhs = list[i].syxmbhs;
                string[] bhlist = syxmbhs.Split(',');
                for (int j = 0; j < bhlist.Length; j++)
                {
                    if (!string.IsNullOrEmpty(bhlist[j]))
                    {
                        string sql = string.Format("INSERT INTO I_S_NBRY_SYXM ([USERCODE],ROLECODE,[SYXMBH]) values ('{0}','{1}','{2}')", usercode, rolecode, bhlist[j]);
                        CommonDao.ExecCommand(sql, CommandType.Text);
                    }
                }

            }
            return true;
        }




        #endregion

        #region
        #region  设置工程试验项目报备 status=0 新建  status=1 修改
        [Transaction(ReadOnly = false)]
        public bool SetGcsyxmbb(string gcbh, string jsondata, int status, string bbid)
        {
            string guid = Guid.NewGuid().ToString("N");
            string sql = "";
#if DEBUG
            string qybh = "407a4ecd6e43421babd6d0de0235fc62";
            string qymc = "测试检测机构";
#else
            string qybh = CurrentUser.Qybh;
            string qymc = CurrentUser.Qymc;
#endif

            if (status == 0)
            {
                sql = string.Format("INSERT INTO PR_M_GC_SYXMBB ([SSDWBH],[SSDWMC],[GCBH],[GCMC],[ZT],[LRRY],[LRRYXM],[LRSJ],BBid) select '{0}','{1}','{2}',gcmc,0,'{3}','{4}',getdate(),'{5}' from i_m_gc where gcbh='{6}'", qybh, qymc, gcbh, CurrentUser.UserName, CurrentUser.RealName, guid, gcbh);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            else //(status==1)
            {
                guid = bbid;
                sql = "delete from PR_S_GC_SYXMBB where parentid='" + bbid + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<JcjtJcjgXM> list = jss.Deserialize<List<JcjtJcjgXM>>(jsondata);
            for (int i = 0; i < list.Count; i++)
            {
                string syxmbh = list[i].syxmbh;
                string syxmmc = list[i].syxmmc;
                sql = string.Format("INSERT INTO PR_S_GC_SYXMBB ([ParentId],[SYXMBH],[SYXMMC],[SFYX],[XSSX]) values ('{0}','{1}','{2}',1,1)", guid, syxmbh, syxmmc);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            return true;
        }


        public IList<IDictionary<string, string>> GetGcsyxmbb(string gcbh, string bbid)
        {
            string sql = "select * from PR_S_GC_SYXMBB where ParentId='" + bbid + "'";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }
        #endregion
        #region
        public IList<JcjtJcjgZZ> GetGCBBSyxm(string bbid, int status = 1)
        {
#if DEBUG
            string qybh = "407a4ecd6e43421babd6d0de0235fc62";
            string qymc = "测试检测机构";
#else
            string qybh = CurrentUser.Qybh;
            string qymc = CurrentUser.Qymc;
#endif


            IList<JcjtJcjgZZ> bbzz = GetJcjgZZxx_Byqybh(qybh, 1);
            string sql = "select *from PR_M_GC_SYXMBB a left join  PR_S_GC_SYXMBB b on a.bbid=b.parentid where a.bbid='" + bbid + "'";
            IList<IDictionary<string, string>> bbdt = CommonDao.GetDataTable(sql);
            List<string> list = new List<string>();
            for (int i = 0; i < bbdt.Count; i++)
            {
                list.Add(bbdt[i]["syxmbh"]);
            }


            for (int i = 0; i < bbzz.Count; i++)
            {
                List<JcjtJcjgZZFL> zzfl = bbzz[i].rows;
                for (int j = 0; j < zzfl.Count; j++)
                {
                    List<JcjtJcjgXM> xmlist = zzfl[i].xmrows;
                    for (int k = 0; k < xmlist.Count; k++)
                    {
                        if (list.Contains(xmlist[k].syxmbh))
                            xmlist[k].sfyx = "True";
                        else
                            xmlist[k].sfyx = "False";
                    }
                }
            }
            return bbzz;
        }
        #endregion
        #endregion
        #region 保存试验项目合同
        [Transaction(ReadOnly = false)]
        public bool SetSyxmht(string gcbh, string jchtbh, string jsondata)
        {
            //string guid = Guid.NewGuid().ToString("");
            string sqls = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<JcjtJcjgXM> list = jss.Deserialize<List<JcjtJcjgXM>>(jsondata);
            for (int i = 0; i < list.Count; i++)
            {
                string syxmbh = list[i].syxmbh;
                string syxmmc = list[i].syxmmc;
                sqls = string.Format("INSERT INTO PR_S_HT_SYXM([GCBH],[JCHTBH],[SYXMBH],[SYXMMC],[SFYX]) values ('{0}','{1}','{2}','{3}',1)", gcbh, jchtbh, syxmbh, syxmmc);
                CommonDao.ExecSql(sqls);
            }
            return true;
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除委托单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteDjds(string ids, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (ids.Length == 0)
                {
                    ret = true;
                    return ret;
                }

                string inids = ids.Trim(new char[] { ',' }).FormatSQLInStr();
                string sql = "select s_byrecid as recid,syxmmc,djdbh,syxmbh,s_dj_byzt as zt from View_DJD_Hnt where s_byrecid in (" + inids + ")";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                string undeleteinfo = "";
                foreach (IDictionary<string, string> row in dt)
                {
                    string recid = row["recid"].GetSafeString();
                    string syxmmc = row["syxmmc"].GetSafeString();
                    string djdbh = row["djdbh"].GetSafeString();
                    string syxmbh = row["syxmbh"].GetSafeString();
                    string zt = row["zt"].GetSafeString();

                    if (zt == "1")
                    {
                        if (undeleteinfo != "")
                            undeleteinfo += "，";
                        undeleteinfo += "[" + row["syxmmc"] + "]" + row["djdbh"] + "已提交委托的登记单，无法删除";
                    }
                    else
                    {
                        List<string> sqls = new List<string>();

                        sqls.Add("delete from s_dj_by where recid='" + recid + "'");

                        sqls.Add("delete from s_dj_" + syxmbh + " where recid='" + recid + "' ");

                        foreach (string str in sqls)
                        {
                            CommonDao.ExecCommand(str, CommandType.Text);
                        }

                    }
                }

                if (undeleteinfo != "")
                    msg = undeleteinfo;
                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }
        #endregion

        #region 登记单管理

        public bool GetDjdht(string htrecid, out string recid)
        {
            bool code = false;
            recid = "";
            string sql = "select * from m_dj_by where htbh='" + htrecid + "'";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count > 0)
            {
                code = true;
                recid = dt[0]["recid"];
            }
            return code;
        }


        [Transaction(ReadOnly = false)]
        public bool DJDTjWt(string recids, out string msg)
        {
            bool code = true;
            msg = "";
            string m_dj_byrecid = "";
            string inids = recids.Trim(new char[] { ',' }).FormatSQLInStr();
            string sql = "select recid,s_syxmmc as syxmmc ,djdbh,s_syxmbh as syxmbh,s_byrecid ,zs from View_DJD_Hnt where s_byrecid in (" + inids + ")";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count > 0)
            {
                m_dj_byrecid = dt[0]["recid"].GetSafeString();
                string syxmmc = dt[0]["syxmmc"].GetSafeString();
                string syxmbh = dt[0]["syxmbh"].GetSafeString();
                string m_recid = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                string SJDBH = m_recid;
                sql = "INSERT INTO M_BY([RECID],[WTDBH],[SJDBH],[SJRECID],[SCWTS],[SCWTSDZ],[QRXZ],[JG_XMDH],[SYXMBH],[SYXMMC],[HTBH],[NBHTBH],[GCBH],[YTDWBH],[YTDWMC],[SYDWBH],[SYDWMC],[HTJCBMBH],[GCMC],[HTJCBMMC],[SYJCBMBH],[KHDWMC],[KHDWBH],[SYJCBMMC],[SGDWBH],[SYJCBMDZ],[SGDWMC],[KHDWZH],[JLDWBH],[JLDWMC],[FBHTBH],[JSDWBH],[JSDWMC],[SJDWBH],[SJDWMC],[ZT],[YSJCF],[SSZJZBH],[SSZJZMC],[SSJCF],[ZJDJH],[JM],[XGCS],[FKFSBH],[BGZS],[JCDD],[WTDGDH],[WTSLRRZH],[WTSLRRXM],[WTSLRSJ],[WTSDYRZH],[WTSDYRXM],[JZRBH],[WTSDYSJ],[JZRXM],[JZRDH],[JZRZH],[WTSFHRZH],[WTLXRXM],[WTLXRDH],[WTBZ],[JYRZH],[JYRXM],[WTLXRDZ],[GCDZ],[JYSJ],[JCLX],[YPCLFS],[BGLQFS],[YCZT],[SFXF],[WTSMB],[WTSWJH],[JCFAPZRZH],[JCFAPZRXM],[XCXMQRRZH],[XCXMQRRXM],[JCYJ],[PDBZ],[SPYSC]) select '" + m_recid + "' ,[WTDBH],'" + SJDBH + "',[SJRECID],[SCWTS],[SCWTSDZ],[QRXZ],[JG_XMDH],'" + syxmbh + "','" + syxmmc + "',[HTBH],[NBHTBH],[GCBH],[YTDWBH],[YTDWMC],[SYDWBH],[SYDWMC],[HTJCBMBH],[GCMC],[HTJCBMMC],[SYJCBMBH],[KHDWMC],[KHDWBH],[SYJCBMMC],[SGDWBH],[SYJCBMDZ],[SGDWMC],[KHDWZH],[JLDWBH],[JLDWMC],[FBHTBH],[JSDWBH],[JSDWMC],[SJDWBH],[SJDWMC],[ZT],[YSJCF],[SSZJZBH],[SSZJZMC],[SSJCF],[ZJDJH],[JM],[XGCS],[FKFSBH],[BGZS],[JCDD],[WTDGDH],[WTSLRRZH],[WTSLRRXM],getdate(),[WTSDYRZH],[WTSDYRXM],[JZRBH],[WTSDYSJ],[JZRXM],[JZRDH],[JZRZH],[WTSFHRZH],[WTLXRXM],[WTLXRDH],[WTBZ],[JYRZH],[JYRXM],[WTLXRDZ],[GCDZ],[JYSJ],[JCLX],[YPCLFS],[BGLQFS],[YCZT],[SFXF],[WTSMB],[WTSWJH],[JCFAPZRZH],[JCFAPZRXM],[XCXMQRRZH],[XCXMQRRXM],[JCYJ],[PDBZ],[SPYSC] from m_dj_by where recid='" + m_dj_byrecid + "'";
                List<string> sqls = new List<string>();
                sqls.Add(sql); // M_BY表
                sql = "Insert into M_" + syxmbh + " (recid) values('" + m_recid + "')";
                sqls.Add(sql);//m_syxm
                int zhcount = 1;
                foreach (IDictionary<string, string> row in dt)
                {
                    string recid = row["recid"].GetSafeString(); //m_dj_by recid
                    string s_dj_byrecid = row["s_byrecid"].GetSafeString(); //s_dj_by recid
                    string djdbh = row["djdbh"].GetSafeString();

                    int zs = row["zs"].GetSafeInt(1);
                    for (int i = 0; i < zs; i++)
                    {
                        string s_recid = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                        sql = "INSERT INTO S_BY([RECID],[BYZBRECID],[DJDBH],[YPEWM],[ZH],[GCBW],[YQSYRQ],[DZSSJCF],[SYZBDH],[SYZBMC],[JM],[DZYSJCF],[YPYSC],[YPZT])select '" + s_recid + "','" + m_recid + "','" + djdbh + "',[YPEWM],'" + zhcount + "',[GCBW],[YQSYRQ],[DZSSJCF],[SYZBDH],[SYZBMC],[JM],[DZYSJCF],[YPYSC],[YPZT]from s_dj_by where BYZBRECID='" + m_dj_byrecid + "' and recid='" + s_dj_byrecid + "'";
                        sqls.Add(sql); //s_by
                        sql = "INSERT INTO S_" + syxmbh + "([KYQD1],[KYQD2],[DDSJQD],[MIDAVG],[SJDJ],[JCXM],[CQS],[YHJ],[HZCASE],[SJCC],[SJCC1],[ZLPHB],[SNPZBH],[SXDMS],[YHTJ],[YHWD],[YHSD],[GGXH],[YPSL],[DWSNYL],[TLD],[SNCDJPH],[SZLJ],[HSXS],[TTJHSXS],[KYPJ],[DBSL],[ZZRQ],[LQ],[KYHZ1],[KYHZ2],[KYHZ3],[KYQD3],[SJDDJ],[SJDJBH],[RECID],[BYZBRECID])select [KYQD1],[KYQD2],[DDSJQD],[MIDAVG],[SJDJ],[JCXM],[CQS],[YHJ],[HZCASE],[SJCC],[SJCC1],[ZLPHB],[SNPZBH],[SXDMS],[YHTJ],[YHWD],[YHSD],[GGXH],[YPSL],[DWSNYL],[TLD],[SNCDJPH],[SZLJ],[HSXS],[TTJHSXS],[KYPJ],[DBSL],[ZZRQ],[LQ],[KYHZ1],[KYHZ2],[KYHZ3],[KYQD3],[SJDDJ],[SJDJBH],'" + s_recid + "','" + m_recid + "' from  s_dj_hnt  where BYZBRECID='" + m_dj_byrecid + "' and recid='" + s_dj_byrecid + "'";
                        sqls.Add(sql);//s_syxm
                        zhcount++;
                    }
                    sql = "update s_dj_by set zt=1 where recid='" + s_dj_byrecid + "'";
                    sqls.Add(sql);//更新提交状态

                }
                foreach (string str in sqls)
                {
                    CommonDao.ExecCommand(str, CommandType.Text);
                }

            }

            return code;
        }
        #endregion


        #region 获取检测机构科室
        public IList<IDictionary<string, string>> GetH_JCKS(string qybh)
        {
            string sql = "select ksbh,ksmc from h_jcks where ssdwbh='" + qybh + "' order by xssx ";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 获取检测试验人员
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="ksbh"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetH_JCSYRY(string qybh, string ksbh)
        {
            string sql = "select rybh,ryxm from i_m_nbry_jc where jcjgbh='" + qybh + "' and ssksbh='" + ksbh + "'";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 设置实验项目科室、试验人员、试验日期，插入任务相关表
        /// </summary>
        /// <param name="syks"></param>
        /// <param name="syrxm"></param>
        /// <param name="syrbh"></param>
        /// <param name="s_byrecids"></param>
        /// <param name="SYKSSJ"></param>
        /// <param name="SYJSSJ"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetS_BYsyks(string syks, string syrxm, string syrbh, string s_byrecids, DateTime SYKSSJ, DateTime SYJSSJ, out string msg)
        {
            bool code = true;
            msg = "";
            string inids = s_byrecids.Trim(new char[] { ',' }).FormatSQLInStr();
            string t_byrecids = s_byrecids.Trim(new char[] { ',' });
            string[] listrecid = t_byrecids.Split(',');
            string sql = "select top 1 byzbrecid from s_by where recid in (" + inids + ") and syrxm!='' and syrxm is not null";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count > 0 && listrecid.Length > 0)
            {
                List<string> sqls = new List<string>();
                sql = "update s_by set sybm='" + syks + "',syrxm='" + syrxm + "',syrzh='" + syrbh + "',sykssj='" + SYKSSJ.ToString() + "',syjssj='" + SYJSSJ.ToString() + "' where recid in (" + inids + ")";
                sqls.Add(sql); // 更新s_by设置实验项目科室、试验人员、试验日期
                //更新任务相关表
                string m_byrecid = dt[0]["byzbrecid"];
                string m_rw_recid = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                sql = "INSERT INTO [dbo].[M_BY_RW]([RECID],[YWZBID],[RWBZ],[ZT],[FPRZH],[FPRXM],[FPSJ],[JSRZH],[JSRXM],[JSSJ],[YXZZWCSJ],[YXZCWCSJ],[SFYX])";
                sql += "select '" + m_rw_recid + "','" + m_byrecid + "','','02','" + CurrentUser.UserName + "','" + CurrentUser.RealName + "',getdate(),'" + syrbh + "','" + syrxm + "',getdate(),'" + SYKSSJ.ToString() + "','" + SYJSSJ.ToString() + "',1";
                sqls.Add(sql);
                for (int i = 0; i < listrecid.Length; i++)
                {
                    string s_rw_recid = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                    sql = "INSERT INTO [dbo].[S_BY_RW_XQ]([RECID],[RWZBRECID],[YWCBID],[ZT],[SFYX])select '" + s_rw_recid + "','" + m_rw_recid + "','" + listrecid[i] + "','02',1 ";
                    sqls.Add(sql);
                }
                foreach (string str in sqls)
                {
                    CommonDao.ExecCommand(str, CommandType.Text);
                }
            }
            else
            {
                msg = "所选任务已分配，不能重复分配";
                code = false;
            }
            return code;
        }

        public IList<IDictionary<string, string>> GetS_BYsyks(string s_byrecids, out string msg)
        {

            msg = "";
            string inids = s_byrecids.Trim(new char[] { ',' }).FormatSQLInStr();
            string sql = "select syrxm,syrzh,sykssj,syjssj from s_by where recid in (" + inids + ")";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count == 0)
            {
                msg = "任务没有分配";
            }
            return dt;
        }



        #endregion


        #region 委托单管理

        //public bool SetWtdsy(string wtdrecids, out string msg)
        //{
        //    string inids = wtdrecids.Trim(new char[] { ',' }).FormatSQLInStr();
        //    //string sql=""
        //}
        #endregion


        #region 自动更新编号值+1
        [Transaction(ReadOnly = false)]
        public IList<JcjtJcjgBhms> GetJcjtBhms(string ssdwbh, string bhmsjz, string zd1, string bhms, string zdbh, string lx)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(lx);
            sb.Append(zd1);
            string strBHMS = bhms;
            //string strzdlx=sb.ToString();
            DateTime dt = DateTime.Now;
            string nowyear = dt.Year.ToString();
            string nowmonth = dt.Month.ToString();
            if (nowmonth.Length == 1)
            {
                nowmonth = "0" + nowmonth;
            }
            string nowtime = nowyear + nowmonth;
            sb.Append(nowtime);
            string strzdlxtime = sb.ToString();
            if (ssdwbh != null)
            {
                //string zdbh1 = zdbh.Substring(zdbh.Length - strzdlx.Length, strzdlx.Length);
                string zdbh1 = zdbh.Substring(strzdlxtime.Length);
                string str1 = "1";
                //zdbh = (int.Parse(zdbh1) + int.Parse(str1)).ToString().PadLeft(strzdlx.Length,'00000');
                string zdbh2 = (int.Parse(zdbh1) + int.Parse(str1)).ToString().PadLeft(zdbh1.Length, '0');
                string zdbh3 = string.Format("{0}{1}{2}{3}", lx, zd1, nowtime, zdbh2);
                string sql = string.Format("UPDATE pr_s_bhms_js set zdbh='{0}' where ssdwbh='{1}' and zd1='{2}' and lx='{3}'", zdbh3, ssdwbh, zd1, lx);
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            string sql_bhms = "select * from pr_s_bhms_js where ssdwbh='" + ssdwbh + "' and lx='" + lx + "' and zd1='" + zd1 + "' ";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            IList<IDictionary<string, string>> dt_bhms = CommonDao.GetDataTable(sql_bhms);


            IList<JcjtJcjgBhms> listob = new List<JcjtJcjgBhms>();
            JcjtJcjgBhms ob = new JcjtJcjgBhms();
            ob.zdbh = dt_bhms[0]["zdbh"].ToUpper();
            listob.Add(ob);

            return listob;
        }

        #endregion


        /// <summary>
        /// 设置人员单位
        /// </summary>
        /// <param name="rybhs"></param>
        /// <param name="czr"></param>
        /// <param name="czrxm"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetJCRydw(string recids, string czr, string czrxm, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            string curjcjgbh = CurrentUser.Qybh;
            string CPCODE = CurrentUser.CompanyCode;
            string sql = $"select * from i_m_nbry_jc where recid in ({recids.FormatSQLInStr()}) and sfzhm not in (select sfzhm from i_m_nbry_jc where jcjgbh='{curjcjgbh}')";
            var dt = CommonDao.GetDataTable(sql);
            if (dt.Count == 0)
                throw new Exception("已经录用，不能重复操作");
            List<string> sqls = new List<string>();
            string usercodes = string.Join(",", dt.Select(u => u["usercode"]).ToList());
            sqls.Add($"update I_M_NBRY_JC set usingnow=0 where usercode in ({usercodes.FormatSQLInStr()})");
            List<string> uprecids = new List<string>();
            List<string> insertrecids = new List<string>();
            for (int i = 0; i < dt.Count; i++)
            {
                string recid = dt[i]["recid"].GetSafeString();
                string jcjgbh = dt[i]["jcjgbh"].GetSafeString();
                if (string.IsNullOrEmpty(jcjgbh))
                    uprecids.Add(recid);
                else
                {
                    sqls.Add($"INSERT INTO [dbo].[I_M_NBRY_JC]([ZH],[CPCODE],[USERCODE],[JCJGBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[LRRZH],[LRRXM],[LRSJ],[SFYX],[SFSYR],[WxKey],[usingnow])" +
                        $"select  [ZH],'{CPCODE}',[USERCODE],'{curjcjgbh}',[RYBH],[RYXM],[XB],[SFZHM],[SJHM],'{CurrentUser.UserCode}','{CurrentUser.RealName}',getdate(),[SFYX],[SFSYR],[WxKey],1 from i_m_nbry_jc where recid='{recid}'");
                }
            }
            if (uprecids.Count > 0)
            {
                sqls.Add($"update I_M_NBRY_JC set CPCODE='{CPCODE}',jcjgbh='{curjcjgbh}',LRRZH='{CurrentUser.UserCode}',LRRXM='{CurrentUser.RealName}',LRSJ=getdate(),usingnow=1 where recid in ({string.Join(",", uprecids).FormatSQLInStr()})");
            }


            sqls.ForEach(x => CommonDao.ExecCommand(x));

            code = true;

            return code;
        }
        /// <summary>
        /// 辞退人员
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="czr"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CtJCRy(string usercode, string czr, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            string sql = $"select * from i_m_nbry_jc where jcjgbh='{CurrentUser.Qybh}' and usercode='{usercode}'";
            var rydt = CommonDao.GetDataTable(sql);
            if (rydt.Count == 0)
                return false;
            sql = $"update i_m_nbry_jc set jcjgbh='',cpcode='' where usercode = '{usercode}' and jcjgbh='{CurrentUser.Qybh}'";
            code = CommonDao.ExecSql(sql);
            //清除人员的角色

            string username = rydt[0]["zh"];
            string realname = rydt[0]["ryxm"];
            string cpcode = "";
            string ksbh = "";
            string sfsyr = "";
            string depcode = "";
            string postdm = "";
            IList<IDictionary<string, string>> dt = GetH_JCJG(CurrentUser.UserCode);
            if (dt.Count > 0)
            {
                cpcode = dt[0]["cpcode"].GetSafeString();
            }
            string xb = rydt[0]["xb"];
            string sjhm = rydt[0]["sjhm"];
            string rolecodelist = "";
            string procode = Configs.AppId; //"WZJDBG";
            string clearrole = "true";
            string sfzhm = rydt[0]["sfzhm"];

            ModifyUserInfoByUsercode(username, realname, usercode, xb, sfsyr, sjhm, cpcode, depcode, ksbh, "", procode, rolecodelist, clearrole, sfzhm, "");
            return code;
        }

        /// <summary>
        /// 录用业务员
        /// </summary>
        /// <param name="rybhs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetYwy(string rybhs, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string sql = $"update i_m_ry set qybh='{CurrentUser.Qybh}',isywy=1 where rybh in ({rybhs.FormatSQLInStr()}) and isnull(qybh,'')=''";
                CommonDao.ExecSql(sql);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 辞退业务员
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="czr"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CtYwy(string usercode, string czr, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            string sql = string.Format(@"update I_M_RY set isywy=0,qybh= '' where rybh = '{0}' ", usercode);
            code = CommonDao.ExecSql(sql);

            return code;
        }

        /// <summary>
        /// 获取检测机构或者检测人员的检测机构编号和名称
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> getJcryJCJG(string usercode, out string msg)
        {
            msg = "";
            string sql = "select jcjgbh,qymc as jcjgmc from View_I_M_NBRY_QYMC where usercode='" + usercode + "' and usingnow=1 ";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count == 0)
            {
                sql = "select qybh as jcjgbh,qymc as jcjgmc from i_m_qy where qybh =(select qybh from i_m_qyzh where yhzh='" + usercode + "')";
                dt = CommonDao.GetDataTable(sql);
                if (dt.Count == 0)
                    msg = "不在检测机构人员库和检测机构库中";
            }
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="password"></param>
        /// <param name="sfzh"></param>
        /// <param name="xb"></param>
        /// <param name="sfsyr"></param>
        /// <param name="sjhm"></param>
        /// <param name="cpcode"></param>
        /// <param name="depcode"></param>
        /// <param name="ksbh"></param>
        /// <param name="postdm"></param>
        /// <param name="rolecodelist"></param>
        /// <param name="syxmjson"></param>
        /// <param name="LoginType"> 1、监管用户系统校验</param>
        /// <returns></returns>
        public string AddUser(string jcjgbh, string username, string realname, string password, string sfzh, string xb, string sfsyr, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string rolecodelist, string syxmjson, string LoginType = "")
        {

            string err = "";
            string ret = "";
            string timestring = GetTimeStamp();
            string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
            string url = umsurl + "/Api/Service";
            string dates = "method=User&opt=AddUser&username=" + username + "&realname=" + realname + "&sfzh=" + sfzh + "&password=" + password + "&cpcode=" + cpcode + "&depcode=" + depcode + "&rolecodelist=" + rolecodelist + "&postdm=" + postdm + "&timestring=" + timestring + "&sign=" + sign;
            ret = SendDataByPost(url, dates);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            UmsRet umsret = jss.Deserialize<UmsRet>(ret);
            if (umsret.success)
            {
                Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                string rybh = Guid.NewGuid().ToString("N");
                if (sfsyr == "")
                    sfsyr = "0";
                string sql = "INSERT INTO I_M_NBRY_JC([ZH],[USERCODE],CPCODE,[JCJGBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX],LRSJ,SSKSBH,SFSYR,usingnow,LoginType) " +
                    "SELECT '" + username + "','" + param["usercode"] + "','" + cpcode + "','" + jcjgbh + "','" + rybh + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1,getdate(),'" + ksbh + "','" + sfsyr + "',1,'" + LoginType + "'";// from View_I_M_JCZH where yhzh='" + CurrentUser.UserCode + "'";  //'UR201907000002'";  //
                CommonDao.ExecSql(sql);

                sql = "INSERT INTO [I_M_QYZH]([QYBH],[YHZH],[SFQYZZH],[LRRZH],[LRRXM],[LRSJ],[ZHLX]) select '" + rybh + "','" + param["usercode"] + "',0,'" + CurrentUser.UserName + "','" + CurrentUser.RealName + "',getdate(),'R'";
                CommonDao.ExecSql(sql);
                if (!string.IsNullOrEmpty(syxmjson))
                    SaveJCRYSYXM_INIT(rybh, syxmjson);
            }
            return ret;
        }
        public string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sfsyr, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string procode, string rolecodelist, string clearrole, string sfzhm, string type)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=ModifyUserInfoByUsercode&username=" + username + "&realname=" + realname + "&usercode=" + usercode + "&procode=" + procode + "&cpcode=" + cpcode + "&depcode=" + depcode + "&postdm=" + postdm + "&rolecodelist=" + rolecodelist + "&clearrole=" + clearrole + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    // string sql = "update I_M_NBRY_JC set zh='" + username + "',zjzbh='" + cpcode + "',ryxm='" + realname + "',sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "' where rybh='" + usercode + "'";

                    // string sql = "INSERT INTO I_M_NBRY_JC([ZH],[USERCODE],CPCODE,[JCJGBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX],LRSJ,SSKSBH) SELECT '" + username + "','" + param["usercode"] + "','" + cpcode + "',qybh,'" + rybh + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1,getdate(),'" + ksbh + "' from View_I_M_JCZH where yhzh='" + CurrentUser.UserCode + "'";  //'UR201907000002'";  //

                    string sql = "";
                    if (type == "ywy")
                        sql = "update I_M_RY set sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "' where zh='" + username + "'";
                    else
                        sql = "update I_M_NBRY_JC set sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "', SSKSBH ='" + ksbh + "',sfsyr='" + sfsyr + "' where usercode='" + usercode + "' and jcjgbh='" + CurrentUser.Qybh + "'";
                    CommonDao.ExecSql(sql);
                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }



       /// <summary>
       /// 新增科室
       /// </summary>
       /// <returns></returns>
        public bool CreateJcks(string type, string ksbh, string qybh, string ksmc, string ksdz, string lxdh, string ksys, string kszcode, string kszxm, string jsfzrcode, string jsfzrxm, string zlfzrcode, string zlfzrxm)
        {
            string sql = "";
            if (type == "N")
            {
                ksbh = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                sql = "INSERT INTO [dbo].[H_JCKS]([SSDWBH],[KSMC],[KSBH],KSDZ,LXDH,KSYS,[XSSX],[KSZCODE],[KSZXM],[JSFZRCODE],[JSFZRXM],[ZLFZRCODE],[ZLFZRXM])" +
                    "select '" + qybh + "','" + ksmc + "','" + ksbh + "','" + ksdz + "','" + lxdh + "','" + ksys + "',1,'" + kszcode + "','" + kszxm + "','" + jsfzrcode + "','" + jsfzrxm + "','" + zlfzrcode + "','" + zlfzrxm + "'";
            }
            else
            {
                sql = "UPDATE H_JCKS SET ksdz='" + ksdz + "',lxdh='" + lxdh + "',ksys='" + ksys + "',kszcode='" + kszcode + "',kszxm='" + kszxm + "',jsfzrcode='" + jsfzrcode + "',jsfzrxm='" + jsfzrxm + "',zlfzrcode='" + zlfzrcode + "',zlfzrxm='" + zlfzrxm + "' where ksbh='" + ksbh + "'";
            }

            return CommonDao.ExecSql(sql);
        }
    }
}
