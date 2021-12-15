using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using System.Text.RegularExpressions;
using System.Collections;

namespace BD.Jcbg.Web.Func.SCXPT
{
    public static class ZJJGPublicData
    {
        /// <summary>
        /// 平台接口基础URL
        /// </summary>
        private static string baseurl = "";

        /// <summary>
        /// 账号
        /// </summary>
        private static string UserName = "";
        /// <summary>
        /// 密码
        /// </summary>
        private static string PassWord = "";

        /// <summary>
        /// 固定参数
        /// </summary>
        private static string callback = "";

        /// <summary>
        /// 返回固定的URL参数
        /// </summary>
        /// <returns></returns>
        public static string GetFixedParams()
        {
            Init();
            return string.Format("UserName={0}&PassWord={1}&callback={2}", HttpUtility.UrlDecode(UserName), HttpUtility.UrlDecode(PassWord), HttpUtility.UrlDecode(callback));
        }

        /// <summary>
        /// 根据企业名称获取企业基本信息
        /// 包含：企业基本信息、外资情况、企业下属公司情况、企业简历、资质证书信息
        /// </summary>
        /// <param name="CorpName">企业名称（必填）</param>
        /// <param name="SCUCode">企业统一社会信用代码（非必填）</param>
        /// <param name="msg"></param>
        /// <param name="basicinfo"></param>
        /// <returns></returns>
        public static bool GetCorpBasicInfo(string CorpName, string SCUCode, out string msg, out object basicinfo)
        {
            bool ret = true;
            basicinfo = null;
            msg = "";
            try
            {
                string method = "getcorpbasicinfo";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "CorpName", CorpName},
                    { "SCUCode", SCUCode}
                };
                

                ret = GetPublicInterfaceData(method, data, out msg, out basicinfo);
                
            }
            catch (Exception e )
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
                
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CorpName">企业名称（必填）</param>
        /// <param name="SCUCode">企业统一社会信用代码（非必填）</param>
        /// <param name="msg"></param>
        /// <param name="personinfo"></param>
        /// <returns></returns>
        public static bool GetCorpPersonInfo(string CorpName, string SCUCode, out string msg, out object personinfo)
        {
            bool ret = true;
            personinfo = null;
            msg = "";
            try
            {
                string method = "getcorppersonsinfo";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "CorpName", CorpName},
                    { "SCUCode", SCUCode}
                };               

