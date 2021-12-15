using BD.IDataInputBll;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.IBll;
using BD.Jcbg.Web;
using BD.Jcbg.Web.Func;
using BD.Jcbg.Web.Remote;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Collections.Specialized;
using Bd.jcbg.Common;

namespace BD.Jcbg.Web.Controllers
{
    public class WzwgryFunController : Controller
    {
        #region 服务
        private ISystemService _systemService = null;
        private ISystemService SystemService
        {
            get
            {
                try
                {
                    if (_systemService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _systemService;
            }
        }
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                try
                {
                    if (_commonService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _commonService;
            }
        }
        private BD.Log.IBll.ILogService _logService = null;
        private BD.Log.IBll.ILogService LogService
        {
            get
            {
                if (_logService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _logService = webApplicationContext.GetObject("LogService") as BD.Log.IBll.ILogService;
                }
                return _logService;
            }
        }
        private BD.WorkFlow.Bll.RemoteUserService _remoteUserService = null;
        private BD.WorkFlow.Bll.RemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as BD.WorkFlow.Bll.RemoteUserService;
                }
                return _remoteUserService;
            }
        }

        public IWgryKqjService _wgrykqjService = null;
        public IWgryKqjService WgryKqjService
        {
            get
            {
                if (_wgrykqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _wgrykqjService = webApplicationContext.GetObject("WgryKqjService") as IWgryKqjService;
                }
                return _wgrykqjService;
            }
        }

        private IYcbaService _ycbaService = null;
        private IYcbaService YcbaService
        {
            get
            {
                if (_ycbaService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _ycbaService = webApplicationContext.GetObject("YcbaService") as IYcbaService;
                }
                return _ycbaService;
            }
        }
        private IDataInputService _dataInputService = null;
        private IDataInputService DataInputService
        {
            get
            {
                if (_dataInputService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataInputService = webApplicationContext.GetObject("DataInputService") as IDataInputService;
                }
                return _dataInputService;
            }
        }

