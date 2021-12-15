using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
	public interface ISystemService
	{
		#region 桌面项
		/// <summary>
		/// 根据用户权限获取可用的桌面项
		/// </summary>
		/// <returns></returns>
		IList<HelpDesktopItem> GetDesktopItems(string userrights);
		/// <summary>
		/// 获取用户桌面项，包括用户设置隐藏的
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<ViewSelfDesktopItem> GetUserDesktopItems(string username);
		/// <summary>
		/// 设置用户桌面项
		/// </summary>
		/// <param name="username"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		bool InitUserDesktopItem(string username, string userrights, out string error);
		
		#endregion
		#region 日程安排
		/// <summary>
		/// 获取用户日程安排
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<VUserCalendar> GetUserCalendar(string curname, string username, DateTime start, DateTime end);
		/// <summary>
		/// 获取日程安排
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		VUserCalendar GetCalendar(int id, string username);
		/// <summary>
		/// 保存日常安排
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool SaveCalendar(VUserCalendar calendar, out string msg);
		/// <summary>
		/// 删除日程安排
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool DeleteCalendar(int id, string username, out string msg);
		#endregion

		#region 其他杂项
		/// <summary>
		/// 获取列表文件详情
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filename"></param>
		/// <param name="filecontent"></param>
		/// <returns></returns>
		bool GetFileDetail(string id, out string filename, out byte[] filecontent);
        /// <summary>
        /// 获取用户设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        UserSetting GetUserSetting(string username, string key);
        /// <summary>
        /// 保存用户设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetUserSetting(string username, string key, string value);
        /// <summary>
        /// 获取当前用户手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        string GetUserMobile(string usercode);
		#endregion

		#region 工作托管
		/// <summary>
		/// 获取托管工作给我的人
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<string> GetHostedUsers(string username);
		/// <summary>
		/// 获取我工作托管的人
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		string GetHostingUser(string username);
		/// <summary>
		/// 取消托管
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		bool CancelHostingUser(string username, out string msg);
		/// <summary>
		/// 工作托管
		/// </summary>
		/// <param name="username"></param>
		/// <param name="hostingname"></param>
		/// <returns></returns>
		bool SaveHostingUser(string username, string hostingname,out string msg);
		#endregion


        #region 考勤处理
        /// <summary>
        /// 根据用户名和日期，或者考勤记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="SignDate"></param>
        /// <returns></returns>
        KqUserSign getUserSign(string username, string SignDate);

        void updateUserSign(KqUserSign item);
        #endregion

        #region 系统配置，dns映射
        /// <summary>
        /// 返回dns首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetLoginUrls();
        /// <summary>
        /// 返回dns用户首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetMainUrls();
        /// <summary>
        /// 返回dns用户首页配置，全局首页key为____
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetMainFrameUrls();
        /// <summary>
        /// 返回页面跳转设置
        /// </summary>
        /// <returns></returns>
        IList<VSysJumpPage> GetJumpPages();
        #endregion
    }
}
