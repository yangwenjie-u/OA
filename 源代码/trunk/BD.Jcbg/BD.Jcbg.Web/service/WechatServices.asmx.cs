using System;
using System.Web.Services;
using BD.DataInputCommon;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.Web.RemoteUserService;
using Spring.Context;
using Spring.Context.Support;
using SysLog4 = BD.DataInputCommon.SysLog4;

namespace BD.Jcbg.Web.service
{
    /// <summary>
    /// WechatServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。
    // [System.Web.Script.Services.ScriptService]
    public class WechatServices : WebService
    {

        private IJcService _jcService;
        private IJcService JcService
        {
            get
            {
                try
                {
                    if (_jcService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcService;
            }
        }

        /// <summary>
        /// 校验帐号密码
        /// </summary>
        /// <param name="username">检测系统登录名</param>
        /// <param name="userpass">检测系统登录密码</param>
        /// <returns>0-表示参数错误,1-表示校验成功,-1-表示校验失败.2-表示异常错误.</returns>
        [WebMethod(Description = "描述：校验帐号密码。传入参数：username(检测系统登录名),userpass(检测系统登录密码)。返回值：1-表示校验成功,-1-表示校验失败.2-表示异常错误.")]
        public int CheckUser(string username, string userpass)
        {
            int ret = 1;
            try
            {
                username = System.Net.WebUtility.HtmlEncode(username);
                userpass = System.Net.WebUtility.HtmlEncode(userpass);
                Services userServices = new Services();
                if (!userServices.CheckLogin(username, userpass, Configs.Code))
                {
                    ret = -1;
                }
            }
            catch
            {
                ret = 2;
            }
            return ret;
        }

        /// <summary>
        ///  根据二维码获取报告
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2015-04-07 10:24:50
        [WebMethod(Description = "描述：获取报告生成的图片。返回值：图片url或者文字")]
        public string GetImageUrl(string bgwyh)
        {
            try
            {
                string wtdbh = JcService.GetWtdbh(bgwyh);
                if (string.IsNullOrEmpty(wtdbh))
                {
                    return "当前编号的报告还没有生成,无法查询!";
                }
                if (JcService.IsWtdZf(wtdbh))
                {
                    return "委托单已作废";
                }
                string value = BD.Jcbg.Common.CryptFun.Encode(Configs.UserName + "," + Configs.PassWord + "," + Configs.ViewReport + wtdbh);
                return value;
            }
            catch (Exception)
            {
                return "当前编号的报告还没有生成,无法查询!";
            }
        }

    }
}
