using System.Collections.Generic;
using System.Data;
using System;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Common;

namespace BD.Jcbg.IBll
{
    /// <summary>
    /// 支付平台服务
    /// 作者：冯海夫
    /// 创建时间：2018-05-13
    /// 最后修改时间：2018-05-17
    /// </summary>
    public interface IPayService
    {
        /// <summary>
        /// 创建银行账号
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        VPayCreateBankAccountReturn CreateBankAccount(string lx, IDictionary<string,string> row);
        /// <summary>
        /// 查询账户信息
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        VPayCreateBankAccountReturn QueryBankAccountInfo(string lx, IDictionary<string, string> row);
        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        bool GetAccountInfo(string userid, out IList<IDictionary<string,string>> datas, out string msg);
        /// <summary>
        /// 获取子账号
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="qybh"></param>
        /// <param name="userid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetSubAccounts(string gcbh, string qybh, string userid, int pagesize, int pageindex, out int totalcount, out string msg);
        
        /// <summary>
        /// 删除工程发放单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteGcFfdw(string id, out string msg);
        /// <summary>
        /// 余额划出（企业到工程）
        /// </summary>
        /// <param name="fromusercode"></param>
        /// <param name="tozhid"></param>
        /// <param name="money"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool MoneyOut(string fromuserid, string tozhid, decimal money, string bz, string usercode, string realname, out string msg);
        /// <summary>
        /// 余额划入（工程到企业）
        /// </summary>
        /// <param name="fromzhid"></param>
        /// <param name="touserid"></param>
        /// <param name="money"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool MoneyIn(string fromzhid, string touserid, decimal money, string bz, string usercode, string realname, out string msg);
        /// <summary>
        /// 获取额度划拨记录
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="hbzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetMoneyTransList(string qyusercode, string hbzh, int pagesize, int pageindex, out int totalcount, out string msg);
        /// <summary>
        /// 工程发放账号
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="gcmc"></param>
        /// <param name="sgdw"></param>
        /// <param name="ffzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetProjectPayHistorySummary(string usercode, string gcmc, string sgdw, string ffzh, int pagesize, int pageindex, out int totalcount, out string msg);
        /// <summary>
        /// 获取发放类型
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetFflx(out string msg);
        /// <summary>
        /// 保存支付申请
        /// </summary>
        /// <param name="payinfos"></param>
        /// <param name="usercode"></param>
        /// <param name="yhyhid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SavePayListApply(IList<IDictionary<string, string>> payinfos, string usercode, string realname, string yhyhid, string fflx, string ffny, string bz1, string bz2, out string msg);
        /// <summary>
        /// 保存接口传入发放
        /// </summary>
        /// <param name="payinfos"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        VTransPayWgryRespBase SavePayListApply(VTransPayWgryReqSetPayRollMain payinfos);

