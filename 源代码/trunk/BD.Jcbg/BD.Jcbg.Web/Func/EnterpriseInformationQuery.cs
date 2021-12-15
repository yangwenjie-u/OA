using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace BD.Jcbg.Web.Func.SCXPT
{

    public class EnterpriseInformationQuery
    {
        public result Query(string CompanyName)
        {
            result ret = new result();
            try
            {
                CookieContainer cookie = new CookieContainer();
                Uri d = new Uri("http://115.29.2.37:8080/enterprise_ajax.php");
                var p = $"page=&CorpName={CompanyName}&APTITUDEKINDNAME=&CertID=&City=&EndDate=";
                var res = Post(d, p, cookie);
                HtmlAgilityPack.HtmlDocument html = new HtmlDocument();
                html.LoadHtml(res);
                var auto_hs = html.DocumentNode.Descendants("tr").Where(x => x.Attributes["class"]?.Value == "auto_h").ToList();
                if (auto_hs.Count == 1)
                {
                    var url = auto_hs.First().Descendants("a").FirstOrDefault(x => x.Attributes["href"] != null)?.Attributes["href"].Value;
                    if (String.IsNullOrEmpty(url))
                    {
                        ret.Success = false;
                        ret.Message = "获取URL失败";
                    }
                    else
                    {
                        d = new Uri(d, url);
                        res = Get(d, cookie);
                        html = new HtmlDocument();
                        html.LoadHtml(res);
                        var detail = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "detail");
                        if (detail != null)
                        {
                            CL_EnterpriseInformationQuery_JBXX JBXX = new CL_EnterpriseInformationQuery_JBXX();
                            JBXX.QQMC = detail.SelectSingleNode("div[2]/table/tr[1]/td[2]").InnerText.Trim();
                            JBXX.YYZZH = detail.SelectSingleNode("div[2]/table/tr[2]/td[2]").InnerText.Trim();
                            JBXX.ZCZB = detail.SelectSingleNode("div[2]/table/tr[2]/td[4]").InnerText.Trim();
                            JBXX.TYSHXYDMZZJGDM = detail.SelectSingleNode("div[2]/table/tr[2]/td[6]").InnerText.Trim();
                            JBXX.FRDB = detail.SelectSingleNode("div[2]/table/tr[3]/td[2]").InnerText.Trim();
                            JBXX.FRDBZC = detail.SelectSingleNode("div[2]/table/tr[3]/td[4]").InnerText.Trim();
                            JBXX.FRDBZW = detail.SelectSingleNode("div[2]/table/tr[3]/td[6]").InnerText.Trim();
                            JBXX.QYJL = detail.SelectSingleNode("div[2]/table/tr[4]/td[2]").InnerText.Trim();
                            JBXX.QYJLZC = detail.SelectSingleNode("div[2]/table/tr[4]/td[4]").InnerText.Trim();
                            JBXX.QYJLZW = detail.SelectSingleNode("div[2]/table/tr[4]/td[6]").InnerText.Trim();
                            JBXX.QYFZR = detail.SelectSingleNode("div[2]/table/tr[5]/td[2]").InnerText.Trim();
                            JBXX.QYFZRZC = detail.SelectSingleNode("div[2]/table/tr[5]/td[4]").InnerText.Trim();
                            JBXX.QYFZRZW = detail.SelectSingleNode("div[2]/table/tr[5]/td[6]").InnerText.Trim();
                            JBXX.QYDJZCLX = detail.SelectSingleNode("div[2]/table/tr[6]/td[2]").InnerText.Trim();
                            JBXX.CLSJ = detail.SelectSingleNode("div[2]/table/tr[6]/td[4]").InnerText.Trim();
                            JBXX.YZBM = detail.SelectSingleNode("div[2]/table/tr[6]/td[6]").InnerText.Trim();
                            JBXX.LXR = detail.SelectSingleNode("div[2]/table/tr[7]/td[2]").InnerText.Trim();
                            JBXX.SSSS = detail.SelectSingleNode("div[2]/table/tr[7]/td[4]").InnerText.Trim();
                            JBXX.SSCS = detail.SelectSingleNode("div[2]/table/tr[7]/td[6]").InnerText.Trim();
                            JBXX.LXDZ = detail.SelectSingleNode("div[2]/table/tr[8]/td[2]").InnerText.Trim();
                            ret.data.JBXX = JBXX;
                        }

                        var t1 = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "classContent t1");
                        if (t1 != null)
                        {
                            var zizhis = t1.Descendants("div").Where(x => x.Attributes["class"]?.Value == "zizhi").ToList();
                            foreach (var zizhi in zizhis)
                            {
                                CL_EnterpriseInformationQuery_QYZZ QYZZ = new CL_EnterpriseInformationQuery_QYZZ();
                                QYZZ.ZZMC = zizhi.SelectSingleNode("div[1]").InnerText.Trim();
                                QYZZ.ZZZSBH = zizhi.SelectSingleNode("div[3]/table/tr[1]/td[2]").InnerText.Trim();
                                QYZZ.FZJG = zizhi.SelectSingleNode("div[3]/table/tr[1]/td[4]").InnerText.Trim();
                                QYZZ.QYFZR = zizhi.SelectSingleNode("div[3]/table/tr[1]/td[6]").InnerText.Trim();

                                QYZZ.FZRQ = zizhi.SelectSingleNode("div[3]/table/tr[2]/td[2]").InnerText.Trim();
                                QYZZ.YXQZ = zizhi.SelectSingleNode("div[3]/table/tr[2]/td[4]").InnerText.Trim();
                                QYZZ.JSFZR = zizhi.SelectSingleNode("div[3]/table/tr[2]/td[6]").InnerText.Trim();

                                var trs = zizhi.SelectNodes("div[3]/table/tr[3]/td[2]/table/tr");
                                for (int i = 1; i < trs?.Count; i++)
                                {
                                    var tr = trs.ElementAt(i);
                                    CL_EnterpriseInformationQuery_ZZFW ZZFW = new CL_EnterpriseInformationQuery_ZZFW();
                                    ZZFW.ZZFWMC = tr.SelectSingleNode("td[1]").InnerText.Trim();
                                    ZZFW.JSFZR = tr.SelectSingleNode("td[2]").InnerText.Trim();
                                    QYZZ.ZZFW.Add(ZZFW);
                                }
                                ret.data.QYZZ.Add(QYZZ);
                            }
                        }

                        var t2 = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "classContent t2");
                        if (t2 != null)
                        {
                            var trs = t2.SelectNodes("table/tr");
                            for (int i = 1; i < trs?.Count; i++)
                            {
                                var tr = trs.ElementAt(i);
                                CL_EnterpriseInformationQuery_ZhuCeRY RY = new CL_EnterpriseInformationQuery_ZhuCeRY();
                                RY.RYXM = tr.SelectSingleNode("td[2]").InnerText.Trim();
                                RY.ZJBH = tr.SelectSingleNode("td[3]").InnerText.Trim();
                                RY.ZCLXJDJ = tr.SelectSingleNode("td[4]").InnerText.Trim();
                                RY.ZCZSBH = tr.SelectSingleNode("td[5]").InnerText.Trim();
                                RY.ZY = tr.SelectSingleNode("td[6]").InnerText.Trim();
                                RY.FZRQ = tr.SelectSingleNode("td[7]").InnerText.Trim();
                                RY.YXQZ = tr.SelectSingleNode("td[8]").InnerText.Trim();
                                ret.data.ZhuCeRY.Add(RY);
                            }
                        }

                        var t3 = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "classContent t3");
                        if (t3 != null)
                        {
                            var trs = t3.SelectNodes("table/tr");
                            for (int i = 1; i < trs?.Count; i++)
                            {
                                var tr = trs.ElementAt(i);
                                CL_EnterpriseInformationQuery_ZhiChengRY RY = new CL_EnterpriseInformationQuery_ZhiChengRY();
                                RY.RYXM = tr.SelectSingleNode("td[2]").InnerText.Trim();
                                RY.ZJBH = tr.SelectSingleNode("td[3]").InnerText.Trim();
                                RY.JSZC = tr.SelectSingleNode("td[4]").InnerText.Trim();
                                RY.ZY = tr.SelectSingleNode("td[5]").InnerText.Trim();
                                RY.XL = tr.SelectSingleNode("td[6]").InnerText.Trim();
                                ret.data.ZhiChengCRY.Add(RY);
                            }
                        }

                        var t4 = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "classContent t4");
                        if (t4 != null)
                        {
                            var trs = t4.SelectNodes("table/tr");
                            for (int i = 1; i < trs?.Count; i++)
                            {
                                var tr = trs.ElementAt(i);
                                CL_EnterpriseInformationQuery_XCRY RY = new CL_EnterpriseInformationQuery_XCRY();
                                RY.RYXM = tr.SelectSingleNode("td[2]").InnerText.Trim();
                                RY.ZJBH = tr.SelectSingleNode("td[3]").InnerText.Trim();
                                RY.RYLX = tr.SelectSingleNode("td[4]").InnerText.Trim();
                                RY.ZCZSBH = tr.SelectSingleNode("td[5]").InnerText.Trim();
                                RY.FZRQ = tr.SelectSingleNode("td[6]").InnerText.Trim();
                                RY.YXQZ = tr.SelectSingleNode("td[7]").InnerText.Trim();
                                ret.data.XCRY.Add(RY);
                            }
                        }

                        var t5 = html.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes["class"]?.Value == "classContent t5");
                        if (t5 != null)
                        {
                            var trs = t5.SelectNodes("table/tr");
                            for (int i = 1; i < trs?.Count; i++)
                            {
                                var tr = trs.ElementAt(i);
                                CL_EnterpriseInformationQuery_JSRY RY = new CL_EnterpriseInformationQuery_JSRY();
                                RY.RYXM = tr.SelectSingleNode("td[2]").InnerText.Trim();
                                RY.ZJBH = tr.SelectSingleNode("td[3]").InnerText.Trim();
                                RY.RYLX = tr.SelectSingleNode("td[4]").InnerText.Trim();
                                RY.ZCZSBH = tr.SelectSingleNode("td[5]").InnerText.Trim();
                                RY.FZRQ = tr.SelectSingleNode("td[6]").InnerText.Trim();
                                RY.YXQZ = tr.SelectSingleNode("td[7]").InnerText.Trim();
                                ret.data.JSRY.Add(RY);
                            }
                        }
                        ret.Success = true;
                        ret.Message = "成功";
                    }
                }
                else
                {
                    ret.Success = false;
                    ret.Message = auto_hs.Count == 0 ? "无查询结果" : $"查询有多条记录，当前页记录数：{auto_hs.Count}";
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
                ret.Message = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// POST提交
        /// </summary>
        /// <param name="d"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private string Post(Uri d, string postData, CookieContainer cookie)
        {
            try
            {
                HttpWebRequest req1 = (HttpWebRequest)HttpWebRequest.Create(d);
                byte[] bs = null;
                bs = Encoding.UTF8.GetBytes(postData);
                req1.Accept = "text/html, */*; q=0.01";
                req1.ContentLength = bs.Length;
                req1.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                req1.Headers.Add("Cache-control", "no-cache");
                req1.KeepAlive = true;
                req1.CookieContainer = cookie;
                req1.ServicePoint.Expect100Continue = false;
                req1.Method = "POST";
                req1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.25 Safari/537.36 Core/1.70.3704.400 QQBrowser/10.4.3587.400";
                req1.Headers.Add("X-Requested-With: XMLHttpRequest");
                using (Stream rs = req1.GetRequestStream())
                {
                    rs.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req1.GetResponse())
                {
                    var sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// GET提交
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private string Get(Uri d, CookieContainer cookie)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(d);
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.KeepAlive = true;
                req.Headers.Add("Cache-control", "no-cache");
                req.Method = "GET";
                req.CookieContainer = cookie;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.25 Safari/537.36 Core/1.70.3704.400 QQBrowser/10.4.3587.400";
                using (WebResponse wr = req.GetResponse())
                {
                    var sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
    /// <summary>
    /// 返回类
    /// </summary>
    public class result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public CL_EnterpriseInformationQuery data { get; set; }

        public result()
        {
            data = new CL_EnterpriseInformationQuery();
        }
    }
    /// <summary>
    /// 数据
    /// </summary>
    public class CL_EnterpriseInformationQuery
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public CL_EnterpriseInformationQuery_JBXX JBXX { get; set; }

        /// <summary>
        /// 企业资质
        /// </summary>
        public List<CL_EnterpriseInformationQuery_QYZZ> QYZZ { get; set; } = new List<CL_EnterpriseInformationQuery_QYZZ>();

        /// <summary>
        /// 注册人员
        /// </summary>
        public List<CL_EnterpriseInformationQuery_ZhuCeRY> ZhuCeRY { get; set; } = new List<CL_EnterpriseInformationQuery_ZhuCeRY>();

        /// <summary>
        /// 职称人员
        /// </summary>
        public List<CL_EnterpriseInformationQuery_ZhiChengRY> ZhiChengCRY { get; set; } = new List<CL_EnterpriseInformationQuery_ZhiChengRY>();

        /// <summary>
        /// 现场人员
        /// </summary>
        public List<CL_EnterpriseInformationQuery_XCRY> XCRY { get; set; } = new List<CL_EnterpriseInformationQuery_XCRY>();

        /// <summary>
        /// 技术人员
        /// </summary>
        public List<CL_EnterpriseInformationQuery_JSRY> JSRY { get; set; } = new List<CL_EnterpriseInformationQuery_JSRY>();

    }
    /// <summary>
    /// 基本信息
    /// </summary>
    public class CL_EnterpriseInformationQuery_JBXX
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string QQMC { get; set; }
        /// <summary>
        /// 营业执照号
        /// </summary>
        public string YYZZH { get; set; }
        /// <summary>
        /// 注册资本
        /// </summary>
        public string ZCZB { get; set; }
        /// <summary>
        /// 统一社会信用代码/组织机构代码
        /// </summary>
        public string TYSHXYDMZZJGDM { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        public string FRDB { get; set; }
        /// <summary>
        /// 法人代表职称
        /// </summary>
        public string FRDBZC { get; set; }
        /// <summary>
        /// 法人代表职务
        /// </summary>
        public string FRDBZW { get; set; }
        /// <summary>
        /// 企业经理
        /// </summary>
        public string QYJL { get; set; }
        /// <summary>
        /// 企业经理职称
        /// </summary>
        public string QYJLZC { get; set; }
        /// <summary>
        /// 企业经理职务
        /// </summary>
        public string QYJLZW { get; set; }
        /// <summary>
        /// 企业负责人
        /// </summary>
        public string QYFZR { get; set; }
        /// <summary>
        /// 企业负责人职称
        /// </summary>
        public string QYFZRZC { get; set; }
        /// <summary>
        /// 企业负责人职务
        /// </summary>
        public string QYFZRZW { get; set; }
        /// <summary>
        /// 企业注册登记类型
        /// </summary>
        public string QYDJZCLX { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>
        public string CLSJ { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string YZBM { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LXR { get; set; }
        /// <summary>
        /// 所属省市
        /// </summary>
        public string SSSS { get; set; }
        /// <summary>
        /// 所属城市
        /// </summary>
        public string SSCS { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string LXDZ { get; set; }

    }

    /// <summary>
    /// 企业资质
    /// </summary>
    public class CL_EnterpriseInformationQuery_QYZZ
    {
        /// <summary>
        /// 资质名称
        /// </summary>
        public string ZZMC { get; set; }
        /// <summary>
        /// 资质证书编号
        /// </summary>
        public string ZZZSBH { get; set; }
        /// <summary>
        /// 发证机关共
        /// </summary>
        public string FZJG { get; set; }
        /// <summary>
        /// 企业负责人
        /// </summary>
        public string QYFZR { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public string FZRQ { get; set; }
        /// <summary>
        /// 有效期至
        /// </summary>
        public string YXQZ { get; set; }
        /// <summary>
        /// 技术负责人
        /// </summary>
        public string JSFZR { get; set; }
        /// <summary>
        /// 资质范围
        /// </summary>
        public List<CL_EnterpriseInformationQuery_ZZFW> ZZFW { get; set; } = new List<CL_EnterpriseInformationQuery_ZZFW>();
    }

    /// <summary>
    /// 资质范围
    /// </summary>
    public class CL_EnterpriseInformationQuery_ZZFW
    {
        /// <summary>
        /// 资质范围名称
        /// </summary>
        public string ZZFWMC { get; set; }
        /// <summary>
        /// 技术负责人
        /// </summary>
        public string JSFZR { get; set; }
    }
    /// <summary>
    /// 注册人员
    /// </summary>
    public class CL_EnterpriseInformationQuery_ZhuCeRY
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string RYXM { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string ZJBH { get; set; }
        /// <summary>
        /// 注册类型及等级
        /// </summary>
        public string ZCLXJDJ { get; set; }
        /// <summary>
        /// 注册证书编号
        /// </summary>
        public string ZCZSBH { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string ZY { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public string FZRQ { get; set; }
        /// <summary>
        /// 有效期至
        /// </summary>
        public string YXQZ { get; set; }
    }

    /// <summary>
    /// 职称人员
    /// </summary>
    public class CL_EnterpriseInformationQuery_ZhiChengRY
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string RYXM { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string ZJBH { get; set; }
        /// <summary>
        /// 技术职称
        /// </summary>
        public string JSZC { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string ZY { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string XL { get; set; }
    }

    /// <summary>
    /// 现场人员
    /// </summary>
    public class CL_EnterpriseInformationQuery_XCRY
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string RYXM { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string ZJBH { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        public string RYLX { get; set; }
        /// <summary>
        /// 注册证书编号
        /// </summary>
        public string ZCZSBH { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public string FZRQ { get; set; }
        /// <summary>
        /// 有效期至
        /// </summary>
        public string YXQZ { get; set; }
    }

    /// <summary>
    /// 技术人员
    /// </summary>
    public class CL_EnterpriseInformationQuery_JSRY
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string RYXM { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string ZJBH { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        public string RYLX { get; set; }
        /// <summary>
        /// 注册证书编号
        /// </summary>
        public string ZCZSBH { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public string FZRQ { get; set; }
        /// <summary>
        /// 有效期至
        /// </summary>
        public string YXQZ { get; set; }
    }

}
