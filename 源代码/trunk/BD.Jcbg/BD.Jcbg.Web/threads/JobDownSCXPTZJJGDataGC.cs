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
    /// 省诚信平台下载企业工程项目信息
    /// </summary>
    public class JobDownSCXPTZJJGDataGC: ISchedulerJob
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
            SysLog4.WriteError("开始线程JobDownSCXPTZJJGDataGC");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string upsql = "";
                    IList<string> lsql = new List<string>();
                    int days = GlobalVariable.GetConfigValue("zjjg_gc_days").GetSafeInt();
                    int topnum = GlobalVariable.GetConfigValue("zjjg_gc_topnum").GetSafeInt();
                    string sql = "select top " + topnum.ToString() + " qybh, qymc, zzjgdm from I_M_QY " +
                                " where ( SCXPTGCLastUpdateTime is null or dateadd(day," + days.ToString() + ",SCXPTGCLastUpdateTime) < getdate())";

                    //SysLog4.WriteError(sql);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string qybh = row["qybh"].GetSafeString();
                            string qymc = row["qymc"].GetSafeString();
                            string zzjgdm = row["zzjgdm"].GetSafeString();
                            object prjlist = null;

                            if (qybh != "" && qymc != "")
                            {
                                if (ZJJGPublicData.GetCorpProjectList(qymc, out msg, out prjlist))
                                {
                                    if (prjlist != null)
                                    {
                                        ArrayList prjl = prjlist as ArrayList;
                                        List<Dictionary<string, object>> projectlist = new List<Dictionary<string, object>>();
                                        if (prjl != null && prjl.Count > 0)
                                        {
                                            foreach (var item in prjl)
                                            {
                                                projectlist.Add((Dictionary<string, object>)item);
                                            }
                                            // 遍历每个工程项目
                                            if (projectlist.Count > 0)
                                            {
                                                foreach (var item in projectlist)
                                                {
                                                    string PrjGuid = item["PrjGuid"].GetSafeString();
                                                    string prjname = item["PRJNAME"].GetSafeString();
                                                    //SysLog4.WriteError("PrjGuid: " + PrjGuid);
                                                    if (PrjGuid !="")
                                                    {
                                                        object projectinfo = null;
                                                        if (ZJJGPublicData.GetProjectInfo(PrjGuid, out msg, out projectinfo))
                                                        {
                                                            if (projectinfo !=null)
                                                            {
                                                                Dictionary<string, object> projectdata = (Dictionary<string, object>)projectinfo;
                                                                if (projectdata !=null && projectdata.Count > 0)
                                                                {
                                                                    #region 提取工程项目详情
                                                                    List<Dictionary<string, object>> TbProjectInfo = new List<Dictionary<string, object>>();
                                                                    List<Dictionary<string, object>> TbTenderInfo = new List<Dictionary<string, object>>();
                                                                    List<Dictionary<string, object>> TbContractRecordManage = new List<Dictionary<string, object>>();
                                                                    List<Dictionary<string, object>> TbBuilderLicenceManage = new List<Dictionary<string, object>>();
                                                                    List<Dictionary<string, object>> TbSubjectInfo = new List<Dictionary<string, object>>();
                                                                    List<Dictionary<string, object>> TbProjectFinishManage = new List<Dictionary<string, object>>();

                                                                    ArrayList list = projectdata["TbProjectInfo"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbProjectInfo.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    list = projectdata["TbTenderInfo"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbTenderInfo.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    list = projectdata["TbContractRecordManage"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbContractRecordManage.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    list = projectdata["TbBuilderLicenceManage"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbBuilderLicenceManage.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    list = projectdata["TbSubjectInfo"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbSubjectInfo.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    list = projectdata["TbProjectFinishManage"] as ArrayList;
                                                                    foreach (var itm in list)
                                                                    {
                                                                        TbProjectFinishManage.Add((Dictionary<string, object>)itm);
                                                                    }
                                                                    #endregion

                                                                    #region 生成sql语句
                                                                    string gcmc = "";
                                                                    string xmbh = "";
                                                                    string gcszdsf = "";
                                                                    string gcszdcs = "";
                                                                    string gcszdxq = "";
                                                                    string htbh = "";
                                                                    string sgxkzbh = "";
                                                                    string xmjl = "";
                                                                    string gclb = "";
                                                                    string jszb = "";
                                                                    string htj = "";
                                                                    string jsj = "";
                                                                    string sgcbfs = "";
                                                                    string sgzzfs = "";
                                                                    string kgsj = "";
                                                                    string jgsj = "";
                                                                    string jsdw = "";
                                                                    string jsdwlxr = "";
                                                                    string jsdwlxdh = "";
                                                                    string ysdw = "";
                                                                    string ysdwlxr = "";
                                                                    string ysdwlxdh = "";

                                                                    // 工程基本信息
                                                                    if (TbProjectInfo.Count > 0)
                                                                    {
                                                                        gcmc = TbProjectInfo[0]["PrjName"].GetSafeString();
                                                                        xmbh = TbProjectInfo[0]["PrjNum"].GetSafeString();
                                                                        gcszdsf= TbProjectInfo[0]["ProvinceName"].GetSafeString();
                                                                        gcszdcs = TbProjectInfo[0]["CityName"].GetSafeString();
                                                                        gcszdxq = TbProjectInfo[0]["CountyName"].GetSafeString();
                                                                        gclb = TbProjectInfo[0]["PrjTypeName"].GetSafeString();
                                                                        jsdw = TbProjectInfo[0]["BuildCorpName"].GetSafeString();
                                                                    }
                                                                    // 招投标信息
                                                                    if (TbTenderInfo.Count > 0)
                                                                    {
                                                                        xmjl = TbTenderInfo[0]["ConsCorpLeader"].GetSafeString();
                                                                    }
                                                                    // 合同备案信息
                                                                    if (TbContractRecordManage.Count > 0)
                                                                    {
                                                                        htbh = TbContractRecordManage[0]["RecordNum"].GetSafeString();
                                                                        htj = TbContractRecordManage[0]["ContractMoney"].GetSafeString();
                                                                        sgcbfs = TbContractRecordManage[0]["ContractTypeName"].GetSafeString();
                                                                        if (sgcbfs == "施工总包")
                                                                        {
                                                                            sgcbfs = "施工总承包";
                                                                        }

                                                                    }
                                                                    // 施工许可证信息
                                                                    if (TbBuilderLicenceManage.Count > 0)
                                                                    {
                                                                        sgxkzbh = TbBuilderLicenceManage[0]["BuilderLicenceNum"].GetSafeString();
                                                                        xmjl = TbBuilderLicenceManage[0]["ConsCorpLeader"].GetSafeString(); ;
                                                                        gclb = TbBuilderLicenceManage[0]["PrjTypeName"].GetSafeString();
                                                                    }
                                                                    // 竣工验收备案信息
                                                                    if (TbProjectFinishManage.Count > 0)
                                                                    {
                                                                        kgsj = TbProjectFinishManage[0]["BDate"].GetSafeString();
                                                                        jgsj = TbProjectFinishManage[0]["EDate"].GetSafeString();
                                                                        jsj = TbProjectFinishManage[0]["FactCost"].GetSafeString();
                                                                    }

                                                                    if (gcmc !="")
                                                                    {
                                                                        string procstr = string.Format("DownSCXPTGC('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                                                qybh, PrjGuid, gcmc, xmbh, gcszdsf, gcszdcs, gcszdxq, htbh, sgxkzbh, xmjl, gclb, jszb, htj, jsj, sgcbfs, sgzzfs, kgsj, jgsj, jsdw, jsdwlxr, jsdwlxdh, ysdw, ysdwlxr, ysdwlxdh
                                                                            );
                                                                        //SysLog4.WriteError(procstr);
                                                                        CommonService.ExecProc(procstr, out msg);
                                                                    }
                                                                    #endregion



                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            SysLog4.WriteError("下载企业工程信息出错：\r\n企业名称：" + qymc + "]\r\n工程名称：" + prjname + "\r\n项目GUID：" + PrjGuid);
                                                        }
                                                    }
                                                }

                                            }
                                            


                                        }

                                        #region 更新最后处理时间
                                        sql = string.Format("update i_m_qy set SCXPTGCLastUpdateTime=getdate() where qybh='{0}'", qybh);
                                        //SysLog4.WriteError(sql);
                                        CommonService.Execsql(sql);
                                        #endregion
                                    }
                                    else
                                    {
                                        SysLog4.WriteError("下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空");
                                    }
                                }
                                else
                                {
                                    SysLog4.WriteError("下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：" + msg);
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