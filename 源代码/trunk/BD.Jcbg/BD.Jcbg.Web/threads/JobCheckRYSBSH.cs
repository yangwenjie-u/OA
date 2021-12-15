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
using BD.Jcbg.Web.Func;
using System.Collections;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 绍兴市建筑业资质管理系统
    /// 定时校验人员社保
    /// 只校验资料申报进行到是建设局的那些企业相关的人员
    /// </summary>
    public class JobCheckRYSBSH : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobCheckRYSBSH");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string upsql = "";
                    int days = GlobalVariable.GetConfigValue("rysb_days").GetSafeInt();
                    int topnum = GlobalVariable.GetConfigValue("rysb_topnum").GetSafeInt();
                    string  twhere = "qybh in (select qybh from jdbg_qyzzsb where sqzt in (3,4,5,6,10,11,12,13) )";
                    #region 校验技术负责人
                    string sql = "select top " + topnum.ToString() + " * from view_i_s_qy_jsfzr  where " + twhere + " order by sblastupdatetime";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    SysLog4.WriteError("技术负责人数量：" + dt.Count);
                    IList<string> lsql = new List<string>();
                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string recid = row["recid"];
                            string name = row["ryxm"];
                            string idcard = row["sfzhm"];
                            string qymc = row["qymc"];
                            string areaAK = row["areaak"];
                            string qysbdwbh = row["qysbdwbh"];
                            bool issysbyc = false;
                            string sysbycnr = "";
                            success = CheckSb(name, idcard, areaAK, qymc, qysbdwbh, out msg, out issysbyc, out sysbycnr);
                            upsql = string.Format("update i_s_qy_jsfzr set issbyc={0}, sbycnr='{1}',SBLASTUPDATETIME=getdate(), issysbyc={3},sysbycnr='{4}' where recid={2}", success ? "0" : "1", msg, recid,issysbyc ? "1":"0", sysbycnr);
                            lsql.Add(upsql);
                            //CommonService.Execsql(upsql);
                        }
                        if (lsql.Count > 0)
                        {
                            CommonService.ExecTrans(lsql);
                        }
                        
                    }
                    #endregion

                    #region 校验注册建造师
                    lsql.Clear();
                    sql = "select top " + topnum.ToString() + " * from view_i_s_qy_zcjzs where " + twhere + " order by sblastupdatetime";
                    dt = CommonService.GetDataTable(sql);
                    SysLog4.WriteError("注册建造师数量：" + dt.Count);
                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string recid = row["recid"];
                            string name = row["ryxm"];
                            string idcard = row["sfzhm"];
                            string qymc = row["qymc"];
                            string qysbdwbh = row["qysbdwbh"];
                            string areaAK = row["areaak"];
                            bool issysbyc = false;
                            string sysbycnr = "";
                            success = CheckSb(name, idcard, areaAK, qymc, qysbdwbh, out msg, out issysbyc, out sysbycnr);
                            upsql = string.Format("update i_s_qy_zcjzs set issbyc={0}, sbycnr='{1}',SBLASTUPDATETIME=getdate(), issysbyc={3},sysbycnr='{4}'  where recid={2}", success ? "0" : "1", msg, recid, issysbyc ? "1" : "0", sysbycnr);
                            lsql.Add(upsql);
                            SysLog4.WriteError(upsql);
                            //CommonService.Execsql(upsql);
                        }
                        if (lsql.Count > 0)
                        {
                            CommonService.ExecTrans(lsql);
                        }
                        
                    }
                    #endregion

                    #region 校验中级以上职称人员
                    lsql.Clear();
                    sql = "select top " + topnum.ToString() + " * from view_i_s_qy_zjyszcry where " + twhere + " order by sblastupdatetime";
                    dt = CommonService.GetDataTable(sql);
                    SysLog4.WriteError("中级以上职称人员数量：" + dt.Count);
                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string recid = row["recid"];
                            string name = row["ryxm"];
                            string idcard = row["sfzhm"];
                            string qymc = row["qymc"];
                            string qysbdwbh = row["qysbdwbh"];
                            string areaAK = row["areaak"];
                            bool issysbyc = false;
                            string sysbycnr = "";
                            success = CheckSb(name, idcard, areaAK, qymc, qysbdwbh, out msg, out issysbyc, out sysbycnr);
                            upsql = string.Format("update i_s_qy_zjyszcry set issbyc={0}, sbycnr='{1}',SBLASTUPDATETIME=getdate(), issysbyc={3},sysbycnr='{4}'  where recid={2}", success ? "0" : "1", msg, recid, issysbyc ? "1" : "0", sysbycnr);
                            lsql.Add(upsql);
                            //CommonService.Execsql(upsql);
                        }
                        if (lsql.Count > 0)
                        {
                            CommonService.ExecTrans(lsql);
                        }

                    }
                    #endregion

                    #region 校验现场管理人员
                    lsql.Clear();
                    sql = "select top " + topnum.ToString() + " * from view_i_s_qy_xcglry  where " + twhere + " order by sblastupdatetime";
                    dt = CommonService.GetDataTable(sql);

                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string recid = row["recid"];
                            string name = row["ryxm"];
                            string idcard = row["sfzhm"];
                            string qymc = row["qymc"];
                            string qysbdwbh = row["qysbdwbh"];
                            string areaAK = row["areaak"];
                            bool issysbyc = false;
                            string sysbycnr = "";
                            success = CheckSb(name, idcard, areaAK, qymc, qysbdwbh, out msg, out issysbyc, out sysbycnr);
                            upsql = string.Format("update i_s_qy_xcglry set issbyc={0}, sbycnr='{1}',SBLASTUPDATETIME=getdate(), issysbyc={3},sysbycnr='{4}'  where recid={2}", success ? "0" : "1", msg, recid, issysbyc ? "1" : "0", sysbycnr);
                            lsql.Add(upsql);
                            //CommonService.Execsql(upsql);
                        }
                        if (lsql.Count > 0)
                        {
                            CommonService.ExecTrans(lsql);
                        }

                    }
                    #endregion

                    #region 校验技术工人
                    lsql.Clear();
                    sql = "select top " + topnum.ToString() + " * from view_i_s_qy_jsgr  where " + twhere + " order by sblastupdatetime";
                    dt = CommonService.GetDataTable(sql);
                    SysLog4.WriteError("技术工人数量：" + dt.Count);
                    if (dt.Count > 0)
                    {

                        foreach (var row in dt)
                        {
                            string recid = row["recid"];
                            string name = row["ryxm"];
                            string idcard = row["sfzhm"];
                            string qymc = row["qymc"];
                            string qysbdwbh = row["qysbdwbh"];
                            string areaAK = row["areaak"];
                            bool issysbyc = false;
                            string sysbycnr = "";
                            success = CheckSb(name, idcard, areaAK, qymc, qysbdwbh, out msg, out issysbyc, out sysbycnr);
                            upsql = string.Format("update i_s_qy_jsgr set issbyc={0}, sbycnr='{1}',SBLASTUPDATETIME=getdate(), issysbyc={3},sysbycnr='{4}'  where recid={2}", success ? "0" : "1", msg, recid, issysbyc ? "1" : "0", sysbycnr);
                            lsql.Add(upsql);
                            //CommonService.Execsql(upsql);
                        }
                        if (lsql.Count > 0)
                        {
                            CommonService.ExecTrans(lsql);
                        }

                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }

        }

        /// <summary>
        /// 校验人员社保信息
        /// </summary>
        private bool CheckSb(string  name, string idcard, string areaAK, string qymc, string qysbdwbh, out string msg, out bool issysbyc, out string sysbycnr)
        {
            bool ret = true;
            msg = "";
            issysbyc =  false ;
            sysbycnr = "";
            try
            {
                string result = "";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                // 能够获取到社保接口返回的信息
                // 不能获取到的社保接口返回信息，不作处理
                if (PersonnelSocialSecurity.GetPersonnelSocialSecurity(name, idcard, areaAK,out msg, out result))
                {
                    if (result != "")
                    {
                        //SysLog4.WriteError(result);
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                        if (retdata != null)
                        {
                            string code = retdata["code"].GetSafeString();
                            if (code == "00")
                            {
                                string info = retdata["datas"].GetSafeString();
                                if (info != "")
                                {
                                    Dictionary<string, object> dtinfo = jss.Deserialize<Dictionary<string, object>>(info);
                                    if (dtinfo != null)
                                    {
                                        bool success = dtinfo["success"].GetSafeBool();
                                        // 成功获取个人社保信息
                                        if (success)
                                        {
                                            // 分析社保信息中的单位名称和单位编号
                                            // 如果单位名称一致,表示该人员社保正常
                                            Dictionary<string, object> singleData = (Dictionary<string, object>)dtinfo["singleData"];
                                            if (singleData!=null && singleData.Count > 0)
                                            {
                                                // 社保信息中的单位相关的信息
                                                //固定格式：单位名称(单位编号)
                                                // (单位编号) 有可能没有，只有单位名称
                                                // 如：浙江标点信息科技有限公司(10008038)
                                                string qyinfo = singleData["aab004"].GetSafeString();
                                                if (qyinfo!="")
                                                {
                                                    string sbdwmc = "";
                                                    int idx = qyinfo.LastIndexOf('(');
                                                    // 未找到括号
                                                    if (idx == -1)
                                                    {
                                                        sbdwmc = qyinfo;
                                                    }
                                                    else
                                                    {
                                                        sbdwmc = qyinfo.Substring(0, idx);
                                                    }

                                                    if (qymc != sbdwmc)
                                                    {
                                                        ret = false;
                                                        msg = string.Format("单位名称不一致：系统中的单位名称为[{0}]，社保中的单位名称为[{1}]", qymc, sbdwmc);
                                                    }
                                                    else
                                                    {
                                                        string ylzt = singleData["ylzt"].GetSafeString(); // 养老
                                                        string ybzt = singleData["ybzt"].GetSafeString(); // 医保
                                                        string gszt = singleData["gszt"].GetSafeString(); // 工商
                                                        string syzt = singleData["syzt"].GetSafeString(); // 失业
                                                        string syuzt = singleData["syuzt"].GetSafeString();// 生育
                                                        // 只校验养老保险（社保局反馈，5个险种是一起交的，不存在一部分不交的情况）
                                                        if (!CheckJF(ylzt))
                                                        {
                                                            ret = false;
                                                            msg = string.Format("缴费异常");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ret = false;
                                                    msg = "社保单位信息为空！";
                                                }

                                            }

                                            // 分析最近三个月社保的单位是否一致
                                            ArrayList collectionData = (ArrayList)dtinfo["collectionData"];
                                            issysbyc = CheckSYSBYC(collectionData, out sysbycnr);


                                        }
                                        // 未获取到个人社保信息，一般是【无该人员信息,或人员信息有误】
                                        else
                                        {
                                            ret = false;
                                            msg = dtinfo["message"].GetSafeString();
                                        }

                                    }
                                }
                            }
                            else if (code !="44")  // 忽略调用者服务异常
                            {
                                ret = false;
                                msg = retdata["msg"].GetSafeString();
                            }
                        }
                    }
                   
                }
                else
                {
                    ret = false;
                    msg = result;
                    SysLog4.WriteError(result);
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

        /// <summary>
        /// 校验最近三个月社保单位是否一致
        /// </summary>
        /// <param name="collectionData"></param>
        /// <param name="sysbycnr"></param>
        /// <returns></returns>
        private bool CheckSYSBYC(ArrayList collectionData, out string sysbycnr)
        {
            // 默认: 近三月社保单位不一致
            // 只有在近三个月社保单位编号都不为空，并且相等的情况下
            // 才表示近三月社保单位一致
            bool ret = true;
            sysbycnr = "近三月社保单位不一致";
            int num = 3;
            if (collectionData!= null && collectionData.Count >= num)
            {
                int total = collectionData.Count;
                List<string> dwbmlist = new List<string>();
                for(int j = total - num; j < total; j++)
                {
                    Dictionary<string, object> jfxq = (Dictionary<string, object>)collectionData[j];
                    if (jfxq != null)
                    {
                        string dwbm = jfxq["aab001"].GetSafeString();
                        dwbmlist.Add(dwbm);
                    }
                }
                if ((dwbmlist.Where(x=>x!="").Count() == num) && (dwbmlist.Distinct().Count()==1) )
                {
                    ret = false;
                    sysbycnr = "";
                }

                
            }
            return ret;
        }

        /// <summary>
        /// 校验是否属于缴费异常
        /// 判断缴费状态信息
        /// </summary>
        /// <param name="jfstr"></param>
        /// <returns></returns>
        private bool CheckJF(string jfstr)
        {
            bool ret = true;
            List<string> jfyclist = new List<string>() {
                "终止缴费",
                "中止缴费",
                "未参保",
                "暂停缴费"
            };
            if (jfyclist.Contains(jfstr))
            {
                ret = false;
            }
            return ret;
        }
    }
}