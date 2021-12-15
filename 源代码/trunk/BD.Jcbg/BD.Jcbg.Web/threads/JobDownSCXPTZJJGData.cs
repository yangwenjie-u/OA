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
    /// 从省诚信平台上下载企业信息
    /// </summary>
    public class JobDownSCXPTZJJGData : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobDownSCXPTZJJGData");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string upsql = "";
                    IList<string> lsql = new List<string>();
                    int days = GlobalVariable.GetConfigValue("zjjg_days").GetSafeInt();
                    int topnum = GlobalVariable.GetConfigValue("zjjg_topnum").GetSafeInt();
                    string sql = "select top " + topnum.ToString() +" qybh, qymc, zzjgdm from I_M_QY " +
                                " where ( SCXPTLastUpdateTime is null or dateadd(day," + days.ToString() + ",SCXPTLastUpdateTime) < getdate())";

                    //SysLog4.WriteError(sql);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var row in dt)
                        {
                            lsql.Clear();
                            string qybh = row["qybh"].GetSafeString();
                            string qymc = row["qymc"].GetSafeString();
                            string zzjgdm = row["zzjgdm"].GetSafeString();
                            if (qybh !="" && qymc !="") 
                            {
                                DownQyInfo(qybh, qymc, zzjgdm);
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

        private void DownQyInfo(string qybh, string qymc, string zzjgdm)
        {
            IList<string> lsql = new List<string>();
            string upsql = "";
            string sql = "";
            bool ret = true;
            string msg = "";

            #region 下载企业基本信息
            object basicinfo = null;
            // 获取企业基本信息
            if (ZJJGPublicData.GetCorpBasicInfo(qymc, zzjgdm, out msg, out basicinfo))
            {
                if (basicinfo != null)
                {
                    Dictionary<string, object> qydata = (Dictionary<string, object>)basicinfo;
                    if (qydata != null)
                    {
                        if (qydata.ContainsKey("tbCorpBasicInfo"))
                        {
                            var qyinfolist = qydata["tbCorpBasicInfo"] as ArrayList;
                            if (qyinfolist != null && qyinfolist.Count > 0)
                            {
                                Dictionary<string, object> qyinfo = (Dictionary<string, object>)qyinfolist[0];

                                if (qyinfo != null && qyinfo.Count > 0)
                                {
                                    #region 更新企业基本信息

                                    upsql = "update i_m_qy set " +
                                            " XSJLSJ='{0}' ," +
                                            " YYZZBH='{1}' ," +
                                            " ZCZJ='{2}' ," +
                                            " JJXZ='{3}' ," +
                                            " ZCD1='{4}' ," +
                                            " ZCD2='{5}' ," +
                                            " ZCD3='{6}' ," +
                                            " XSYB='{7}' ," +
                                            " LXDH='{8}' ," +
                                            " DWCZ='{9}' ," +
                                            " LXYX='{10}' ," +
                                            " ZCDYB='{11}' ," +
                                            " BGDZ='{12}' ," +
                                            " DWWZ='{13}' ," +
                                            " QYFR='{14}' ," +
                                            " QYFRSFZHM='{15}' ," +
                                            " XSFRZW='{16}' ," +
                                            " XSFRZC='{17}' ," +
                                            " ZCD4='{18}', " +
                                            " XSZCD1='{21}', " +
                                            " XSZCD2='{22}', " +
                                            " XSZCD3='{23}', " +
                                            " SCXPTLastUpdateTime=getdate() " +
                                            " where qybh='{19}' and qymc='{20}' ";

                                    string zcd3 = qyinfo["CountyName"].GetSafeString();
                                    if (zcd3 == "市辖区")
                                    {
                                        zcd3 = "越城区";
                                    }

                                    upsql = string.Format(upsql,
                                            qyinfo["CorpBirthDate"].GetSafeString(),
                                            qyinfo["LicenseNum"].GetSafeString(),
                                            qyinfo["RegPrin"].GetSafeString(),
                                            qyinfo["EconTypeName"].GetSafeString(),
                                            qyinfo["ProvinceName"].GetSafeString(),
                                            qyinfo["CityName"].GetSafeString(),
                                            zcd3,
                                            qyinfo["BusPostalCode"].GetSafeString(),
                                            qyinfo["OfficePhone"].GetSafeString(),
                                            qyinfo["Fax"].GetSafeString(),
                                            qyinfo["EMail"].GetSafeString(),
                                            qyinfo["PostalCode"].GetSafeString(),
                                            qyinfo["BusAddress"].GetSafeString(),
                                            qyinfo["Url"].GetSafeString(),
                                            qyinfo["LegalManName"].GetSafeString(),
                                            qyinfo["LegalManIDCard"].GetSafeString(),
                                            qyinfo["LegalManDutyName"].GetSafeString(),
                                            qyinfo["LegalManTitleName"].GetSafeString(),
                                            qyinfo["Address"].GetSafeString(),
                                            qybh, qymc,
                                            qyinfo["ProvinceName"].GetSafeString(),
                                            qyinfo["CityName"].GetSafeString(),
                                            zcd3
                                            );

                                    lsql.Add(upsql);

                                    //SysLog4.WriteError(upsql);

                                    #endregion

                                    #region 更新证书信息
                                    var zzzsinfolist = qydata["TBCorpCertInfo"] as ArrayList;
                                    var zzfwinfolist = qydata["TBCorpCertDetailInfo"] as ArrayList;
                                    if (zzzsinfolist != null && zzzsinfolist.Count > 0)
                                    {
                                        #region 提取所有资质范围
                                        List<Dictionary<string, object>> zzfwlist = new List<Dictionary<string, object>>();
                                        foreach (var zzfw in zzfwinfolist)
                                        {
                                            Dictionary<string, object> zzfwinfo = (Dictionary<string, object>)zzfw;
                                            if (zzfwinfo != null && zzfwinfo.Count > 0)
                                            {
                                                zzfwlist.Add(new Dictionary<string, object>() {
                                                                        {"ZZZSBH", zzfwinfo["CertID"].GetSafeString()},
                                                                        {"ZZFW", zzfwinfo["Mark"].GetSafeString()},
                                                                        {"JSFZR", ""}
                                                                    });
                                            }
                                        }

                                        //SysLog4.WriteError("获取到的资质范围总数：" + zzfwlist.Count.ToString());
                                        #endregion

                                        #region 提取所有资质证书
                                        List<Dictionary<string, object>> zslist = new List<Dictionary<string, object>>();
                                        List<Dictionary<string, object>> aqsczslist = new List<Dictionary<string, object>>();

                                        foreach (var zzzs in zzzsinfolist)
                                        {
                                            Dictionary<string, object> zzzsinfo = (Dictionary<string, object>)zzzs;
                                            if (zzzsinfo != null && zzzsinfo.Count > 0)
                                            {
                                                if (zzzsinfo["CertTypeName"].GetSafeString().StartsWith("建筑业"))
                                                {
                                                    // 获取当前资质拥有的资质范围
                                                    List<Dictionary<string, object>> ownzzfw = new List<Dictionary<string, object>>();
                                                    foreach (var fw in zzfwlist)
                                                    {
                                                        //SysLog4.WriteError("zzzsbh: " + fw["ZZZSBH"].GetSafeString());
                                                        //SysLog4.WriteError("CertID:" + zzzsinfo["CertID"].GetSafeString());
                                                        if (fw["ZZZSBH"].GetSafeString() == zzzsinfo["CertID"].GetSafeString())
                                                        {
                                                            ownzzfw.Add(fw);
                                                        }
                                                    }
                                                    zslist.Add(new Dictionary<string, object>() {
                                                                            {"ZZZSBH", zzzsinfo["CertID"].GetSafeString() },
                                                                            {"FZRQ", zzzsinfo["OrganDate"].GetSafeString() },
                                                                            {"ZSYXQ", zzzsinfo["EndDate"].GetSafeString() },
                                                                            {"FZJG", zzzsinfo["OrganName"].GetSafeString() },
                                                                            { "ZZFWLIST", ownzzfw}
                                                                        });

                                                }
                                                else if (zzzsinfo["CertTypeName"].GetSafeString() == "安全生产许可")
                                                {
                                                    aqsczslist.Add(new Dictionary<string, object>() {
                                                                            {"ZZZSBH", zzzsinfo["CertID"].GetSafeString() },
                                                                            {"FZRQ", zzzsinfo["OrganDate"].GetSafeString() },
                                                                            {"ZSYXQ", zzzsinfo["EndDate"].GetSafeString() },
                                                                            {"FZJG", zzzsinfo["OrganName"].GetSafeString() }
                                                                        });
                                                }
                                            }

                                        }
                                        #endregion

                                        #region 生成SQL
                                        if (zslist.Count > 0)
                                        {
                                            foreach (var zs in zslist)
                                            {
                                                string zzzsbh = zs["ZZZSBH"].GetSafeString();
                                                string fzrq = zs["FZRQ"].GetSafeString();
                                                string zsyxq = zs["ZSYXQ"].GetSafeString();
                                                string fzjg = zs["FZJG"].GetSafeString();
                                                List<Dictionary<string, object>> fwlist = zs["ZZFWLIST"] as List<Dictionary<string, object>>;
                                                // 根据资质证书编号删除资质范围
                                                upsql = "delete from jdbg_qyzz_zzfw " +
                                                        " where zzid in (select id from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}')";
                                                upsql = string.Format(upsql, qybh, zzzsbh);
                                                lsql.Add(upsql);

                                                // 根据资质证书编号删除资质证书
                                                upsql = string.Format(" delete from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}'", qybh, zzzsbh);
                                                lsql.Add(upsql);

                                                // 插入资质证书信息
                                                string id = Guid.NewGuid().ToString("N");

                                                upsql = "insert into jdbg_qyzz (id,qybh,zzzsbh,fzrq,zsyxq,fzjg) " +
                                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                                                upsql = string.Format(upsql, id, qybh, zzzsbh, fzrq, zsyxq, fzjg);
                                                lsql.Add(upsql);

                                                SysLog4.WriteError("id： " + id + "资质范围总数：" + fwlist.Count);
                                                if (fwlist != null && fwlist.Count > 0)
                                                {
                                                    foreach (var fw in fwlist)
                                                    {
                                                        string zzfwmc = fw["ZZFW"].GetSafeString();
                                                        string jsfzr = fw["JSFZR"].GetSafeString();
                                                        // 资质范围等级名称替换，保持与保准一致
                                                        zzfwmc = zzfwmc.Replace("壹级", "一级").Replace("贰级", "二级").Replace("叁级", "三级");
                                                        if (zzfwmc != "")
                                                        {
                                                            upsql = "insert into jdbg_qyzz_zzfw (zzid, zzfw,jsfzr) " +
                                                                    "values ('{0}','{1}','{2}')";
                                                            upsql = string.Format(upsql, id, zzfwmc, jsfzr);
                                                            lsql.Add(upsql);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (aqsczslist.Count > 0)
                                        {
                                            string aqsczhbh = aqsczslist[0]["ZZZSBH"].GetSafeString();
                                            string aqsczhyxq = aqsczslist[0]["ZSYXQ"].GetSafeString();
                                            upsql = "update I_M_QY set AQSCXKZBH='{0}', AQSCXKZBHDQSJ='{1}' " +
                                                                    " where qybh='{2}'";
                                            upsql = string.Format(upsql, aqsczhbh, aqsczhyxq, qybh);
                                            lsql.Add(upsql);

                                        }
                                        #endregion

                                    }
                                    #endregion

                                    #region 更新企业附件
                                    string QyToRowguid = qyinfo["ToRowGuid"].GetSafeString();
                                    if (QyToRowguid !="")
                                    {
                                        object corpfiledata = null; 
                                        if(ZJJGPublicData.GetCorpFileList(QyToRowguid, out msg, out corpfiledata))
                                        {
                                            if (corpfiledata != null )
                                            {
                                                ArrayList corpfilelist = corpfiledata as ArrayList;
                                                if (corpfilelist !=null && corpfilelist.Count > 0)
                                                {
                                                    List<string> qyfjlsql = new List<string>();
                                                    foreach (var corpfile in corpfilelist)
                                                    {
                                                        Dictionary<string, object> file = (Dictionary<string, object>)corpfile;
                                                        if (file !=null && file.Count >0)
                                                        {
                                                            string FileGuid = file["FileGuid"].GetSafeString().Replace("'", "");
                                                            string FileName = file["FileName"].GetSafeString().Replace("'", "");
                                                            string FileGroupName = file["FileGroupName"].GetSafeString().Replace("'", "");
                                                            string FileBase64String = file["FileBase64String"].GetSafeString();
                                                            byte[] filecontent = FileBase64String.DecodeBase64Array();
                                                            if (FileName == "")
                                                            {
                                                                FileName = Guid.NewGuid().ToString("N")+".jpg";
                                                            }
                                                            if (FileName !="" && filecontent!=null && filecontent.Length > 0)
                                                            {
                                                                string fileid = "";
                                                                string ext = "";
                                                                if (FileName.IndexOf(".") > 0)
                                                                {
                                                                    ext = FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.'));
                                                                }
                                                                if (SaveFile(FileName, ext, filecontent,out fileid, out msg))
                                                                {
                                                                    upsql = "insert into scxpt_i_s_qy_fj (fileguid,filename,filegroupname,fileid,qybh,qymc) " +
                                                                            " values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                                                                    upsql = string.Format(upsql, FileGuid, FileName, FileGroupName, fileid, qybh, qymc);
                                                                    qyfjlsql.Add(upsql);
                                                                }
                                                                else
                                                                {
                                                                    SysLog4.WriteError(msg);
                                                                    SysLog4.WriteError("保存企业附件失败，附件名称：" + FileName);
                                                                }

                                                            }

                                                        }
                                                    }
                                                    if (qyfjlsql.Count > 0)
                                                    {
                                                        
                                                        // 删除已有的企业附件
                                                        upsql = string.Format("delete from scxpt_i_s_qy_fj where qybh='{0}'", qybh);
                                                        qyfjlsql.Insert(0, upsql);
                                                        // 删除已经保存的企业附件
                                                        upsql = string.Format("delete from datafile where fileid in (select fileid from scxpt_i_s_qy_fj where qybh='{0}')", qybh);
                                                        qyfjlsql.Insert(0, upsql);
                                                        // 更新营业执照正本
                                                        upsql = "update i_m_qy set yyzzfj=stuff((select '|'+fileid+','+filename from scxpt_i_s_qy_fj where qybh='{0}' and filegroupname='{1}' for xml path('')),1,1,'') where qybh='{2}'";
                                                        upsql = string.Format(upsql, qybh, "企业法人营业执照正本", qybh);
                                                        qyfjlsql.Add(upsql);
                                                        // 更新组织机构代码证
                                                        upsql = "update i_m_qy set zzjgzs=stuff((select '|'+fileid+','+filename from scxpt_i_s_qy_fj where qybh='{0}' and filegroupname='{1}' for xml path('')),1,1,'') where qybh='{2}'";
                                                        upsql = string.Format(upsql, qybh, "组织机构代码证", qybh);
                                                        qyfjlsql.Add(upsql);

                                                        foreach (var s in qyfjlsql)
                                                        {
                                                            lsql.Add(s);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SysLog4.WriteError("下载企业附件出错\r\n企业名称：" + qymc);
                                        }
                                    }
                                    #endregion

                                    #region 执行更新
                                    if (lsql.Count > 0)
                                    {
                                        CommonService.ExecTrans(lsql);
                                    }
                                    #endregion


                                }
                            }


                        }

                    }

                }

            }
            else
            {
                SysLog4.WriteError("下载企业信息出错：企业名称[" + qymc + "]");
            }
            #endregion

            #region 下载企业人员信息
            lsql.Clear();
            object personinfo = null;
            if (ZJJGPublicData.GetCorpPersonInfo(qymc, zzjgdm, out msg, out personinfo))
            {
                if (personinfo != null)
                {
                    ArrayList persondata = personinfo as ArrayList;
                    if (persondata != null && persondata.Count > 0)
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
                                        if (ess != null && ess.Count > 0)
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

                                #region 处理人员附件
                                string rysfzhm = item["ESS_IDCard"].GetSafeString();
                                string ryxm = item["ESS_PersonName"].GetSafeString();
                                if (rysfzhm !="" && rowguid!="" && userguid!="")
                                {
                                    object personfiledata = null;
                                    if (ZJJGPublicData.GetPersonFileList(rowguid, userguid,rysfzhm,out msg, out personfiledata))
                                    {
                                        ArrayList personfilelist = personfiledata as ArrayList;
                                        if (personfilelist !=null && personfilelist.Count > 0)
                                        {
                                            List<string> ryfjlsql = new List<string>();
                                            foreach (var personfile in personfilelist)
                                            {
                                                Dictionary<string, object> file = (Dictionary<string, object>)personfile;
                                                if (file != null && file.Count > 0)
                                                {
                                                    string FileGuid = file["FileGuid"].GetSafeString().Replace("'", "");
                                                    string FileName = file["FileName"].GetSafeString().Replace("'", "");
                                                    string FileGroupName = file["FileGroupName"].GetSafeString().Replace("'", "");
                                                    string FileBase64String = file["FileBase64String"].GetSafeString();
                                                    byte[] filecontent = FileBase64String.DecodeBase64Array();
                                                    if (FileName == "")
                                                    {
                                                        FileName = Guid.NewGuid().ToString("N") + ".jpg";
                                                    }
                                                    if (FileName != "" && filecontent != null && filecontent.Length > 0)
                                                    {
                                                        string fileid = "";
                                                        string ext = "";
                                                        if (FileName.IndexOf(".") > 0)
                                                        {
                                                            ext = FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.'));
                                                        }
                                                        if (SaveFile(FileName, ext, filecontent, out fileid, out msg))
                                                        {
                                                            upsql = "insert into scxpt_i_s_qy_ry_fj (fileguid,filename,filegroupname,fileid,qybh,sfzhm,ryxm) " +
                                                                    " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                                                            upsql = string.Format(upsql, FileGuid, FileName, FileGroupName, fileid, qybh, rysfzhm, ryxm);
                                                            ryfjlsql.Add(upsql);
                                                        }
                                                        else
                                                        {
                                                            SysLog4.WriteError(msg);
                                                            SysLog4.WriteError("保存企业附件失败，附件名称：" + FileName);
                                                        }

                                                    }

                                                }
                                            }
                                            if (ryfjlsql.Count > 0)
                                            {
                                                // 删除已有的企业附件
                                                upsql = string.Format("delete from scxpt_i_s_qy_ry_fj where qybh='{0}' and sfzhm='{1}'", qybh, rysfzhm);
                                                ryfjlsql.Insert(0, upsql);
                                                upsql = string.Format("delete from datafile where fileid in (select fileid from scxpt_i_s_qy_ry_fj where qybh='{0}' and sfzhm='{1}')", qybh, rysfzhm);
                                                ryfjlsql.Insert(0, upsql);
                                                CommonService.ExecTrans(ryfjlsql);
                                                


                                            }
                                        }
                                    }
                                    else
                                    {
                                        SysLog4.WriteError("下载人员附件出错\r\n人员姓名：" + ryxm + "\r\n身份证号码：" + rysfzhm);
                                    }
                                }
                                #endregion


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
                            foreach (var zyzg in zyzglist)
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
                                List<Dictionary<string, string>> deletingzcjzs = new List<Dictionary<string, string>>();
                                string rysfzhm = "";
                                foreach (var zsjzszs in zcjzszslist)
                                {
                                    //string ryxm = zsjzszs["PersonName"].GetSafeString();
                                    string ryxm = item["ESS_PersonName"].GetSafeString();
                                    string sfzhm = zsjzszs["IDCard"].GetSafeString();
                                    string zy = zsjzszs["RegTradeTypeName"].GetSafeString();
                                    string jb = zsjzszs["SpecialtyTypeName"].GetSafeString();
                                    if (jb == "注册建造师（一级）")
                                    {
                                        jb = "一级注册建造师";
                                    }
                                    else if (jb == "注册建造师（二级）")
                                    {
                                        jb = "二级注册建造师";
                                    }

                                    string zczsbh = zsjzszs["CertNum"].GetSafeString();

                                    string fzrq = zsjzszs["AwardDate"].GetSafeString();
                                    if (fzrq != "")
                                    {
                                        DateTime tmpdt = new DateTime();
                                        if (DateTime.TryParse(fzrq, out tmpdt))
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
                                    deletingzcjzs.Add(new Dictionary<string, string>() {
                                        {"zczsbh",zczsbh },
                                        { "zy", zy}
                                    });
                                    
                                    rysfzhm = sfzhm;
                                    allzcjzssfzlist.Add(sfzhm);
                                    string procstr = string.Format("DownSCXPTZCJZS('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                            qybh, ryxm, sfzhm, zy, jb, zczsbh, fzrq, zsyxq
                                        );
                                    CommonService.ExecProc(procstr, out msg);

                                }

                                //除了本次下载的证书信息，其他的都删了
                                if (deletingzcjzs.Count > 0 && rysfzhm != "")
                                {
                                    string notstr = "";
                                    foreach (var dzc in deletingzcjzs)
                                    {
                                        if (notstr != "")
                                        {
                                            notstr += " or ";
                                        }
                                        notstr += string.Format("(zy='{0}' and zczsbh='{1}')", dzc["zy"], dzc["zczsbh"]);
                                    }
                                    sql = string.Format("delete from i_s_qy_zcjzs where qybh='{0}' and sfzhm='{1}' and  not ({2})",
                                            qybh, rysfzhm, notstr
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
                            if (zclist != null && zclist.Count > 0)
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
                                if (deletingzc.Count > 0 && rysfzhm != "")
                                {
                                    string notstr = "";
                                    foreach (var dzc in deletingzc)
                                    {
                                        if (notstr != "")
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
                            if (gwlist != null && gwlist.Count > 0)
                            {
                                string rysfzhm = "";
                                List<string> zsbhlist = new List<string>();
                                foreach (var gwdata in gwlist)
                                {
                                    Dictionary<string, object> gw = (Dictionary<string, object>)gwdata;
                                    if (gw != null && gw.Count > 0)
                                    {
                                        string gwname = gw["PostClassName"].GetSafeString();
                                        // 只处理技术工人
                                        List<string> allgwlist = new List<string>() {
                                                                "现场作业人员",
                                                                "特种作业人员"
                                                            };
                                        if (allgwlist.Contains(gwname))
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
            #endregion

            #region 下载企业工程信息
            lsql.Clear();
            object prjlist = null;
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
                                if (PrjGuid != "")
                                {
                                    object projectinfo = null;
                                    if (ZJJGPublicData.GetProjectInfo(PrjGuid, out msg, out projectinfo))
                                    {
                                        if (projectinfo != null)
                                        {
                                            Dictionary<string, object> projectdata = (Dictionary<string, object>)projectinfo;
                                            if (projectdata != null && projectdata.Count > 0)
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
                                                    gcszdsf = TbProjectInfo[0]["ProvinceName"].GetSafeString();
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

                                                if (gcmc != "")
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
            #endregion

        }

        private bool SaveFile(string FileName, string ext, byte[] content, out string id,out string msg)
        {
            bool ret = true;
            msg = "";
            id = "";
            try
            {
                string fileid = Guid.NewGuid().ToString("N");
                string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILENAME", FileName);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILECONTENT", content);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILEEXT", ext);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlparams.Add(sqlparam);
                if (CommonService.ExecTrans(sqlstr, sqlparams, out msg))
                {
                    ret = true;
                    msg = "";
                    id = fileid;
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

    }
}