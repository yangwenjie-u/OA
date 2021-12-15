using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 工地人员考勤处理类，设置考勤日志所在的工程和企业，设置app确认
    /// 暂时用于周总项目
    /// </summary>
    public class JobOutUserKqLog:ISchedulerJob
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
            SysLog4.WriteError("开始线程JobOutUserKqLog");
            while (true)
            {
                try
                {
                    // 工程信息
                    IList<IDictionary<string,string>> dtgcs = CommonService.GetDataTable("select gcbh,gcmc from i_m_gc");
                    // 企业信息
                    IList<IDictionary<string,string>> dtqys = CommonService.GetDataTable("select qybh,qymc from i_m_qy");
                    // 考勤机信息
                    IList<IDictionary<string, string>> dtkqjs = CommonService.GetDataTable("select * from i_m_kqj");
                    // 人员信息
                    IList<IDictionary<string, string>> dtrys = CommonService.GetDataTable("select rybh,sfzhm from i_m_ry");
                    // 考勤信息
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select recid,userid,serial,logdate from kqjuserlog where hasdeal=0");
                    IList<string> sqls = new List<string>();
                    foreach (IDictionary<string, string> row in dt)
                    {
                        string recid = row["recid"];
                        string sfzhm = row["userid"];
                        string serial = row["serial"];
                        DateTime logdate = row["logdate"].GetSafeDate();
                        // 获取考勤机的工程和企业编号
                        string qybh = "", gcbh = "";
                        var q = from e in dtkqjs where e["kqjbh"]==serial select e;
                        if (q.Count() > 0){
                            var kqj = q.First();
                            qybh = kqj["qybh"];
                            gcbh = kqj["gcbh"];
                            // 设置考勤记录的公司和工程信息
                            sqls.Add("update kqjuserlog set companyid='" + qybh + "',placeid='" + gcbh + "' where recid=" + recid);
                        }
                        // 获取人员编号
                        string rybh = "";
                        q = from e in dtrys where e["sfzhm"] == sfzhm select e;
                        if (q.Count() > 0)
                        {
                            rybh = q.First()["rybh"];
                        }
                        // 考勤地点
                        string placename = "";
                        if (gcbh != "")
                        {
                            q = from e in dtgcs where e["gcbh"] == gcbh select e;
                            if (q.Count() > 0)
                                placename = q.First()["gcmc"];
                        }
                        else if (qybh != "")
                        {
                            q = from e in dtqys where e["qybh"] == qybh select e;
                            if (q.Count() > 0)
                                placename = q.First()["qymc"];
                        }
                        // 手机确认
                        if (rybh != "")
                        {
                            sqls.Add("INSERT INTO [PhoneAlert]([Reader],[title],[context],[Createon],[type],[logid]) VALUES('"+rybh+"','考勤确认','您"+logdate.ToString("yyyy-MM-dd HH:mm:ss")+"在【"+placename+"】考勤成功，请确认',getdate() ,'qr',"+recid+")");
                        }
                        
                        // 设置已处理
                        sqls.Add("update kqjuserlog set hasdeal=1 where recid=" + recid);
                    }
                    CommonService.ExecTrans(sqls);
                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);
                    
                }


                Thread.Sleep(Interval);
            }

            SysLog4.WriteError("退出线程JobOutUserKqLog");
        }
    }
}