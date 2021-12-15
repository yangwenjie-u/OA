using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;

namespace BD.Jcbg.Web.Controllers
{
    public class ZJ_RemoteUserController : Controller
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
        #region 各种操作
        [Authorize]
        public void AddUserLWGS()
        {
            bool code = false;
            string msg = "";

            string zh = Request["usercode"].GetSafeString();
            string qymc = Request["qymc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号

                do
                {
                    // 查找人员信息，获取人员类型、代码、姓名
                    sql = "select lwgsbh from i_m_lwgs where lwgsbh='" + zh + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        msg = "该账户创建失败";
                        break;
                    }
                    username = zh;
                    if (username == "")
                        username = zh;
                    realname = qymc;

                    // 查找人员类型信息，获取默认单位、部门、角色                        

                    string companycode = Configs.GetConfigItem("lwgscompanycode");
                    string depcode = Configs.GetConfigItem("lwgsdepcode");
                    string rolecode = Configs.GetConfigItem("lwgsrole");
                    postcode = Configs.GetConfigItem("lwgsbmbh");
                    // 判断账号是否已创建
                    //sql = "select * from i_m_lwgs where yhzh='" + username + "'";
                    //dt = CommonService.GetDataTable(sql);
                    //if (dt.Count > 0)
                    //{
                    //    msg = "账号已经存在";
                    //    code = false;
                    //    break;
                    //}

                    string password = GlobalVariable.GetDefaultUserPass();
                    if (password == "")
                        password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                    code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                    if (!code)
                        break;
                    string yhzh = msg;
                    //code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                    //if (!code)
                    //    break;

                    sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + zh + "','" + yhzh + "',1,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                    IList<string> sqls = new List<string>();
                    sqls.Add(sql);
                    code = CommonService.ExecTrans(sqls, out msg);
                    if (code)
                    {
                        Session["USER_INFO_USERNAME"] = username;
                        Session["USER_INFO_PASSWORD"] = password;
                    }
                } while (false);

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
        /// 添加劳务员
        /// </summary>
        [Authorize]
        public void AddUserLWY()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select zh,qybh,lwgsbh from i_m_lzzgy_zh where zh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "该账户创建失败";
                            break;
                        }
                        username = usercode;
                        if (username == "")
                            username = usercode;
                        realname = gcmc + "-劳务员";
                        string lwgsbh = dt[0]["lwgsbh"];
                        // 查找劳务人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_rylx where lxbh='06'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        //sql = "select * from i_m_qyzh where yhzh='" + username + "' or qybh='" + username + "'";
                        //dt = CommonService.GetDataTable(sql);
                        //if (dt.Count > 0)
                        //{
                        //    msg = "账号已经存在";
                        //    code = false;
                        //    break;
                        //}