        private ISelfService _selfService = null;
        private ISelfService SelfService
        {
            get
            {
                try
                {
                    if (_selfService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _selfService = webApplicationContext.GetObject("SelfService") as ISelfService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _selfService;
            }
        }
        #endregion 

        #region 页面
        [Authorize]
        public ActionResult ifr()
        {
            ViewBag.url = Request["url"].GetSafeString();
            return View();
        }
        [Authorize]
        public ActionResult sfzqrcode()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            ViewData.Add("jdzch", jdzch);
            ViewData.Add("sfzhm", sfzhm);
            return View();
        }
        [Authorize]
        public ActionResult sfzWxqrcode()
        {
            string ryxm = Request["ryxm"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            ViewData.Add("ryxm", ryxm);
            ViewData.Add("sfzhm", sfzhm);
            return View();
        }
        [Authorize]
        public ActionResult uploadFile()
        {
            return View();
        }

        [Authorize]
        public ActionResult saveMonthPay()
        {
            string jdzch = CurrentUser.Jdzch;
            DateTime dt = DateTime.Now;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            string url = "/kqj/setmonthpay_gc?jdzch=" + HttpUtility.UrlEncode(jdzch, System.Text.Encoding.UTF8) + "&year=" + HttpUtility.UrlEncode(year, System.Text.Encoding.UTF8) +
                        "&month=" + HttpUtility.UrlEncode(month, System.Text.Encoding.UTF8);

            return Content("<script>window.open('" + Url.Content(url) + "', '_self')</script>");
        }

        /// <summary>
        /// 地图标注
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Map()
        {
            ViewBag.pos = Request["pos"].GetSafeString();
            ViewBag.title = Request["title"].GetSafeString();
            ViewBag.gcbh = Request["jdzch"].GetSafeString();
            return View();
        }
        [Authorize]
        public ActionResult MapLine()
        {

            ViewBag.jdzch = Request["jdzch"].GetSafeString(); ;
            ViewBag.title = Request["title"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 根据上层平台更新GCBH_YC字段
        /// </summary>
        public ActionResult UpGCBH_YC()
        {
            ViewBag.callid = Request["callid"].GetSafeString();
            ViewBag.version = Request["version"].GetSafeString();
            ViewBag.url = Request["url"].GetSafeString();
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }
        #endregion

        #region 方法
        [Authorize]
        public void test()
        {
            string msg = "";
            try
            {
                return;
                string sql = "select jdzch,gcmc,sfzhm,ryxm from i_m_wgry  where jdzch like '%LG%'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    string jdzch = list[i]["jdzch"];
                    string gcmc = list[i]["gcmc"];
                    string sfzhm = list[i]["sfzhm"];
                    string ryxm = list[i]["ryxm"];

                    string text = EncryUtil.Encode(jdzch + "_" + sfzhm);

                    byte[] barimage = Barcode.GetBarcode2(text, 200, 200);

                    sql = "select * from I_S_GC_QRCode where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                    IList<IDictionary<string, string>> list2 = CommonService.GetDataTable(sql);
                    if (list2.Count > 0)
                    {
                       
                    }
                    else
                    {
                        IList<IDataParameter> sqlparams = new List<IDataParameter>();
                        IDataParameter sqlparam = new SqlParameter("@pic", barimage);
                        sqlparams.Add(sqlparam);
                        sql = "insert into I_S_GC_QRCode (jdzch,gcmc,sfzhm,ryxm,HT_QRCode) select jdzch ,gcmc,sfzhm,ryxm,@pic from i_m_wgry where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";

                       // CommonService.ExecTrans(sql, sqlparams, out msg);
                    }      
                }

            }
            catch(Exception e) 
            {
                msg = e.Message;
            }
        }
        [Authorize]
        public JsonResult delyzdj()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string sql = "select * from I_S_GC_YZ where recid='" + recid + "'";
                IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql);
                if(dt.Count>0)
                {
                    string sfzhm=dt[0]["sfzhm"];
                    string jdzch=dt[0]["jdzch"];
                    string logyear=dt[0]["yzyear"];
                    string logmonth=dt[0]["yzmonth"];
                    string yzpay = dt[0]["yzpay"];
                    sql = "update KqjUserMonthPay set yzpay=(isnull(yzpay,0)-" + yzpay + ") where jdzch='" + jdzch + "' and userid='" + sfzhm + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' ";
                    code=CommonService.Execsql(sql);
                    if(code)
                    {
                        sql = "delete from I_S_GC_YZ where recid='" + recid + "'";
                        CommonService.Execsql(sql);
                    }

                }
            }
            catch(Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg }, JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// 数字转大写
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult numTostring()
        {
            bool code = false;
            string msg = "";
            try
            {
                double num = Request["num"].GetSafeDouble();
                msg = numt2string.num2String(num);
                code = true;
            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg},JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存工程人员二维码
        /// </summary>
        [Authorize]
        public void SaveQRCode()
        {
            string sfzhm = Request["sfzhm"];
            string jdzch = Request["jdzch"];
            bool code = false;
            string msg="";
            try
            {
                string sign64 = "";
                string text = EncryUtil.Encode(jdzch + "_" + sfzhm);

                byte[] barimage = Barcode.GetBarcode2(text, 200, 200);

                //System.IO.File.WriteAllBytes("d:\\b2.jpg", barimage);

                sign64 = barimage.EncodeBase64();

                string sql = "select * from I_S_GC_QRCode where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
                if(list.Count>0)
                {
                    code = true;
                }
                else
                {
                    IList<IDataParameter> sqlparams = new List<IDataParameter>();
                    IDataParameter sqlparam = new SqlParameter("@pic", barimage);
                    sqlparams.Add(sqlparam);
                    IList<string> sqls = new List<string>();
                    sql = "insert into I_S_GC_QRCode (jdzch,gcmc,sfzhm,ryxm,HT_QRCode) select jdzch ,gcmc,sfzhm,ryxm,@pic from i_m_wgry where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                    sqls.Add(sql);
                    code = CommonService.ExecTrans(sql, sqlparams, out msg);
                }                       
            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        [Authorize]
        /// <summary>
        /// 获取工程人员二维码
        /// </summary>
        public void ShowQRCode()
        {
            byte[] ret = null;
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                byte[] conten=null ;
                string sql = "select ht_qrcode from I_S_GC_QRCode where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                IList<IDictionary<string, object>> data = CommonService.GetDataTable2(sql);
                if (data.Count > 0)
                {
                    conten = (byte[])data[0]["ht_qrcode"];                   
                }



                string filename = "sign.jpg";
                string mime = MimeMapping.GetMimeMapping(filename);
                Response.Clear();
                Response.ContentType = mime;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                //Response.AddHeader("Content-Length", filesize.ToString());
                Response.BinaryWrite(conten);
                Response.Flush();
                Response.End();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 绑定微信公众号二维码
        /// </summary>
        [Authorize]
        public void ShowWXQRCode()
        {
            byte[] ret = null;
            try
            {
                string ryxm = Request["ryxm"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                string text = EncryUtil.Encode(ryxm + "_" + sfzhm).EncodeBase64();

                //http请求
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxca0d1617f711ddc4&secret=3ebd8fc006147b4ab13d5a9a25c59693";
                string Access_token = getAccess_token(geturl); //"10_-BtRwhOV-m6svzvzKc8m_ziWpwL7JO8kxIAegzcgEzGrkti3n3BbdpO2PkSwRq0IJxrIq3bMkPxHqPDPDOKYcnQ9k0nUa88CfaCc-3Xi6AuTvOsaIZ92dJ-tv4QrsBSB9YpHzFGq4TZLJAjNHEVaAIAROF";


                string wgryinurl = "http://zjwx.jtjsypt.com/User/Index?content=" + text;
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=" + HttpUtility.UrlEncode(wgryinurl) + "&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=http%3a%2f%2fzjwx.jtjsypt.com%2fUser%2fSignIn&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                string msg = LongToShortUrl(url, Access_token);

                byte[] barimage = Barcode.GetBarcode2(msg, 200, 200);

                string filename = "sign.jpg";
                string mime = MimeMapping.GetMimeMapping(filename);
                Response.Clear();
                Response.ContentType = mime;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                //Response.AddHeader("Content-Length", filesize.ToString());
                Response.BinaryWrite(barimage);
                Response.Flush();
                Response.End();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }
        /// <summary>
        /// 保存绑定微信公众号二维码-人员
        /// </summary>
        [Authorize]
        public void SaveBindQRcode()
        {
            string msg = "";
            bool code = false;
            try
            {
                string ryxm = Request["ryxm"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                string text = EncryUtil.Encode(ryxm + "_" + sfzhm).EncodeBase64();

                //http请求
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxca0d1617f711ddc4&secret=3ebd8fc006147b4ab13d5a9a25c59693";
                string Access_token = getAccess_token(geturl); //"10_-BtRwhOV-m6svzvzKc8m_ziWpwL7JO8kxIAegzcgEzGrkti3n3BbdpO2PkSwRq0IJxrIq3bMkPxHqPDPDOKYcnQ9k0nUa88CfaCc-3Xi6AuTvOsaIZ92dJ-tv4QrsBSB9YpHzFGq4TZLJAjNHEVaAIAROF";


                string wgryinurl = "http://zjwx.jtjsypt.com/User/Index?content=" + text;
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=" + HttpUtility.UrlEncode(wgryinurl) + "&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=http%3a%2f%2fzjwx.jtjsypt.com%2fUser%2fSignIn&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                string ShortUrl = LongToShortUrl(url, Access_token);

                byte[] barimage = Barcode.GetBarcode2(ShortUrl, 200, 200);
                string content=barimage.EncodeBase64();

                string sql = "update i_m_ry_info set BindQRCode='" + content + "'  where sfzhm='" + sfzhm + "'";
                code=CommonService.Execsql(sql);

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 工地考勤的二维码-管理人员端下载
        /// </summary>
        [Authorize]
        public void SaveGCQRCode()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            if(jdzch=="")
                jdzch = CurrentUser.Jdzch;
            string guid = Guid.NewGuid().ToString("N");
            bool code = false;
            string msg = "";
            try
            {
                //http请求
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxca0d1617f711ddc4&secret=3ebd8fc006147b4ab13d5a9a25c59693";
                string Access_token = getAccess_token(geturl); //"10_-BtRwhOV-m6svzvzKc8m_ziWpwL7JO8kxIAegzcgEzGrkti3n3BbdpO2PkSwRq0IJxrIq3bMkPxHqPDPDOKYcnQ9k0nUa88CfaCc-3Xi6AuTvOsaIZ92dJ-tv4QrsBSB9YpHzFGq4TZLJAjNHEVaAIAROF";

                string wgryinurl = "http://zjwx.jtjsypt.com/User/SignIn?jdzch=" + jdzch + "&guid=" + guid;
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=" + HttpUtility.UrlEncode(wgryinurl)+ "&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxca0d1617f711ddc4&redirect_uri=http%3a%2f%2fzjwx.jtjsypt.com%2fUser%2fSignIn&response_type=code&scope=snsapi_base&state=123&connect_redirect=1#wechat_redirect";
                msg = LongToShortUrl(url, Access_token);

                byte[] barimage = Barcode.GetBarcode2(msg, 300, 300);

                string filename = "sign.jpg";
                string mime = MimeMapping.GetMimeMapping(filename);
                Response.Clear();
                Response.ContentType = mime;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                Response.BinaryWrite(barimage);
                Response.Flush();
                Response.End();
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            finally
            {
                //Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        
        [Authorize]
        public void GetWgryHTFJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sfzhm = Request["sfzhm"];
                string jdzch = Request["jdzch"];
                string sql = "select top 1 recid from i_s_gc_ht where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "' order by lrsj desc";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if(dt.Count>0)
                {
                    msg = dt[0]["recid"];
                }
                else
                {
                    code = false;
                    msg = "没有已上传的合同";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 下发单人模板
        /// </summary>
        [Authorize]
        public void downIris()
        {
            string sfzhm = Request["sfzhm"];
            string msg = "";
            bool code = true;
            try
            {
                IList<IDictionary<string, string>> kqjs = WgryKqjService.GetRyGcKqj(sfzhm);
                IList<string> sqls = new List<string>();
                foreach (IDictionary<string, string> row in kqjs)
                {
                    string serial = "";
                    if (row.TryGetValue("kqjbh", out serial))
                    {
                        string sql = "insert into KqjDeviceCommand ([Serial],[Command] ,[UserId],[RealName] ,[UserStation] ,[IrisModule]) Select '" + serial + "',21,'" + sfzhm + "',ryxm,'',HM from i_m_wgry where sfzhm='" + sfzhm + "' and HM!='' and jdzch='"+CurrentUser.Jdzch+"'";
                        sqls.Add(sql);
                    }
                }
                code=CommonService.ExecSqls(sqls);
                
            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        
        [Authorize]
        public void GetQYXX()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string qybh = Request["qybh"];


                string where = " where 1=1 ";
                where += " and bdqybh='" + qybh + "'";
                string sql = "select gcmc,gclxbh,szsf,szcs,szxq,qymc,gw,gwry from View_GC_QY ";
                sql += where;
               
  
                dt = CommonService.GetDataTable(sql);
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }
        /// <summary>
        /// 根据企业编号获取相关工程信息
        /// </summary>
        [Authorize]
        public void GetGClistByqybh()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string qybh = Request["qybh"];


                string where = " where 1=1 ";
                where += " and bdqybh='" + qybh + "'";
                string sql = "select gcmc,gclxbh,szsf,szcs,szxq,qymc,gw,gwry from View_GC_QY ";
                sql += where;
               
  
                dt = CommonService.GetDataTable(sql);
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 批量打印安全教育登记表
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult WGRYReportPrintMore()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            string recids = Request["recids"].GetSafeString();
            string sfzhms = Request["sfzhms"].GetSafeString();
            string reportfile = Request["reportfile"].GetSafeString();
            string tablename = Request["tablename"].GetSafeString();
            try
            {
                StringBuilder sbGuids = new StringBuilder();
                if (recids != "")
                {
                    recids = recids.Trim(',');
                    string[] recidlist = recids.Split(',');
                    for (int j = 0; j < recidlist.Length; j++)
                    {
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.fileindex = "0";
                        c.filename = reportfile;
                        c.table = tablename;
                        c.where = "recid=" + recidlist[j]+"";
                        c.openType = ReportPrint.OpenType.PDFPrint;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;                     
                        c.signindex = 0;
                        c.AllowVisitNum = 1;

                        var guid = g.Add(c);
                        sbGuids.Append(guid + "|");              
                    }
                    string strGuid = sbGuids.ToString().Trim(new char[] { '|' });
                    string url = "/ReportPrint/BatchPrinting?id=" + strGuid + "&c=1";
                    return new RedirectResult(url);
                }

            }
            catch (Exception e)
            {

            }
            return null;
        }
        [Authorize]
        public JsonResult getwgrybgmc()
        {
            string msg="";
            bool code=false;
            string reportname = "";
            try
            {
                string gcbh=Request["gcbh"].GetSafeString();
                string bglx=Request["bglx"].GetSafeString();
                string sql="select * from H_GCBG where bglx='"+bglx+"' and gcbh='"+gcbh+"'";
                IList<IDictionary<string, string>> gcbglist = CommonService.GetDataTable(sql);
                if (gcbglist.Count!=0)
                {
                    reportname = gcbglist[0]["reportname"];
                    code = true;
                }
                else
                {
                    sql = "select * from h_wgrybg where bglx='" + bglx + "' and szxq in (select szxq from i_m_gc where gcbh='" + gcbh + "')";
                    IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                    if (list.Count > 0)
                    {
                        reportname = list[0]["reportname"];
                        code = true;
                    }
                    else
                    {
                        reportname = "劳动合同";
                        code = true;
                    }
                }
              
            }
            catch(Exception e)
            {
                code = false;
                msg = e.Message;
            }
            IDictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("code", code?"0":"1");
            ret.Add("msg", msg);
            ret.Add("reportname", reportname);
            return Json(ret); 
        }

        /// <summary>
        /// 获取地图默认标注点
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetDefaultMap()
        {
            string title = "";
            string pos = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string sql = "select szcs,szxq from i_m_gc where gcbh='" + gcbh + "'";
        
                IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
                if(list.Count>0)
                {
                    title = list[0]["szxq"];
                    if(title=="")
                        title = list[0]["szcs"];
                    if (title == "")
                        title = "杭州市";
                }
                else
                {
                    title = GlobalVariable.DefaultMapTitle;
                    pos = GlobalVariable.DefaultMapPos;
                }
            }
            catch(Exception e)
            {
                title = GlobalVariable.DefaultMapTitle;
                pos = GlobalVariable.DefaultMapPos;
            }
            return Json(new { title = title, pos = pos });
        }
        /// <summary>
        /// 获取工程坐标
        /// </summary>
        [Authorize]
        public void GetGcbz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();

                string sql = "select jdzch,lon,lat from I_M_GC_JWD where jdzch='" + jdzch + "'";
                IList<IDictionary<string, string>> sqllist = CommonService.GetDataTable(sql);
                IList<string> sqls = new List<string>();
                if (sqllist.Count != 0)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    msg = jss.Serialize(sqllist);

                }
                else
                {
                    code = false;
                    msg = "没有设置坐标";
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
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 工程标注
        /// </summary>
        [Authorize]
        public void SetGcbz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string pos = Request["pos"].GetSafeString();
                string lon = "";
                string lat = "";
                if (pos != "")
                {
                    string[] list = pos.Split(',');
                    if (list.Count() == 2)
                    {
                        lon = list[0];
                        lat = list[1];
                    }
                    else
                    {
                        code = false;
                        msg = "设置经纬度失败";
                    }
                }
                if (msg == "")
                {
                    string sql = "select * from I_M_GC_JWD where jdzch='" + jdzch + "'";
                    IList<IDictionary<string, string>> sqllist = CommonService.GetDataTable(sql);
                    IList<string> sqls = new List<string>();
                    sqls.Add("update i_m_gc set gczb='" + pos + "' where gcbh='" + jdzch + "'");
                    if (sqllist.Count != 0)
                    {
                        sqls.Add("update i_m_gc_jwd set lon='" + lon + "',lat='" + lat + "' where jdzch='" + jdzch + "'");

                    }
                    else
                    {
                        sqls.Add("insert into i_m_gc_jwd (jdzch,lon,lat,orderby) values('"
                            + jdzch + "','"
                            + lon + "','"
                            + lat + "',1)");
                    }
                    code = CommonService.ExecTrans(sqls);
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
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        /// <summary>
        /// 设置考勤二维码坐标
        /// </summary>
        public void SetQRcodezb()
        {
            bool code = false;
            string msg = "";
            try
            {
                string username = Request["username"].GetSafeString();
                string password = Request["password"].GetSafeString();
                string jdzch = Request["jdzch"].GetSafeString();
                string xlh = Request["guid"].GetSafeString() ;
                double lon = Request["lon"].GetSafeDouble();
                double lat = Request["lat"].GetSafeDouble();
                string usercode = "";
                string realname = "";
                code = UserService.CheckLogin(username, password, out usercode, out realname, out msg);
                if(code)
                {
                    string t_sql = "select * from I_M_LZZGY_ZH where zh='" + username + "'";
                    IList<IDictionary<string, string>> lzylist = CommonService.GetDataTable(t_sql);
                    if(lzylist.Count>0)
                    {
                        if(lzylist[0]["jdzch"]!=jdzch)
                        {
                            code = false;
                            msg = "账号没有权限定位该工程";
                        }
                        else
                        {
                            string qybh = lzylist[0]["qybh"];
                            string sql = "select * from i_m_qrcode where xlh='" + xlh + "'";
                            IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                            if (list.Count > 0)
                            {
                                if(list[0]["gcbh"]!=jdzch)
                                {
                                    code = false;
                                    msg = "该二维码工程不符合";
                                }
                                else
                                {
                                    sql = "update I_M_QRCODE set lon='" + lon + "',lat='" + lat + "' where xlh='" + xlh + "'";
                                    code = CommonService.Execsql(sql);
                                }
                              
                            }
                            else
                            {
                                sql = "INSERT INTO I_M_QRCODE ([XLH],[Lon],[Lat],[gcbh],[qybh]) values('" + xlh + "','"
                                    + lon + "','"
                                    + lat + "','"
                                    + jdzch + "','"
                                    + qybh + "')";
                                code = CommonService.Execsql(sql);
                            }
                            if (code)
                            {
                                msg = "定位成功";
                            }
                            else
                                msg = "定位失败";
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "账号没有权限定位";
                    }                
                }
                else
                {
                    msg = "用户名密码错误";
                }
            }
            catch(Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 获取班组长及下面小班主的人员
        /// </summary>
        /// <returns></returns>

        public JsonResult getbzfzrlist()
        {
            string fzrstr = "";
            string msg = "";
            bool code=false;
            List<string> fzrlist = new List<string>();
            try
            {
                string bzfzrxm = Request["bzfzrxm"].GetSafeString();
                string jdzch=CurrentUser.Jdzch;
                if (bzfzrxm != "")
                {
                    List<string> fzr_sfzlist = new List<string>();
                    string sql = "select ryxm,sfzhm from i_m_wgry where ryxm='" + bzfzrxm + "' and sfbzfzr='是' and jdzch='" + jdzch + "'";
                    IList<IDictionary<string, string>> rylist = CommonService.GetDataTable(sql);
                    if (rylist.Count == 0)
                    {
                        code = false;
                        msg = "没有该人员信息";
                    }
                    else
                    {
                        fzrlist.Add(rylist[0]["sfzhm"]);
                        SelfService.getBzfzrlist(rylist[0]["sfzhm"], jdzch, ref fzrlist);
                        for (int i = 0; i < fzrlist.Count; i++)
                        {
                            fzrstr += fzrlist[i] + ",";
                        }
                        fzrstr = fzrstr.FormatSQLInStr();
                        code = true;
                        msg = jdzch;
                    }
                }
                else
                {
                    code = true;
                    msg = jdzch;
                }              
            }
            catch(Exception e)
            { 
                code=false;
                msg=e.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, list = fzrstr });
        }
        /// <summary>
        /// 获取项目（劳资员）账号
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult getxmzh()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = CurrentUser.Jdzch;
                string username=CurrentUser.UserName;
                msg = username;
            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg});
        }

        /// <summary>
        /// 务工人员保存pdf文件到本地
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult SaveWGRYReportFDF()
        {
            bool code = true;
            string msg = "";
            try
            {
                string url = "";
                string reportFile = Request["reportfile"].GetSafeString();
                string tablename = Request["tablename"].GetSafeString();
                string where = Request["where"].GetSafeString();
                int type = Request["type"].GetSafeInt();
                string opentype = Request["opentype"].GetSafeString();
                string rguid=Request["rguid"].GetSafeString();

                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                // c.type = ReportPrint.EnumType.Excel;
                if (type == 1)
                    c.type = (ReportPrint.EnumType)type;
                else
                    c.type = ReportPrint.EnumType.Word;
                //c.field = reportFile;
                c.fileindex = "0";

                c.filename = reportFile;
                c.table = tablename;
                c.where = where;

                c.openType = ReportPrint.OpenType.PDF;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                c.signindex = 0;
                c.customtools = "1,|2,|3,|4,|5,|6,|12,";
                c.AllowVisitNum = 1;

                //var guid = g.Add(c);
                byte[] fileBytes;
                string err="";
                code = g.GetFile(c, out fileBytes, out err);
                if(code)
                {
                    //1.获取模块的完整路径。  
                    string path1 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                    //2.获取和设置当前目录(该进程从中启动的目录)的完全限定目录  
                    string path2 = System.Environment.CurrentDirectory;

                    //3.获取应用程序的当前工作目录  
                    string path3 = System.IO.Directory.GetCurrentDirectory();

                    //4.获取程序的基目录  
                    string path4 = System.AppDomain.CurrentDomain.BaseDirectory;

                    //5.获取和设置包括该应用程序的目录的名称  
                    string path5 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

 
                    SaveFileFun.saveExcel(rguid + ".pdf", fileBytes, "wgrypdf");
                }
                else
                {
                    
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return Json(new { code = code ? "0" : "1", msg = msg});
        }


        /// <summary>
        /// 校验企业名称或者五证合一码是否存在，存在，返回false，定位到重置密码页面(劳务公司)
        /// </summary>
        /// <param name="qymc"></param>
        /// <param name="qydm"></param>
        /// <returns>存在（或异常），返回1；不存在，返回0</returns>
        public JsonResult CheckQyValid(string qymc, string qydm)
        {
            bool code = false;
            string msg = "";
            bool toreset = false;
            try
            {
                qymc = qymc.GetSafeRequest().Trim();
                qydm = qydm.GetSafeRequest().Trim().Replace("-", "");// 从00000000-0换算成000000000
                if (qydm.Length == 0)
                {
                    code = true;
                }
                else if (qydm.Length == 9 || qydm.Length == 18)
                {
                    string where = " lwgsmc='" + qymc + "' or zzjgdm='" + qydm + "' ";
                    // 老的组织机构代码
                    if (qydm.Length == 9)
                    {
                        string qydmlong = qydm.Insert(8, "-");// 从000000000换算成00000000-0
                        where += " or zzjgdm='" + qydmlong + "' or zzjgdm like '________" + qydm + "_' ";
                    }
                    // 新的五证合一码
                    else if (qydm.Length == 18)
                    {
                        string qydmshort = qydm.Substring(8, 9);    // 组织机构代码
                        string qydmlong = qydmshort.Insert(8, "-"); // 组织机构代码带'-'
                        where += " or zzjgdm='" + qydmshort + "' or zzjgdm='" + qydmlong + "' ";
                    }
                    string sql = "select lwgsbh, lwgsmc, zzjgdm from i_m_lwgs where " + where;
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        code = true;
                    }
                    else
                    {
                        code = false;
                        toreset = true;
                        msg = dt[0]["lwgsmc"];
                    }
                }
                // 无效的输入
                else
                {
                    msg = "组织机构代码或社会统一信用代码无效";
                    toreset = false;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", toreset = toreset ? "0" : "1", msg = msg });
        }

        #region 根据上层平台关联GCBH_YC字段
        /// <summary>
        /// 根据上层平台关联GCBH_YC字段
        /// </summary>
        public JsonResult UpdateGCBH_YC()
        {

            string callid=Request["callid"].GetSafeString();
            string version=Request["version"].GetSafeString();
            string key=Request["key"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            bool code = false;
            string msg = "";
            return Json(new { code = code ? "0" : "1", msg = msg });

            try
            {
                IList<SysYcdyStation> stations = YcbaService.GetStations();
                stations = stations.Where(e => e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase)).ToList();
                SysYcdyStation station = stations[0];
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                IDictionary<string, object> obj = jsonSerializer.Deserialize<IDictionary<string, object>>(key);
                IList<IDictionary<string, object>> sqls = new List<IDictionary<string, object>>();

                SysYcdyApiObject apiObj = new SysYcdyApiObject(
                    YcbaService.GetUrl(callid, version),
                    YcbaService.GetTables(callid, version),
                    YcbaService.GetParams(callid, version),
                    YcbaService.GetTableRelations(callid, version),
                    YcbaService.GetFields(callid, version),
                    CommonService, DataInputService, YcbaService, station.RootUrl, callid + "," + version);

                if (!apiObj.IsValid())
                    msg = "无效的调用ID或者版本";
                else
                {
                    SysYcdyTable rootTable = null;
                    SysYcdyTableRelation rootRelation = null;
                    code = apiObj.GetRootTable(out rootTable, out rootRelation);
                    if (!code)
                        msg = "找不到根表定义";
                    else
                    {
                        bool needUpdate = apiObj.NeedUpdate(obj, out msg);
                        // 没错误并且需要更新
                        if (msg == "" && needUpdate)
                        {

                            IList<IDictionary<string, object>> rowsDatas = new List<IDictionary<string, object>>();
                            foreach (string property in obj.Keys)
                            {
                                object propertyvalue = obj[property];
                                IList<SysYcdyTable> findTables = apiObj.GetTables(property);
                                // json对象在数据库中没定义，抛弃
                                if (findTables.Count == 0)
                                    continue;

                                // 非三重表结构                                
                                if (!apiObj.IsDetailStruct(property))
                                {
                                    SysYcdyTable findTable = findTables[0];
                                    SysYcdyTableRelation findRelation = apiObj.GetRelation(findTable.LocalTableName);
                                    if (findRelation == null)
                                    {
                                        msg = findTable.LocalTableName + "找不到relation记录";
                                        SysLog4.WriteError(msg);
                                        break;
                                    }
                                    // 主表单条数据
                                    if (!findTable.IsJsonArray)
                                    {
                                        IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                        rowsData.Add("findtable", findTable);
                                        rowsData.Add("findrelation", findRelation);
                                        rowsData.Add("rootdata", propertyvalue);
                                        rowsData.Add("tableinfo", property);
                                        rowsDatas.Add(rowsData);
                                    }
                                    // 从表多条记录
                                    else
                                    {
                                        object[] subDatas = propertyvalue as object[];
                                        foreach (object subrow in subDatas)
                                        {
                                            IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                            rowsData.Add("findtable", findTable);
                                            rowsData.Add("findrelation", findRelation);
                                            rowsData.Add("rootdata", subrow);
                                            rowsData.Add("tableinfo", property);
                                            rowsDatas.Add(rowsData);
                                        }

                                    }
                                }
                                // 三重结构
                                else
                                {

                                    object[] rootSubDatas = propertyvalue as object[];
                                    foreach (object tmpData in rootSubDatas)
                                    {
                                        bool tmpCode = true;
                                        IDictionary<string, object> rowData = tmpData as IDictionary<string, object>;
                                        foreach (string subproperty in rowData.Keys)
                                        {
                                            IList<SysYcdyTable> detailTables = findTables.Where(e => e.RemoteTable.EndsWith(subproperty)).ToList();
                                            if (detailTables.Count == 0)
                                                continue;
                                            SysYcdyTable findTable = detailTables[0];
                                            SysYcdyTableRelation findRelation = apiObj.GetRelation(findTable.LocalTableName);
                                            if (findRelation == null)
                                            {
                                                msg = findTable.LocalTableName + "找不到relation记录";
                                                SysLog4.WriteError(msg);
                                                tmpCode = false;
                                                break;
                                            }
                                            if (!findTable.IsJsonArray)
                                            {
                                                IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                                rowsData.Add("findtable", findTable);
                                                rowsData.Add("findrelation", findRelation);
                                                rowsData.Add("rootdata", rowData[subproperty] as IDictionary<string, object>);
                                                rowsData.Add("tableinfo", property + "." + subproperty);
                                                rowsDatas.Add(rowsData);
                                            }
                                            else
                                            {
                                                object[] subDatas = rowData[subproperty] as object[];

                                                foreach (object tempData in subDatas)
                                                {
                                                    IDictionary<string, object> subrow = tempData as IDictionary<string, object>;
                                                    IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                                    rowsData.Add("findtable", findTable);
                                                    rowsData.Add("findrelation", findRelation);
                                                    rowsData.Add("rootdata", subrow);
                                                    rowsData.Add("tableinfo", property + "." + subproperty);
                                                    rowsDatas.Add(rowsData);
                                                }
                                            }
                                        }
                                        if (!tmpCode)
                                            break;
                                    }
                                }
                            }
                            string gcbh_yc ="";
                            IList<string> up_sqls = new List<string>();
                            foreach (IDictionary<string, object> rowdata in rowsDatas)
                            {
                                SysYcdyTable findTable = rowdata["findtable"] as SysYcdyTable;
                                SysYcdyTableRelation findRelation = rowdata["findrelation"] as SysYcdyTableRelation;
                                IDictionary<string, object> data = rowdata["rootdata"] as IDictionary<string, object>;
                                string tableinfo = rowdata["tableinfo"] as string;
                                bool isHave = apiObj.IsUpdate(data, findTable, findRelation, out msg);
                                if (msg != "")
                                    break;

                                string sql = "";
                                IList<VSqlParam> sqlParams = null;
                                IList<VDataFileItem> sqlFiles = null;
                                if (!isHave)
                                {
                                    string str_yc = "";
                                    if(tableinfo=="工程信息")
                                    {
                                        //gcbh_yc = data["gcbh"] as string;
                                        //string wgptbh = data["wgptbh"] as string;
                                        //sql = "update i_m_gc set gcbh_yc='" + gcbh_yc + "',wgptbh='" + wgptbh + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_sgdw set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_jldw set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_jsdw set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_kcdw set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_sjdw set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_sgry set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_jlry set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_jsry set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_kcry set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                        //sql = "update i_s_gc_sjry set gcbh='" + gcbh_yc + "' where gcbh='" + gcbh + "'";
                                        //up_sqls.Add(sql);
                                    }
                                    
                                }
                                else
                                {
                                    msg = "本地没有该信息，无法关联";
                                    break;
                                }
                              
                            }

                            if (msg == "")
                                CommonService.ExecSqls(up_sqls);
                                //code = YcbaService.SaveData(sqls, out msg);
                        }


                    }
                }
                code = msg == "";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        private bool GetUpdateSql(IDictionary<string, object> allDatas, IDictionary<string, object> data, SysYcdyTable table, SysYcdyApiObject apiObj, string tableinfo, string GCBH,
           out string sql, out IList<VSqlParam> sqlParams, out IList<VDataFileItem> files,  out string msg)
        {
            bool code = false;
            msg = "";
            sql = "";
            sqlParams = new List<VSqlParam>();
            files = new List<VDataFileItem>();
            try
            {
                StringBuilder fieldStr = new StringBuilder();
                IList<SysYcdyField> fields = apiObj.GetSaveFields(table.LocalTableName, true);
                IList<KeyValuePair<string, object>> valueFields = new List<KeyValuePair<string, object>>();
                foreach (SysYcdyField field in fields)
                {
                    if (field.LocalField != "GCBH_YC" && field.LocalField != "GCQYBH_YC" && field.LocalField != "RECID_YC")
                        continue;
                    bool isDynamic;
                    object fieldValue = apiObj.GetFieldValue(field, data, allDatas, out isDynamic);
                    if (isDynamic)
                        continue;
                    if (fieldStr.Length > 0)
                        fieldStr.Append(",");
                    fieldStr.Append(field.LocalField + "=@" + field.LocalField + "");

                    if (field.IsFile)
                    {
                        string fieldFile = fieldValue.GetSafeString();
                        if (!String.IsNullOrEmpty(fieldFile))
                        {
                            IList<VDataFileItem> fieldFiles = apiObj.GetFiles(ref fieldFile);
                            if (fieldFiles.Count > 0)
                                ((List<VDataFileItem>)files).AddRange(fieldFiles);
                            fieldValue = fieldFile;
                        }
                    }

                    sqlParams.Add(new VSqlParam() { IsDynamic = isDynamic, ParamName = field.LocalField, ParamValue = fieldValue });

                }
                SysYcdyTableRelation relation = apiObj.GetRelation(table.LocalTableName);
                string localPlatformKey = relation.RemotePrimaryKey;
                string remotePlatformKey = apiObj.GetRemotePlatformKey(table.LocalTableName, localPlatformKey);
                //if (tableinfo == "工程信息")
                //{
                //    sql = "update " + table.LocalTableName + " set " + fieldStr.ToString() + " where " + localPlatformKey + "='" + data[remotePlatformKey] + "'";
                //}
                //else if (tableinfo.Contains(".单位"))
                //{
                //    str_yc = "gcqybh_yc";
                //}
                //else if (tableinfo.Contains(".人员"))
                //{
                //    str_yc = "recid_yc";
                //}
                if (fieldStr.Length>0)
                    sql = "update " + table.LocalTableName + " set " + fieldStr.ToString() + " where gcbh='" + GCBH + "'";
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion

        #endregion

        #region 列表界面获取数据

        /// <summary>
        /// 职工花名册
        /// </summary>
        /// <returns></returns>
        public JsonResult GetZGHMC()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            FORMDATA jsondata = new FORMDATA();
            S_FORMDATA s_data = new S_FORMDATA();
            try
            {
                //string RYXM = "";
                //string XB = "";
                //string SJHM = "";
                //string GZ = "";
                //string GW = "";
                //string SFZHM = "";
                //string bzfzrxm = "";
                string formdm = Request["formdm"].GetSafeString();
                string formStatus = Request["formstatus"].GetSafeString();
                string sort = Request["sort"].GetSafeString();
                string order = Request["order"].GetSafeString();
                    //时间戳
                string timestring = Request["timestring"].GetSafeString();
                //校验码
                string sign = Request["sign"].GetSafeString();
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["pagesize"], 20);
                int totalcount = 0;

                string signstr = String.Format("timestring={0}&secret={1}", timestring, "WebList");
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    List<string> zdlist = SelfService.getFormZDZD(formdm, formStatus);
                    if(zdlist!=null&&zdlist.Count!=0)
                    {
                        string filterRules = Request["filterRules"].GetSafeString();
                        //解析过滤条件
                        //JToken jsons = JToken.Parse(filterRules);//转化为JToken（JObject基类）   //[]表示数组，{} 表示对象
                       // string where = " ";
                        string where = SelfService.getSqlfzr(filterRules, zdlist, jdzch, false);
                        //for (int j = 0; j < zdlist.Count; j++)
                        //{
                        //    foreach (JToken baseJ in jsons)//遍历数组  //[]表示数组要循环
                        //    {
                        //        string fieldname = baseJ.Value<string>("fieldname");
                        //        string fieldvalue = baseJ.Value<string>("fieldvalue");
                        //        string fieldopt = baseJ.Value<string>("fieldopt");
                        //        string filtertype = baseJ.Value<string>("filtertype");
  
                        //        if (fieldname.ToLower() == zdlist[j].ToLower() && !string.IsNullOrEmpty(fieldvalue))
                        //        {
                        //            if (fieldname.ToLower() == "bzfzrxm")
                        //            {
                        //                string fzr_str = SelfService.getbzfzr_str(fieldvalue, jdzch);
                        //                where += " and bzfzr in (" + fzr_str + ")";
                        //            }
                        //            else
                        //                where += " and " + fieldname + " like '%" + fieldvalue + "%'";
                        //            break;
                        //        }
                        //        //if (fieldname == "XB")
                        //        //{
                        //        //    XB = fieldvalue;
                        //        //}
                        //        //if (fieldname == "SJHM")
                        //        //{
                        //        //    SJHM = fieldvalue;
                        //        //}
                        //        //if (fieldname == "GZ")
                        //        //{
                        //        //    GZ = fieldvalue;
                        //        //}
                        //        //if (fieldname == "GW")
                        //        //{
                        //        //    GW = fieldvalue;
                        //        //}
                        //        //if (fieldname == "SFZHM")
                        //        //{
                        //        //    SFZHM = fieldvalue;
                        //        //}
                        //        //if (fieldname == "bzfzrxm")
                        //        //{
                        //        //    bzfzrxm = fieldvalue;
                        //        //}
                        //    }

                        //}

                        //if (RYXM != "")
                        //    where += " and ryxm like '%" + RYXM + "%'";
                        //if (XB != "")
                        //    where += " and xb = '" + XB + "'";
                        //if (SJHM != "")
                        //    where += " and sjhm like '%" + SJHM + "%'";
                        //if (GZ != "")
                        //    where += " and gz like '%" + GZ + "%'";
                        //if (GW != "")
                        //    where += " and gw like '%" + GW + "%'";
                        //if (SFZHM != "")
                        //    where += " and SFZHM like '%" + SFZHM + "%'";
                        //if (bzfzrxm != "")
                        //{
                        //    string fzr_str = SelfService.getbzfzr_str(bzfzrxm, jdzch);
                        //    where += " and bzfzr in (" + fzr_str + ")";
                        //}

                        string sql = "select * from View_I_M_WGRY_HMC where jdzch ='" + jdzch + "'";
                        sql += where;
                        if (sort==""||order=="")
                            sql += " order by xssx asc,bzfzr asc";
                        else
                            sql += " order by " + sort + " "+order;
                        IList<IDictionary<string, string>> alllist = new List<IDictionary<string, string>>();

                        alllist = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                        // alllist = SelfService.GetSelfFormData(rows);
                        s_data.total = totalcount.ToString();
                        s_data.rows = alllist;
                        jsondata.data = s_data;
                        jsondata.success = true;
                        jsondata.msg = "";
                   
                    }
                    else
                    {
                        jsondata.success = false;
                        jsondata.msg = "ZDZD查找失败";
                    }
                }
                else
                {
                    jsondata.success = false;
                    jsondata.msg = "校验失败";
                }

                
            }
            catch(Exception e)
            {
                jsondata.success = false;
                jsondata.msg = e.Message;
            }
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 月考勤详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetYKQXQ()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            FORMDATA jsondata = new FORMDATA();
            S_FORMDATA s_data = new S_FORMDATA();
            try
            {
                //时间戳
                string timestring = Request["timestring"].GetSafeString();
                //校验码
                string sign = Request["sign"].GetSafeString();
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["pagesize"], 20);
                int totalcount = 0;

                string signstr = String.Format("timestring={0}&secret={1}", timestring, "WebList");
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    string formdm = Request["formdm"].GetSafeString();
                    string formStatus = Request["formstatus"].GetSafeString();
                    string sort = Request["sort"].GetSafeString();
                    string order = Request["order"].GetSafeString();

                    List<string> zdlist = SelfService.getFormZDZD(formdm, formStatus);
                    if (zdlist != null && zdlist.Count != 0)
                    {
                        string filterRules = Request["filterRules"].GetSafeString();
                        //解析过滤条件
                        JToken jsons = JToken.Parse(filterRules);//转化为JToken（JObject基类）   //[]表示数组，{} 表示对象
                        string where = SelfService.getSqlfzr(filterRules, zdlist, jdzch,true,"bzzuserid");

                        string sql = "select * from ViewKqjUserMonthPay where jdzch ='" + jdzch + "'";
                        sql += where;
                        if (sort == "" || order == "")
                            sql += " order by xssx asc,logyear desc,logmonth desc,bzzuserid asc";
                        else
                            sql += " order by " + sort + " " + order;
                        IList<IDictionary<string, string>> alllist = new List<IDictionary<string, string>>();

                        alllist = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                        s_data.total = totalcount.ToString();
                        s_data.rows = alllist;
                        jsondata.data = s_data;
                        jsondata.success = true;
                        jsondata.msg = "";                       
                    }
                    else
                    {
                        jsondata.success = false;
                        jsondata.msg = "ZDZD查找失败";
                    }
                }
                else
                {
                    jsondata.success = false;
                    jsondata.msg = "校验失败";
                }
                
            }
            catch (Exception e)
            {
                jsondata.success = false;
                jsondata.msg = e.Message;
            }
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 月工资统计
        /// </summary>
        /// <returns></returns>
        public JsonResult GetYGZTJ()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            FORMDATA jsondata = new FORMDATA();
            S_FORMDATA s_data = new S_FORMDATA();
            try
            {
                //时间戳
                string timestring = Request["timestring"].GetSafeString();
                //校验码
                string sign = Request["sign"].GetSafeString();
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["pagesize"], 20);
                int totalcount = 0;

                string signstr = String.Format("timestring={0}&secret={1}", timestring, "WebList");
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    string formdm = Request["formdm"].GetSafeString();
                    string formStatus = Request["formstatus"].GetSafeString();
                    string sort = Request["sort"].GetSafeString();
                    string order = Request["order"].GetSafeString();

                    List<string> zdlist = SelfService.getFormZDZD(formdm, formStatus);
                    if (zdlist != null && zdlist.Count != 0)
                    {
                        string filterRules = Request["filterRules"].GetSafeString();
                        //解析过滤条件
                        JToken jsons = JToken.Parse(filterRules);//转化为JToken（JObject基类）   //[]表示数组，{} 表示对象
                        string where = SelfService.getSqlfzr(filterRules, zdlist, jdzch,true);

                        string sql = "select * from ViewKqjUserMonthLog where jdzch ='" + jdzch + "'";
                        sql += where;
                        if (sort == "" || order == "")
                            sql += " order by xssx,logyear desc,logmonth desc,bzfzr asc";
                        else
                            sql += " order by " + sort + " " + order;
                        IList<IDictionary<string, string>> alllist = new List<IDictionary<string, string>>();

                        alllist = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                        s_data.total = totalcount.ToString();
                        s_data.rows = alllist;
                        jsondata.data = s_data;
                        jsondata.success = true;
                        jsondata.msg = "";
                    }
                    else
                    {
                        jsondata.success = false;
                        jsondata.msg = "ZDZD查找失败";
                    }
                }
                else
                {
                    jsondata.success = false;
                    jsondata.msg = "校验失败";
                }

            }
            catch (Exception e)
            {
                jsondata.success = false;
                jsondata.msg = e.Message;
            }
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 微信长连接转短连接
        [Authorize]
        private string LongToShortUrl(string longUrl, string Access_token)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=" + Access_token;
            string datas = "{\"action\":\"long2short\","
                    + "\"long_url\":\"" + longUrl + "\"}";            
            string result=SelfService.PostData(url, datas);
            JToken jsons = JToken.Parse(result);//转化为JToken（JObject基类）
            string errcode = jsons["errcode"].GetSafeString();
            if (errcode!="0")
            {
                result = jsons["errmsg"].GetSafeString();
            }
            else
                result = jsons["short_url"].GetSafeString();

            return result;

        }
        /// <summary>
        /// 获取微信的Access_token
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Authorize]
        private string getAccess_token(string url)
        {
            string Access_token = "";
            string sql = "select * from sys_wx where  lx='token'";
            IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
            if(list.Count==0)
            {

            }
            else
            {
                int s_expires_in = list[0]["expires_in"].GetSafeInt();
                DateTime s_gxsj = list[0]["gxsj"].GetSafeDate();
                if (DateTime.Now > s_gxsj.AddSeconds(s_expires_in - 300)) //有效时间到钱5分钟更新需要更新
                {
                    string result = SelfService.GetHttpResponse(url);
                    JToken jsons = JToken.Parse(result);//转化为JToken（JObject基类）
                    Access_token = jsons["access_token"].GetSafeString();
                    string expires_in = jsons["expires_in"].GetSafeString();
                    sql="update sys_wx set access_token='"+Access_token+"',expires_in='"+expires_in+"',gxsj=getdate()  where  lx='token'";
                    CommonService.Execsql(sql);
                }
                else
                {
                    Access_token = list[0]["access_token"];
                }
            }

            return Access_token;
        }
       


        #endregion

        #region 下发五大员虹膜模板
        public void DownWdyiris()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string ryxm = Request["ryxm"].GetSafeString();

                if ((sfzhm == "") || gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }
                else
                {
                    sfzhm = CryptFun.Decode(sfzhm);
                    gcbh = CryptFun.Decode(gcbh); 
                }
                if (code)
                {

                    string hm = Request["hm"].GetSafeString();
                    IList<IDictionary<string, string>> kqjs = WgryKqjService.GetYCGcKqj(gcbh);
                    IList<string> sqls = new List<string>();
                    foreach (IDictionary<string, string> row in kqjs)
                    {
                        string serial = "";
                        if (row.TryGetValue("kqjbh", out serial))
                        {
                            string sql = "insert into KqjDeviceCommand ([Serial],[Command] ,[UserId],[RealName] ,[UserStation] ,[IrisModule]) values ('" + serial + "',21,'" + sfzhm + "','" + ryxm + "','','" + hm + "')";
                            sqls.Add(sql);
                        }
                    }
                    if (sqls.Count>0)
                        code = CommonService.ExecSqls(sqls);
                    else
                    {
                        msg = "没有找到相应考勤机！";
                        code = false;
                    }
                }

            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除五大员虹膜模板
        /// </summary>
        public void DelWdyiris()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();

                if ((sfzhm == "") || gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }
                sfzhm = CryptFun.Decode(sfzhm);
                gcbh = CryptFun.Decode(gcbh);
                if (code)
                {
                    IList<IDictionary<string, string>> kqjs = WgryKqjService.GetYCGcKqj(gcbh);
                    IList<string> sqls = new List<string>();
                    foreach (IDictionary<string, string> row in kqjs)
                    {
                        string serial = "";
                        if (row.TryGetValue("kqjbh", out serial))
                        {                           
                            string sql = "insert into KqjDeviceCommand ([Serial],[Command] ,[UserId],[RealName] ,[UserStation] ,[IrisModule]) values('" + serial + "',22,'" + sfzhm + "','','','')";
                            sqls.Add(sql);
                        }
                    }
                    if (sqls.Count > 0)
                        code = CommonService.ExecSqls(sqls);
                    else
                    {
                        msg = "没有找到相应考勤机！";
                        code = false;
                    }
                }

            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 获取五大员考勤记录
        /// </summary>
        public void GetWdyKqlist()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int totalcount = 0;
            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string starttime = Request["dt1"].GetSafeString();
                string endtime = Request["dt2"].GetSafeString();
                string recid = Request["recid"].GetSafeString();

                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 100);

                if ((sfzhm == ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }
               
                sfzhm = CryptFun.Decode(sfzhm).FormatSQLInStr();
                gcbh = CryptFun.Decode(gcbh);
                wgptbh = CryptFun.Decode(wgptbh);
                string where = "";
                if (gcbh != "")
                    where += " and placeid=(select gcbh from i_m_gc where gcbh_yc='" + gcbh + "' and wgptbh='" + wgptbh + "')";
                else
                    where += " and placeid=(select gcbh from i_m_gc where wgptbh='" + wgptbh + "')";
                if (starttime != "")
                    where += " and logdate>='" + starttime + "'";
                if (endtime != "")
                    where += " and logdate<='" + endtime + "'";
                if (recid != "")
                    where += " and recid>'" + recid + "'"; 
                if (code)
                {
                    string sql = "select * from kqjuserlog where userid in (" + sfzhm + ")" + where + " order by logdate desc";
                    dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(dt)));
            }
        }
        #endregion


        #region 工资操作
        [Authorize]
        public void SetYZpay()
        {
            return;
            string recid = Request["recid"].GetSafeString();
            string msg = "";
            bool code = false;
            try
            {
                string yzpay = "0";
                string sql = "select * from I_S_GC_YZ where recid='" + recid + "'";
                IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql);
                if(dt.Count>0)
                {
                    yzpay = dt[0]["yzpay"];
                    string sfzhm=dt[0]["sfzhm"];
                    string jdzch=dt[0]["jdzch"];
                    string year=dt[0]["yzyear"];
                    string month=dt[0]["yzmonth"];
                    string where=" where jdzch='"+jdzch+"' and userid='"+sfzhm+"' and logyear='"+year+"' and logmonth='"+month+"'";
                    sql = "update KqjUserMonthPay set yzpay='" + yzpay + "' " + where;
                    code=CommonService.Execsql(sql);
                }
            }
            catch(Exception e)
            { }
            Response.Write(JsonFormat.GetRetString(code, msg));
        }
        /// <summary>
        /// 工资册推送接口
        /// </summary>
        [Authorize]
        public void SetPayroll()
        {
            bool code=true;
            string msg="";
            Payroll payroll = new Payroll();      
            try{
                string rguid=Request["rguid"].GetSafeString();
                string sql="select * from View_I_M_XZFF where rguid='"+rguid+"'";
                IList<IDictionary<string, string>> dt1=CommonService.GetDataTable(sql);
                if(dt1.Count>0)
                {
                    string key = MD5Util.StringToMD5Hash(BD.Jcbg.Web.Func.GlobalVariable.WGRYPAY_KEY);
                    string recids = dt1[0]["payrecids"];
                    string gcbh = dt1[0]["jdzch"];
                    string sgdwbh = dt1[0]["sgdwbh"];
                    string lwgsbh = dt1[0]["lwgsbh"];
                    string year = dt1[0]["payyear"];
                    string month = dt1[0]["paymonth"];
                    string paytype = "";
                    string remark1 = "";
                    string remark2 = "";
                    payroll.paycode = rguid;
                    payroll.projectcode = gcbh;
                    payroll.companycode = sgdwbh;
                    payroll.paycompanycode = lwgsbh == "" ? sgdwbh : lwgsbh;
                    payroll.payyear = year;
                    payroll.paymonth = month;
                    payroll.paytype = paytype;
                    payroll.remark1 = remark1;
                    payroll.remark2 = remark2;
                    List<Payrollrows> payrowslist = new List<Payrollrows>();
                    sql = "select * from View_KQJUSERMONTHPAY where recid in(" + recids + ")"; 
                    IList<IDictionary<string, string>> rydetails=CommonService.GetDataTable(sql);
                    for(int i=0;i<rydetails.Count;i++)
                    {
                        Payrollrows payrows = new Payrollrows();
                        string Name = rydetails[i]["ryxm"];
                        string phone = rydetails[i]["sjhm"];
                        string IdNumber = rydetails[i]["userid"];
                        string CardNumber = rydetails[i]["yhkh"];
                        string BankNumber = rydetails[i]["yhhh"];
                        string Paysum = rydetails[i]["havepay"];
                        string Remark1 = "";
                        payrows.Name = Name;
                        payrows.Phone = phone;
                        payrows.IdNumber = IdNumber;
                        payrows.CardNumber = CardNumber;
                        payrows.BankNumber = BankNumber;
                        payrows.Paysum = Paysum;
                        payrows.Remark1 = Remark1;
                        payrowslist.Add(payrows);
                    }
                    payroll.rows = payrowslist;

                }               
            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string url = "http://ip[:port]/ api/apipay/SetPayroll";
                string json = jss.Serialize(payroll);

                if (!MyHttp.Post(url, json, out msg))
                {
                    code = false;
                    msg = "推送工资册失败";
                }
                Response.Write(JsonFormat.GetRetString(code, msg));
             
            }
        }
       
        /// <summary>
        /// 工资发放结果推送,该接口用于支付平台向务工人员平台推送工资发放结果
        /// </summary>

        public JsonResult GetPayrollResult()
        {
            bool code = false;
            string msg = "";
            try
            {
                string json = Request["json"];
                JavaScriptSerializer js = new JavaScriptSerializer();
                PayrollResult payResult = js.Deserialize<PayrollResult>(json); //反序列化
                if(payResult!=null)
                {
                    string key = MD5Util.StringToMD5Hash(BD.Jcbg.Web.Func.GlobalVariable.WGRYPAY_KEY);
                    string ResultKey = payResult.key;
                    if(key==ResultKey)
                    {
                        string Paycode = payResult.Paycode;
                        string Shouldpay = payResult.Shouldpay;
                        string Realpay = payResult.Realpay;
                        string Msg = payResult.Message;
                        string recids = "";
                        string sql = "select * from View_I_M_XZFF where rguid='" + Paycode + "'";
                        IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable(sql);
                        if (dt1.Count > 0)
                        {
                            recids = dt1[0]["payrecids"];
                            string[] recidlist = recids.Split(',');
                            sql = "update INFO_M_XZFF set ShouldPay='" + Shouldpay + "',Realpay='" + Realpay + "',Message='" + Msg + "' where rguid='" + Paycode + "'";
                            CommonService.Execsql(sql);
                        }


                        List<PayrollResultrows> rows = payResult.rows;
                        IList<string> sqls = new List<string>();
                        foreach (PayrollResultrows row in rows)
                        {
                            string Name = row.Name;
                            string Phone = row.Phone;
                            string IdNumber = row.IdNumber;
                            string CardNumber = row.CardNumber;
                            string s_Shouldpay = row.Shouldpay;
                            string s_Realpay = row.Realpay;
                            string message = row.message;
                            sql = "update KqjUserMonthPay set bankpay='" + s_Realpay + "',message='" + message + "',CardNumber='" + CardNumber + "',Phone='" + Phone + "' where userid='" + IdNumber + "' and ryxm='" + Name + "' and recid in(" + recids + ")";
                            sqls.Add(sql);
                        }
                        code = CommonService.ExecSqls(sqls);
                    }
                    else
                    {
                        code = false;
                        msg = "校验失败";
                    }
                }

            }
            catch(Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { success = code ? "0000" : "1", message = msg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 该接口用于务工人员平台把人员卡号或手机号变更信息推送给支付平台
        /// </summary>
        [Authorize]
        public void SetPersonCard()
        {
            bool code = true;
            string msg = "";
            PersonCard pcard = new PersonCard();
            try
            {
                string ryxm = Request["ryxm"].GetSafeString() ;
                string sfzhm = Request["idnumber"].GetSafeString();
                string Fromcard = Request["fromcard"].GetSafeString(); //原银行卡号
                string Tocard = Request["tocard"].GetSafeString(); //新银行卡号
                string ToBankCode = Request["ToBankCode"].GetSafeString(); //新银行行号
                string Formphone = Request["Formphone"].GetSafeString(); //原手机号码
                string Tophone = Request["Tophone"].GetSafeString();  //新手机号码
                List<PersonCardrows> pcardrowslist = new List<PersonCardrows>();
                PersonCardrows pcardrows = new PersonCardrows();

                pcardrows.Name = ryxm;

                pcardrows.IdNumber=sfzhm;
                pcardrows.Fromcard=Fromcard;
                pcardrows.Tocard=Tocard;
                pcardrows.ToBankCode=ToBankCode;
                pcardrows.Fromephone = Formphone;
                pcardrows.Tophone=Tophone;
                pcardrowslist.Add(pcardrows);


                string key = MD5Util.StringToMD5Hash(BD.Jcbg.Web.Func.GlobalVariable.WGRYPAY_KEY);
                pcard.success = "0000";
                pcard.message = "";
                pcard.rows = pcardrowslist;
             
            }
            catch (Exception e)
            {
                msg = e.Message;
                pcard.success = "1111";
                pcard.message = msg;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string url = "http://ip[:port]/ api/apipay/SetPersonCard";
                string json = jss.Serialize(pcard);
                //Response.Write(json);
                MyHttp.Post(url, json, out msg);
            }
        }
        #endregion


        #region 务工人员上层下载
        public void PageWgryList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string lasttime = Request["lasttime"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
         
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and (yhkh is not null or yhkh <> '') and (yhhh is not null or yhhh <> '') and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from I_M_WGRY where 1=1 " + strwhere + " order by jdzch desc";
                sql = "select ryxm,sfzhm,sjhm,YHKH,YHHH " + sql;

                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> dstdate=new Dictionary<string, object>();
                    string sfzhm =CryptFun.Encode(date["sfzhm"]);
                    string yhkh = CryptFun.Encode(date["yhkh"]);

                    dstdate.Add("ryxm", date["ryxm"]);
                    dstdate.Add("sfzhm", sfzhm);
                    dstdate.Add("sjhm", date["sjhm"]);
                    dstdate.Add("yhkh", yhkh);
                    dstdate.Add("yhhh", date["yhhh"]);
                    records.Add(dstdate);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }
        #endregion
    }

}