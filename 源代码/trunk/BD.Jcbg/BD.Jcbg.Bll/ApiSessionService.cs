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
using System.Web;
using System.Web.Caching;

namespace BD.Jcbg.Bll
{
	public class ApiSessionService : IApiSessionService
	{
		#region 数据库对象
		public ICommonDao CommonDao { get; set; }
        public ISysSessionDao SysSessionDao { get; set; }

		#endregion

        #region 缓存变量名
        private const string KEY_TIME_OUT = "ApiSessionService_TimeOut";        // session有效期缓存关键字
        private const string KEY_SESSION_PREFIX = "ApiSessionService_Session_"; // 用户session前缀
        #endregion

        #region Session服务
        /// <summary>
        /// 添加session并缓存
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetSession(string username, string password, string usercode, string realname, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                SysSession exists = SysSessionDao.GetByUser(username);
                if (exists != null)
                    SysSessionDao.Delete(exists);
                SysSession session = new SysSession()
                {
                    LoginTime = DateTime.Now,
                    Password = password.EncodeDes(),
                    UserName = username,
                    UserCode = usercode,
                    RealName = realname,
                    SessionId = Guid.NewGuid().ToString()
                };
                SysSessionDao.Save(session);
                HttpRuntime.Cache.Insert(KEY_SESSION_PREFIX + session.SessionId, session);
                
                code = true;
                msg = session.SessionId;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }
        /// <summary>
        /// 根据sessionid获取未过期的session对象
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public SysSession GetSessionUser(string sessionid, out string msg)
        {
            SysSession ret = null;
            msg = "";
            try
            {
                SysSession session = HttpRuntime.Cache.Get(KEY_SESSION_PREFIX + sessionid) as SysSession;
                if (session == null)
                {
                    session = SysSessionDao.Get(sessionid);
                    if (session != null)
                    {
                        if (session.LoginTime.AddHours(GetExpireHour()) < DateTime.Now)
                        {
                            SysSessionDao.Delete(sessionid);
                            session = null;
                        }
                    }
                    if (session != null)
                        HttpRuntime.Cache.Insert(KEY_SESSION_PREFIX + sessionid, session);
                }
                ret = session;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DelSession(string sessionid, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //删除数据库中会话
                SysSession session = SysSessionDao.Get(sessionid);
                if(session != null)
                    SysSessionDao.Delete(sessionid);
                //删除缓存中会话
                session = HttpRuntime.Cache.Get(KEY_SESSION_PREFIX + sessionid) as SysSession;
                if (session != null)
                    HttpRuntime.Cache.Remove(KEY_SESSION_PREFIX + sessionid);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }

        /// <summary>
        /// 加载所有session
        /// </summary>
        /// <returns></returns>
        public IList<SysSession> GetAllSession()
        {
            IList<SysSession> sessions = new List<SysSession>();
            try
            {
                sessions = SysSessionDao.GetAll();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return sessions;
        }

        public void DeleteExpireSessions(int expirehours)
        {
            try
            {
                SysSessionDao.DeleteExpire(expirehours);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }
        /// <summary>
        /// 获取sess过期时间，小时
        /// </summary>
        /// <returns></returns>
        
        private int GetExpireHour()
        {
            int ret = 24;
            try
            {
                if (HttpRuntime.Cache.Get(KEY_TIME_OUT) == null)
                {                    
                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select settingvalue from syssetting where settingcode='API_SESSION_TIMEOUT'");
                    if (dt.Count > 0)
                        ret = dt[0].GetSafeInt(24);
                    HttpRuntime.Cache.Insert(KEY_TIME_OUT, ret);
                }
                ret = HttpRuntime.Cache.Get(KEY_TIME_OUT).GetSafeInt(24);
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
