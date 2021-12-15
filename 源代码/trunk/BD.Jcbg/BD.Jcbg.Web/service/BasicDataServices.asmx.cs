using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.service
{
    /// <summary>
    /// BasicDataServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class BasicDataServices :WebService
    {

        #region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                }
                return _commonService;
            }
        }

        private IJdbgService _jdbgService = null;
        private IJdbgService JdbgService
        {
            get
            {
                if (_jdbgService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jdbgService = webApplicationContext.GetObject("JdbgService") as IJdbgService;
                }
                return _jdbgService;
            }
        }

        private IBasicDataService _basicDataService = null;
        private IBasicDataService BasicDataService
        {
            get
            {
                if (_basicDataService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _basicDataService = webApplicationContext.GetObject("BasicDataService") as IBasicDataService;
                }
                return _basicDataService;
            }
        }
        #endregion

        #region 接口

        #region 人员
        [WebMethod(Description = "获取所有人员信息， 返回信息为json字符串, timestring：时间戳， sign：检验内容, wherestr: 查询条件 （{'rybh':'','ryxm':''}），orderbystr: 排序字符串, needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetAllRYList(string timestring, string sign, string wherestr, string orderbystr="", bool needCompress = true)
        {
            return GetAllList(BasicDataType.RY, timestring, sign, wherestr, orderbystr, needCompress);
        }

        [WebMethod(Description = "获取分页的人员信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （{'rybh':'','ryxm':''})，orderbystr: 排序字符串, pagesize:每页记录数（默认为20），pageindex:页码（从1开始,默认为1），needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetRYList(string timestring, string sign, string wherestr, string pagesize, string pageindex, string orderbystr = "", bool needCompress = true)
        {
            return GetDataList(BasicDataType.RY, timestring, sign, wherestr, pagesize, pageindex, orderbystr, needCompress);
        }
        #endregion

        #region 企业

        [WebMethod(Description = "获取所有企业信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （{'qybh':'','qymc':''})，orderbystr: 排序字符串, needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetAllQYList(string timestring, string sign, string wherestr, string orderbystr = "", bool needCompress = true)
        {
            return GetAllList(BasicDataType.QY, timestring, sign, wherestr, orderbystr, needCompress);
        }


        [WebMethod(Description = "获取分页的企业信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （{'qybh':'','qymc':''})，orderbystr: 排序字符串, pagesize:每页记录数（默认为20），pageindex:页码（从1开始,默认为1），needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetQYList(string timestring, string sign, string wherestr, string pagesize, string pageindex, string orderbystr = "", bool needCompress = true)
        {
            return GetDataList(BasicDataType.QY, timestring, sign, wherestr, pagesize, pageindex, orderbystr, needCompress);
        }
        #endregion

        #region 工程

        [WebMethod(Description = "获取所有工程信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （{'gcbh':'','gcmc':''})，orderbystr: 排序字符串, needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetAllGCList(string timestring, string sign, string wherestr, string orderbystr = "", bool needCompress = true)
        {
            return GetAllList(BasicDataType.GC, timestring, sign, wherestr, orderbystr, needCompress);
        }


        [WebMethod(Description = "获取分页的工程信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （{'gcbh':'','gcmc':''})，orderbystr: 排序字符串, pagesize:每页记录数（默认为20），pageindex:页码（从1开始,默认为1），needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetGCList(string timestring, string sign, string wherestr, string pagesize, string pageindex, string orderbystr = "", bool needCompress = true)
        {
            return GetDataList(BasicDataType.GC, timestring, sign, wherestr, pagesize, pageindex, orderbystr, needCompress);
        }
        #endregion

        #region 设备
        [WebMethod(Description = "获取所有设备信息， 返回信息为json字符串, timestring：时间戳， sign：检验内容, wherestr: 查询条件 （待定），orderbystr: 排序字符串, needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetAllSBList(string timestring, string sign, string wherestr, string orderbystr = "", bool needCompress = true)
        {
            return GetAllList(BasicDataType.SB, timestring, sign, wherestr, orderbystr, needCompress);
        }

        [WebMethod(Description = "获取分页的设备信息， 返回信息为json字符串, timestring：时间戳, sign：检验内容，wherestr: 查询条件 （待定)，orderbystr: 排序字符串, pagesize:每页记录数（默认为20），pageindex:页码（从1开始,默认为1），needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetSBList(string timestring, string sign, string wherestr, string pagesize, string pageindex, string orderbystr = "", bool needCompress = true)
        {
            return GetDataList(BasicDataType.SB, timestring, sign, wherestr, pagesize, pageindex, orderbystr, needCompress);
        }
        #endregion

        #region 获取附件
        [WebMethod(Description = "获取文件信息（包括文件名、文件二进制数据），返回信息为json字符串, timestring：时间戳, sign：检验内容， fileid: 文件id， filetype: 文件类型（filetype=\"big\": 原始文件, filetype=\"small\": 缩略图， 默认值为small），needCompress: 是否需要压缩返回的数据（默认值：是）")]
        public string GetFile(string timestring, string sign, string fileid, string filetype="small", bool needCompress = true)
        {
            string ret = "";
            bool code = true;
            string msg = "";
            byte[] filedata = null;
            string filename = "";
            try
            {
                fileid = fileid.GetSafeRequest();
                filetype = filetype.GetSafeRequest();
                if (fileid == "")
                {
                    code = false;
                    msg = "文件ID不能为空!";
                }
                else
                {

                    string fname = "";
                    if (filetype == "small")
                    {
                        fname = "SMALLCONTENT";
                    }
                    else if (filetype == "big")
                    {
                        fname = "FILECONTENT";
                    }

                    if (fname == "")
                    {
                        code = false;
                        msg = "文件类型错误！";
                    }
                    else
                    {
                        string sql = string.Format("select {0} as thumbattachment,FILENAME from DATAFILE where [FILEID]='{1}'", fname, fileid);
                        IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                        if (dt.Count > 0)
                        {
                            filedata = dt[0]["thumbattachment"] as byte[];
                            filename = dt[0]["filename"].GetSafeString();
                        }

                    }

                }
                
                
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                // 准备需要返回的数据
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("filename", filename);
                if (filedata == null)
                {
                    data.Add("filedata", filedata);
                }
                else
                {
                    data.Add("filedata", Convert.ToBase64String(needCompress ? GZipUtil.Compress(filedata) : filedata));
                }


                ret = GetJson(code, msg, data, false);
            }
            return ret;
        }
        #endregion

        #endregion

        #region 统一入口
        /// <summary>
        /// 获取满足查询条件的所有数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timestring"></param>
        /// <param name="sign"></param>
        /// <param name="wherestr"></param>
        /// <param name="needCompress"></param>
        /// <returns></returns>
        private string GetAllList(BasicDataType type, string timestring, string sign, string wherestr, string orderbystr, bool needCompress = true)
        {
            string ret = "";
            bool code = true;
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            string msg = "";
            try
            {
                if (!CheckSign(timestring.GetSafeString(), sign.GetSafeString()))
                {
                    code = false;
                    msg = "无效【sign】数据！";
                }
                else
                {

                    BasicDataBase where = GetWhere(type, wherestr, out msg);

                    if (msg == "")
                    {
                        code = BasicDataService.GetAllData(type, where, orderbystr, out totalcount, out records, out msg);
                    }
                }


            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                // 准备需要返回的数据
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("total", totalcount);
                data.Add("records", records);

                ret = GetJson(code, msg, data, needCompress);
            }
            return ret;
        }
        /// <summary>
        /// 获取满足查询条件的分页数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timestring"></param>
        /// <param name="sign"></param>
        /// <param name="wherestr"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="needCompress"></param>
        /// <returns></returns>
        private string GetDataList(BasicDataType type, string timestring, string sign, string wherestr, string pagesize, string pageindex, string orderbystr, bool needCompress = true)
        {
            string ret = "";
            bool code = true;
            int totalcount = 0;
            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            string msg = "";
            try
            {
                if (!CheckSign(timestring.GetSafeString(), sign.GetSafeString()))
                {
                    code = false;
                    msg = "无效【sign】数据！";
                }
                else
                {

                    BasicDataBase where = GetWhere(type, wherestr, out msg);

                    if (msg == "")
                    {
                        code = BasicDataService.GetDataList(type, where, orderbystr, pagesize.GetSafeInt(20), pageindex.GetSafeInt(1), out totalcount, out records, out msg);
                    }
                }


            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                // 准备需要返回的数据
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("total", totalcount);
                data.Add("records", records);

                ret = GetJson(code, msg, data, needCompress);
            }
            return ret;
        }

        #endregion

        #region 帮助函数
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        /// <summary>
        /// 检验参数
        /// </summary>
        /// <param name="timestring"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private bool CheckSign(string timestring, string sign)
        {
            //return true;
            bool ret = false;
            string signstr = String.Format("timestring={0}&secret={1}", timestring, "BASICDATA");
            if (MD5Util.StringToMD5Hash(signstr) == sign)
                ret = true;

            return ret;

        }

        /// <summary>
        /// 获取json字符串
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <param name="needCompress"></param>
        /// <returns></returns>
        private string GetJson(bool code, string msg, Dictionary<string, object> data , bool needCompress)
        {
            string ret = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 102400000;

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("code", code.ToString().ToLower());
            row.Add("msg", msg);

            if (needCompress)
            {
                row.Add("data", GZipUtil.CompressString(jss.Serialize(data)));
            }
            else
            {
                row.Add("data", jss.Serialize(data));
            }

            ret = jss.Serialize(row);

            return ret;

        }

        
        private BasicDataBase GetWhere(BasicDataType type, string wherestr, out string msg)
        {
            BasicDataBase where = new BasicDataBase();
            msg = "";
            wherestr = wherestr.GetSafeString();
            try
            {
                switch (type)
                {
                    case BasicDataType.GC:
                        where = wherestr == "" ? new VBasicDataGetGc() : new JsonDeSerializer<VBasicDataGetGc>().DeSerializer(wherestr, out msg);
                        break;
                    case BasicDataType.QY:
                        where = wherestr == "" ? new VBasicDataGetQy() : new JsonDeSerializer<VBasicDataGetQy>().DeSerializer(wherestr, out msg);
                        break;
                    case BasicDataType.RY:
                        where = wherestr == "" ? new VBasicDataGetRy() : new JsonDeSerializer<VBasicDataGetRy>().DeSerializer(wherestr, out msg);
                        break;
                    case BasicDataType.SB:
                        where = wherestr == "" ? new VBasicDataGetSb() : new JsonDeSerializer<VBasicDataGetSb>().DeSerializer(wherestr, out msg);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e )
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            
            return where;
        }
        #endregion

        


    }
}