        /// <summary>
        /// 获取待创建的账号列表：q-企业账号,p-工程账号,r-人员账号
        /// </summary>
        /// <returns></returns>
        IDictionary<string, IList<IDictionary<string, string>>> GetUnCreateInfos(out string msg);
        /// <summary>
        /// 获取未设置的账户信息：q-企业账号,p-工程账号,r-人员账号
        /// </summary>
        /// <returns></returns>
        IDictionary<string, IList<IDictionary<string, string>>> GetUnSetBankInfos(out string msg);
        /// <summary>
        /// 设置银行绑卡标识
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBindCardSign(string lx, string bh, string userid, out string msg);
        /// <summary>
        /// 创建银行虚拟账户标识
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetCreateAccountSign(string lx, string bh, string userid, out string msg);
        /// <summary>
        /// 设置银行用户创建信息
        /// </summary>
        /// <param name=""></param>
        /// <param name="bh"></param>
        /// <param name="yhyhid"></param>
        /// <param name="xnzh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetAccountCreateInfo(string lx, string bh, VPayCreateBankAccountReturn info, out string msg);
        /// <summary>
        /// 获取发放记录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="zt"></param>
        /// <param name="gcmc"></param>
        /// <param name="sgdw"></param>
        /// <param name="ffzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPayHistory(string usercode, string zt, string gcmc, string sgdw, string ffzh, string gcbh, string sgdwbh, string bz1, string gzzq1, string gzzq2, string spsj1, string spsj2, int pagesize, int pageindex, out int totalcount, out string msg, out decimal t1, out decimal t2);
        /// <summary>
        /// 获取发放详情
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPayDetail(string recid, string ffwc, string xm, int pagesize, int pageindex, out int totalcount, out string msg);
        /// <summary>
        /// 设置发放状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetPayStatus(string recid, int status, string usercode, string realname, string tradecode, out string msg, string totalmoney="", string payinfo="");
        /// <summary>
        /// 设置发放详情表内容
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetPayDetail(IList<IDictionary<string, string>> infos, out string msg);
        /// <summary>
        /// 获取正在发放的列表
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPayingList(int status, out string msg);
        /// <summary>
        /// 工人发工资
        /// </summary>
        /// <param name="fromzhid"></param>
        /// <param name="touserid"></param>
        /// <param name="money"></param>
        /// <param name="bz"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool PayToWorker(string fromzhid, string tozhid, decimal money, string usercode, string realname, string bz, string yhpt, out string msg);
        /// <summary>
        /// 回写发放结果
        /// </summary>
        /// <param name="ffid"></param>
        /// <param name="xqid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetPayResult(string ffid, string xqid, decimal ffje, string bz, out string msg);
        /// <summary>
        /// 获取账号关联的工程
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetRelateProjects(string usercode, out string msg);
        /// <summary>
        /// 获取账号关联的施工单位
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetRelateCompanys(string usercode, string gcbh, out string msg);
        /// <summary>
        /// 设置人员卡号无效
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetWgryInvalid(string id, string usercode, string realname, out string msg);
        /// <summary>
        /// 录用其他单位职员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool AddOtherEmployee(string usercode, string rybh, out string msg);
        /// <summary>
        /// 删除其他单位职员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteOtherEmployee(string usercode, string rybh, out string msg);
        /// <summary>
        /// 支付接口发送验证码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBankVerifyCode(string usercode, out string msg);
        /// <summary>
        /// 支付接口设置手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBankPhone(string usercode, string verifycode, string codeid, out string msg);
        /// <summary>
        /// 设置银行平台预发放
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBankPrePay(string payid, out string msg);
        /// <summary>
        /// 获取银行平台支付详情
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetBankPayDetail(string payid, string usercode, out string msg);
        /// <summary>
        /// 支付接口提现验证码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBankPayCode(string payid, string usercode, out string msg);
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetBankPay(string payid, string codeid, string verifycode, string usercode, out string msg);
        /// <summary>
        /// 获取当前账号对应的企业列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetUserCompanys(string usercode, out string msg);
        /// <summary>
        /// 务工人员系统设置发放结果
        /// </summary>
        /// <returns></returns>
        bool SetWgryPayResult(string payid, string key, out string msg);
        /// <summary>
        /// 获取还未推送消息给务工人员得支付
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetUnMessagePay(out string msg);
        /// <summary>
        /// 设置已通知给务工人员
        /// </summary>
        /// <param name="payid"></param>
        /// <returns></returns>
        bool SetWgryMessageSend(string payid);
        /// <summary>
        /// 支付是否又绑卡错误
        /// </summary>
        /// <param name="payid"></param>
        /// <returns></returns>
        bool PayHasBindcardError(string payid);
        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        VTransPayWgryRespBase SaveWgryInfo(VTransPayWgryReqBindCardMain info, out IDictionary<string, string> createInfos);
        /// <summary>
        /// 获取支付企业的工程信息，带当前管理人员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPayCompnyProjects(string usercode, out string msg);
        /// <summary>
        /// 获取某个支付企业财务的
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetFinancePersonProjects(string rybh, string lx, string usercode, out string msg);
        /// <summary>
        /// 保存财务人员工程
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveFinancePersonProjects(string rybh, string gczfqyid, string lx, string usercode, out string msg);
        /// <summary>
        /// 获取待审批的工资册
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetUncheckPays(string gcbh, string sgdwbh, string usercode, out string msg);
        /// <summary>
        /// 工程册审批
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="status"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="tradecode"></param>
        /// <param name="msg"></param>
        /// <param name="totalmoney"></param>
        /// <returns></returns>
        bool CheckPay(string recid, bool agree, string usercode, string realname, ref string msg);
        /// <summary>
        /// 获取某个工资册的附件
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetPayrollAttachs(string recid, out string msg);
        /// <summary>
        /// 获取附件内容
        /// </summary>
        /// <param name="attachid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        KeyValuePair<string,byte[]> GetAttachContent(string attachid, out string msg);
        /// <summary>
        /// 获取发放所有附件内容，base64编码
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string,string>> GetAttachContentByPay(string payid, out string msg);
        /// <summary>
        /// 修改人员卡号
        /// </summary>
        /// <param name="items"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        VTransPayWgryRespSetPersonCard ChangeRyCardInfo(VTransPayWgryReqSetPersonCardItem[] items);
        /// <summary>
        /// 修改一个人员卡号，修改一条发放记录的
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool ChangeOneRyCardInfo(string ffcbid, string cardno, string usercode, string realnam, out string msg);
        /// <summary>
        /// 工资发放余额是否足够
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool PayMoneyEnough(string payid, string usercode, out string msg);

