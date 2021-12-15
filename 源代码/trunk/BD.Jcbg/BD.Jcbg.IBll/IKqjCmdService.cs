using System.Collections.Generic;
using System.Data;
using System;

namespace BD.Jcbg.IBll
{
    /// <summary>
    /// 考勤机命令服务
    /// </summary>
    public interface IKqjCmdService
    {
        /// <summary>
        /// 下发人员模板
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DownRyIris(string rybh, out string msg);

        bool DownWGRyIris(string rybh, out string msg);
        /// <summary>
        /// 下发考勤机所有模板
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DownKqjIris(string kqjbh, out string msg);

		bool DownWgryKqjIris(string kqjbh, out string msg);
        /// <summary>
        /// 初始化考勤机
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool InitKqj(string kqjbh, out string msg);
        /// <summary>
        /// 重启考勤机
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool RestartKqj(string kqjbh, out string msg);
        /// <summary>
        /// 企业考勤统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        List<IDictionary<string, string>> GetQyKq(string startTime, string endTime);

        /// <summary>
        /// 工程信息下拉框
        /// </summary>
        List<IDictionary<string, string>> GetGcName();

        /// <summary>
        /// 工程企业考勤统计
        /// </summary>
        /// <param name="gcbh">工程编号</param>
        /// <param name="kqTime">考勤时间</param>
        List<IDictionary<string, string>> GetGcKq(string gcbh, string kqTime);

        /// <summary>
        /// 获取工程信息,工程企业考勤信息,工程经纬度信息
        /// </summary>
        DataSet GetGcInfos(string gczt, string qymc);

        /// <summary>
        /// 涉外企业信息
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/28 14:07
        DataSet GetSwqyInfos(string wdqy);

        /// <summary>
        /// 获取考勤机信息,经纬度,断线间隔
        /// </summary>
        DataSet GetKqjInfos(string wdqy);

        /// <summary>
        /// 获取人员信息
        /// </summary>
        IList<IDictionary<string, string>> GetRyInfos(string qylx, string qybh, string ryxm, string wdqy);

        /// <summary>
        /// 单位类型
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:26
        IList<IDictionary<string, string>> GetDwlx();

        /// <summary>
        /// 根据类型查找企业名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:58
        IList<IDictionary<string, string>> GetDwmc(string lxbh, string wdqy);

        /// <summary>
        /// 根据是否为外地企业,查询企业下拉框
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/7/25 13:18
        IList<IDictionary<string, string>> GetGcQyList(string wdqy);

        /// <summary>
        /// 单位人员月考勤信息
        /// </summary>
        DataTable GetDwKqInfos(string qymc, string rq);

        /// <summary>
        /// 单位人员月考勤统计报表
        /// </summary>
        DataTable GetDwKqTotal(string qymc, string rq);
		// <summary>
        /// 获取kqluserlog中HasDeal为False的数据
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetKqjUserLog();

        /// <summary>
        /// 保存一条考勤信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        bool SaveUserLog(string serial, string userid, DateTime time);

        bool SaveUserLogWithOUTkqj(string userid, DateTime time, string qybh, string jdzch);

        /// <summary>
        /// 获取班组负责人
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetBzfzrs();

        /// <summary>
        /// 获取工种
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGzs();


        IList<IDictionary<string, string>> GetGzGws(string gz);

         /// <summary>
        /// 获取所有人某月的工资册
        /// </summary>
        /// <param name="sfz"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetUserMonthPay(string jdzch, string xm, string sfz, string dt1, string dt2, string gz, string gw,string bzfzr, int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 更新月工资册
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="realpay"></param>
        /// <param name="advance"></param>
        /// <param name="paid"></param>
        /// <returns></returns>
        bool UpdateUserMonthPay(string jdzch, string userid, string realname, string year, string month, string gzgz, string shouldpay, string realpay,string yzpay);


    }
}
