using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net;
using System.Threading;

namespace BD.Jcbg.Bll
{
    public class SxtptService : ISxtptService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        #endregion
        #region 服务
        /// <summary>
        /// 根据我们软件的摄像头编号往平台注册摄像头
        /// </summary>
        /// <param name="deviceserial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Register(string sxtid, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sxtlx from i_m_jcsxt where recid='" + sxtid + "'");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName="";
                    int channelNo = -1;

                    //SysLog4.WriteError("----1");
                    if (!GetSxtInfoYsy(sxtid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    //SysLog4.WriteError("----2");
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;
                    //SysLog4.WriteError("----3");
                    string accessToken = msg;
                    code = RegisterYsyDevice(rooturl, accessToken, deviceSerial, validateCode, out msg);
                    //SysLog4.WriteError("----4");
                    if (code)
                        code = SetYsyDeviceName(rooturl, accessToken, deviceSerial, deviceName, out msg);
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }
        /// <summary>
        /// 根据我摄像头编号，查询摄像头是否在线
        /// </summary>
        /// <param name="deviceserial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool QueryOnline(string sxtid,out string sxtwyh, out string sxtmc, out string msg)
        {
            bool code = false;
            msg = "";
            sxtmc = "";
            sxtwyh = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select recid,sxtlx,sxtmc from i_m_jcsxt where sbxx1='" + sxtid + "' and sfyx=1");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                string recid = dt[0]["recid"].GetSafeString();
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                sxtwyh = recid;
                sxtmc = dt[0]["sxtmc"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName = "";
                    int channelNo = -1;

                    //SysLog4.WriteError("----1");
                    if (!GetSxtInfoYsy(recid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    //SysLog4.WriteError("----2:"+msg);
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;
                    //SysLog4.WriteError("----3:"+msg);
                    code = QueryYsyDeviceOnline(rooturl, msg, deviceSerial, out msg);
                    //SysLog4.WriteError("----4:"+msg);
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }
        /// <summary>
        /// 获取摄像头播放地址
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetPlayUrl(string sxtid, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sxtlx from i_m_jcsxt where recid='" + sxtid + "' and sfyx=1 ");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName = "";
                    int channelNo = -1;
                    if (!GetSxtInfoYsy(sxtid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;
                    code = QueryYsyDevicePlayUrl(rooturl, msg, deviceSerial,channelNo, out msg);
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }
        /// <summary>
        /// 查询摄像头是否在商家平台注册
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsRegister(string sxtid, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sxtlx from i_m_jcsxt where recid='" + sxtid + "'");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName = "";
                    int channelNo = -1;

                    if (!GetSxtInfoYsy(sxtid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;

                    List<VTransSxtYsyRespGetDeviceListItem> datas = null;
                    code = QueryYsyDeviceList(rooturl, msg, out datas, out msg);
                    var q = from e in datas where e.deviceSerial.Equals(deviceSerial, StringComparison.OrdinalIgnoreCase) select e;
                    code = q.Count() > 0;
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }

        /// <summary>
        /// 根据我们软件的摄像头编号移除平台摄像头
        /// </summary>
        /// <param name="deviceserial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Remove(string sxtid, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sxtlx from i_m_jcsxt where recid='" + sxtid + "'");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName = "";
                    int channelNo = -1;

                    if (!GetSxtInfoYsy(sxtid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;
                    string accessToken = msg;
                    code = RemoveYsyDevice(rooturl, accessToken, deviceSerial, out msg);
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }
        
        /// <summary>
        /// 抓拍图片保存到数据库，返回图片id
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <param name="tranType">空表示默认当前事务,1表示开启新事务</param>
        /// <returns></returns>
        [Transaction(ReadOnly=false)]
        public bool CaptuerImage(string sxtid, string usercode, string realname, out string msg, string tranType = "", string wtdbh = "", string zh = "")
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sxtlx from i_m_jcsxt where recid='" + sxtid + "'");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    SysLog4.WriteError(String.Format("平台摄像头编号{0}无效", sxtid));
                    return code;
                }
                string sxtlx = dt[0]["sxtlx"].GetSafeString();
                if (sxtlx == "01")
                {
                    string appkey = "";
                    string secret = "";
                    string rooturl = "";
                    string deviceSerial = "";
                    string validateCode = "";
                    string deviceName = "";
                    int channelNo = -1;

                    if (!GetSxtInfoYsy(sxtid, out appkey, out secret, out rooturl, out deviceSerial, out validateCode, out channelNo, out deviceName, out msg))
                        return code;
                    if (!GetYsyAccessToken(rooturl, appkey, secret, out msg))
                        return code;
                    byte[] image = CaptureYsyImage(rooturl, msg, deviceSerial, channelNo, out msg);
                    if (image == null)
                        return code;

                    string recid = Guid.NewGuid().ToString();
                    string sql = string.Format("insert into UP_SXTZP(TPWYH,SXTBH,WTDBH,SCSJ,SCR,SCRXM,TPNR,SFYX,ZH) values('{0}','{1}','{2}',getdate(),'{3}','{4}',@tpnr,1,'{5}')",
                        recid, sxtid, wtdbh, usercode, realname, zh);
                    IList<IDataParameter> arrParams = new List<IDataParameter>();
                    SqlParameter param = new SqlParameter("@tpnr", SqlDbType.VarBinary) { Value = image };
                    arrParams.Add(param);
                    //图片链问题,线程中不能使用,需要开启新事务
                    if (tranType == "1")
                        code = CommonDao.ExecCommandOpenSession(sql, CommandType.Text, arrParams);
                    else
                        code = CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                    //
                    if (code)
                        msg = recid;
                    else
                        msg = "保存图片到数据库失败";
                }
                else
                {
                    msg = "无效的摄像头类型";
                    return code;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return code;
        }

      

        /// <summary>
        /// 获取某个时间开始的图片
        /// </summary>
        /// <param name="fromTime"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetCaptureImages(string sxtid, string fromTime, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                string sql = "select TPWYH from UP_SXTZP where SXTBH='" + sxtid + "' and SFYX=1 ";
                if (fromTime != "")
                    sql += " and SCSJ>=convert(datetime,'" + fromTime.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                sql += " order by SCSJ desc";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取抓拍的图片内容
        /// </summary>
        /// <param name="tpid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] GetCaptureImageContent(string tpid, out string msg)
        {
            byte[] ret = null;
            msg = "";
            try
            {
                string sql = "select TPNR from UP_SXTZP where TPWYH='" + tpid + "' ";
                IList<IDictionary<string,object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                    ret = dt[0]["tpnr"] as byte[];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;

        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 根据摄像头id， 获取摄像头访问信息
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="sxtlx"></param>
        /// <param name="appkey"></param>
        /// <param name="secret"></param>
        /// <param name="rooturl"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected bool GetSxtInfoYsy(string sxtid, out string appkey, out string secret, out string rooturl, 
            out string deviceSerial, out string validateCode, out int channelNo, out string deviceName, out string msg)
        {
            bool code = false;

            appkey = "";
            secret = "";
            rooturl = "";
            deviceSerial = "";
            validateCode = "";
            channelNo = -1;
            deviceName = "";
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select a.sxtlx,a.sxtmc, a.sbxx1,a.sbxx2,a.sbxx3, b.appkey,b.secret,c.dzxq from I_M_JCSXT a left outer join H_YSY_SECRET b on a.ptrz=b.recid left outer join H_YSY_url c on a.ptdz=c.recid where a.recid='" + sxtid + "'");
                if (dt.Count == 0)
                {
                    msg = "平台摄像头编号无效";
                    return code;
                }
                appkey = dt[0]["appkey"].GetSafeString();
                secret = dt[0]["secret"].GetSafeString();
                
                if (appkey == "" || secret == "")
                {
                    msg = "平台认证信息为空";
                    return code;
                }
                rooturl = dt[0]["dzxq"].GetSafeString();
                if (rooturl == "")
                {
                    msg = "平台根目录为空";
                    return code;
                }
                deviceSerial = dt[0]["sbxx1"].GetSafeString();
                if (deviceSerial == "")
                {
                    msg = "设备序列号为空";
                    return code;
                }
                validateCode = dt[0]["sbxx2"].GetSafeString();
                if (validateCode == "")
                {
                    msg = "设备验证码为空";
                    return code;
                }
                channelNo = dt[0]["sbxx3"].GetSafeInt(-1);
                if (channelNo == -1)
                {
                    msg = "通道值无效";
                    return code;
                }
                deviceName = dt[0]["sxtmc"].GetSafeString();
                if (deviceName == "")
                {
                    msg = "摄像头名称为空";
                    return code;
                }
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
        /// 从远程下载图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private byte[] DownImage(string url, out string msg)
        {
            byte[] ret = null;
            msg = "";
            try
            {
                WebClient client = new WebClient();
                ret = client.DownloadData(url);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion
        #region 萤石云访问

        #region 获取访问令牌
        private static IDictionary<string, VTransSxtYsyRespGetAccessTokenData> YsyTokenInfo = new Dictionary<string, VTransSxtYsyRespGetAccessTokenData>(StringComparer.OrdinalIgnoreCase);

        private bool GetYsyAccessToken(string root, string appkey, string secret, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                bool exists = false;
                VTransSxtYsyRespGetAccessTokenData token = new VTransSxtYsyRespGetAccessTokenData();
                if (YsyTokenInfo.TryGetValue(root, out token))
                {
                    if (token.expireTime.GetSafeLong().GetTimeFormUtcMs() > DateTime.Now)
                        exists = true;
                    else
                        YsyTokenInfo.Remove(root);
                }
                if (!exists)
                {
                    IDictionary<string, string> queryParams = new Dictionary<string, string>();
                    queryParams.Add("appKey", appkey);
                    queryParams.Add("appSecret", secret);

                    code = MyHttp.Post("https://" + root + "/api/lapp/token/get", queryParams, out msg);
                    VTransSxtYsyRespGetAccessToken resp = new VTransSxtYsyRespGetAccessToken();
                    if (code)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        resp = jss.Deserialize<VTransSxtYsyRespGetAccessToken>(msg);
                        YsyTokenInfo.Add(root, resp.data);
                    }
                }

                if (YsyTokenInfo.TryGetValue(root, out token))
                {
                    msg = token.accessToken;
                    code = true;
                }
                else
                {
                    msg = "获取访问令牌失败";
                    code = false;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion

        #region 注册设备
        private bool RegisterYsyDevice(string root, string accessToken, string deviceSerial, string validateCode, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);
                queryParams.Add("validateCode", validateCode);

                code = MyHttp.Post("https://" + root + "/api/lapp/device/add", queryParams, out msg);
                //SysLog4.WriteError("----accesstoken:"+accessToken+",deviceserial:"+deviceSerial+",vilidatecode:"+validateCode+",return:" + msg);
                VTransSxtYsyRespBase resp = new VTransSxtYsyRespBase();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespBase>(msg);
                    code = resp.code.GetSafeInt() == 200;
                    if (!code)
                        msg = resp.msg;
                    else
                        msg = "";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion
        #region 修改设备名称
        private bool SetYsyDeviceName(string root, string accessToken, string deviceSerial, string deviceName, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);
                queryParams.Add("deviceName", deviceName);

                code = MyHttp.Post("https://" + root + "/api/lapp/device/name/update", queryParams, out msg);
                VTransSxtYsyRespBase resp = new VTransSxtYsyRespBase();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespBase>(msg);
                    code = resp.code.GetSafeInt() == 200;
                    if (!code)
                        msg = resp.msg;
                    else
                        msg = "";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion
        #region 查询设备信息
        private bool QueryYsyDeviceOnline(string root, string accessToken, string deviceSerial, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);

                code = MyHttp.Post("https://" + root + "/api/lapp/device/info", queryParams, out msg);
                VTransSxtYsyRespGetDeviceInfo resp = new VTransSxtYsyRespGetDeviceInfo();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespGetDeviceInfo>(msg);
                    code = resp.code.GetSafeInt() == 200;

                    if (!code)
                        msg = resp.msg;
                    else
                    {
                        code = resp.data.status != 0;
                        msg = "";
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion
        #region 获取播放地址
        private bool QueryYsyDevicePlayUrl(string root, string accessToken, string deviceSerial, int channelNo, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);
                queryParams.Add("channelNo", channelNo.ToString());
                queryParams.Add("expireTime", "3600");
                //SysLog4.WriteError("accessToken:" + accessToken + ",deviceSerial:" + deviceSerial + ",channelNo:" + channelNo+ ",expireTime:");

                code = MyHttp.Post("https://" + root + "/api/lapp/live/address/limited", queryParams, out msg);
                VTransSxtYsyRespGetDevicePlayUrl resp = new VTransSxtYsyRespGetDevicePlayUrl();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespGetDevicePlayUrl>(msg);
                    code = resp.code.GetSafeInt() == 200;

                    if (!code)
                        msg = resp.msg;
                    else
                    {
                        if (resp.data.status == 0)
                        {
                            code = false;
                            msg = "获取地址失败";
                        }
                        else
                        {
                            code = true;
                            msg = resp.data.liveAddress;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion
        #region 获取设备列表
        private bool QueryYsyDeviceList(string root, string accessToken, out List<VTransSxtYsyRespGetDeviceListItem> datas,
            out string msg)
        {
            bool code = false;
            msg = "";
            datas = new List<VTransSxtYsyRespGetDeviceListItem>();
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("pageStart", "0");
                queryParams.Add("pageSize", "50");

                while (true)
                {
                    code = MyHttp.Post("https://" + root + "/api/lapp/device/list", queryParams, out msg);
                    VTransSxtYsyRespGetDeviceList resp = new VTransSxtYsyRespGetDeviceList();
                    if (code)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        resp = jss.Deserialize<VTransSxtYsyRespGetDeviceList>(msg);
                        code = resp.code.GetSafeInt() == 200;

                        if (!code)
                        {
                            msg = resp.msg;
                            break;
                        }
                        else
                        {
                            datas.AddRange(resp.data);
                            if ((resp.page.page + 1) * resp.page.size >= resp.page.total)
                                break;
                            queryParams["pageStart"] = (resp.page.page + 1).ToString();
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion
        #region 从平台移除摄像头
        private bool RemoveYsyDevice(string root, string accessToken, string deviceSerial, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);

                code = MyHttp.Post("https://" + root + "/api/lapp/device/delete", queryParams, out msg);
                VTransSxtYsyRespBase resp = new VTransSxtYsyRespBase();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespBase>(msg);
                    code = resp.code.GetSafeInt() == 200;
                    if (!code)
                        msg = resp.msg;
                    else
                        msg = "";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        #endregion

        #region 抓拍图片
        private byte[] CaptureYsyImage(string root, string accessToken, string deviceSerial, int channelNo, out string msg)
        {
            byte[] ret = null;
            msg = "";
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("accessToken", accessToken);
                queryParams.Add("deviceSerial", deviceSerial);
                queryParams.Add("channelNo", channelNo.ToString());

                bool code = MyHttp.Post("https://" + root + "/api/lapp/device/capture", queryParams, out msg);
                VTransSxtYsyRespCaptureImage resp = new VTransSxtYsyRespCaptureImage();
                if (code)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<VTransSxtYsyRespCaptureImage>(msg);
                    code = resp.code.GetSafeInt() == 200;

                    if (!code)
                        msg = resp.msg;
                    else
                        ret = DownImage(resp.data.picUrl, out msg);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        
        #endregion
        #endregion

        #region 摄像头抓拍现场
        //系统启动加载摄像头
        public void InitThread()
        {
            //判断是否有未启动线程
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select * from I_M_SXT_THREAD where isnull(JSSJ,'')=''");
            //如果没有未启动的，退出
            if (dt.Count == 0)
                return;
            //摄像头信息
            string sxtxx = "";
            string wtdwyh = "";
            string zh = "";
            string username = "";
            string realname = "";
            //循环启动未处理图片线程
            foreach(var item in dt)
            {
                //设置数据
                sxtxx = item["SXTXX"].GetSafeString();
                wtdwyh = item["WTDWYH"];
                zh = item["ZH"];
                username = item["USERNAME"];
                realname = item["REALNAME"];
                //判断摄像头信息是否存在
                if (sxtxx == "")
                    continue;
                try
                {
                    //开启每个线程
                    IList<VTransXcjcReqStartItem> useSxts = JsonSerializer.Deserialize<List<VTransXcjcReqStartItem>>(sxtxx);
                    if(useSxts.Count == 0)
                        continue;
                    ThreadStartSxt(useSxts, username, realname, wtdwyh, zh, true);
                }
                catch (Exception e)
                {
                    SysLog4.WriteError(String.Format("摄像头初始化出错，原因：{0}", e.Message));
                }                   
            }
        }

        protected List<SxtThreadList> m_SxtThreadslist = new List<SxtThreadList>();

        /// <summary>
        /// 开始摄像头
        /// </summary>
        /// <param name="useSxts"></param>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="zh"></param>
        /// <param name="ck">是否是初始化重启使用</param>
        public void ThreadStartSxt(object useSxts, string username, string realname, string wtdwyh, string zh, bool ck = false)
        {
            //线程
            SxtPZ pz = new SxtPZ();
            pz.useSxts = (IList<VTransXcjcReqStartItem>)useSxts;
            //判断是否有摄像头
            if (pz.useSxts.Count == 0)
                return;
            pz.username = username;
            pz.realname = realname;
            pz.wtdwyh = wtdwyh;
            pz.zh = zh;
            Thread th = new Thread(new ParameterizedThreadStart(ThreadMethod)); //创建线程                     
            th.Start(pz); //启动线程

            SxtThreadList t_sxtthread = new SxtThreadList();
            t_sxtthread.m_SxtThread = th;
            t_sxtthread.key = wtdwyh;
            t_sxtthread.zh = zh;
            m_SxtThreadslist.Add(t_sxtthread);

            try
            {
                //插入SQL
                string sql = String.Format("insert into I_M_SXT_THREAD(WTDWYH, ZH, USERNAME, REALNAME, SXTXX, KSSJ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", wtdwyh, zh, username, realname, JsonSerializer.Serialize(pz.useSxts), TimeUtil.GetDateTime());
                CommonDao.ExecSql(sql);
            }
            catch (Exception e)
            {
               SysLog4.WriteError(String.Format("开始摄像头线程出错，原因：{0}", e.Message));
            }        
        }
        private void ThreadMethod(object B)
        {
            SxtPZ spz = (SxtPZ)B;
            IList<VTransXcjcReqStartItem> useSxts = spz.useSxts;
            while (true)
            {
                for (int i = 0; i < useSxts.Count; i++)
                {
                    string msg = "";
                    bool code = false;
                    code = CaptuerImage(useSxts[i].sxtbh, spz.username, spz.realname, out msg, "1", spz.wtdwyh, spz.zh);
                    if (code)
                    {
                        string getmsg = "";
                        byte[] filebytes =null;
                        filebytes=GetCaptureImageContent(msg, out getmsg);
                        if(filebytes!=null)
                        {
                            UploadJPPic(filebytes, spz.wtdwyh, useSxts[i].sxtbh, "", spz.zh);
                        }
                    }
                }
                //等待15分钟抓图
                Thread.Sleep(60000 * 15);
            }
        }

        /// <summary>
        /// 结束摄像头线程
        /// </summary>
        /// <param name="wtdwyh">委托单唯一号</param>
        /// <param name="zh">组号</param>
        public void DropSxtThread(string wtdwyh, string zh)
        {
            foreach (SxtThreadList o_thread in m_SxtThreadslist)
            {
                //判断唯一号及桩号是否一致
                if(o_thread.key==wtdwyh && o_thread.zh == zh)
                {
                    try
                    {
                        //插入SQL
                        string sql = String.Format("update I_M_SXT_THREAD set JSSJ = '{0}' where WTDWYH = '{1}' and ZH = '{2}'", TimeUtil.GetDateTime(), wtdwyh, zh);
                        CommonDao.ExecSql(sql);
                    }
                    catch (Exception e)
                    {
                        SysLog4.WriteError(String.Format("结束摄像头线程出错，原因：{0}", e.Message));
                    }       
                    //线程
                    Thread thread = o_thread.m_SxtThread;
                    thread.Abort();
                    m_SxtThreadslist.Remove(o_thread);
                }

            }
        }

        /// <summary>
        /// 上传抓拍图片
        /// </summary>
        /// <param name="filebytes"></param>
        /// <param name="tpwyh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>

        private void UploadJPPic(byte[] filebytes, string tpwyh, string sxtbh,string tplx, string zh)
        {
            string tpxqwyh = Guid.NewGuid().ToString();
            var sql = string.Format("insert into UP_XCCJTP ([WTDWYH],[CONTEXT] ,[IMGURL] ,[URL],[RQ],[COLOR],[SXTBH] ,[TYPE],[ZH]) values('{0}','',@ossCdnUrl,'',convert(nvarchar(20),getdate(),20),'','{1}', '', '{2}')",
                 tpwyh, sxtbh, zh);

            IList<IDataParameter> arrParams = new List<IDataParameter>();

            try
            {
                OSS_CDN oss = new OSS_CDN();
                var result = oss.UploadFile(Configs.OssCdnCodeJz, filebytes, string.Format("jz_{0}.jpg", tpxqwyh));

                //如果没有上传到OSS上，通过后台线程去上传
                if (result.success)
                {
                    arrParams.Add(new SqlParameter("@ossCdnUrl", result.Url));
                }
                else
                {
                    arrParams.Add(new SqlParameter("@ossCdnUrl", string.Empty));
                }
            }
            catch
            {
                arrParams.Add(new SqlParameter("@ossCdnUrl", string.Empty));
                SysLog4.WriteError(string.Format("[{0}]见证图片上传到OSS失败,改为上传到数据库中", tpxqwyh));
            }

            CommonDao.ExecCommandOpenSession(sql, CommandType.Text, arrParams);

            //sw.Stop();
            //SysLog4.WriteError(result.Url + "上传见证图片耗时:" + sw.ElapsedMilliseconds + "毫秒\r\n");
        } 
        #endregion
    }
}