        /// <summary>
        /// 获取企业的资金划拨人，及企业所有人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetMoneyTransPersons(string usercode, out string msg);
        /// <summary>
        /// 获取企业的第一步审批人，及企业所有人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetCheck1Persons(string usercode, out string msg);
        /// <summary>
        /// 获取企业的第二步审批人，及企业所有人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetCheck2Persons(string usercode, out string msg);
        /// <summary>
        /// 保存企业的财务人员岗位
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveFinacePerson(string usercode, string userids1, string userids2, string userids3, string userid11, string userid12, out string msg);
        /// <summary>
        /// 用户能否资金划拨
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CanMoneyTrans(string usercode, out string msg);
        /// <summary>
        /// 获取银行回单
        /// </summary>
        /// <returns></returns>
        byte[] GetPayBankBack(string payid, string usercode, out string msg);
        /// <summary>
        /// 重新提交一条记录
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool ResubmitPayItem(string ffcbid, string usercode, string realnam, out string msg);
        /// <summary>
        /// 获取支付系统行号
        /// </summary>
        /// <param name="cardno"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetYhhh(string cardno, out string msg);
        /// <summary>
        /// 取消一条记录
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool CancelPayItem(string ffcbid, string usercode, string realnam, out string msg);
        /// <summary>
        /// 直接取消银行发放（用于本地人员无记录）
        /// </summary>
        /// <param name="ffcbid"></param>
        /// <param name="usercode"></param>
        /// <param name="realnam"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CancelBankPayItem(string payid, string orderid, string usercode, string realnam, out string msg);
        bool TempResubmit(string ffcbid, out string msg);
        bool UnbindCard(string rykbh, out string msg);
        bool BindCard(string rykbh, out string msg);
        /// <summary>
        /// 月发放统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetMonthPayStatistic(int year, int month, string usercode, out string msg);
        /// <summary>
        /// 获取人员银行信息
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <returns></returns>
        IList<VTransPayRespQueryUserDataItem> GetUserBankInfos(string sfzhm, string usercode, out string msg);
        /// <summary>
        /// 验证人员卡号是否正确；先查询本地记录；如果本地记录找不或者没错误，去银行接口验证，如果绑卡成功，写入数据库；如果绑卡失败，不写入数据库
        /// </summary>
        /// <param name="reqinfo"></param>
        /// <returns></returns>
        VTransPayWgryRespBase VerifyUserCard(VTransPayWgryReqBindCardMain reqinfo);
        /// <summary>
        /// 校验身份证喝银行卡是否有重复
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        bool CheckRepeatPayItem(ref IList<IDictionary<string, string>> datas);
        /// <summary>
        /// 校验导入的工资册中无效的内容
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        void CheckInvalidPayItem(ref IList<IDictionary<string, string>> datas);

        /// <summary>
        /// 获取务工人员卡号信息
        /// </summary>
        /// <returns></returns>
        IDictionary<string,IList<IDictionary<string, string>>> GetWgryCards();
        /// <summary>
        /// 删除务工人员银行卡
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        bool DeleteWgryCard(string recid, out string msg);
        /// <summary>
        /// 获取需要预提现的列表
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		IList<IDictionary<string, string>> GetNeedPrePayList(out string msg);
        /// <summary>
        /// 获取劳务公司列表
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetPayCompanys(out string msg);
        /// <summary>
        /// 获取劳务公司工程子账号
        /// </summary>
        /// <param name="paycompayids"></param>
        /// <returns></returns>
        IList<VTransPayRespGetUserInfoDataItem> GetSubAccuonts(string usercode, string paycompayids, out string msg);
        /// <summary>
        /// 纠正创建账户失败的条目
        /// </summary>
        void CorrectErrorCreateCardItems();
        /// <summary>
        /// 纠正提现失败的条目
        /// </summary>
        void CorrectErrorWithdrawItems();
        /// <summary>
        /// 获取所有管理人员，及公司所属管理人员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		IList<IDictionary<string, string>> GetManagePersons(string usercode, out string msg);
        /// <summary>
        /// 获取管理得支付企业
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns>企业编号(qybh)，企业名称(qymc)</returns>
        IList<IDictionary<string, string>> GetManagePayCompanys(string usercode, string gw, out string msg);
        /// <summary>
        /// 获取管理的工程
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="paycompany"></param>
        /// <param name="msg"></param>
        /// <returns>工程编号(gcbh),工程名称(gcmc)</returns>
        IList<IDictionary<string, string>> GetManageProjects(string usercode, string paycompany, string gw, out string msg);
        /// <summary>
        /// 获取管理得发放记录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="zt"></param>
        /// <param name="gcmc"></param>
        /// <param name="sgdw"></param>
        /// <param name="ffzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetManagePayHistory(string usercode, string zt, string gcmc, string sgdw, string ffzh, string zfdwbh, string gcbh, string sgdwbh, string bz1, string gzzq1, string gzzq2, string spsj1, string spsj2, int pagesize, int pageindex, out int totalcount, out string msg, out decimal t1, out decimal t2);
        /// <summary>
        /// 获取转账记录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="hbzh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<VTransPayRespGetRechargeListItem> GetRechargeList(string usercode, string hbzh, int pagesize, int pageindex, out int totalcount, out string msg);
    }
}
