using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Bll
{
	public class SystemService : ISystemService
	{
		#region 数据库对象
		public ICommonDao CommonDao { get; set; }
		public IHelpDesktopItemDao HelpDesktopItemDao { get; set; }
		public ISelfDesktopItemDao SelfDesktopItemDao { get; set; }
		public IViewSelfDesktopItemDao ViewSelfDesktopItemDao { get; set; }

        public IUserSettingDao UserSettingDao { get; set; }

        public IKqUserSignDao KqUserSignDao { get; set; }
        private ISysSessionDao SysSessionDao { get; set; }

		#endregion

		#region 桌面项
		/// <summary>
		/// 根据用户权限获取可用的桌面项
		/// </summary>
		/// <returns></returns>
		public IList<HelpDesktopItem> GetDesktopItems(string userrights)
		{
			IList<HelpDesktopItem> ret = new List<HelpDesktopItem>();
			try
			{
				userrights = "," + userrights + ",";

				IList<HelpDesktopItem> items = HelpDesktopItemDao.GetAll();

				var q = from e in items where e.ItemPower.GetSafeString() == "" || userrights.IndexOf("," + e.ItemPower + ",") > -1 orderby e.DisplayOrder select e;
				ret = q.ToList<HelpDesktopItem>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取用户桌面项，包括用户设置隐藏的
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public IList<ViewSelfDesktopItem> GetUserDesktopItems(string username)
		{
			IList<ViewSelfDesktopItem> ret = new List<ViewSelfDesktopItem>();
			
			try
			{
				ret = ViewSelfDesktopItemDao.GetByUser(username);				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 设置用户桌面项
		/// </summary>
		/// <param name="username"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		[Transaction(ReadOnly=false)]
		public bool InitUserDesktopItem(string username, string userrights, out string error)
		{
			bool ret = false;
			error = "";
			try
			{
				IList<HelpDesktopItem> validitems = GetDesktopItems(userrights);
				IList<SelfDesktopItem> useritems = SelfDesktopItemDao.GetByUser(username);

				// 待添加的
				var qadd = validitems.Where((i) =>
					{
						return useritems.Where((j) => j.ItemKey == i.ItemKey).Count() == 0;
					});
				foreach (HelpDesktopItem itmadd in qadd)
				{
					SelfDesktopItem itm = new SelfDesktopItem() { DisplayOrder = itmadd.DisplayOrder, IsDisplay = true, ItemColumn = "0", ItemKey = itmadd.ItemKey, UserName = username };
					SelfDesktopItemDao.Save(itm);
				}
				// 待删除的
				var qdel = useritems.Where((i) =>
					{
						return validitems.Where((j) => j.ItemKey == i.ItemKey).Count() == 0;
					});
				foreach (SelfDesktopItem itmdel in qdel)
				{
					SelfDesktopItemDao.Delete(itmdel);
				}
				ret = true;
			}
			catch (Exception e)
			{
				error = "InitUserDesktopItem异常，异常信息：" + e.Message + "。堆栈信息：" + SysLog4.GetStackTraceInfo();
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		
		#endregion

		#region 日程安排
		/// <summary>
		/// 获取用户日程安排
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public IList<VUserCalendar> GetUserCalendar(string curname, string username, DateTime start, DateTime end)
		{
			IList<VUserCalendar> ret = new List<VUserCalendar>();
			try
			{
				// 用户编辑的日程
				string sql = "select * from usercalendar where username='" + curname + "' and (year(endtime)=1970 or endtime>=convert(datetime,'" + start.ToString() + "')) and starttime<convert(datetime,'" + end.ToString() + "')";
				IList<IDictionary<string, string>> calendars = CommonDao.GetDataTable(sql);
				foreach (IDictionary<string, string> itm in calendars)
				{
					string starttime = itm["starttime"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					string endtime = itm["endtime"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					ret.Add(new VUserCalendar()
					{
						allDay = itm["allday"].GetSafeBool(),
						canEdit = true,
						color = itm["color"],
						end = endtime,
						id = itm["recid"],
						realname = itm["realname"],
						start = starttime,
						title = itm["title"],
						url = itm["url"],
						username = itm["username"]
					});

				}
				// 外出记录
				sql = "select a.*,b.color from userleave a left outer join helpuserleavetype b on a.leavetype=b.itemkey where a.username in (" + username.FormatSQLInStr() + ") and a.isallowed=1 and a.todate>=convert(datetime,'" + start.ToString() + "') and a.fromdate<convert(datetime,'" + end.ToString() + "')";
				IList<IDictionary<string, string>> leaves = CommonDao.GetDataTable(sql);
				foreach (IDictionary<string, string> itm in leaves)
				{
					string starttime = itm["fromdate"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					string endtime = itm["todate"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					bool allday = itm["todate"].GetSafeDate().Subtract(itm["fromdate"].GetSafeDate()).TotalDays >= 1;
					ret.Add(new VUserCalendar()
					{
						allDay = allday,
						canEdit = false,
						color = itm["color"],
						end = endtime,
						id = "l_" + itm["recid"],
						realname = itm["realname"],
						start = starttime,
						title = "[" + itm["realname"] + "]" + itm["leavetype"] + itm["leavetype2"] + itm["reason"],
						url = "",
						username = itm["username"]
					});

				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取日程安排
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public VUserCalendar GetCalendar(int id, string username)
		{
			VUserCalendar ret = null;
			try
			{
				string sql = "select * from usercalendar where recid="+id+" and username='"+username+"'";
				IList<IDictionary<string, string>> calendars = CommonDao.GetDataTable(sql);
				foreach (IDictionary<string, string> itm in calendars)
				{
					string starttime = itm["starttime"].GetSafeDate().ToString("yyyy-MM-dd HH;mm");
					string endtime = itm["endtime"].GetSafeDate().ToString("yyyy-MM-dd HH;mm");
					ret = new VUserCalendar()
					{
						allDay = itm["allday"].GetSafeBool(),
						canEdit = true,
						color = itm["color"],
						end = endtime,
						id = itm["recid"],
						realname = itm["realname"],
						start = starttime,
						title = itm["title"],
						url = itm["url"],
						username = itm["username"]
					};
					break;

				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 保存日常安排
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool SaveCalendar(VUserCalendar calendar, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				string sql = "";
				if (calendar.id == "")
				{
					sql = "insert into usercalendar(title,starttime,endtime,url,allday,color,username,realname) values(@title,@starttime,@endtime,@url,@allday,@color,@username,@realname)";
					IList<IDataParameter> sqlparams = new List<IDataParameter>();
					IDataParameter sqlparam = new SqlParameter("@title", calendar.title);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@starttime", calendar.start.GetSafeDate());
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@endtime", calendar.end.GetSafeDate());
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@url", calendar.url);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@allday", calendar.allDay);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@color", calendar.color);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@username", calendar.username);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@realname", calendar.realname);
					sqlparams.Add(sqlparam);
					ret = CommonDao.ExecCommand(sql, CommandType.Text, sqlparams);
				}
				else
				{
					sql = "update usercalendar set title=@title,starttime=@starttime,endtime=@endtime,url=@url,allday=@allday,color=@color,realname=@realname where recid=@recid and username=@username";
					IList<IDataParameter> sqlparams = new List<IDataParameter>();
					IDataParameter sqlparam = new SqlParameter("@title", calendar.title);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@starttime", calendar.start.GetSafeDate());
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@endtime", calendar.end.GetSafeDate());
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@url", calendar.url);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@allday", calendar.allDay);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@color", calendar.color);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@username", calendar.username);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@realname", calendar.realname);
					sqlparams.Add(sqlparam);
					sqlparam = new SqlParameter("@recid", calendar.id.GetSafeInt());
					sqlparams.Add(sqlparam);
					ret = CommonDao.ExecCommand(sql, CommandType.Text, sqlparams);

				}				
			}
			catch (Exception e)
			{
				ret = false;
				msg = e.Message;
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 删除日程安排
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeleteCalendar(int id, string username, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				string sql = "delete from usercalendar where recid=@recid and username=@username";
				IList<IDataParameter> sqlparams = new List<IDataParameter>();
				IDataParameter sqlparam = new SqlParameter("@username", username);
				sqlparams.Add(sqlparam);
				sqlparam = new SqlParameter("@recid", id);
				sqlparams.Add(sqlparam);
				ret = CommonDao.ExecCommand(sql, CommandType.Text, sqlparams);
			}
			catch (Exception e)
			{
				ret = false;
				msg = e.Message;
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		#endregion


		#region 其他杂项
		/// <summary>
		/// 获取列表文件详情
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filename"></param>
		/// <param name="filecontent"></param>
		/// <returns></returns>
		public bool GetFileDetail(string id, out string filename, out byte[] filecontent)
		{
			bool ret = true;
			filename = "";
			filecontent = null;
			try
			{
				string sql = "select * from DataEntryFile where fileid='" + id + "'";
				IList<IDictionary<string, object>> calendars = CommonDao.GetBinaryDataTable(sql);
				ret = false;
				foreach (IDictionary<string, object> itm in calendars)
				{
					filename = itm["filename"].GetSafeString();
					filecontent = itm["filecontent"] as byte[];
					ret = true;
					break;

				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}

        /// <summary>
        /// 获取用户设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public UserSetting GetUserSetting(string username, string key)
        {
            UserSetting ret = null;
            try
            {
                ret = UserSettingDao.Get(username, key);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 保存用户设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetUserSetting(string username, string key, string value)
        {
            bool ret = true;
            try
            {
                bool edit = true;
                UserSetting exists = GetUserSetting(username, key);
                if (exists == null || exists.RECID == 0)
                {
                    if (exists == null)
                        exists = new UserSetting();
                    edit = false;
                }
                exists.UserName = username;
                exists.SettingId = key;
                exists.SettingValue = value;
                if (edit)
                    UserSettingDao.Update(exists);
                else
                    UserSettingDao.Save(exists);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 获取当前用户手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetUserMobile(string usercode)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sjhm from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='"+usercode+"')");
                if (dt.Count > 0)
                {
                    ret = dt[0]["sjhm"].GetSafeString();
                    return ret;
                }
                dt = CommonDao.GetDataTable("select lxsj from i_m_qy where qybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "')");
                if (dt.Count > 0)
                {
                    ret = dt[0]["lxsj"].GetSafeString();
                    return ret;
                }
                dt = CommonDao.GetDataTable("select phone from userinfo where usercode='" + usercode + "'");
                if (dt.Count > 0)
                {
                    ret = dt[0]["phone"].GetSafeString();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
		#endregion

		#region 工作托管
		/// <summary>
		/// 获取托管工作给我的人
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public IList<string> GetHostedUsers(string username)
		{
			IList<string> ret = new List<string>();
			try
			{
				string sql = "select UserName from userhosting where hostinguser='" + username + "' and InUse=1";
				IList<IDictionary<string, string>> hostings = CommonDao.GetDataTable(sql);
				foreach (IDictionary<string, string> hosting in hostings)
					ret.Add(hosting["username"]);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取我工作托管的人
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public string GetHostingUser(string username)
		{
			string ret = "";
			try
			{
				string sql = "select HostingUser from userhosting where UserName='" + username + "' and InUse=1";
				IList<IDictionary<string, string>> hostings = CommonDao.GetDataTable(sql);
				if (hostings.Count > 0)
					ret = hostings[0]["hostinguser"];
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 取消托管
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
        [Transaction(ReadOnly=false)]
		public bool CancelHostingUser(string username, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				string sql = "update userhosting set inuse=0,EndTime=getdate() where UserName='" + username + "' and InUse=1";
				CommonDao.ExecCommand(sql, CommandType.Text);
			}
			catch (Exception e)
			{
				ret = false;
				msg = e.Message;
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 工作托管
		/// </summary>
		/// <param name="username"></param>
		/// <param name="hostingname"></param>
		/// <returns></returns>
        [Transaction(ReadOnly=false)]
		public bool SaveHostingUser(string username, string hostingname, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				string sql = "update userhosting set inuse=0,EndTime=getdate() where UserName='" + username + "' and InUse=1";
				CommonDao.ExecCommand(sql, CommandType.Text);
				sql = "insert into userhosting(UserName,HostingUser,StartTime,InUse) values('" + username + "','" + hostingname + "',getdate(),1)";
				ret = CommonDao.ExecCommand(sql, CommandType.Text);
			}
			catch (Exception e)
			{
				ret = false;
				msg = e.Message;
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		#endregion


        #region 考勤处理

        /// <summary>
        /// 根据用户名和日期，或者考勤记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="SignDate"></param>
        /// <returns></returns>
        public KqUserSign getUserSign(string username, string SignDate)
        {
            KqUserSign ret = null;

            try
            {
                ret = KqUserSignDao.Get(username, SignDate);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        public void updateUserSign(KqUserSign item)
        {
            try
            {
                KqUserSignDao.Updatesign(item);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }

        #endregion

        #region 系统配置，dns映射
        /// <summary>
        /// 返回dns首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        public IDictionary<string,string> GetLoginUrls()
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                IList<IDictionary<string, string>> items = CommonDao.GetDataTable("select settingvalue from syssetting where settingcode='GLOBAL_PAGE_LOGIN_LOGINURL'");
                if (items.Count > 0)
                    ret.Add("____", items[0]["settingvalue"]);
                items = CommonDao.GetDataTable("select dns,settingvalue from syssettingdns where settingcode='GLOBAL_PAGE_LOGIN_LOGINURL'");
                foreach (IDictionary<string,string> row in items)
                {
                    string dns = row["dns"];
                    string url = row["settingvalue"];
                    if (!ret.ContainsKey(dns))
                        ret.Add(dns, url);
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 返回dns用户首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        public IDictionary<string,string> GetMainUrls()
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                IList<IDictionary<string, string>> items = CommonDao.GetDataTable("select settingvalue from syssetting where settingcode='GLOBAL_PAGE_MAIN_URL'");
                if (items.Count > 0)
                    ret.Add("____", items[0]["settingvalue"]);
                items = CommonDao.GetDataTable("select dns,settingvalue from syssettingdns where settingcode='GLOBAL_PAGE_MAIN_URL'");
                foreach (IDictionary<string, string> row in items)
                {
                    string dns = row["dns"];
                    string url = row["settingvalue"];
                    if (!ret.ContainsKey(dns))
                        ret.Add(dns, url);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 返回dns用户首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetMainFrameUrls()
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                IList<IDictionary<string, string>> items = CommonDao.GetDataTable("select settingvalue from syssetting where settingcode='GLOBAL_PAGE_MAIN_FRAME_URL'");
                if (items.Count > 0)
                    ret.Add("____", items[0]["settingvalue"]);
                items = CommonDao.GetDataTable("select dns,settingvalue from syssettingdns where settingcode='GLOBAL_PAGE_MAIN_FRAME_URL'");
                foreach (IDictionary<string, string> row in items)
                {
                    string dns = row["dns"];
                    string url = row["settingvalue"];
                    if (!ret.ContainsKey(dns))
                        ret.Add(dns, url);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 返回页面跳转设置
        /// </summary>
        /// <returns></returns>
        public IList<VSysJumpPage> GetJumpPages()
        {
            IList<VSysJumpPage> ret = new List<VSysJumpPage>();
            try
            {
                IList<IDictionary<string, string>> items = CommonDao.GetDataTable("select a.pageurl as srcurl,b.dns,b.jumpurl,b.usertype from sysjumpsrcpage a inner join sysjumpdestpage b on a.pageid=b.sourcepageid where a.inuse=1 and b.inuse=1");
                
                foreach (IDictionary<string, string> row in items)
                {
                    VSysJumpPage page = new VSysJumpPage()
                    {
                        Dns = row["dns"].GetSafeString(),
                        JumpUrl = row["jumpurl"].GetSafeString(),
                        SrcUrl = row["srcurl"].GetSafeString(),
                        UserType = row["usertype"].GetSafeString()
                    };
                    ret.Add(page);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        
        #endregion

    }
}
