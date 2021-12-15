using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Web.Script.Serialization;
using BD.Jcbg.Web.Func;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace BD.Jcbg.Web.service
{
    /// <summary>
    /// jczx 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://jc.wlzaz.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class jczx : System.Web.Services.WebService
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
        private ISmsService _smsService = null;
        private ISmsService SmsService
        {
            get
            {
                if (_smsService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                }
                return _smsService;
            }
        }

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

        [WebMethod(Description = "获取工程信息，返回信息为json字符串。dwbh:单位编号,jsonstr:查询条件（{'gcbh':'','gcmc':'','gcqy':'','gclx':'','jsdw':'','sgdw':'','jldw':'','jzry':'','syry':''}), pagesize:每页记录数（默认20）,pageindex:页码（从1开始）,key:校验码")]
        public string GetGcs(string dwbh, string jsonstr, string pagesize, string pageindex, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        VTransDownGetGc where = null;

                        if (jsonstr == "")
                            where = new VTransDownGetGc();
                        else
                        {
                            JsonDeSerializer<VTransDownGetGc> whereDe = new JsonDeSerializer<VTransDownGetGc>();
                            where = whereDe.DeSerializer(jsonstr, out msg);
                            if (msg != "")
                                msg = "查询条件jsonstr转换json失败，详细信息：" + msg;
                        }
                        if (msg == "")
                        {
                            if (GlobalVariable.UseNbht())
                                code = JcService.GetGcs(dwbh, "", where, pagesize.GetSafeInt(), pageindex.GetSafeInt(), out totalcount, out records, out msg);
                            else
                                code = JcService.GetGcs("", "", where, pagesize.GetSafeInt(), pageindex.GetSafeInt(), out totalcount, out records, out msg);
                        }

                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("total", totalcount);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }

            return ret;

        }

        [WebMethod(Description = "获取委托单内容，返回信息为json字符串。dwbh:单位编号,wtdwyh:委托单唯一号,key:校验码")]
        public string GetWtd(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            IDictionary<string, string> mtable = new Dictionary<string, string>();
            IList<IDictionary<string, string>> stable = new List<IDictionary<string, string>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                        code = JcService.GetWtd(wtdwyh, out mtable, out stable, out msg);
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", code ? "" : msg);
                row.Add("syxmbh", code ? msg : "");
                row.Add("m", mtable);
                row.Add("s", stable);
                ret = jss.Serialize(row);
            }

            return ret;
            
        }

        [WebMethod(Description = "获取委托单内容，返回信息为json字符串。dwbh:单位编号,jsonstr:json格式条件(格式如下[{'gcbh':'','lrr':''}]),key:校验码")]
        public string GetWtds(string dwbh, string jsonstr, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        if (jsonstr == "")
                            msg = "查询条件不能为空";
                        else
                        {
                            VTransDownGetWtd[] where = null;

                            JsonDeSerializer<VTransDownGetWtd[]> whereDe = new JsonDeSerializer<VTransDownGetWtd[]>();
                            where = whereDe.DeSerializer(jsonstr, out msg);
                            if (msg != "")
                                msg = "查询条件jsonstr转换json失败，详细信息：" + msg;
                            else
                            {
                                /*
                                foreach (VTransDownGetWtd item in where)
                                {
                                    if (item.lrr != "")
                                        item.lrr = Remote.UserService.GetUserCode(item.lrr);
                                }*/
                                code = JcService.GetWtds(dwbh, where, out records, out msg);
                            }
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                try
                {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = Int32.MaxValue;
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", code ? "" : msg);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("code", "false");
                    row.Add("msg", "json序列化失败:"+ex.Message);
                    ret = jss.Serialize(row);
                }
            }

            return ret;

        }

        [WebMethod(Description = "设置委托单的实际送样单位。dwbh:单位编号,wtdwyh:委托单唯一号，多个编号组成json数组,key:校验码")]
        public string SetWtdSydw(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";            

            IList<VTransDownSetWtd> records = new List<VTransDownSetWtd>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        JsonDeSerializer<VTransUpSetWtd[]> wtdDe = new JsonDeSerializer<VTransUpSetWtd[]>();
                        VTransUpSetWtd[] arrWtd = wtdDe.DeSerializer(wtdwyh, out msg);
                        if (msg != "")
                            msg = "委托单唯一号转json失败，详细内容：" + msg;
                        else
                        {
                            foreach (VTransUpSetWtd item in arrWtd)
                            {                                
                                code = JcService.SetWtdStatusXf(item.wtdwyh.GetSafeRequest(), dwbh, out msg);
                                VTransDownSetWtd retItem = new VTransDownSetWtd() { wtdwyh = item.wtdwyh, code = code.ToString().ToLower(), msg = msg };
                                records.Add(retItem);
                            }
                            code = true;
                            msg = "";
                        }
                    }

                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);

            }
            return ret;
        }
        [WebMethod(Description = "取消委托单的实际送样单位。dwbh:单位编号,wtdwyh:委托单唯一号，多个编号组成json数组,key:校验码")]
        public string CancelWtdSydw(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            
            IList<VTransDownSetWtd> records = new List<VTransDownSetWtd>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        JsonDeSerializer<VTransUpSetWtd[]> wtdDe = new JsonDeSerializer<VTransUpSetWtd[]>();
                        VTransUpSetWtd[] arrWtd = wtdDe.DeSerializer(wtdwyh, out msg);
                        if (msg != "")
                            msg = "委托单唯一号转json失败，详细内容：" + msg;
                        else
                        {
                            foreach (VTransUpSetWtd item in arrWtd)
                            {
                                code = JcService.CancelWtdStatusXf(item.wtdwyh.GetSafeRequest(), dwbh, out msg);
                                VTransDownSetWtd retItem = new VTransDownSetWtd() { wtdwyh = item.wtdwyh, code = code.ToString().ToLower(), msg = msg };
                                records.Add(retItem);
                            }
                            code = true;
                            msg = "";
                        }
                    }

                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);

            }
            return ret;
        }
        [WebMethod(Description = "上传试验原始数据。dwbh:单位编号,wtdwyh:委托单唯一号,syxmmc:试验项目名称,sybh:样品编号,zh:获取委托单从表里的组号（报告数据对应）,syr:试验人,sysb:试验设备,sykssj:试验开始时间,syjssj:试验结束时间,syqx:采集曲线jpg格式再base64编码,datajson:采集数据（单条）,videofiles:视频文件名（多个逗号分隔）,recordfiles:录屏文件名（多个逗号分隔）,czdatajson:重做记录json,sfbc:试验是否保存(1保存,其他都是没保存),key:密钥")]
        public string UpData(string dwbh, string wtdwyh, string syxmmc, string ypbh, string zh, string syr, string sysb, 
            string sykssj, string syjssj, string syqx, string videofiles, string recordfiles,
            string datajson, string czdatajson, string sfbc, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                syxmmc = syxmmc.GetSafeRequest();
                ypbh = ypbh.GetSafeRequest();
                zh = zh.GetSafeRequest();
                syr = syr.GetSafeRequest();
                sysb = sysb.GetSafeRequest();


                bool bsfbc = (sfbc == "1" || sfbc.ToLower() == "true") ? true : false;

                DateTime dtsykssj = sykssj.GetSafeDate(DateTime.MinValue);
                DateTime dtsyjssj = syjssj.GetSafeDate(DateTime.MinValue);
                
                if (dtsykssj.Year == DateTime.MinValue.Year)
                {
                    msg = "试验开始时间无效";
                }
                else if (dtsyjssj.Year == DateTime.MinValue.Year)
                {
                    msg = "试验结束时间无效";

                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                            code = JcService.UpData(dwbh, wtdwyh, syxmmc, ypbh, zh, syr, sysb, dtsykssj, dtsyjssj, syqx, videofiles, recordfiles, datajson, czdatajson, bsfbc, out msg);
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                if (!code)
                {
                    SysLog4.WriteError("上传试验数据失败，单位编号：" + dwbh + "，委托单唯一号：" + wtdwyh + ",样品编号：" + ypbh + ",组号：" + zh + ",试验曲线：" + (syqx.Length > 0 ? "不为空" : "为空"));
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;

        }

        [WebMethod(Description = "上传报告。dwbh:单位编号,wtdwyh:委托单唯一号,bgbh:报告编号,syr:试验人,shr:审核人,qfr:签发人,syrq:试验日期,qfrq:签发日期, jcjg:检测结果(0不下结论，1合格，2不合格)，jcjgms:检测结果描述,mdatajson:报告数据内容主表,sdatajson:报告数据内容从表,pdfjson:pdf文件内容,key:密钥")]
        public string UpReport(string dwbh, string wtdwyh, string bgbh, string syr, string shr, string qfr, 
            string syrq, string qfrq, string jcjg, string jcjgms, string mdatajson, string sdatajson, string pdfjson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                bgbh = bgbh.GetSafeRequest();
                shr = syr.GetSafeRequest();
                qfr = qfr.GetSafeRequest();
                jcjg = jcjg.GetSafeRequest();
                jcjgms = jcjgms.GetSafeRequest();

                DateTime dtsyrq, dtqfrq;
                dtsyrq = syrq.GetSafeDate(DateTime.MinValue);
                dtqfrq = qfrq.GetSafeDate(DateTime.MinValue);
                if (dtsyrq.Year == DateTime.MinValue.Year)
                {
                    msg = "试验日期无效";
                }
                else if (dtqfrq.Year == DateTime.MinValue.Year)
                {
                    msg = "签发日期无效";

                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            code = JcService.UpReport(dwbh, wtdwyh, bgbh, syr, shr, qfr,
                                dtsyrq, dtqfrq, jcjg.GetSafeInt(), jcjgms, mdatajson, sdatajson, pdfjson, false, false, null, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                if (!code)
                {
                    SysLog4.WriteError("上传报告数据失败，单位编号：" + dwbh + "，委托单唯一号：" + wtdwyh + "，原因：" + msg);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;

        }

        [WebMethod(Description = "获取最后一份加了二维码的报告。dwbh:单位编号,wtdwyh:委托单唯一号,key:密钥")]
        public string GetReport(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = true;
            string msg = "";
            IList<IDictionary<string,string>> records = new List<IDictionary<string,string>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        IDictionary<string, byte[]> files = JcService.GetReportFiles(wtdwyh, out msg);
                        if (files == null)
                            code = false;
                        else
                        {
                            foreach (var item in files)
                            {
                                IDictionary<string, string> row = new Dictionary<string, string>();
                                row.Add("bgbh", item.Key);
                                row.Add("file", item.Value.EncodeBase64());
                                records.Add(row);
                                //System.IO.File.WriteAllBytes("d:\\1.pdf", file);
                            }
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }

            return ret;

        }
        [WebMethod(Description = "上传变更单。dwbh:单位编号,wtdwyh:委托单唯一号,bgyy:变更原因,bgsj:变更时间,bgdjson:变更单json格式,key:密钥")]
        public string UpBgd(string dwbh, string wtdwyh, string bgyy, string bgsj, string bgdjson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                bgyy = bgyy.GetSafeRequest();

                DateTime dtbgsj;
                dtbgsj = bgsj.GetSafeDate(DateTime.MinValue);
                if (bgyy == "")
                {
                    msg = "变更原因无效";
                }
                else if (dtbgsj.Year == DateTime.MinValue.Year)
                {
                    msg = "变更时间无效";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            msg = "";
                            VTransUpBgd[] where = null;

                            JsonDeSerializer<VTransUpBgd[]> whereDe = new JsonDeSerializer<VTransUpBgd[]>();
                            where = whereDe.DeSerializer(bgdjson, out msg);
                            if (msg != "")
                                msg = "查询条件jsonstr转换json失败，详细信息：" + msg;
                            else
                                code = JcService.UpBgd(dwbh, wtdwyh, bgyy, dtbgsj, where, out msg);

                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;
        }
        /// <summary>
        /// 获取委托单二维码文字，检测中心设置中，允许提前获取二维码的才可以
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [WebMethod(Description="获取二维码")]
        public string GetBarcode(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    code = JcService.GetBarcode(dwbh, wtdwyh, out msg);
                }
                else
                    msg = "单位编号无效";
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;
        }
        /// <summary>
        /// 上传带二维码的报告，平台不需要加二维码，允许提前获取二维码的才可以
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="bgbh"></param>
        /// <param name="syr"></param>
        /// <param name="shr"></param>
        /// <param name="qfr"></param>
        /// <param name="syrq"></param>
        /// <param name="qfrq"></param>
        /// <param name="jcjg"></param>
        /// <param name="jcjgms"></param>
        /// <param name="mdatajson"></param>
        /// <param name="sdatajson"></param>
        /// <param name="pdfjson"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebMethod(Description = "上传报告")]
        public string UpReportHasBarcode(string dwbh, string wtdwyh, string bgbh, string syr, string shr, string qfr,
            string syrq, string qfrq, string jcjg, string jcjgms, string mdatajson, string sdatajson, string pdfjson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                bgbh = bgbh.GetSafeRequest();
                shr = syr.GetSafeRequest();
                qfr = qfr.GetSafeRequest();
                jcjg = jcjg.GetSafeRequest();
                jcjgms = jcjgms.GetSafeRequest();

                DateTime dtsyrq, dtqfrq;
                dtsyrq = syrq.GetSafeDate(DateTime.MinValue);
                dtqfrq = qfrq.GetSafeDate(DateTime.MinValue);
                if (dtsyrq.Year == DateTime.MinValue.Year)
                {
                    msg = "试验日期无效";
                }
                else if (dtqfrq.Year == DateTime.MinValue.Year)
                {
                    msg = "签发日期无效";

                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            code = JcService.UpReport(dwbh, wtdwyh, bgbh, syr, shr, qfr,
                                dtsyrq, dtqfrq, jcjg.GetSafeInt(), jcjgms, mdatajson, sdatajson, pdfjson, false, false, null, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;
        }
        // 获取用户代码
        [WebMethod]
        public string GetUsercode(string dwbh, string username, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = Remote.UserService.GetUserCode(username);
                        code = true;
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }

            return ret;
        }
        // 获取检测合同，返回信息为json字符串。dwbh:单位编号,jsonstr:查询条件（{'htlx':'合同类型名称,全匹配，选项:企业合同,监督合同','jchtbh':'合同编号',gcbh:'工程编号','gcmc':'工程名称','khdwmc':'客户单位名称','zjzmc':'质监站名称','zjdjh':'质监登记号','syrxm':'送样人姓名','sybmmc':'送样部门名称','gsbmmc':'归属部门名称','htqdr','合同签订人'}), pagesize:每页记录数（默认20）,pageindex:页码（从1开始）,key:校验码
        [WebMethod]
        public string GetJchts(string dwbh, string jsonstr, string pagesize, string pageindex, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        VTransDownGetJcht where = null;

                        if (jsonstr == "")
                            where = new VTransDownGetJcht();
                        else
                        {
                            JsonDeSerializer<VTransDownGetJcht> whereDe = new JsonDeSerializer<VTransDownGetJcht>();
                            where = whereDe.DeSerializer(jsonstr, out msg);
                            if (msg != "")
                                msg = "查询条件jsonstr转换json失败，详细信息：" + msg;
                        }
                        if (msg == "")
                        {
                            if (GlobalVariable.UseNbht())
                                code = JcService.GetJchts(dwbh, where, pagesize.GetSafeInt(), pageindex.GetSafeInt(), out totalcount, out records, out msg);
                        }

                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("total", totalcount);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }

            return ret;

        }
        // 校验见证信息，返回信息为json字符串。dwbh:单位编号,wtdwyh:委托单唯一号，多个逗号分隔
        [WebMethod]
        public string CheckJzxx(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";

                        code = JcService.JzqyGetWtdTpzt(dwbh, wtdwyh, out msg);
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }
            //SysLog4.WriteError("唯一号:"+  wtdwyh + ",结果:" + ret);
            return ret;

        }

        // 设置委托单锁定（不能更改），返回信息为json字符串。dwbh:单位编号,wtdwyh:委托单唯一号，多个逗号分隔。sfsd:是否锁定，1-锁定，0-解锁
        [WebMethod]
        public string SetWtdsd(string dwbh, string wtdwyh, string sfsd, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";

                        code = JcService.SetWtdSd(dwbh, wtdwyh, sfsd.GetSafeInt(), out msg);
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;

        }
        // 给见证人发送短信提醒。dwbh:单位编号,wtdwyh:委托单唯一号，多个逗号分隔。
        [WebMethod]
        public string SendJzrSms(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                
                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        string gcmc;
                        IList<string> ryxxs = new List<string>();
                        if (JcService.JzqyGetJzrSmsInfo(wtdwyh, out gcmc, out ryxxs))
                        {
                            SmsRequestMessageJzqy objContent = new SmsRequestMessageJzqy()
                            {
                                invokeId = GlobalVariable.GetSmsBaseInvokeId(),
                                phoneNumber = "",
                                templateCode = GlobalVariable.GetSmsMessageTemplateCodeJzqy(),
                                contentVar = new SmsVarJzqy()
                                {
                                    gcmc = gcmc
                                }
                            };
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            foreach (string sjhm in ryxxs)
                            {
                                objContent.phoneNumber = sjhm;
                                string contents = jss.Serialize(objContent);

                                SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), sjhm, contents, out msg);
                            }
                        }
                    }
                    code = true;
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;

        }
        /*
        [WebMethod(Description = "获取待上传收样图片的委托单列表。dwbh:单位编号,jsonstr:查询条件({'syxmbh':'试验项目编号','gcbh':'工程编号','key':'查询关键字'}),key:密钥")]
        public string GetSyWtdList(string dwbh, string jsonstr, string pagesize, string pageindex, string key);
        [WebMethod(Description = "获取单个委托单见证信息。dwbh:单位编号,jsonstr:查询条件({'wtdwyh':'委托单唯一号','ewmbh':'二维码编号'}),key:密钥")]
        public string GetSyWtd(string dwbh, string wtdwyh, string ewmbh, string key);
        [WebMethod(Description = "上传送样图片。dwbh:单位编号,ewmbh:二维码编号,scrxm:上传人姓名,lon:经度,lat:纬度,key:密钥")]
        public string UpSyImage(string dwbh, string ewmbh, string scrxm, string lon, string lat, string imagestr, string key);
        */
        // 获取质监站对应的工程
        // 获取工程信息，返回信息为json字符串。dwbh:单位编号,jsonstr:查询条件（{'gcbh':'','gcmc':'','gcqy':'','gclx':'','jsdw':'','sgdw':'','jldw':'','jzry':'','syry':''}), pagesize:每页记录数（默认20）,pageindex:页码（从1开始）,key:校验码
        [WebMethod]
        public string GetZjzGcs(string stationid, string jsonstr, string pagesize, string pageindex, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                stationid = stationid.GetSafeRequest();

                code = JcService.GetStationEnctyptKey(stationid, "1", out msg);
                if (code)
                {
                    string shouldKey = stationid.EncodeDesJk(msg);
                    if (!shouldKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "无效的key，请检查密钥或者是md5算法，md5结果不包含-";
                    }
                    else
                    {
                        msg = "";
                        VTransDownGetGc where = null;

                        if (jsonstr == "")
                            where = new VTransDownGetGc();
                        else
                        {
                            JsonDeSerializer<VTransDownGetGc> whereDe = new JsonDeSerializer<VTransDownGetGc>();
                            where = whereDe.DeSerializer(jsonstr, out msg);
                            if (msg != "")
                                msg = "查询条件jsonstr转换json失败，详细信息：" + msg;
                        }
                        if (msg == "")
                        {
                            if (GlobalVariable.UseNbht())
                                code = JcService.GetGcs("", stationid, where, pagesize.GetSafeInt(), pageindex.GetSafeInt(), out totalcount, out records, out msg);
                            else
                                code = JcService.GetGcs("", stationid, where, pagesize.GetSafeInt(), pageindex.GetSafeInt(), out totalcount, out records, out msg);
                        }
                    }
                }
                else
                    msg = "站点编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("total", totalcount);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }

            return ret;

        }
        // 设置委托单作废
        [WebMethod]
        public string SetWtdZf(string dwbh, string wtdwyh, string reason, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(reason))
                {
                    msg = "作废原因不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            msg = "";

                            code = JcService.SetWtdZf(dwbh, wtdwyh, reason, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }
        // 设置结算人余额
        [WebMethod]
        public string SetJsrYe(string dwbh, string jsrbh, string ye, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (!Regex.IsMatch(ye, @"^((-)?[1-9]\d{0,9}|0)([.]?|(\.\d{1,2})?)$"))
                {
                    msg = "余额必须为数字";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            msg = "";

                            code = JcService.SetJsrYe(dwbh, jsrbh, ye.GetSafeDecimal(), out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        // 将收样的委托单退回
        [WebMethod(Description = "将收样的委托单退回, dwbh: 单位编号, wtdwyh: 委托单唯一号, key: 密钥")]
        public string SetWtdWSY(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(wtdwyh))
                {
                    msg = "委托单唯一号不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            msg = "";
                            code = JcService.SetWtdWSY(dwbh, wtdwyh, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        //设置委托单报告查看
        [WebMethod(Description = "设置委托单报告查看, dwbh: 单位编号, wtdwyh: 委托单唯一号, key: 密钥")]
        public string SetWtdBgck(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(wtdwyh))
                {
                    msg = "委托单唯一号不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            msg = "";
                            code = JcService.SetWtdBgck(dwbh, wtdwyh, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }
        
        [WebMethod(Description = "判断是否监管唯一号。dwbh:单位编号,wtdwyh:委托单唯一号,key:密钥")]
        public string CheckWtdwyh(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            string data = "";
            ResultParam retObj = new ResultParam();
            try
            {
                dwbh = dwbh.GetSafeRequest();
                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        retObj = JcService.CheckWtdwyh(wtdwyh);
                        code = retObj.success;
                        data = retObj.data.GetSafeString();
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("data", data);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        [WebMethod(Description = "判断是否监管唯一号。dwbh:单位编号,wtdwyh:委托单唯一号,key:密钥")]
        public string CheckWtdwyhData(string dwbh, string wtdwyh, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            object data = null;
            ResultParam retObj = new ResultParam();
            try
            {
                dwbh = dwbh.GetSafeRequest();
                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        retObj = JcService.CheckWtdwyhData(wtdwyh);
                        code = retObj.success;
                        data = retObj.data;
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("data", data);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单受理委托编号
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="dataJson"></param>
        /// <param name="key"></param>
        /// <returns></returns>

        [WebMethod(Description = "设置委托单受理委托编号, dwbh: 单位编号, dataJson: 参数, key: 密钥")]
        public string SetSlWtdbh(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(dataJson))
                {
                    msg = "参数不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            var datas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(dataJson, out msg);

                            if (!string.IsNullOrEmpty(msg))
                                msg = "参数转json失败，详细内容：" + msg;
                            else
                            {
                                var result = JcService.SetSlWtdbh(dwbh, datas);

                                if (result.success)
                                {
                                    code = true;
                                    msg = "";
                                    var list = result.data as List<string>;
                                    code = CommonService.ExecTrans(list);

                                    if (!code)
                                        msg = "sql执行出错";
                                }
                                else
                                {
                                    code = false;
                                    msg = result.msg;
                                }
                            }
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单打印文件
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="dataJson"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取委托单打印文件, dwbh: 单位编号, dataJson: 参数, key: 密钥")]
        public string GetWtdDywj(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            string url = string.Empty;
            var records = new List<Dictionary<string,string>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(dataJson))
                {
                    msg = "参数不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            var datas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(dataJson, out msg);

                            if (!string.IsNullOrEmpty(msg))
                                msg = "参数转json失败，详细内容：" + msg;
                            else
                            {
                                foreach (var data in datas)
                                {
                                    var result = GetWtdDywjUrl(data, out url);
                                    var retItem = new Dictionary<string, string>();
                                    retItem.Add("wtdwyh", data["wtdwyh"]);
                                    retItem.Add("code", result.success.ToString().ToLower());
                                    retItem.Add("msg", result.msg);
                                    retItem.Add("url", url);
                                    records.Add(retItem);
                                }

                                code = true;
                                msg = "";
                            }
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        private ResultParam GetWtdDywjUrl(Dictionary<string, string> dict, out string url)
        {
            var result = JcService.GetWtdDywj(dict);
            url = string.Empty;
            var msg = string.Empty;

            if (result.success)
            {
                var resultData = result.data as IDictionary<string, string>;
                url = resultData["scwtsdz"];

                if (string.IsNullOrEmpty(url))
                {
                    string recid = resultData["recid"];
                    string syxmbh = resultData["syxmbh"];
                    string wtsmb = resultData["wtsmb"];

                    if (string.IsNullOrEmpty(wtsmb))
                    {
                        wtsmb = syxmbh;
                    }
                    string[] items = new string[] { syxmbh + "|" + recid };
                    var datas = JcService.GetWtdPrintInfos(items, out msg);
                    var data = datas[recid];

                    var g = new ReportPrint.GenerateGuid();
                    var c = g.Get();
                    c.type = ReportPrint.EnumType.Excel;
                    c.openType = ReportPrint.OpenType.PDF;
                    //c.field = reportFile;
                    c.fileindex = "1";
                    c.table = "m_by|s_by|m_d_" + syxmbh + "|s_d_" + syxmbh + "|m_" + syxmbh + "|s_" + syxmbh;
                    c.filename = wtsmb.Replace(",", "|");
                    //c.field = "formid";
                    c.where = "recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc|recid='" + recid + "'|byzbrecid='" + recid + "' order by recid asc";
                    c.signindex = 0;
                    //c.openType = ReportPrint.OpenType.Print ;
                    c.AllowVisitNum = 1;
                    c.libType = ReportPrint.LibType.OpenXmlSdk;
                    c.data = data;
                    c.customtools = "2,|3,|4,|5,|6,";

                    byte[] file = null;
                    if (g.GetFile(c, out file, out msg))
                    {
                        if (file != null)
                        {
                            OSS_CDN oss = new OSS_CDN(Configs.FileOssCdn);
                            var s = oss.UploadFile(Configs.OssCdnCodeWtd, file, "WTS" + recid + ".pdf");
                            if (s.success)
                            {
                                url = s.Url;
                                JcService.SetWtdUploadResult(recid, true, ref s.Url);
                            }
                            else
                            {
                                result.success = false;
                                result.msg = "上传文件失败：" + s.message;
                            }
                        }
                        else
                        {
                            result.success = false;
                            result.msg = "获取委托单模文件为空：" + recid + "," + msg;
                        }
                    }
                    else
                    {
                        result.success = false;
                        result.msg = "获取委托单模板失败：" + recid + "," + msg;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 设置委托单试验状态
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="dataJson"></param>
        /// <param name="key"></param>
        /// <returns></returns>

        [WebMethod(Description = "设置委托单试验状态, dwbh: 单位编号, dataJson: 参数, key: 密钥")]
        public string SetWtdSyZt(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            var records = new List<VTransDownSetWtd>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (string.IsNullOrEmpty(dataJson))
                {
                    msg = "参数不能为空";
                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson, out msg);

                            if (!string.IsNullOrEmpty(msg))
                                msg = "参数转json失败，详细内容：" + msg;
                            else
                            {
                                var result = JcService.SetWtdSyZt(dwbh, data);

                                code = result.success;
                                msg = result.msg;
                            }
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        [WebMethod(Description = "设置委托单的实际送样单位。dwbh:单位编号, dataJson: 参数, key:校验码")]
        public string SetWtdSydw2(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";

            IList<VTransDownSetWtd> records = new List<VTransDownSetWtd>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        var datas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(dataJson, out msg);
                        if (!string.IsNullOrEmpty(msg))
                            msg = "参数转json失败，详细内容：" + msg;
                        else
                        {
                            foreach (var data in datas)
                            {
                                code = JcService.SetWtdStatusXf2(dwbh, data, out msg);
                                VTransDownSetWtd retItem = new VTransDownSetWtd() { wtdwyh = data["wtdwyh"], code = code.ToString().ToLower(), msg = msg };
                                records.Add(retItem);
                            }
                            code = true;
                            msg = "";
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }
            return ret;
        }

        [WebMethod(Description = "获取委托单修改申请记录。dwbh:单位编号, dataJson: 参数, key:校验码")]
        public string GetWtdModifyApply(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            var records = new List<Dictionary<string, object>>();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson, out msg);
                        if (!string.IsNullOrEmpty(msg))
                            msg = "参数转json失败，详细内容：" + msg;
                        else
                        {
                            var result = JcService.GetWtdModifyApply(dwbh, data);
                            code = result.success;
                            msg = result.msg;
                            records = result.data as List<Dictionary<string, object>>;
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", records);
                ret = jss.Serialize(row);
            }
            return ret;
        }

        [WebMethod(Description = "审核委托单修改申请。dwbh:单位编号, dataJson: 参数, key:校验码")]
        public string AuditWtdModifyApply(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";

            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson, out msg);
                        if (!string.IsNullOrEmpty(msg))
                            msg = "参数转json失败，详细内容：" + msg;
                        else
                        {
                            var result = JcService.AuditWtdModifyApply(dwbh, data);
                            string wtdwyh = result.data as string;

                            //重新计算费用
                            if (result.success && !string.IsNullOrEmpty(wtdwyh))
                            {
                                InvokeDllHelper.InvokeCalculate(wtdwyh);
                            }

                            code = result.success;
                            msg = result.msg;
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }
            return ret;
        }

        [WebMethod(Description = "设置委托单审核签发状态。dwbh:单位编号, dataJson: 参数, key:校验码")]
        public string SetWtdSHQF(string dwbh, string dataJson, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";

            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson, out msg);
                        if (!string.IsNullOrEmpty(msg))
                            msg = "参数转json失败，详细内容：" + msg;
                        else
                        {
                            var result = JcService.SetWtdSHQF(dwbh, data);
                            code = result.success;
                            msg = result.msg;
                        }
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);
            }
            return ret;
        }

        #region 萧山协会接口
        /// <summary>
        /// 根据参数获取对应试验项目返回对应需要上传的具体指定字段内容
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "参数dwbh单位编号，syxmbh试验项目编号，status:1表示是标点检测系统；不是1表示其他单位，key密钥s")]
        public string XsxhInterface_UploadField(string dwbh, string syxmbh, string status, string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            ResultParam retObj = new ResultParam();
            try
            {
                dwbh = dwbh.GetSafeRequest();

                if (JcService.GetJcjgmy(dwbh, out msg))
                {
                    string jcjgmy = msg;
                    string str = dwbh;
                    str = str.EncodeDesJk(jcjgmy);
                    if (key != str)
                        msg = "数据校验错误，请正确配置数据上传密钥";
                    else
                    {
                        msg = "";
                        retObj = JcService.XsxhInterface_UploadField(syxmbh, status);
                        code = retObj.success;
                    }
                }
                else
                    msg = "单位编号无效";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                row.Add("records", retObj.data);
                ret = jss.Serialize(row);
            }

            return ret;
        }

        [WebMethod(Description = "上传报告。dwbh:单位编号,wtdwyh:委托单唯一号,bgbh:报告编号,syr:试验人,shr:审核人,qfr:签发人,syrq:试验日期,qfrq:签发日期, jcjg:检测结果(0不下结论，1合格，2不合格)，jcjgms:检测结果描述,mdatajson:报告数据内容主表,sdatajson:报告数据内容从表,pdfjson:pdf文件内容,datajson:协会数据包,key:密钥")]
        public string UpReport2(string dwbh, string wtdwyh, string bgbh, string syr, string shr, string qfr,
            string syrq, string qfrq, string jcjg, string jcjgms, string mdatajson, string sdatajson, string pdfjson, string datajson,string key)
        {
            string ret = "";
            bool code = false;
            string msg = "";
            try
            {
                dwbh = dwbh.GetSafeRequest();
                wtdwyh = wtdwyh.GetSafeRequest();
                bgbh = bgbh.GetSafeRequest();
                shr = syr.GetSafeRequest();
                qfr = qfr.GetSafeRequest();
                jcjg = jcjg.GetSafeRequest();
                jcjgms = jcjgms.GetSafeRequest();

                DateTime dtsyrq, dtqfrq;
                dtsyrq = syrq.GetSafeDate(DateTime.MinValue);
                dtqfrq = qfrq.GetSafeDate(DateTime.MinValue);
                if (dtsyrq.Year == DateTime.MinValue.Year)
                {
                    msg = "试验日期无效";
                }
                else if (dtqfrq.Year == DateTime.MinValue.Year)
                {
                    msg = "签发日期无效";

                }
                else
                {
                    if (JcService.GetJcjgmy(dwbh, out msg))
                    {
                        string jcjgmy = msg;
                        string str = dwbh;
                        str = str.EncodeDesJk(jcjgmy);
                        if (key != str)
                            msg = "数据校验错误，请正确配置数据上传密钥";
                        else
                        {
                            code = JcService.UpReport(dwbh, wtdwyh, bgbh, syr, shr, qfr,
                                dtsyrq, dtqfrq, jcjg.GetSafeInt(), jcjgms, mdatajson, sdatajson, pdfjson, true, false, datajson, out msg);
                        }
                    }
                    else
                        msg = "单位编号无效";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                if (!code)
                {
                    SysLog4.WriteError("上传报告数据失败，单位编号：" + dwbh + "，委托单唯一号：" + wtdwyh + "，原因：" + msg);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code.ToString().ToLower());
                row.Add("msg", msg);
                ret = jss.Serialize(row);

            }
            return ret;
        }
        #endregion     
    }
}
