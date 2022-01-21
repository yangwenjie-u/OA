using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IBll
{
	/// <summary>
	/// 办公接口
	/// </summary>
	public interface IOaService
	{
		#region 公告
		/// <summary>
		/// 获取公告
		/// </summary>
		/// <param name="announceid"></param>
		/// <returns></returns>
		CompanyAnnounce GetAnnounce(int announceid);		
		/// <summary>
		/// 保存公告
		/// </summary>
		/// <param name="announce"></param>
		/// <param name="readers"></param>
		/// <param name="files"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool SaveAnnounce(CompanyAnnounce announce, IList<CompanyReader> readers, out string msg);
		/// <summary>
		/// 删除公告
		/// </summary>
		/// <param name="announceid"></param>
		/// <param name="msg"></param>
		/// <returns></returns>		
		bool DeleteAnnounce(int announceid, out string msg);
		/// <summary>
		/// 删除公告文件
		/// </summary>
		/// <param name="announceid"></param>
		/// <param name="fileid"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteAnnounceFile(int announceid, int fileid, out string msg);
		/// <summary>
		/// 设置用户读公告状态
		/// </summary>
		/// <param name="announceid"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool SetAnnounceRead(int announceid, string username, out string msg);
		/// <summary>
		/// 获取查看公告列表
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindtx"></param>
		/// <param name="totalcount"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetPageAnnounces(string username, string key, string hasread, 
			int pagesize, int pageindex, out int totalcount);
		/// <summary>
		/// 获取管理公告列表
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindtx"></param>
		/// <param name="totalcount"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetPageAnnounces(string username, string key, string date1, string date2,
			int pagesize, int pageindex, out int totalcount);
		#endregion
		#region 邮件
		/// <summary>
		/// 获取邮件信息
		/// </summary>
		/// <param name="mailid"></param>
		/// <returns></returns>
		UserMail GetMail(int mailid);
		/// <summary>
		/// 获取邮箱列表
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<IDictionary<string,string>> GetMailFolders(string username);
		/// <summary>
		/// 保存邮件
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="readers"></param>
		/// <param name="files"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool SaveMail(UserMail mail, IList<CompanyReader> readers, out string msg);
		/// <summary>
		/// 删除邮件
		/// </summary>
		/// <param name="mailid"></param>
		/// <param name="readerid"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteMail(int mailid, int readerid, string username, out string msg);
		/// <summary>
		/// 获取邮件列表
		/// </summary>
		/// <param name="folderid"></param>
		/// <param name="key"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindtx"></param>
		/// <param name="totalcount"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetPageMails(string foldertype, string username, string key, string hasread,
			string dt1, string dt2, int pagesize, int pageindex, out int totalcount);
        /// <summary>
        /// 获取新邮件数量
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetNewMailSum(string username);
		/// <summary>
		/// 设置用户读邮件状态
		/// </summary>
		/// <param name="announceid"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool SetMailRead(int mailid, string username, out string msg);
		#endregion

		#region 共享文件
		/// <summary>
		/// 获取用户的所有文件夹
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<UserShareFileFolder> GetShareFolders(string username);
		/// <summary>
		/// 保存文件夹
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		UserShareFileFolder SaveFileFolder(UserShareFileFolder folder, out string msg);
		/// <summary>
		/// 删除文件夹
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteFileFolder(int folderid, string username, out string msg);
		/// <summary>
		/// 共享文件夹
		/// </summary>
		/// <param name="folderid"></param>
		/// <param name="readers"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool ShareFileFolder(int folderid, IList<CompanyReader> readers, out string msg);
		/// <summary>
		/// 删除共享文件夹
		/// </summary>
		/// <param name="folderid"></param>
		/// <param name="readerid"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteShareFileFolder(int folderid, int readerid, string username, out string msg);
		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="ufile"></param>
		/// <param name="file"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		UserShareFile SaveFile(UserShareFile ufile, out string msg);
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="shareifleid"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteFile(int fileid, string username, out string msg);
		/// <summary>
		/// 共享件夹
		/// </summary>
		/// <param name="folderid"></param>
		/// <param name="readers"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool ShareFile(int fileid, IList<CompanyReader> readers, out string msg);
		/// <summary>
		/// 删除共享文件
		/// </summary>
		/// <param name="folderid"></param>
		/// <param name="readerid"></param>
		/// <param name="username"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool DeleteShareFile(int fileid, int readerid, string username, out string msg);
		/// <summary>
		/// 查找文件（包括文件夹），如果关键字查找，忽略文件夹
		/// </summary>
		/// <param name="folderid">foldertype:1表示别人共享给我的；2表示我共享给别人的</param>
		/// <param name="username"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetShareFiles(string folderid, string foldertype, string username, string key);
		/// <summary>
		/// 获取列表的文件或文件夹记录
		/// </summary>
		/// <param name="isfolder">是否文件夹</param>
		/// <param name="recid">记录号</param>
		/// <returns></returns>
		IDictionary<string, string> GetShareFileRow(bool isfolder, int recid);
		/// <summary>
		/// 重命名文件
		/// </summary>
		/// <param name="fileid">文件id</param>
		/// <param name="newname">新文件名</param>
		/// <param name="username">文件所有者帐号</param>
		/// <param name="msg">错误信息</param>
		/// <returns></returns>
		bool RenameShareFileName(int fileid, string newname, string username, out string msg);
		/// <summary>
		/// 重命名文件夹
		/// </summary>
		/// <param name="folderid">文件夹id</param>
		/// <param name="newname">新文件夹名</param>
		/// <param name="username">文件夹所有者帐号</param>
		/// <param name="msg">错误信息</param>
		/// <returns></returns>
		bool RenameShareFileFolderName(int folderid, string newname, string username, out string msg);
		/// <summary>
		/// 设置父目录
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="parentid"></param>
		/// <returns></returns>
		bool SaveShareFileFolderParent(int folderid, int parentid, string username, out string msg);
		/// <summary>
		/// 设置目录
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="parentid"></param>
		/// <returns></returns>
		bool SaveShareFileFileFolder(int fileid, int parentid, string username, out string msg);
		#endregion

		#region 读者
		/// <summary>
		/// 获取某个实体的读者
		/// </summary>
		/// <param name="entityname"></param>
		/// <param name="endityid"></param>
		/// <returns></returns>
		IList<CompanyReader> GetCompanyReader(string entityname, string entityid); 
		#endregion

		#region 文件存储
		/// <summary>
		/// 获取某个实体的文件
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		IList<CompanyFileStorage> GetCompanyFileStorages(string ids);
		/// <summary>
		/// 获取某个文件
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		CompanyFileStorage GetCompanyFileStorage(int id);
		/// <summary>
		/// 根据文件名获取图像和文件描述
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		SysFileImage GetFileImage(string filename, bool ispath);
		/// <summary>
		/// 保存文件
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		CompanyFileStorage SaveFile(CompanyFileStorage file,out string err);
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="fileid"></param>
		/// <param name="tablename"></param>
		/// <param name="fieldname"></param>
		/// <param name="recid"></param>
		/// <returns></returns>
		bool DeleteFile(int fileid,out string err);
		#endregion

        /// <summary>
        /// 获取某个变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region 变更公示
        /// <summary>
        /// 获取变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyChange GetChange(int id);
        /// <summary>
        /// 获取某次变更项目
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        IList<CompanyChangeItem> GetCompanyChangeItem(int changeid);
         /// <summary>
        /// 获取变更汇总
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyChangeTotal GetChangeToal(int id);
        /// <summary>
        /// 获取某次变更汇总项目
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        IList<CompanyChangeTotalItem> GetCompanyChangeTotalItem(int changetotalid);
        /// <summary>
        /// 根据时间筛选变更记录
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        IList<CompanyChange> GetCompanyChange(string startdate, string enddate);
        /// <summary>
        /// 统计某次变更的条目和金额
        /// </summary>
        /// <param name="changeid"></param>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="money"></param>
        void GetCompanyChangeSum(int changeid, int type, out int no, out decimal money);
         /// <summary>
        /// 统计累计的条目和金额
        /// </summary>
        /// <param name="depid"></param>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="money"></param>
        void GetCompanyChangeAllSum(string depid, int type, out int no, out decimal money);
        /// <summary>
        /// 保存或更新汇总
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalid"></param>
        /// <param name="title"></param>
        /// <param name="createdby"></param>
        /// <param name="createdon"></param>
        /// <param name="items"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveCompanyChangeAllSum(int id, out int totalid, string title, string createdby, string createdon, IList<CompanyChangeTotalItem> items, out string msg);

		#endregion

		/// <summary>
		/// 获取设置通知
		/// </summary>
		/// <param name="recid"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetAnnouncementNotice(string recid);

		bool DelAnnouncementNotice(string recids);

		/// <summary>
		/// 保存通知
		/// </summary>
		/// <param name="recid"></param>
		/// <param name="title"></param>
		/// <param name="NoticeContent"></param>
		/// <returns></returns>
		bool SaveAnnouncementNotice(string recid, string title, string NoticeContent);


		/// <summary>
		/// 检测机构科室
		/// </summary>
		/// <returns></returns>
		bool CreateJcks(string type, string ksbh, string qybh, string ksmc, string ksdz, string lxdh, string ksys, string kszcode, string kszxm, string jsfzrcode, string jsfzrxm, string zlfzrcode, string zlfzrxm);

		/// <summary>
		/// 修改材料信息
		/// </summary>
		/// <param name="recid">材料记录唯一号</param>
		/// <param name="materId"></param>
		/// <param name="materName"></param>
		/// <param name="unitId"></param>
		/// <param name="unitName"></param>
		/// <param name="price"></param>
		/// <param name="purchasePrice"></param>
		/// <param name="quantity"></param>
		/// <param name="purpose"></param>
		/// <param name="technicalRequirement"></param>
		/// <param name="supplier"></param>
		/// <param name="manufacturer"></param>
		/// <param name="requisitioner"></param>
		/// <returns></returns>
		bool PurchaseOrderModify(string recid,string serialRecid, string materBH, string materId, string materName, string unitId, string unitName,
					string price, string purchasePrice, string quantity, string purpose, string technicalRequirement, string supplier, string manufacturer, string requisitioner);

	}

}
