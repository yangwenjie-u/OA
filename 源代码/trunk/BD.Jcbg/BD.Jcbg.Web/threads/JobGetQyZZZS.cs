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

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 从省诚信平台自动下载企业资质证书
    /// </summary>
    public class JobGetQyZZZS:ISchedulerJob
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
            SysLog4.WriteError("开始线程JobGetQyZZZS");
            while (true)
            {
                try
                {
                    // 每次获取未更新或者更新超过两天的企业信息
                    string sql = "select top 1 qybh,qymc from i_m_qy " +
                        " where ZZZSLastUpdateTime is null or ZZZSLastUpdateTime < dateadd(day,-2,getdate()) ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string qybh = dt[0]["qybh"].GetSafeString();
                        string qymc = dt[0]["qymc"].GetSafeString();
                        if (qymc !="")
                        {
                            var q = new EnterpriseInformationQuery();
                            var ret = q.Query(qymc);
                            // 获取成功，将所有建筑业资质写入数据库
                            if (ret.Success)
                            {
                                if (ret.data.QYZZ.Count > 0)
                                {
                                    foreach (var qyzz in ret.data.QYZZ)
                                    {
                                        List<string> lsql = GetZZSql(qyzz, qybh);
                                        if (lsql.Count > 0)
                                        {
                                            if (CommonService.ExecTrans(lsql))
                                            {
                                                sql = string.Format("update i_m_qy set ZZZSLastUpdateTime=getdate() where qybh='{0}'", qybh);
                                                CommonService.Execsql(sql);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                SysLog4.WriteError(string.Format("执行线程出错：\r\n企业名称：｛0｝\r\n错误：{1}", qymc, ret.Message));
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

        private List<string> GetZZSql(CL_EnterpriseInformationQuery_QYZZ qyzz, string qybh)
        {
            List<string> lsql = new List<string>();
            if (qyzz.ZZMC == "建筑业（新）")
            {
                string zzzsbh = qyzz.ZZZSBH.GetSafeString();
                string fzjg = qyzz.FZJG.GetSafeString();
                string fzrq = qyzz.FZRQ.GetSafeString();
                string yxq = qyzz.YXQZ.GetSafeString();
                // 根据资质证书编号删除资质范围
                string sql = "delete from jdbg_qyzz_zzfw " +
                    " where zzid in (select id from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}')";
                sql = string.Format(sql, qybh, zzzsbh);
                lsql.Add(sql);

                // 根据资质证书编号删除资质证书
                sql = string.Format(" delete from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}'", qybh, zzzsbh);
                lsql.Add(sql);

                // 插入资质证书信息
                string id = Guid.NewGuid().ToString("N");
                if (fzrq !="")
                {
                    fzrq = fzrq.Replace("年", "-").Replace("月", "-").Replace("日", "");
                }

                if (yxq != "")
                {
                    yxq = yxq.Replace("年", "-").Replace("月", "-").Replace("日", "");
                }
                sql = "insert into jdbg_qyzz (id,qybh,zzzsbh,fzrq,zsyxq,fzjg) " +
                        " values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                sql = string.Format(sql, id, qybh, zzzsbh, fzrq, yxq, fzjg);
                lsql.Add(sql);

                // 插入资质证书范围
                if (qyzz.ZZFW.Count > 0)
                {
                    foreach (var zzfw in qyzz.ZZFW)
                    {
                        string zzfwmc = zzfw.ZZFWMC;
                        string jsfzr = zzfw.JSFZR;
                        // 资质范围等级名称替换，保持与保准一致
                        zzfwmc = zzfwmc.Replace("壹级", "一级").Replace("贰级", "二级").Replace("叁级", "三级");
                        if (zzfwmc !="")
                        {
                            sql = "insert into jdbg_qyzz_zzfw (zzid, zzfw,jsfzr) " +
                                    "values ('{0}','{1}','{2}')";
                            sql = string.Format(sql, id, zzfwmc, jsfzr);
                            lsql.Add(sql);
                        }
                    }
                }



            }

            return lsql;
        }
    }
}