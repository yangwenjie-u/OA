using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// companyreader表格中的实体id
	/// </summary>
	public static class CompanyEntityType
	{
		public static string CompanyAnnounce = "CompanyAnnounce";			// 公告
		public static string UserMail = "UserMail";							// 邮件
		public static string UserShareFile = "UserShareFile";				// 共享文件
		public static string UserShareFileFolder = "UserShareFileFolder";	// 共享文件夹
	}
	/// <summary>
	/// 邮箱类型
	/// </summary>
	public static class MailFolderType
	{
		public static string ReceiveBox = "1";	// 收件箱
		public static string DraftBox = "2";	// 草稿箱
		public static string SendBox = "3";		// 已发送
		public static string DeleteBox = "4";	// 已删除
	}
	/// <summary>
	/// 共享文件模块中，文件共享类型
	/// </summary>
	public static class ShareFolderType
	{
		public static string Normal = "0";		// 正常文件夹
		public static string OtherShare = "1";	// 别人共享给我的
		public static string MyShare = "2";		//我共享给别人的
		public static string All = "3";			// 所有，查找时根目录
	}
	/// <summary>
	/// 共享文件类型
	/// </summary>
	public static class ShareFileType
	{
		public static string File = "2";		// 文件
		public static string Folder = "1";		// 文件夹
	}

	public class UserLogType
    {
        public const string In = "1";	// 上班考勤
        public const string Out = "2";	// 下班考勤
        public const string Check = "3"; //门禁
    }
    /// <summary>
    /// 人员类型
    /// </summary>
    public enum UserType
    {
        Invalid,            // 无效
        InnerUser,          // 内部用户
        QyUser,             // 企业用户
        RyUser,              // 人员用户
        ZjzUser             // 质监站用户
    }

}
