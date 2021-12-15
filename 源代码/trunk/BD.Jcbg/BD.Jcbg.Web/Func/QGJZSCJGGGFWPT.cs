using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using System.Text.RegularExpressions;

namespace BD.Jcbg.Web.Func
{
    /// <summary>
    /// 全国建筑市场监管公共服务平台
    /// （原全国建筑市场监管与诚信信息发布平台）
    /// 数据（页面）接口
    /// </summary>
    public class QGJZSCJGGGFWPT
    {
        #region URL
        // 平台域名
        private static string pturl = "http://jzsc.mohurd.gov.cn";

        // 查询企业列表URL(返回为html页面)
        private static string queryCompUrl = "/dataservice/query/comp/list";

        // 查询企业详情URL
        // 返回企业详情，包含资质证书和注册人员等信息
        private static string queryCompDetailUrl= "/dataservice/query/comp/compDetail/{qyid}";

        // 查询企业所有证书列表
        private static string queryCompCaDetailList = "/dataservice/query/comp/caDetailList/{qyid}";

        // 查询企业注册人员列表
        private static string queryCompRegStaffList = "/dataservice/query/comp/regStaffList/{qyid}";

        // 查询企业某个证书的详情
        private static string queryCompCaCertDetail = "/dataservice/query/comp/caCertDetail/{qyid}?certno={certno}";


        #endregion

        #region 具体接口

        /// <summary>
        /// 根据企业统一信用代码，获取该企业在平台的ID
        /// </summary>
        /// <param name="zzjgdm"></param>
        /// <param name="msg"></param>
        /// <param name="qyid"></param>
        /// <returns></returns>
        public static bool  GetQyid(string zzjgdm, out string msg, out string qyid)
        {
            bool ret = true;
            msg = "";
            qyid = "";
            try
            {
                string result = "";
                string postdata = "complexname=" + zzjgdm.UrlEncode();
                ret = MyHttp.Post(pturl+queryCompUrl, postdata, out result);
                // 调用成功,分析返回的html内容，获取平台中的企业ID
                if (ret )
                {
                    // 能够查到企业信息
                    if(result.IndexOf("data-header=\"统一社会信用代码\"") > -1)
                    {
                        Regex reg = new Regex("href=\"/dataservice/query/comp/compDetail/(\\d+)\"", RegexOptions.IgnoreCase);
                        MatchCollection matchCol = reg.Matches(result);
                        if (matchCol.Count > 0)
                        {
                            Match matchItem = matchCol[0];
                            if (matchItem.Groups.Count >=2)
                            {
                                qyid = matchItem.Groups[1].Value;
                            }
                        }

                    }
                }
                else
                {
                    msg = result;
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;

            }
            return ret;
        }


        public static bool GetQueryCompDetailUrl(string zzjgdm, out string msg, out string url)
        {
            bool ret = true;
            msg = "";
            url = "";
            try
            {
                string qyid = "";
                ret = GetQyid(zzjgdm, out msg, out qyid);
                if (ret )
                {
                    if (qyid!="")
                    {
                        url = GetCompDetailUrl(qyid);
                    }
                    else
                    {
                        ret = false;
                        msg = "无法获取企业ID";
                    }
                }
            }
            catch (Exception e )
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }

        public static string GetCompDetailUrl(string qyid)
        {
            return pturl + queryCompDetailUrl.Replace("{qyid}", qyid);
        }

        public static string GetCompCaDetailList(string qyid)
        {
            return pturl + queryCompCaDetailList.Replace("{qyid}", qyid);
        }


        public static string GetCompRegStaffList(string qyid)
        {
            return pturl + queryCompRegStaffList.Replace("{qyid}", qyid);
        }


        public static string GetCompCaCertDetail(string qyid, string certno)
        {
            return pturl + queryCompCaCertDetail.Replace("{qyid}", qyid).Replace("{certno}", certno);
        }

        #endregion

    }
}