using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BD.Jcbg.Web.Controllers
{
    public class QyController:Controller
    {
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

        #region 页面
        [Authorize]
        public ActionResult Qysqsp()
        {
            return View();
        }
        [Authorize]
        public ActionResult Qyzzsp()
        {
            return View();
        }
		public ActionResult Kgbasp()
        {
            return View();
        }
		public ActionResult Ysbasp()
        {
            return View();
        }
        /// <summary>
        /// 查看企业自己信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SelfInfo()
        {
            string buttons = Server.UrlEncode("保存|TJ| | ");
            string title = Server.UrlEncode("企业信息");
            string sql = "select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                string url = "/datainput/Index?zdzdtable=zdzd_jc&t1_tablename=i_m_qy&&t1_pri=qybh&t1_title=" + title + "&button=" + buttons + "&rownum=1&LX=I&jydbh=" + dt[0]["qybh"];
                return new RedirectResult(url);
            }
            else
                return null;
        }
        /// <summary>
        /// 企业资质附件查看
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Qyzzck()
        {
            ViewBag.qybh = Request["qybh"].GetSafeRequest();
            return View();
        }
        /// <summary>
        /// 企业查看
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewQy()
        {
            ViewBag.qybh = Request["qybh"].GetSafeRequest();
            return View();
        }


        [Authorize]
        public ActionResult Qyview()
        {
            ViewBag.qybh = Request["qybh"].GetSafeString();
            return View();
        }
        public ActionResult Ryview()
        {
            ViewBag.rybh = Request["rybh"].GetSafeString();
            return View();
        }
        public ActionResult QySearch()
        {
            ViewBag.rybh = Request["rybh"].GetSafeString();
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取未审批的企业申请数量
        /// </summary>
        [Authorize]        
        public void GetQysqsl()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from view_i_m_zhsq");
                msg = dt[0]["sum"];
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
        /// 获取资质审批数量
        /// </summary>
        [Authorize]
        public void GetQyzzsqsl()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from i_s_qy_qyzz where sptg=0");
                msg = dt[0]["sum"];
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
        /// 获取工程企业类型
        /// </summary>
        [Authorize]
        public void GETGCQYLX()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            bool code = true;
            string msg = "";
            try
            {
                string sql = "select qylxmc from View_GC_QY where gcbh='" + gcbh + "' and ZH='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    msg = dt[0]["qylxmc"];
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public JsonResult GetQyByName()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string key = Request["key"].GetSafeString();
                string sql = "select top 20 * from i_m_qy where 1=1";
                if (key != "")
                    sql += " and QYMC like '%" + key + "%'";
                else
                    sql += " and 1=2 ";
                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(ret);
        }



        /// <summary>
        /// 注册时校验
        /// </summary>
        public void CheckRegister()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string yzm = Request["yzm"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string yzmE = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“发送验证码”获取验证码";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["REGISTER_VERIFY_CODE"] = null;

                        if (!string.IsNullOrEmpty(username))
                        {
                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from (( select zh from i_m_ry where zh='" + username + "') union all ( select zh from i_m_qy where zh='" + username + "') union all ( select yhzh from i_m_qyzh where yhzh='" + username + "')) as t1");
                            if (dt[0]["sum"].GetSafeInt() > 0)
                                msg = "登录账号已存在";
                            else
                            {
                                if (UserService.UserExists(username))
                                    msg = "账号已存在";
                            }
                        }
                        if (!string.IsNullOrEmpty(qymc))
                        {

                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh from I_M_QY where QYMC='" + qymc + "'");
                            if (dt.Count > 0)
                            {
                                msg = "【" + qymc + "】系统中已经存在，企业账户：" + dt[0]["zh"].GetSafeString();
                            }
                        }
                        
                        code = msg == "";
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
        /// 忘记密码时校验
        /// </summary>
        public void CheckRegister2()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                string pwd = Request["pwd"].GetSafeString();
                if (yzmE == null)
                    msg = "验证码无效，请点击“发送验证码”获取验证码";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["REGISTER_VERIFY_CODE"] = null;

                        code = ReSertPassWord(username, pwd, out msg);
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
        /// 重置密码
        /// </summary>
        public bool ReSertPassWord(string username, string pwd, out string msg)
        {
            bool code = true;
            try
            {
                string json = UserService.ReSetPassWord(username, pwd);

                JToken jsons = JToken.Parse(json);//转化为JToken（JObject基类）
                code = jsons["success"].GetSafeBool();
                msg = jsons["msg"].GetSafeString();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            return code;

        }


        /// <summary>
        /// 校验企业名称或者五证合一码是否存在，存在，返回false，定位到重置密码页面
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
                    // 企业名称有重复的，直接跳转到重置密码页面
                    string sql = string.Format("select qymc from i_m_qy where qymc='{0}'", qymc);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        code = true;
                    }
                    else
                    {
                        code = false;
                        toreset = true;
                        msg = dt[0]["qymc"];
                    }

                }
                else if (qydm.Length == 9 || qydm.Length == 18)
                {
                    string where = " qymc='"+qymc+"' or zzjgdm='"+qydm+"' ";
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
                    string sql = "select qybh, qymc, zzjgdm from i_m_qy where " + where;
                    IList<IDictionary<string,string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        code = true;
                    }
                    else
                    {
                        code = false;
                        toreset = true;
                        msg = dt[0]["qymc"];
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
            return Json(new { code = code ? "0" : "1", toreset=toreset?"0":"1", msg = msg });
        }
        /// <summary>
        /// 根据当前用户账号获取企业编号
        /// </summary>
        [Authorize]
        public void GetQybh()
        {
            string msg = "";
            bool code = false;
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qybh from i_m_qyzh where yhzh='"+CurrentUser.UserName+"'");
                if (dt.Count > 0)
                {
                    code = true;
                    msg = dt[0]["qybh"];
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

        [Authorize]
        public JsonResult GetQyzzImages(string qybh)
        {
            bool code = false;
            string msg = "";
            IList<IDictionary<string, string>> images = new List<IDictionary<string, string>>();
            try
            {
                images = CommonService.GetDataTable("select a.zzmc,b.ZZNRMC, a.ZZWJ from I_S_QY_QYZZ a left outer join H_QYZZNR b on a.ZZNRBH=b.ZZNRBH where (a.ZZWJ is not null and a.ZZWJ<>'') and a.qybh='" + qybh + "'");
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", images = images, msg = msg });
        }
        #endregion

        #region 更新数据
        /// <summary>
        /// 企业申请审批
        /// </summary>
        [Authorize]
        public void SetQysqsp()
        {
            bool code = true;
            string msg = "";
            try
            {
                string qybh = Request["usercode"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string spbz = Request["spbz"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_qy set sptg=1,sfyx=" + checkoption + ",spbz='" + spbz + "' where qybh='" + qybh + "'");
                code = CommonService.ExecTrans(sqls);
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

        [Authorize]
        public void CheckQyzz()
        {
            bool code = false;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                IList<string> sqls = new List<string>();
                string qyfzrsj="";
                string username = "";
                string sql = "";
                //获取资质的企业信息
                if (checkoption == 0)
                {
                    sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=0,spsj=getdate(),sprzh='"+CurrentUser.UserName+"',sprxm='"+CurrentUser.RealName+"' where zzbh='" + zzbh + "'");
                }
                else
                {                
                    sql = "select zh from i_m_qy where qybh=(select qybh from i_s_qy_qyzz where zzbh='" + zzbh + "')";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0 || dt[0]["zh"] == "")
                        msg = "获取企业账号失败";
                    else
                    {                  
						username = dt[0]["zh"];
                        sql = "select zhjsbh from h_qylx where lxbh=(select qylxbh from i_s_qy_qyzz where zzbh='" + zzbh + "')";
                        dt = CommonService.GetDataTable(sql);
                        // 需要创建角色
                        if (dt.Count > 0 && dt[0]["zhjsbh"] != "")
                        {
                            string roleid = dt[0]["zhjsbh"];
                            string[] listroleid = roleid.Split(',');
                            for (int i = 0; i < listroleid.Length; i++)
                            {
                                if (UserService.AddUserRole(username, listroleid[i], out msg))
                                {
                                    msg = "";
                                }
                            }
                        }
                        if (msg == "")
                            sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where zzbh='" + zzbh + "'");
                    
                    }
                }
                if (msg == "")                  
                    code = CommonService.ExecTrans(sqls);

                if (msg==""&&GlobalVariable.GetConfigValue("qyzzsptg") == "true")
                {
                    string phones = qyfzrsj;
                    if(phones!="")
                    {
                        IList<IDataParameter> sqlparams = new List<IDataParameter>();
                        IDataParameter sqlparam = null;

                        string content = "";
                        if (checkoption == 0)
                            content = "你的企业资质（类型）申请未通过，需重新提交，";
                        else
                            content = "你的企业资质（类型）申请已通过，";
                        sql = "Insert INTO INFO_SMS ([guid],[Phone],[Message],[HasDeal],[LX]) values (NEWID(),@lxdhs,@msg,0,'qyzzsptg')";
                        sqlparams = new List<IDataParameter>();
                        sqlparam = new SqlParameter("@lxdhs", phones);
                        sqlparams.Add(sqlparam);
                        sqlparam = new SqlParameter("@msg", content);
                        sqlparams.Add(sqlparam);

                        CommonService.ExecTrans(sql, sqlparams, out msg);
                    }
                   
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        [Authorize]
        public void CheckQyzz2()
        {
            bool code = false;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeString();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=0,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',thyy='" + reason + "' where zzbh in (" + zzbh + ")");
                }
                else
                {
                    string sql = "select zh from i_m_qy where qybh in (select qybh from i_s_qy_qyzz where zzbh in (" + zzbh + "))";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0 || dt[0]["zh"] == "")
                        msg = "获取企业账号失败";
                    else
                    {
                        string username = dt[0]["zh"];
                        sql = "select zhjsbh from h_qylx where lxbh in (select qylxbh from i_s_qy_qyzz where zzbh in (" + zzbh + "))";
                        dt = CommonService.GetDataTable(sql);
                        // 需要创建角色
                        if (dt.Count > 0 && dt[0]["zhjsbh"] != "")
                        {
                            string roleid = dt[0]["zhjsbh"];
                            if (UserService.AddUserRole(username, roleid, out msg))
                            {
                                msg = "";

                            }
                            else
                                msg = "创建企业角色失败!";
                        }
                        if (msg == "")
                            sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where zzbh in (" + zzbh + ")");
                    }
                }
                if (msg == "")
                    code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        


        [Authorize]
        public void SetRydw()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set qybh=(select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "') where rybh in (" + rybh.FormatSQLInStr() + ") and (qybh is null or qybh='')");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void ClearRydw()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set qybh='' where rybh in (" + rybh.FormatSQLInStr() + ")");

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		[Authorize]
        public void ClearRydwINFO()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recids = Request["recids"].GetSafeRequest();
                string sfzhms=Request["sfzhms"].GetSafeRequest();
                string jdzch = Request["jdzch"].GetSafeRequest();
               // string[] jdzchlist = jdzchs.Split(',');
                IList<string> sqls = new List<string>();
                //sqls.Add("update i_m_ry set qybh='' where recid in (" + recids.FormatSQLInStr() + ")");
                sqls.Add("delete from i_m_ry where recid in (" + recids.FormatSQLInStr() + ")");
                string sql = "";
                //设置退场时间
                sql="Update I_M_RY_History set outtime ='"+DateTime.Now.ToString()+"' where jdzch ='"+jdzch+"' and sfzhm in (" + sfzhms.FormatSQLInStr() + ")";
                sql += " and Intime is not null and OutTime is null";
                sqls.Add(sql);
                

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void SetWgryLeave()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recids = Request["recids"].GetSafeRequest();
                string sfzhms = Request["sfzhms"].GetSafeRequest();
                string jdzch = Request["jdzch"].GetSafeRequest();
                // string[] jdzchlist = jdzchs.Split(',');
                IList<string> sqls = new List<string>();
                //sqls.Add("update i_m_ry set qybh='' where recid in (" + recids.FormatSQLInStr() + ")");
                sqls.Add("update i_m_wgry set hasdelete=1 where jdzch ='" + jdzch + "' and sfzhm in (" + sfzhms.FormatSQLInStr() + ")");
                string sql = "";
                //设置退场时间
               // sql = "Update I_M_RY_History set outtime ='" + DateTime.Now.ToString() + "' where jdzch ='" + jdzch + "' and sfzhm in (" + sfzhms.FormatSQLInStr() + ")";
                sql = "Update InfoWgryHistory set outtime ='" + DateTime.Now.ToString() + "' where projectid ='" + jdzch + "' and sfzhm in (" + sfzhms.FormatSQLInStr() + ")";
                sql += " and Intime is not null and OutTime is null";
                sqls.Add(sql);


                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

		[Authorize]
        public void AddRoleForQY()
        {
            bool code = false;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeRequest();
                IList<string> sqls = new List<string>();

                string sql = "select zh from i_m_qy where qybh=(select qybh from i_s_qy_qyzz where zzbh='" + zzbh + "')";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0 || dt[0]["zh"] == "")
                    msg = "获取企业账号失败";
                else
                {
                    string username = dt[0]["zh"];
                    sql = "select zhjsbh from h_qylx where lxbh=(select qylxbh from i_s_qy_qyzz where zzbh='" + zzbh + "')";
                    dt = CommonService.GetDataTable(sql);
                    // 需要创建角色
                    if (dt.Count > 0 && dt[0]["zhjsbh"] != "")
                    {
                        string roleid = dt[0]["zhjsbh"];
                        string[] listroleid = roleid.Split(',');
                        for (int i = 0; i < listroleid.Length; i++)
                        {
                            if (UserService.AddUserRole(username, listroleid[i], out msg))
                            {
                                msg = "";
                            }
                        }        
                    }
                    if (msg == "")
                        sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where zzbh='" + zzbh + "'");
                }

                if (msg == "")
                    code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 提交工程
        /// </summary>
        [Authorize]
        public void SubmitGc()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_gc set zt='LR' where gcbh='" + gcbh + "' and zt='YT'");

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        /// <summary>
        /// 取消预填
        /// </summary>
        [Authorize]
        public void ChancelGc()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_gc set zt='YT', Remark='' where gcbh='" + gcbh + "' and zt='LR'");

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

		/// <summary>
        /// 提交工程开工备案
        /// </summary>
        [Authorize]
        public void SubmitGcKGBA()
        {
            bool code = false;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update I_M_GC_KGBA set SPTG=0,SFYX=0,TJSP=1,mark='' where jdzch='" + jdzch + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 主体验收备案
        /// </summary>
        [Authorize]
        public void SubmitYSBA()
        {
            bool code = false;
            string msg = "";
            try
            {
                //string jdzch = Request["jdzch"].GetSafeRequest();
                string recid = Request["recid"].GetSafeRequest();
                string ysbalx = Request["ysbalx"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                sqls.Add("update I_M_GC_YSBA set SFSP=0,SFYX=0,TJSP=1,mark='' where recid='" + recid + "'and ysbalx='" + ysbalx + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
   

        [Authorize]
        public void SetKgbasp()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string spbz = Request["spbz"].GetSafeString();
                string reason = Request["reason"].GetSafeString();
                string serverip = Configs.GetOaServerIp;
                IList<string> sqls = new List<string>();           
                if(checkoption==1)
                    sqls.Add("update " + serverip + "[oa_jx].dbo.MJDZC SET ZT='正常' where JDZCH='" + jdzch + "'");                   
                else
                    sqls.Add("update " + serverip + "[oa_jx].dbo.MJDZC SET ZT='开工备案未通过' where JDZCH='" + jdzch + "'");
                sqls.Add("update I_M_GC_KGBA set sptg=1,tjsp=0,sfyx=" + checkoption + ",mark='" + reason + "',spry='" + CurrentUser.UserName + "',spryxm='" + CurrentUser.RealName + "',spsj='" +DateTime.Now + "' where jdzch='" + jdzch + "'");
                code = CommonService.ExecSqls(sqls);
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
        [Authorize]
        public void SetYsbasp()
        {
            bool code = true;
            string msg = "";
            try
            {
                string jdzch = Request["jdzch"].GetSafeRequest();
                string recid = Request["recid"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeString();
                string ysbalx = Request["ysbalx"].GetSafeString();
                string serverip = Configs.GetOaServerIp;
                IList<string> sqls = new List<string>();
                if (checkoption == 1)
                    sqls.Add("update " + serverip + "[oa_jx].dbo.MJDZC SET YSBAZT='" + ysbalx + "' where JDZCH='" + jdzch + "'");
                else
                    sqls.Add("update " + serverip + "[oa_jx].dbo.MJDZC SET YSBAZT='" + ysbalx + "备案未通过' where JDZCH='" + jdzch + "'");
                sqls.Add("update I_M_GC_YSBA set sfsp=1,sfyx=" + checkoption + ",tjsp=0,mark='" + reason + "',spsj=getdate(),spry='" + CurrentUser.UserName + "',spryxm='" + CurrentUser.RealName + "' where recid='" + recid + "'");
                code = CommonService.ExecSqls(sqls);
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
        [Authorize]
        public void UpdateQyRole()
        {
            bool code = true;
            string msg = "";
            try
            {
                string qybh = Request["qybh"].GetSafeRequest();
                string username = "", usercode = "";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh from i_m_qy where qybh='" + qybh + "'");
                if (dt.Count == 0 || dt[0]["zh"] == "")
                {
                    //dt = CommonService.GetDataTable("select yhzh as zh from i_m_qyzh where yhzh='" + rybh + "'");
                    //if (dt.Count == 0)
                    //{
                    code = false;
                    msg = "找不到企业记录，更新角色失败，请联系管理员";
                    //}
                }
                else
                    username = dt[0]["zh"];

                if (code)
                {
                    if (username != "" || usercode != "")
                    {
                        dt = CommonService.GetDataTable("select zhjsbh from h_qylx where lxbh in (select qylxbh from i_s_qy_qyzz where qybh='" + qybh + "' and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_qy where qybh='" + qybh + "')");
                        IList<string> roleCodes = new List<string>();
                        foreach (IDictionary<string, string> row in dt)
                            roleCodes.Add(row["zhjsbh"].GetSafeString());
                        UserService.UpdateUserRole(username, usercode, roleCodes, out msg);
                        code = msg == "";
                    }
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
        // 设置送样人员
        [Authorize]
        public void SetSyry()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set sfsyry=1 where rybh in (" + rybh.FormatSQLInStr() + ") and (qybh=(select qybh from i_m_qy where zh='"+CurrentUser.RealUserName+"'))");

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		// 取消送样人员
        [Authorize]
        public void RemoveSyry()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set sfsyry=0 where rybh in (" + rybh.FormatSQLInStr() + ") and (qybh=(select qybh from i_m_qy where zh='" + CurrentUser.RealUserName + "'))");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        public void SubmitGcAJ()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_gc set zt='LR' ,ajtj=1,ajthyy='' where gcbh='" + gcbh + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        [Authorize]
        public void updateQYsjhm()
        {
            bool code = true;
            string msg = "";
            string sjhm = Request["sjhm"].GetSafeRequest();
            string rylx = Request["rylx"].GetSafeRequest();
            string qybh = Request["qybh"].GetSafeRequest();
            try
            {
                IList<string> sqls = new List<string>();
                string sjzd = "";
                if (rylx == "企业负责人")
                    sjzd = "lxsj";
                else if (rylx == "企业法人")
                    sjzd = "lxdh";
                else //企业技术负责人
                    sjzd = "qyjsfzrsj";
                string sql = "Update I_M_QY SET " + sjzd + "='" + sjhm + "' where qybh='" + qybh + "';";
                sqls.Add(sql);
                code = CommonService.ExecTrans(sqls);
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
        /// 企业查询
        /// </summary>
        public void searchQY()
        {
            string qymc = Request["qymc"].GetSafeRequest();
            bool code = true;
            string msg = "";
            try
            {
                if (qymc == "")
                {
                    code = false;
                    msg = "企业名称为空";
                }
                else
                {
                    string sql = "select qymc,zh from I_M_QY where qymc ='" + qymc + "'";

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string qy = dt[0]["qymc"].ToString();
                        string zh = dt[0]["zh"].ToString();
                        msg = "该企业名称已注册，企业名称：" + qy + ";企业账号:" + zh;
                        code = false;
                    }
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }


        #endregion

    }
}