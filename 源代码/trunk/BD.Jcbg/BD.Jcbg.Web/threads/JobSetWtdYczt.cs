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
    /// 设置委托单异常状态
    /// </summary>
    public class JobSetWtdYczt:ISchedulerJob
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

        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                if (_jcService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                }
                return _jcService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobSetWtdYczt");
            while (true)
            {
                try
                {
                    //---------------上传报告处理------------
                    DateTime dtDealtime = DateTime.Now;
                    // 未处理数据
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select distinct wtdbh from ((select distinct wtdbh from up_bgsj where sfcl is null or sfcl=0) union all (select distinct wtdbh from up_sysj where sfcl is null or sfcl=0)) as t1");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        string wtdbh = row["wtdbh"];
                        string msg = "";
                        bool code = SetWtdycztIn(wtdbh, out msg);
                        if (!code)
                        {
                            SysLog4.WriteError("设置委托单异常状态失败，委托单唯一号：" + wtdbh + "，错误消息：" + msg);
                        }
                        else
                        {
                            IList<string> sqls = new List<string>();
                            sqls.Add("update up_bgsj set sfcl=1 where wtdbh='" + wtdbh + "' and scsj<=convert(datetime,'" + dtDealtime.ToString("yyyy-MM-dd HH:mm:ss") + "') and (sfcl is null or sfcl=0)");
                            sqls.Add("update up_sysj set sfcl=1 where wtdbh='" + wtdbh + "' and scsj<=convert(datetime,'" + dtDealtime.ToString("yyyy-MM-dd HH:mm:ss") + "') and (sfcl is null or sfcl=0)");
                            CommonService.ExecTrans(sqls);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);
                    
                }


                Thread.Sleep(Interval);
            }

            //SysLog4.WriteError("退出线程JobSetWtdYczt");
        }

        private bool SetWtdycztIn(string wtdwyh, out string msg)
        {

            bool ret = false;
            msg = "";
            //return true;
            try
            {
                // ------------------获取委托单送样单位---------------
                IList<IDictionary<string,string>> wtd = CommonService.GetDataTable("select sydwbh from m_by where recid='"+wtdwyh+"'");
                string dwbh = "";
                if (wtd.Count > 0)
                    dwbh = wtd[0]["sydwbh"];
                // -------------获取上传的最后一份报告数据-------------
                IList<IDictionary<string, string>> upbgsj = CommonService.GetDataTable("select bgwyh from UP_BGSJ where wtdbh='" + wtdwyh + "' order by scsj desc");
                string bgwyh = "";
                if (upbgsj.Count > 0)
                    bgwyh = upbgsj[0]["bgwyh"];
                IList<IDictionary<string, string>> upbgxqm = null;
                IList<IDictionary<string, string>> upbgxqs = null;
                if (bgwyh != "")
                {
                    upbgxqm = CommonService.GetDataTable("select * from UP_BGXQM where bgwyh='" + bgwyh + "'");
                    upbgxqs = CommonService.GetDataTable("select * from UP_BGXQS where bgwyh='" + bgwyh + "'");
                }
                // --------------获取上传的采集数据-------------
                IList<IDictionary<string, string>> upsysj = CommonService.GetDataTable("select * from UP_SYSJ where wtdbh='" + wtdwyh + "' order by scsj asc");
                string sywyhs = "";
                bool hasVideo = false;
                foreach (IDictionary<string, string> row in upsysj)
                {
                    sywyhs += row["sywyh"] + ",";
                    if (row["spwj"].GetSafeString() != "" || row["lpwj"].GetSafeString() != "")
                        hasVideo = true;
                }
                sywyhs = sywyhs.FormatSQLInStr();
                IList<IDictionary<string, string>> upsyxq = CommonService.GetDataTable("select * from UP_SYXQ where sywyh in (" + sywyhs + ")");
                IList<IDictionary<string, string>> upsyqx = CommonService.GetDataTable("select count(*) as c1 from up_syqx where sywyh in (" + sywyhs + ")");
                IList<IDictionary<string, string>> upczsj = CommonService.GetDataTable("select count(*) as c1 from UP_CZSJ where sywyh in (" + sywyhs + ")");
                
                // -----------------获取变更单--------------------
                IList<IDictionary<string, string>> upbgds = CommonService.GetDataTable("select count(*) as c1 from up_bgd where wtdbh='" + wtdwyh + "'");
                // -----------------设置数据状态------------------                
                WtsSjzt sjzt = new WtsSjzt();
                // 采集数据
                if (upsysj.Count > 0)
                    sjzt.AddStatus(WtsSjzt.HasData);
                // 曲线
                if (upsyqx[0]["c1"].GetSafeInt() > 0)
                    sjzt.AddStatus(WtsSjzt.HasCurve);
                // 视频
                if (hasVideo)
                    sjzt.AddStatus(WtsSjzt.HasVideo);
                // -------------------设置异常状态-----------------
                WtsYczt yczt = new WtsYczt();
                // 委托单修改
                /*
                var q1 = from e in upbgxqm where e["bjjg"].GetSafeInt() == 1 select e;
                var q2 = from e in upbgxqs where e["bjjg"].GetSafeInt() == 1 select e;
                if (q1.Count() > 0 || q2.Count() > 0)
                    yczt.AddStatus(WtsYczt.WtsModify);*/
                if (upbgds[0]["c1"].GetSafeInt() > 0)
                    yczt.AddStatus(WtsYczt.WtsModify);

                // 自动采集数据修改
                if (DataModify(bgwyh, upbgxqs, upsysj, upsyxq))
                    yczt.AddStatus(WtsYczt.DataModify);
                // 未保存数据
                var q4 = from e in upsysj where e["sfbc"].GetSafeBool()==false select e;
                if (q4.Count() > 0)
                    yczt.AddStatus(WtsYczt.DataUnsave);
                // 重做数据
                if (upczsj[0]["c1"].GetSafeInt() > 0)
                    yczt.AddStatus(WtsYczt.DataRedo);
                // 重复报告
                if (upbgsj.Count > 1)
                    yczt.AddStatus(WtsYczt.ReportRepeat);
                // 重复试验
                if (DataRepest(upsysj))
                    yczt.AddStatus(WtsYczt.DataRepeat);
                // --------------------人员属于单位及考勤记录比对------------------
                if (Configs.LabAssTimeTagCompare.IndexOf(dwbh)>-1)
                {
                    IList<IDictionary<string, string>> dtlogs = CommonService.GetDataTable("select a.sywyh,a.syjssj,a.syr,b.sydwbh,c.rybh,(select min(logdate) from kqjuserlog d where d.userid=c.sfzhm and convert(varchar(50),d.logdate,23)=convert(varchar(50),a.syjssj,23)) as logdate from up_sysj a inner join m_by b on a.wtdbh=b.recid left outer join i_m_ry c on c.qybh=b.sydwbh and c.ryxm=a.syrxm where a.wtdbh='"+wtdwyh+"' and  a.sfbc=1");
                    q4 = from e in dtlogs where e["rybh"].GetSafeString() == "" select e;
                    if (q4.Count() > 0)
                        yczt.AddStatus(WtsYczt.PersonNotFind);
                    else
                    {
                        q4 = from e in dtlogs where e["logdate"].GetSafeString() == "" select e;
                        if (q4.Count() > 0)
                            yczt.AddStatus(WtsYczt.PersonLogNotFind);
                    }
                }
                string sql = "update m_by set yczt=" + yczt.Status + ",sjzt=" + sjzt.Status + ",SYSJZHSCSJ=(select max(scsj) from up_sysj where wtdbh='" + wtdwyh + "'),BGZHSCSJ=(select max(scsj) from up_bgsj where wtdbh='" + wtdwyh + "'),SYKSSJ=(select top 1 sykssj from up_sysj where wtdbh='" + wtdwyh + "' order by scsj desc),SYJSSJ=(select top 1 SYJSSJ from up_sysj where wtdbh='" + wtdwyh + "' order by scsj desc),jcjg=(select top 1 jcjg from up_bgsj where wtdbh='" + wtdwyh + "' order by scsj desc),jcjgms=(select top 1 jcjgms from up_bgsj where wtdbh='" + wtdwyh + "' order by scsj desc)  where recid='" + wtdwyh + "'";
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
                ret = CommonService.ExecTrans(sqls);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取一个委托单的最后试验唯一号，根据symc,zh分组
        /// </summary>
        /// <param name="sysjs">已经根据scsj逆序</param>
        /// <returns>试验名称，试验唯一号的集合</returns>
        private IDictionary<string, string> GetLastSywyhGroupBySymc(IList<IDictionary<string, string>> sysjs)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                foreach (IDictionary<string, string> row in sysjs)
                {
                    string sywyh = row["sywyh"];
                    string symc = row["symc"];
                    string zh = row["zh"];
                    string key = symc + "_" + zh;
                    if (!ret.ContainsKey(key))
                        ret.Add(key, sywyh);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 判断是否有重复数据，更具symc和zh分组
        /// </summary>
        /// <param name="sysjs"></param>
        /// <returns></returns>
        private bool DataRepest(IList<IDictionary<string, string>> sysjs)
        {
            bool ret = false;
            try
            {
                IList<string> exists = new List<string>();
                foreach (IDictionary<string, string> row in sysjs)
                {
                    if (row["sfbc"].GetSafeBool() == false)
                        continue;
                    string symc = row["symc"];
                    string zh = row["zh"];
                    string key = symc+"_"+zh;
                    var q = from e in exists where e == key select e;
                    if (q.Count() > 0)
                        return true;
                    exists.Add(key);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 判断采集数据是否有修改
        /// </summary>
        /// <param name="bgwyh">报告唯一号</param>
        /// <param name="upbgxqs">报告详情从表</param>
        /// <param name="upsysjs">试验数据</param>
        /// <param name="upsyxqs">试验详情</param>
        /// <returns></returns>
        private bool DataModify(string bgwyh, 
            IList<IDictionary<string,string>> upbgxqs,
            IList<IDictionary<string,string>> upsysjs,
            IList<IDictionary<string,string>> upsyxqs)
        {
            bool ret = false;
            try
            {
                if (bgwyh == "")
                    return ret;
                if (upsysjs == null || upsysjs.Count == 0)
                    return ret;
                // 获取不同项目，组的最后一个试验
                IList<IDictionary<string, string>> sywyhs = new List<IDictionary<string, string>>();
                string strSywyhs = "";
                IList<string> zhs = new List<string>();
                var qf = from e in upsysjs where e["sfbc"].GetSafeBool() == true select e;
                IList<IDictionary<string, string>> savedUpsysjs = qf.ToList();
                for (int i = 0; i < savedUpsysjs.Count; i++)
                {
                    IDictionary<string, string> currow = savedUpsysjs[i];
                    bool isvalid = true;
                    for (int j = i + 1; j < savedUpsysjs.Count; j++)
                    {
                        IDictionary<string, string> nextrow = savedUpsysjs[j];
                        if (nextrow["sfbc"].GetSafeBool() == false)
                            continue;
                        if (currow["symc"] == nextrow["symc"] && currow["zh"] == nextrow["zh"])
                        {
                            isvalid = false;
                            break;
                        }
                    }
                    if (isvalid)
                    {
                        sywyhs.Add(currow);
                        strSywyhs += currow["sywyh"] + ",";
                        string zh = currow["zh"];
                        var q = from e in zhs where e == zh select e;
                        if (q.Count() == 0)
                            zhs.Add(zh);
                    }
                }
                // 过滤试验详情
                qf = from e in upsyxqs where strSywyhs.IndexOf(e["sywyh"]) > -1 select e;
                IList<IDictionary<string, string>> dtSysjs = qf.ToList();
                // 获取采集记录所有字段含义
                IList<string> heads = new List<string>();
                foreach (IDictionary<string, string> row1 in dtSysjs)
                {
                    string zdyh = row1["zdhy"];
                    var q = from e in heads where e == zdyh select e;
                    if (q.Count() == 0)
                        heads.Add(zdyh);
                }
                // 比较
                var qz = from e in zhs orderby e.GetSafeInt() select e;
                foreach (var zh in qz)
                {
                    // 获取该组号的试验唯一号
                    foreach (string zdhy in heads)
                    {
                        // 试验数据
                        var q1 = from e in sywyhs where e["zh"] == zh select e["sywyh"];
                        string sysj = "";
                        foreach (var sywyh in q1)
                        {
                            var q2 = from e in dtSysjs where e["sywyh"] == sywyh && e["zdhy"] == zdhy select e;
                            if (q2.Count() > 0)
                            {
                                sysj = q2.First()["zdz"];
                                break;
                            }
                        }
                        // 报告数据
                        string bgsj = "";
                        var q3 = from e in upbgxqs where e["zh"] == zh && e["zdhy"] == zdhy select e;
                        if (q3.Count() > 0)
                        {
                            bgsj = q3.First()["zdz"];
                        }

                        if (sysj.GetSafeInt() != bgsj.GetSafeInt())
                        {
                            ret = true;
                            break;
                        }
                    }
                    if (ret)
                        break;
                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
    }
}