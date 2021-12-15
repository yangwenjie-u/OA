using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace BD.Jcbg.IBll
{
    public interface ISelfService
	{
        bool SaveDataFile(HttpPostedFileBase file, HttpServerUtilityBase server, string fileid, out string msg);
        /// <summary>
        /// 获取人员手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetPhone(string usercode, out string msg);

        string getCity(string gcid);

        IList<IList<IDictionary<string, string>>> GetSelfFormData(IList<IDictionary<string, string>> rows);


        IList<IDictionary<string, string>> getBzfzrlist(string bzfzr,string jdzch);
        void getBzfzrlist(string bzfzr, string jdzch, ref List<string> fzrlist);

        string getbzfzr_str(string bzfzrxm, string jdzch);

        List<string> getFormZDZD(string formdm, string formStatus);

        /// <summary>
        /// 获取班组长sql
        /// </summary>
        /// <param name="filterRules"></param>
        /// <param name="zdlist"></param>
        /// <param name="jdzch"></param>
        /// <returns></returns>
        string getSqlfzr(string filterRules, List<string> zdlist, string jdzch, bool ym,string bzfzrzd = "bzfzr");

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        string PostData(string url, string datas);
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetHttpResponse(string url);

        /// <summary>
        /// 获取区域统计
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="jd"></param>
        /// <param name="qybh"></param>
        /// <param name="gcbh"></param>
        /// <param name="gclx"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGC_QYFBTJ(string province, string city, string district, string jd, string qybh, string gcbh, string gclx, string key);

        IList<IDictionary<string, string>> GetQYGCZT(string province, string city, string district, string jd, string qybh, string gcbh, string gclx, string key,string gczt);


    }
}
