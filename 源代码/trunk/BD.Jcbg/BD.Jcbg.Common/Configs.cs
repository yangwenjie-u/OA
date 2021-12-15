using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 配置类
    /// </summary>
    public static class Configs
    {
        /// <summary>
        /// 获取配置项值
        /// </summary>
        /// <returns></returns>
        public static string GetConfigItem(string name, string xmlName = "configs.xml")
        {
            XDocument document = XDocument.Load(string.Format(@"{0}\configs\" + xmlName, SysEnvironment.CurPath));
            var query = from m in document.Elements("configs").Elements(name)
                        select m;

            if (query.FirstOrDefault() == null)
            {
                return "";
            }

            return query.First().Value;
        }

        #region 微信配置文件

        /// <summary>
        /// 用户权限代码
        /// </summary>
        public static string Code
        {
            get { return GetConfigItem("code", "wechat.xml"); }
        }

        /// <summary>
        /// 默认用户账号
        /// </summary>
        public static string UserName
        {
            get { return GetConfigItem("username", "wechat.xml"); }
        }

        /// <summary>
        /// 默认用户密码
        /// </summary>
        public static string PassWord
        {
            get { return GetConfigItem("password", "wechat.xml"); }
        }

        /// <summary>
        /// 查看报告的URL
        /// </summary>
        public static string ViewReport
        {
            get { return GetConfigItem("viewreport", "wechat.xml"); }
        }

        /// <summary>
        /// 查看报告服务器
        /// </summary>
        public static string ViewHost
        {
            get { return GetConfigItem("viewhost", "wechat.xml"); }
        }

        #endregion
        /// <summary>
        /// 网站标题
        /// </summary>
        public static string Title
        {
            get { return GetConfigItem("title"); }
        }
        /// <summary>
        /// 用户默认密码
        /// </summary>
        public static string DefaultPassword
        {
            get { return GetConfigItem("defpass"); }
        }
        /// <summary>
        /// 视频格式
        /// </summary>
        public static string VideoExt
        {
            get { return GetConfigItem("videoext"); }
        }


        public static string AppId
        {
            get { return GetConfigItem("appid"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string AppId2
        {
            get { return GetConfigItem("appid2"); }
        }

        public static int SignMaxWidth
        {
            get { return GetConfigItem("signwidth").GetSafeInt(); }
        }

        public static int SignMaxHeight
        {
            get { return GetConfigItem("signheight").GetSafeInt(); }
        }

        public static string PageVersion
        {
            get { return GetConfigItem("pageversion"); }
        }
		public static string GetOaServerIp
        {
            get { return GetConfigItem("oaserver"); }
        }
        public static string GetLzzgyRole
        {
            get { return GetConfigItem("lzzgyrole"); }
        }
        public static string FileShareExecudeDeps
        {
            get { return GetConfigItem("fileshareexecudedeps"); }
        }

        public static string FileOssViewPath
        {
            get { return GetConfigItem("FileOssViewPath"); }
        }

        public static string FileOssViewVideoPath
        {
            get
            {
                return FileOssViewPath + "?t=video&id=";
            }
        }

        public static string LabAssTimeTagCompare
        {
            get { return GetConfigItem("LabAssTimeTagCompare"); }
        }
        /// <summary>
        /// 支付平台地址
        /// </summary>
        public static string PayInterfaceUrl
        {
            get { return GetConfigItem("payinterfaceurl"); }
        }

        #region 图片转换
        #region 原图
        public static bool OrgImageDeal
        {
            get { return GetConfigItem("OrgImageDeal").GetSafeBool(); }
        }
        public static int OrgImageWidth
        {
            get { return GetConfigItem("OrgImageWidth").GetSafeInt(); }
        }
        public static int OrgImageHeight
        {
            get { return GetConfigItem("OrgImageHeight").GetSafeInt(); }
        }
        public static bool OrgImageZoomOut
        {
            get { return GetConfigItem("OrgImageZoomOut").GetSafeBool(); }
        }
        public static bool OrgImageLockRate
        {
            get { return GetConfigItem("OrgImageLockRate").GetSafeBool(); }
        }
        #endregion
        #region 缩略图
        public static int ThumbImageWidth
        {
            get { return GetConfigItem("ThumbImageWidth").GetSafeInt(); }
        }
        public static int ThumbImageHeight
        {
            get { return GetConfigItem("ThumbImageHeight").GetSafeInt(); }
        }
        public static bool ThumbImageZoomOut
        {
            get { return GetConfigItem("ThumbImageZoomOut").GetSafeBool(); }
        }
        public static bool ThumbImageLockRate
        {
            get { return GetConfigItem("ThumbImageLockRate").GetSafeBool(); }
        }
        #endregion
        #region 缩略图

        #endregion
        #endregion
        /// <summary>
        /// 从平台最后同步数据的时间
        /// </summary>
        public static string LastSyncTime
        {
            get
            {
                string orgStr = GetConfigItem("lastsynctime");
                DateTime dt = orgStr.GetSafeDate();
                if (dt.Year.Equals(1900))
                    return "";
                return orgStr; 
            }
            set
            {
                SetConfigItem("lastsynctime", value);

            }
        }

        public static void SetConfigItem(string name, string value, string xmlName = "configs.xml")
        {
            try
            {
                string filepath = string.Format(@"{0}\configs\" + xmlName, SysEnvironment.CurPath);
                XDocument document = XDocument.Load(filepath);
                var query = from m in document.Elements("configs").Elements(name)
                            select m;
                if (query.Count() > 0)
                {
                    XElement ele = query.First<XElement>();
                    ele.Value = value;
                    document.Save(filepath);
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }
        /// <summary>
        /// 支付平台用到务工人员设置地址
        /// </summary>
        public static string WgryPlatformUrl
        {
            get { return GetConfigItem("wgryplatformurl"); }
        }
        /// <summary>
        /// 打印服务地址
        /// </summary>
        public static string PrintServiceUrl
        {
            get
            {
                return GetConfigItem("printserviceurl");
            }
        }

        #region redis连接
        /// <summary>
        /// redis连接
        /// </summary>
        public static string RedisConnection
        {
            get
            {
                return GetConfigItem("redisConnection");
            }
        }
        #endregion

        #region 微信公众号
        /// <summary>
        /// 微信公众号APPID
        /// </summary>
        public static string WxAppid
        {
            get
            {
                return GetConfigItem("WxAppid");
            }
        }

        /// <summary>
        /// 微信公众号Secret
        /// </summary>
        public static string WxSecret
        {
            get
            {
                return GetConfigItem("WxSecret");
            }
        }
        #endregion

        /// <summary>
        /// 见证图片,报告OssCdn地址
        /// </summary>
        public static string FileOssCdn
        {
            get
            {
                return GetConfigItem("FileOssCdn");
            }
        }

        /// <summary>
        /// 见证OssCode
        /// </summary>
        public static string OssCdnCodeJz
        {
            get
            {
                return GetConfigItem("OssCdnCodeJz");
            }
        }

        /// <summary>
        /// 报告OssCode   
        /// </summary>
        public static string OssCdnCodeBg
        {
            get
            {
                return GetConfigItem("OssCdnCodeBg");
            }
        }

        /// <summary>
        /// 委托单OssCode
        /// </summary>
        public static string OssCdnCodeWtd
        {
            get
            {
                return GetConfigItem("OssCdnCodeWtd");
            }
        }

        /// <summary>
        /// 静态CdnUrl
        /// </summary>
        public static string StaticCdnUrl
        {
            get
            {
                return GetConfigItem("StaticCdnUrl");
            }
        }

        /// <summary>
        /// 调用现场检测Key值
        /// </summary>
        public static string XcjcKey
        {
            get
            {
                return GetConfigItem("XcjcKey");
            }
        }

        #region 用户系统
        /// <summary>
        /// 用户系统URL
        /// </summary>
        public static string UmsUrl
        {
            get
            {
                return GetConfigItem("umsurl");
            }
        }
        #endregion

        #region 采集系统
        /// <summary>
        /// 采集系统密钥
        /// </summary>
        public static string CjxtSecret
        {
            get
            {
                return GetConfigItem("CjxtSecret");
            }
        }

        #endregion

        public static string OssCdnCodeXq
        {
            get
            {
                return GetConfigItem("OssCdnCodeXq");
            }
        }
    }
}