                        if(UserService.UserExists(username))
                        {
                            msg = "账号已经存在";
                            code = false;
                            break;
                        }

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
                        //code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                        //if (!code)
                        //    break;

                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + lwgsbh + "','" + yhzh + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                        string sql2 = "update I_M_LZZGY_ZH set usercode='" + yhzh + "' where zh='" + usercode + "'";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        sqls.Add(sql2);
                        code = CommonService.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            Session["USER_INFO_USERNAME"] = username;
                            Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);


                }
                else
                {
                    code = false;
                    msg = "无效的用户类型";
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
        /// 添加银行
        /// </summary>
        [Authorize]
        public void AddUserYH()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            string yhmc = Request["yhmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建银行
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select zh  from info_yh where zh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "该账户创建失败";
                            break;
                        }
                        username = usercode;
                        if (username == "")
                            username = usercode;
                        realname = yhmc ;
                        string zh = dt[0]["zh"];
                        // 查找劳务人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_yhlx where lxbh='20'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到银行类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        //sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        //dt = CommonService.GetDataTable(sql);
                        //if (dt.Count > 0)
                        //{
                        //    msg = "账号已经存在";
                        //    code = false;
                        //    break;
                        //}

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
                        //code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                        //if (!code)
                        //    break;

                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + zh + "','" + yhzh + "',1,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                        string sql2 = "update INFO_YH set usercode='" + yhzh + "' where zh='" + usercode + "'";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        sqls.Add(sql2);
                        code = CommonService.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            Session["USER_INFO_USERNAME"] = username;
                            Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);


                }
                else
                {
                    code = false;
                    msg = "无效的用户类型";
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
        /// 创建政府账号
        /// </summary>
        [Authorize]
        public void AddUserZF()
        {
            bool code = false;
            string msg = "";
            string usercode = Request["usercode"].GetSafeString();
            string zhmc = Request["zhmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "";
                IList<IDictionary<string, string>> dt = null;
                // 政府账号

                do
                {
                    // 查找人员信息，获取人员类型、代码、姓名
                    sql = "select zh  from i_m_zfzh where zh='" + usercode + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        msg = "该账户创建失败";
                        break;
                    }
                    username = usercode;
                    if (username == "")
                        username = usercode;
                    realname = zhmc;
                    string zh = dt[0]["zh"];
                    // 查找劳务人员类型信息，获取默认单位、部门、角色                        
                    sql = "select * from h_zflx where lxbh='22'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        msg = "找不到政府类型记录";
                        break;
                    }
                    // 不用创建账号，返回
                    if (!dt[0]["sfcjzh"].GetSafeBool())
                    {
                        code = true;
                        break;
                    }
                    string companycode = dt[0]["zhdwbh"];
                    string depcode = dt[0]["zhbmbh"];
                    string rolecode = dt[0]["zhjsbh"];
                    postcode = dt[0]["gwbh"];
                    // 判断账号是否已创建
                    //sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                    //dt = CommonService.GetDataTable(sql);
                    //if (dt.Count > 0)
                    //{
                    //    msg = "账号已经存在";
                    //    code = false;
                    //    break;
                    //}

                    string password = GlobalVariable.GetDefaultUserPass();
                    if (password == "")
                        password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                    code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                    if (!code)
                        break;
                    string yhzh = msg;

                    sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + zh + "','" + yhzh + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
                    string sql2 = "update i_m_zfzh set usercode='" + yhzh + "' where zh='" + usercode + "'";
                    IList<string> sqls = new List<string>();
                    sqls.Add(sql);
                    sqls.Add(sql2);
                    code = CommonService.ExecTrans(sqls, out msg);
                    if (code)
                    {
                        Session["USER_INFO_USERNAME"] = username;
                        Session["USER_INFO_PASSWORD"] = password;
                    }
                } while (false);

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
        public void DeleteUser()
        {
            bool code = false;
            string msg = "";
            string usercode = Request["usercode"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {

            }
            catch (Exception e)
            {

            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void AddUserQY()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString(); //qybh_yc
            string gzname = Request["gzname"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "", qybh = "";
                IList<IDictionary<string, string>> dt = null;

                // 创建单位账号
                if (usertype.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找单位信息，获取单位类型、代码、名称
                        sql = "select lxbh,qymc,zh,qybh from i_m_qy where qybh_yc='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业记录";
                            break;
                        }
                        qybh = dt[0]["qybh"];
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = qybh;
                        realname = dt[0]["qymc"];

                        // 查找企业类型信息，获取默认单位、部门、角色    
                        string lxbh = "00";
                        if (string.IsNullOrEmpty(dt[0]["lxbh"]))
                            lxbh = "00";
                        sql = "select * from h_qylx where lxbh='" + lxbh + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        if (UserService.UserExists(username))
                        {
                            msg = "账号已经存在于用户系统";
                            code = false;
                            break;
                        }
                        sql = "select * from i_m_qyzh where yhzh='" + qybh + "' or qybh='"+qybh+"'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在,请联系技术人员";
                            break;
                        }
                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.NumberAndChar, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
                        sql = "select qylxbh from I_S_QY_QYZZ where qybh='" + usercode + "' group by qylxbh";
                        IList<IDictionary<string, string>> qylxbhlist=CommonService.GetDataTable(sql);
                        for (int i = 0; i < qylxbhlist.Count;i++ )
                        {
                            sql = "select * from h_qylx where lxbh='" + qylxbhlist[i]["qylxbh"] + "'";
                            dt = CommonService.GetDataTable(sql);
                            if(dt.Count>0)
                            {
                                rolecode = dt[0]["zhjsbh"];
                                code = UserService.AddUserRole(username, rolecode, out msg);
                                if (!code)
                                    break;
                            }                   
                        }
                           
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + qybh + "','" + yhzh + "',1,'','',getdate())";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        code = CommonService.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            Session["USER_INFO_USERNAME"] = username;
                            Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);
                }
                else
                {
                    code = false;
                    msg = "无效的用户类型";
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

        #endregion
    }
}