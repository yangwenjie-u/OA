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
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;

namespace BD.Jcbg.Web.Controllers
{
    public class RyController:Controller
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

        public ActionResult Rysqsp()
        {
            return View();
        }

        /// <summary>
        /// 录入界面上传人员签名
        /// </summary>
        [Authorize]
        public ActionResult LrSetRyqm()
        {
            string buttons = Server.UrlEncode("保存|TJ| | ");
            string title = Server.UrlEncode("签名设置");
            string sql = "select rybh from i_m_ry where rybh in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {

                string url = "/datainput/Index?zdzdtable=zdzd_jc&t1_tablename=i_m_ry&&t1_pri=rybh&t1_title=" + title + "&button=" + buttons + "&rownum=1&LX=Q&jydbh=" + dt[0]["rybh"];
                return new RedirectResult(url);
            }
            else
            {
                return new RedirectResult("/user/setsign");
                //return null;
            }
        }
		[Authorize]
        public ActionResult TSLR()
        {
            return View();
        }

        /// <summary>
        /// 录入界面上传人员招聘
        /// </summary>
        [Authorize]
        public ActionResult LrSetRyzp()
        {
            string buttons = Server.UrlEncode("保存|TJ| | ");
            string title = Server.UrlEncode("照片设置");
            string sql = "select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {

                string url = "/datainput/Index?zdzdtable=zdzd_jc&t1_tablename=i_m_ry&&t1_pri=rybh&t1_title=" + title + "&button=" + buttons + "&rownum=1&LX=Z&jydbh=" + dt[0]["qybh"];
                return new RedirectResult(url);
            
            }
            else
                return null;
        }

        /// <summary>
        /// 注册人员账号查看个人信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SelfInfo()
        {
            return View();
        }

        [Authorize]
        public ActionResult Ryzzsp()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取未审批的人员申请数量
        /// </summary>
        [Authorize]
        public void GetRysqsl()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from View_I_M_RY where sptg=0");
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
        /// 注册时校验
        /// </summary>
        public void CheckRegister()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string idno = Request["idno"];
                string yzm = Request["yzm"].GetSafeString();
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

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from i_m_ry where sfzhm='" + idno + "'");
                        if (dt[0]["sum"].GetSafeInt() > 0)
                            msg = "身份证号码已存在";
                        else
                        {
                            if (!string.IsNullOrEmpty(username))
                            {
                                dt = CommonService.GetDataTable("select count(*) as sum from (( select zh from i_m_ry where zh='" + username + "') union all ( select zh from i_m_qy where zh='" + username + "') union all ( select yhzh from i_m_qyzh where yhzh='" + username + "')) as t1");
                                if (dt[0]["sum"].GetSafeInt() > 0)
                                    msg = "登录账号已存在";
                                else
                                {
                                    if (UserService.UserExists(username))
                                        msg = "账号已存在";
                                }
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
        /// 修改手机号时进行校验
        /// </summary>
        public void CheckSJHMRegister()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string yzm = Request["yzm"].GetSafeString();
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
        /// 注册时校验
        /// </summary>
        public void CheckRegister2()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string idno = Request["idno"];
                string yzm = Request["yzm"].GetSafeString();
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

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from (( select zh from i_m_ry where zh='" + username + "') union all ( select zh from i_m_qy where zh='" + username + "') union all ( select yhzh from i_m_qyzh where yhzh='" + username + "')) as t1");

                        if (dt[0]["sum"].GetSafeInt() > 0)
                            msg = "登录账号已存在";
                        else
                        {
                            if (UserService.UserExists(username))
                                msg = "账号已存在";
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
        /// 根据当前用户账号获取人员编号
        /// </summary>
        [Authorize]
        public void GetRybh()
        {
            string msg = "";
            bool code = false;
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qybh  as rybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                if (dt.Count > 0)
                {
                    code = true;
                    msg = dt[0]["rybh"];
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
        public void updatesjhm()
        {
            bool code = true;
            string msg = "";
            string sjhm = Request["sjhm"].GetSafeRequest();
            string username = CurrentUser.UserName;
            try
            {
                IList<string> sqls = new List<string>();
                string sql = "Update I_M_RY SET SJHM='" + sjhm + "' where zh='" + username + "';";
                sqls.Add(sql);
                sql = "select rylx,rybh from View_GC_RY_ZH where zh='" + username + "';";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {

                }
                else
                {
                    string rylx = dt[0]["rylx"].ToString();
                    string rybh = dt[0]["rybh"].ToString();
                    string tbname = "";
                    switch (rylx)
                    {
                        case "01": tbname = "i_s_gc_jsry"; break;
                        case "02": tbname = "i_s_gc_sgry"; break;
                        case "03": tbname = "i_s_gc_jlry"; break;
                        case "04": tbname = "i_s_gc_sjry"; break;
                        case "05": tbname = "i_s_gc_kcry"; break;
                        case "06": tbname = "i_s_gc_tsry"; break;
                        default: break;
                    }
                    sql = "Update " + tbname + " set DH='" + sjhm + "'where rybh='" + rybh + "';";
                    sqls.Add(sql);
                }
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

        public void GetRYinfo()
        {
            string rybh = Request["rybh"].GetSafeRequest();
            string yxrq = Request["yxrq"].GetSafeRequest();
            bool code = true; string msg = "";
            try
            {
                string sql = "select rybh,gcmc from View_GC_RY_QYRYCK where rybh='" + rybh + "' and zt in (select BH from H_GCZT where xssx <(select xssx from H_GCZT where BH='JGYS'))  ;";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    if (yxrq == "")
                    {
                        code = false;
                        msg = "该证书有效日期有误";
                    }
                    else
                    {
                        int t = yxrq.GetSafeDate(DateTime.Now).CompareTo(Convert.ToDateTime(DateTime.Now.ToString("yy/MM/dd")));
                        if (t == -1)
                        {
                            code = false;
                            msg = "该证书已过期";
                        }
                    }

                }
                else
                {
                    code = false;
                    string gcmc = dt[0]["gcmc"].ToString();
                    msg = "该人员已经在岗！所在工程:" + gcmc;
                }

                dt = CommonService.GetDataTable("select * from I_S_QY_RY where RYBH='" + rybh + "'");
                if (dt.Count > 0)
                {
                    code = false;
                    msg = "该人员目前属于企业技术管理岗位，不能兼职工程在岗！";
                }
                dt = CommonService.GetDataTable("select SFYX from I_M_RY where RYBH='" + rybh + "'");
                if (dt.Count > 0)
                {
                    if (dt[0]["sfyx"].GetSafeBool(false) == false)
                    {
                        code = false;
                        msg = "该人员目前尚未审批通过，不能工程在岗！";
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




        [Authorize]
        public void GetRYinfo2()
        {
            string rybh = Request["rybh"].GetSafeRequest();
            string yxrq = Request["yxrq"].GetSafeRequest();
            bool code = true; string msg = "";
            try
            {
                string sql = "select * from View_GC_RY_QYRYCK where rybh='" + rybh + "' and zt in (select BH from H_GCZT where xssx <(select xssx from H_GCZT where BH='JGYS'))  ;";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    if (yxrq == "")
                    {
                        code = false;
                        msg = "该证书有效日期有误";
                    }
                    else
                    {
                        int t = yxrq.GetSafeDate(DateTime.Now).CompareTo(Convert.ToDateTime(DateTime.Now.ToString("yy/MM/dd")));
                        if (t == -1)
                        {
                            code = false;
                            msg = "该证书已过期";
                        }
                    }

                    dt = CommonService.GetDataTable("select * from I_S_QY_RY where RYBH='" + rybh + "'");
                    if (dt.Count > 0)
                    {
                        code = false;
                        msg = "该人员目前属于企业技术管理岗位，不能兼职工程在岗！";
                    }
                    dt = CommonService.GetDataTable("select SFYX from I_M_RY where RYBH='" + rybh + "'");
                    if (dt.Count > 0)
                    {
                        if (dt[0]["sfyx"].GetSafeBool(false) == false)
                        {
                            code = false;
                            msg = "该人员目前尚未审批通过，不能工程在岗！";
                        }
                    }
                }
                else
                {




                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select YXJGS from I_S_RY_JG where RYBH='" + rybh + "' order by YXJGS desc ");
                    int temnu = 1;
                    if (dt1.Count > 0)
                        temnu = dt1[0]["yxjgs"].GetSafeInt(1);
                    bool yxjg = false;
                    dt1 = CommonService.GetDataTable("select jgzt from I_M_RY where RYBH='" + rybh + "'");
                    if (dt1.Count > 0)
                        yxjg = dt1[0]["jgzt"].GetSafeBool(false);
                    if (yxjg)
                    {
                        code = true;
                    }
                    else
                    {

                        if (temnu > dt.Count)
                        {
                            code = true;
                            /*
                            code = false;
                            string gcmc = dt[0]["gcmc"].ToString();
                            msg = "该人员已经在岗！所在工程:" + gcmc;*/
                        }
                        else
                        {

                            string sjbmc = dt[0]["sjbmc"].GetSafeString();

                            string jzfl = "";
                            string jzmj = "";
                            string gw = "";
                            if (sjbmc == "i_s_gc_sgry")
                            {
                                jzfl = dt[0]["jzfl"].GetSafeString();
                                jzmj = dt[0]["jzmj"].GetSafeString();
                                if (jzfl == "工业建筑")
                                {
                                    if (jzmj.GetSafeDecimal() < 20000)
                                    {
                                        if (dt.Count >= 2)
                                        {
                                            code = false;
                                            msg = "该人员已接有两个工程，不能继续兼岗！";
                                        }
                                    }
                                    else
                                    {
                                        code = false;
                                        msg = "该人员已接有建筑面积大于2万的工程，不能继续兼岗！";
                                    }
                                }
                                else if (jzfl == "公共建筑")
                                {
                                    code = false;
                                    msg = "该人员已接有公共建筑工程，不能继续兼岗！";
                                }
                                else //其他类型
                                {
                                    if (dt.Count >= 1)
                                    {
                                        code = false;
                                        string gcmc = dt[0]["gcmc"].ToString();
                                        msg = "该人员已在工程[" + gcmc + "]，不能继续兼岗！";
                                    }
                                }
                            }
                            else if (sjbmc == "i_s_gc_jlry") //监理人员
                            {
                                jzfl = dt[0]["jzfl"].GetSafeString();
                                gw = dt[0]["gw"].GetSafeString();
                                if (gw == "总监")
                                {
                                    if (dt.Count >= 2)
                                    {
                                        code = false;
                                        msg = "该人员已接有两个工程，不能继续兼岗！";
                                    }
                                }
                                else if (gw == "安装监理工程师")
                                {
                                    if (dt.Count >= 3)
                                    {
                                        code = false;
                                        msg = "该人员已接工程数量为最大数，不能继续兼岗！";
                                    }
                                }
                                else if (gw == "安装监理员")
                                {
                                    code = true;
                                }
                                else //其他类型
                                {
                                    if (dt.Count >= 1)
                                    {
                                        code = false;
                                        string gcmc = dt[0]["gcmc"].ToString();
                                        msg = "该人员已在工程[" + gcmc + "]，不能继续兼岗！";
                                    }
                                }
                                if (jzfl == "公共建筑")
                                {
                                    if (dt.Count >= 1)
                                    {
                                        code = false;
                                        string gcmc = dt[0]["gcmc"].ToString();
                                        msg = "该人员已在工程[" + gcmc + "]，不能继续兼岗！";
                                    }
                                }
                            }




                        }
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

        /// <summary>
        /// 获取人员照片
        /// </summary>
        public void GetRyZp()
        {
            try
            {
                string ryid = RouteData.Values["id"].GetSafeString();

                IList<IDictionary<string,string>> datas = CommonService.GetDataTable("select zp from i_m_ry where rybh='" + ryid + "'");

                if (datas.Count > 0)
                {
                    IDictionary<string, string> row = datas[0];
                    string zpstr = row["zp"];
                    if (zpstr != "")
                    {
                        byte[] arrzp = Convert.FromBase64String(zpstr);
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "image/jpeg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("pics.jpg"));
                        Response.BinaryWrite(arrzp);
                    }
                }
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
        /// 获取人员照片
        /// </summary>
        [Authorize]
        public void GetWgRyZp()
        {
            try
            {
                string ryid = RouteData.Values["id"].GetSafeString();

                IList<IDictionary<string, string>> datas = CommonService.GetDataTable("select zp from i_m_ry_info where rybh='" + ryid + "'");

                if (datas.Count > 0)
                {
                    IDictionary<string, string> row = datas[0];
                    string zpstr = row["zp"];
                    if (zpstr != "")
                    {
                        byte[] arrzp = Convert.FromBase64String(zpstr);
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "image/jpeg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("pics.jpg"));
                        Response.BinaryWrite(arrzp);
                    }
                }
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
        /// 人员账号，查看个人信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetRySelfInfo()
        {
            bool code = true; 
            string msg = "";
            IDictionary<string, string> row = new Dictionary<string, string>();
            try
            {
                string sql = "select a.*,b.qymc from i_m_ry a left outer join i_m_qy b on a.qybh=b.qybh where rybh in (select qybh from i_m_qyzh where yhzh='"+CurrentUser.UserName+"')";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);

                row = dt[0];
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                
            }
            return Json(new { code = code ? "0" : "1", msg = msg, data = row });
        }
        #endregion

        #region 更新数据
        /// <summary>
        /// 企业申请审批
        /// </summary>
        [Authorize]
        public void SetRysqsp()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["usercode"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string spbz = Request["spbz"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set sptg=1,sfyx=" + checkoption + ",spbz='"+spbz+"' where rybh='" + rybh + "'");
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
        public void UpdateUserRole()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                string username = "", usercode = "";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select yhzh,zhlx from i_m_qyzh where qybh='" + rybh + "'");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "找不到人员账号记录，更新角色失败，请联系管理员";
                }
                else 
                {
                    usercode = dt[0]["yhzh"];
                    string rylx = dt[0]["zhlx"];
                    if (rylx.Equals("q", StringComparison.OrdinalIgnoreCase))
                    {
                        dt = CommonService.GetDataTable("select zh from i_m_qy where qybh='" + rybh + "'");
                        if (dt.Count == 0)
                        {
                            code = false;
                            msg = "找不到企业记录，更新角色失败，请联系管理员";
                        }
                        else
                            username = dt[0]["zh"].GetSafeString();
                    } 
                    else if (rylx.Equals("r", StringComparison.OrdinalIgnoreCase))
                    {
                        dt = CommonService.GetDataTable("select zh from i_m_ry where rybh='" + rybh + "'");
                        if (dt.Count == 0)
                        {
                            code = false;
                            msg = "找不到人员记录，更新角色失败，请联系管理员";
                        }
                        else
                            username = dt[0]["zh"].GetSafeString();
                    }
                    else if (rylx.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        dt = CommonService.GetDataTable("select zh from i_m_nbry where rybh='" + rybh + "'");
                        if (dt.Count == 0)
                        {
                            code = false;
                            msg = "找不到内部人员记录，更新角色失败，请联系管理员";
                        }
                        else
                            username = dt[0]["zh"].GetSafeString();
                    }
                    else
                    {
                        code = false;
                        msg = "无效的账号类型："+rylx;
                    }
                    if (code)
                    {
                        if (rylx.Equals("q", StringComparison.OrdinalIgnoreCase))
                            dt = CommonService.GetDataTable("select zhjsbh from h_qylx where lxbh in (select qylxbh from I_S_QY_QYZZ where qybh='" + rybh + "' and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_qy where qybh='" + rybh + "')");
                        else if (rylx.Equals("r", StringComparison.OrdinalIgnoreCase))
                            dt = CommonService.GetDataTable("select zhjsbh from h_rylx where lxbh in (select rylxbh from i_s_ry_ryzz where rybh='" + rybh + "' and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_ry where rybh='" + rybh + "')");
                        else
                            dt = CommonService.GetDataTable("select zhjsbh from H_NBRYLX where lxbh in (select lxbh from I_M_NBRY where rybh='" + rybh + "' and sptg=1 and sfyx=1)");

                        IList<string> roleCodes = new List<string>();
                        foreach (IDictionary<string, string> row in dt)
                        {
                            string str = row["zhjsbh"].GetSafeString();
                            if (str.Length == 0)
                                continue;
                            string[] arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string str1 in arr)
                                roleCodes.Add(str1);
                        }
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
        /// <summary>
        /// 添加人员角色
        /// </summary>
        [Authorize]
        public void UpdateUserRole2()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                string gcbh = Request["gcbh"].GetSafeRequest();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh,zhjsbh from View_GC_RY_QYRYCK where rybh='" + rybh + "' and gcbh='" + gcbh + "';");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "找不到人员记录，更新角色失败，请联系管理员";
                }
                else
                {
                    string zhjsbh = dt[0]["zhjsbh"].GetSafeString();
                    string zh = dt[0]["zh"].GetSafeString();
                    if (zh != "" && zhjsbh != "" && zhjsbh != null)
                    {

                        if (UserService.AddUserRole(zh, zhjsbh, out msg))
                            code = msg == "";
                        else
                        {
                            code = false;
                            msg = "添加权限失败!";
                        }
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
        /// <summary>
        /// 删除人员角色
        /// </summary>
        [Authorize]
        public void DelUserRole()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                string gcbh = Request["gcbh"].GetSafeRequest();
                string zhjsbh = Request["zhjsbh"].GetSafeRequest();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh from I_M_RY where rybh='" + rybh + "';");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "找不到人员记录，删除角色失败，请联系管理员";
                }
                else
                {
                    //string zhjsbh = dt[0]["zhjsbh"].GetSafeString();
                    string zh = dt[0]["zh"].GetSafeString();
                    if (zh != "" && zhjsbh != "" && zhjsbh != null)
                    {

                        if (UserService.DelUserRole(zh, zhjsbh, out msg))
                            code = msg == "";
                        else
                        {
                            code = false;
                            msg = "添加权限失败!";
                        }
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

        [Authorize]
        public void DeleteUserRole()
        {
            bool code = true;
            string msg = "";
            try
            {

                int recid = Request["id"].GetSafeInt();
                string sjbmc = Request["sjbmc"].GetSafeString();

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh,zhjsbh from View_GC_RY_QYRYCK where recid='" + recid + "' and sjbmc='" + sjbmc + "'");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "找不到人员记录，更新角色失败，请联系管理员";
                }
                else
                {
                    string zhjsbh = dt[0]["zhjsbh"].GetSafeString();
                    string zh = dt[0]["zh"].GetSafeString();
                    if (zh != "" && zhjsbh != "" && zhjsbh != null)
                    {

                        if (UserService.DeleteUserRole(zh, zhjsbh, out msg))
                            code = msg == "";
                        else
                        {
                            code = false;
                            msg = "删除角色失败!";
                        }
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
        /// <summary>
        /// 更新工程人员角色
        /// </summary>
        [Authorize]
        public void UpdateGCUserRole()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();


                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from view_gc_ry where gcbh='" + gcbh + "'");
                for (int i = 0; i < dt.Count; i++)
                {
                    string username = "", usercode = "";
                    IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select zh from i_m_ry where rybh='" + dt[i]["rybh"] + "'");
                    if (dt2.Count == 0 || dt2[0]["zh"] == "")
                    {
                    }
                    else
                        username = dt2[0]["zh"];

                    if (username != "" || usercode != "")
                    {
                        string tem = "";
                        dt2 = CommonService.GetDataTable("select zhjsbh from h_RYLX where lxmc like '%" + dt[i]["rylxmc"] + "%' ");
                        IList<string> roleCodes = new List<string>();
                        foreach (IDictionary<string, string> row in dt2)
                        {
                            string[] rolelist = row["zhjsbh"].GetSafeString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var role in rolelist)
                            {
                                roleCodes.Add(role);
                            }
                            
                            tem += row["zhjsbh"].GetSafeString() + ";";
                        }
                        UserService.UpdateUserRoleNotDelet(username, usercode, roleCodes, out msg);
                        //msg = "name:" + username + ".usercode:" + usercode + "." + tem;
                        //code = msg == "";
                    }

                }

                /*
                string username = "", usercode = "";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh from i_m_ry where rybh='" + rybh + "'");
                if (dt.Count == 0 || dt[0]["zh"] == "")
                {
                    //dt = CommonService.GetDataTable("select yhzh as zh from i_m_qyzh where yhzh='" + rybh + "'");
                    //if (dt.Count == 0)
                    //{
                    code = false;
                    msg = "找不到人员记录，更新角色失败，请联系管理员";
                    //}
                }
                else
                    username = dt[0]["zh"];

                if (code)
                {
                    if (username != "" || usercode != "")
                    {
                        dt = CommonService.GetDataTable("select zhjsbh from h_rylx where lxbh in (select rylxbh from i_s_ry_ryzz where rybh='" + rybh + "' and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_ry where rybh='" + rybh + "')");
                        IList<string> roleCodes = new List<string>();
                        foreach (IDictionary<string, string> row in dt)
                            roleCodes.Add(row["zhjsbh"].GetSafeString());
                        UserService.UpdateUserRole(username, usercode, roleCodes, out msg);
                        code = msg == "";
                    }
                }*/
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
        /// 人员资质审批
        /// </summary>
        [Authorize]
        public JsonResult CheckRyzz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("update I_S_RY_RYZZ set sptg=1,sfyx=" + checkoption + " where recid='"+recid+"'");
                code = CommonService.ExecTrans(sqls);
                if (code && checkoption == 1)
                {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh from i_m_ry where rybh in (select rybh from I_S_RY_RYZZ where recid='" + recid + "')");
                    if (dt.Count > 0)
                    {
                        string username = dt[0]["zh"];
                        dt = CommonService.GetDataTable("select zhjsbh from h_rylx where lxbh in (select rylxbh from i_s_ry_ryzz where rybh in (select rybh from I_S_RY_RYZZ where recid='" + recid + "') and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_ry where rybh in (select rybh from I_S_RY_RYZZ where recid='" + recid + "'))");
                        IList<string> roleCodes = new List<string>();
                        foreach (IDictionary<string, string> row in dt)
                        {
                            string[] rolelist = row["zhjsbh"].GetSafeString().Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var role in rolelist)
                            {
                                roleCodes.Add(role);
                            }
                            
                        }
                            
                        UserService.UpdateUserRoleNotDelet(username, "", roleCodes, out msg);
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
                
            }
            return Json(new { code = (code ? "0" : "1"), msg = msg});
        }


        /// <summary>
        /// 提交人员审批
        /// </summary>
        [Authorize]
        public void SubmitRyzz()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set sptg=0,sfyx=0,sbsp=1,remark=''  where rybh in (" + rybh.FormatSQLInStr() + ")");
                sqls.Add("update i_s_ry_ryzz set sptg=0,sfyx=0  where rybh in (" + rybh.FormatSQLInStr() + ")");

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
        /// 审批人员资质
        /// </summary>
        [Authorize]
        public void CheckRyzz2()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update i_m_ry set sptg=1,sfyx=0,sbsp=0,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',REMARK='" + reason + "' where rybh='" + rybh + "'");
                }
                else
                {
                    //string sql = "select zh from i_m_qy where rybh='" + rybh + "')";
                    //IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    //if (dt.Count == 0 ) //没有人员账号
                    //    msg = "";
                    //else
                    //{
                    //string username = dt[0]["zh"];
                    //sql = "select zhjsbh from h_rylx where lxbh=(select rylxbh from i_s_ry_ryzz where rybh='" + rybh + "')";
                    //dt = CommonService.GetDataTable(sql);
                    //// 需要创建角色
                    //if (dt.Count > 0 && dt[0]["zhjsbh"] != "")
                    //{
                    //    string roleid = dt[0]["zhjsbh"];
                    //    if (UserService.AddUserRole(username, roleid, out msg))
                    //    {
                    //        msg = "";

                    //    }
                    //    else
                    //        msg = "创建企业角色失败!";
                    //}
                    if (msg == "")
                    {
                        sqls.Add("update i_m_ry set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where rybh='" + rybh + "'");
                        sqls.Add("update i_s_ry_ryzz set sptg=1,sfyx=1 where rybh='" + rybh + "'");
                    }
                    // }
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

        public void CheckRyzzS()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybhs = Request["rybhs"].GetSafeString();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update i_m_ry set sptg=1,sfyx=0,sbsp=0,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',REMARK='" + reason + "' where rybh in (" + rybhs + ")");
                }
                else
                {

                    if (msg == "")
                    {
                        sqls.Add("update i_m_ry set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where rybh in (" + rybhs + ")");
                        sqls.Add("update i_s_ry_ryzz set sptg=1,sfyx=1 where rybh in (" + rybhs + ")");
                    }
                    // }
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


        public void setRYJG()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybhs = Request["rybhs"].GetSafeString();
                IList<string> sqls = new List<string>();
                string sql = "update I_M_RY set jgzt=1 where rybh in (" + rybhs + ")";
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

        public void cancelryjg()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybhs = Request["rybhs"].GetSafeString();
                IList<string> sqls = new List<string>();
                string sql = "update I_M_RY set jgzt=0 where rybh in (" + rybhs + ")";
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


        [Authorize]
        public void UpdateKF()
        {
            bool code = true;
            string msg = "";
            try
            {
                string type = Request["type"].GetSafeString();
                int recid = Request["id"].GetSafeInt();
                IList<string> sqls = new List<string>();

                if (type == "1")
                {
                    string sql = "update I_S_RY_JF_List set KSZT=1, KSTotal=Total,LRRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid + " and Total>=6 and Total<10";
                    sqls.Add(sql);
                    sql = "update I_S_RY_JF_List set KSZT=2, KSTotal=Total,LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid + " and Total>=10";
                    sqls.Add(sql);
                }
                else
                {
                    string sql = "update I_S_RY_JF_List set KSZT=-1, KSTotal=Total,LRRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid;
                    sqls.Add(sql);
                }
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



        #endregion
    }
}