                ret = GetPublicInterfaceData(method, data, out msg, out personinfo);
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }

        /// <summary>
        /// 根据人员的RowGuid和UserGuid获取人员的基本信息、执业信息、资格信息、简历信息、岗位信息、技术职称信息
        /// </summary>
        /// <param name="rowguid">关联主键</param>
        /// <param name="userguid">人员guid</param>
        /// <param name="msg"></param>
        /// <param name="userrecordinfo"></param>
        /// <returns></returns>
        public static bool GetUserRecordInfo(string rowguid, string userguid, out string msg, out object userrecordinfo)
        {
            bool ret = true;
            userrecordinfo = null;
            msg = "";
            try
            {
                string method = "getuserrecordinfo";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "rowguid", rowguid},
                    { "userguid", userguid}
                };

                ret = GetPublicInterfaceData(method, data, out msg, out userrecordinfo);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }

        /// <summary>
        /// 根据项目代码获取工程项目详细信息、招投标信息、合同备案信息、施工许可信息、项目参与主体信息、竣工验收信息
        /// </summary>
        /// <param name="prjcode">项目guid</param>
        /// <param name="msg"></param>
        /// <param name="projectinfo"></param>
        /// <returns></returns>
        public static bool GetProjectInfo(string prjguid,  out string msg, out object projectinfo)
        {
            bool ret = true;
            projectinfo = null;
            msg = "";
            try
            {
                string method = "getprojectinfo";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "prjguid", prjguid}
                    
                };

                ret = GetPublicInterfaceData(method, data, out msg, out projectinfo);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }


        /// <summary>
        /// 传入页码和每页数量分页获取企业备案信息
        /// </summary>
        /// <param name="CorpName">企业名称</param>
        /// <param name="CorpCode">组织机构代码或统一社会信用代码</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="pageindex">当前页页数</param>
        /// <param name="msg"></param>
        /// <param name="basiclist"></param>
        /// <returns></returns>
        public static bool GetCorpBasicList(string CorpName, string CorpCode,int pagesize, int pageindex, out string msg, out object basiclist)
        {
            bool ret = true;
            basiclist = null;
            msg = "";
            try
            {
                string method = "getprojectinfo";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "CorpName", CorpName},
                    { "CorpCode", CorpCode},
                    { "pagesize", pagesize.ToString()},
                    { "pageindex", pageindex.ToString()}

                };

                ret = GetPublicInterfaceData(method, data, out msg, out basiclist);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }
        /// <summary>
        /// 根据企业名称获取参与项目信息
        /// </summary>
        /// <param name="CorpName">企业名称</param>
        /// <param name="msg"></param>
        /// <param name="projectlist"></param>
        /// <returns></returns>
        public static bool GetCorpProjectList(string CorpName, out string msg, out object projectlist)
        {
            bool ret = true;
            projectlist = null;
            msg = "";
            try
            {
                string method = "getcorpprjlist";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "CorpName", CorpName}
                };

                ret = GetPublicInterfaceData(method, data, out msg, out projectlist);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }
        /// <summary>
        /// 根据企业ToRowGuid获取企业附件信息
        /// </summary>
        /// <param name="prjguid"></param>
        /// <param name="msg"></param>
        /// <param name="projectinfo"></param>
        /// <returns></returns>
        public static bool GetCorpFileList(string ToRowGuid, out string msg, out object corpfilelist)
        {
            bool ret = true;
            corpfilelist = null;
            msg = "";
            try
            {
                string method = "getcorpfilelist";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "ToRowGuid", ToRowGuid}

                };

                ret = GetPublicInterfaceData(method, data, out msg, out corpfilelist);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }

        public static bool GetPersonFileList(string ToRowguid, string UserGuid, string IDCard, out string msg, out object personfilelist)
        {
            bool ret = true;
            personfilelist = null;
            msg = "";
            try
            {
                string method = "getpersonfilelist";
                Dictionary<string, string> data = new Dictionary<string, string>() {
                    { "ToRowguid", ToRowguid},
                    { "UserGuid", UserGuid},
                    { "IDCard", IDCard}

                };

                ret = GetPublicInterfaceData(method, data, out msg, out personfilelist);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);

            }
            return ret;
        }

        public static void Init()
        {
            baseurl = GlobalVariable.GetConfigValue("zjjg_baseurl");
            UserName = GlobalVariable.GetConfigValue("zjjg_username");
            PassWord = GlobalVariable.GetConfigValue("zjjg_password");
            callback = GlobalVariable.GetConfigValue("zjjg_callback");
        }
        private static bool GetPublicInterfaceData(string method, Dictionary<string, string> postdata, out string msg, out object data)
        {
            bool ret = true;
            msg = "";
            data = null;
            try
            {
                Init();

                string url = baseurl + method;
                //SysLog4.WriteError(url);
                //SysLog4.WriteError(method);
                Dictionary<string, string> realdata = new Dictionary<string, string>();
                realdata.Add("UserName", UserName);
                realdata.Add("PassWord", PassWord);
                realdata.Add("callback", callback);
                if (postdata != null && postdata.Count > 0)
                {
                    foreach (var item in postdata)
                    {
                        realdata.Add(item.Key, item.Value);
                    }
                }

                string postdatastr = "";
                foreach (var item in realdata)
                {
                    if (postdatastr != "")
                    {
                        postdatastr += "&";
                    }
                    postdatastr += item.Key + "=" + HttpUtility.UrlEncode(item.Value);

                }
                url = url + "?" + postdatastr;
                //SysLog4.WriteError(url);
                //SysLog4.WriteError(postdatastr);

                string retdata = MyHttp.SendDataByGET(url);
                //SysLog4.WriteError(retdata);
                if (retdata !=null && retdata!="")
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(retdata);
                    if (dt!=null && dt.Count > 0)
                    {
                        int code = dt["Code"].GetSafeInt();
                        // 返回错误
                        if (code ==1)
                        {
                            ret = true;
                            msg = "";
                            data = dt["Data"];
                        }
                        else
                        {
                            ret = false;
                            msg = dt["Message"].GetSafeString();
                        }
                        
                    }
                    else
                    {
                        ret = false;
                        msg = "获取数据为空！";
                    }
                }
                else
                {
                    ret = false;
                    msg = "请求数据失败！";
                }

                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }
    }
}