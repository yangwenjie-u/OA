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
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.IBll;

namespace BD.Jcbg.Web.Controllers
{
    public class WorkFlowHelpController : Controller
    {
		#region 服务
		
		private BD.Jcbg.IBll.ICommonService _commonService = null;
		private BD.Jcbg.IBll.ICommonService CommonService
		{
			get
			{
				try
				{
					if (_commonService == null)
					{
						IApplicationContext webApplicationContext = ContextRegistry.GetContext();
						_commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
					}
				}
				catch (Exception e)
				{
					SysLog4.WriteLog(e);
				}
				return _commonService;
			}
		}

        

        #endregion

        #region 获取各种列表
        [Authorize]
		public void GetCompanySupplies()
		{
			IDictionary<string, string> ret = new Dictionary<string, string>();
			try
			{
				string sql = "select * from CompanySupplies where recid=" + Request["recid"].GetSafeInt();
				IList<IDictionary<string, string>> table = CommonService.GetDataTable(sql);
				if (table.Count > 0)
					ret = table[0];

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				Response.ContentType = "text/plain";
				Response.Write(JsonFormat.GetRetString(ret.Count > 0 ? 0 : 1, ret)); ;
				Response.End();
			}
		}


        public void GetProjectList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                 if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                string strwhere = "";
                string sql = string.Format("select zhlx from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string zhlx = dt[0]["zhlx"];
                    if (zhlx.Equals("Q", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_qy_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                    else if (zhlx.Equals("R", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_ry_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                }
                if (key != "")
                {
                    strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                }
                //string sql = " from View_I_M_GC a where ZT not in ('LR','JDBG','BA') " + strwhere + " and (','+a.tjjdy+',' like '%," + CurrentUser.UserName + ",%' or ','+a.aqjdy+',' like '%," + CurrentUser.UserName + ",%' or ','+a.azjdy+',' like '%," + CurrentUser.UserName + ",%' or ','+a.jdgcs+',' like '%," + CurrentUser.UserName + ",%' or exists(select * from View_GC_QY_ZH x where x.gcbh=a.gcbh and x.zh='" + username + "')or exists(select * from View_GC_RY_ZH x where x.gcbh=a.gcbh and x.zh='" + username + "')) order by gcbh desc";
                sql = " from View_I_M_GC a where ZT not in ('LR') " + strwhere +  " order by gcbh desc";
                if (type == "lxgc")
                {
                    sql = "select GCBH,GCMC,ZJDJH " + sql;
                    CommonService.GetPageData(sql, 1, 1, out totalcount);
                    pagesize = totalcount;

                }
                else
                {
                    sql = "select * " + sql;
                }
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        /// <summary>
        /// 设备备案列表
        /// </summary>
        public void GetBAList()
        {

            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";

                //if (key != "")
                //{
                //    strwhere += " and BeiAnICP like '%" + key + "%' ";

                //}
                if (type != "" && key != "")
                {
                    strwhere += " and " + type + " like '%" + key + "%' ";
                }
                string sql = "select * from SB_BA where BeiAnStatus=1 " + strwhere;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        /// <summary>
        /// 特种作业人员列表
        /// </summary>
        public void GetPesonList()
        {

            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";

                //if (key != "")
                //{
                //    strwhere += " and TrueName like '%" + key + "%' ";
                //}
                if (type != "" && key != "")
                {
                    strwhere += " and " + type + " like '%" + key + "%' ";
                }
                string sql = "select * from dbo.SB_Person where 1=1 " + strwhere;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        /// <summary>
        /// 安装单位列表
        /// </summary>
        public void GetAZDWList()
        {
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";

                if (key != "")
                {
                    strwhere += " and DWMC like '%" + key + "%' ";
                }
                string sql = "select * from dbo.H_AZDW where 1=1 " + strwhere;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        /// <summary>
        /// 获取企业列表，带企业用户code
        /// </summary>
        /// <returns></returns>

        public JsonResult GetQylist()
        {
            string key = Request["key"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";

                if (key != "")
                {
                    strwhere += " and qymc like '%" + key + "%' ";
                }
                string sql = "select a.*,b.yhzh from i_m_qy a inner join i_m_qyzh b on a.qybh=b.qybh where 1=1 " + strwhere;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

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
            return Json(new { code = code ? "0" : "1", msg = msg, total = totalcount, rows = datas });
        }
       


        #region 请假

        /// <summary>
        /// 获取质监站所有内部员工
        /// </summary>
        /// <returns></returns>
        private IList<RemoteUserService.VUser> GetAllUsers()
        {
            string strExcludeDeps = Configs.GetConfigItem("zcqybm") + "," + Configs.GetConfigItem("zcrybm");
            string strExcludeUsers = "wzzjzadmin";
            IList<RemoteUserService.VUser> users = new List<RemoteUserService.VUser>();
            IList<RemoteUserService.VUser> all_users = Remote.UserService.Users;

            if (all_users != null && all_users.Count > 0)
            {
                foreach (var u in all_users)
                {
                    if (("," + strExcludeDeps + ",").IndexOf("," + u.DEPCODE + ",") > -1)
                    {
                        continue;
                    }
                    else if (("," + strExcludeUsers + ",").IndexOf("," + u.USERNAME+",") > -1)
                    {
                        continue;
                    }
                    else
                    {
                        users.Add(u);
                    }
                }
            }
            return users;
        }

        // 获取请假人员列表
        public void GetQJRYList()
        {
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            IList<RemoteUserService.VUser> users = GetAllUsers();
            int totalcount = users.Count;

            try
            {
                if (type != "" && key != "")
                {
                    if (type== "REALNAME")
                    {
                        users = users.Where(u => u.REALNAME.Contains(key)).ToList();
                    }
                    else if (type== "DEPNAME")
                    {
                        users = users.Where(u => u.DEPNAME.Contains(key)).ToList();
                    }
                }
                users = users.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(users)));
                Response.End();
            }

        }

        // 获取监督记录列表
        public void GetJDJLList()
        {
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";

                if (type != "" && key != "")
                {
                    strwhere += " and " + type +  " like '%" + key + "%' ";
                }
                if (gcbh !="")
                {
                    strwhere += " and gcbh='" + gcbh + "' ";
                }
                string sql = "select * from  view_jdbg_jdjl where 1=1 and LX='JDJL' " + strwhere + " order by recid desc ";
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        
        #endregion

        #endregion

    }
}
