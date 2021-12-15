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
    public class RemoteUserController:Controller
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
        public void AddUser()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeRequest();
            string usercode = Request["usercode"].GetSafeRequest();
			string gzname = Request["gzname"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "", qybh="";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 如果用户表zh为空，赋成编号
                        IList<string> sqlszh = new List<string>();
                        sql = "update i_m_ry set zh=rybh where rybh='" + usercode + "' and (zh is null or zh='')";
                        sqlszh.Add(sql);
                        CommonService.ExecTrans(sqlszh);
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select lxbh,ryxm,qybh,zh from i_m_ry where rybh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["ryxm"];
                        qybh = dt[0]["qybh"];
                        // 查找人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_rylx where lxbh='" + dt[0]["lxbh"] + "'";
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
                        string companycode = dt[0]["zhdwbh"].GetSafeString();
                        string depcode = dt[0]["zhbmbh"].GetSafeString();
                        string rolecodelist = dt[0]["zhjsbh"];
 						postcode = dt[0]["gwbh"].GetSafeString();
                        string[] rolelist = rolecodelist.Split(',');
                        string rolecode = rolelist[0];
                       
                        // 判断账号是否已创建
                        /*
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }*/

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
						if(gzname=="劳资专管员")
                        {
                            code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                            if (!code)
                                break;
                        }else{
	                        if (rolelist.Length > 1)
	                        {
	                            for (int i = 1; i < rolelist.Length; i++)
	                            {
	                                UserService.AddUserRole(username, rolelist[i], out msg);
	                            }
	                        }
						}
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('" + usercode + "','" + yhzh + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate(),'R')";
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
                // 创建单位账号
                else if (usertype.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 如果用户表zh为空，赋成编号
                        IList<string> sqlszh = new List<string>();
                        sql = "update i_m_qy set zh=qybh where qybh='" + usercode + "' and (zh is null or zh='')";
                        sqlszh.Add(sql);
                        //更新企业远程编号
                        sql = "update i_m_qy set qybh_yc='" + usercode + "' where qybh='" + usercode + "'";
                        sqlszh.Add(sql);                        
                        CommonService.ExecTrans(sqlszh);

                        // 查找单位信息，获取单位类型、代码、名称
                        sql = "select lxbh,qymc,zh from i_m_qy where qybh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["qymc"];
                        // 查找企业类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_qylx where lxbh='" + dt[0]["lxbh"] + "'";
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
                        string rolecodelist = dt[0]["zhjsbh"];
                        string[] rolelist = rolecodelist.Split(',');
                        string rolecode = rolelist[0];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }
                        
                         string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string yhzh = msg;
                        if (rolelist.Length > 1)
                        {
                            for (int i = 1; i < rolelist.Length; i++)
                            {
                                UserService.AddUserRole(username, rolelist[i], out msg);
                            }
                        }

                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('" + usercode + "','" + yhzh + "',1,'','',getdate(),'Q')";
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
				// 创建内部人员账号
                else if (usertype.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 如果用户表zh为空，赋成编号
                        IList<string> sqlszh = new List<string>();
                        sql = "update i_m_nbry set zh=rybh where rybh='" + usercode + "' and (zh is null or zh='')";
                        sqlszh.Add(sql);
                        CommonService.ExecTrans(sqlszh);
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select lxbh,ryxm,zjzbh,zh from i_m_nbry where rybh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["ryxm"];
                        qybh = dt[0]["zjzbh"];          
                        // 查找人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_nbrylx where lxbh='" + dt[0]["lxbh"] + "'";
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
                        string companycode = dt[0]["zhdwbh"].GetSafeString();
                        string depcode = dt[0]["zhbmbh"].GetSafeString();
                        string rolecode = dt[0]["zhjsbh"].GetSafeString();
                        postcode = dt[0]["gwbh"].GetSafeString();

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                        {
                            code = false;
                            break;
                        }
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('" + usercode + "','" + msg + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate(),'N')";
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
		[Authorize]
        public void AddUserLZZGY()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "", qybh = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select zh,qybh from i_m_lzzgy_zh where zh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "该账户创建失败";
                            break;
                        }
                        username = usercode;
                        if (username == "")
                            username = usercode;
                        realname = gcmc + "劳资";
                        qybh = dt[0]["qybh"];
                        // 查找人员类型信息，获取默认单位、部门、角色                        



                        sql = "select * from h_rylx where lxbh='02'";
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
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
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

                        code = UserService.AddUserRole(username, Configs.GetLzzgyRole, out msg);
                        if (!code)
                            break;

                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + qybh + "','" + username + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";

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
        public void CreateAllUsers()
        {
            bool code = true;
            string msg = "";
            string errmsg = "";
            try
            {
                IList<IDictionary<string, string>> qylxs = CommonService.GetDataTable("select lxbh,zhjsbh,zhdwbh,zhbmbh,gwbh from h_qylx where sfcjzh=1");
                IList<IDictionary<string, string>> datas = CommonService.GetDataTable("select lxbh,qybh,qymc,lrrzh,lrrxm,lrsj from view_i_m_qy where yhzh is null or yhzh=''");
                foreach (IDictionary<string, string> row in datas)
                {
                    string lxbh = row["lxbh"];
                    var q = from e in qylxs where e["lxbh"] == lxbh select e;
                    if (q.Count() == 0)
                        continue;
                    var qylx = q.First();
                    string zhjsbh = qylx["zhjsbh"];
                    string zhdwbh = qylx["zhdwbh"];
                    string zhbmbh = qylx["zhbmbh"];
                    string gwbh = qylx["gwbh"];

                    string qymc = row["qymc"];
                    string qybh = row["qybh"];
                    string lrrzh = row["lrrzh"];
                    string lrrxm = row["lrrxm"];
                    DateTime lrsj = row["lrsj"].GetSafeDate();

                    if (!UserService.UserExists(qybh))
                    {
                        code = UserService.AddUser(zhdwbh, zhbmbh, qybh, qymc, zhjsbh, gwbh, Configs.DefaultPassword, out msg);
                        if (!code)
                        {
                            errmsg += "创建用户：" + qybh + "失败，原因：" + msg + "。";
                        }
                    }
                    if (code)
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + qybh + "','" + msg + "',1,'"+lrrzh+"','"+lrrxm+"',convert(datetime,'"+lrsj.ToString("yyyy-MM-dd HH:mm:ss")+"'))");
                        CommonService.ExecTrans(sqls);
                    }
                    code = true;


                }

                IList<IDictionary<string, string>> rylxs = CommonService.GetDataTable("select lxbh,zhjsbh,zhdwbh,zhbmbh,gwbh from h_rylx where sfcjzh=1");
                datas = CommonService.GetDataTable("select lxbh,qybh,rybh,ryxm,lrrzh,lrrxm,lrsj from view_i_m_ry where yhzh is null or yhzh=''");
                foreach (IDictionary<string, string> row in datas)
                {
                    string lxbh = row["lxbh"];
                    var q = from e in rylxs where e["lxbh"] == lxbh select e;
                    if (q.Count() == 0)
                        continue;
                    var qylx = q.First();
                    string zhjsbh = qylx["zhjsbh"];
                    string zhdwbh = qylx["zhdwbh"];
                    string zhbmbh = qylx["zhbmbh"];
                    string gwbh = qylx["gwbh"];

                    string ryxm = row["ryxm"];
                    string rybh = row["rybh"];
                    string qybh = row["qybh"];
                    string lrrzh = row["lrrzh"];
                    string lrrxm = row["lrrxm"];
                    DateTime lrsj = row["lrsj"].GetSafeDate();

                    if (!UserService.UserExists(rybh))
                    {
                        code = UserService.AddUser(zhdwbh, zhbmbh, rybh, ryxm, zhjsbh, gwbh, Configs.DefaultPassword, out msg);
                        if (!code)
                        {
                            errmsg += "创建用户：" + qybh + "失败，原因：" + msg + "。";
                        }
                    }
                    if (code)
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + rybh + "','" + msg + "',0,'" + lrrzh + "','" + lrrxm + "',convert(datetime,'" + lrsj.ToString("yyyy-MM-dd HH:mm:ss") + "'))");
                        CommonService.ExecTrans(sqls);
                    }
                    code = true;


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            Response.Write(JsonFormat.GetRetString(code, msg));
        }


        /// <summary>
        /// 用于人员备案管理中，为没有账号的用户新建账号
        /// </summary>
        public void AddUserWithDefaultRoles()
        {
            bool code = false;
            string msg = "";
            string usertype = Request["usertype"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();
            try
            {
                string sql = "", username = "", realname = "", postcode = "", qybh = "", lxbh="";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select lxbh,ryxm,qybh,zh from i_m_ry where rybh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["ryxm"];
                        qybh = dt[0]["qybh"];
                        // 查找人员类型信息，获取默认单位、部门、角色  
                        lxbh = dt[0]["lxbh"];
                        sql = "select * from h_rylx where lxbh='" + dt[0]["lxbh"] + "'";
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
                        /*
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }*/

                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        string userid = msg;
                        
                        // 根据RYBH在I_S_RY_RYZZ表中，找到该人员的RYLXBH, 然后为该人员添加相应的角色
                        sql = "select rylxbh from i_s_ry_ryzz where rybh='" + usercode + "'";
                        dt= CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            List<string> lxbhlist = new List<string>();
                            lxbhlist.Add(lxbh);
                            for (int i = 0; i < dt.Count; i++)
                            {
                                lxbhlist.Add(dt[i]["rylxbh"]);
                            }
                            if (lxbhlist.Count > 0)
                            {
                                sql = "select zhjsbh from h_rylx where lxbh in (" + DataFormat.FormatSQLInStr(lxbhlist) + ")";
                                dt = CommonService.GetDataTable(sql);
                                if (dt.Count > 0)
                                {
                                    List<string> newroles = new List<string>();
                                    for (int i = 0; i < dt.Count; i++)
                                    {
                                        newroles.Add(dt[i]["zhjsbh"]);
                                    }
                                    if (newroles.Count  > 0)
                                    {
                                        code = UserService.UpdateUserRole(username, "", newroles, out msg);
                                        if (!code)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        // 在i_m_qyzh表中插入账号信息
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + usercode + "','" + userid + "',0,'" + CurrentUser.UserName.GetSafeString() + "','" + CurrentUser.RealName.GetSafeString() + "',getdate())";
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
                // 创建单位账号
                else if (usertype.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找单位信息，获取单位类型、代码、名称
                        sql = "select lxbh,qymc,zh from i_m_qy where qybh='" + usercode + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["qymc"];
                        // 查找企业类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_qylx where lxbh='" + dt[0]["lxbh"] + "'";
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
                        /*
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }*/
                        string password = GlobalVariable.GetDefaultUserPass();
                        if (password == "")
                            password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                        code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + usercode + "','" + msg + "',1,'','',getdate())";
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