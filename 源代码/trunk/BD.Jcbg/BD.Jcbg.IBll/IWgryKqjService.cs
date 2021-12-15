using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IWgryKqjService
    {
        /// <summary>
        /// 获取kqluserlog中HasDeal为False的数据
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetKqjUserLog();

        IList<IDictionary<string, string>> GetWxKqjUserLog();

        /// <summary>
        /// 保存一条考勤信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        bool SaveUserLog(string serial, string userid, DateTime time);

        /// <summary>
        /// 扫描微信二维码考勤
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <param name="kqjlx"></param>
        /// <returns></returns>
        bool SaveUserLog_WX(string serial, string userid, DateTime time, string kqjlx);

        bool SaveUserLogByWx(string userid, DateTime time, string qybh, string jdzch, string kqtype);

        IList<IDictionary<string, string>> GetCurrrentInWgryStatistic(string gcid,string gcbh_yc);

        /// <summary>
        /// 获取工程列表
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGClist();


        IList<IDictionary<string, string>> GetYHGClist();
        /// <summary>
        /// 判断工资册是否有未填写的
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool SetGzcYJ(string gcbh, string gcmc,DateTime dt);
        bool SetGzcYJErrInfo(string gcbh, string gcmc, DateTime dt);
        /// <summary>
        /// 判断银行账户
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="gcmc"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool SetYHYEYJ(string gcbh, string gcmc, DateTime dt);


        /// <summary>
        ///设置人员自动退出工地时间 
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <param name="gcbh"></param>
        void UpdateSgryOutSchedule(string checkTime);

        /// <summary>
        /// 根据身份证号获取考勤机
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetRyGcKqj(string sfzhm);

        IList<IDictionary<string, string>> GetGcKqj(string gcbh);

        IList<IDictionary<string, string>> GetYCGcKqj(string gcbh);

        void UpdateSgryOutDay(string gcbh);

        /// <summary>
        /// 获取未推送的工资册
        /// </summary>
        IList<IDictionary<string, string>> GetWgryPaylist();

        bool SetPayroll(string rguid);

        IList<IDictionary<string, string>> GetWgryYHBGlist();
        /// <summary>
        /// 变更务工人员银行卡
        /// </summary>
        /// <param name="row"></param>
        void SetPersonCard(IDictionary<string, string> row);
    }
}
