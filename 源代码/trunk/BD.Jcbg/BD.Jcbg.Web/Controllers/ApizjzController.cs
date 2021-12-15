using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using SysLog4 = BD.Jcbg.Common.SysLog4;
using System.Web.Http;
using System.Web;
using System.Net.Http;

namespace BD.Jcbg.Web.Controllers
{

    public class ApizjzController : ApiController
    {
        #region 服务
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

        #region 标点老办公接口
        public IDictionary<string, object> DownReports([FromBody]VTransZjzDownReportsCondition condition)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            IList<IDictionary<string, string>> records = new List<IDictionary<string, string>>();
            try
            {
                condition.StationId = condition.StationId.GetSafeRequest();
                condition.LastId = condition.LastId.GetSafeRequest();
                code = JcService.GetStationEnctyptKey(condition.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(condition.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        records = JcService.GetDownReports(condition.StationId, condition.LastId, condition.Count, out msg);
                        code = msg == "";
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
                retObj.Add("records", records);
            }
            return retObj;
        }

        public IDictionary<string, object> PageReports([FromBody]VTransZjzPageReportsCondition condition)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            int totalcount = 0;
            IList<IDictionary<string, string>> records = new List<IDictionary<string, string>>();
            try
            {
                code = JcService.GetStationEnctyptKey(condition.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(condition.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        records = JcService.GetDownReports(condition.StationId, condition.Jcdwbh, condition.Jcdwmc,
                            condition.Wtdbh, condition.Syxmbh, condition.Syxmmc, condition.Bgbh, condition.Zjdjh,
                            condition.Gcbh, condition.Gcmc, condition.Khdwmc, condition.Jcjg, condition.Qfrq1,
                            condition.Qfrq2, condition.Scsj1, condition.Scsj2,
                            condition.PageSize, condition.PageIndex, condition.OrderField,
                            out totalcount, out msg);
                        code = msg == "";
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
                retObj.Add("totalcount", totalcount);
                retObj.Add("records", records);
            }
            return retObj;
        }

        public IDictionary<string, object> SetReportDealOpinion([FromBody]VTransZjzReportDealOpinion opinion)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            try
            {
                code = JcService.GetStationEnctyptKey(opinion.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(opinion.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        code = JcService.SetReportDealOpinion(opinion.StationId, opinion.Bgwyh, opinion.Opinion, out msg);
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
            }
            return retObj;
        }

        public IDictionary<string, object> SetReportDealResult([FromBody]VTransZjzReportDealResult result)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            try
            {
                code = JcService.GetStationEnctyptKey(result.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(result.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        code = JcService.SetReportDealResult(result.StationId, result.Bgwyh, result.Result, out msg);
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
            }
            return retObj;
        }

        public IDictionary<string, object> PageProjects([FromBody]VTransZjzPageProjectCondition condition)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                code = JcService.GetStationEnctyptKey(condition.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(condition.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        VTransDownGetGc where = new VTransDownGetGc()
                        {
                            gcbh = condition.Gcbh,
                            gclx = condition.Gclx,
                            gcmc = condition.Gcmc,
                            gcqy = condition.Gcqy,
                            jldw = condition.Jldw,
                            jsdw = condition.Jsdw,
                            jzry = condition.Jzry,
                            sgdw = condition.Sgdw,
                            syry = condition.Syry
                        };
                        code = JcService.GetGcs("", condition.StationId, where,
                            condition.PageSize, condition.PageIndex,
                            out totalcount, out records, out msg);
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
                retObj.Add("totalcount", totalcount);
                retObj.Add("records", records);
            }
            return retObj;
        }
        #endregion

        #region 获取质监站下的所有检测机构
        [HttpPost]
        public IDictionary<string, object> GetJcJgs([FromBody]VTransZjzJcjgsCondition opinion)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            IList<IDictionary<string, string>> records = new List<IDictionary<string, string>>();
            try
            {
                code = JcService.GetStationEnctyptKey(opinion.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(opinion.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        code = JcService.GetJcjgs(opinion.StationId, out records, out msg);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
                retObj.Add("records", records);
            }
            return retObj;
        }
        #endregion

        #region 根据检测机构获取试验项目
        [HttpPost]
        public IDictionary<string, object> GetSyxms([FromBody]VTransZjzSyxmsCondition opinion)
        {
            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                code = JcService.GetStationEnctyptKey(opinion.StationId, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(opinion.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        code = JcService.GetSyxms(opinion.StationId, opinion.Qybh, out records, out msg);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code.ToString().ToLower());
                retObj.Add("msg", msg);
                retObj.Add("records", records);
            }
            return retObj;
        }
        #endregion

        #region 更新监督抽查联系单内容
        [HttpPost]
        public IDictionary<string, object> SaveJDCCLXD()
        {
            HttpRequest Request = HttpContext.Current.Request;
            VJG_JDBG_JDCCRWWTJL option = new VJG_JDBG_JDCCRWWTJL();
            option.stationid = Request["stationid"];
            option.key = Request["key"];
            option.recid = Request["recid"].GetSafeInt();
            option.gcbh = Request["gcbh"];
            option.zjdjh = Request["zjdjh"];
            option.qybh = Request["qybh"];
            option.qymc = Request["qymc"];
            option.syxmbh = Request["syxmbh"];
            option.syxmmc = Request["syxmmc"];
            option.jbrzh = Request["jbrzh"];
            option.jbrxm = Request["jbrxm"];
            option.wtdbh = Request["wtdbh"];
            option.no = Request["no"];
            option.gcmc = Request["gcmc"];
            option.gcdd = Request["gcdd"];
            option.sy_cyrq = Request["sy_cyrq"].GetSafeDate();
            option.cynr = Request["cynr"];
            option.cjbw = Request["cjbw"];
            option.lxr = Request["lxr"];
            option.cjnr = Request["cjnr"];
            option.ypggsl = Request["ypggsl"];
            option.wtks = Request["wtks"];
            option.bz = Request["bz"];
            option.workserial = Request["workserial"];
            option.fileid = "";
            option.lrsj = Request["lrsj"].GetSafeDate();
            option.sprzh = Request["sprzh"];
            option.sprxm = Request["sprxm"];
            option.spsj = Request["spsj"].GetSafeDate();
            option.spzt = Request["spzt"].GetSafeBool();
            option.spyj = Request["spyj"];
            option.word = Request["word"];


            IDictionary<string, object> retObj = new Dictionary<string, object>();
            bool code = false;
            string msg = "";
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                code = JcService.GetStationEnctyptKey(option.stationid, "1", out msg);
                if (code)
                {
                    string shouldKey = MD5Util.GetCommonMD5(msg);
                    if (!shouldKey.Equals(option.key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {

                        code = JcService.InsertJG_JDBG_JDCCRWWTJL(option, out msg);
                        if (code)
                        {
                            JcService.InsertJG_JDBG_JDCCRWWTJL_DX(option, out msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                retObj.Add("code", code ? "0" : "1");
                retObj.Add("msg", msg);
            }
            return retObj;
        }
        #endregion
    }
}
