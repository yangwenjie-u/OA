using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;
using System.IO;
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BD.Jcbg.Web.Func.SCXPT;
using System.Collections;
using BD.Jcbg.Web.Func;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 从省诚信平台上下载企业中相关人员信息
    /// 包括技术负责人、注册建造师、中级以上职称人员、现场管理人员、技术工人
    /// </summary>
    public class JobDownSCXPTZJJGDataRY:ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
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

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobDownSCXPTZJJGDataRY");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string upsql = "";
                    IList<string> lsql = new List<string>();
                    int days = GlobalVariable.GetConfigValue("zjjg_ry_days").GetSafeInt();
                    int topnum = GlobalVariable.GetConfigValue("zjjg_ry_topnum").GetSafeInt();
                    string sql = "select top " + topnum.ToString() + " qybh, qymc, zzjgdm from I_M_QY " +
                                " where ( SCXPTRYLastUpdateTime is null or dateadd(day," + days.ToString() + ",SCXPTRYLastUpdateTime) < getdate())";

                    //SysLog4.WriteError(sql);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        
                        foreach (var row in dt)
                        {
                            string qybh = row["qybh"].GetSafeString();
                            string qymc = row["qymc"].GetSafeString();
                            string zzjgdm = row["zzjgdm"].GetSafeString();
                            object personinfo = null;
                            
                            if (qybh != "" && qymc != "")
                            {
                                if(ZJJGPublicData.GetCorpPersonInfo(qymc,zzjgdm, out msg, out personinfo))
                                {
                                    if(personinfo != null)
                                    {
                                        ArrayList persondata = personinfo as ArrayList;
                                        if (persondata !=null && persondata.Count > 0)
                                        {
                                            List<Dictionary<string, object>> jsfzrlist = new List<Dictionary<string, object>>();
                                            List<Dictionary<string, object>> zcjzslist = new List<Dictionary<string, object>>();
                                            List<Dictionary<string, object>> zjyszcrylist = new List<Dictionary<string, object>>();
                                            List<Dictionary<string, object>> jsgrlist = new List<Dictionary<string, object>>();
                                            List<Dictionary<string, object>> xcglrylist = new List<Dictionary<string, object>>();
                                            // 获取所有的人员列表
                                            List<Dictionary<string, object>> allpersonlist = new List<Dictionary<string, object>>();
                                            foreach (var personrow in persondata)
                                            {
                                                allpersonlist.Add((Dictionary<string, object>)personrow);
                                            }
                                            // 下载所有人员详情
                                            foreach (var item in allpersonlist)
                                            {
                                                string rowguid = item["RowGuid"].GetSafeString();
                                                string userguid = item["UserGuid"].GetSafeString();
                                                object userrecord = null;
                                                if (ZJJGPublicData.GetUserRecordInfo(rowguid, userguid, out msg, out userrecord))
                                                {
                                                    Dictionary<string, object> persondetail = (Dictionary<string, object>)userrecord;
                                                    item.Add("PersonDetail", persondetail);
                                                    if (persondetail != null && persondetail.Count > 0)
                                                    {
                                                        ArrayList list = persondetail["EssentialInfo"] as ArrayList;
                                                        item.Add("Detail_EssentialInfo", list);
                                                        if (list != null && list.Count > 0)
                                                        {
                                                            Dictionary<string, object> ess = (Dictionary<string, object>)list[0];
                                                            if (ess !=null && ess.Count > 0)
                                                            {
                                                                item["ESS_NationName"] = ess["NationName"].GetSafeString();
                                                                item["ESS_EduLevelName"] = ess["EduLevelName"].GetSafeString();
                                                                item["ESS_DegreeName"] = ess["DegreeName"].GetSafeString();
                                                                item["ESS_IDCardTypeName"] = ess["IDCardTypeName"].GetSafeString();
                                                                item["ESS_IDCard"] = ess["IDCard"].GetSafeString();
                                                                item["ESS_PersonName"] = ess["PersonName"].GetSafeString();
                                                                item["ESS_SexName"] = ess["SexName"].GetSafeString();
                                                            }
                                                        }

                                                        list = persondetail["PractisingInfo"] as ArrayList;
                                                        item.Add("Detail_PractisingInfo", list);

                                                        list = persondetail["PerformanceInfo"] as ArrayList;
                                                        item.Add("Detail_PerformanceInfo", list);

                                                        list = persondetail["PostInfo"] as ArrayList;
                                                        item.Add("Detail_PostInfo", list);
                                                        list = persondetail["TBPersonTechTitleInfo"] as ArrayList;
                                                        item.Add("Detail_TBPersonTechTitleInfo", list);
                                                    }

                                                    // 生成注册人员、职称人员、现场人员、技术工人数据
                                                    if (item["iszhuce"].GetSafeBool())
                                                    {
                                                        zcjzslist.Add(item);
                                                    }

                                                    if (item["iszhicheng"].GetSafeBool())
                                                    {
                                                        zjyszcrylist.Add(item);
                                                    }

                                                    if (item["isxianchang"].GetSafeBool())
                                                    {
                                                        xcglrylist.Add(item);
                                                    }

                                                    if (item["isjishugongren"].GetSafeBool())
                                                    {
                                                        jsgrlist.Add(item);
                                                    }
                                                }
                                                else
                                                {
                                                    SysLog4.WriteError("获取人员详细信息失败：人员姓名[" + item["personname"].GetSafeString() + "]\r\n错误：" + msg);
                                                }
                                            }

                                            #region 处理注册建造师
                                            List<string> allzcjzssfzlist = new List<string>();
                                            foreach (var item in zcjzslist)
                                            {
                                                // 从人员的执业资格信息中获取注册证书
                                                var zyzglist = item["Detail_PractisingInfo"] as ArrayList;

                                                // 获取注册建造师证书列表
                                                List<Dictionary<string, object>> zcjzszslist = new List<Dictionary<string, object>>();
                                                foreach ( var zyzg in zyzglist)
                                                {
                                                    Dictionary<string, object> zyzginfo = (Dictionary<string, object>)zyzg;
                                                    List<string> jzszsmclist = new List<string>() {
                                                        "注册建造师（一级）",
                                                        "注册建造师（二级）"
                                                    };
                                                    if (zyzginfo != null && zyzginfo.Count > 0)
                                                    {
                                                        if (jzszsmclist.Contains(zyzginfo["SpecialtyTypeName"].GetSafeString()))
                                                        {
                                                            zcjzszslist.Add(zyzginfo);
                                                        }
                                                    }
                                                }

                                                if (zcjzszslist.Count > 0)
                                                {
                                                    // 存储本次下载的证书信息
                                                    List<string> zsbhlist = new List<string>();
                                                    string rysfzhm = "";
                                                    foreach (var zsjzszs in zcjzszslist)
                                                    {
                                                        string ryxm = zsjzszs["PersonName"].GetSafeString();
                                                        string sfzhm = zsjzszs["IDCard"].GetSafeString();
                                                        string zy = zsjzszs["RegTradeTypeName"].GetSafeString();
                                                        string jb = zsjzszs["SpecialtyTypeName"].GetSafeString();
                                                        if (jb == "注册建造师（一级）")
                                                        {
                                                            jb = "一级注册建造师";
                                                        }
                                                        else if(jb == "注册建造师（二级）")
                                                        {
                                                            jb = "二级注册建造师";
                                                        }

                                                        string zczsbh = zsjzszs["CertNum"].GetSafeString();
                                                        
                                                        string fzrq = zsjzszs["AwardDate"].GetSafeString();
                                                        if (fzrq !="")
                                                        {
                                                            DateTime tmpdt = new DateTime();
                                                            if(DateTime.TryParse(fzrq, out tmpdt))
                                                            {
                                                                fzrq = tmpdt.ToString("yyyy-MM-dd");
                                                            }
                                                        }
                                                        string zsyxq = zsjzszs["EffectDate"].GetSafeString();
                                                        if (zsyxq != "")
                                                        {
                                                            DateTime tmpdt = new DateTime();
                                                            if (DateTime.TryParse(zsyxq, out tmpdt))
                                                            {
                                                                zsyxq = tmpdt.ToString("yyyy-MM-dd");
                                                            }
                                                        }

                                                        zsbhlist.Add(zczsbh);
                                                        rysfzhm = sfzhm;
                                                        allzcjzssfzlist.Add(sfzhm);
                                                        string procstr = string.Format("DownSCXPTZCJZS('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                                qybh, ryxm,sfzhm,zy,jb,zczsbh,fzrq,zsyxq
                                                            );
                                                        CommonService.ExecProc(procstr, out msg);

                                                    }

                                                    //除了本次下载的证书信息，其他的都删了
                                                    if (zsbhlist.Count > 0 && rysfzhm!="")
                                                    {
                                                        sql = string.Format("delete from i_s_qy_zcjzs where qybh='{0}' and sfzhm='{1}' and zczsbh not in({2})", 
                                                                qybh, rysfzhm, string.Join(",",zsbhlist).FormatSQLInStr()
                                                            );
                                                        CommonService.Execsql(sql);
                                                    }
                                                    
                                                }

                                            }
                                            // 除了本次下载的注册建造师，其他的都删了
                                            if (allzcjzssfzlist.Count > 0)
                                            {
                                                sql = string.Format(" delete from i_s_qy_zcjzs where qybh='{0}' and sfzhm not in({1})",
                                                        qybh, string.Join(",", allzcjzssfzlist.Distinct().ToList()).FormatSQLInStr()
                                                    );
                                                CommonService.Execsql(sql);
                                            }

                                            #endregion

                                            #region 处理中级以上职称人员
                                            List<string> allzjyszcrysfzlist = new List<string>();
                                            foreach (var item in zjyszcrylist)
                                            {
                                                var zclist = item["Detail_TBPersonTechTitleInfo"] as ArrayList;
                                                if (zclist !=null && zclist.Count > 0)
                                                {
                                                    string rysfzhm = "";
                                                    List<Dictionary<string, string>> deletingzc = new List<Dictionary<string, string>>();
                                                    foreach (var zcdata in zclist)
                                                    {
                                                        Dictionary<string, object> zcinfo = (Dictionary<string, object>)zcdata;
                                                        if (zcinfo != null && zcinfo.Count > 0)
                                                        {
                                                            string ryxm = item["ESS_PersonName"].GetSafeString();
                                                            string xl = item["ESS_EduLevelName"].GetSafeString();
                                                            string zc = zcinfo["TechTitleName"].GetSafeString();
                                                            if (zc.StartsWith("高级"))
                                                            {
                                                                zc = "高级";
                                                            }
                                                            else
                                                            {
                                                                zc = "中级";
                                                            }
                                                            string sfzhm = item["ESS_IDCard"].GetSafeString();
                                                            string zczy = zcinfo["MajorName"].GetSafeString();
                                                            string xlzy = "";
                                                            deletingzc.Add(new Dictionary<string, string>() {
                                                                { "zc", zc},
                                                                { "zczy", zczy}
                                                            });
                                                            rysfzhm = sfzhm;
                                                            allzjyszcrysfzlist.Add(sfzhm);
                                                            string procstr = string.Format("DownSCXPTZJYSZCRY('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                    qybh, ryxm, xl, zc, sfzhm, zczy, xlzy
                                                                );
                                                            CommonService.ExecProc(procstr, out msg);
                                                        }
                                                    }

                                                    // 除了本次下载的职称，其他的全部删掉
                                                    if (deletingzc.Count > 0 && rysfzhm!="")
                                                    {
                                                        string notstr = "";
                                                        foreach (var dzc in deletingzc)
                                                        {
                                                            if (notstr !="")
                                                            {
                                                                notstr += " or ";
                                                            }
                                                            notstr += string.Format("(zc='{0}' and zczy='{1}')", dzc["zc"], dzc["zczy"]);
                                                        }
                                                        sql = string.Format("delete from i_s_qy_zjyszcry where qybh='{0}' and sfzhm='{1}' and not ({2})",
                                                                qybh, rysfzhm, notstr
                                                            );
                                                        //SysLog4.WriteError(sql);
                                                        CommonService.Execsql(sql);
                                                    }
                                                }
                                            }
                                            // 除了本次下载的职称人员，其他的都删了
                                            if (allzjyszcrysfzlist.Count > 0)
                                            {
                                                sql = string.Format(" delete from i_s_qy_zjyszcry where qybh='{0}' and sfzhm not in({1})",
                                                        qybh, string.Join(",", allzjyszcrysfzlist.Distinct().ToList()).FormatSQLInStr()
                                                    );
                                                CommonService.Execsql(sql);
                                            }
                                            #endregion

                                            #region 处理技术工人
                                            // 保存本次下载的所有技术工人的身份证
                                            // 本次下载之后，删除系统中所有身份证不在里面的人
                                            List<string> alljsgrsfzlist = new List<string>();
                                            foreach (var item in jsgrlist)
                                            {
                                                var gwlist = item["Detail_PostInfo"] as ArrayList;
                                                if (gwlist!=null && gwlist.Count > 0)
                                                {
                                                    string rysfzhm = "";
                                                    List<string> zsbhlist = new List<string>();
                                                    foreach (var gwdata in gwlist)
                                                    {
                                                        Dictionary<string, object> gw = (Dictionary<string, object>)gwdata;
                                                        if (gw!=null && gw.Count > 0)
                                                        {
                                                            string gwname = gw["PostClassName"].GetSafeString();
                                                            // 只处理技术工人
                                                            List<string> allgwlist = new List<string>() {
                                                                "现场作业人员",
                                                                "特种作业人员"
                                                            };
                                                            if (allgwlist.Contains(gwname) )
                                                            {
                                                                string ryxm = item["ESS_PersonName"].GetSafeString();
                                                                string sfzhm = item["ESS_IDCard"].GetSafeString();
                                                                string jndj = gw["CertLevelName"].GetSafeString();
                                                                string zygz = gw["PostTypeName"].GetSafeString();
                                                                string zsbh = gw["CertNum"].GetSafeString();
                                                                string fzdw = gw["OrganName"].GetSafeString();
                                                                string sfzy = "是";
                                                                string fzrq = gw["OrganDate"].GetSafeString();
                                                                string zsyxq = gw["EndDate"].GetSafeString();

                                                                rysfzhm = sfzhm;
                                                                zsbhlist.Add(zsbh);
                                                                alljsgrsfzlist.Add(sfzhm);
                                                                string procstr = string.Format("DownSCXPTJSGR('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                                                                    qybh, ryxm, sfzhm, jndj, zygz, zsbh, fzdw, sfzy, fzrq, zsyxq
                                                                                );
                                                                CommonService.ExecProc(procstr, out msg);
                                                            }
                                                        }

                                                    }

                                                    // 除了本次下载的技术工人岗位，其他的都删了
                                                    if (zsbhlist.Count > 0 && rysfzhm != "")
                                                    {
                                                        sql = string.Format("delete from i_s_qy_jsgr where qybh='{0}' and sfzhm='{1}' and zsbh not in({2})",
                                                                qybh, rysfzhm, string.Join(",", zsbhlist).FormatSQLInStr()
                                                            );
                                                        CommonService.Execsql(sql);
                                                    }
                                                    

                                                }
                                               
                                            }
                                            // 除了本次下载的技术工人，其他的都删了
                                            if (alljsgrsfzlist.Count > 0)
                                            {
                                                sql = string.Format(" delete from i_s_qy_jsgr where qybh='{0}' and sfzhm not in({1})", 
                                                        qybh, string.Join(",", alljsgrsfzlist.Distinct().ToList()).FormatSQLInStr()
                                                    );
                                                CommonService.Execsql(sql);
                                            }
                                            #endregion

                                            #region 更新最后处理时间
                                            sql = string.Format("update i_m_qy set SCXPTRYLastUpdateTime=getdate() where qybh='{0}'", qybh);
                                            CommonService.Execsql(sql);
                                            #endregion


                                        }
                                    }
                                    else
                                    {
                                        SysLog4.WriteError("下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空");
                                    }
                                }
                                else
                                {
                                    SysLog4.WriteError("下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：" + msg);
                                }


                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }

        }
    }
}