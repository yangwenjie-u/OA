using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Data;

namespace BD.Jcbg.IBll
{
    /// <summary>
    /// 检测服务接口
    /// </summary>
    public interface IJcService
    {
        #region 服务

        /// <summary>
        /// 获取委托单信息
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetWtd(string recid, out IDictionary<string, string> mtable, out IList<IDictionary<string, string>> stable,
            out string msg);

        bool GetWtds(string depcode, VTransDownGetWtd[] where, out IList<IDictionary<string, object>> records,
            out string msg);

        /// <summary>
        /// 设置资质有效期
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetZzyxq(int recid, string datetype, DateTime datevalue, out string msg);

        /// <summary>
        /// 检测单位项目禁用启用
        /// </summary>
        /// <param name="dicIds"></param>
        /// <returns></returns>
        bool EnableJcdwXm(IDictionary<int, int> dicIds, out string msg);

        /// <summary>
        /// 删除委托单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteWtds(string ids, string userCode, out string msg);

        /// <summary>
        /// 根据报告唯一号获取委托单编号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/6/6 13:19
        string GetWtdbh(string bgwyh);

        /// <summary>
        /// 获取最后一份已有二维码的报告文件
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, byte[]> GetReportFiles(string wtdwyh, out string msg);

        /// <summary>
        /// 查询所有委托单，包括收样和未收样的
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="zt"></param>
        /// <param name="lrrzh"></param>
        /// <param name="gcbh"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetWtds(string syxmbh, string zt, string lrrzh, string gcbh, int pageSize, int pageIndex,
            out int totalCount, out IList<IDictionary<string, string>> records, out string msg);

        /// <summary>
        /// 获取人员相关工程
        /// </summary>
        /// <param name="ryzh"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetGcs(string ryzh, out IList<IDictionary<string, string>> records, out string msg);

        /// <summary>
        /// 根据用户代码获取企业编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        string GetQybh(string usercode);

        /// <summary>
        /// 获取企业名称
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        string GetQymc(string qybh);

        /// <summary>
        /// 根据用户代码获取登录账号编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        string GetUserbh(string usercode);

        /// <summary>
        /// 根据企业编号获取企业类型编号
        /// </summary>
        /// <param name="qyzhbh"></param>
        /// <returns></returns>
        string GetLxbh(string qyzhbh);

        /// <summary>
        /// 获取现场检测委托单的摄像头编号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        string GetWtdXcjcSxt(string wtdwyh);

        /// <summary>
        /// 获取委托单对应项目打印的委托单份数
        /// </summary>
        /// <param name="wtdwyhs">委托单唯一号列表</param>
        /// <param name="msg">错误信息</param>
        /// <returns>返回字典列表：wtdwyh,wtddyfs</returns>
        IList<IDictionary<string, string>> GetSyxmWtddyfs(IList<string> wtdwyhs, out string msg);

        /// <summary>
        /// 获取委托单打印内容
        /// </summary>
        /// <param name="wheres">syxmbh|recid</param>
        /// <returns>(recid,委托单数据（表名，表记录集）)</returns>
        IDictionary<string, IDictionary<string, IList<IDictionary<string, object>>>> GetWtdPrintInfos(string[] wheres,
            out string msg);

        #endregion

        #region 对外接口

