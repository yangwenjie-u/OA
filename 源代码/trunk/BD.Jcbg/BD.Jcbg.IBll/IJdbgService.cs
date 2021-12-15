using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Common;

namespace BD.Jcbg.IBll
{
    /// <summary>
    /// 监督办公接口服务
    /// </summary>
    public interface IJdbgService
    {
        /// <summary>
        /// 获取某个工程所有种类报告数量
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        VJdbgReportSumItem GetReportSum(string gcbh, out string msg);
        /// <summary>
        /// 获取某个工程某个类型报告数量
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        int GetReportSum(string gcbh, string item, out string msg);
        /// <summary>
        /// 手机上传内容查找
        /// </summary>
        /// <param name="key"></param>
        /// <param name="gcbh"></param>
        /// <param name="username"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="ispub"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetProblems(string key, string gcbh, string username, string dt1, string dt2, string ispub,
            int pageSize, int pageIndex, out int totalCount);
        /// <summary>
        /// 获取问题图片小图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte[] GetProblemImageSmall(string id);
        /// <summary>
        /// 获取问题图片大图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        byte[] GetProblemImageBig(string id);
        /// <summary>
        /// 获取问题语音
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte[] GetProblemVoice(string id);

        IList<IDictionary<string, string>> GetProblemImages(string gcbh, string username, string status);

        IList<IDictionary<string, string>> GetProblemContents(string gcbh, string username, string status);

        IList<IDictionary<string, string>> GetProblemContentsByWorkserial(string workserial, string username, string status);

        IList<IDictionary<string, string>> GetProblemImagesByWorkserial(string workserial, string username, string status);

        bool SaveDJCFQ(string procstr, out string err, out string FKID);
        /// <summary>
        /// 获取工程和分工程的开始结束层数
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="fgcbhs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGccs(string gcbh, string fgcbhs, out string msg);

         /// <summary>
        ///  获取工程的验收状态
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGcyszts();
        /// <summary>
        /// 更新工程及分工程的开始结束层数
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        bool SetGccs(IList<IDictionary<string, string>> infos, out string msg);
        /// <summary>
        /// 返回人员类型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserType GetUserType(string username);
        /// <summary>
        /// 获取工程类型
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGclx();
        /// <summary>
        /// 工程统计，返回工程数量、总面积、总造价
        /// </summary>
        /// <param name="kgnf"></param>
        /// <param name="jgnf"></param>
        /// <param name="gczt"></param>
        /// <param name="gclx"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGcStatistic(string kgnf, string jgnf, string gczt, string gclx);
        /// <summary>
        /// 首页工程地图标注
        /// </summary>
        /// <param name="kgnf"></param>
        /// <param name="jgnf"></param>
        /// <param name="gczt"></param>
        /// <param name="gclx"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetGcList(string kgnf, string jgnf, string gczt, string gclx);
    }
}
