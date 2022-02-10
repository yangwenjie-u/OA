using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.DataModal.Entities;
using Spring.Transaction.Interceptor;

namespace BD.Jcbg.Bll
{
    public class OaService : IOaService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        ICompanyAnnounceDao CompanyAnnounceDao { get; set; }
        ICompanyFileStorageDao CompanyFileStorageDao { get; set; }
        ICompanyReaderDao CompanyReaderDao { get; set; }
        IUserMailDao UserMailDao { get; set; }
        IUserMailFolderDao UserMailFolderDao { get; set; }
        IUserShareFileDao UserShareFileDao { get; set; }
        IUserShareFileFolderDao UserShareFileFolderDao { get; set; }
        ISysFileImageDao SysFileImageDao { get; set; }
        ICompanyChangeDao CompanyChangeDao { get; set; }
        ICompanyChangeItemDao CompanyChangeItemDao { get; set; }
        ICompanyChangeTotalDao CompanyChangeTotalDao { get; set; }
        ICompanyChangeTotalItemDao CompanyChangeTotalItemDao { get; set; }
        #endregion
        #region 公告
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="announceid"></param>
        /// <returns></returns>
        public CompanyAnnounce GetAnnounce(int announceid)
        {
            CompanyAnnounce ret = null;
            try
            {
                ret = CompanyAnnounceDao.Get(announceid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 保存公告
        /// </summary>
        /// <param name="announce"></param>
        /// <param name="readers"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveAnnounce(CompanyAnnounce announce, IList<CompanyReader> readers, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                bool isnew = announce.Recid == 0;

                if (isnew)
                    announce = CompanyAnnounceDao.Save(announce);
                else
                    CompanyAnnounceDao.Update(announce);
                // 删除读者
                if (!isnew)
                    CompanyReaderDao.Delete(CompanyEntityType.CompanyAnnounce, announce.Recid.ToString());
                // 添加读者
                for (int i = 0; i < readers.Count; i++)
                {
                    readers[i].ParentEntity = CompanyEntityType.CompanyAnnounce;
                    readers[i].ParentId = announce.Recid.ToString();
                    readers[i].HasReader = false;
                    readers[i].HasDelete = false;

                    CompanyReaderDao.Save(readers[i]);
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
        /// 删除公告
        /// </summary>
        /// <param name="announceid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>		
        [Transaction(ReadOnly = false)]
        public bool DeleteAnnounce(int announceid, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                CompanyAnnounce ann = CompanyAnnounceDao.Get(announceid);
                // 删除读者
                CompanyReaderDao.Delete(CompanyEntityType.CompanyAnnounce, ann.Recid.ToString());
                // 删除文件
                if (ann.FileIds != "")
                {
                    string[] arrfiles = ann.FileIds.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string file in arrfiles)
                    {

                        CompanyFileStorageDao.Delete(file.Split(new char[] { '|' })[1].GetSafeInt());
                    }
                }
                // 删除公告
                CompanyAnnounceDao.Delete(announceid);
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
        /// 删除公告文件
        /// </summary>
        /// <param name="announceid"></param>
        /// <param name="fileid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteAnnounceFile(int announceid, int fileid, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                CompanyAnnounce ann = CompanyAnnounceDao.Get(announceid);
                // 删除文件
                CompanyFileStorageDao.Delete(fileid);
                // 保存公告
                ann.FileIds = ann.FileIds.Replace(fileid + ",", "");
                CompanyAnnounceDao.Update(ann);
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
        /// 设置用户读公告状态
        /// </summary>
        /// <param name="announceid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetAnnounceRead(int announceid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string sql = "update companyreader set HasReader=1,ReaderTime=getdate() where ParentEntity='" + CompanyEntityType.CompanyAnnounce + "' and ParentId='" + announceid.ToString() + "' and HasReader=0 and username='" + username + "'";
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
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindtx"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageAnnounces(string username, string key, string hasread,
            int pagesize, int pageindex, out int totalcount)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            totalcount = 0;
            try
            {
                string sql = "select a.*,b.HasReader from CompanyAnnounce a inner join CompanyReader b on a.recid=b.parentid where b.ParentEntity='" + CompanyEntityType.CompanyAnnounce + "' and b.HasDelete=0 and b.UserName='" + username + "' ";
                if (key != "")
                    sql += " and a.Title like '%" + key + "%' ";
                if (hasread != "")
                    sql += " and b.HasReader=" + hasread + " ";
                sql += " order by a.CreatedTime desc";
                ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;


        }
        /// <summary>
        /// 获取管理公告列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindtx"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageAnnounces(string username, string key, string date1, string date2,
            int pagesize, int pageindex, out int totalcount)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            totalcount = 0;
            try
            {
                string sql = "select* from CompanyAnnounce where 1=1 ";
                if (key != "")
                    sql += " and Title like '%" + key + "%' ";
                if (date1 != "")
                    sql += " and createdtime>=convert(datetime,'" + date1 + "') ";
                if (date2 != "")
                    sql += " and createdtime<convert(datetime,'" + date2.GetSafeDate().AddDays(1).ToString() + "') ";
                sql += " order by recid desc";
                ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;


        }
        #endregion
        #region 邮件
        /// <summary>
        /// 获取邮件信息
        /// </summary>
        /// <param name="mailid"></param>
        /// <returns></returns>
        public UserMail GetMail(int mailid)
        {
            UserMail ret = null;
            try
            {
                ret = UserMailDao.Get(mailid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取邮箱列表
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetMailFolders(string username)
        {

            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from UserMailFolder where UserName='' or Username='" + username + "' order by username, displayorder";

                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 保存邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="readers"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveMail(UserMail mail, IList<CompanyReader> readers, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                bool isnew = mail.Recid == 0;

                // 已发送邮件不允许修改
                if (!isnew)
                {
                    UserMail oldmail = UserMailDao.Get(mail.Recid);
                    if (oldmail.HasSend.Value)
                    {
                        ret = false;
                        msg = "邮件已发送，无法修改";
                        return ret;
                    }
                }
                if (isnew)
                    mail = UserMailDao.Save(mail);
                else
                    UserMailDao.Update(mail);
                // 删除读者
                if (!isnew)
                    CompanyReaderDao.Delete(CompanyEntityType.UserMail, mail.Recid.ToString());
                // 添加读者
                for (int i = 0; i < readers.Count; i++)
                {
                    readers[i].ParentEntity = CompanyEntityType.UserMail;
                    readers[i].ParentId = mail.Recid.ToString();
                    readers[i].HasReader = false;
                    readers[i].HasDelete = false;

                    CompanyReaderDao.Save(readers[i]);
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
        /// 删除邮件
        /// </summary>
        /// <param name="mailid"></param>
        /// <param name="readerid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteMail(int mailid, int readerid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                // 发件人删除，已发送只打标记；未发送删除
                if (readerid == 0)
                {
                    string sql = "delete from UserMail where recid=" + mailid + " and HasSend=0";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    sql = "update UserMail set HasDelete=1 where recid=" + mailid + " and HasSend=1";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }
                // 收件人删除，如果是第一次，把reader的delete标机打上，并移动到删除箱，第二次物理删除
                else
                {
                    string sql = "delete from CompanyReader where recid=" + readerid + " and HasDelete=1";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    sql = "update CompanyReader set HasDelete=1 where recid=" + readerid + " and HasDelete=0";
                    CommonDao.ExecCommand(sql, CommandType.Text);
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
        /// 获取邮件列表
        /// </summary>
        /// <param name="folderid"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindtx"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageMails(string foldertype, string username, string key, string hasread,
            string dt1, string dt2, int pagesize, int pageindex, out int totalcount)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            totalcount = 0;
            try
            {
                string sql = "";
                // 收件箱
                if (foldertype == MailFolderType.ReceiveBox)
                {
                    sql = "select a.*,b.recid as readerid,b.hasreader,convert(varchar(50), a.sendtime,120) as sy_sendtime from usermail a inner join companyreader b on a.recid=b.ParentId where b.ParentEntity='" + CompanyEntityType.UserMail + "' and b.HasDelete=0 and b.UserName='" + username + "' and a.HasSend=1 ";

                }
                // 草稿箱
                else if (foldertype == MailFolderType.DraftBox)
                {
                    sql = "select a.*,0 as readerid,0 as hasreader,convert(varchar(50), a.sendtime,120) as sy_sendtime from usermail a where a.Sender='" + username + "' and a.HasSend=0";

                }
                // 已发邮件
                else if (foldertype == MailFolderType.SendBox)
                {
                    sql = "select a.*,0 as readerid,0 as hasreader,convert(varchar(50), a.sendtime,120) as sy_sendtime from usermail a where a.Sender='" + username + "' and a.HasSend=1 and a.HasDelete=0";

                }
                // 已删除
                else if (foldertype == MailFolderType.DeleteBox)
                {
                    sql = "select a.*,b.recid as readerid,b.hasreader,convert(varchar(50) , a.sendtime,120) as sy_sendtime from usermail a inner join companyreader b on a.recid=b.ParentId where b.ParentEntity='" + CompanyEntityType.UserMail + "' and b.HasDelete=1 and b.UserName='" + username + "' ";

                }
                if (dt1 != "")
                    sql += " and a.SendTime>=convert(datetime,'" + dt1 + "') ";
                if (dt2 != "")
                    sql += " and a.SendTime<convert(datetime,'" + dt2.GetSafeDate().AddDays(1).ToShortDateString() + "') ";
                if (key != "")
                    sql += " and a.Title like '%" + key + "%' ";
                if (hasread != "")
                    sql += " and hasreader='" + hasread + "' ";
                sql += " order by a.recid desc";
                ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取新邮件数量
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetNewMailSum(string username)
        {
            int ret = 0;
            try
            {
                string sql = "select count(*) as mailsum from usermail a inner join companyreader b on a.recid=b.ParentId where b.ParentEntity='" + CompanyEntityType.UserMail + "' and b.HasDelete=0 and b.UserName='" + username + "' and a.HasSend=1 and b.HasReader=0 ";
                ret = CommonDao.GetDataTable(sql)[0]["mailsum"].GetSafeInt();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 设置用户读邮件状态
        /// </summary>
        /// <param name="announceid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetMailRead(int mailid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            string sql = "";
            try
            {
                sql = "update companyreader set HasReader=1,ReaderTime=getdate() where ParentEntity='" + CompanyEntityType.UserMail + "' and ParentId='" + mailid.ToString() + "' and HasReader=0 and username='" + username + "'";
                ret = CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(sql, e);
            }
            return ret;
        }
        #endregion

        #region 共享文件
        /// <summary>
        /// 获取用户的所有文件夹
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<UserShareFileFolder> GetShareFolders(string username)
        {
            IList<UserShareFileFolder> ret = new List<UserShareFileFolder>();
            try
            {
                ret = UserShareFileFolderDao.Gets(username);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 保存文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public UserShareFileFolder SaveFileFolder(UserShareFileFolder folder, out string msg)
        {
            msg = "";
            try
            {
                bool isnew = folder.Recid == 0;
                if (isnew)
                    folder = UserShareFileFolderDao.Save(folder);
                else
                    UserShareFileFolderDao.Update(folder);
            }
            catch (Exception e)
            {
                folder.Recid = 0;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            return folder;
        }
        /// <summary>
        /// 删除文件夹,有子文件或目录不允许删除
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteFileFolder(int folderid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                int chindrensum = 0;
                string sql = "select count(*) as totalcount from usersharefile where folderid=" + folderid;
                IList<IDictionary<string, string>> table = CommonDao.GetDataTable(sql);
                if (table.Count > 0)
                    chindrensum = table[0]["totalcount"].GetSafeInt();
                if (chindrensum == 0)
                {
                    sql = "select count(*) as totalcount from UserShareFileFolder where ParentId=" + folderid;
                    table = CommonDao.GetDataTable(sql);
                    if (table.Count > 0)
                        chindrensum = table[0]["totalcount"].GetSafeInt();
                }
                if (chindrensum > 0)
                {
                    ret = false;
                    msg = "目录包含子目录或者文件，请先清空目录内容再删除";
                }
                else
                {
                    sql = "delete from UserShareFileFolder where Recid=" + folderid + " and UserName='" + username + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
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
        /// 共享文件夹
        /// </summary>
        /// <param name="folderid"></param>
        /// <param name="readers"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool ShareFileFolder(int folderid, IList<CompanyReader> readers, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {

                // 删除以前共享
                CompanyReaderDao.Delete(CompanyEntityType.UserShareFileFolder, folderid.ToString());
                // 添加共享
                for (int i = 0; i < readers.Count; i++)
                {
                    readers[i].ParentEntity = CompanyEntityType.UserShareFileFolder;
                    readers[i].ParentId = folderid.ToString();
                    readers[i].HasReader = false;
                    readers[i].HasDelete = false;

                    CompanyReaderDao.Save(readers[i]);
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
        /// 删除共享文件夹，共享人删除，删除所有reader；读的人删除，删除自己的reader
        /// </summary>
        /// <param name="folderid"></param>
        /// <param name="readerid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteShareFileFolder(int folderid, int readerid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                // 共享人取消共享
                if (readerid == 0)
                {
                    CompanyReaderDao.Delete(CompanyEntityType.UserShareFileFolder, folderid.ToString());
                }
                // 读的人删除共享
                else
                {
                    CompanyReaderDao.Delete(readerid);
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
        /// 上传文件
        /// </summary>
        /// <param name="ufile"></param>
        /// <param name="file"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UserShareFile SaveFile(UserShareFile ufile, out string msg)
        {
            msg = "";
            try
            {
                // 保存共享文件条目
                ufile = UserShareFileDao.Save(ufile);
            }
            catch (Exception e)
            {
                ufile.Recid = 0;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            return ufile;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="shareifleid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteFile(int fileid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                UserShareFile file = UserShareFileDao.Get(fileid);
                CompanyFileStorageDao.Delete(file.FileId.Value);
                UserShareFileDao.Delete(file);
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
        /// 共享件夹
        /// </summary>
        /// <param name="folderid"></param>
        /// <param name="readers"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool ShareFile(int fileid, IList<CompanyReader> readers, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {

                // 删除以前共享
                CompanyReaderDao.Delete(CompanyEntityType.UserShareFile, fileid.ToString());
                // 添加共享
                for (int i = 0; i < readers.Count; i++)
                {
                    readers[i].ParentEntity = CompanyEntityType.UserShareFile;
                    readers[i].ParentId = fileid.ToString();
                    readers[i].HasReader = false;
                    readers[i].HasDelete = false;

                    CompanyReaderDao.Save(readers[i]);
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
        /// 删除共享文件
        /// </summary>
        /// <param name="folderid"></param>
        /// <param name="readerid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteShareFile(int fileid, int readerid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                // 共享人取消共享
                if (readerid == 0)
                {
                    CompanyReaderDao.Delete(CompanyEntityType.UserShareFile, fileid.ToString());
                }
                // 读的人删除共享
                else
                {
                    CompanyReaderDao.Delete(readerid);
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
        /// 查找文件（包括文件夹），如果关键字查找，忽略文件夹
        /// </summary>
        /// <param name="folderid">foldertype:0表示正常的，1表示别人共享给我的；2表示我共享给别人的</param>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetShareFiles(string folderid, string foldertype, string username, string key)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                // 我的文件夹
                string sql1 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b where b.imagetype='1' and a.username='" + username + "' order by a.foldername asc)";
                // 共享给我的文件夹（父目录为0）
                string sql2 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b,companyreader c where a.recid=c.ParentId and b.imagetype='1' and c.username='" + username + "' and c.ParentEntity='" + CompanyEntityType.UserShareFileFolder + "' order by a.foldername asc)";
                // 共享给我的文件夹，父目录非0）
                string sql12 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b,companyreader c where a.parentid=" + folderid + " order by a.foldername asc)";
                // 我的文件
                string sql3 = "(select top 100 percent 0 as isvirtual,b.recid as fileid,a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where b.username='" + username + "'  order by b.filedesc asc)";
                // 共享给我的文件（查找里面用）
                string sql4 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where a.recid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFile + "') or a.folderid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFileFolder + "') order by b.filedesc asc)";
                // 共享给我的文件（列表用，父目录为0用）
                string sql9 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where a.recid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 共享给我的文件（列表用，父目录为非零用）
                string sql11 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where  a.folderid=" + folderid + " order by b.filedesc asc)";

                // 我共享给别人的文件夹
                string sql5 = "(select top 100 percent 0 as isvirtual,0 as fileid, a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b where b.imagetype='1' and a.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFileFolder + "') order by a.foldername asc)";
                // 我共享给别人的文件（查找里面用）
                string sql6 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where b.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 我共享给别人的文件（列表用）
                string sql10 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where b.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 根目录中的其他人共享给我的
                string sql7 = "(select 1 as isvirtual,0 as fileid,0 as recid,0 as parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,'收到的共享' as [filename],'" + username + "' as username, '' as realname, '' as createdtime,imagename,filedesc as filetypedesc,0 as filesize from sysfileimage where imagetype='3')";
                // 根目录中我共享给别人的
                string sql8 = "(select 1 as isvirtual,0 as fileid,0 as recid,0 as parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,'我的共享' as [filename],'" + username + "' as username,'' as realname,'' as createdtime, imagename,filedesc as filetypedesc,0 as filesize from sysfileimage where imagetype='3')";

                string sql = "";
                if (key != "")
                    sql = "select * from (" + sql1 + " union all " + sql2 + " union all " + sql3 + " union all " + sql4 + ") as t1 where ([filename] like '%" + key + "%' or [filetypedesc] like '%" + key + "%') order by filename asc";
                else
                {
                    // 正常文件夹
                    if (foldertype == ShareFolderType.Normal)
                    {
                        sql = "select * from (" + sql1 + " union all " + sql3 + ") as t1 where parentid=" + folderid + " ";
                    }
                    // 别人共享给我的
                    else if (foldertype == ShareFolderType.OtherShare)
                    {
                        //sql = "select * from (" + sql2 + " union all " + sql4 + ") as t1 where parentid=" + folderid + " ";
                        if (folderid.GetSafeInt() == 0)
                            sql = "select * from (" + sql2 + " union all " + sql9 + ") as t1 ";
                        else
                            sql = "select * from (" + sql12 + " union all " + sql11 + ") as t1 ";
                    }
                    // 我共享给别人的
                    else if (foldertype == ShareFolderType.MyShare)
                    {
                        //sql = "select * from (" + sql5 + " union all " + sql6 + ") as t1 where parentid=" + folderid+" ";
                        sql = "select * from (" + sql5 + " union all " + sql6 + ") as t1 ";
                    }
                    else if (foldertype == ShareFolderType.All)
                    {
                        sql = "select * from (" + sql7 + " union all " + sql8 + " union all " + sql1 + " union all " + sql3 + ") as t1 where parentid=0  ";
                    }
                }
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取列表的文件或文件夹记录
        /// </summary>
        /// <param name="isfolder">是否文件夹</param>
        /// <param name="recid">记录号</param>
        /// <returns></returns>
        public IDictionary<string, string> GetShareFileRow(bool isfolder, int recid)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "";
                // 我的文件夹
                if (isfolder)
                    sql = "select 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b where b.imagetype='1' and a.recid=" + recid + "";
                // 我的文件
                else
                    sql = "select 0 as isvirtual,b.recid as fileid,a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where a.recid=" + recid + "";

                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            if (ret.Count > 0)
                return ret[0];
            return null;
        }
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="fileid">文件id</param>
        /// <param name="newname">新文件名</param>
        /// <param name="username">文件所有者帐号</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool RenameShareFileName(int fileid, string newname, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string sql = "update companyfilestorage set filedesc='" + newname + "' where recid in (select fileid from usersharefile where recid=" + fileid + ") and username='" + username + "'";
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
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="folderid">文件夹id</param>
        /// <param name="newname">新文件夹名</param>
        /// <param name="username">文件夹所有者帐号</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool RenameShareFileFolderName(int folderid, string newname, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string sql = "update usersharefilefolder set foldername='" + newname + "' where recid=" + folderid + " and username='" + username + "'";
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
        /// <summary>
        /// 设置父目录
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveShareFileFolderParent(int folderid, int parentid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                IList<UserShareFileFolder> folders = GetShareFolders(username);
                TreeOrder<UserShareFileFolder> tree = new TreeOrder<UserShareFileFolder>(folders, "Recid", "ParentId", "0");
                IList<UserShareFileFolder> trees = tree.GetSubNodes(folders, folderid.ToString());
                var q = from e in trees where e.Recid == parentid select e;
                if (q.Count() > 0)
                {
                    msg = "不能把子目录设置成父目录";
                    ret = false;
                    return ret;
                }
                UserShareFileFolder folder = UserShareFileFolderDao.Get(folderid);
                folder.ParentId = parentid;
                UserShareFileFolderDao.Update(folder);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = "";
            }
            return ret;
        }
        /// <summary>
        /// 设置目录
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveShareFileFileFolder(int fileid, int parentid, string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                UserShareFile file = UserShareFileDao.Get(fileid);
                file.FolderId = parentid;
                UserShareFileDao.Update(file);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = "";
            }
            return ret;
        }

        #endregion

        #region 读者
        /// <summary>
        /// 获取某个实体的读者
        /// </summary>
        /// <param name="entityname"></param>
        /// <param name="endityid"></param>
        /// <returns></returns>
        public IList<CompanyReader> GetCompanyReader(string entityname, string entityid)
        {
            IList<CompanyReader> ret = new List<CompanyReader>();
            try
            {
                ret = CompanyReaderDao.Gets(entityname, entityid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        #endregion

        #region 文件存储
        /// <summary>
        /// 获取某个实体的文件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<CompanyFileStorage> GetCompanyFileStorages(string ids)
        {
            IList<CompanyFileStorage> ret = new List<CompanyFileStorage>();
            try
            {
                ret = CompanyFileStorageDao.Gets(ids);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取某个文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyFileStorage GetCompanyFileStorage(int id)
        {
            CompanyFileStorage ret = null;
            try
            {
                ret = CompanyFileStorageDao.Get(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 根据文件名获取图像和文件描述
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public SysFileImage GetFileImage(string filename, bool ispath)
        {
            SysFileImage ret = null;
            try
            {
                IList<SysFileImage> images = SysFileImageDao.GetAll();

                if (ispath)
                {
                    var q = from e in images where e.ImageType == "1" select e;
                    if (q.Count() > 0)
                        ret = q.First();
                }
                else
                {
                    string ext = System.IO.Path.GetExtension(filename).Trim(new char[] { '.' });
                    ext = "," + ext + ",";
                    var q = from e in images where ("," + e.FileExt + ",").IndexOf(ext, StringComparison.OrdinalIgnoreCase) > -1 select e;
                    if (q.Count() > 0)
                        ret = q.First();
                    else
                    {
                        q = from e in images where e.ImageType == "2" select e;
                        if (q.Count() > 0)
                            ret = q.First();
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public CompanyFileStorage SaveFile(CompanyFileStorage file, out string err)
        {
            err = "";
            try
            {
                file = CompanyFileStorageDao.Save(file);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            return file;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="tablename"></param>
        /// <param name="fieldname"></param>
        /// <param name="recid"></param>
        /// <returns></returns>
        public bool DeleteFile(int fileid, out string err)
        {
            bool ret = true;
            err = "";
            try
            {
                CompanyFileStorageDao.Delete(fileid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 变更公示

        /// <summary>
        /// 获取某个变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyChange GetChange(int id)
        {
            CompanyChange ret = null;
            try
            {
                ret = CompanyChangeDao.Get(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取某次变更项目
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        public IList<CompanyChangeItem> GetCompanyChangeItem(int changeid)
        {
            IList<CompanyChangeItem> ret = null;
            try
            {
                ret = CompanyChangeItemDao.GetItems(changeid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取变更汇总
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyChangeTotal GetChangeToal(int id)
        {
            CompanyChangeTotal ret = null;
            try
            {
                ret = CompanyChangeTotalDao.Get(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取某次变更汇总项目
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        public IList<CompanyChangeTotalItem> GetCompanyChangeTotalItem(int changetotalid)
        {
            IList<CompanyChangeTotalItem> ret = null;
            try
            {
                ret = CompanyChangeTotalItemDao.GetItems(changetotalid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 根据时间筛选变更记录
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        public IList<CompanyChange> GetCompanyChange(string startdate, string enddate)
        {
            IList<CompanyChange> ret = null;
            try
            {
                ret = CompanyChangeDao.GetItems(startdate, enddate);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 统计某次变更的条目和金额
        /// </summary>
        /// <param name="changeid"></param>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="money"></param>
        public void GetCompanyChangeSum(int changeid, int type, out int no, out decimal money)
        {
            no = 0;
            money = 0;

            CompanyChangeItemDao.GetSum(changeid, type, out no, out money);

        }
        /// <summary>
        /// 统计累计的条目和金额
        /// </summary>
        /// <param name="depid"></param>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="money"></param>
        public void GetCompanyChangeAllSum(string depid, int type, out int no, out decimal money)
        {
            no = 0;
            money = 0;

            CompanyChangeItemDao.GetAllSum(depid, type, out no, out money);

        }

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
        public bool SaveCompanyChangeAllSum(int id, out int totalid, string title, string createdby, string createdon, IList<CompanyChangeTotalItem> items, out string msg)
        {
            bool ret = false;
            msg = "";
            totalid = 0;
            try
            {
                CompanyChangeTotal changetotal = null;
                decimal OldMoney = 0;
                decimal TempMoney = 0;
                int CNo = 0;
                decimal CMoney = 0;
                int CTotalNo = 0;
                decimal CTotalMoney = 0;
                int BNo = 0;
                decimal BMoney = 0;
                int BTotalNo = 0;
                decimal BTotalMoney = 0;
                int ANo = 0;
                decimal AMoney = 0;
                int ATotalNo = 0;
                decimal ATotalMoney = 0;
                int BigNo = 0;
                decimal BigMoney = 0;
                int BigTotalNo = 0;
                decimal BigTotalMoney = 0;
                int AlllNo = 0;
                decimal AllMoney = 0;
                int TotalNo = 0;
                decimal TotalMoney = 0;


                if (id > 0)
                {
                    changetotal = GetChangeToal(id);
                    if (changetotal != null)
                    {
                        changetotal.TotoleName = title;
                        changetotal.CreatedBy = createdby;
                        changetotal.CreatedOn = createdon;
                        changetotal.UserName = CurrentUser.UserName;
                        changetotal.RealName = CurrentUser.RealName;

                        CompanyChangeTotalDao.Update(changetotal);
                    }
                    else
                    {
                        changetotal = new CompanyChangeTotal();
                        changetotal.TotoleName = title;
                        changetotal.CreatedBy = createdby;
                        changetotal.CreatedOn = createdon;
                        changetotal.UserName = CurrentUser.UserName;
                        changetotal.RealName = CurrentUser.RealName;

                        CompanyChangeTotalDao.Save(changetotal);



                    }
                    //readers = jss.Deserialize<IList<CompanyChangeTotalItem>>("");
                    //应该差不多了，保存，修改，显示，然后就是直接发通告
                }
                else
                {
                    changetotal = new CompanyChangeTotal();
                    changetotal.TotoleName = title;
                    changetotal.CreatedBy = createdby;
                    changetotal.CreatedOn = createdon;
                    changetotal.UserName = CurrentUser.UserName;
                    changetotal.RealName = CurrentUser.RealName;

                    CompanyChangeTotalDao.Save(changetotal);
                }

                foreach (CompanyChangeTotalItem item in items)
                {
                    item.ChangeTotalid = changetotal.ChangeTotalid;
                    OldMoney = OldMoney + item.OldMoney.GetSafeDecimal();
                    TempMoney = TempMoney + item.TempMoney.GetSafeDecimal();
                    CNo = CNo + item.Cno.GetSafeInt();
                    CMoney = CMoney + item.Cmoney.GetSafeDecimal();
                    CTotalNo = CTotalNo + item.CtotalNo.GetSafeInt();
                    CTotalMoney = CTotalMoney + item.CtotalMoney.GetSafeDecimal();
                    BNo = BNo + item.Bno.GetSafeInt();
                    BMoney = BMoney + item.Bmoney.GetSafeDecimal();
                    BTotalNo = BTotalNo + item.BtotalNo.GetSafeInt();
                    BTotalMoney = BTotalMoney + item.BtotalMoney.GetSafeDecimal();
                    ANo = ANo + item.Ano.GetSafeInt();
                    AMoney = AMoney + item.Amoney.GetSafeDecimal();
                    ATotalNo = ATotalNo + item.AtotalNo.GetSafeInt();
                    ATotalMoney = ATotalMoney + item.AtotalMoney.GetSafeDecimal();
                    BigNo = BigNo + item.BigNo.GetSafeInt();
                    BigMoney = BigMoney + item.BigMoney.GetSafeDecimal();
                    BigTotalNo = BigTotalNo + item.BigTotalNo.GetSafeInt();
                    BigTotalMoney = BigTotalMoney + item.BigTotalMoney.GetSafeDecimal();
                    AlllNo = AlllNo + item.AlllNo.GetSafeInt();
                    AllMoney = AllMoney + item.AllMoney.GetSafeDecimal();
                    TotalNo = TotalNo + item.TotalNo.GetSafeInt();
                    TotalMoney = TotalMoney + item.TotalMoney.GetSafeDecimal();
                    if (item.ChangeTotalItemid > 0)
                        CompanyChangeTotalItemDao.Update(item);
                    else
                        CompanyChangeTotalItemDao.Save(item);
                }

                changetotal.OldMoney = OldMoney.ToString();
                changetotal.TempMoney = TempMoney.ToString();
                changetotal.Cno = CNo;
                changetotal.Cmoney = CMoney;
                changetotal.CtotalNo = CTotalNo;
                changetotal.CtotalMoney = CTotalMoney;
                changetotal.Bno = BNo;
                changetotal.Bmoney = BMoney;
                changetotal.BtotalNo = BTotalNo;
                changetotal.BtotalMoney = BTotalMoney;
                changetotal.Ano = ANo;
                changetotal.Amoney = AMoney;
                changetotal.AtotalNo = ATotalNo;
                changetotal.AtotalMoney = ATotalMoney;
                changetotal.BigNo = BigNo;
                changetotal.BigMoney = BigMoney;
                changetotal.BigTotalNo = BigTotalNo;
                changetotal.BigTotalMoney = BigTotalMoney;
                changetotal.AlllNo = AlllNo;
                changetotal.AllMoney = AllMoney;
                changetotal.TotalNo = TotalNo;
                changetotal.TotalMoney = TotalMoney;

                CompanyChangeTotalDao.Update(changetotal);


                totalid = changetotal.ChangeTotalid;
                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                totalid = 0;
                ret = false;
                msg = e.Message;
            }

            return ret;
        }

        #endregion


        //key为服务器识别，更换服务器后需要改变
        public string GetRecid_Random()
        {
            string key = "1"; //不能为0
            string recid = DateTime.Now.ToString("yyMM") + key + EncryBD.GetRandomString(10);
            return recid;
        }

        #region 通知功能接口
        //设置通知
        /// <summary>
        /// 设置通知
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="title"></param>
        /// <param name="NoticeContent"></param>
        /// <returns></returns>
        public bool SaveAnnouncementNotice(string recid, string title, string NoticeContent)
        {
            string sql = $"INSERT INTO [dbo].[AnnouncementNotice]([RECID],[TITLE],[NoticeContent],[LRRY],LRRYXM,[LRSJ],JCJGBH)" +
                $"VALUES('{GetRecid_Random()}','{title}','{NoticeContent}','{CurrentUser.UserCode}','{CurrentUser.RealName}',getdate(),'{CurrentUser.Qybh}')";
            if (recid != "")
                sql = $"update AnnouncementNotice set TITLE='{title}',NoticeContent='{NoticeContent}' where recid='{recid}'";
            bool code = CommonDao.ExecSql(sql);
            return code;
        }
        /// <summary>
        /// 获取设置通知
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetAnnouncementNotice(string recid)
        {
            string where = "";
            if (recid != "")
                where = $" and recid='{recid}'";
            string sql = $"select * from AnnouncementNotice where jcjgbh='{CurrentUser.Qybh}' {where} order by lrsj desc";
            var dt = CommonDao.GetDataTable(sql);
            return dt;
        }


        //公告上传图片
        public string UploadAnnouncementFile(byte[] upload)
        {
            OSS_CDN oss = new OSS_CDN();
            //byte[] pdffile = Convert.FromBase64String(photo);
            string ossurl = "";
            string msg = "";
            var s = oss.UploadFile(Configs.OssCdnCodeXq, upload, GetRecid_Random() + ".jpg");
            if (s.success)
            {
                ossurl = s.Url;
                msg = ossurl;
            }
            else
            {
                SysLog4.WriteError("上传图片失败：" + s.message);
                throw new Exception("上传图片失败：" + s.message);
            }
            return ossurl;
        }

        public bool DelAnnouncementNotice(string recids)
        {
            string sql = $"delete from AnnouncementNotice where recid in ({recids.FormatSQLInStr()})";
            CommonDao.ExecSql(sql);
            return true;
        }
        #endregion

        #region 人员管理
        /// <summary>
        /// 新增科室
        /// </summary>
        /// <returns></returns>
        public bool CreateJcks(string type, string ksbh, string qybh, string ksmc, string ksdz, string lxdh, string ksys, string kszcode, string kszxm, string jsfzrcode, string jsfzrxm, string zlfzrcode, string zlfzrxm)
        {
            string sql = "";
            if (type == "N")
            {
                ksbh = DateTime.Now.ToString("yyMM") + BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                sql = "INSERT INTO [dbo].[H_JCKS]([SSDWBH],[KSMC],[KSBH],KSDZ,LXDH,KSYS,[XSSX],[KSZCODE],[KSZXM],[JSFZRCODE],[JSFZRXM],[ZLFZRCODE],[ZLFZRXM])" +
                    "select '" + qybh + "','" + ksmc + "','" + ksbh + "','" + ksdz + "','" + lxdh + "','" + ksys + "',1,'" + kszcode + "','" + kszxm + "','" + jsfzrcode + "','" + jsfzrxm + "','" + zlfzrcode + "','" + zlfzrxm + "'";
            }
            else
            {
                sql = "UPDATE H_JCKS SET ksdz='" + ksdz + "',KSMC='" + ksmc + "',lxdh='" + lxdh + "',ksys='" + ksys + "',kszcode='" + kszcode + "',kszxm='" + kszxm + "',jsfzrcode='" + jsfzrcode + "',jsfzrxm='" + jsfzrxm + "',zlfzrcode='" + zlfzrcode + "',zlfzrxm='" + zlfzrxm + "' where ksbh='" + ksbh + "'";
            }

            return CommonDao.ExecSql(sql);
        }

        #endregion

        #region 材料管理
        /// <summary>
        /// 修改材料信息
        /// </summary>
        /// <returns></returns>
        public bool PurchaseOrderModify(string recid, string serialRecid,string materBH, string materId, string materName, string unitId, string unitName,
            string price, string purchasePrice, string quantity, string purpose, string technicalRequirement, string supplier, string manufacturer, string requisitioner)
        {
            //7fcc1d82 - efb2 - 4d9a - 828d - 42665d6f6868
            string sql = $" select Recid from OA_PurchaseOrder  where serialRecid='{serialRecid}'";

            List<string> sqls = new List<string>();
            var data = CommonDao.GetDataTable(sql);
            if (data.Count() > 0)
            {
                recid = data[0]["recid"];
            }
            if (string.IsNullOrEmpty(recid))
            {
                recid = Guid.NewGuid().ToString("N");
                sql = "INSERT INTO [dbo].[OA_PurchaseOrder]([Recid],serialRecid,MaterialBH,[MaterialID],[MaterialName],[MaterialSpecID],[MaterialSpec],[Price]," +
                 "[PurchasePrice],[Quantity],[Purpose],[TechnicalRequirement],[Supplier],[Manufacturer],[Requisitioner],[JCJGBH],[Checker]," +
                 "[CheckTime],[CreateTime],[Creator],[UpdateTime],[Updater],[Status]) " +
                 "VALUES(" +
                 " '" + recid + "', '" + serialRecid + "', " + materBH + ", " + materId + ",  '" + materName + "', " + unitId + ",  '" + unitName + "', " + price + "," + purchasePrice + ", " + quantity + "" +
                 ", '" + purpose + "', '" + technicalRequirement + "', '" + supplier + "',  '" + manufacturer + "',  '" + requisitioner + "',  '" + CurrentUser.Qybh + "'" +
                 ", null, null, getdate(), '" + CurrentUser.RealName + "', getdate(), '" + CurrentUser.RealName + "', 1)";
            }
            else
            {
                sql = string.Format(" update OA_PurchaseOrder set MaterialID='" + materId + "'  ,MaterialName='" + materName + "'" +
                ",MaterialSpecID='" + unitId + "' ,MaterialSpec='" + unitName + "'" +
                ",Price='" + price + "' ,PurchasePrice='" + purchasePrice + "'" +
                ",Quantity='" + quantity + "' ,Purpose='" + purpose + "'" +
                ",technicalRequirement='" + technicalRequirement + "' ,Supplier='" + supplier + "'" +
                ",manufacturer='" + manufacturer + "' ,requisitioner='" + requisitioner + "'" +
                ",UpdateTime=getdate() ,Updater='" + CurrentUser.RealName + "' " +
                " where  recid='" + recid + "'");
            }

            sqls.Add(sql);
            sqls.Add($" update OA_MaterialInfo set LastPrice='{price}',PurchasePrice='{purchasePrice}',Purpose='{purpose}',TechnicalRequirement='{technicalRequirement}'" +
                $",Supplier='{supplier}',Manufacturer='{manufacturer}',Requisitioner='{requisitioner}'" +
                $",UpdateTime=getdate() ,Updater='{CurrentUser.RealName }'" +
                $" where  MaterialBH='{materBH}'");

            return CommonDao.ExecSql(sql);
        }
        #endregion
    }
}