        /// <summary>
        /// 上传报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgbh"></param>
        /// <param name="syr"></param>
        /// <param name="shr"></param>
        /// <param name="qfr"></param>
        /// <param name="syrq"></param>
        /// <param name="qfrq"></param>
        /// <param name="jcjg"></param>
        /// <param name="jcjgms"></param>
        /// <param name="datajson"></param>
        /// <param name="pdfjson"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool UpReport(string dwbh, string wtdwyh, string bgbh, string syr, string shr, string qfr,
            DateTime syrq, DateTime qfrq, int jcjg, string jcjgms, string mdatajson, string sdatajson, string pdfjson,
            bool setBarcode, bool sdsc, string datajson,
            out string msg);

        /// <summary>
        /// 上传试验数据
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="syxmmc"></param>
        /// <param name="ypbh"></param>
        /// <param name="zh"></param>
        /// <param name="syr"></param>
        /// <param name="sysb"></param>
        /// <param name="sykssj"></param>
        /// <param name="syjssj"></param>
        /// <param name="syqx"></param>
        /// <param name="datajson"></param>
        /// <param name="czdatajson"></param>
        /// <param name="sfbc"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool UpData(string dwbh, string wtdwyh, string syxmmc, string ypbh, string zh, string syr, string sysb,
            DateTime sykssj, DateTime syjssj, string syqx, string videofiles, string recordfiles, string datajson,
            string czdatajson, bool sfbc, out string msg);

        /// <summary>
        /// 上传变更单
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="bgyy"></param>
        /// <param name="bgsj"></param>
        /// <param name="where"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool UpBgd(string dwbh, string wtdwyh, string bgyy, DateTime bgsj, VTransUpBgd[] bgds, out string msg);

        /// <summary>
        /// 获取委托单的二维码字符串。检测中心设置中，允许提前获取二维码的才可以
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetBarcode(string dwbh, string wtdwyh, out string msg);

        /// <summary>
        /// 获取检测合同
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetJchts(string dwbh, VTransDownGetJcht where, int pagesize, int pageindex, out int totalcount,
            out IList<IDictionary<string, object>> records, out string msg);

        /// <summary>
        /// 设置结算人账户余额
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="jsrbh"></param>
        /// <param name="ye"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetJsrYe(string dwbh, string jsrbh, decimal ye, out string msg);

        /// <summary>
        /// 判断委托单是否已作废
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        bool IsWtdZf(string wtdwyh);

        #endregion

        #region 委托单状态设置

        /// <summary>
        /// 设置委托单提交状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusTj(string recid, out string msg);

        /// <summary>
        /// 设置委托单保存状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusBc(string recid, out string msg);

        /// <summary>
        /// 设置委托单打印状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusDy(string recid, out string msg);

        /// <summary>
        /// 设置委托单状态为下发到检测机构
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusXf(string recid, string qybh, out string msg, bool sdsy = false);

        /// <summary>
        /// 取消委托单下发状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CancelWtdStatusXf(string recid, string qybh, out string msg);

        /// <summary>
        /// 设置委托单为已试验状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusSy(string recid, out string msg);

        /// <summary>
        /// 设置委托单为报告已出状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdStatusBg(string recid, out string msg);

        /// <summary>
        /// 获取检测机构数据传输密钥
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetJcjgmy(string qybh, out string msg);

        /// <summary>
        /// 获取最后一份已有二维码的报告唯一号和顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetReportAbs(string wtdwyh, out string bgwyh, out string sxh, out string msg);

        bool GetReportAbsByBgwyh(string bgwyh, string sdsc, out string sxh, out string msg);

        /// <summary>
        /// 返回某个委托单所有已有二维码的报告唯一号和顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgabs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetReportAbs(string wtdwyh, out IList<IDictionary<string, string>> bgabs, out string msg);

        /// <summary>
        /// 根据报告唯一号和顺序号获取报告内容
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetReportFile(string bgwyh, string sxh, out byte[] file, out string msg);

        /// <summary>
        /// 设置委托单异常状态，以及数据状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdyczt(string wtdwyh, out string msg);

        /// <summary>
        /// 设置所有委托单的异常状态，及数据状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdycztAll(out string msg);

        /// <summary>
        /// 工程信息查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetGcs(string jcjgbh, string stationid, VTransDownGetGc where, int pagesize, int pageindex,
            out int totalcount, out IList<IDictionary<string, object>> records, out string msg);

        /// <summary>
        /// 获取某次试验的数据
        /// </summary>
        /// <param name="sywyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetSysjxq(string sywyh, out string msg);

        /// <summary>
        /// 获取试验曲线图片
        /// </summary>
        /// <param name="sywyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        byte[] GetSysjqx(string sywyh, out string msg);

        /// <summary>
        /// 获取委托单的所有试验记录
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetWtdSysjs(string wtdwyh, out string msg);

        /// <summary>
        /// 获取委托单所有试验记录详情
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetWtdSysjxqs(string wtdwyh, out string msg);

        /// <summary>
        /// 获取委托单的所有报告和报告顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetWtdReports(string wtdwyh, out string msg);

        /// <summary>
        /// 设置委托单锁定解锁，sfsd-1锁定，0解锁
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdSd(string dwbh, string recids, int sfsd, out string msg);

        /// <summary>
        /// 设置委托单作废
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="reason"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdZf(string dwbh, string recid, string reason, out string msg);

        /// <summary>
        /// 设置委托单未送样
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdWSY(string dwbh, string recid, out string msg);

        /// <summary>
        /// 设置委托单报告查看
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdBgck(string dwbh, string recid, out string msg);

        #endregion

        #region 非检测中心调用接口

        /// <summary>
        /// 获取加密key
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="calltype"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetStationEnctyptKey(string stationid, string calltype, out string msg);

        /// <summary>
        /// 获取下发的报告条目
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="lastid"></param>
        /// <param name="count"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetDownReports(string stationid, string lastid, int count, out string msg);

        /// <summary>
        /// 获取分页的报告数据
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="jcdwbh"></param>
        /// <param name="jcdwmc"></param>
        /// <param name="wtdbh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="syxmmc"></param>
        /// <param name="bgbh"></param>
        /// <param name="zjdjh"></param>
        /// <param name="gcbh"></param>
        /// <param name="gcmc"></param>
        /// <param name="khdwmc"></param>
        /// <param name="jcjg">0-不下结论，1-合格，2-不合格</param>
        /// <param name="qfrq1"></param>
        /// <param name="qfrq2"></param>
        /// <param name="scsj1"></param>
        /// <param name="scsj2"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetDownReports(string stationid, string jcdwbh, string jcdwmc,
            string wtdbh, string syxmbh, string syxmmc, string bgbh, string zjdjh, string gcbh, string gcmc,
            string khdwmc, string jcjg, string qfrq1, string qfrq2, string scsj1, string scsj2,
            int pagesize, int pageindex, string orderfield, out int totalcount, out string msg);

        bool SetReportDealOpinion(string stationid, string bgwyh, string opinion, out string msg);
        bool SetReportDealResult(string stationid, string bgwyh, string result, out string msg);

        #endregion

        #region 现场检测调用

        /// <summary>
        /// 人员手机sim卡号是否已绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        bool HasBindPhoneSim(string usercode);

        /// <summary>
        /// 绑定账号手机sim
        /// </summary>
        /// <param name="username">登录名，不是用户代码</param>
        /// <param name="sim"></param>
        /// <returns></returns>
        bool BindPhoneSim(string usercode, string sim, out string msg);

        /// <summary>
        /// sim卡是否已被别的人绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        bool HasSimUsed(string usercode, string sim);

        /// <summary>
        /// 获取分页现场项目
        /// </summary>
        /// <param name="htxmisxm"></param>
        /// <param name="dwbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSyxmList(string dwbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取分页现场试验编号
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSybhList(string dwbh, string syxmbh, string key, int pagesize,
            int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取分页现场试验设备列表
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="ptbh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSyDevList(string dwbh, string ptbh, int pagesize, int pageindex,
            out int totalcount, out string msg);

        /// <summary>
        /// 获取分页现场试验同检人员列表
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="usercode"></param>
        /// <param name="ptbh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSyrList(string dwbh, string usercode, string ptbh, int pagesize,
            int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取委托单详情
        /// </summary>
        /// <param name="ptbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> GetXcjcSyDetail(string ptbh, out string msg);

        /// <summary>
        /// 获取人员企业编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        string GetRyQybh(string username, out string msg);

        /// <summary>
        /// 获取分页现场试验部位
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSybwList(string syxmbh, string key, int pagesize, int pageindex,
            out int totalcount, out string msg);

        /// <summary>
        /// 获取分页现场摄像头
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcSxtList(string dwbh, string key, int pagesize, int pageindex,
            out int totalcount, out string msg);

        /// <summary>
        /// 现场检测开始试验
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sxts"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool XcjcStartExperment(string wtdwyh, int lsh, string zh, string zlx, string syrzh, string syrxm,
            string longitude, string latitude, IList<VTransXcjcReqStartItem> sxts, IList<byte[]> files, out string msg);

        /// <summary>
        /// 试验图片上传
        /// </summary>
        /// <param name="sxts"></param>
        /// <param name="imageType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool XcjcUpImage(string wtdwyh, string zh, IList<byte[]> files, int imageType, out string msg);

        /// <summary>
        /// 结束试验
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sxts"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool XcjcStopExperment(string wtdwyh, string zh, IList<byte[]> files, out string msg);

        /// <summary>
        /// 获取正在试验的编号
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetXcjcInSybhList(string dwbh, string syrzh, string syxmbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 保存人员签字
        /// </summary>
        /// <param name="username"></param>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool XcjcSetRySign(string username, string filename, byte[] image, out string msg);

        /// <summary>
        /// 获取某个委托单现场上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> XcjcGetImages(string url, string wtdwyh, string zh, out string msg);

        /// <summary>
        /// 获取现场检测图片详情
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        byte[] GetXcjcUpImage(string tpid);

        /// <summary>
        /// 获取正在进行的现场检测
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> XcjcGetOnExperments(string usercode);

        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="splx"></param>
        /// <param name="videos"></param>
        /// <returns></returns>
        bool XcjcUploadVideo(string wtdwyh, string splx, IList<VTransXcjcReqVideoInfoItem> videos, out string msg);

        /// <summary>
        /// 设置厂家试验编号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="cjsybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool XcjcSetCjsybh(string wtdwyh, string zhuanghao, string cjsybh, out string msg);

        /// <summary>
        /// 现场图片链
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="zh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        ResultParam XcjcTpl(string wtdwyh, string zh, int pagesize, int pageindex);

        /// <summary>
        /// 根据委托单获取采集试验编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>

        ResultParam XcjcGetCjsybhs(string wtdwyh);

        /// <summary>
        /// 根据委托单获取采集图片编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        ResultParam XcjcGetCjtp(string wtdwyh);

        /// <summary>
        /// 根据委托单获取视频编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        ResultParam XcjcGetCjsp(string wtdwyh);

        /// <summary>
        /// 根据委托单获取图片链
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        ResultParam XcjcGetCjtpl(string wtdwyh);

        #endregion

        #region 见证取样

        /// <summary>
        /// 获取分页所有实验项目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPageBaseSyxmList(string key,
            int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取见证人工程列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPageJzrGcList(string username, string key,
            int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取收样人工程列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPageSyrGcList(string qybh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取人员类型，非I_M_RY表中的人员，返回空；否则返回i_m_ry中的rylx加i_s_ry_ryzz中的人员类型（这00类型不返回），再加上i_m_ry中的zwbh
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool GetVirtualRylx(string username, out string msg);

        /// <summary>
        /// 获取见证人见证试验列表
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> JzqyGetJzrSybhList(string qybh, string gcbh, string syxmbh, string key,
            string zt, int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取需要见证的试验列表，包含图片详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> JzqyGetJzrSybhListWithDetail(string username, string gcbh, string syxmbh,
            string key, string zt,
            string tprq1, string tprq2, string url, out string msg);

        /// <summary>
        /// 获取收样人见证试验列表
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> JzqyGetSyrSybhList(string qybh, string gcbh, string syxmbh, string key,
            string zt, int pagesize, int pageindex, out int totalcount, out string msg);

        /// <summary>
        /// 获取需要见证的试验列表，包含图片详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> JzqyGetSyrSybhListWithDetail(string qybh, string gcbh, string syxmbh,
            string key, string zt,
            string tprq1, string tprq2, string url, out string msg);

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyUpImage(string wtdwyh, string qybh, string ryzh, string ryxm, IList<byte[]> files, string ewm,
            string lon, string lat, string zh, string imagetype, out string msg);

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyUpImage2(string wtdwyh, string qybh, string ryzh, string ryxm,
            IList<IDictionary<string, byte[]>> files, string ewm, string lon, string lat, string zh, out string msg);

        /// <summary>
        /// 设置单组完成状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqySetDzwcStatus(string wtdwyh, string zh, out string msg);

        /// <summary>
        /// 设置工程坐标
        /// </summary>
        /// <param name="gcbh">工程编号</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqySetGczb(string gcbh, string longitude, string latitude, out string msg);


        /// <summary>
        /// 获取某个委托单见证详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> JzqyGetWtdJzxq(string url, string wtdwyh, string viewWtdUrl, out string msg);

        /// <summary>
        /// 获取UP_WTDTPXQ内容
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        byte[] GetWtdUpImage(string tpid);

        /// <summary>
        /// 设置见证取样是否同意
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool JzqySetStatus(string qybh, string ryzh, string ryxm, string wtdwyh, bool agree, out string msg);

        /// <summary>
        /// 获取检测软件账号
        /// </summary>
        /// <param name="ryzh"></param>
        /// <returns></returns>
        string GetJcrjzh(string ryzh);

        /// <summary>
        /// 获取某个委托单见证试验详情
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> JzqyGetSyInfo(string wtdwyh, string viewWtdUrl, string url, out string msg);

        /// <summary>
        /// 根据二维码获取委托单信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> JzqyGetSyInfoByEwm(string ewmbh, out string msg);

        /// <summary>
        /// 无安全信息上传送样图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyUpImageNotSafe(string wtdwyh, string qybh, string ryxm, IList<byte[]> files, string ewm, string lon,
            string lat, string zh, out string msg);

        /// <summary>
        /// 根据二维码获取委托单信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> JzqyGetImagesByEwm(string url, string ewmbh, out string msg);

        /// <summary>
        /// 根据平台编号，组号获取委托单信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> JzqyGetImagesByZh(string url, string ptbh, string zh, out string msg);

        /// <summary>
        /// 获取某几个编号是否现场图片没拍，收样图片没拍
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyGetWtdTpzt(string qybh, string recids, out string msg);

        /// <summary>
        /// 删除二维码
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyDelEwm(string ewm, out string msg);

        /// <summary>
        /// 根据委托单获取工程信息及送样人信息
        /// </summary>
        /// <param name="wtdwyhs"></param>
        /// <param name="gcmc"></param>
        /// <param name="ryxx"></param>
        /// <returns></returns>
        bool JzqyGetJzrSmsInfo(string wtdwyhs, out string gcmc, out IList<string> ryxx);

        #endregion

        #region 见证取样(微信公众号)

        #endregion

        #region 监管首页面

        /// <summary>
        /// 管理人员对应区域列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJgAreaList(string usercode, out string msg);

        /// <summary>
        /// 管理人员对应检测机构列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJgJcjgList(string usercode, out string msg);

        /// <summary>
        /// 管理人员对应工程列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJgGcList(string usercode, string sfid, string csid, string qxid,
            string jdid, out string msg);

        /// <summary>
        /// 检测机构统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IDictionary<string, string> GetJgStatistic(string usercode, string sfid, string csid, string qxid, string jdid,
            string jcjgid, string gcid, out string msg);

        /// <summary>
        /// 获取报告合格不合格统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IDictionary<string, string> GetJgBgStatistic(string usercode, string sfid, string csid, string qxid,
            string jdid, string jcjgid, string gcid, out string msg);

        /// <summary>
        /// 获取委托单状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IDictionary<string, string> GetJgWtdztStatistic(string usercode, string sfid, string csid, string qxid,
            string jdid, string jcjgid, string gcid, out string msg);

        /// <summary>
        /// 获取委托单异常状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IDictionary<string, string> GetJgWtdycztStatistic(string usercode, string sfid, string csid, string qxid,
            string jdid, string jcjgid, string gcid, out string msg);

        /// <summary>
        /// 获取检测结构委托单状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IDictionary<string, IDictionary<string, string>> GetJgJcjgWtdztStatistic(string usercode, string sfid,
            string csid, string qxid, string jdid, string jcjgid, string gcid, out string msg);

        #endregion

        #region 组合项目

        bool CopyCombinationInfos(string wtdbh, out string msg);

        #endregion

        #region 手动上传报告

        /// <summary>
        /// 删除手动上传的报告，如果报告没有了，设置为试验状态
        /// </summary>
        /// <param name="bgwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteReportSdsc(string bgwyh, out string msg);

        /// <summary>
        /// 获取最后一份有效报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        List<byte[]> GetLastReportFile(string wtdwyh, out string msg);

        #endregion

        #region 登录上传图片

        IList<SysLogPic> SysLogPicGets(string userCode);

        SysLogPic SysLogPicSave(SysLogPic sysLogPic);

        #endregion

        #region 系统变量

        #region 其他设置

        /// <summary>
        /// 是否启用收样图片（0-不启用，1-启用）
        /// </summary>
        /// <returns></returns>
        bool SysUseSytp();

        #endregion

        #endregion

        #region 无见证二维码

        /// <summary>
        /// 无二维码情况
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sqms"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool NoQrcodeReq(string wtdwyh, string sqms, string username, string realName, out string msg);

        #endregion

        #region 委托单上传到oss

        /// <summary>
        /// 获取需要上传的委托单
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetUnUploadWtds();

        /// <summary>
        /// 设置委托单上传结果
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="succedd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWtdUploadResult(string recid, bool succeed, ref string msg);

        /// <summary>
        /// 获取已上传到oss的委托单地址
        /// </summary>
        /// <returns></returns>
        string GetUploadWtdUrl(string recid);

        #endregion

        #region 见证取样-标点

        /// <summary>
        /// 根据试验项目获取见证图片
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IDictionary<string, object> JzqyGetSyxmTpType(string syxmbh, out string msg);

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="qybh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="files"></param>
        /// <param name="ewm"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyUpImage3(string wtdwyh, string qybh, string ryzh, string ryxm,
            IList<IDictionary<string, byte[]>> files, byte[] ewm, string lon, string lat, string zh, out string msg);

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="qybh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="files"></param>
        /// <param name="ewm"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool JzqyUpImage4(string wtdwyh, string qybh, string ryzh, string ryxm,
            IList<IDictionary<string, byte[]>> files, string ewm, string lon, string lat, string zh, out string msg);

        //标点APP上传见证图片
        bool JzqyUpImage5(string wtdwyh, string qybh, string ryzh, string ryxm, IList<byte[]> files, string ewm,
            string lon, string lat, string zh, string type, out string msg);

        #endregion

        #region 更新委托单确定下载

        bool UpdateConfirmDownload(string recids, int qrxz, out string msg);

        #endregion

        #region 获取需要上传到Oss上的报告

        IList<IDictionary<string, object>> GetUploadReport();

        #endregion

        #region 设置报告上传到Oss成功的结果

        bool SetUploadReportResult(string bgwyh, int syh, string ossCdnUrl);

        #endregion

        #region 获取需要上传到Oss上的见证图片

        IList<IDictionary<string, object>> GetUploadJzPic();

        #endregion

        #region 设置见证图片上传到Oss成功的结果

        bool SetUploadJzPicResult(string tpxqwyh, string ossCdnUrl);

        #endregion

        #region 微信图片上传

        /// <summary>
        /// 下载微信图片
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        ResultParam DownloadWeiXinImage(string mediaid);

        #endregion

        #region 获取质监站下的所有检测机构

        bool GetJcjgs(string stationid, out IList<IDictionary<string, string>> records, out string msg);

        #endregion

        #region 根据检测机构获取试验项目

        bool GetSyxms(string stationid, string qybh, out IList<IDictionary<string, object>> records, out string msg);

        #endregion

        #region 委托单取消见证

        bool CancelWtdJz(string wtdwyh, out string msg);

        #endregion

        #region 委托单关联见证

        bool ContactWtdJz(string wtdwyh, string oldwtdwyh, out string msg);

        #endregion

        #region 查询当前用户可以关联的委托单

        bool GetContactWtd(out string msg, out string wtdwyhs);

        #endregion

        #region 微信端

        /// <summary>
        /// 二维码扫描
        /// </summary>
        /// <param name="bgwyh"></param>
        /// <returns></returns>
        ResultParam QRAntiFake(string bgwyh);

        #endregion

        #region 获取人员工程和人员类型(见证人,取样人)

        IList<IDictionary<string, string>> GetGcAndRylx(string userName, out bool code, out string msg);

        #endregion

        #region 非监督工程关联上级监督工程

        bool ContactJdgc(string gcbh, string ssjcjgbh, string sjgcbh, out string msg);

        #endregion

        #region 判断上级监督工程是否被检测机构重复引用

        bool IsJdgcUsed(string sjgcbh, out string qybh, out string msg);

        #endregion

        #region 更新监督抽查联系单内容

        bool InsertJG_JDBG_JDCCRWWTJL(VJG_JDBG_JDCCRWWTJL opinion, out string msg);

        bool InsertJG_JDBG_JDCCRWWTJL_DX(VJG_JDBG_JDCCRWWTJL opinion, out string msg);

        #endregion

        #region 判断委托单是否可以保存，校验见证类型和非见证类型的区别

        bool IsWtdSave(IDbCommand cmd, string jydbh, out List<string> jzRecIds, out string msg);

        #endregion

        #region 同步工程信息给建研

        bool SyncGcInfo(string gcbh, out Dictionary<string, string> dict, out string msg);

        #endregion

        #region 同步合同, 同步委托单

        ResultParam SyncJcjgHt(string htJson, string userCode, string userName);

        ResultParam SyncJcjgWtd(string mJson, string sJson, string userCode, string userName);

        #endregion

        #region 获取需要从建研那边同步的图片(建研)

        IList<IDictionary<string, string>> GetJyJzPic();

        #endregion

        #region 插入见证图片(建研)
        bool InsertWtdJzPic(IDictionary<string, string> jyjzPic, out string msg);
        #endregion

        #region 获取需要同步委托状态的见证记录(建研)

        IList<IDictionary<string, string>> GetJyOrderStatus();

        #endregion

        #region 更新委托状态同步标记(建研)

        bool UpdateJyOrderStatus(int recId, out string msg);

        #endregion

        #region 获取需要同步的见证取样修改记录(建研)

        IList<IDictionary<string, string>> GetJyItemUpdate();

        #endregion

        #region 获取需要同步的取样见证记录(建研)
        IList<IDictionary<string, string>> GetJyQyData(); 
        #endregion

        #region 插入见证取样修改记录

        bool InsertJyItemUpdate(string recid, string wtdwyh, string sRecId, List<Dictionary<string, string>> datas,
            out string msg);

        #endregion

        #region 插入见证取样记录
        bool InsertJyQyData(string recid, string wtdwyh, string jzRecId, out string msg);
        #endregion

        #region 根据账号获取人员编号

        bool GetRybhByZh(string zhs, out IList<IDictionary<string, string>> rys, out string msg);

        #endregion

        #region 根据工程编号获取试验项目

        bool GetSyxmsByGcbh(string gcbh, out IList<IDictionary<string, object>> records, out string msg);

        #endregion

        #region 获取工程坐标

        bool GetGczb(string gcbh, out List<Dictionary<string, string>> result, out string msg);

        #endregion

        #region 更新gcmcnew字段

        bool UpdateGCMCNEW();

        #endregion

        #region 获取单个工程的坐标

        bool GetGcPos(string gcbh, out object result, out string msg);

        #endregion

        #region 获取所有工程的坐标

        bool GetAllGcPos(string qybh, out List<Dictionary<string, object>> result, out string msg);

        #endregion

        #region 无二维码申请审核

        bool NoQrCodeAudit(string data, string userCode, string userName, out string msg);

        #endregion

        #region 设置非监督工程检测审核项目

        bool SetFjdGcAuditXm(string gcbh, string zjzbh, Dictionary<string, int> dict, out string msg);

        #endregion

        #region 判断非监督工程检测项目是否可以填单

        bool JudgeFjdGcAuditXm(string dwbh, string gcbh, string zjzbh, string syxmbh, out string msg);

        #endregion

        #region 新增人员变更记录

        bool InsertRyBghj(string zh, string czr, string czrxm, int czlx, out string msg);

        #endregion

        #region 设置人员单位

        bool SetRydw(string rybhs, string czr, string czrxm, out string msg);

        #endregion

        #region 清除人员单位

        bool ClearRydw(string rybhs, string czr, string czrxm, out string msg);

        #endregion

        #region 判断检测机构是否填写现场监管检测的检测日期

        bool JudgeXcjgjc(string qybh, string wtdwyhs, out string infoMsg, out string msg);

        #endregion

        #region 删除企业信息

        ResultParam DeleteIQy(string qybh);

        #endregion

        #region 设置受理委托编号

        ResultParam SetSlWtdbh(string dwbh, List<Dictionary<string, string>> datas);

        #endregion

        #region 获取委托单打印文件

        ResultParam GetWtdDywj(Dictionary<string, string> data);

        #endregion

        #region 设置委托单试验状态

        ResultParam SetWtdSyZt(string dwbh, Dictionary<string, string> data);

        #endregion

        #region 设置委托单状态为下发到检测机构, 判断检测系统和监管系统委托单是否一致

        bool SetWtdStatusXf2(string qybh, IDictionary<string, string> data, out string msg, bool sdsy = false);

        #endregion

        #region 共用函数

        /// <summary>
        /// 判断唯一号是否存在
        /// </summary>
        /// <returns></returns>
        ResultParam CheckWtdwyh(string wtdwyh);

        /// <summary>
        /// 判断唯一号是否存在及返回对应的单据类型信息
        /// </summary>
        /// <returns></returns>
        ResultParam CheckWtdwyhData(string wtdwyh);

        #endregion

        #region 萧山协会报告上传

        /// <summary>
        /// 获取上传协会字段接口
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultParam XsxhInterface_UploadField(string syxmbh, string status);

        #endregion

        #region 萧山获取接口类型

        IList<IDictionary<string, string>> getSysYcjkLx();
        bool getSysYcjkLx(string qybh, out string msg);

        /// <summary>
        /// 获取品铭工程信息
        /// </summary>
        /// <returns></returns>
        ResultParam XspmInterface_DownloadProject(string startRq, string endRq);

        #endregion

        #region 文件存储方式(1-OSS, 2-数据库)

        int GetSysFileStorage();

        #endregion

        #region 温州监管建研新见证流程(0-不启用, 1-启用)

        int GetSysWzJgJyNewJzqy();

        #endregion

        #region 人员录用申请(0-不启用, 1-启用)

        int GetSysRylysq();

        #endregion

        #region 一个见证人员只允许添加一个工程(0-不启用, 1-启用)

        int GetSysJzryZyxOneGc();

        #endregion

        #region 检测机构区域审批(0-不启用,1-启用)

        int GetSysJcjgQySp();

        #endregion

        #region 判断委托单修改申请

        ResultParam JudgeWtdModifyApply(string wtdwyh);

        #endregion

        #region 保存委托单修改申请

        ResultParam SaveWtdModifyApply(string wtdwyh, string applyReason, string applyUser, string applyUserName);

        #endregion

        #region 获取委托单修改申请

        ResultParam GetWtdModifyApply(string dwbh, Dictionary<string, string> data);

        #endregion

        #region 审核委托单修改申请

        ResultParam AuditWtdModifyApply(string dwbh, Dictionary<string, string> data);

        #endregion

        #region 根据委托单获取修改申请记录

        ResultParam GetViewWtdModifyApply(string wtdwyh);

        #endregion

        #region 获取修改申请详情

        ResultParam GetViewWtdModifyDetail(string applyId);

        #endregion

        #region 获取修改申请记录Id
        ResultParam GetWtdModifyApplyId(string wtdwyh);
        #endregion

        #region 获取现场项目类别

        IList<IDictionary<string, string>> GetXcxmlb();

        #endregion

        #region 获取现场项目

        IList<IDictionary<string, string>> GetXcxmData(string syxmbhs, int pageSize, int pageIndex, out int totalCount);

        #endregion

        #region 设置委托单审核签发状态

        ResultParam SetWtdSHQF(string dwbh, Dictionary<string, string> data);

        #endregion

        #region 获取建研见证取样配置

        IList<IDictionary<string, string>> GetJyJzqyHelpLink(string syxmbhs);

        #endregion

        #region 设置建研委托清单传递参数

        ResultParam InsertJyWtqd(string wtqdContent);

        #endregion

        #region 获取建研委托清单传递参数

        string GetJyWtqd(string recid);

        #endregion

        #region 人员录用申请审核

        ResultParam RylySqSh(string recids, string shr, string shrxm);

        #endregion

        #region 获取需要同步的工程见证坐标

        IList<IDictionary<string, string>> GetSyncGcPos(string startTime, string endTime);

        #endregion

        #region 数据库配置值

        string GetSysConfigValue(string configKey);

        bool SetSysConfigValue(string configKey, string configValue);

        #endregion

        #region 设置工程见证坐标记录

        ResultParam InsertGcPos(IDictionary<string, string> dict);

        #endregion

        #region 现场检测数据查看地址

        string GetXcjcUrl();

        #endregion

        #region 现场检测桩号修改申请

        ResultParam SaveXcjcModifyApply(string busynessid, string syid, string newptbh, string newzh, string oldptbh,
            string oldzh,
            string applyUser, string applyUserName, string applyReason);

        IList<IDictionary<string, string>> GetXcjcModifyApply(string syids);

        ResultParam GetAllXcjcModifyContent(string busynessid, string syid);

        ResultParam GetXcjcModifyContent(string busynessid, string syid);

        ResultParam AuditXcjcModifyApply(string applyId, string auditUser, string auditUserName, int audit);

        #endregion

        #region 校验检测机构区域内外是否已经审批

        ResultParam CheckJcjgQySp(string dwbh, string htbh);

        #endregion

        #region 区域内检测机构审批

        ResultParam JcJgQynSp(string recids, string expiryDate, string userCode, string realName);

        #endregion

        #region 初始化建研见证取样配置

        ResultParam InitJyJzqyHelpLink(string url);

        #endregion

        #region 判断是否使用标点检测系统
        ResultParam JudgeBDJcxt(string qybh); 
        #endregion

        #region 获取采集异常字段排除
        IList<IDictionary<string, string>> GetCjycZdpc(string syxmbh); 
        #endregion

        #region 获取报告异常字段排除
        IList<IDictionary<string, string>> GetBgycZdpc(string syxmbh); 
        #endregion

        #region 插入不合格报告的回复内容
        ResultParam InsertBhgBgHf(string bgwyh, string userCode, string userName, string hfnr, int hflx); 
        #endregion

        #region 采集系统接口

        /// <summary>
        /// 判断用户登录是否正确
        /// </summary>
        /// <param name="data">数据包</param>
        /// <returns></returns>
        ResultParam CjxtCheckUser(string data);

        /// <summary>
        /// 根据参数获取用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResultParam CjxtGetUserInfo(string data);

        /// <summary>
        /// 根据参数获取用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResultParam CjxtUserPower(string data);

        #endregion

        #region 统一入口
        /// <summary>
        /// 服务类(2019-10-19 杨鑫钢)
        /// </summary>
        /// <returns></returns>
        ResultParam Service();
        #endregion

        #region 大屏数据

        /// <summary>
        /// 获取检测机构与工程数
        /// </summary>
        /// <returns></returns>
        ResultParam GetScreenJcjgWtds();

        /// <summary>
        /// 获取基础数据统计
        /// </summary>
        /// <returns></returns>
        ResultParam GetScreenJbsjTj();

        /// <summary>
        /// 本月委托单与报告
        /// </summary>
        /// <returns></returns>
        ResultParam GetByWtdAndBg();

        /// <summary>
        /// 获取机构数
        /// </summary>
        /// <returns></returns>
        ResultParam GetJgs();


        /// <summary>
        /// 获取机构数
        /// </summary>
        /// <returns></returns>
        ResultParam GetGcs();

        /// <summary>
        /// 工程信息
        /// </summary>
        /// <returns></returns>
        ResultParam GetGcInfo();

        /// <summary>
        /// 检测机构数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        ResultParam GetScreenJcjgData(string queryJson, int pageSize, int pageIndex);
        #endregion

        #region 是否存在form配置
        ResultParam ExistFormDm(string formDm, string formStatus); 
        #endregion

        #region 萧山手动上传报告(协会二维码)
        ResultParam XsxhUploadBgField(string wtdwyh, string syxmbh, int type);

        ResultParam XsxhUploadBgData(string wtdwyh, string dataJson);
        #endregion

        #region 首页数据
        IDictionary<string, string> GetHomePageGcInfo();

        IDictionary<string, string> GetHomePageWtdInfo();

        IDictionary<string, string> GetHomePageJcjgInfo();

        IDictionary<string, string> GetHomePageReportInfo();

        IDictionary<string, string> GetHomePageCjInfo();

        IDictionary<string, string> GetHomePageYcReportInfo();

        List<Dictionary<string, string>> GetHomePageDbsxInfo(string userCode);

        IList<IDictionary<string, string>> GetHomePageGcData(int type);

        IDictionary<string, string> GetHomePageGcDetailData(string gcbh);

        Dictionary<string, object> GetHomePageJcjgData(int pageIndex, int pageSize);
        #endregion

        #region 企业申报
        ResultParam GetQyApplyQyInfo(string qybh);

        ResultParam GetQyApplyRyInfo(string qybh, int pageIndex, int pageSize);

        ResultParam GetQyApplySbInfo(string qybh, int pageIndex, int pageSize);

        ResultParam GetQyApplyModifyInfo(string recid);

        ResultParam UpdateQyApply(string qyJson, string ryJson, string sbJson, string zzJson, string cnsJson, string saveType,
            string sqr, string sqrxm);

        ResultParam AuditQyApply(string recid, string zt, string shr, string shrxm, string shsm);

        ResultParam DelQyApply(string recid);
        #endregion
    }
}
