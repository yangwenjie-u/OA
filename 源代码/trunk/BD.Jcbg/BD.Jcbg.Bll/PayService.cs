using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Bank.Domain.Virtual;
using Bd.jcbg.Common;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;

namespace BD.Jcbg.Bll
{
	// Token: 0x02000006 RID: 6
	public class PayService : IPayService
	{
        #region Dao
        public ICommonDao CommonDao { get; set; }
        #endregion
             
        #region 服务
        /// <summary>
        /// 创建银行账号
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public VPayCreateBankAccountReturn CreateBankAccount(string lx, IDictionary<string, string> row)
		{
			VPayCreateBankAccountReturn vpayCreateBankAccountReturn = new VPayCreateBankAccountReturn();
			try
			{
				CL_RegisterUser cl_RegisterUser = new CL_RegisterUser();
				CL_BindSettlement bindInfo = new CL_BindSettlement();
				CL_BindPersonalSettlement cl_BindPersonalSettlement = new CL_BindPersonalSettlement();
				CL_QueryUser cl_QueryUser = new CL_QueryUser
				{
					PapersCode = ""
				};
				if (row == null)
				{
					vpayCreateBankAccountReturn.Succeed = false;
					vpayCreateBankAccountReturn.ErrorMessage = "数据内容不能为空";
					return vpayCreateBankAccountReturn;
				}
				if (lx.Equals("r", StringComparison.OrdinalIgnoreCase))
				{
					vpayCreateBankAccountReturn.NeedBindCard = (row["bkzt"] == "0");
					vpayCreateBankAccountReturn.NeedCreateAccount = (row["cjyhzhzt"] == "0");
				}
				else if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
					vpayCreateBankAccountReturn.NeedBindCard = (row["bkzt"] == "0");
					vpayCreateBankAccountReturn.NeedCreateAccount = (row["cjyhzhzt"] == "0");
				}
				else if (lx.Equals("p", StringComparison.OrdinalIgnoreCase))
				{
					vpayCreateBankAccountReturn.NeedBindCard = (row["r_bkzt"] == "0");
					vpayCreateBankAccountReturn.NeedCreateAccount = (row["r_cjyhzhzt"] == "0");
				}
				if (!vpayCreateBankAccountReturn.NeedBindCard && !vpayCreateBankAccountReturn.NeedCreateAccount)
				{
					return vpayCreateBankAccountReturn;
				}
				string yhpt;
				string userid;
				if (lx.Equals("r", StringComparison.OrdinalIgnoreCase))
				{
					cl_RegisterUser.TradeMemBerName = row["ryxm"];
					cl_RegisterUser.TradeMemberProperty = "1";
					cl_RegisterUser.Contact = "";
					cl_RegisterUser.ContactPhone = "";
					cl_RegisterUser.Phone = row["sjhm"];
					cl_RegisterUser.ContactAddr = "";
					cl_RegisterUser.BusinessName = "";
					cl_RegisterUser.PapersType = "10";
					cl_RegisterUser.PapersCode = row["sfzhm"].DecodeDes("8zzsjd95", "fcb95eze");
					cl_RegisterUser.IsMessager = "2";
					cl_RegisterUser.MessagePhone = "";
					cl_RegisterUser.Email = "";
					cl_RegisterUser.Remark1 = "";
					cl_RegisterUser.Remark2 = "";
					cl_RegisterUser.Remark3 = "";
					cl_RegisterUser.Remark4 = "";
					cl_RegisterUser.Remark5 = "";
					vpayCreateBankAccountReturn.AccountUser = row["r_yhyhid"];
					yhpt = row["r_yhpt"];
					userid = row["r_lrrzh"];
					cl_BindPersonalSettlement.SettleAccountName = cl_RegisterUser.TradeMemBerName;
					cl_BindPersonalSettlement.SettleAccount = row["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
					cl_BindPersonalSettlement.PayBank = row["yhhh"];
					cl_QueryUser.PapersCode = cl_RegisterUser.PapersCode;
					string resp = "";
					if (this.QueryUser(cl_QueryUser, userid, yhpt, out resp))
					{
						VTransPayRespQueryUser vtransPayRespQueryUser = this.ParseResponse<VTransPayRespQueryUser>(resp);
						if (vtransPayRespQueryUser.IsSucceed)
						{
							IEnumerable<VTransPayRespQueryUserDataItem> source = from e in vtransPayRespQueryUser.data.data
							where e.SettleAccount.Equals(bindInfo.SettleAccount, StringComparison.OrdinalIgnoreCase) && e.TradeMemBerName.Equals(bindInfo.SettleAccountName, StringComparison.OrdinalIgnoreCase)
							select e;
                            source = source.OrderByDescending(e => e.SettleAccount);
							if (source.Count<VTransPayRespQueryUserDataItem>() > 0)
							{
								VTransPayRespQueryUserDataItem vtransPayRespQueryUserDataItem = source.ElementAt(0);
								vpayCreateBankAccountReturn.AccountUser = vtransPayRespQueryUserDataItem.UserId;
								vpayCreateBankAccountReturn.AccountCode = vtransPayRespQueryUserDataItem.OthBankPayeeSubAcc;
								vpayCreateBankAccountReturn.AccountName = vtransPayRespQueryUserDataItem.OthBankPayeeSubAccName;
								vpayCreateBankAccountReturn.BankPointName = vtransPayRespQueryUserDataItem.OthBankPayeeSubAccSetteName;
								vpayCreateBankAccountReturn.CreateAccountSucceed = true;
								vpayCreateBankAccountReturn.BindCardSucceed = vtransPayRespQueryUserDataItem.BindSucceed;
								vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespQueryUserDataItem.SettleAccountMessage;
							}
						}
					}
				}
				else if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
                    string zhmc = row["qymc"];
                    if (zhmc.Length > 30)
                        zhmc = zhmc.Substring(0, 30);
					cl_RegisterUser.TradeMemBerName = zhmc;
					cl_RegisterUser.TradeMemberProperty = "0";
					cl_RegisterUser.Contact = row["qyfzr"];
					cl_RegisterUser.ContactPhone = row["lxdh"];
					cl_RegisterUser.Phone = row["lxsj"];
					cl_RegisterUser.ContactAddr = row["yydz"];
					cl_RegisterUser.BusinessName = row["qyfr"];
					cl_RegisterUser.PapersType = "16";
					cl_RegisterUser.PapersCode = row["zzjgdm"];
					cl_RegisterUser.IsMessager = "2";
					cl_RegisterUser.MessagePhone = row["lxsj"];
					cl_RegisterUser.Email = "";
					cl_RegisterUser.Remark1 = "";
					cl_RegisterUser.Remark2 = "";
					cl_RegisterUser.Remark3 = "";
					cl_RegisterUser.Remark4 = "";
					cl_RegisterUser.Remark5 = "";
					vpayCreateBankAccountReturn.AccountUser = row["yhyhid"];
					yhpt = row["yhpt"];
					userid = row["lrrzh"];
					bindInfo.IsOther = "1";
					bindInfo.AccountSign = row["jsklx"];
					bindInfo.IsOtherBank = row["sfbh"];
					bindInfo.SettleAccountName = zhmc;
					bindInfo.SettleAccount = row["jskh"];
					bindInfo.IsSecondAcc = "0";
					bindInfo.PayBank = row["yhhh"];
					bindInfo.BankName = "";
					bindInfo.StrideValidate = "1";
					cl_QueryUser.PapersCode = cl_RegisterUser.PapersCode;
					string resp2 = "";
					if (this.QueryUser(cl_QueryUser, userid, yhpt, out resp2))
					{
						VTransPayRespQueryUser vtransPayRespQueryUser2 = this.ParseResponse<VTransPayRespQueryUser>(resp2);
						if (vtransPayRespQueryUser2.IsSucceed)
						{
							IEnumerable<VTransPayRespQueryUserDataItem> source2 = from e in vtransPayRespQueryUser2.data.data
							where e.SettleAccount.Equals(bindInfo.SettleAccount, StringComparison.OrdinalIgnoreCase)
							select e;
							if (source2.Count<VTransPayRespQueryUserDataItem>() > 0)
							{
								VTransPayRespQueryUserDataItem vtransPayRespQueryUserDataItem2 = source2.ElementAt(0);
								vpayCreateBankAccountReturn.AccountUser = vtransPayRespQueryUserDataItem2.UserId;
								vpayCreateBankAccountReturn.AccountCode = vtransPayRespQueryUserDataItem2.OthBankPayeeSubAcc;
								vpayCreateBankAccountReturn.AccountName = vtransPayRespQueryUserDataItem2.OthBankPayeeSubAccName;
								vpayCreateBankAccountReturn.BankPointName = vtransPayRespQueryUserDataItem2.OthBankPayeeSubAccSetteName;
								vpayCreateBankAccountReturn.CreateAccountSucceed = true;
								vpayCreateBankAccountReturn.BindCardSucceed = vtransPayRespQueryUserDataItem2.BindSucceed;
								vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespQueryUserDataItem2.SettleAccountMessage;
							}
						}
					}
				}
				else
				{
					if (!lx.Equals("p", StringComparison.OrdinalIgnoreCase))
					{
						vpayCreateBankAccountReturn.ErrorMessage = "无效的类型";
						vpayCreateBankAccountReturn.Succeed = false;
						return vpayCreateBankAccountReturn;
					}
                    string jgzh = row["jgzh"].GetSafeString();
                    string jghm = row["jghm"].GetSafeString();

                    if (string.IsNullOrEmpty(jghm))
                    {
                        jghm = row["gcmc"];
                        if (jghm.Length > 30)
                            jghm = jghm.Substring(0, 30);
                    }
                    if (string.IsNullOrEmpty(jgzh))
                    {
                        jgzh = row["zzjgdm"] + "_" + row["r_recid"].GetSafeString("");
                    }
					cl_RegisterUser.TradeMemBerName = jghm;
					cl_RegisterUser.TradeMemberProperty = "0";
					cl_RegisterUser.Contact = row["qyfzr"];
					cl_RegisterUser.ContactPhone = row["lxdh"];
					cl_RegisterUser.Phone = row["lxsj"];
					cl_RegisterUser.ContactAddr = row["yydz"];
					cl_RegisterUser.BusinessName = row["qyfr"];
					cl_RegisterUser.PapersType = "16";
					cl_RegisterUser.PapersCode = jgzh;
					cl_RegisterUser.IsMessager = "2";
					cl_RegisterUser.MessagePhone = row["lxsj"];
					cl_RegisterUser.Email = "";
					cl_RegisterUser.Remark1 = "";
					cl_RegisterUser.Remark2 = "";
					cl_RegisterUser.Remark3 = "";
					cl_RegisterUser.Remark4 = "";
					cl_RegisterUser.Remark5 = "";
					cl_RegisterUser.ParentUserId = row["yhyhid"];
					vpayCreateBankAccountReturn.AccountUser = row["r_yhyhid"];
					yhpt = row["yhpt"];
					userid = row["r_lrrzh"];
					bindInfo.IsOther = "1";
					bindInfo.AccountSign = row["jsklx"];
					bindInfo.IsOtherBank = row["sfbh"];
					bindInfo.SettleAccountName = jghm;
					bindInfo.SettleAccount = row["jskh"];
					bindInfo.IsSecondAcc = "0";
					bindInfo.PayBank = row["r_yhhh"];
					bindInfo.BankName = "";
					bindInfo.StrideValidate = "1";
					if (string.IsNullOrEmpty(cl_RegisterUser.ParentUserId))
					{
						vpayCreateBankAccountReturn.ErrorMessage = "企业银行账户未创建";
						vpayCreateBankAccountReturn.Succeed = false;
						return vpayCreateBankAccountReturn;
					}
				}
				if (string.IsNullOrEmpty(cl_RegisterUser.Email))
				{
					cl_RegisterUser.Email = "bdsoft@sina.com";
				}
				string resp3 = "";
				if (vpayCreateBankAccountReturn.NeedCreateAccount && !vpayCreateBankAccountReturn.CreateAccountSucceed)
				{
					vpayCreateBankAccountReturn.Succeed = this.RegisterUser(cl_RegisterUser, userid, yhpt, out resp3);
					if (vpayCreateBankAccountReturn.Succeed)
					{
						VTransPayRespRegisterUser vtransPayRespRegisterUser = this.ParseResponse<VTransPayRespRegisterUser>(resp3);
						if (vtransPayRespRegisterUser.IsSucceed)
						{
							vpayCreateBankAccountReturn.CreateAccountSucceed = true;
							vpayCreateBankAccountReturn.BankErrorMessage = "";
							vpayCreateBankAccountReturn.AccountUser = vtransPayRespRegisterUser.data.UserId;
						}
						else
						{
							vpayCreateBankAccountReturn.Succeed = false;
							vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespRegisterUser.message;
						}
					}
				}
				if (!vpayCreateBankAccountReturn.Succeed)
				{
					return vpayCreateBankAccountReturn;
				}
				if (string.IsNullOrEmpty(vpayCreateBankAccountReturn.AccountUser))
				{
					vpayCreateBankAccountReturn.Succeed = false;
					vpayCreateBankAccountReturn.ErrorMessage = "银行用户ID获取失败";
					return vpayCreateBankAccountReturn;
				}
				if (vpayCreateBankAccountReturn.NeedBindCard && !vpayCreateBankAccountReturn.BindCardSucceed)
				{
					if (lx.Equals("r", StringComparison.OrdinalIgnoreCase))
					{
						cl_BindPersonalSettlement.UserId = vpayCreateBankAccountReturn.AccountUser;
						vpayCreateBankAccountReturn.Succeed = this.BindPersonalSettlement(cl_BindPersonalSettlement, userid, yhpt, out resp3);
						if (vpayCreateBankAccountReturn.Succeed)
						{
							VTransPayRespBindSettlement vtransPayRespBindSettlement = this.ParseResponse<VTransPayRespBindSettlement>(resp3);
							if (vtransPayRespBindSettlement.IsSucceed)
							{
								vpayCreateBankAccountReturn.BindCardSucceed = true;
								vpayCreateBankAccountReturn.BankErrorMessage = "";
							}
							else
							{
								vpayCreateBankAccountReturn.BindCardSucceed = false;
								vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespBindSettlement.message;
							}
						}
					}
					else
					{
						bindInfo.UserId = vpayCreateBankAccountReturn.AccountUser;
						vpayCreateBankAccountReturn.Succeed = this.BindSettlement(bindInfo, userid, yhpt, out resp3);
						if (vpayCreateBankAccountReturn.Succeed)
						{
							VTransPayRespBindSettlement vtransPayRespBindSettlement2 = this.ParseResponse<VTransPayRespBindSettlement>(resp3);
							if (vtransPayRespBindSettlement2.IsSucceed)
							{
								vpayCreateBankAccountReturn.BindCardSucceed = true;
								vpayCreateBankAccountReturn.BankErrorMessage = "";
							}
							else
							{
								vpayCreateBankAccountReturn.BindCardSucceed = false;
								vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespBindSettlement2.message;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				vpayCreateBankAccountReturn.ErrorMessage = ex.Message;
				vpayCreateBankAccountReturn.Succeed = false;
			}
			return vpayCreateBankAccountReturn;
		}
        /// <summary>
        /// 查询账户信息
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="row"></param>
        /// <returns></returns>
		public VPayCreateBankAccountReturn QueryBankAccountInfo(string lx, IDictionary<string, string> row)
		{
			VPayCreateBankAccountReturn vpayCreateBankAccountReturn = new VPayCreateBankAccountReturn
			{
				NeedCreateAccount = true,
				CreateAccountSucceed = true
			};
			try
			{
				CL_QueryUser cl_QueryUser = new CL_QueryUser
				{
					PapersCode = ""
				};
				string cardno = "";
				string cardname = "";
				if (row == null)
				{
					vpayCreateBankAccountReturn.Succeed = false;
					vpayCreateBankAccountReturn.ErrorMessage = "数据内容不能为空";
					return vpayCreateBankAccountReturn;
				}
				string yhpt;
				string userid;
				if (lx.Equals("r", StringComparison.OrdinalIgnoreCase))
				{
					cl_QueryUser.PapersCode = row["sfzhm"].DecodeDes("8zzsjd95", "fcb95eze");
					yhpt = row["r_yhpt"];
					userid = row["r_lrrzh"];
					cardno = row["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
					cardname = row["ryxm"];
				}
				else if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
					cl_QueryUser.PapersCode = row["zzjgdm"];
					yhpt = row["yhpt"];
					userid = row["lrrzh"];
					cardno = row["jskh"];
					cardname = "";
				}
				else
				{
					if (!lx.Equals("p", StringComparison.OrdinalIgnoreCase))
					{
						vpayCreateBankAccountReturn.ErrorMessage = "无效的类型";
						vpayCreateBankAccountReturn.Succeed = false;
						return vpayCreateBankAccountReturn;
					}
                    string jgzh = row["jgzh"].GetSafeString();
                    if (string.IsNullOrEmpty(jgzh))
                        jgzh = row["zzjgdm"] + "_" + row["r_recid"].GetSafeString("");

                    cl_QueryUser.PapersCode = jgzh;
					yhpt = row["r_yhpt"];
					userid = row["r_lrrzh"];
					cardno = row["r_jskh"];
					cardname = "";
				}
				string resp = "";
				if (this.QueryUser(cl_QueryUser, userid, yhpt, out resp))
				{
					VTransPayRespQueryUser vtransPayRespQueryUser = this.ParseResponse<VTransPayRespQueryUser>(resp);
					if (vtransPayRespQueryUser.IsSucceed)
					{
						IEnumerable<VTransPayRespQueryUserDataItem> source = from e in vtransPayRespQueryUser.data.data
						where e.UserId == row["r_yhyhid"]
						select e;
                        source = source.OrderByDescending(e => e.SettleAccount);
						if (source.Count<VTransPayRespQueryUserDataItem>() > 0)
						{
							VTransPayRespQueryUserDataItem vtransPayRespQueryUserDataItem = source.ElementAt(0);
							vpayCreateBankAccountReturn.AccountUser = vtransPayRespQueryUserDataItem.UserId;
							vpayCreateBankAccountReturn.AccountCode = vtransPayRespQueryUserDataItem.OthBankPayeeSubAcc;
							vpayCreateBankAccountReturn.AccountName = vtransPayRespQueryUserDataItem.OthBankPayeeSubAccName;
							vpayCreateBankAccountReturn.BankPointName = vtransPayRespQueryUserDataItem.OthBankPayeeSubAccSetteName;
							vpayCreateBankAccountReturn.BankErrorMessage = vtransPayRespQueryUserDataItem.SettleAccountMessage;
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				vpayCreateBankAccountReturn.ErrorMessage = ex.Message;
				vpayCreateBankAccountReturn.Succeed = false;
			}
			return vpayCreateBankAccountReturn;
		}
        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        public bool GetAccountInfo(string userid, out IList<IDictionary<string, string>> datas, out string msg)
		{
			bool flag = true;
			msg = "";
			datas = new List<IDictionary<string, string>>();
			try
			{
				List<string> list = new List<string>();
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_qyzh where yhzh='" + userid + "'");
				if (dataTable.Count == 0)
				{
					flag = false;
					msg = "非企业或人员账号";
					return flag;
				}
				string text = dataTable[0]["sfqyzzh"].GetSafeBool(false) ? "q" : "qr";
				string safeString = dataTable[0]["qybh"].GetSafeString("");
				string yhpt;
				if (text.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
					IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable("select yhpt,yhyhid,yhzh,yhzhmc from i_s_qy_yhzh where qybh='" + safeString + "'");
					if (dataTable2.Count == 0)
					{
						msg = "找不企业记录";
						flag = false;
						return flag;
					}
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("zhid", dataTable2[0]["yhyhid"]);
					dictionary.Add("zzh", "1");
					dictionary.Add("yhzh", dataTable2[0]["yhzh"]);
					dictionary.Add("yhzhmc", dataTable2[0]["yhzhmc"]);
					datas.Add(dictionary);
					yhpt = dataTable2[0]["yhpt"];
					list.Add(dataTable2[0]["yhyhid"]);
				}
				else if (text.Equals("qr", StringComparison.OrdinalIgnoreCase))
				{
					IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable("select yhpt,yhyhid,yhzh,yhzhmc from i_s_qy_yhzh where qybh=(select qybh from i_m_ry where rybh='" + safeString + "')");
					if (dataTable3.Count == 0)
					{
						msg = "找不企业记录";
						flag = false;
						return flag;
					}
					IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
					dictionary2.Add("zhid", dataTable3[0]["yhyhid"]);
					dictionary2.Add("zzh", "1");
					dictionary2.Add("yhzh", dataTable3[0]["yhzh"]);
					dictionary2.Add("yhzhmc", dataTable3[0]["yhzhmc"]);
					datas.Add(dictionary2);
					yhpt = dataTable3[0]["yhpt"];
					list.Add(dataTable3[0]["yhyhid"]);
				}
				else
				{
					if (!text.Equals("r", StringComparison.OrdinalIgnoreCase))
					{
						flag = false;
						msg = "无效的类型";
						return flag;
					}
					IList<IDictionary<string, string>> dataTable4 = this.CommonDao.GetDataTable("select yhpt,yhyhid from i_m_wgry where rybh='" + safeString + "'");
					if (dataTable4.Count == 0)
					{
						msg = "找不到人员记录";
						flag = false;
						return flag;
					}
					yhpt = dataTable4[0]["yhpt"];
					list.Add(dataTable4[0]["yhyhid"]);
					IDictionary<string, string> dictionary3 = new Dictionary<string, string>();
					dictionary3.Add("zhid", dataTable4[0]["yhyhid"]);
					dictionary3.Add("zzh", "1");
					datas.Add(dictionary3);
				}
				CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
				cl_GetUserInfos.UserIds.AddRange(list);
				flag = this.GetUserInfos(cl_GetUserInfos, userid, yhpt, out msg);
				if (flag)
				{
					VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
					if (vtransPayRespGetUserInfo.IsSucceed)
					{
						msg = "操作成功";
						VTransPayRespGetUserInfoDataItem[] data = vtransPayRespGetUserInfo.data;
						for (int i = 0; i < data.Length; i++)
						{
							VTransPayRespGetUserInfoDataItem vitem = data[i];
							IDictionary<string, string> dictionary4 = (from e in datas
							where e["zhid"].Equals(vitem.UserId)
							select e).First<IDictionary<string, string>>();
							if (dictionary4 != null)
							{
								dictionary4.Add("zhye", vitem.AccountBalance.ToString());
							}
						}
					}
					else
					{
						flag = false;
						msg = vtransPayRespGetUserInfo.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				flag = false;
			}
			return flag;
		}
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
		public IList<IDictionary<string, string>> GetSubAccounts(string gcbh, string qybh, string userid, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            msg = "";
            IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            totalcount = 0;
            try
            {
                IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_qyzh where yhzh='" + userid + "'");
                if (dataTable.Count == 0)
                {
                    msg = "非企业或人员账号";
                    return list;
                }
                string text = dataTable[0]["sfqyzzh"].GetSafeBool(false) ? "q" : "qr";
                string safeString = dataTable[0]["qybh"].GetSafeString("");
                IList<IDictionary<string, string>> list2 = new List<IDictionary<string, string>>();
                
                string text2 = "";
                if (gcbh != "")
                {
                    text2 = text2 + " and a.gcbh='" + gcbh + "' ";
                }
                if (qybh != "")
                {
                    text2 = text2 + " and a.qybh='" + qybh + "' ";
                }
                if (text.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    list2 = this.CommonDao.GetPageData("select a.yhpt,a.yhyhid,a.yhzh,b.gcbh,b.gcmc,c.qymc,a.qybh,(select count(*) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffcs, (select sum(sfze) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffje,(select max(CJSJ) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffsj,(select sum(yfze) from i_m_pay where i_m_pay.ffzh=a.yhyhid and i_m_pay.zt=0) as spje from i_s_gc_zfdw a inner join i_m_gc b on a.gcbh=b.gcbh inner join i_s_gc_sgdw c on a.qybh=c.qybh and b.gcbh=c.gcbh where a.zfqybh='" + safeString + "'" + text2, pagesize, pageindex, out totalcount);
                }
                else
                {
                    list2 = this.CommonDao.GetPageData("select a.yhpt,a.yhyhid,a.yhzh,b.gcbh,b.gcmc,c.qymc,a.qybh,(select count(*) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffcs, (select sum(sfze) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffje,(select max(CJSJ) from i_m_pay where i_m_pay.ffzh=a.yhyhid) as ffsj,(select sum(yfze) from i_m_pay where i_m_pay.ffzh=a.yhyhid and i_m_pay.zt=0) as spje from i_s_gc_zfdw a inner join i_m_gc b on a.gcbh=b.gcbh inner join i_s_gc_sgdw c on a.qybh=c.qybh and b.gcbh=c.gcbh where a.zfqybh=(select qybh from i_m_ry where rybh='" + safeString + "') " + text2, pagesize, pageindex, out totalcount);
                }

                List<string> yhpts = new List<string>();
                foreach (IDictionary<string, string> dictionary in list2)
                {
                    IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
                    dictionary2.Add("zhid", dictionary["yhyhid"]);
                    dictionary2.Add("zhmc", dictionary["gcmc"]);
                    dictionary2.Add("sgqymc", dictionary["qymc"]);
                    dictionary2.Add("gcbh", dictionary["gcbh"]);
                    dictionary2.Add("zzh", "0");
                    dictionary2.Add("ffcs", dictionary["ffcs"]);
                    dictionary2.Add("ffje", dictionary["ffje"]);
                    dictionary2.Add("sgqybh", dictionary["qybh"]);
                    dictionary2.Add("yhzh", dictionary["yhzh"]);
                    if (dictionary["ffsj"] != "")
                    {
                        dictionary2.Add("ffsj", dictionary["ffsj"].GetSafeDate().ToString("yyy-MM-dd HH:mm"));
                    }
                    else
                    {
                        dictionary2.Add("ffsj", "");
                    }
                    dictionary2.Add("spje", dictionary["spje"]);
                    list.Add(dictionary2);
                    var findYhpts = yhpts.Where(e => e.Equals(dictionary["yhpt"], StringComparison.OrdinalIgnoreCase));
                    if (findYhpts.Count() == 0)
                        yhpts.Add(dictionary["yhpt"]);
                }
                foreach (string yhpt in yhpts)
                {
                    List<string> list3 = new List<string>();
                    foreach (IDictionary<string, string> dictionary in list2)
                    {
                        if (dictionary["yhpt"].Equals(yhpt, StringComparison.OrdinalIgnoreCase))
                            list3.Add(dictionary["yhyhid"]);                        
                    }
                    CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
                    cl_GetUserInfos.UserIds.AddRange(list3);
                    if (this.GetUserInfos(cl_GetUserInfos, userid, yhpt, out msg))
                    {
                        VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
                        if (vtransPayRespGetUserInfo.IsSucceed)
                        {
                            msg = "";
                            VTransPayRespGetUserInfoDataItem[] data = vtransPayRespGetUserInfo.data;
                            for (int i = 0; i < data.Length; i++)
                            {
                                VTransPayRespGetUserInfoDataItem vitem = data[i];
                                IDictionary<string, string> dictionary3 = (from e in list
                                                                           where e["zhid"].Equals(vitem.UserId)
                                                                           select e).First<IDictionary<string, string>>();
                                if (dictionary3 != null)
                                {
                                    dictionary3.Add("zhye", vitem.AccountBalance.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return list;
        }
        /// <summary>
        /// 删除工程发放单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool DeleteGcFfdw(string id, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				result = this.CommonDao.ExecCommand("delete from I_S_GC_ZFDW where recid='" + id + "'", CommandType.Text, null, -1);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 余额划出（企业到工程）
        /// </summary>
        /// <param name="fromusercode"></param>
        /// <param name="tozhid"></param>
        /// <param name="money"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool MoneyOut(string fromuserid, string tozhid, decimal money, string bz, string usercode, string realname, out string msg)
		{
			bool flag = true;
			msg = "";
			try
			{
				if (string.IsNullOrEmpty(fromuserid))
				{
					flag = false;
					msg = "转出用户不能为空";
					return flag;
				}
				if (string.IsNullOrEmpty(tozhid))
				{
					flag = false;
					msg = "转入用户不能为空";
					return flag;
				}
				if (!this.CanMoneyTrans(usercode, out msg))
				{
					msg = "当前用户没有额度划拨权限";
					return false;
				}
				List<string> list = new List<string>();
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select yhpt,qybh,qymc,yhyhid from i_s_qy_yhzh where qybh=(select qybh from i_m_qyzh where yhzh='",
					fromuserid,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					fromuserid,
					"'))"
				}));
				if (dataTable.Count == 0)
				{
					msg = "找不划出企业记录";
					flag = false;
					return flag;
				}
				string yhpt = dataTable[0]["yhpt"];
				string text = dataTable[0]["qybh"];
				string text2 = dataTable[0]["qymc"];
				string text3 = dataTable[0]["yhyhid"];
				list.Add(text3);
				dataTable = this.CommonDao.GetDataTable("select b.gcmc,b.gcbh,c.qybh,c.qymc from i_s_gc_zfdw a inner join i_m_gc b on a.gcbh=b.gcbh left outer join i_s_gc_sgdw c on a.gcbh=b.gcbh and a.qybh=c.qybh where a.yhyhid='" + tozhid + "'");
				if (dataTable.Count == 0)
				{
					msg = "找不划入工程记录";
					flag = false;
					return flag;
				}
				string text4 = dataTable[0]["qybh"];
				string text5 = dataTable[0]["qymc"];
				string text6 = dataTable[0]["gcbh"];
				string text7 = dataTable[0]["gcmc"];
				CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
				cl_GetUserInfos.UserIds.AddRange(list);
				flag = this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
					if (vtransPayRespGetUserInfo.IsSucceed)
					{
						msg = "获取成功";
						if (vtransPayRespGetUserInfo.data.Length == 0)
						{
							flag = false;
							msg = "获取企业账户信息失败";
						}
						else if (vtransPayRespGetUserInfo.data[0].AccountBalance < money)
						{
							flag = false;
							msg = "账户余额为不足，当前账户余额：" + vtransPayRespGetUserInfo.data[0].AccountBalance;
						}
					}
					else
					{
						flag = false;
						msg = "获取企业账户信息失败，详细信息：" + vtransPayRespGetUserInfo.message;
					}
				}
				if (!flag)
				{
					return flag;
				}
				CL_Pay info = new CL_Pay
				{
					Amount = money,
					FromUserId = text3,
					ToUserId = tozhid,
					TradeComment = bz
				};
				flag = this.Pay(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespPay vtransPayRespPay = this.ParseResponse<VTransPayRespPay>(msg);
					if (vtransPayRespPay.IsSucceed)
					{
						msg = "划拨成功";
						string cmdTxt = string.Concat(new object[]
						{
							"insert into I_M_EDFB ([RECID],[ZFQYBH],[SGQYBH],[HBSJ],[CZRZH],[CZRXM],[SGQYMC],[ZFQYMC],[ZFQYZH],[GCBH],[GCMC],[HBZH],[HBJE],[ZT],[MARK],[JYLSH]) values('",
							Guid.NewGuid().ToString(),
							"','",
							text,
							"','",
							text4,
							"',getdate(),'",
							usercode,
							"','",
							realname,
							"','",
							text5,
							"','",
							text2,
							"','",
							text3,
							"','",
							text6,
							"','",
							text7,
							"','",
							tozhid,
							"',",
							money,
							",1,'",
							bz,
							"','",
							vtransPayRespPay.data.OrderId,
							"')"
						});
						flag = this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					}
					else
					{
						flag = false;
						msg = "划拨失败，详细信息：" + vtransPayRespPay.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				flag = false;
			}
			return flag;
		}
        /// <summary>
        /// 余额划入（工程到企业）
        /// </summary>
        /// <param name="fromzhid"></param>
        /// <param name="touserid"></param>
        /// <param name="money"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool MoneyIn(string fromzhid, string touserid, decimal money, string bz, string usercode, string realname, out string msg)
		{
			bool flag = true;
			msg = "";
			try
			{
				if (string.IsNullOrEmpty(fromzhid))
				{
					flag = false;
					msg = "转出用户不能为空";
					return flag;
				}
				if (string.IsNullOrEmpty(touserid))
				{
					flag = false;
					msg = "转入用户不能为空";
					return flag;
				}
				if (!this.CanMoneyTrans(usercode, out msg))
				{
					msg = "当前用户没有额度划拨权限";
					return false;
				}
				List<string> list = new List<string>();
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select yhpt,qybh,qymc,yhyhid from i_s_qy_yhzh where qybh=(select qybh from i_m_qyzh where yhzh='",
					touserid,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					touserid,
					"'))"
				}));
				if (dataTable.Count == 0)
				{
					msg = "找不划入企业记录";
					flag = false;
					return flag;
				}
				string yhpt = dataTable[0]["yhpt"];
				string text = dataTable[0]["qybh"];
				string text2 = dataTable[0]["qymc"];
				string text3 = dataTable[0]["yhyhid"];
				list.Add(fromzhid);
				dataTable = this.CommonDao.GetDataTable("select b.gcmc,b.gcbh,c.qybh,c.qymc from i_s_gc_zfdw a inner join i_m_gc b on a.gcbh=b.gcbh left outer join i_s_gc_sgdw c on a.gcbh=b.gcbh and a.qybh=c.qybh where a.yhyhid='" + fromzhid + "'");
				if (dataTable.Count == 0)
				{
					msg = "找不划出工程记录";
					flag = false;
					return flag;
				}
				string text4 = dataTable[0]["qybh"];
				string text5 = dataTable[0]["qymc"];
				string text6 = dataTable[0]["gcbh"];
				string text7 = dataTable[0]["gcmc"];
				CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
				cl_GetUserInfos.UserIds.AddRange(list);
				flag = this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
					if (vtransPayRespGetUserInfo.IsSucceed)
					{
						msg = "获取成功";
						if (vtransPayRespGetUserInfo.data.Length == 0)
						{
							flag = false;
							msg = "获取工程账户信息失败";
						}
						else if (vtransPayRespGetUserInfo.data[0].AccountBalance < money)
						{
							flag = false;
							msg = "账户余额为不足，当前账户余额：" + vtransPayRespGetUserInfo.data[0].AccountBalance;
						}
					}
					else
					{
						flag = false;
						msg = "获取企业账户信息失败，详细信息：" + vtransPayRespGetUserInfo.message;
					}
				}
				if (!flag)
				{
					return flag;
				}
				CL_Pay info = new CL_Pay
				{
					Amount = money,
					FromUserId = fromzhid,
					ToUserId = text3,
					TradeComment = bz
				};
				flag = this.Pay(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespPay vtransPayRespPay = this.ParseResponse<VTransPayRespPay>(msg);
					if (vtransPayRespPay.IsSucceed)
					{
						msg = "划拨成功";
						string cmdTxt = string.Concat(new object[]
						{
							"insert into I_M_EDFB ([RECID],[ZFQYBH],[SGQYBH],[HBSJ],[CZRZH],[CZRXM],[SGQYMC],[ZFQYMC],[ZFQYZH],[GCBH],[GCMC],[HBZH],[HBJE],[ZT],[MARK],[JYLSH]) values('",
							Guid.NewGuid().ToString(),
							"','",
							text,
							"','",
							text4,
							"',getdate(),'",
							usercode,
							"','",
							realname,
							"','",
							text5,
							"','",
							text2,
							"','",
							fromzhid,
							"','",
							text6,
							"','",
							text7,
							"','",
							text3,
							"',",
							money,
							",1,'",
							bz,
							"','",
							vtransPayRespPay.data.OrderId,
							"')"
						});
						flag = this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					}
					else
					{
						flag = false;
						msg = "划拨失败，详细信息：" + vtransPayRespPay.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				flag = false;
			}
			return flag;
		}
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
		public IList<IDictionary<string, string>> GetMoneyTransList(string qyusercode, string hbzh, int pagesize, int pageindex, out int totalcount, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			totalcount = 0;
			try
			{
				string text = string.Concat(new string[]
				{
					"select * from i_m_edfb where (ZFQYBH=(select qybh from i_m_qyzh where yhzh='",
					qyusercode,
					"') or ZFQYBH=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					qyusercode,
					"'))) "
				});
				if (!string.IsNullOrEmpty(hbzh))
				{
					text = string.Concat(new string[]
					{
						text,
						" and (ZFQYZH='",
						hbzh,
						"' or HBZH='",
						hbzh,
						"') "
					});
				}
				text += " order by HBSJ desc";
				list = this.CommonDao.GetPageData(text, pagesize, pageindex, out totalcount);
				foreach (IDictionary<string, string> dictionary in list)
				{
					dictionary["hbsj"] = dictionary["hbsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
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
		public IList<IDictionary<string, string>> GetProjectPayHistorySummary(string usercode, string gcmc, string sgdw, string ffzh, int pagesize, int pageindex, out int totalcount, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			totalcount = 0;
			try
			{
				string text = "select a.gcmc,b.qymc,c.yhyhid,(select count(*) from i_m_pay where i_m_pay.ffzh=c.yhyhid) as ffcs, (select sum(sfze) from i_m_pay where i_m_pay.ffzh=c.yhyhid) as ffje,(select max(CJSJ) from i_m_pay where i_m_pay.ffzh=c.yhyhid) as ffsj,(select sum(yfze) from i_m_pay where i_m_pay.ffzh=c.yhyhid and i_m_pay.zt=0) as spje from i_m_gc a inner join i_s_gc_sgdw b on a.gcbh=b.gcbh inner join i_s_gc_zfdw c on b.gcbh=c.gcbh and b.qybh=c.qybh where c.zfqybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "') ";
				if (!string.IsNullOrEmpty(gcmc))
				{
					text = text + " and a.gcmc like '%" + gcmc + "%' ";
				}
				if (!string.IsNullOrEmpty(sgdw))
				{
					text = text + " and b.qymc like '%" + sgdw + "%' ";
				}
				if (!string.IsNullOrEmpty(ffzh))
				{
					text = text + " and c.yhyhid like '%" + ffzh + "%' ";
				}
				text += " order by a.gcmc asc";
				list = this.CommonDao.GetPageData(text, pagesize, pageindex, out totalcount);
				foreach (IDictionary<string, string> dictionary in list)
				{
					if (dictionary["ffsj"] != "")
					{
						dictionary["ffsj"] = dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
        /// <summary>
        /// 获取发放类型
        /// </summary>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetFflx(out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				result = this.CommonDao.GetDataTable("select * from h_fflx where sfyx=1 order by xssx,lxmc");
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
         /// <summary>
        /// 保存支付申请
        /// </summary>
        /// <param name="payinfos"></param>
        /// <param name="usercode"></param>
        /// <param name="yhyhid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SavePayListApply(IList<IDictionary<string, string>> payinfos, string usercode, string realname, string yhyhid, string fflx, string ffny, string bz1, string bz2, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
                CheckInvalidPayItem(ref payinfos);
                CheckRepeatPayItem(ref payinfos);
                payinfos = payinfos.Where(e => e["sfyx"] != "2").ToList();
                
				foreach (IDictionary<string, string> dictionary in payinfos)
				{
                    if (string.IsNullOrEmpty(dictionary["yhhh"]))
                        dictionary["yhhh"] = DefaultBankNumber;
				}

                IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable("select a.gcbh,a.gcmc,b.qybh,b.qymc,c.zfqybh,c.zfqymc,c.YHPT from i_m_gc a inner join i_s_gc_sgdw b on a.gcbh=b.gcbh inner join I_S_GC_ZFDW c on b.gcbh=c.gcbh and b.qybh=c.qybh where c.yhyhid='" + yhyhid + "'");
				if (dataTable3.Count == 0)
				{
					msg = "获取工程信息失败";
					return result;
				}
				string text3 = dataTable3[0]["gcbh"];
				string text4 = dataTable3[0]["gcmc"];
				string text5 = dataTable3[0]["qybh"];
				string text6 = dataTable3[0]["qymc"];
				string text7 = dataTable3[0]["zfqybh"];
				string text8 = dataTable3[0]["zfqymc"];
                string yhpt = dataTable3[0]["yhpt"];

				StringBuilder sbIds = new StringBuilder();
				StringBuilder sbCards = new StringBuilder();
				StringBuilder sbNames = new StringBuilder();
				foreach (IDictionary<string, string> dictionary2 in payinfos)
				{
                    string idnumber = dictionary2["身份证号码"].GetSafeRequest();
                    string card = dictionary2["银行卡号"].GetSafeRequest();
                    string encodeId1 = CryptFun.LrEncode(idnumber.ToLower());
                    string encodeId2 =  CryptFun.LrEncode(idnumber.ToUpper());
                    string encodeCard1 = CryptFun.LrEncode(card.ToLower());
                    string encodeCard2 =  CryptFun.LrEncode(card.ToUpper());
					string strName = dictionary2["姓名"].GetSafeRequest();
					sbIds.Append(encodeId1 + ",");
                    if (encodeId1 != encodeId2)
                        sbIds.Append(encodeId2 + ",");

                    sbCards.Append(encodeCard1 + ",");
                    if (encodeCard1 != encodeCard2)
                        sbCards.Append(encodeCard2 + ","); 

					sbNames.Append(strName + ",");
				}
				string[] array = ffny.Split(new char[]
				{
					'-'
				});
				if (array.Length < 2 || array[0].GetSafeInt(0) == 0 || array[1].GetSafeInt(0) == 0)
				{
					msg = "发放年月无效";
					return result;
				}
				int safeInt = array[0].GetSafeInt(0);
				int safeInt2 = array[1].GetSafeInt(0);
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from I_M_WGRY where sfzhm in (",
					sbIds.ToString().FormatSQLInStr(),
					") and ryxm in (",
					sbNames.ToString().FormatSQLInStr(),
					")"
				}));
				IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from i_s_wgry_yhzh where sfyx=1 and YHPT='"+yhpt+"' and rybh in (select rybh from I_M_WGRY where sfzhm in (",
					sbIds.ToString().FormatSQLInStr(),
					")) and JSKH in (",
					sbCards.ToString().FormatSQLInStr(),
					")"
				}));
				
				string text10 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
				decimal num = 0m;
				string cmdTxt;
                IList<KeyValuePair<string,string>> addWgrys = new List<KeyValuePair<string,string>>();
                IList<KeyValuePair<string,string>> addWgrycards = new List<KeyValuePair<string,string>>();
				foreach (IDictionary<string, string> dictionary3 in payinfos)
				{
					string xm = dictionary3["姓名"].GetSafeRequest();
					string dh = dictionary3["电话"].GetSafeRequest();
                    string orgsfzhm = dictionary3["身份证号码"].GetSafeRequest();
					string sfzhm = CryptFun.LrEncode(orgsfzhm);
                    string orgyhkh = dictionary3["银行卡号"].GetSafeRequest();
					string yhkh = CryptFun.LrEncode(orgyhkh);
					decimal safeDecimal = dictionary3["实发工资"].GetSafeDecimal(0m);
					string safeRequest3 = dictionary3["备注"].GetSafeRequest();
					string safeString = dictionary3["yhhh"].GetSafeString("");
					IEnumerable<IDictionary<string, string>> source = from e in dataTable
					where CryptFun.LrDecode(e["sfzhm"]).Equals(orgsfzhm, StringComparison.OrdinalIgnoreCase) && e["ryxm"] == xm select e;
                    if (source.Count() > 1)
                    {
                        IList<IDictionary<string, string>> realDts = new List<IDictionary<string, string>>();
                        foreach (var mrow in source)
                        {
                            var find = from e in dataTable2
					            where e["rybh"] == mrow["rybh"] && CryptFun.LrDecode(e["jskh"]).Equals(orgyhkh, StringComparison.OrdinalIgnoreCase)
					            select e;
                            if (find.Count() > 0)
                            {
                                realDts.Add(mrow);
                                break;
                            }                            
                        }
                        source = realDts;
                    }
                    var findAddWgrys = addWgrys.Where(e => e.Key.Equals(orgsfzhm, StringComparison.OrdinalIgnoreCase));
					string rybh = "";
                    if (source.Count<IDictionary<string, string>>() == 0 && findAddWgrys.Count() == 0)
                    {
                        rybh = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        this.CommonDao.ExecCommand(string.Concat(new string[]
                        {
                            "insert into i_m_wgry(SSJCJGBH,LXBH,QYBH,RYBH,RYXM,SFZHM,LRRZH,LRRXM,SSDWBH,SSDWMC,LRSJ,SPTG,SFYX,ZH,SJHM) values('','11','','",
                            rybh,
                            "','",
                            xm,
                            "','",
                            sfzhm,
                            "','",
                            usercode,
                            "','",
                            realname,
                            "','','',getdate(),1,1,'','",
                            dh,
                            "')"
                        }), CommandType.Text, null, -1);
                        addWgrys.Add(new KeyValuePair<string, string>(orgsfzhm, rybh));
                    }
                    else if (source.Count<IDictionary<string, string>>() > 0)
                    {
                        rybh = source.ElementAt(0)["rybh"];
                    }
                    else if (findAddWgrys.Count() > 0)
                        rybh = findAddWgrys.ElementAt(0).Value;
					IEnumerable<IDictionary<string, string>> source2 = from e in dataTable2
					where e["rybh"] == rybh && CryptFun.LrDecode(e["jskh"]).Equals(orgyhkh, StringComparison.OrdinalIgnoreCase)
					select e;
                    var findCards = addWgrycards.Where(e => e.Key.Equals(orgsfzhm + orgyhkh, StringComparison.OrdinalIgnoreCase));
                    string text11 = "";
                    if (source2.Count<IDictionary<string, string>>() == 0 && findCards.Count() == 0)
                    {
                        text11 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        bool flag = BankInfo.IsHuaxiaCard(orgyhkh);
                        cmdTxt = string.Concat(new object[]
                        {
                            "insert into i_s_wgry_yhzh(RECID,RYBH,LRRZH,LRRXM,LRSJ,SPTG,SFYX,YHYHID,SFBH,JSKLX,JSKH,YHPT,MRZH,YHHH,BKZT,CJYHZHZT) values('",
                            text11,
                            "','",
                            rybh,
                            "','",
                            usercode,
                            "','",
                            realname,
                            "',getdate(),1,1,'',",
                            flag ? 0 : 1,
                            ",0,'",
                            yhkh,
                            "','",
                            yhpt,
                            "',0,'",
                            safeString,
                            "',0,0)"
                        });
                        this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
                        addWgrycards.Add(new KeyValuePair<string, string>(orgsfzhm + orgyhkh, text11));
                    }
                    else if (source2.Count<IDictionary<string, string>>() > 0)
                    {
                        text11 = source2.ElementAt(0)["recid"];
                    }
                    else if (findCards.Count() > 0)
                        text11 = findCards.ElementAt(0).Value;
                     
					string text12 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
					cmdTxt = string.Concat(new object[]
					{
						"insert into I_S_PAY_XQ(RECID,RYBH,RYXM,RYKBH,YFJE,SFJE,ZT,CJSJ,FFID,bz) values('",
						text12,
						"','",
						rybh,
						"','",
						xm,
						"','",
						text11,
						"',",
						safeDecimal,
						",0,0,getdate(),'",
						text10,
						"','",
						safeRequest3,
						"')"
					});
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					num += safeDecimal.GetSafeDecimal(0m);
				}
				cmdTxt = string.Concat(new object[]
				{
					"insert into I_M_PAY(RECID,GCBH,QYBH,ZFQYBH,FFZH,YFZE,SFZE,FFLX,FFNF,FFYF,ZT,CJSJ,CJRZH,CJRXM,GCMC,QYMC,ZFQYMC,PTLSH,BZ1,BZ2,YHPT) values('",
					text10,
					"','",
					text3,
					"','",
					text5,
					"','",
					text7,
					"','",
					yhyhid,
					"',",
					num,
					",0,'",
					fflx,
					"',",
					safeInt,
					",",
					safeInt2,
					",0,getdate(),'",
					usercode,
					"','",
					realname,
					"','",
					text4,
					"','",
					text6,
					"','",
					text8,
					"','','",
					bz1,
					"','",
					bz2,
					"','"+yhpt+"')"
				});
				this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
				result = true;
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
         /// <summary>
        /// 保存接口传入发放
        /// </summary>
        /// <param name="payinfos"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public VTransPayWgryRespBase SavePayListApply(VTransPayWgryReqSetPayRollMain payinfos)
		{
			VTransPayWgryRespBase vtransPayWgryRespBase = new VTransPayWgryRespBase
			{
				success = "0000",
				message = ""
			};
			string text = "interface";
			string text2 = "接口导入";
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				StringBuilder stringBuilder3 = new StringBuilder();
				if (string.IsNullOrEmpty(payinfos.paytype))
				{
					payinfos.paytype = "01";
				}
				if (string.IsNullOrEmpty(payinfos.paycode))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "发放编号不能为空");
					return vtransPayWgryRespBase;
				}
				if (string.IsNullOrEmpty(payinfos.projectcode))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "工程编号不能为空");
					return vtransPayWgryRespBase;
				}
				if (string.IsNullOrEmpty(payinfos.companycode))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "施工企业编号不能为空");
					return vtransPayWgryRespBase;
				}
				if (string.IsNullOrEmpty(payinfos.paycompanycode))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "发放企业编号不能为空");
					return vtransPayWgryRespBase;
				}
				if (string.IsNullOrEmpty(payinfos.payyear))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "发放年份不能为空");
					return vtransPayWgryRespBase;
				}
				if (string.IsNullOrEmpty(payinfos.paymonth))
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "发放月份不能为空");
					return vtransPayWgryRespBase;
				}
				if (payinfos.rows == null || payinfos.rows.Length == 0)
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "发放列表不能为空");
					return vtransPayWgryRespBase;
				}
                VTransPayWgryReqSetPayRollItem[] items = payinfos.rows;
                string msg = CheckInvalidPayItem(ref items);
                if (!string.IsNullOrEmpty(msg))
                {
                    vtransPayWgryRespBase.success = VTransPayWgryRespBase.ErrorParam;
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, msg);
					return vtransPayWgryRespBase;
                }
                msg = CheckRepeatPayItem(ref items);
                if (!string.IsNullOrEmpty(msg))
                {
                    vtransPayWgryRespBase.success = VTransPayWgryRespBase.ErrorParam;
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, msg);
					return vtransPayWgryRespBase;
                }
                IList<VTransPayWgryReqSetPayRollItem> validpayrows = new List<VTransPayWgryReqSetPayRollItem>();
                foreach (VTransPayWgryReqSetPayRollItem vtransPayWgryReqSetPayRollItem in payinfos.rows)
                {
                    vtransPayWgryReqSetPayRollItem.idnumber = vtransPayWgryReqSetPayRollItem.idnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64).GetValidPayString().EncodeDes(LRLB_KEY_64, LRLB_IV_64);
                    vtransPayWgryReqSetPayRollItem.cardnumber = vtransPayWgryReqSetPayRollItem.cardnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64).GetValidPayString().EncodeDes(LRLB_KEY_64, LRLB_IV_64);

                    vtransPayWgryReqSetPayRollItem.name = vtransPayWgryReqSetPayRollItem.name.GetValidPayString();
                    vtransPayWgryReqSetPayRollItem.phone = vtransPayWgryReqSetPayRollItem.phone.GetValidPayString();
                    vtransPayWgryReqSetPayRollItem.paysum = vtransPayWgryReqSetPayRollItem.paysum.GetValidPayString();

                    if (!string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.name) &&
                        !string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.idnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64)) &&
                        !string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.cardnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64)) &&
                        !string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.phone) &&
                        !string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.paysum))
                    {
                        if (string.IsNullOrEmpty(vtransPayWgryReqSetPayRollItem.banknumber))
                        {
                            string tmpmsg = "";
                            if (GetBankNameByAli(vtransPayWgryReqSetPayRollItem.cardnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64), out tmpmsg))
                                vtransPayWgryReqSetPayRollItem.banknumber = tmpmsg;
                            else
                                vtransPayWgryReqSetPayRollItem.banknumber = "104100000004";
                        }
                        if (IDCardValidation.CheckIDCard(vtransPayWgryReqSetPayRollItem.idnumber.DecodeDes(LRLB_KEY_64, LRLB_IV_64)))
                        {
                            validpayrows.Add(vtransPayWgryReqSetPayRollItem);
                        }
                    }
                }
                payinfos.rows = validpayrows.ToArray();
				foreach (VTransPayWgryReqSetPayRollItem vtransPayWgryReqSetPayRollItem in payinfos.rows)
				{
                    string idnumber = CryptFun.LrDecode(vtransPayWgryReqSetPayRollItem.idnumber);
                    string card = CryptFun.LrDecode(vtransPayWgryReqSetPayRollItem.cardnumber);
                    string encodeId1 = CryptFun.LrEncode(idnumber.ToLower());
                    string encodeId2 =  CryptFun.LrEncode(idnumber.ToUpper());
                    string encodeCard1 = CryptFun.LrEncode(card.ToLower());
                    string encodeCard2 =  CryptFun.LrEncode(card.ToUpper());
					stringBuilder.Append(encodeId1 + ",");
                    if (encodeId1 != encodeId2)
                        stringBuilder.Append(encodeId2 + ",");

                    stringBuilder2.Append(encodeCard1 + ",");
                    if (encodeCard1 != encodeCard2)
                        stringBuilder2.Append(encodeCard2 + ","); 

                    
					stringBuilder3.Append(vtransPayWgryReqSetPayRollItem.name + ",");
				}
				int safeInt = payinfos.payyear.GetSafeInt(0);
				int safeInt2 = payinfos.paymonth.GetSafeInt(0);
				if (this.CommonDao.GetDataTable("select * from i_m_pay where ptlsh='" + payinfos.paycode + "'").Count > 0)
				{
					vtransPayWgryRespBase.success = "0004";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "工资册不能重复推送");
					return vtransPayWgryRespBase;
				}
				IEnumerable<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select qybh,qybh_yc from i_m_qy where qybh_yc in ('",
					payinfos.companycode,
					"','",
					payinfos.paycompanycode,
					"')"
				}));
				string text3 = "";
				string text4 = "";
				string text5 = "";
				foreach (IDictionary<string, string> dictionary in dataTable)
				{
					if (dictionary["qybh_yc"].Equals(payinfos.companycode, StringComparison.OrdinalIgnoreCase))
					{
						text3 = dictionary["qybh"];
					}
					if (dictionary["qybh_yc"].Equals(payinfos.paycompanycode, StringComparison.OrdinalIgnoreCase))
					{
						text4 = dictionary["qybh"];
					}
				}
				IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable("select gcbh,gcbh_yc from i_m_gc where gcbh_yc='" + payinfos.projectcode + "'");
				if (dataTable2.Count<IDictionary<string, string>>() > 0)
				{
					text5 = dataTable2[0]["gcbh"];
				}
				IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from I_M_WGRY where sfzhm in (",
					stringBuilder.ToString().FormatSQLInStr(),
					") and ryxm in (",
					stringBuilder3.ToString().FormatSQLInStr(),
					")"
				}));
                
				IList<IDictionary<string, string>> dataTable5 = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select a.gcbh,a.gcmc,b.qybh,b.qymc,c.zfqybh,c.zfqymc,c.yhyhid,c.YHPT from i_m_gc a inner join i_s_gc_sgdw b on a.gcbh=b.gcbh inner join I_S_GC_ZFDW c on b.gcbh=c.gcbh and b.qybh=c.qybh where c.gcbh='",
					text5,
					"' and c.qybh='",
					text3,
					"' and c.zfqybh='",
					text4,
					"'"
				}));
				if (dataTable5.Count == 0)
				{
					vtransPayWgryRespBase.success = "0003";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, string.Concat(new string[]
					{
						"工程:",
						text5,
						",=》施工企业:",
						text3,
						",=》支付企业:",
						text4,
						"，在支付平台未绑定"
					}));
					return vtransPayWgryRespBase;
				}
				string text6 = dataTable5[0]["gcbh"];
				string text7 = dataTable5[0]["gcmc"];
				string text8 = dataTable5[0]["qybh"];
				string text9 = dataTable5[0]["qymc"];
				string text10 = dataTable5[0]["zfqybh"];
				string text11 = dataTable5[0]["zfqymc"];
				string text12 = dataTable5[0]["yhyhid"];
                string yhpt = dataTable5[0]["yhpt"];
				IList<IDictionary<string, string>> dataTable4 = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from i_s_wgry_yhzh where sfyx=1 and YHPT='"+yhpt+"' and rybh in (select rybh from I_M_WGRY where sfzhm in (",
					stringBuilder.ToString().FormatSQLInStr(),
					")) and JSKH in (",
					stringBuilder2.ToString().FormatSQLInStr(),
					")"
				}));
				string text14 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
				decimal num = 0m;
				VTransPayWgryReqSetPayRollItem[] rows = payinfos.rows;
				string cmdTxt;
                IList<KeyValuePair<string,string>> addWgrys = new List<KeyValuePair<string,string>>();
                IList<KeyValuePair<string,string>> addWgrycards = new List<KeyValuePair<string,string>>();

				for (int i = 0; i < rows.Length; i++)
				{
					VTransPayWgryReqSetPayRollItem vtransPayWgryReqSetPayRollItem2 = rows[i];                    
					string xm = vtransPayWgryReqSetPayRollItem2.name;

					string phone = vtransPayWgryReqSetPayRollItem2.phone;
					string sfzhm = vtransPayWgryReqSetPayRollItem2.idnumber;
                    string orgsfzhm = CryptFun.LrDecode(sfzhm);
					string yhkh = vtransPayWgryReqSetPayRollItem2.cardnumber;
                    string orgyhkh = CryptFun.LrDecode(yhkh);
					decimal safeDecimal = vtransPayWgryReqSetPayRollItem2.paysum.GetSafeDecimal(0m);
					string banknumber = vtransPayWgryReqSetPayRollItem2.banknumber;
					string remark = vtransPayWgryReqSetPayRollItem2.remark1;
					IEnumerable<IDictionary<string, string>> source = from e in dataTable3
					where CryptFun.LrDecode(e["sfzhm"]).Equals(CryptFun.LrDecode(sfzhm), StringComparison.OrdinalIgnoreCase) && e["ryxm"] == xm
					select e;

                    if (source.Count() > 1)
                    {
                        IList<IDictionary<string, string>> realDts = new List<IDictionary<string, string>>();
                        foreach (var mrow in source)
                        {
                            var find = from e in dataTable4
					            where e["rybh"] == mrow["rybh"] && CryptFun.LrDecode(e["jskh"]).Equals(orgyhkh, StringComparison.OrdinalIgnoreCase)
					            select e;
                            if (find.Count() > 0)
                            {
                                realDts.Add(mrow);
                                break;
                            }                            
                        }
                        source = realDts;
                    }
                    var findAddWgrys = addWgrys.Where(e => e.Key.Equals(orgsfzhm, StringComparison.OrdinalIgnoreCase));
					string rybh = "";
					if (source.Count<IDictionary<string, string>>() == 0 && findAddWgrys.Count() == 0)
					{
						rybh = Guid.NewGuid().ToString().Replace("-", "").ToLower();
						this.CommonDao.ExecCommand(string.Concat(new string[]
						{
							"insert into i_m_wgry(SSJCJGBH,LXBH,QYBH,RYBH,RYXM,SFZHM,LRRZH,LRRXM,SSDWBH,SSDWMC,LRSJ,SPTG,SFYX,ZH,SJHM) values('','11','','",
							rybh,
							"','",
							xm,
							"','",
							sfzhm,
							"','",
							text,
							"','",
							text2,
							"','','',getdate(),1,1,'','",
							phone,
							"')"
						}), CommandType.Text, null, -1);
                        addWgrys.Add(new KeyValuePair<string, string>(orgsfzhm, rybh));
					}
					else if (source.Count()>0)
						rybh = source.ElementAt(0)["rybh"];
                    else if (findAddWgrys.Count() > 0)
                        rybh = findAddWgrys.ElementAt(0).Value;
					IEnumerable<IDictionary<string, string>> source2 = from e in dataTable4
					where e["rybh"] == rybh && CryptFun.LrDecode(e["jskh"]).Equals(orgyhkh, StringComparison.OrdinalIgnoreCase)
					select e;
                    var findCards = addWgrycards.Where(e => e.Key.Equals(orgsfzhm + orgyhkh, StringComparison.OrdinalIgnoreCase));
					string text15 = "";
					if (source2.Count<IDictionary<string, string>>() == 0 && findCards.Count() == 0)
					{
						text15 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
						bool flag = BankInfo.IsHuaxiaCard(yhkh);
						cmdTxt = string.Concat(new object[]
						{
							"insert into i_s_wgry_yhzh(RECID,RYBH,LRRZH,LRRXM,LRSJ,SPTG,SFYX,YHYHID,SFBH,JSKLX,JSKH,YHPT,MRZH,YHHH,BKZT,CJYHZHZT) values('",
							text15,
							"','",
							rybh,
							"','",
							text,
							"','",
							text2,
							"',getdate(),1,1,'',",
							flag ? 0 : 1,
							",0,'",
							yhkh,
							"','",
							yhpt,
							"',0,'",
							vtransPayWgryReqSetPayRollItem2.banknumber,
							"',0,0)"
						});
						this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
                        addWgrycards.Add(new KeyValuePair<string, string>(orgsfzhm + orgyhkh, text15));
					}
					else if (source2.Count()>0)
						text15 = source2.ElementAt(0)["recid"];
                    else if (findCards.Count() > 0)
                        text15 = findCards.ElementAt(0).Value;

					string text16 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
					cmdTxt = string.Concat(new object[]
					{
						"insert into I_S_PAY_XQ(RECID,RYBH,RYXM,RYKBH,YFJE,SFJE,ZT,CJSJ,FFID,bz) values('",
						text16,
						"','",
						rybh,
						"','",
						xm,
						"','",
						text15,
						"',",
						safeDecimal,
						",0,0,getdate(),'",
						text14,
						"','",
						remark,
						"')"
					});
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					num += safeDecimal.GetSafeDecimal(0m);
				}

				cmdTxt = string.Concat(new object[]
				{
					"insert into I_M_PAY(RECID,GCBH,QYBH,ZFQYBH,FFZH,YFZE,SFZE,FFLX,FFNF,FFYF,ZT,CJSJ,CJRZH,CJRXM,GCMC,QYMC,ZFQYMC,PTLSH,BZ1,BZ2,YHPT) values('",
					text14,
					"','",
					text6,
					"','",
					text8,
					"','",
					text10,
					"','",
					text12,
					"',",
					num,
					",0,'",
					payinfos.paytype,
					"',",
					safeInt,
					",",
					safeInt2,
					",0,getdate(),'",
					text,
					"','",
					text2,
					"','",
					text7,
					"','",
					text9,
					"','",
					text11,
					"','",
					payinfos.paycode,
					"','",
					payinfos.remark1,
					"','",
					payinfos.remark2,
					"','"+yhpt+"')"
				});
				this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
				if (!string.IsNullOrEmpty(payinfos.attach))
				{
					cmdTxt = string.Concat(new string[]
					{
						"insert into I_S_PAY_FJ(RECID,FFID,FJ,ZT,LX,WJHZ) values('",
						Guid.NewGuid().ToString(),
						"','",
						text14,
						"',@fj,0,1,'pdf')"
					});
					IList<IDataParameter> list = new List<IDataParameter>();
					SqlParameter item = new SqlParameter("@fj", SqlDbType.VarBinary)
					{
						Value = Convert.FromBase64String(payinfos.attach)
					};
					list.Add(item);
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, list, -1);
				}
				if (payinfos.pictures != null)
				{
					foreach (string text17 in payinfos.pictures)
					{
						if (!string.IsNullOrEmpty(text17))
						{
							cmdTxt = string.Concat(new string[]
							{
								"insert into I_S_PAY_FJ(RECID,FFID,FJ,ZT,LX,WJHZ) values('",
								Guid.NewGuid().ToString(),
								"','",
								text14,
								"',@fj,0,2,'jpg')"
							});
							IList<IDataParameter> list2 = new List<IDataParameter>();
							SqlParameter item2 = new SqlParameter("@fj", SqlDbType.VarBinary)
							{
								Value = Convert.FromBase64String(text17)
							};
							list2.Add(item2);
							this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, list2, -1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				vtransPayWgryRespBase.message = ex.Message;
				vtransPayWgryRespBase.success = "0001";
				throw ex;
			}
			return vtransPayWgryRespBase;
		}

        /// <summary>
        /// 纠正创建账户失败的条目
        /// </summary>
        [Transaction(ReadOnly =false)]
        public void CorrectErrorCreateCardItems()
        {
            try
            {                
                IList<IDictionary<string, string>> errCounts = CommonDao.GetDataTable("select count(*) as t1 from I_S_WGRY_YHZH where CJYHZHZT=2");
                if (errCounts.Count() > 0 && errCounts[0]["t1"].GetSafeInt() > 0)
                    CommonDao.ExecCommand("update I_S_WGRY_YHZH set CJYHZHZT=0,bkzt=0 where CJYHZHZT=2", CommandType.Text);
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }

        /// <summary>
        /// 纠正提现失败的条目
        /// </summary>
        [Transaction(ReadOnly =false)]
        public void CorrectErrorWithdrawItems()
        {
            try
            {                
                IList<IDictionary<string, string>> errCounts = CommonDao.GetDataTable("select count(*) as t1 from i_m_pay where zt="+PayStatus.PrePayException);
                if (errCounts.Count() > 0 && errCounts[0]["t1"].GetSafeInt() > 0)
                    CommonDao.ExecCommand("update i_m_pay set zt="+PayStatus.Submit+" where zt="+PayStatus.PrePayException, CommandType.Text);
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }

        /// <summary>
        /// 获取待创建的账号列表：q-企业账号,p-工程账号,r-人员账号
        /// </summary>
        /// <returns></returns>        
		public IDictionary<string, IList<IDictionary<string, string>>> GetUnCreateInfos(out string msg)
		{
			msg = "";
			IDictionary<string, IList<IDictionary<string, string>>> dictionary = new Dictionary<string, IList<IDictionary<string, string>>>();
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_s_qy_yhzh where (cjyhzhzt=1 and bkzt=0) or (cjyhzhzt=0 )");
				dictionary.Add("q", dataTable);
				dataTable = this.CommonDao.GetDataTable("select a.*,b.yhhh,b.yhzh,b.yhzhmc,b.khwdmc,b.bkzt,b.cjyhzhzt,b.yhyhid as r_yhyhid,b.LRRZH as r_lrrzh,b.lrrxm as r_lrrxm,b.recid as r_recid,b.yhpt as r_yhpt,b.sfbh,b.jsklx,b.jskh from i_m_wgry a inner join i_s_wgry_yhzh b on a.rybh=b.rybh where (b.cjyhzhzt=1 and b.bkzt=0) or (b.cjyhzhzt=0)");
				dictionary.Add("r", dataTable);
				dataTable = this.CommonDao.GetDataTable("select a.*,b.yhhh as r_yhhh,b.yhzh as r_yhzh,b.yhzhmc as r_yhzhmc,b.khwdmc as r_khwdmc,b.bkzt as r_bkzt,b.cjyhzhzt as r_cjyhzhzt,b.yhyhid as r_yhyhid,b.LRRZH as r_lrrzh,b.lrrxm as r_lrrxm,b.recid as r_recid,c.gcmc,b.JGZH,b.JGHM from I_S_QY_YHZH a inner join I_s_gc_zfdw b on a.qybh=b.zfqybh and a.YHPT=b.YHPT inner join i_m_gc c on b.gcbh=c.gcbh where (b.cjyhzhzt=1 and b.bkzt=0) or (b.cjyhzhzt=0)");
				dictionary.Add("p", dataTable);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return dictionary;
		}
        /// <summary>
        /// 获取未设置的账户信息：q-企业账号,p-工程账号,r-人员账号
        /// </summary>
        /// <returns></returns>
		public IDictionary<string, IList<IDictionary<string, string>>> GetUnSetBankInfos(out string msg)
		{
			msg = "";
			IDictionary<string, IList<IDictionary<string, string>>> dictionary = new Dictionary<string, IList<IDictionary<string, string>>>();
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select *,yhyhid as r_yhyhid from I_S_QY_YHZH where cjyhzhzt=1 and yhzh=''");
				dictionary.Add("q", dataTable);
				dataTable = this.CommonDao.GetDataTable("select a.*,b.yhzh,b.yhzhmc,b.khwdmc,b.bkzt,b.cjyhzhzt,b.yhyhid as r_yhyhid,b.LRRZH as r_lrrzh,b.lrrxm as r_lrrxm,b.recid as r_recid,b.yhpt as r_yhpt,b.sfbh,b.jsklx,b.jskh from i_m_wgry a inner join i_s_wgry_yhzh b on a.rybh=b.rybh where b.cjyhzhzt=1 and b.yhzh=''");
				dictionary.Add("r", dataTable);
				dataTable = this.CommonDao.GetDataTable("select a.*,b.jskh as r_jskh,b.yhpt as r_yhpt,b.yhzh as r_yhzh,b.yhzhmc as r_yhzhmc,b.khwdmc as r_khwdmc,b.bkzt as r_bkzt,b.cjyhzhzt as r_cjyhzhzt,b.yhyhid as r_yhyhid,b.LRRZH as r_lrrzh,b.lrrxm as r_lrrxm,b.recid as r_recid,b.jgzh,b.jghm from I_S_QY_YHZH a inner join I_s_gc_zfdw b on a.qybh=b.zfqybh  and a.YHPT=b.YHPT where b.cjyhzhzt=1 and b.yhzh=''");
				dictionary.Add("p", dataTable);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return dictionary;
		}
        /// <summary>
        /// 设置银行绑卡标识
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetBindCardSign(string lx, string bh, string userid, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
					IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_qy where qybh='" + bh + "'", CommandType.Text, null, -1);
					if (dataTable.Count == 0)
					{
						msg = "找不到企业记录";
						return result;
					}
					IDictionary<string, string> dictionary = dataTable[0];
					if (dictionary["bkzt"] != "-1")
					{
						return true;
					}
					if (string.IsNullOrEmpty(dictionary["qymc"]) || string.IsNullOrEmpty(dictionary["zzjgdm"]) || string.IsNullOrEmpty(dictionary["lxdh"]) || string.IsNullOrEmpty(dictionary["lxsj"]) || string.IsNullOrEmpty(dictionary["yydz"]) || string.IsNullOrEmpty(dictionary["qyfr"]) || string.IsNullOrEmpty(dictionary["qyfzr"]) || string.IsNullOrEmpty(dictionary["jsklx"]) || string.IsNullOrEmpty(dictionary["jskh"]) || string.IsNullOrEmpty(dictionary["yhpt"]) || string.IsNullOrEmpty(dictionary["yhhh"]))
					{
						msg = "企业名称、组织结构代码、企业法人、企业联系人、联系电话、手机号码、联系地址、结算卡类型、结算卡号、银行平台、结算卡银行不能为空";
						return result;
					}
					bool flag = BankInfo.IsHuaxiaCard(dictionary["jskh"]);
					result = this.CommonDao.ExecCommand(string.Concat(new object[]
					{
						"update i_m_qy set bkzt=0,sfbh=",
						flag ? 0 : 1,
						" where qybh='",
						bh,
						"'"
					}), CommandType.Text, null, -1);
				}
				else
				{
					msg = "无效的类型";
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
        /// <summary>
        /// 创建银行虚拟账户标识
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="bh"></param>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetCreateAccountSign(string lx, string bh, string userid, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
					IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_qy where qybh='" + bh + "'", CommandType.Text, null, -1);
					if (dataTable.Count == 0)
					{
						msg = "找不到企业记录";
						return result;
					}
					IDictionary<string, string> dictionary = dataTable[0];
					if (dictionary["cjyhzhzt"] != "-1" && !string.IsNullOrEmpty(dictionary["cjyhzhzt"]))
					{
						return true;
					}
					if (string.IsNullOrEmpty(dictionary["qymc"]) || string.IsNullOrEmpty(dictionary["zzjgdm"]) || string.IsNullOrEmpty(dictionary["lxdh"]) || string.IsNullOrEmpty(dictionary["lxsj"]) || string.IsNullOrEmpty(dictionary["yydz"]) || string.IsNullOrEmpty(dictionary["qyfr"]) || string.IsNullOrEmpty(dictionary["qyfzr"]))
					{
						msg = "企业名称、组织结构代码、企业法人、企业联系人、联系电话、手机号码、联系地址不能为空";
						return result;
					}
					result = this.CommonDao.ExecCommand("update i_m_qy set CJYHZHZT=0 where qybh='" + bh + "'", CommandType.Text, null, -1);
				}
				else
				{
					msg = "无效的类型";
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
        /// <summary>
        /// 设置银行用户创建信息
        /// </summary>
        /// <param name=""></param>
        /// <param name="bh"></param>
        /// <param name="yhyhid"></param>
        /// <param name="xnzh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetAccountCreateInfo(string lx, string bh, VPayCreateBankAccountReturn info, out string msg)
		{
			bool result = true;
			msg = "";
			try
			{
				string text = "";
				if (info.NeedCreateAccount)
				{
					if (info.CreateAccountSucceed)
					{
                        if (string.IsNullOrEmpty(info.AccountUser))
                            return result;
						text = string.Concat(new string[]
						{                            
							text,
							",yhyhid='",
							info.AccountUser,
							"',YHZH='",
							info.AccountCode,
							"',YHZHMC='",
							info.AccountName,
							"',KHWDMC='",
							info.BankPointName,
							"',CJYHZHZT=1"
						});
					}
					else
					{
						text += ",CJYHZHZT=2";
					}
				}
				if (info.NeedBindCard)
				{
					if (info.BindCardSucceed)
					{
						text += ",BKZT=1";
					}
					else
					{
						text += ",BKZT=2";
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					return result;
				}
				text = "kkcwxx='" + info.BankErrorMessage + "'" + text;
				if (lx.Equals("q", StringComparison.OrdinalIgnoreCase))
				{
                    string sql = string.Concat(new string[]
                    {
                        "update i_s_qy_yhzh set ",
                        text,
                        " where recid='",
                        bh,
                        "'"
                    });
					result = this.CommonDao.ExecCommand(sql, CommandType.Text, null, -1);
				}
				else if (lx.Equals("p", StringComparison.OrdinalIgnoreCase))
				{
					result = this.CommonDao.ExecCommand(string.Concat(new string[]
					{
						"update i_s_gc_zfdw set ",
						text,
						" where recid='",
						bh,
						"'"
					}), CommandType.Text, null, -1);
				}
				else if (lx.Equals("r", StringComparison.OrdinalIgnoreCase))
				{
                    string sql = string.Concat(new string[]
                    {
                        "update i_s_wgry_yhzh set ",
                        text,
                        " where recid='",
                        bh,
                        "'"
                    });
					result = this.CommonDao.ExecCommand(sql, CommandType.Text, null, -1);
					if ((info.NeedCreateAccount && !info.CreateAccountSucceed) || (info.NeedBindCard && !info.BindCardSucceed))
					{
						result = this.CommonDao.ExecCommand(string.Concat(new object[]
						{
							"update I_S_PAY_XQ set zt=",
							99,
							",ffbz='",
							info.BankErrorMessage,
							"' where rykbh='",
							bh,
							"'"
						}), CommandType.Text, null, -1);
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
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
		public IList<IDictionary<string, string>> GetPayHistory(string usercode, string zt, string gcmc, string sgdw, string ffzh, string gcbh, string sgdwbh, string bz1, string gzzq1, string gzzq2, string spsj1, string spsj2, int pagesize, int pageindex, out int totalcount, out string msg, out decimal t1, out decimal t2)
		{
			msg = "";
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			totalcount = 0;
            t1 = 0;
            t2 = 0;
			try
			{
                string text = "select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid ) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh  where ";
                string where = " (a.zfqybh=(select qybh from i_m_qyzh where yhzh='" + usercode +
                    "') or a.zfqybh in (select qybh from i_m_ry where rybh in (select qybh from I_M_QYZH where yhzh='" + usercode + "'))) ";
				if (!string.IsNullOrEmpty(gcmc))
				{
					where = where + " and a.gcmc like '%" + gcmc + "%' ";
				}
				if (!string.IsNullOrEmpty(sgdw))
				{
					where = where + " and a.qymc like '%" + sgdw + "%' ";
				}
				if (!string.IsNullOrEmpty(ffzh))
				{
					where = where + " and a.ffzh like '%" + ffzh + "%' ";
				}
				if (!string.IsNullOrEmpty(zt))
				{
					where = where + " and a.zt in (" + zt.FormatSQLInStr() + ")";
				}
				if (!string.IsNullOrEmpty(gcbh))
				{
					where = where + " and a.gcbh='" + gcbh + "' ";
				}
				if (!string.IsNullOrEmpty(sgdwbh))
				{
					where = where + " and a.qybh='" + sgdwbh + "' ";
				}
				if (!string.IsNullOrEmpty(bz1))
				{
					where = where + " and a.bz1 like '%" + bz1 + "%' ";
				}
				if (!string.IsNullOrEmpty(gzzq1))
				{
					string[] array = gzzq1.Split(new char[]
					{
						'-'
					});
					int safeInt = array[0].GetSafeInt(0);
					int safeInt2 = array[1].GetSafeInt(0);
                    where += " and (a.ffnf>" + safeInt + " or a.ffnf=" + safeInt + " and a.ffyf>=" + safeInt2 + ")";
				}
				if (!string.IsNullOrEmpty(gzzq2))
				{
					string[] array2 = gzzq2.Split(new char[]
					{
						'-'
					});
					int safeInt3 = array2[0].GetSafeInt(0);
					int safeInt4 = array2[1].GetSafeInt(0);
                    where += " and (a.ffnf<" + safeInt3 + " or a.ffnf=" + safeInt3 + " and a.ffyf<=" + safeInt4 + ")";
				}
				if (!string.IsNullOrEmpty(spsj1))
				{
					where = where + " and (a.shsj is not null and a.shsj>=convert(datetime,'" + spsj1 + "')) ";
				}
				if (!string.IsNullOrEmpty(spsj2))
				{
					where = where + " and (a.shsj is not null and a.shsj<=convert(datetime,'" + spsj2 + " 23:59:59')) ";
				}
				text += where + " order by a.cjsj desc";
				if (pagesize > 0)
				{
					list = this.CommonDao.GetPageData(text, pagesize, pageindex, out totalcount);
				}
				else
				{
					list = this.CommonDao.GetDataTable(text);
					totalcount = list.Count;
				}
                                
                IList<IDictionary<string,string>> dt1 = CommonDao.GetDataTable("select sum(a.yfze) as s1, sum(a.sfze) as s2 from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where " + where);
                t1 = dt1[0]["s1"].GetSafeDecimal();
                t2 = dt1[0]["s2"].GetSafeDecimal();

                foreach (IDictionary<string, string> dictionary in list)
				{
					if (dictionary["cjsj"] != "")
					{
						dictionary["cjsj"] = dictionary["cjsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["shsj"] != "")
					{
						dictionary["shsj"] = dictionary["shsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					else if (dictionary["shsj0"] != "")
					{
						dictionary["shsj"] = dictionary["shsj0"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["ffsj"] != "")
					{
						dictionary["ffsj"] = dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					dictionary["ztms"] = PayStatus.GetDesc(dictionary["zt"].GetSafeInt(0));
                    dictionary["zhye"] = "";
				}

                List<string> yhpts = new List<string>();
                foreach (IDictionary<string,string> row in list)
                {
                    var findYhpts = yhpts.Where(e => e.Equals(row["yhpt"], StringComparison.OrdinalIgnoreCase));
                    if (findYhpts.Count() == 0)
                        yhpts.Add(row["yhpt"]);
                }
                foreach (string yhpt in yhpts)
                {
                    List<string> list2 = new List<string>();
                    foreach (IDictionary<string, string> dictionary in list)
                    {
                        if (dictionary["yhpt"].Equals(yhpt, StringComparison.OrdinalIgnoreCase))
                            list2.Add(dictionary["ffzh"]);
                    }
                    CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
                    cl_GetUserInfos.UserIds.AddRange(list2);
                    if (this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg))
                    {
                        VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
                        if (vtransPayRespGetUserInfo.IsSucceed)
                        {
                            msg = "";
                            using (IEnumerator<IDictionary<string, string>> enumerator = list.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    
                                    IDictionary<string, string> row = enumerator.Current;
                                    var finds = from e in vtransPayRespGetUserInfo.data
                                                where e.UserId.Equals(row["ffzh"])
                                                select e;

                                    VTransPayRespGetUserInfoDataItem vtransPayRespGetUserInfoDataItem = null;
                                    if (finds.Count() > 0)
                                        vtransPayRespGetUserInfoDataItem = finds.First();
                                    if (vtransPayRespGetUserInfoDataItem != null)
                                    {
                                        row["zhye"] = vtransPayRespGetUserInfoDataItem.AccountBalance.ToString();
                                    }
                                }
                            }
                        }
                        //msg = vtransPayRespGetUserInfo.message;
                    }
                }
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
        /// <summary>
        /// 获取待审批的工资册
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetUncheckPays(string gcbh, string sgdwbh, string usercode, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select qybh,rybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "')");
				if (dataTable.Count == 0)
				{
					msg = "当前不是财务人员账号";
					return ret;
				}
				string qybh = dataTable[0]["qybh"].GetSafeString("");
				string rybh = dataTable[0]["rybh"].GetSafeString("");
                StringBuilder sbWhere = new StringBuilder();
                if (!string.IsNullOrEmpty(gcbh))
                    sbWhere.Append(" and a.gcbh='" + gcbh + "' ");
                if (!string.IsNullOrEmpty(sgdwbh))
                    sbWhere.Append(" and a.qybh='" + sgdwbh + "' ");
                string sql = "select * from ((select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where a.zfqybh='" + qybh  + "' "+ sbWhere.ToString() +
                    " and zt=0 and (exists (select * from I_S_QY_RY x where x.qybh=a.zfqybh and x.rybh='" + rybh + "' and x.gw='2'))) " +
                  " union all (select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where a.zfqybh='" + qybh + "' " + sbWhere.ToString() +
                    " and zt=2 and (exists (select * from I_S_QY_RY x where x.qybh=a.zfqybh and x.rybh='" + rybh + "' and x.gw='3')))) as t1 order by cjsj desc";
                /*
                string sql = "(select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where a.zfqybh='" + qybh  + "' "+ sbWhere.ToString() +
                    " and zt=0 and (exists (select * from I_S_GC_CWRY x where x.gcbh=a.gcbh and x.sgqybh=a.qybh and x.zfqybh=a.zfqybh and x.rybh='" + rybh + "' and x.lx='1') or not exists (select * from I_S_GC_CWRY x where x.gcbh=a.gcbh and x.sgqybh=a.qybh and x.zfqybh=a.zfqybh and x.lx='1'))) " +
                  " union all (select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where a.zfqybh='" + qybh + "' " + sbWhere.ToString() +
                    " and zt=2 and (exists (select * from I_S_GC_CWRY x where x.gcbh=a.gcbh and x.sgqybh=a.qybh and x.zfqybh=a.zfqybh and x.rybh='" + rybh + "'and x.lx='2') or not exists (select * from I_S_GC_CWRY x where x.gcbh=a.gcbh and x.sgqybh=a.qybh and x.zfqybh=a.zfqybh and x.lx='2')))";
				*/
				ret = this.CommonDao.GetDataTable(sql);
                List<string> yhpts = new List<string>();
				foreach (IDictionary<string, string> dictionary in ret)
				{
					if (dictionary["cjsj"] != "")
					{
						dictionary["cjsj"] = dictionary["cjsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["shsj"] != "")
					{
						dictionary["shsj"] = dictionary["shsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["ffsj"] != "")
					{
						dictionary["ffsj"] = dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					dictionary["ztms"] = PayStatus.GetDesc(dictionary["zt"].GetSafeInt(0));

                    var findYhpts = yhpts.Where(e => e.Equals(dictionary["yhpt"], StringComparison.OrdinalIgnoreCase));
                    if (findYhpts.Count() == 0)
                        yhpts.Add(dictionary["yhpt"]);
				}
                foreach (string yhpt in yhpts)
                {
                    List<string> list2 = new List<string>();
                    foreach (IDictionary<string, string> dictionary in ret)
                    {
                        if (dictionary["yhpt"].Equals(yhpt, StringComparison.OrdinalIgnoreCase))
                            list2.Add(dictionary["ffzh"]);
                    }
                    CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
                    cl_GetUserInfos.UserIds.AddRange(list2);
                    if (this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg))
                    {
                        VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
                        if (vtransPayRespGetUserInfo.IsSucceed)
                        {
                            msg = "";
                            using (IEnumerator<IDictionary<string, string>> enumerator = ret.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    IDictionary<string, string> row = enumerator.Current;
                                    var finds = from e in vtransPayRespGetUserInfo.data
                                                where e.UserId.Equals(row["ffzh"])
                                                select e;

                                    VTransPayRespGetUserInfoDataItem vtransPayRespGetUserInfoDataItem = null;
                                    if (finds.Count() > 0)
                                        vtransPayRespGetUserInfoDataItem = finds.First();

                                    if (vtransPayRespGetUserInfoDataItem == null)
                                    {
                                        row.Add("zhye", "");
                                    }
                                    else
                                    {
                                        row.Add("zhye", vtransPayRespGetUserInfoDataItem.AccountBalance.ToString());
                                    }
                                }
                            }
                        }
                        //msg = vtransPayRespGetUserInfo.message;
                    }
                }
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return ret;
		}
        /// <summary>
        /// 获取发放详情
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetPayDetail(string recid, string ffwc, string xm, int pagesize, int pageindex, out int totalcount, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			totalcount = 0;
			try
			{
				string text = "";
				if (recid != "")
				{
					text = text + " and b.ffid='" + recid + "' ";
				}
				if (ffwc == "1")
				{
					text += " and b.sfje>=b.yfje ";
				}
				if (!string.IsNullOrEmpty(xm))
				{
					text = text + " and b.ryxm like '%" + xm + "%' ";
				}
				string sql = "select b.*,a.YHYHID,a.YHZH,a.JSKH,a.KKCWXX,c.sfzhm,c.sjhm,a.bkzt,a.cjyhzhzt from I_S_PAY_XQ b inner join I_S_WGRY_YHZH a inner join i_m_wgry c on c.rybh=a.rybh on a.recid=b.RYKBH where 1=1 " + text + " order by b.rowid asc ";
				list = this.CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
				foreach (IDictionary<string, string> dictionary in list)
				{
					if (dictionary["cjsj"] != "")
					{
						dictionary["cjsj"] = dictionary["cjsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["ffsj"] != "")
					{
						dictionary["ffsj"] = dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
                        
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}

        /// <summary>
        /// 设置发放状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetPayStatus(string recid, int status, string usercode, string realname, string tradecode, out string msg, string totalmoney = "", string payinfo = "")
		{
			bool result = true;
			msg = "";
			try
			{
				string text = PayStatus.GetPreStatus(status);
				if (!string.IsNullOrEmpty(text))
				{
					text = " and zt in (" + text + ") ";
				}
				string text2 = "zt=" + status;
				if (!string.IsNullOrEmpty(usercode))
				{
					text2 = string.Concat(new string[]
					{
						text2,
						",shsj=getdate(),shrzh='",
						usercode,
						"',shrxm='",
						realname,
						"'"
					});
				}
				if (!string.IsNullOrEmpty(tradecode))
				{
					text2 = text2 + ",tradecode='" + tradecode + "'";
				}
				if (!string.IsNullOrEmpty(totalmoney))
				{
					text2 = text2 + ",sfze=" + totalmoney.GetSafeDecimal(0m);
				}
				if (!string.IsNullOrEmpty(payinfo))
				{
					text2 = text2 + ",ffbz1='" + payinfo + "'";
				}
				else if (status == 3)
				{
					text2 += ",ffbz1=''";
				}
				result = this.CommonDao.ExecCommand(string.Concat(new string[]
				{
					"update i_m_pay set ",
					text2,
					" where recid='",
					recid,
					"' ",
					text
				}), CommandType.Text, null, -1);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
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
		[Transaction(ReadOnly = false)]
		public bool CheckPay(string recid, bool agree, string usercode, string realname, ref string msg)
		{
			bool result = false;
			try
			{
				string text = msg;
				msg = "";
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + recid + "'");
				if (dataTable.Count == 0)
				{
					msg = "无效的工资册";
					return result;
				}
				IDictionary<string, string> dictionary = dataTable[0];
				int safeInt = dictionary["zt"].GetSafeInt(0);
				if (safeInt != 0 && safeInt != 2)
				{
					msg = "工资册不需要审批";
					return result;
				}
				string rylx;
				if (safeInt == 0)
				{
					rylx = "2";
				}
				else
				{
					rylx = "3";
				}

				dataTable = this.CommonDao.GetDataTable("select b.yhzh from I_S_QY_RY a inner join i_m_qyzh b on a.rybh=b.qybh where a.gw='"+rylx+"' and a.qybh='"+dictionary["zfqybh"]+"'");
				IEnumerable<IDictionary<string, string>> source = from e in dataTable
				where e["yhzh"].Equals(usercode)
				select e;
				if (dataTable.Count<IDictionary<string, string>>() > 0 && source.Count<IDictionary<string, string>>() == 0)
				{
					msg = "您没有工资册的审批权限";
					return result;
				}
				string text3;
				if (safeInt == 0)
				{
					text3 = string.Concat(new object[]
					{
						"zt=",
						agree ? 1 : -1,
						",shsj0=getdate(),shrzh0='",
						usercode,
						"',shrxm0='",
						realname,
						"'"
					});
				}
				else
				{
					text3 = string.Concat(new object[]
					{
						"zt=",
						agree ? 5 : -1,
						",shsj=getdate(),shrzh='",
						usercode,
						"',shrxm='",
						realname,
						"'"
					});
				}
				if (!string.IsNullOrEmpty(text))
				{
					text3 = text3 + ",ffbz1='" + text + "'";
				}
                string sql = "update i_m_pay set " + text3 + " where recid='" + recid + "' ";

				result = this.CommonDao.ExecCommand(sql, CommandType.Text, null, -1);
                SysLog4.WriteLog("====CheckPay    payid:" + recid + ",sql:" + sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
        /// <summary>
        /// 获取正在发放的列表
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetPayingList(int status, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				string sql = "select * from i_m_pay where zt=" + status + " order by shsj asc ";
				result = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 获取需要预提现的列表
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetNeedPrePayList(out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				string sql = "select * from i_m_pay where zt=" + PayStatus.Submit + " and not exists (select x.recid from I_S_PAY_XQ x INNER JOIN I_S_WGRY_YHZH y on x.rykbh=y.recid where (y.cjyhzhzt=0 or y.bkzt=0) and x.ffid=i_m_pay.recid) order by shsj asc ";
				result = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        
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
		public bool PayToWorker(string fromzhid, string tozhid, decimal money, string usercode, string realname, string bz, string yhpt, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				CL_Pay info = new CL_Pay
				{
					Amount = money,
					FromUserId = fromzhid,
					ToUserId = tozhid,
					TradeComment = bz
				};
				flag = this.Pay(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespPay vtransPayRespPay = this.ParseResponse<VTransPayRespPay>(msg);
					if (vtransPayRespPay.IsSucceed)
					{
						msg = "";
						flag = true;
					}
					else
					{
						flag = false;
						msg = vtransPayRespPay.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return flag;
		}
        /// <summary>
        /// 回写发放结果
        /// </summary>
        /// <param name="ffid"></param>
        /// <param name="xqid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetPayResult(string ffid, string xqid, decimal ffje, string bz, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string cmdTxt = string.Concat(new object[]
				{
					"update I_S_PAY_XQ set sfje=sfje+",
					ffje,
					", ffbz='",
					bz,
					"',ffsj=getdate() where recid='",
					xqid,
					"'"
				});
				this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
				if (ffje > 0m)
				{
					cmdTxt = string.Concat(new object[]
					{
						"update i_m_pay set sfze=sfze+",
						ffje,
						" where recid='",
						ffid,
						"'"
					});
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					cmdTxt = "update i_m_pay set zt=2 where recid='" + ffid + "' and yfze=sfze";
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
        /// <summary>
        /// 获取账号关联的工程
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetRelateProjects(string usercode, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				string sql = string.Concat(new string[]
				{
					"select gcbh,gcmc from i_m_gc where gcbh in (select gcbh from I_S_GC_ZFDW where zfqybh in (select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or zfqybh in (select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"'))) "
				});
				result = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取账号关联的施工单位
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetRelateCompanys(string usercode, string gcbh, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				string text = "";
				if (!string.IsNullOrEmpty(gcbh))
				{
					text = " and gcbh='" + gcbh + "' ";
				}
				string sql = string.Concat(new string[]
				{
					"select qybh,qymc from i_m_qy where qybh in (select qybh from I_S_GC_ZFDW where (zfqybh in (select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or zfqybh in (select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"' ))) ",
					text,
					")"
				});
				result = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 设置人员卡号无效
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetWgryInvalid(string id, string usercode, string realname, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string cmdTxt = string.Concat(new string[]
				{
					"update I_S_WGRY_YHZH set sfyx=0,lrrxm='",
					realname,
					"',lrrzh='",
					usercode,
					"',lrsj=getdate() where (yhyhid is null or yhyhid='') and recid='",
					id,
					"'"
				});
				result = this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
        /// <summary>
        /// 录用其他单位职员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool AddOtherEmployee(string usercode, string rybh, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select qybh from i_m_qy where qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"'))"
				}));
				string text = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					text = dataTable[0]["qybh"];
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "企业编号为空";
					return result;
				}
				dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from I_S_QY_RY where rybh='",
					rybh,
					"' and (qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"')))"
				}));
				if (dataTable.Count > 0)
				{
					return true;
				}
				string cmdTxt = string.Concat(new string[]
				{
					"insert into I_S_QY_RY(rybh,qybh) values('",
					rybh,
					"','",
					text,
					"')"
				});
				result = this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
        /// <summary>
        /// 删除其他单位职员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool DeleteOtherEmployee(string usercode, string rybh, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select qybh from i_m_qy where qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"'))"
				}));
				string text = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					text = dataTable[0]["qybh"];
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "企业编号为空";
					return result;
				}
				string cmdTxt = string.Concat(new string[]
				{
					"delete from I_S_QY_RY where rybh='",
					rybh,
					"' and qybh='",
					text,
					"'"
				});
				result = this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
        /// <summary>
        /// 支付接口发送验证码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetBankVerifyCode(string usercode, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select yhyhid,yhpt from i_s_qy_yhzh where qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"'))"
				}));
				string text = "";
				string yhpt = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					text = dataTable[0]["yhyhid"];
					yhpt = dataTable[0]["yhpt"];
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "找不到企业记录";
					return flag;
				}
				CL_GetChangePhoneCode info = new CL_GetChangePhoneCode
				{
					UserId = text
				};
				flag = this.GetChangePhoneCode(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespGetChangePhoneCode vtransPayRespGetChangePhoneCode = this.ParseResponse<VTransPayRespGetChangePhoneCode>(msg);
					if (vtransPayRespGetChangePhoneCode.IsSucceed)
					{
						msg = vtransPayRespGetChangePhoneCode.data.CodeId;
						flag = true;
					}
					else
					{
						flag = false;
						msg = vtransPayRespGetChangePhoneCode.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return flag;
		}
        /// <summary>
        /// 支付接口设置手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetBankPhone(string usercode, string verifycode, string codeid, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select yhyhid,yhpt,lxsj,qybh from i_s_qy_yhzh where qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"'))"
				}));
				string value = "";
				string yhpt = "";
				string phone = "";
				string text = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					value = dataTable[0]["yhyhid"];
					yhpt = dataTable[0]["yhpt"];
					phone = dataTable[0]["lxsj"];
					text = dataTable[0]["qybh"];
				}
				if (string.IsNullOrEmpty(value))
				{
					msg = "找不到企业记录";
					return flag;
				}
				dataTable = this.CommonDao.GetDataTable("select sjhm from i_m_ry where lxbh='13' and (qybh='" + text + "')");
				List<string> list = new List<string>();
				foreach (IDictionary<string, string> dictionary in dataTable)
				{
					if (!string.IsNullOrEmpty(dictionary["sjhm"]))
					{
						list.Add(dictionary["sjhm"]);
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "企业联系人手机号码不能为空";
					return flag;
				}
				CL_ChangePhone info = new CL_ChangePhone
				{
					CodeId = codeid,
					VerifyCode = verifycode,
					Phone = phone,
					UserPhones = list
				};
				flag = this.ChangePhone(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespBase vtransPayRespBase = this.ParseResponse<VTransPayRespBase>(msg);
					if (vtransPayRespBase.IsSucceed)
					{
						flag = true;
					}
					else
					{
						flag = false;
						msg = vtransPayRespBase.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return flag;
		}
        /// <summary>
        /// 设置银行平台预发放
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool SetBankPrePay(string payid, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				string sql = "select b.yhyhid,c.yhpt,a.qybh,c.cjrzh,cjrxm,c.ffnf,c.ffyf from i_m_qy a inner join I_S_GC_ZFDW b on a.qybh=b.ZFQYBH inner join i_m_pay c on b.yhyhid=c.ffzh where c.recid='" + payid + "' ";
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(sql);
				string text = "";
				string yhpt = "";
				string userid = "";
                string ffny = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					text = dataTable[0]["yhyhid"];
					yhpt = dataTable[0]["yhpt"];
					string text2 = dataTable[0]["qybh"];
					userid = dataTable[0]["cjrzh"];
					string text3 = dataTable[0]["cjrxm"];
                    ffny = dataTable[0]["ffnf"].GetSafeInt().ToString("0000") + dataTable[0]["ffyf"].GetSafeInt().ToString("00");
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "在                                                                     ";
					return flag;
				}
				CL_PrePay cl_PrePay = new CL_PrePay
				{
					FromUserId = text
				};
				dataTable = this.CommonDao.GetDataTable("select a.*,b.yhyhid,b.yhhh,b.jskh,c.sjhm,c.sfzhm from i_s_pay_xq a inner join I_S_WGRY_YHZH b on a.rykbh=b.recid inner join i_m_wgry c on a.rybh=c.rybh where a.ffid='" + payid + "' ");
				foreach (IDictionary<string, string> dictionary in dataTable)
				{
					CL_PreSubPay cl_PreSubPay = new CL_PreSubPay();
					cl_PreSubPay.Amount = dictionary["yfje"].GetSafeDecimal(0m);
					cl_PreSubPay.ToUserId = dictionary["yhyhid"];
                    if (yhpt.Equals("HXB", StringComparison.OrdinalIgnoreCase))
                        cl_PreSubPay.TradeComment = CryptFun.LrDecode(dictionary["jskh"]);// dictionary["bz"];
                    else if (yhpt.Equals("ABC", StringComparison.OrdinalIgnoreCase))
                        cl_PreSubPay.TradeComment = ffny;
					cl_PreSubPay.PayBankCode = dictionary["yhhh"];
					cl_PreSubPay.UserName = dictionary["ryxm"];
					cl_PreSubPay.UserPhone = dictionary["sjhm"];
					cl_PreSubPay.SettleAccount = dictionary["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
					cl_PreSubPay.PapersCode = dictionary["sfzhm"].DecodeDes("8zzsjd95", "fcb95eze");
					if (string.IsNullOrEmpty(cl_PreSubPay.TradeComment))
					{
						cl_PreSubPay.TradeComment = "工资发放";
					}
					cl_PrePay.preSubPays.Add(cl_PreSubPay);
				}
				flag = this.PrePay(cl_PrePay, userid, yhpt, out msg);
				if (flag)
				{
					VTransPayRespPrePay vtransPayRespPrePay = this.ParseResponse<VTransPayRespPrePay>(msg);
					if (vtransPayRespPrePay.IsSucceed)
					{
						flag = true;
						msg = vtransPayRespPrePay.data.TradeCode;
					}
					else
					{
						flag = false;
						msg = vtransPayRespPrePay.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return flag;
		}
        /// <summary>
        /// 获取银行平台支付详情
        /// </summary>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetBankPayDetail(string payid, string usercode, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				string sql = "select b.yhyhid,c.yhpt,a.qybh,c.cjrzh,cjrxm from i_m_qy a inner join I_S_GC_ZFDW b on a.qybh=b.ZFQYBH inner join i_m_pay c on b.yhyhid=c.ffzh where c.recid='" + payid + "' ";
				IList<IDictionary<string, string>> dt = this.CommonDao.GetDataTable(sql);
				string value = "";
				string yhpt = "";
				string text = "";
				if (dt.Count<IDictionary<string, string>>() > 0)
				{
					value = dt[0]["yhyhid"];
					yhpt = dt[0]["yhpt"];
					string text2 = dt[0]["qybh"];
					text = dt[0]["cjrzh"];
					string text3 = dt[0]["cjrxm"];
				}
				if (string.IsNullOrEmpty(value))
				{
					msg = "找不到企业记录";
					return list;
				}
				if (string.IsNullOrEmpty(usercode))
				{
					usercode = text;
				}
				dt = this.CommonDao.GetDataTable("select tradecode from i_m_pay where recid='" + payid + "'");
				if (dt.Count == 0)
				{
					msg = "找不到发放记录";
					return list;
				}
				string text4 = dt[0]["tradecode"];
				if (string.IsNullOrEmpty(text4))
				{
					dt = this.CommonDao.GetDataTable("select b.*,a.ryxm,a.yfje,a.sfje,a.zt,a.bz as r_bz,c.SJHM,c.SFZHM,a.recid as r_recid,a.ffsj,a.ffbz from I_S_PAY_XQ a inner join I_S_WGRY_YHZH b on a.rykbh=b.RECID inner join I_M_WGRY c on b.RYBH=c.rybh where a.ffid='" + payid + "' order by a.rowid");
					foreach (IDictionary<string, string> localData2 in dt)
					{
						list.Add(this.GetResponsePayDetailItem(null, localData2));
					}
					return list;
				}
				CL_GetPayList info = new CL_GetPayList
				{
					TradeCode = text4
				};
				if (this.GetPayList(info, usercode, yhpt, out msg))
				{
					VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
					if (vtransPayRespGetPayList.IsSucceed)
					{
						msg = "";
						dt = this.CommonDao.GetDataTable("select b.*,a.ryxm,a.yfje,a.sfje,a.zt,a.bz as r_bz,c.SJHM,c.SFZHM,a.recid as r_recid,a.ffsj,a.ffbz from I_S_PAY_XQ a inner join I_S_WGRY_YHZH b on a.rykbh=b.RECID inner join I_M_WGRY c on b.RYBH=c.rybh where a.ffid='" + payid + "' order by a.rowid ");
						using (IEnumerator<IDictionary<string, string>> enumerator = dt.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								IDictionary<string, string> localData = enumerator.Current;
								IEnumerable<VTransPayRespGetPayListDataItem> source = from e in vtransPayRespGetPayList.data
								where e.ToUserId.Equals(localData["yhyhid"]) && e.Amount.GetSafeDecimal(0m) == localData["yfje"].GetSafeDecimal(0m)
								select e;
								VTransPayRespGetPayListDataItem bankData = null;
								if (source.Count<VTransPayRespGetPayListDataItem>() > 0)
								{
									bankData = source.ElementAt(0);
								}
								IDictionary<string, string> responsePayDetailItem = this.GetResponsePayDetailItem(bankData, localData);
								list.Add(responsePayDetailItem);
							}
						}
						using (IEnumerator<VTransPayRespGetPayListDataItem> enumerator2 = (from e in vtransPayRespGetPayList.data
						where (from p in dt
						where p["yhyhid"].Equals(e.ToUserId) && p["yfje"].GetSafeDecimal(0m) == e.Amount.GetSafeDecimal(0m)
						select p).Count<IDictionary<string, string>>() == 0
						select e).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								VTransPayRespGetPayListDataItem bankData2 = enumerator2.Current;
								list.Add(this.GetResponsePayDetailItem(bankData2, null));
							}
							goto IL_2F9;
						}
					}
					msg = vtransPayRespGetPayList.message;
				}
				IL_2F9:;
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return list;
		}
        /// <summary>
        /// 支付接口提现验证码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool SetBankPayCode(string payid, string usercode, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select yhpt,tradecode from  i_m_pay where recid='" + payid + "' ");
				string yhpt = "";
				string text = "";
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "支付企业信息不存在";
					return flag;
				}
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					yhpt = dataTable[0]["yhpt"];
					text = dataTable[0]["tradecode"];
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "银行平台交易码未获取";
					return flag;
				}
				dataTable = this.CommonDao.GetDataTable("select sjhm from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "')");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "当前用户无效";
					return flag;
				}
				string text2 = dataTable[0]["sjhm"];
				if (string.IsNullOrEmpty(text2))
				{
					msg = "当前用户手机号码为空";
					return flag;
				}
				CL_GetWithDrawCode info = new CL_GetWithDrawCode
				{
					Phone = text2,
					TradeCode = text
				};
				flag = this.GetWithDrawCode(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespGetWithDrawCode vtransPayRespGetWithDrawCode = this.ParseResponse<VTransPayRespGetWithDrawCode>(msg);
					if (vtransPayRespGetWithDrawCode.IsSucceed)
					{
						flag = true;
						msg = vtransPayRespGetWithDrawCode.data.CodeId;
					}
					else
					{
						flag = false;
						msg = vtransPayRespGetWithDrawCode.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return flag;
		} 
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool SetBankPay(string payid, string codeid, string verifycode, string usercode, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select yhpt,tradecode from i_m_pay where recid='" + payid + "' ");
				string yhpt = "";
				string text = "";
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "支付企业信息不存在";
					return flag;
				}
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					yhpt = dataTable[0]["yhpt"];
					text = dataTable[0]["tradecode"];
				}
				if (string.IsNullOrEmpty(text))
				{
					msg = "银行平台交易码未获取";
					return flag;
				}
				CL_WithDraw info = new CL_WithDraw
				{
					CodeId = codeid,
					TradeCode = text,
					VerifyCode = verifycode
				};
				flag = this.WithDraw(info, usercode, yhpt, out msg);
				if (flag)
				{
					VTransPayRespWithDraw vtransPayRespWithDraw = this.ParseResponse<VTransPayRespWithDraw>(msg);
					if (vtransPayRespWithDraw.IsSucceed)
					{
						flag = true;
					}
					else
					{
						flag = false;
						msg = vtransPayRespWithDraw.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return flag;
		}
        /// <summary>
        /// 设置发放详情表内容
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetPayDetail(IList<IDictionary<string, string>> infos, out string msg)
		{
			bool result = true;
			msg = "";
			try
			{
				var q = from e in infos where e["haslocaldata"] == "1" select e;
				foreach (IDictionary<string, string> dictionary in q)
				{
					string cmdTxt = string.Format("update I_S_PAY_XQ set sfje={0},zt={1},ffsj=convert(datetime,'{2}'),ffbz='{3}' where recid='{4}' and zt in ({5})", new object[]
					{
						dictionary["sfje"].GetSafeDecimal(0m),
						dictionary["zt"].GetSafeInt(0),
						dictionary["ffsj"],
						dictionary["cwdm"],
						dictionary["id"],
						BankPayDetialStaus.GetFinishingString()
					});
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
				}
			}
			catch (Exception ex)
			{
				result = false;
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}
        /// <summary>
        /// 获取当前账号对应的企业列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetUserCompanys(string usercode, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				string sql = string.Concat(new string[]
				{
					"select qybh,qymc from i_m_qy where (qybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') or qybh in (select qybh from i_m_ry where rybh in (select qybh from I_M_QYZH where yhzh='",
					usercode,
					"')) ) "
				});
				result = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 务工人员系统设置发放结果
        /// </summary>
        /// <returns></returns>
		public bool SetWgryPayResult(string payid, string key, out string msg)
		{
			bool flag = true;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select a.*,b.gcbh_yc,b.szsf,b.szcs,b.szqy,(select qybh_yc from i_m_qy where i_m_qy.qybh=a.qybh) as qybh_yc,(select qybh_yc from i_m_qy where i_m_qy.qybh=a.zfqybh) as zfqybh_yc from i_m_pay a left outer join view_i_m_gc b on a.gcbh=b.gcbh where a.recid='" + payid + "'");
				IList<IDictionary<string, string>> bankPayDetail = this.GetBankPayDetail(payid, "", out msg);
				IDictionary<string, string> dictionary = dataTable[0];
				string text = dictionary["ffbz1"];
				if (string.IsNullOrEmpty(text))
				{
					text = PayStatus.GetDesc(dictionary["zt"].GetSafeInt(0));
				}
                VTransPayWgryReqSetPayRollResult vtransPayWgryReqSetPayRollResult = new VTransPayWgryReqSetPayRollResult
                {
                    key = MD5Util.StringToMD5Hash(key, true),
                    Paycode = dictionary["ptlsh"],
                    Shouldpay = dictionary["yfze"],
                    Realpay = dictionary["sfze"],
                    Message = text,
                    Code = dictionary["zt"],
                    PayYear = dictionary["ffnf"].GetSafeInt(),
                    PayMonth = dictionary["ffyf"].GetSafeInt(),
                    CompanyCodePay = dictionary["zfqybh_yc"],
                    CompanyCodeSg = dictionary["qybh_yc"],
                    ProjectCode = dictionary["gcbh_yc"],
                    SerialCode = dictionary["recid"],
                    PayProjectCode = dictionary["gcbh"],
                    PayProjectName = dictionary["gcmc"],
                    Province = dictionary["szsf"],
                    City = dictionary["szcs"],
                    Area = dictionary["szqy"],
                    rows = new List<VTransPayWgryReqSetPayRollResultItem>()
                };
				foreach (IDictionary<string, string> dictionary2 in bankPayDetail)
				{
					VTransPayWgryReqSetPayRollResultItem vtransPayWgryReqSetPayRollResultItem = new VTransPayWgryReqSetPayRollResultItem
					{
						Name = dictionary2["ryxm"],
						Phone = dictionary2["sjhm"],
						IdNumber = HttpUtility.UrlEncode(CryptFun.LrEncode(dictionary2["sfzhm"])),
						CardNumber = HttpUtility.UrlEncode(CryptFun.LrEncode(dictionary2["jskh"])),
						Shouldpay = dictionary2["yfje"],
						Realpay = dictionary2["sfje"],
						Code = dictionary2["zt"],
						message = ""
					};
					string message = VTransPayRespGetPayListDataItem.GetFaildDetail(dictionary2["cwdm"]);
					if (vtransPayWgryReqSetPayRollResultItem.Code == 99.ToString())
					{
						message = BankPayDetialStaus.GetDesc(dictionary2["zt"].GetSafeInt(0));
					}
					vtransPayWgryReqSetPayRollResultItem.message = message;
					vtransPayWgryReqSetPayRollResult.rows.Add(vtransPayWgryReqSetPayRollResultItem);
				}
				flag = MyHttp.Post(Configs.WgryPlatformUrl + "/wzwgryfun/SetPayrollResult", "json=" + new JavaScriptSerializer().Serialize(vtransPayWgryReqSetPayRollResult), out msg);
				if (flag)
				{
					flag = new JavaScriptSerializer().Deserialize<VTransPayWgryRespBase>(msg).IsSuccess;
				}
			}
			catch (Exception ex)
			{
				flag = false;
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return flag;
		}
        /// <summary>
        /// 获取还未推送消息给务工人员得支付
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetUnMessagePay(out string msg)
		{
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
                result = this.CommonDao.GetDataTable("select recid from i_m_pay where zt in (" + PayStatus.GetFinishString() + ") and (wgryxx is null or wgryxx=0) and (ptlsh is not null and ptlsh<>'' or zt in ("+PayStatus.GetImportResultString()+"))");
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 设置已通知给务工人员
        /// </summary>
        /// <param name="payid"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SetWgryMessageSend(string payid)
		{
			bool result = false;
			try
			{
				result = this.CommonDao.ExecCommand("update i_m_pay set wgryxx=1 where recid='" + payid + "'", CommandType.Text, null, -1);
			}
			catch (Exception se)
			{
				SysLog4.WriteLog(se);
			}
			return result;
		}
        /// <summary>
        /// 支付是否又绑卡错误
        /// </summary>
        /// <param name="payid"></param>
        /// <returns></returns>
		public bool PayHasBindcardError(string payid)
		{
			bool result = false;
			try
			{
				result = (this.CommonDao.GetDataTable(string.Concat(new object[]
				{
					"select count(*) as t1 from I_S_PAY_XQ where ffid='",
					payid,
					"' and zt=",
					99
				}))[0]["t1"].GetSafeInt(0) > 0);
			}
			catch (Exception se)
			{
				SysLog4.WriteLog(se);
			}
			return result;
		}
        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public VTransPayWgryRespBase SaveWgryInfo(VTransPayWgryReqBindCardMain info, out IDictionary<string, string> createInfos)
		{
			VTransPayWgryRespBase vtransPayWgryRespBase = new VTransPayWgryRespBase
			{
				success = "0000",
				message = ""
			};
			createInfos = new Dictionary<string, string>();
			string text = "interface";
			string text2 = "接口导入";
			try
			{
				if (string.IsNullOrEmpty(info.name) || string.IsNullOrEmpty(info.phone) || string.IsNullOrEmpty(info.idnumber) || string.IsNullOrEmpty(info.cardnumber))
				{
					vtransPayWgryRespBase.success = "0003";
					return vtransPayWgryRespBase;
				}
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select bkzt,kkcwxx from View_I_M_WGRY_PAY where ryxm='",
					info.name.GetSafeRequest(),
					"' and sfzhm='",
					info.idnumber.GetSafeRequest(),
					"' and jskh='",
					info.cardnumber.GetSafeRequest(),
					"' and sjhm='",
					info.phone.GetSafeRequest(),
					"'"
				}));
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					int safeInt = dataTable[0]["bkzt"].GetSafeInt(0);
					if (safeInt == 1)
					{
						vtransPayWgryRespBase.success = "0007";
						return vtransPayWgryRespBase;
					}
					if (safeInt == 0)
					{
						vtransPayWgryRespBase.success = "0005";
						return vtransPayWgryRespBase;
					}
					vtransPayWgryRespBase.success = "0006";
					vtransPayWgryRespBase.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespBase.success, "") + "：" + dataTable[0]["kkcwxx"];
					return vtransPayWgryRespBase;
				}
				else
				{
					string text3 = info.bankplatform;
					string text4 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
					this.CommonDao.ExecCommand(string.Concat(new string[]
					{
						"insert into i_m_wgry(SSJCJGBH,LXBH,QYBH,RYBH,RYXM,SFZHM,LRRZH,LRRXM,SSDWBH,SSDWMC,LRSJ,SPTG,SFYX,ZH,SJHM) values('','11','','",
						text4,
						"','",
						info.name.GetSafeRequest(),
						"','",
						info.idnumber.GetSafeRequest(),
						"','",
						text,
						"','",
						text2,
						"','','',getdate(),1,1,'','",
						info.phone.GetSafeRequest(),
						"')"
					}), CommandType.Text, null, -1);
					string text5 = Guid.NewGuid().ToString().Replace("-", "").ToLower();
					bool flag = BankInfo.IsHuaxiaCard(info.cardnumber.GetSafeRequest());
					string cmdTxt = string.Concat(new object[]
					{
						"insert into i_s_wgry_yhzh(RECID,RYBH,LRRZH,LRRXM,LRSJ,SPTG,SFYX,YHYHID,SFBH,JSKLX,JSKH,YHPT,MRZH,YHHH,BKZT,CJYHZHZT) values('",
						text5,
						"','",
						text4,
						"','",
						text,
						"','",
						text2,
						"',getdate(),1,1,'',",
						flag ? 0 : 1,
						",0,'",
						info.cardnumber.GetSafeRequest(),
						"','",
						text3,
						"',0,'",
						info.banknumber.GetSafeRequest(),
						"',0,0)"
					});
					this.CommonDao.ExecCommand(cmdTxt, CommandType.Text, null, -1);
					dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
					{
						"select a.*,b.yhhh,b.yhzh,b.yhzhmc,b.khwdmc,b.bkzt,b.cjyhzhzt,b.yhyhid as r_yhyhid,b.LRRZH as r_lrrzh,b.lrrxm as r_lrrxm,b.recid as r_recid,b.yhpt as r_yhpt,b.sfbh,b.jsklx,b.jskh from i_m_wgry a inner join i_s_wgry_yhzh b on a.rybh=b.rybh where a.rybh='",
						text4,
						"' and a.ryxm='",
						info.name.GetSafeRequest(),
						"' and a.sjhm='",
						info.phone.GetSafeRequest(),
						"' and a.sfzhm='",
						info.idnumber.GetSafeRequest(),
						"' and b.jskh='",
						info.cardnumber.GetSafeRequest(),
						"' and b.yhhh='",
						info.banknumber.GetSafeRequest(),
						"'"
					}));
					if (dataTable.Count<IDictionary<string, string>>() > 0)
					{
						createInfos = dataTable[0];
					}
				}
			}
			catch (Exception ex)
			{
				vtransPayWgryRespBase.message = ex.Message;
				vtransPayWgryRespBase.success = "0001";
				throw ex;
			}
			return vtransPayWgryRespBase;
		}
        /// <summary>
        /// 获取支付企业的工程信息，带当前管理人员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetPayCompnyProjects(string usercode, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				result = this.CommonDao.GetDataTable("select a.gcbh,a.gcmc,b.recid,c.qymc,sp1=STUFF((SELECT ',' + isnull(y.ryxm, '') FROM I_S_GC_CWRY x inner join i_m_ry y on x.rybh=y.rybh WHERE x.lx='1' and x.gczfqyid = b.recid FOR XML PATH('')), 1, 1, ''),sp2=STUFF((SELECT ',' + isnull(y.ryxm, '') FROM I_S_GC_CWRY x inner join i_m_ry y on x.rybh=y.rybh WHERE x.lx='2' and x.gczfqyid = b.recid FOR XML PATH('')), 1, 1, '') from i_m_gc a inner join I_S_GC_ZFDW b on a.gcbh=b.gcbh inner join i_s_gc_sgdw c on b.gcbh=c.gcbh and b.QYBH=c.qybh where b.zfqybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "') order by gcmc,qymc");
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取某个支付企业财务的
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetFinancePersonProjects(string rybh, string lx, string usercode, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				result = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select gczfqyid as recid from I_S_GC_CWRY where zfqybh=(select qybh from i_m_qyzh where yhzh='",
					usercode,
					"') and rybh='",
					rybh,
					"' and lx='",
					lx,
					"'"
				}));
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 保存财务人员工程
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SaveFinancePersonProjects(string rybh, string gczfqyid, string lx, string usercode, out string msg)
		{
			msg = "";
			bool result = true;
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (dataTable.Count == 0)
				{
					result = false;
					msg = "当前人员账号无效，可能是因为当前用户是维护账号";
					return result;
				}
				string text = dataTable[0]["qybh"];
				this.CommonDao.ExecCommand(string.Concat(new string[]
				{
					"delete from I_S_GC_CWRY where zfqybh='",
					text,
					"' and rybh='",
					rybh,
					"' and lx='",
					lx,
					"'"
				}), CommandType.Text, null, -1);
				if (!string.IsNullOrEmpty(gczfqyid))
				{
					dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
					{
						"select gcbh,qybh,zfqybh,recid from I_S_GC_ZFDW where recid in (",
						gczfqyid.FormatSQLInStr(),
						") and zfqybh='",
						text,
						"'"
					}));
					string[] array = gczfqyid.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string strRecid = array[i];
						IEnumerable<IDictionary<string, string>> source = from e in dataTable
						where e["recid"] == strRecid
						select e;
						if (source.Count<IDictionary<string, string>>() != 0)
						{
							IDictionary<string, string> dictionary = source.First<IDictionary<string, string>>();
							this.CommonDao.ExecCommand(string.Concat(new string[]
							{
								"insert into I_S_GC_CWRY(rybh,gcbh,sgqybh,zfqybh,lx,gczfqyid) values('",
								rybh,
								"','",
								dictionary["gcbh"],
								"','",
								dictionary["qybh"],
								"','",
								text,
								"','",
								lx,
								"','",
								dictionary["recid"],
								"')"
							}), CommandType.Text, null, -1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				result = false;
			}
			return result;
		}
        /// <summary>
        /// 获取某个工资册的附件
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetPayrollAttachs(string recid, out string msg)
		{
			msg = "";
			IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
			try
			{
				result = this.CommonDao.GetDataTable("select recid from I_S_PAY_FJ where ffid='" + recid + "' and lx='2'");
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取附件内容
        /// </summary>
        /// <param name="attachid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public KeyValuePair<string, byte[]> GetAttachContent(string attachid, out string msg)
		{
			msg = "";
			KeyValuePair<string, byte[]> result = new KeyValuePair<string, byte[]>("", null);
			try
			{
				IList<IDictionary<string, object>> binaryDataTable = this.CommonDao.GetBinaryDataTable("select fj,wjhz from I_S_PAY_FJ where recid='" + attachid + "'");
				if (binaryDataTable.Count<IDictionary<string, object>>() > 0)
				{
					string safeString = binaryDataTable[0]["wjhz"].GetSafeString("");
					byte[] value = binaryDataTable[0]["fj"] as byte[];
					result = new KeyValuePair<string, byte[]>(safeString, value);
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取发放所有附件内容，base64编码
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetAttachContentByPay(string payid, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				foreach (IDictionary<string, object> dictionary in this.CommonDao.GetBinaryDataTable("select fj from I_S_PAY_FJ where ffid='" + payid + "' and lx='2'"))
				{
					list.Add(new Dictionary<string, string>
					{
						{
							"fj",
							(dictionary["fj"] as byte[]).EncodeBase64()
						}
					});
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
        /// <summary>
        /// 修改人员卡号
        /// </summary>
        /// <param name="items"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public VTransPayWgryRespSetPersonCard ChangeRyCardInfo(VTransPayWgryReqSetPersonCardItem[] items)
		{
			VTransPayWgryRespSetPersonCard vtransPayWgryRespSetPersonCard = new VTransPayWgryRespSetPersonCard
			{
				message = "",
				success = "0000",
				rows = null
			};
			try
			{
				string userid = "interface";
				List<VTransPayWgryRespSetPersonCardItem> list = new List<VTransPayWgryRespSetPersonCardItem>();
				foreach (VTransPayWgryReqSetPersonCardItem vtransPayWgryReqSetPersonCardItem in items)
				{
					VTransPayWgryRespSetPersonCardItem vtransPayWgryRespSetPersonCardItem = new VTransPayWgryRespSetPersonCardItem();
					vtransPayWgryRespSetPersonCardItem.Load(vtransPayWgryReqSetPersonCardItem);
					list.Add(vtransPayWgryRespSetPersonCardItem);
					if (vtransPayWgryReqSetPersonCardItem.fromcard.Equals(vtransPayWgryReqSetPersonCardItem.tocard))
					{
						vtransPayWgryRespSetPersonCardItem.success = "0014";
						vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, "fromcard和tocard不能一样");
					}
					else
					{
						IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(string.Concat(new string[]
						{
							"select * from View_I_M_WGRY_PAY where ryxm='",
							vtransPayWgryReqSetPersonCardItem.name,
							"' and sfzhm='",
							vtransPayWgryReqSetPersonCardItem.idnumber,
							"' and jskh='",
							vtransPayWgryReqSetPersonCardItem.fromcard,
							"'"
						}));
						if (dataTable.Count<IDictionary<string, string>>() == 0)
						{
							vtransPayWgryRespSetPersonCardItem.success = "0010";
							vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, string.Concat(new string[]
							{
								vtransPayWgryReqSetPersonCardItem.name,
								",",
								vtransPayWgryReqSetPersonCardItem.idnumber,
								",",
								vtransPayWgryReqSetPersonCardItem.fromcard,
								"不存在"
							}));
						}
						else
						{
							string text = "1";
							string text2 = "";
							string newUserId = "";
							string text3 = "";
							string orgyhyhid = dataTable[0]["yhyhid"];
							string text4 = vtransPayWgryReqSetPersonCardItem.tocard.DecodeDes("8zzsjd95", "fcb95eze");
							IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable(string.Concat(new string[]
							{
								"select * from View_I_M_WGRY_PAY where ryxm='",
								vtransPayWgryReqSetPersonCardItem.name,
								"' and sfzhm='",
								vtransPayWgryReqSetPersonCardItem.idnumber,
								"' and jskh='",
								vtransPayWgryReqSetPersonCardItem.tocard,
								"'"
							}));
							bool flag;
							if (dataTable2.Count == 0)
							{
								IDictionary<string, string> dictionary = dataTable[0];
								BankInfo.IsHuaxiaCard(text4);
								flag = this.BindPersonalSettlement(new CL_BindPersonalSettlement
								{
									SettleAccountName = vtransPayWgryReqSetPersonCardItem.name,
									SettleAccount = text4,
									PayBank = vtransPayWgryReqSetPersonCardItem.tobankcode,
									UserId = dictionary["yhyhid"]
								}, userid, dictionary["yhpt"], out text2);
								VTransPayRespBindSettlement vtransPayRespBindSettlement = this.ParseResponse<VTransPayRespBindSettlement>(text2);
								flag = vtransPayRespBindSettlement.IsSucceed;
								if (!flag)
								{
									vtransPayWgryRespSetPersonCardItem.success = "0006";
									vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, vtransPayRespBindSettlement.message);
								}
							}
							else
							{
								IDictionary<string, string> dictionary2 = dataTable2[0];
								text = dictionary2["sfbh"];
								flag = (dictionary2["bkzt"].GetSafeInt(0) == 1);
								newUserId = dictionary2["yhyhid"].GetSafeString("");
								text3 = dictionary2["recid"].GetSafeString("");
								if (!flag)
								{
									vtransPayWgryRespSetPersonCardItem.success = "0006";
									vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, dictionary2["kkcwxx"]);
								}
							}
							if (flag)
							{
								IDictionary<string, string> dictionary3 = dataTable[0];
								if (dataTable2.Count == 0)
								{
									this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
									{
										"update I_S_WGRY_YHZH set jskh='",
										vtransPayWgryReqSetPersonCardItem.tocard,
										"',sfbh='",
										text,
										"',bkzt=1,yhhh='",
										vtransPayWgryReqSetPersonCardItem.tobankcode,
										"',kkcwxx='' where recid='",
										dictionary3["recid"],
										"'"
									}), CommandType.Text, null, -1);
								}
								foreach (IDictionary<string, string> dictionary4 in this.CommonDao.GetDataTable(string.Concat(new string[]
								{
									"select a.*,b.tradecode,b.zt as mzt from I_S_PAY_XQ a inner join i_m_pay b on a.ffid=b.recid where a.rykbh='",
									dictionary3["recid"],
									"' and a.zt in (",
									BankPayDetialStaus.GetCanModifyStatus().FormatSQLInStr(),
									") and b.ptlsh in (",
                                    vtransPayWgryReqSetPersonCardItem.payids.FormatSQLInStr(),
                                    ")"
								})))
								{
									int safeInt = dictionary4["mzt"].GetSafeInt(0);
									dictionary4["zt"].GetSafeInt(0);
									string text5 = dictionary4["tradecode"];
									string text6 = dictionary4["ffid"];
									string text7 = dictionary4["recid"];
									if (safeInt == 8 || string.IsNullOrEmpty(text5))
									{
										string text8 = "";
										if (dataTable2.Count > 0 && !string.IsNullOrEmpty(text3))
										{
											text8 = ",rykbh='" + text3 + "'";
										}
										this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
										{
											"update I_S_PAY_XQ set zt=0,ffbz=''",
											text8,
											" where recid='",
											text7,
											"'"
										}), CommandType.Text, null, -1);
										this.CommonDao.ExecCommandOpenSession(string.Concat(new object[]
										{
											"update I_M_PAY set zt=",
											1,
											",wgryxx=0 where recid='",
											text6,
											"'"
										}), CommandType.Text, null, -1);
									}
									else
									{
										CL_GetPayList info = new CL_GetPayList
										{
											TradeCode = text5
										};
										bool payList = this.GetPayList(info, userid, dictionary3["yhpt"], out text2);
										if (payList)
										{
											VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(text2);
											if (vtransPayRespGetPayList.IsSucceed)
											{
												IEnumerable<VTransPayRespGetPayListDataItem> data = vtransPayRespGetPayList.data;
												IEnumerable<VTransPayRespGetPayListDataItem> source = data.Where(e=>e.ToUserId.Equals(orgyhyhid));
												if (source.Count<VTransPayRespGetPayListDataItem>() == 0)
												{
													vtransPayWgryRespSetPersonCardItem.success = "0012";
													vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, "");
												}
												else
												{
													string orderId = source.ElementAt(0).OrderId;
													CL_ReSubmitDraw cl_ReSubmitDraw = new CL_ReSubmitDraw
													{
														OrderId = orderId,
														SettleAccount = text4
													};
													if (dataTable2.Count > 0)
													{
														cl_ReSubmitDraw.NewUserId = newUserId;
													}
													if (this.ReSubmitDraw(cl_ReSubmitDraw, userid, dictionary3["yhpt"], out text2))
													{
														VTransPayRespBase vtransPayRespBase = this.ParseResponse<VTransPayRespBase>(text2);
														if (vtransPayRespBase.IsSucceed)
														{
															string text9 = "";
															if (dataTable2.Count > 0 && !string.IsNullOrEmpty(text3))
															{
																text9 = ",rykbh='" + text3 + "'";
															}
															this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
															{
																"update I_S_PAY_XQ set zt=0,ffbz=''",
																text9,
																" where recid='",
																text7,
																"'"
															}), CommandType.Text, null, -1);
															this.CommonDao.ExecCommandOpenSession(string.Concat(new object[]
															{
																"update I_M_PAY set zt=",
																5,
																",wgryxx=0 where recid='",
																text6,
																"'"
															}), CommandType.Text, null, -1);
														}
														else
														{
															vtransPayWgryRespSetPersonCardItem.success = "0013";
															vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, vtransPayRespBase.message);
														}
													}
													else
													{
														vtransPayWgryRespSetPersonCardItem.success = "0013";
														vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, text2);
													}
												}
											}
											else
											{
												vtransPayWgryRespSetPersonCardItem.success = "0011";
												vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, text2);
											}
										}
										else
										{
											vtransPayWgryRespSetPersonCardItem.success = "0011";
											vtransPayWgryRespSetPersonCardItem.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCardItem.success, text2);
										}
									}
								}
							}
						}
					}
				}
				vtransPayWgryRespSetPersonCard.rows = list.ToArray();
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				vtransPayWgryRespSetPersonCard.success = "0001";
				vtransPayWgryRespSetPersonCard.message = VTransPayWgryRespBase.GetErrorInfo(vtransPayWgryRespSetPersonCard.success, ex.Message);
			}
			return vtransPayWgryRespSetPersonCard;
		}
        
        /// <summary>
        /// 修改一个人员卡号，修改一条发放记录的
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
		public bool ChangeOneRyCardInfo(string ffcbid, string cardno, string usercode, string realname, out string msg)
		{
			msg = "";
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(cardno))
				{
					msg = "卡号不能为空";
					return result;
				}
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_s_pay_xq where recid='" + ffcbid + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放人员记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary = dataTable[0];
				IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + dictionary["ffid"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放主记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary2 = dataTable2[0];
				IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + dictionary["rykbh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员卡信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary3 = dataTable3[0];
				IList<IDictionary<string, string>> dataTable4 = this.CommonDao.GetDataTable("select * from I_M_WGRY where rybh='" + dictionary3["rybh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary4 = dataTable4[0];
				if (dictionary3["jskh"].DecodeDes("8zzsjd95", "fcb95eze").Equals(cardno))
				{
					msg = "卡号不能跟原记录一样";
					return result;
				}
				IList<IDictionary<string, string>> dtOtherPerson = this.CommonDao.GetDataTable("select a.ryxm,a.sfzhm,b.jskh from I_M_WGRY a inner join i_s_wgry_yhzh b on a.rybh=b.rybh where b.jskh='" + CryptFun.LrEncode(cardno) + "' ");
				if (dtOtherPerson.Count() > 0)
				{
                    string tmpxm = dtOtherPerson[0]["ryxm"];
                    string tmpsfzhm = dtOtherPerson[0]["sfzhm"];
                    if (tmpxm != dictionary4["ryxm"] || tmpsfzhm != dictionary4["sfzhm"])
                    {
                        msg = "银行卡号" + cardno + "已被" + tmpxm + "绑定（身份证号码为：" + CryptFun.LrDecode(tmpsfzhm)+"），在人员管理里面删除该记录才能继续";
                        return result;
                    }
				}
				if (!BankPayDetialStaus.CanModify(dictionary["zt"].GetSafeInt(0)))
				{
					msg = "当前记录无法操作";
					return result;
				}
				string text = "";

				if (!this.GetBankNameByAli(cardno, out text))
				{
					msg = text;
					return result;
				}
				string text2 = dictionary4["ryxm"];
				dictionary4["sfzhm"].DecodeDes("8zzsjd95", "fcb95eze");
				string text3 = cardno.EncodeDes("8zzsjd95", "fcb95eze");
				string text4 = "1";
				string newUserId = "";
				string text5 = "";
				string orgyhyhid = dictionary3["yhyhid"];
				IList<IDictionary<string, string>> dataTable5 = this.CommonDao.GetDataTable(string.Concat(new string[]
				{
					"select * from View_I_M_WGRY_PAY where ryxm='",
					text2,
					"' and sfzhm='",
					dictionary4["sfzhm"],
					"' and jskh='",
					text3,
					"'"
				}));
				bool flag;
				if (dataTable5.Count == 0)
				{
					BankInfo.IsHuaxiaCard(cardno);
					flag = this.BindPersonalSettlement(new CL_BindPersonalSettlement
					{
						SettleAccountName = text2,
						SettleAccount = cardno,
						PayBank = text,
						UserId = orgyhyhid
					}, usercode, dictionary3["yhpt"], out msg);
					VTransPayRespBindSettlement vtransPayRespBindSettlement = this.ParseResponse<VTransPayRespBindSettlement>(msg);
					flag = vtransPayRespBindSettlement.IsSucceed;
					if (!flag)
					{
						msg = vtransPayRespBindSettlement.message;
					}
					else
					{
						this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
						{
							"update I_S_WGRY_YHZH set jskh='",
							text3,
							"',sfbh='",
							text4,
							"',bkzt=1,yhhh='",
							text,
							"',kkcwxx='' where recid='",
							dictionary3["recid"],
							"'"
						}), CommandType.Text, null, -1);
					}
				}
				else
				{
					IDictionary<string, string> dictionary5 = dataTable5[0];
					text4 = dictionary5["sfbh"];
					flag = (dictionary5["bkzt"].GetSafeInt(0) == 1);
					newUserId = dictionary5["yhyhid"].GetSafeString("");
					text5 = dictionary5["recid"].GetSafeString("");
					if (!flag)
					{
						msg = dictionary5["kkcwxx"];
					}
				}
				if (flag)
				{
					foreach (IDictionary<string, string> dictionary6 in this.CommonDao.GetDataTable(string.Concat(new string[]
					{
						"select a.*,b.tradecode,b.zt as mzt from I_S_PAY_XQ a inner join i_m_pay b on a.ffid=b.recid where a.recid='",
						ffcbid,
						"' and a.zt in (",
						BankPayDetialStaus.GetCanModifyStatus().FormatSQLInStr(),
						")"
					})))
					{
						int safeInt = dictionary6["mzt"].GetSafeInt(0);
						dictionary6["zt"].GetSafeInt(0);
						string text6 = dictionary6["tradecode"];
						string text7 = dictionary6["ffid"];
						string text8 = dictionary6["recid"];
						if (safeInt == 8 || string.IsNullOrEmpty(text6))
						{
							string text9 = "";
							if (dataTable5.Count > 0 && !string.IsNullOrEmpty(text5))
							{
								text9 = ",rykbh='" + text5 + "'";
							}
							this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
							{
								"update I_S_PAY_XQ set zt=0,ffbz=''",
								text9,
								" where recid='",
								text8,
								"'"
							}), CommandType.Text, null, -1);
							this.CommonDao.ExecCommandOpenSession(string.Concat(new object[]
							{
								"update I_M_PAY set zt=",
								1,
								",wgryxx=0 where recid='",
								text7,
								"'"
							}), CommandType.Text, null, -1);
						}
						else
						{
							CL_GetPayList info = new CL_GetPayList
							{
								TradeCode = text6
							};
							if (this.GetPayList(info, usercode, dictionary3["yhpt"], out msg))
							{
								VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
								if (vtransPayRespGetPayList.IsSucceed)
								{
									msg = "";
									IEnumerable<VTransPayRespGetPayListDataItem> data = vtransPayRespGetPayList.data;
									IEnumerable<VTransPayRespGetPayListDataItem> source = data.Where((VTransPayRespGetPayListDataItem e) => e.ToUserId.Equals(orgyhyhid));
									if (source.Count<VTransPayRespGetPayListDataItem>() == 0)
									{
										msg = "获取发放记录失败";
									}
									else
									{
										string orderId = source.ElementAt(0).OrderId;
										CL_ReSubmitDraw cl_ReSubmitDraw = new CL_ReSubmitDraw
										{
											OrderId = orderId,
											SettleAccount = cardno
										};
										if (dataTable5.Count > 0)
										{
											cl_ReSubmitDraw.NewUserId = newUserId;
										}
										if (this.ReSubmitDraw(cl_ReSubmitDraw, usercode, dictionary3["yhpt"], out msg))
										{
											if (this.ParseResponse<VTransPayRespBase>(msg).IsSucceed)
											{
												string text10 = "";
												if (dataTable5.Count > 0 && !string.IsNullOrEmpty(text5))
												{
													text10 = ",rykbh='" + text5 + "'";
												}
												this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
												{
													"update I_S_PAY_XQ set zt=0,ffbz=''",
													text10,
													" where recid='",
													text8,
													"'"
												}), CommandType.Text, null, -1);
												this.CommonDao.ExecCommandOpenSession(string.Concat(new object[]
												{
													"update I_M_PAY set zt=",
													5,
													",wgryxx=0 where recid='",
													text7,
													"'"
												}), CommandType.Text, null, -1);
												msg = "";
											}
											else
											{
												msg = "重新发放失败";
											}
										}
										else
										{
											msg = "重新发放失败";
										}
									}
								}
								else
								{
									msg = "获取发放记录失败";
								}
							}
							else
							{
								msg = "获取发放记录失败";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return msg == "";
		}
        /// <summary>
        /// 工资发放余额是否足够
        /// </summary>
        /// <param name="payid"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool PayMoneyEnough(string payid, string usercode, out string msg)
		{
			bool flag = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + payid + "'");
				if (dataTable.Count == 0)
				{
					msg = "支付记录不存在";
					return flag;
				}
				IDictionary<string, string> dictionary = dataTable[0];
				decimal safeDecimal = dictionary["yfze"].GetSafeDecimal(0m);
				string item = dictionary["ffzh"];
				string str = dictionary["zfqybh"];
				string yhpt = dictionary["yhpt"];
				IList<string> list = new List<string>();
				list.Add(item);
				CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
				cl_GetUserInfos.UserIds.AddRange(list);
				if (this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg))
				{
					VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
					if (vtransPayRespGetUserInfo.IsSucceed)
					{
						msg = "";
						if (vtransPayRespGetUserInfo.data.Length != 0)
						{
							flag = (vtransPayRespGetUserInfo.data[0].AccountBalance >= safeDecimal);
							if (!flag)
							{
								msg = "账户余额不足，请先划拨额度";
							}
							else
							{
								msg = "";
							}
						}
					}
					else
					{
						flag = false;
						msg = vtransPayRespGetUserInfo.message;
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				flag = false;
			}
			return flag;
		}
        /// <summary>
        /// 获取企业的资金划拨人，及企业所有人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetMoneyTransPersons(string usercode, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				list = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (list.Count == 0)
				{
					msg = "当前用户无效";
					return list;
				}
				string str = list[0]["qybh"];
				string sql = "select rybh,ryxm,(select count(*) from i_s_qy_ry where i_s_qy_ry.qybh=i_m_ry.qybh and i_s_qy_ry.rybh=i_m_ry.rybh and gw='1') as c1 from i_m_ry where qybh='" + str + "' order by ryxm";
				list = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}

        /// <summary>
        /// 获取当前人员，带第一步审批人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetCheck1Persons(string usercode, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				list = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (list.Count == 0)
				{
					msg = "当前用户无效";
					return list;
				}
				string str = list[0]["qybh"];
				string sql = "select rybh,ryxm,(select count(*) from i_s_qy_ry where i_s_qy_ry.qybh=i_m_ry.qybh and i_s_qy_ry.rybh=i_m_ry.rybh and gw='2') as c1 from i_m_ry where qybh='" + str + "' order by ryxm";
				list = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
        /// <summary>
        /// 获取企业的第二步审批人，及企业所有人
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetCheck2Persons(string usercode, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				list = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (list.Count == 0)
				{
					msg = "当前用户无效";
					return list;
				}
				string str = list[0]["qybh"];
				string sql = "select rybh,ryxm,(select count(*) from i_s_qy_ry where i_s_qy_ry.qybh=i_m_ry.qybh and i_s_qy_ry.rybh=i_m_ry.rybh and gw='3') as c1 from i_m_ry where qybh='" + str + "' order by ryxm";
				list = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
		/// <summary>
        /// 保存企业的财务人员岗位
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		[Transaction(ReadOnly = false)]
		public bool SaveFinacePerson(string usercode, string userids1, string userids2, string userids3, string userids11, string userids12, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (dataTable.Count == 0)
				{
					msg = "当前用户无效";
					return result;
				}
				string qybh = dataTable[0]["qybh"];
				CommonDao.ExecCommand("delete from i_s_qy_ry where qybh='" + qybh + "' and gw in ('1','2','3','11','12')", CommandType.Text);
				CommonDao.ExecCommand("insert into i_s_qy_ry(RYBH,RYXM,QYBH,GW) select rybh,ryxm,'"+qybh+"','1' from i_m_ry where rybh in ("+userids1.FormatSQLInStr()+")", CommandType.Text);
                CommonDao.ExecCommand("insert into i_s_qy_ry(RYBH,RYXM,QYBH,GW) select rybh,ryxm,'"+qybh+"','2' from i_m_ry where rybh in ("+userids2.FormatSQLInStr()+")", CommandType.Text);
                CommonDao.ExecCommand("insert into i_s_qy_ry(RYBH,RYXM,QYBH,GW) select rybh,ryxm,'"+qybh+"','3' from i_m_ry where rybh in ("+userids3.FormatSQLInStr()+")", CommandType.Text);
                CommonDao.ExecCommand("insert into i_s_qy_ry(RYBH,RYXM,QYBH,GW) select rybh,ryxm,'"+qybh+"','11' from i_m_ry where rybh in ("+userids11.FormatSQLInStr()+")", CommandType.Text);
                CommonDao.ExecCommand("insert into i_s_qy_ry(RYBH,RYXM,QYBH,GW) select rybh,ryxm,'"+qybh+"','12' from i_m_ry where rybh in ("+userids12.FormatSQLInStr()+")", CommandType.Text);
				result = true;
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        
		/// <summary>
        /// 用户能否资金划拨
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool CanMoneyTrans(string usercode, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select count(*) as t1 from i_s_qy_ry where gw='1' and rybh in (select qybh from i_m_qyzh where yhzh='" + usercode + "')");
				result = (dataTable.Count<IDictionary<string, string>>() > 0 && dataTable[0]["t1"].GetSafeInt(0) > 0);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

		/// <summary>
        /// 获取银行回单
        /// </summary>
        /// <returns></returns>
		public byte[] GetPayBankBack(string payid, string usercode, out string msg)
		{
			byte[] result = null;
			msg = "";
			try
			{
				string sql = "select ffzh,yhpt,cjrzh,cjrxm,tradecode from i_m_pay where recid='" + payid + "' ";
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(sql);
				string value = "";
				string yhpt = "";
				string text = "";
                string text4 = "";
				if (dataTable.Count<IDictionary<string, string>>() > 0)
				{
					value = dataTable[0]["ffzh"];
					yhpt = dataTable[0]["yhpt"];
					text = dataTable[0]["cjrzh"];
                    text4 = dataTable[0]["tradecode"];
				}
				if (string.IsNullOrEmpty(value))
				{
					msg = "找不到发放记录";
					return result;
				}
				if (string.IsNullOrEmpty(usercode))
				{
					usercode = text;
				}
				
				if (string.IsNullOrEmpty(text4))
				{
					return result;
				}
				CL_GetPayReturnList info = new CL_GetPayReturnList
				{
					TradeCode = text4
				};
				if (this.GetPaymentReturnList(info, usercode, yhpt, out msg))
				{
					VTransPayRespGetPaymentReturnList vtransPayRespGetPaymentReturnList = this.ParseResponse<VTransPayRespGetPaymentReturnList>(msg);
					if (vtransPayRespGetPaymentReturnList.IsSucceed)
					{
						IList<byte[]> list = new List<byte[]>();
						foreach (VTransPayRespGetPaymentReturnListItem vtransPayRespGetPaymentReturnListItem in vtransPayRespGetPaymentReturnList.data)
						{
							if (!string.IsNullOrEmpty(vtransPayRespGetPaymentReturnListItem.PaymentReturn))
							{
								list.Add(vtransPayRespGetPaymentReturnListItem.PaymentReturn.DecodeBase64Array());
							}
						}
						result = PdfWaterMark.MergeFiles(list, out msg);
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
				throw ex;
			}
			return result;
		}

		/// <summary>
        /// 重新提交一条记录
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
		public bool ResubmitPayItem(string ffcbid, string usercode, string realnam, out string msg)
		{
			msg = "";
			bool result = false;
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_s_pay_xq where recid='" + ffcbid + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放人员记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary = dataTable[0];
				IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + dictionary["ffid"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放主记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary2 = dataTable2[0];
				IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + dictionary["rykbh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员卡信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary3 = dataTable3[0];
				IList<IDictionary<string, string>> dataTable4 = this.CommonDao.GetDataTable("select * from I_M_WGRY where rybh='" + dictionary3["rybh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary4 = dataTable4[0];
				string settleAccount = dictionary3["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
				if (!BankPayDetialStaus.CanModify(dictionary["zt"].GetSafeInt(0)))
				{
					msg = "当前记录无法操作";
					return result;
				}
				string tradeCode = dictionary2["tradecode"];
				string yhyhid = dictionary3["yhyhid"];
				CL_GetPayList info = new CL_GetPayList
				{
					TradeCode = tradeCode
				};
				if (this.GetPayList(info, usercode, dictionary3["yhpt"], out msg))
				{
					VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
					if (vtransPayRespGetPayList.IsSucceed)
					{
						msg = "";
						IEnumerable<VTransPayRespGetPayListDataItem> source = from e in vtransPayRespGetPayList.data
						where e.ToUserId.Equals(yhyhid) && e.Amount.GetSafeDecimal() == dictionary["yfje"].GetSafeDecimal()
						select e;
                        //SysLog4.WriteError(dictionary["yfje"]);
						if (source.Count<VTransPayRespGetPayListDataItem>() == 0)
						{
							msg = "获取发放记录失败";
						}
						else
						{
							string orderId = source.ElementAt(0).OrderId;
                            //SysLog4.WriteError(new JavaScriptSerializer().Serialize(source));
							CL_ReSubmitDraw info2 = new CL_ReSubmitDraw
							{
								OrderId = orderId,
								SettleAccount = settleAccount
							};
							if (this.ReSubmitDraw(info2, usercode, dictionary3["yhpt"], out msg))
							{
								if (this.ParseResponse<VTransPayRespBase>(msg).IsSucceed)
								{
									string text = "";
									this.CommonDao.ExecCommandOpenSession(string.Concat(new string[]
									{
										"update I_S_PAY_XQ set zt=0,ffbz=''",
										text,
										" where recid='",
										ffcbid,
										"'"
									}), CommandType.Text, null, -1);
									this.CommonDao.ExecCommandOpenSession(string.Concat(new object[]
									{
										"update I_M_PAY set zt=",
										5,
										",wgryxx=0 where recid='",
										dictionary2["recid"],
										"'"
									}), CommandType.Text, null, -1);
									msg = "";
								}
								else
								{
									msg = "重新发放返回失败";
								}
							}
							else
							{
								msg = "调用重新发放失败";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return msg == "";
		}

		/// <summary>
        /// 获取支付系统行号
        /// </summary>
        /// <param name="cardno"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetYhhh(string cardno, out string msg)
		{
			return this.GetBankNameByAli(cardno, out msg);
		}
        /// <summary>
        /// 验证人员卡号是否正确；先查询本地记录；如果本地记录找不或者没错误，去银行接口验证，如果绑卡成功，写入数据库；如果绑卡失败，不写入数据库
        /// </summary>
        /// <param name="reqinfo"></param>
        /// <returns></returns>
        public VTransPayWgryRespBase VerifyUserCard(VTransPayWgryReqBindCardMain reqinfo)
        {
            VTransPayWgryRespBase ret = new VTransPayWgryRespBase() { success = VTransPayWgryRespBase.ErrorSuccess, message = "" };
            try
            {
                // 判断参数是否有效
                string msg = "";
                if (!reqinfo.IsValid(out msg))
                {
                    ret.success = VTransPayWgryRespBase.ErrorParam;
                    ret.message = VTransPayWgryRespBase.GetErrorInfo(ret.success, msg);
                    return ret;
                }
                // ------------------  验证本地记录 ----------------------------
                // 获取人员主表身份证一样的记录（最多一条）
                IList<IDictionary<string, string>> rylist = CommonDao.GetDataTable("select * from i_m_wgry where sfzhm='" + reqinfo.idnumber + "'");
                // 获取银行卡从表卡号一样的记录（最多一条）
                IList<IDictionary<string, string>> cardlist = CommonDao.GetDataTable("select * from i_s_wgry_yhzh where jskh='" + reqinfo.cardnumber + "' and yhpt='"+reqinfo.bankplatform+"'");
                // 判断数据库记录是否有效
                IDictionary<string, string> findry = null;
                IDictionary<string, string> findcard = null;

                if (rylist.Count() > 0)
                {
                    if (rylist[0]["ryxm"] != reqinfo.name)
                    {
                        ret.success = VTransPayWgryRespBase.ErrorParam;
                        ret.message = VTransPayWgryRespBase.GetErrorInfo(ret.success, "身份证已绑定了" + rylist[0]["ryxm"]);
                        return ret;
                    }
                    else
                        findry = rylist[0];
                }
                if (cardlist.Count() > 0)
                {
                    if (findry == null || cardlist[0]["rybh"] != findry["rybh"])
                    {
                        ret.success = VTransPayWgryRespBase.ErrorParam;
                        ret.message = VTransPayWgryRespBase.GetErrorInfo(ret.success, "卡号已绑定了" + cardlist[0]["yhzhmc"]);
                        return ret;
                    }
                    else
                        findcard = cardlist[0];
                }
                if (findry != null && findcard != null)
                {
                    int bkzt = findcard["bkzt"].GetSafeInt();
                    if (bkzt == BindCardStatus.Binding)
                    {
                        ret.success = VTransPayWgryRespBase.ErrorBindCardNotComplete;
                        return ret;
                    }
                    else if (bkzt == BindCardStatus.Complete)
                    {
                        ret.success = VTransPayWgryRespBase.ErrorSuccess;
                        return ret;
                    }
                    else
                    {
                        ret.success = VTransPayWgryRespBase.ErrorBindCardBind;
                        return ret;
                    }
                }
                // ---------------------  调用银行接口绑卡  ------------------------
                string yhyhid = "";
                // 查询远程信息
                VTransPayRespQueryUserDataItem findAccountInfo = GetWgryAccountInfo(reqinfo.name, CryptFun.LrDecode(reqinfo.idnumber), CryptFun.LrDecode(reqinfo.cardnumber), reqinfo.bankplatform);
                // 创建账号
                if (findAccountInfo == null)
                {
                    CL_RegisterUser reqRegister = new CL_RegisterUser();
                    reqRegister.TradeMemBerName = reqinfo.name;
                    reqRegister.TradeMemberProperty = "1";
                    reqRegister.Phone = reqinfo.phone;
                    reqRegister.PapersType = "10";
                    reqRegister.PapersCode = CryptFun.LrDecode(reqinfo.idnumber);
                    reqRegister.IsMessager = "2";
                    reqRegister.Email = "bdsoft@sina.com";


                    bool code = RegisterUser(reqRegister, "system", reqinfo.bankplatform, out msg);
                    if (code)
                    {
                        VTransPayRespRegisterUser respRegisterUser = this.ParseResponse<VTransPayRespRegisterUser>(msg);
                        if (!respRegisterUser.IsSucceed)
                        {
                            ret.success = VTransPayWgryRespBase.ErrorBindCardException;
                            ret.message = VTransPayWgryRespBase.GetErrorInfo(ret.success, respRegisterUser.message);
                            return ret;
                        }
                        else
                        {
                            yhyhid = respRegisterUser.data.UserId;                            
                        }
                    }
                    else
                    {
                        ret.success = VTransPayWgryRespBase.ErrorBindCardException;
                        return ret;
                    }
                }
                else
                    yhyhid = findAccountInfo.UserId;
                // 绑卡
                if (findAccountInfo == null || !findAccountInfo.BindSucceed)
                {
                    CL_BindPersonalSettlement info = new CL_BindPersonalSettlement();
                    info.SettleAccountName = reqinfo.name;
                    info.UserId = yhyhid;
                    info.SettleAccount = CryptFun.LrDecode(reqinfo.cardnumber);
                    info.PayBank = reqinfo.banknumber;
                    if (info.PayBank == "")
                    {
                        if (GetBankNameByAli(info.SettleAccount, out msg))
                            info.PayBank = msg;
                        else
                            info.PayBank = DefaultBankNumber;
                    }

                    if (BindPersonalSettlement(info, "system", reqinfo.bankplatform, out msg))
                    {
                        VTransPayRespBindSettlement bindresp = this.ParseResponse<VTransPayRespBindSettlement>(msg);
                        if (bindresp.IsSucceed)
                        {
                            string sql = "";
                            string rybh = Guid.NewGuid().ToString().Replace("-", "");
                            if (findry == null)
                            {
                                sql = "insert into i_m_wgry(SSJCJGBH,LXBH,QYBH,RYBH,RYXM,SFZHM,LRRZH,LRRXM,SSDWBH,SSDWMC,LRSJ,SPTG,SFYX,ZH,SJHM) values('','11','','" + rybh +
                                    "','" + reqinfo.name + "','" + reqinfo.idnumber + "','system','系统','','',getdate(),1,1,'','" + reqinfo.phone + "')";
                                CommonDao.ExecCommandOpenSession(sql, CommandType.Text);
                            }
                            else
                                rybh = findry["rybh"];
                            if (findcard == null)
                            {
                                findAccountInfo = GetWgryAccountInfo(reqinfo.name, CryptFun.LrDecode(reqinfo.idnumber), CryptFun.LrDecode(reqinfo.cardnumber), reqinfo.bankplatform);
                                string yhzh="", yhzhmc="", khwdmc="";
                                if (findAccountInfo != null)
                                {
                                    yhzh = findAccountInfo.OthBankPayeeSubAcc;
                                    yhzhmc = findAccountInfo.SettleAccountName;
                                    khwdmc = findAccountInfo.OthBankPayeeSubAccSetteName;
                                }
                                string recid = Guid.NewGuid().ToString().Replace("-", "");
                                int sfbh = BankInfo.IsHuaxiaCard(CryptFun.LrDecode(reqinfo.cardnumber))?0:1;
                                sql = "insert into i_s_wgry_yhzh(RECID,RYBH,LRRZH,LRRXM,LRSJ,SPTG,SFYX,YHYHID,SFBH,JSKLX,JSKH,YHPT,MRZH,YHHH,BKZT,CJYHZHZT,KKCWXX,yhzh,yhzhmc,khwdmc) values('" +
                                    recid + "','" + rybh + "','system','系统',getdate(),1,1,'" + yhyhid + "'," + sfbh + ",0,'" + reqinfo.cardnumber + 
                                    "','"+reqinfo.bankplatform+"',0,'" + info.PayBank + "',1,1,'','"+yhzh+"','"+yhzhmc+"','"+khwdmc+"')";
                                CommonDao.ExecCommandOpenSession(sql, CommandType.Text);
                            }
                        }
                        else
                        {
                            ret.success = VTransPayWgryRespBase.ErrorBindCardException;
                            ret.message = VTransPayWgryRespBase.GetErrorInfo(ret.success, bindresp.message);
                            return ret;
                        }
                    }
                    else
                    {
                        ret.success = VTransPayWgryRespBase.ErrorBindCardException;
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                ret.success = VTransPayWgryRespBase.ErrorException;
                ret.message = ex.Message;
            }

            return ret;
        } 
        /// <summary>
        /// 校验身份证喝银行卡是否有重复
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool CheckRepeatPayItem(ref IList<IDictionary<string, string>> datas)
        {
            bool ret = false;
            try
            {
                // 过滤掉无效的记录
                var validDatas = datas.Where(e => e["sfyx"] != "2");
                // 表格内对比
                for(int i=0; i< validDatas.Count(); i++)
                {
                    IDictionary<string, string> row = validDatas.ElementAt(i);
                    if (row["sfyx"] == "2")
                        continue;
                    for(int j=i+1;j<validDatas.Count(); j++)
                    {
                        IDictionary<string, string> tmpRow = validDatas.ElementAt(j);
                        if (tmpRow["身份证号码"].Equals(row["身份证号码"], StringComparison.OrdinalIgnoreCase) && tmpRow["姓名"] != row["姓名"])
                        {
                            tmpRow["errmsg"] = "导入表格中，身份证跟"+row["姓名"]+"的重复";
                            tmpRow["sfyx"] = "2";
                        }else if (tmpRow["银行卡号"].Equals(row["银行卡号"], StringComparison.OrdinalIgnoreCase) 
                            && !tmpRow["身份证号码"].Equals(row["身份证号码"], StringComparison.OrdinalIgnoreCase))
                        {
                            tmpRow["errmsg"] = "导入表格中，银行卡号跟"+row["姓名"]+"的重复";
                            tmpRow["sfyx"] = "2";
                        }
                    }
                }
                validDatas = datas.Where(e => e["sfyx"] != "2");

                StringBuilder sbsfzhms = new StringBuilder();
                StringBuilder sbyhkhs = new StringBuilder();
                foreach (IDictionary<string,string> row in validDatas)
                {
                    string encodeId1 = CryptFun.LrEncode(row["身份证号码"].ToLower());
                    string encodeId2 = CryptFun.LrEncode(row["身份证号码"].ToUpper());
                    string encodeCard1 = CryptFun.LrEncode(row["银行卡号"].ToLower());
                    string encodeCard2 = CryptFun.LrEncode(row["银行卡号"].ToUpper());
                    sbsfzhms.Append(encodeId1+",");
                    if (encodeId1 != encodeId2)
                        sbsfzhms.Append(encodeId2 + ",");
                    sbyhkhs.Append(encodeCard1+ ",");
                    if (encodeCard1 != encodeCard2)
                        sbyhkhs.Append(encodeCard2 + ",");
                }
                IList<IDictionary<string, string>> rylist = CommonDao.GetDataTable("select ryxm,sfzhm from i_m_wgry where sfzhm in (" + sbsfzhms.ToString().FormatSQLInStr() + ")");
                IList<IDictionary<string, string>> cardlist = CommonDao.GetDataTable("select b.ryxm,b.sfzhm,a.jskh from i_s_wgry_yhzh a inner join i_m_wgry b on a.rybh=b.rybh where a.jskh in (" + sbyhkhs.ToString().FormatSQLInStr() + ")");
                
                foreach (IDictionary<string,string> tableRow in rylist)
                {
                    foreach (IDictionary<string,string> importRow in validDatas)
                    {
                        if (tableRow["ryxm"] != importRow["姓名"] && 
                            CryptFun.LrDecode(tableRow["sfzhm"]).Equals(importRow["身份证号码"], StringComparison.OrdinalIgnoreCase))
                        {
                            importRow["errmsg"] = "导入表格中，身份证跟数据库中的"+tableRow["ryxm"]+"的重复";
                            importRow["sfyx"] = "2";
                        }
                    }
                }
                foreach (IDictionary<string,string> tableRow in cardlist)
                {
                    foreach (IDictionary<string,string> importRow in validDatas)
                    {
                        if (!CryptFun.LrDecode(tableRow["sfzhm"]).Equals(importRow["身份证号码"], StringComparison.OrdinalIgnoreCase) && 
                            CryptFun.LrDecode(tableRow["jskh"]).Equals(importRow["银行卡号"], StringComparison.OrdinalIgnoreCase))
                        {
                            importRow["errmsg"] = "导入表格中，银行卡号跟数据库中的"+tableRow["ryxm"]+"的重复";
                            importRow["sfyx"] = "2";
                        }
                    }
                }
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 校验身份证喝银行卡是否有重复
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string CheckRepeatPayItem(ref VTransPayWgryReqSetPayRollItem[] datas)
        {
            StringBuilder sbret = new StringBuilder();
            try
            {
                // 表格内对比
                for(int i=0; i< datas.Count(); i++)
                {
                    VTransPayWgryReqSetPayRollItem row = datas[i];
                    for(int j=i+1;j<datas.Count(); j++)
                    {
                        VTransPayWgryReqSetPayRollItem tmpRow = datas[j];
                        if (CryptFun.LrDecode(tmpRow.idnumber).Equals(CryptFun.LrDecode(row.idnumber), StringComparison.OrdinalIgnoreCase) 
                            && tmpRow.name != row.name)
                        {
                            sbret.Append("导入表格中，"+row.name+"的身份证跟"+tmpRow.name+"的重复");
                        }else if (CryptFun.LrDecode(tmpRow.cardnumber).Equals(CryptFun.LrDecode(row.cardnumber), StringComparison.OrdinalIgnoreCase) 
                            && !CryptFun.LrDecode(tmpRow.idnumber).Equals(CryptFun.LrDecode(row.idnumber), StringComparison.OrdinalIgnoreCase))
                        {
                            sbret.Append("导入表格中，" + row.name + "的银行卡号跟" + tmpRow.name + "的重复");
                        }
                    }
                }

                StringBuilder sbsfzhms = new StringBuilder();
                StringBuilder sbyhkhs = new StringBuilder();
                foreach (VTransPayWgryReqSetPayRollItem row in datas)
                {
                    string decodeId = CryptFun.LrDecode(row.idnumber);
                    string decodeCard = CryptFun.LrDecode(row.cardnumber);
                    string encodeId1 = CryptFun.LrEncode(decodeId.ToLower());
                    string encodeId2 = CryptFun.LrEncode(decodeId.ToUpper());
                    string encodeCard1 = CryptFun.LrEncode(decodeCard.ToLower());
                    string encodeCard2 = CryptFun.LrEncode(decodeCard.ToUpper());
                    sbsfzhms.Append(encodeId1+",");
                    if (encodeId1 != encodeId2)
                        sbsfzhms.Append(encodeId2 + ",");
                    sbyhkhs.Append(encodeCard1+ ",");
                    if (encodeCard1 != encodeCard2)
                        sbyhkhs.Append(encodeCard2 + ",");
                }
                IList<IDictionary<string, string>> rylist = CommonDao.GetDataTable("select ryxm,sfzhm from i_m_wgry where sfzhm in (" + sbsfzhms.ToString().FormatSQLInStr() + ")");
                IList<IDictionary<string, string>> cardlist = CommonDao.GetDataTable("select b.ryxm,b.sfzhm,a.jskh from i_s_wgry_yhzh a inner join i_m_wgry b on a.rybh=b.rybh where a.jskh in (" + sbyhkhs.ToString().FormatSQLInStr() + ")");
                
                foreach (IDictionary<string,string> tableRow in rylist)
                {
                    foreach (VTransPayWgryReqSetPayRollItem importRow in datas)
                    {
                        if (tableRow["ryxm"] != importRow.name && 
                            CryptFun.LrDecode(tableRow["sfzhm"]).Equals(CryptFun.LrDecode(importRow.idnumber), StringComparison.OrdinalIgnoreCase))
                        {
                            sbret.Append("导入表格中，" + importRow.name + "的身份证跟数据库中的" + tableRow["ryxm"] + "的重复");
                        }
                    }
                }
                foreach (IDictionary<string,string> tableRow in cardlist)
                {
                    foreach (VTransPayWgryReqSetPayRollItem importRow in datas)
                    {
                        if (!CryptFun.LrDecode(tableRow["sfzhm"]).Equals(CryptFun.LrDecode(importRow.idnumber), StringComparison.OrdinalIgnoreCase) 
                            && CryptFun.LrDecode(tableRow["jskh"]).Equals(CryptFun.LrDecode(importRow.cardnumber), StringComparison.OrdinalIgnoreCase))
                        {
                            sbret.Append("导入表格中，" + importRow.name + "的银行卡号跟数据库中的" + tableRow["ryxm"] + "的重复");
                        }
                    }
                }
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return sbret.ToString();
        }
        /// <summary>
        /// 校验导入的工资册中无效的内容
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public void CheckInvalidPayItem(ref IList<IDictionary<string, string>> datas)
        {
            foreach (IDictionary<string, string> row in datas)
            {
                row["姓名"] = row["姓名"].GetValidPayString();
                row["电话"] = row["电话"].GetValidPayString();
                row["身份证号码"] = row["身份证号码"].GetValidPayString();
                row["银行卡号"] = row["银行卡号"].GetValidPayString();
                row["实发工资"] = row["实发工资"].GetValidPayString();
                if (row.Keys.Contains("yhhh"))
                    row["yhhh"] = "";
                else
                    row.Add("yhhh", "");
                int sfyx = 1;
                string errmsg = "";
                if (string.IsNullOrEmpty(row["姓名"]) || string.IsNullOrEmpty(row["身份证号码"]) ||
                    string.IsNullOrEmpty(row["银行卡号"]) || string.IsNullOrEmpty(row["电话"]) || string.IsNullOrEmpty(row["实发工资"]))
                {
                    sfyx = 2;
                    errmsg = "信息不全，姓名、身份号码、电话、银行卡号、实发工资都不能为空";
                }
                else if (!IDCardValidation.CheckIDCard(row["身份证号码"]))
                {
                    sfyx = 2;
                    errmsg = "无效的身份证号码";
                }
                else if (row["电话"].Length != 11)
                {
                    sfyx = 2;
                    errmsg = "手机号码无效";
                }

                if (sfyx == 1)
                {
                    string tmpmsg = "";
                    bool tmpcode = GetYhhh(row["银行卡号"], out tmpmsg);
                    if (!tmpcode)
                    {
                        sfyx = 3;
                        errmsg = "该银行卡无法发放：" + tmpmsg;
                    }
                    else
                        row["yhhh"] = tmpmsg;
                }
                if (row.Keys.Contains("sfyx"))
                    row["sfyx"] = sfyx.ToString();
                else
                    row.Add("sfyx", sfyx.ToString());
                if (row.Keys.Contains("errmsg"))
                    row["errmsg"] = errmsg;
                else
                    row.Add("errmsg", errmsg);
            }
        }
        /// <summary>
        /// 校验导入的工资册中无效的内容
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string CheckInvalidPayItem(ref VTransPayWgryReqSetPayRollItem[] datas)
        {
            StringBuilder sbret = new StringBuilder();
            foreach (VTransPayWgryReqSetPayRollItem row in datas)
            {
                row.name = row.name.GetValidPayString();
                row.phone = row.phone.GetValidPayString();
                row.idnumber = CryptFun.LrEncode(CryptFun.LrDecode(row.idnumber).GetValidPayString());
                row.cardnumber = CryptFun.LrEncode(CryptFun.LrDecode(row.cardnumber)).GetValidPayString();
                row.paysum = row.paysum.GetValidPayString();
                string decodeidnumber = CryptFun.LrDecode(row.idnumber);
                string decodecardnumber = CryptFun.LrDecode(row.cardnumber);
                
                if (string.IsNullOrEmpty(row.name) || string.IsNullOrEmpty(decodeidnumber) ||
                    string.IsNullOrEmpty(decodecardnumber) || string.IsNullOrEmpty(row.phone) || string.IsNullOrEmpty(row.paysum))
                {
                    sbret.Append(row.name+"信息不全。");
                }
                else if (!IDCardValidation.CheckIDCard(decodeidnumber))
                {
                    sbret.Append(row.name+"身份证号码无效。");
                }
                
            }
            return sbret.ToString();
        }

        /// <summary>
        /// 删除务工人员银行卡
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]
        public bool DeleteWgryCard(string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt1 = CommonDao.GetDataTable("select rybh,bkzt,yhyhid,yhpt from I_S_WGRY_YHZH where recid='" + recid + "'");
                if (dt1.Count() == 0)
                {
                    msg = "记录不存在";
                    return ret;
                }
                IDictionary<string, string> row = dt1[0];
                string yhyhid = row["yhyhid"];
                string rybh = row["rybh"];
                string yhpt = row["yhpt"];
                int bkzt = row["bkzt"].GetSafeInt();
                dt1 = CommonDao.GetDataTable("select count(*) as t1 from I_S_WGRY_YHZH where rybh='" + rybh + "'");
                int totalcount = dt1[0]["t1"].GetSafeInt();
                if (bkzt == 1)
                {
                    CL_UnBindPersonalSettlement info = new CL_UnBindPersonalSettlement()
                    {
                        UserId = yhyhid
                    };
                    if (UnBindPersonalSettlement(info, "system", yhpt, out msg))
                    {
                        VTransPayRespBase resp = ParseResponse<VTransPayRespBase>(msg);
                        if (!resp.IsSucceed)
                        {
                            msg = "解绑失败：" + resp.message;
                            return ret;
                        }
                    }
                    else
                    {
                        msg = "调用解绑函数失败";
                        return ret;
                    }
                }
                msg = "";
                CommonDao.ExecCommandOpenSession("delete from I_S_WGRY_YHZH where recid='" + recid + "'", CommandType.Text);
                if (totalcount<=1)
                    CommonDao.ExecCommandOpenSession("delete from i_m_wgry where rybh='" + rybh + "'", CommandType.Text);
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取劳务公司列表
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPayCompanys(out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                ret = CommonDao.GetDataTable("select  * from i_m_qy where lxbh='31' order by qymc");
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取劳务公司工程子账号
        /// </summary>
        /// <param name="paycompayids"></param>
        /// <returns></returns>
        public IList<VTransPayRespGetUserInfoDataItem> GetSubAccuonts(string usercode, string paycompayids, out string msg)
        {
            IList<VTransPayRespGetUserInfoDataItem> ret = new List<VTransPayRespGetUserInfoDataItem>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> projectDatas = CommonDao.GetDataTable("select a.zfqymc,a.yhyhid,b.gcmc,a.yhpt from I_S_GC_ZFDW a inner join i_m_gc b on a.gcbh=b.gcbh where a.yhyhid is not null and a.yhyhid<>'' and a.zfqybh in (" + paycompayids.FormatSQLInStr() + ")");
                IList<string> yhpts = new List<string>();
                foreach (IDictionary<string,string> row in projectDatas)
                {
                    string yhpt = row["yhpt"];
                    var findYhpts = yhpts.Where(e => e.Equals(yhpt, StringComparison.OrdinalIgnoreCase));
                    if (findYhpts.Count() == 0)
                        yhpts.Add(yhpt);
                }
                foreach (string yhpt in yhpts)
                {
                    IList<string> yhyhids = new List<string>();
                    foreach (IDictionary<string, string> row in projectDatas)
                        if (row["yhpt"].Equals(yhpt, StringComparison.OrdinalIgnoreCase))
                            yhyhids.Add(row["yhyhid"]);
                    CL_GetUserInfos reqInfo = new CL_GetUserInfos();
                    reqInfo.UserIds.AddRange(yhyhids);
                    if (this.GetUserInfos(reqInfo, usercode, yhpt, out msg))
                    {
                        VTransPayRespGetUserInfo respInfo = ParseResponse<VTransPayRespGetUserInfo>(msg);
                        if (respInfo.IsSucceed)
                        {
                            foreach (VTransPayRespGetUserInfoDataItem item in respInfo.data)
                            {
                                var findItems = projectDatas.Where(e => e["yhyhid"] == item.UserId);
                                if (findItems.Count() > 0)
                                    item.TradeMemBerName = findItems.ElementAt(0)["gcmc"];
                            }
                            msg = "";
                            ((List<VTransPayRespGetUserInfoDataItem>)ret).AddRange(respInfo.data.ToList());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取所有管理人员，及公司所属管理人员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public IList<IDictionary<string, string>> GetManagePersons(string usercode, out string msg)
		{
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			msg = "";
			try
			{
				list = this.CommonDao.GetDataTable("select qybh from i_m_qyzh where yhzh='" + usercode + "'");
				if (list.Count == 0)
				{
					msg = "当前用户无效";
					return list;
				}
				string qybh = list[0]["qybh"];
				string sql = "select rybh,ryxm,(select count(*) from i_s_qy_ry where i_s_qy_ry.qybh='"+qybh+"' and i_s_qy_ry.rybh=i_m_ry.rybh and gw='11') as c11,(select count(*) from i_s_qy_ry where i_s_qy_ry.qybh='"+qybh+"' and i_s_qy_ry.rybh=i_m_ry.rybh and gw='12') as c12 from i_m_ry where lxbh='14' order by ryxm";
				list = this.CommonDao.GetDataTable(sql);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}

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
        public IList<VTransPayRespGetRechargeListItem> GetRechargeList(string usercode, string hbzh, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<VTransPayRespGetRechargeListItem> ret = new List<VTransPayRespGetRechargeListItem>();
            totalcount = 0;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> yhptList = CommonDao.GetDataTable("select * from i_s_gc_zfdw where yhyhid='" + hbzh + "'");

                if (yhptList.Count() == 0)
                {
                    msg = "找不到子账号记录";
                    return ret;
                }
                string yhpt = yhptList[0]["yhpt"];
                CL_RechargeList info = new CL_RechargeList();
                info.StartDate = new DateTime(2018, 1, 1);
                info.EndDate = DateTime.Now;
                info.UserIds = new List<string>();
                info.UserIds.Add(hbzh);
                info.PageSize = pagesize;
                info.PageIndex = pageindex;
                info.IsAsc = false;
                if (this.GetRechargeList(info, usercode, yhpt, out msg))
                {
                    VTransPayRespPageBase<VTransPayRespGetRechargeListItem> resp = this.ParseResponse<VTransPayRespPageBase<VTransPayRespGetRechargeListItem>>(msg);
                    if (!resp.IsSucceed)
                        msg = resp.message;
                    else
                    {
                        msg = "";
                        totalcount = resp.data.total;
                        ret = resp.data.data.ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion
                
        #region 银行接口调用
        /// <summary>
        /// 调用银行接口
        /// </summary>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		private bool Call(string action, string data, string userid, string yhpt,  out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
                IDictionary<string, string> yhptRow = GetCacheYhpt(yhpt);
                if (yhptRow == null || yhptRow.Count == 0)
                {
                    msg = "银行平台"+yhpt+"无效";
                    return result;
                }
                string url = yhptRow["url"];
                string key = yhptRow["csmy"];
				SysLog4.WriteLog("====开始调用：url:"+url+"," + action + "，参数:" + data);
				WebClient webClient = new WebClient();
				webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				webClient.Encoding = Encoding.UTF8;
				webClient.Headers.Add("ip", SysEnvironment.IP);
				webClient.Headers.Add("paramstring", Convert.ToBase64String(Encoding.UTF8.GetBytes(userid)));
				webClient.Headers.Add("bankid",yhpt);
				string text = this.ConvertDateTimeInt().Substring(0, 10);
				string text2 = Guid.NewGuid().ToString();
				string value = MD5Util.StringToMD5Hash(text + text2 + key, true);
				webClient.Headers.Add("timestamp", text);
				webClient.Headers.Add("guid", text2);
				webClient.Headers.Add("sign", value);
				string data2 = "action=" + action + "&data=" + CryptFun.Encode(data, "8mfcxf95", "bank0qlz").UrlEncode(null);
				msg = webClient.UploadString(url, data2);
				SysLog4.WriteLog("====结束调用：" + action + "，返回:" + msg);
				result = true;
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RegisterUser(CL_RegisterUser user, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(user);
				result = this.Call("RegisterUser", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

		/// <summary>
        /// 绑定卡号
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool BindSettlement(CL_BindSettlement info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("BindSettlement", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 人员卡号解绑
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UnBindPersonalSettlement(CL_UnBindPersonalSettlement info, string userid, string yhpt, out string msg)
        {
            bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("UnBindPersonalSettlement", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
        }

        /// <summary>
        /// 人员卡号绑定，会自动提现
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool BindPersonalSettlement(CL_BindPersonalSettlement info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("BindPersonalSettlement", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

		/// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetUserInfos(CL_GetUserInfos infos, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(infos);
				result = this.Call("GetUserInfos", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 银行支付
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool Pay(CL_Pay info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("Pay", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool QueryUser(CL_QueryUser info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
                info.PageSize = 100;
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("QueryUser", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 修改银行预留手机号码验证码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetChangePhoneCode(CL_GetChangePhoneCode info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("GetChangePhoneCode", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 修改银行预留电话
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool ChangePhone(CL_ChangePhone info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("ChangePhone", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 预提现
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool PrePay(CL_PrePay info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("PrePay", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取支付列表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetPayList(CL_GetPayList info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("GetPayList", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 获取提现验证码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetWithDrawCode(CL_GetWithDrawCode info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("GetWithDrawCode", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool WithDraw(CL_WithDraw info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("WithDraw", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}

        /// <summary>
        /// 重新提交提现
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool ReSubmitDraw(CL_ReSubmitDraw info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("ReSubmitDraw", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 获取发放结果
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetPaymentReturnList(CL_GetPayReturnList info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("GetPaymentReturnList", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        /// <summary>
        /// 取消绑卡
        /// </summary>
        /// <param name="rykbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UnbindCard(string rykbh, out string msg)
        {
            msg = "";
            bool result = false;
            try
            {
                IList<IDictionary<string, string>> cardinfos = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + rykbh + "' ");
                if (cardinfos.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "人员卡信息不存在";
                    return result;
                }
                IDictionary<string, string> cardinfo = cardinfos[0];
                CL_UnBindPersonalSettlement info = new CL_UnBindPersonalSettlement()
                {
                    UserId = cardinfo["yhyhid"]
                };
                string yhpt = cardinfo["yhpt"];
                if (UnBindPersonalSettlement(info, "system", yhpt, out msg))
                {
                    VTransPayRespBase resp = ParseResponse<VTransPayRespBase>(msg);
                    if (resp.IsSucceed)
                    {
                        CommonDao.ExecCommandOpenSession("update I_S_WGRY_YHZH set bkzt=2 where recid='" + rykbh + "'", CommandType.Text);
                        msg = resp.message;
                    }
                    else
                        msg = "解绑失败："+resp.message;
                }
                else
                    msg = "调用错误";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return msg == "";
        }
        /// <summary>
        /// 绑卡
        /// </summary>
        /// <param name="rykbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool BindCard(string rykbh, out string msg)
        {
            msg = "";
            bool result = false;
            try
            {
                IList<IDictionary<string, string>> cardinfos = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + rykbh + "' ");
                if (cardinfos.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "人员卡信息不存在";
                    return result;
                }
                IDictionary<string, string> cardinfo = cardinfos[0];
                CL_BindPersonalSettlement info = new CL_BindPersonalSettlement();
                info.SettleAccountName = cardinfo["yhzhmc"]; ;
                info.UserId = cardinfo["yhyhid"];
					info.SettleAccount = cardinfo["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
					info.PayBank = cardinfo["yhhh"];

                if (BindPersonalSettlement(info, "system",cardinfo["yhpt"], out msg))
                {
                    VTransPayRespBindSettlement bindresp = this.ParseResponse<VTransPayRespBindSettlement>(msg);
                    if (bindresp.IsSucceed)
                    {
                        CommonDao.ExecCommandOpenSession("update I_S_WGRY_YHZH set bkzt=1 where recid='" + rykbh + "'", CommandType.Text);
                        msg = bindresp.message;
                    }
                    else
                        msg = "绑卡错误："+bindresp.message;
                }
                else
                    msg = "调用错误";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return msg == "";
        }
        /// <summary>
        /// 取消发放
        /// </summary>
        /// <param name="ffcbid"></param>
        /// <param name="usercode"></param>
        /// <param name="realnam"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CancelPayItem(string ffcbid, string usercode, string realnam, out string msg)
		{
			msg = "";
			bool result = false;
			try
			{
				IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable("select * from i_s_pay_xq where recid='" + ffcbid + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放人员记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary = dataTable[0];
				IList<IDictionary<string, string>> dataTable2 = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + dictionary["ffid"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放主记录不存在";
					return result;
				}
				IDictionary<string, string> dictionary2 = dataTable2[0];
				IList<IDictionary<string, string>> dataTable3 = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + dictionary["rykbh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员卡信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary3 = dataTable3[0];
				IList<IDictionary<string, string>> dataTable4 = this.CommonDao.GetDataTable("select * from I_M_WGRY where rybh='" + dictionary3["rybh"] + "' ");
				if (dataTable.Count<IDictionary<string, string>>() == 0)
				{
					msg = "人员信息不存在";
					return result;
				}
				IDictionary<string, string> dictionary4 = dataTable4[0];
				string settleAccount = dictionary3["jskh"].DecodeDes("8zzsjd95", "fcb95eze");
                /*
				if (!BankPayDetialStaus.CanCancel(dictionary["zt"].GetSafeInt(0)))
				{
					msg = "当前记录无法取消";
					return result;
				}*/
				string tradeCode = dictionary2["tradecode"];
				string yhyhid = dictionary3["yhyhid"];
				CL_GetPayList info = new CL_GetPayList
				{
					TradeCode = tradeCode
				};
				if (this.GetPayList(info, usercode, dictionary3["yhpt"], out msg))
				{
					VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
					if (vtransPayRespGetPayList.IsSucceed)
					{
						msg = "";
						IEnumerable<VTransPayRespGetPayListDataItem> source = from e in vtransPayRespGetPayList.data
						where e.ToUserId.Equals(yhyhid)
						select e;
						if (source.Count<VTransPayRespGetPayListDataItem>() == 0)
						{
							msg = "获取发放记录失败";
						}
						else
						{
							string orderId = source.ElementAt(0).OrderId;
                            CL_ReSubmitDraw info2 = new CL_ReSubmitDraw
                            {
                                OrderId = orderId,
                                SettleAccount = settleAccount,
                                IsCancel = "1",
                                IsRevoke = "1"
							};
							if (this.ReSubmitDraw(info2, usercode, dictionary3["yhpt"], out msg))
							{
								if (this.ParseResponse<VTransPayRespBase>(msg).IsSucceed)
								{
									string text = "update I_S_PAY_XQ set zt="+BankPayDetialStaus.Cancel.ToString()+",ffbz=''"+
										" where recid='"+
										ffcbid+
										"'";
									this.CommonDao.ExecCommandOpenSession(text, CommandType.Text, null, -1);
                                    text = "update I_M_PAY set zt=" +
                                        5 +
                                        ",wgryxx=0 where recid='" +
                                        dictionary2["recid"] +
                                        "'";
									this.CommonDao.ExecCommandOpenSession(text, CommandType.Text, null, -1);
									msg = "";
								}
								else
								{
									msg = "取消发放失败";
								}
							}
							else
							{
								msg = "取消发放失败";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return msg == "";
		}
        /// <summary>
        /// 直接取消银行发放（用于本地人员无记录）
        /// </summary>
        /// <param name="ffcbid"></param>
        /// <param name="usercode"></param>
        /// <param name="realnam"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CancelBankPayItem(string payid, string orderid, string usercode, string realnam, out string msg)
		{
			msg = "";
			bool result = false;
			try
			{
				IList<IDictionary<string, string>> dtPay = this.CommonDao.GetDataTable("select tradecode,yhpt from i_m_pay where recid='" + payid+ "' ");
				if (dtPay.Count<IDictionary<string, string>>() == 0)
				{
					msg = "发放主记录不存在";
					return result;
				}
				IDictionary<string, string> payRow = dtPay[0];
                string tradecode = payRow["tradecode"];
                string yhpt = payRow["yhpt"];
                if (string.IsNullOrEmpty(tradecode))
                {
                    msg = "主交易代码不能为空";
                    return result;
                }
				CL_GetPayList info = new CL_GetPayList
				{
					TradeCode = tradecode
				};
				if (this.GetPayList(info, usercode, yhpt, out msg))
				{
					VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
					if (vtransPayRespGetPayList.IsSucceed)
					{
						msg = "";
                        var findItems = vtransPayRespGetPayList.data.Where(e => e.OrderId == orderid);
						if (findItems.Count() == 0)
						{
							msg = "获取发放记录失败";
						}
						else
						{
                            CL_ReSubmitDraw info2 = new CL_ReSubmitDraw
                            {
                                OrderId = orderid,
                                SettleAccount = findItems.ElementAt(0).SettleAccount,
                                IsCancel = "1",
                                IsRevoke = "1"
							};
							if (this.ReSubmitDraw(info2, usercode, yhpt, out msg))
							{
								if (this.ParseResponse<VTransPayRespBase>(msg).IsSucceed)
								{
                                    string sql = "update I_M_PAY set zt=" +PayStatus.InWithdraw+",wgryxx=0 where recid='" +payid+ "'";
									CommonDao.ExecCommandOpenSession(sql, CommandType.Text, null, -1);
									msg = "";
								}
								else
								{
									msg = "取消发放失败";
								}
							}
							else
							{
								msg = "取消发放失败";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return msg == "";
		}
        
        /// <summary>
        /// 月发放统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetMonthPayStatistic(int year, int month, string usercode, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                string dt1 = year + "-" + month + "-1";
                month++;
                if (month==13)
                {
                    year++;
                    month -= 12;
                }
                string dt2 = year + "-" + month + "-1";
                ret = CommonDao.GetDataTable("select sum(yfze) as t1,sum(sfze) as t2,day(shsj) as rq from i_m_pay where zt in (3,4) and shsj>convert(datetime,'" + dt1 + "') and shsj<convert(datetime,'" + dt2 + "') and (zfqybh=(select qybh from i_m_qyzh where yhzh='"+usercode+"') or zfqybh=(select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='"+usercode+"'))) group by day(shsj) order by day(shsj)");
            }catch(Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取人员银行信息
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <returns></returns>
        public IList<VTransPayRespQueryUserDataItem> GetUserBankInfos(string sfzhm, string usercode, out string msg)
        {
            List<VTransPayRespQueryUserDataItem> ret = new List<VTransPayRespQueryUserDataItem>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> zfptLists = CommonDao.GetDataTable("select YHBH from H_YHPT");
                foreach (IDictionary<string, string> row in zfptLists)
                {
                    CL_QueryUser reqinfo = new CL_QueryUser();
                    reqinfo.PapersCode = sfzhm;
                    string resp = "";
                    if (this.QueryUser(reqinfo, usercode, row["yhbh"], out resp))
                    {
                        VTransPayRespQueryUser respobj = this.ParseResponse<VTransPayRespQueryUser>(resp);
                        if (respobj.IsSucceed)
                        {
                            ret.AddRange(respobj.data.data);
                        }
                        else
                            msg = "返回失败：" + respobj.message;
                    }
                    else
                        msg = "调用失败";
                }
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 查询充值记录
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userid"></param>
        /// <param name="yhpt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetRechargeList(CL_RechargeList info, string userid, string yhpt, out string msg)
		{
			bool result = false;
			msg = "";
			try
			{
				string data = new JavaScriptSerializer().Serialize(info);
				result = this.Call("RechargeList", data, userid, yhpt, out msg);
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return result;
		}
        #endregion
        		
        #region 其他基础函数
        /// <summary>
        /// 日期转换成整数
        /// </summary>
        /// <returns></returns>
        private string ConvertDateTimeInt()
		{
			DateTime now = DateTime.Now;
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			string text = (now - d).TotalSeconds.ToString();
			text = text.Replace(".", "");
			return text.Substring(0, text.Length - 2);
		}

        /// <summary>
        /// 银行接口返回解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
		private T ParseResponse<T>(string resp)
		{
			return new JavaScriptSerializer().Deserialize<T>(resp);
		}
        
        /// <summary>
        /// 根据银行发放结果和本地数据，合成结果
        /// </summary>
        /// <param name="bankData"></param>
        /// <param name="localData"></param>
        /// <returns></returns>
		private IDictionary<string, string> GetResponsePayDetailItem(VTransPayRespGetPayListDataItem bankData, IDictionary<string, string> localData)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			if (bankData == null && localData == null)
			{
				return dictionary;
			}
			try
			{
				string text = "";
				if (bankData != null)
				{
					text = ((bankData.TradeState == "0") ? bankData.FaildCode : "");
				}
				dictionary.Add("id", (localData != null) ? localData["r_recid"] : "");
				dictionary.Add("ryxm", (localData != null) ? localData["ryxm"] : "");
				dictionary.Add("sjhm", (localData != null) ? localData["sjhm"] : "");
				dictionary.Add("sfzhm", (localData != null) ? localData["sfzhm"].DecodeDes("8zzsjd95", "fcb95eze") : "");
				dictionary.Add("jskh", (bankData != null) ? bankData.SettleAccount : localData["jskh"].DecodeDes("8zzsjd95", "fcb95eze"));
				dictionary.Add("yfje", (bankData != null) ? bankData.Amount : localData["yfje"]);
				dictionary.Add("bz", (localData != null) ? localData["r_bz"]:"");
				dictionary.Add("bkzt", (localData != null) ? localData["bkzt"] : "");
				dictionary.Add("sfje", (bankData != null) ? bankData.RealAmount : localData["sfje"]);
				dictionary.Add("zt", (bankData != null) ? bankData.TradeState : localData["zt"]);
				dictionary.Add("ffsj", (bankData != null) ? bankData.TradeTime : localData["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss"));
                if (dictionary["ffsj"] == "")
                    dictionary.Add("ffrq", "");
                else
                    dictionary.Add("ffrq", dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd"));
				dictionary.Add("cwdm", (bankData != null) ? text : localData["ffbz"]);
                if (bankData != null)
                    dictionary.Add("cwxx", bankData.Remark1);
                else
        				dictionary.Add("cwxx", VTransPayRespGetPayListDataItem.GetFaildDetail(dictionary["cwdm"]));
				dictionary.Add("kkcwxx", (localData != null) ? localData["kkcwxx"] : "");
				dictionary.Add("hasbankdata", (bankData == null) ? "0" : "1");
				dictionary.Add("haslocaldata", (localData == null) ? "0" : "1");
				int num = 0;
				string value = "";
				if (bankData == null && localData["zt"].GetSafeInt(0) != 99)
				{
					num++;
					value = "银行平台无记录";
				}
				if (localData == null)
				{
					num += 2;
					value = "本地无记录";
				}
				dictionary.Add("yczt", num.ToString());
				dictionary.Add("ycztms", value);
                if (bankData != null)
                    dictionary.Add("orderid", bankData.OrderId);
                else
                    dictionary.Add("orderid", "");
			}
			catch (Exception se)
			{
				SysLog4.WriteLog(se);
			}
			return dictionary;
		}
        /// <summary>
        /// 从阿里平台查询结果
        /// </summary>
        /// <param name="yhkh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public bool GetBankNameByAli(string yhkh, out string msg)
		{
			msg = "";
			bool result = false;
			try
			{
				if (MyHttp.PostAli("https://ccdcapi.alipay.com/validateAndCacheCardInfo.json", new Dictionary<string, string>
				{
					{
						"input_charset",
						"utf-8"
					},
					{
						"cardNo",
						yhkh
					},
					{
						"cardBinCheck",
						"true"
					}
				}, out msg))
				{
					VTransPayAliRespBankInfo vtransPayAliRespBankInfo = new JavaScriptSerializer().Deserialize<VTransPayAliRespBankInfo>(msg);
					if (vtransPayAliRespBankInfo.validated)
					{
						if (vtransPayAliRespBankInfo.cardType == "DC")
						{
							string bank = vtransPayAliRespBankInfo.bank;
							string sql = "select b.yhhh from H_ZFBYHMC a inner join H_YHHH b on a.yhmc=b.yhmc where a.carddm='" + bank + "'";
							IList<IDictionary<string, string>> dataTable = this.CommonDao.GetDataTable(sql);
							if (dataTable.Count > 0)
							{
								result = true;
								msg = dataTable[0]["yhhh"];
							}
							else
							{
								msg = "没有查到所属银行";
							}
						}
						else
						{
							msg = "该卡不是储蓄卡,请更换";
						}
					}
					else
					{
						msg = "获取银行名称失败";
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				msg = "获取银行行号失败";
			}
			return result;
		}
        /// <summary>
        /// 根据姓名，身份证号码，银行卡号 获取银行用户信息。这三个信息在银行端只有一条记录（以前的记录可能有多条，先获取绑卡成功的）
        /// </summary>
        /// <param name="xm"></param>
        /// <param name="sfzhm"></param>
        /// <param name="yhkh"></param>
        /// <returns></returns>
        private VTransPayRespQueryUserDataItem GetWgryAccountInfo(string xm, string sfzhm, string yhkh, string yhpt)
        {
            VTransPayRespQueryUserDataItem ret = null;

            CL_QueryUser queryUserInfo = new CL_QueryUser();
            queryUserInfo.PapersCode = sfzhm;
            string resp = "";
            if (this.QueryUser(queryUserInfo, "system", yhpt, out resp))
            {
                VTransPayRespQueryUser respobj = this.ParseResponse<VTransPayRespQueryUser>(resp);
                if (respobj.IsSucceed)
                {
                    var findCards = respobj.data.data.Where(e => e.SettleAccountName.Equals(xm) && e.SettleAccount.Equals(yhkh) && e.PapersCode.Equals(sfzhm));
                    if (findCards.Count() > 0)
                    {
                        var findBindSucceedCards = findCards.Where(e => e.BindSucceed);
                        if (findBindSucceedCards.Count() > 0)
                            ret = findBindSucceedCards.ElementAt(0);
                        else
                            ret = findCards.ElementAt(0);
                    }
                }
            }
            return ret;
        }


        #endregion
        
        #region 变量
        private const string LRLB_KEY_64 = "8zzsjd95";
		private const string LRLB_IV_64 = "fcb95eze";
		private const string KEY_64 = "8mfcxf95";
		private const string IV_64 = "bank0qlz";

        private const string DefaultBankNumber = "104100000004";
        #endregion

        #region 临时测试函数
        
        
        /// <summary>
        /// 临时函数
        /// </summary>
        /// <param name="ffcbid"></param>
        /// <param name="cardno"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool TempResubmit(string ffcbid, out string msg)
        {
            msg = "";
            bool result = false;
            try
            {
                IList<IDictionary<string, string>> spays = this.CommonDao.GetDataTable("select * from i_s_pay_xq where recid='" + ffcbid + "' ");
                if (spays.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "发放人员记录不存在";
                    return result;
                }
                IDictionary<string, string> spay = spays[0];
                IList<IDictionary<string, string>> mpays = this.CommonDao.GetDataTable("select * from i_m_pay where recid='" + spay["ffid"] + "' ");
                if (mpays.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "发放主记录不存在";
                    return result;
                }
                IDictionary<string, string> mpay = mpays[0];
                IList<IDictionary<string, string>> cardinfos = this.CommonDao.GetDataTable("select * from I_S_WGRY_YHZH where recid='" + spay["rykbh"] + "' ");
                if (cardinfos.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "人员卡信息不存在";
                    return result;
                }
                IDictionary<string, string> cardinfo = cardinfos[0];
                IList<IDictionary<string, string>> ryinfos = this.CommonDao.GetDataTable("select * from I_M_WGRY where rybh='" + cardinfo["rybh"] + "' ");
                if (ryinfos.Count<IDictionary<string, string>>() == 0)
                {
                    msg = "人员信息不存在";
                    return result;
                }
                IDictionary<string, string> ryinfo = ryinfos[0];
                string ryxm = ryinfo["ryxm"];
                string sfzhm = ryinfo["sfzhm"].DecodeDes(LRLB_KEY_64, LRLB_IV_64);
                string encodecardno = cardinfo["jskh"];
                string cardno = encodecardno.DecodeDes(LRLB_KEY_64, LRLB_IV_64);
                SysLog4.WriteLog("12");
                CL_GetPayList info = new CL_GetPayList
                {
                    TradeCode = mpay["tradecode"]
                };
                if (this.GetPayList(info, "system", mpay["yhpt"], out msg))
                {
                    VTransPayRespGetPayList vtransPayRespGetPayList = this.ParseResponse<VTransPayRespGetPayList>(msg);
                    if (vtransPayRespGetPayList.IsSucceed)
                    {
                        msg = "";
                        IEnumerable<VTransPayRespGetPayListDataItem> data = vtransPayRespGetPayList.data;
                        var finddatas = data.Where(e => e.SettleAccount.Equals(cardno));
                        SysLog4.WriteLog("16");
                        if (finddatas.Count() == 0)
                        {
                            msg = "获取发放记录失败";
                        }
                        else
                        {
                            string orderId = finddatas.ElementAt(0).OrderId;
                            CL_ReSubmitDraw cl_ReSubmitDraw = new CL_ReSubmitDraw
                            {
                                OrderId = orderId,
                                SettleAccount = cardno,
                                NewUserId = cardinfo["yhyhid"]
                            };
                            if (this.ReSubmitDraw(cl_ReSubmitDraw, "system",mpay["yhpt"], out msg))
                            {
                                
                                if (this.ParseResponse<VTransPayRespBase>(msg).IsSucceed)
                                {
                                    string sql = "update I_S_PAY_XQ set zt=0,ffbz='' where recid='" + ffcbid + "'";
                                    CommonDao.ExecCommandOpenSession(sql, CommandType.Text);
                                }
                                else
                                {
                                    msg = "重新发放失败";
                                }
                            }
                            else
                            {
                                msg = "重新发放失败";
                            }
                        }
                    }
                    else
                    {
                        msg = "获取发放记录失败";
                    }
                }
                else
                {
                    msg = "获取发放记录失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return msg == "";
        }
        /// <summary>
        /// 获取务工人员卡号信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string,IList<IDictionary<string, string>>> GetWgryCards()
        {
            IList<IDictionary<string, string>> dt1 = CommonDao.GetDataTable("select rybh,ryxm,sfzhm from I_M_WGRY"); 
            IList<IDictionary<string, string>> dt2 = CommonDao.GetDataTable("select recid,rybh,jskh,yhyhid,bkzt,yhzh from I_S_WGRY_YHZH");
            IDictionary<string, IList<IDictionary<string, string>>> ret = new Dictionary<string, IList<IDictionary<string, string>>>();
            ret.Add("m", dt1);
            ret.Add("s", dt2);
            return ret;
        }
        #endregion

        #region 管理多个公司的函数
        /// <summary>
        /// 获取管理得支付企业
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns>企业编号(qybh)，企业名称(qymc)</returns>
        public IList<IDictionary<string, string>> GetManagePayCompanys(string usercode, string gw, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                ret = CommonDao.GetDataTable("select qybh,qymc from i_m_qy where qybh in (select qybh from I_S_QY_RY where gw='" + gw + "' and rybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "')) order by qymc");
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取管理的工程
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="paycompany"></param>
        /// <param name="msg"></param>
        /// <returns>工程编号(gcbh),工程名称(gcmc)</returns>
        public IList<IDictionary<string, string>> GetManageProjects(string usercode, string paycompany, string gw, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                string sql = "select gcbh,gcmc from i_m_gc where gcbh in (select gcbh from I_S_GC_ZFDW where zfqybh in (select qybh from I_S_QY_RY where gw='" + gw + "' and rybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "'))";
                if (!string.IsNullOrEmpty(paycompany))
                    sql += " and qybh='" + paycompany + "' ";
                sql += ") order by gcmc";
                ret = CommonDao.GetDataTable(sql);
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取管理发放记录
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
		public IList<IDictionary<string, string>> GetManagePayHistory(string usercode, string zt, string gcmc, string sgdw, string ffzh, string zfdwbh, 
            string gcbh, string sgdwbh, string bz1, string gzzq1, string gzzq2, string spsj1, string spsj2, int pagesize, 
            int pageindex, out int totalcount, out string msg, out decimal t1, out decimal t2)
		{
			msg = "";
			IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
			totalcount = 0;
            t1 = 0;
            t2 = 0;
			try
			{
                string text = "select a.*,b.lxmc,(select count(*) from i_s_pay_fj x where x.ffid=a.recid and lx='2') as fj2,(select count(*) from i_s_pay_xq x where x.ffid=a.recid ) as rysl from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh  where ";
                string where = " (a.zfqybh in (select qybh from I_S_QY_RY where gw='11' and rybh=(select qybh from i_m_qyzh where yhzh='" + usercode + "')) ";
                if (!string.IsNullOrEmpty(zfdwbh))
                    where += " and a.zfqybh='"+zfdwbh+"'";
                where += ") ";
				if (!string.IsNullOrEmpty(gcmc))
				{
					where = where + " and a.gcmc like '%" + gcmc + "%' ";
				}
				if (!string.IsNullOrEmpty(sgdw))
				{
					where = where + " and a.qymc like '%" + sgdw + "%' ";
				}
				if (!string.IsNullOrEmpty(ffzh))
				{
					where = where + " and a.ffzh like '%" + ffzh + "%' ";
				}
				if (!string.IsNullOrEmpty(zt))
				{
					where = where + " and a.zt in (" + zt.FormatSQLInStr() + ")";
				}
				if (!string.IsNullOrEmpty(gcbh))
				{
					where = where + " and a.gcbh='" + gcbh + "' ";
				}
				if (!string.IsNullOrEmpty(sgdwbh))
				{
					where = where + " and a.qybh='" + sgdwbh + "' ";
				}
				if (!string.IsNullOrEmpty(bz1))
				{
					where = where + " and a.bz1 like '%" + bz1 + "%' ";
				}
				if (!string.IsNullOrEmpty(gzzq1))
				{
					string[] array = gzzq1.Split(new char[]
					{
						'-'
					});
					int safeInt = array[0].GetSafeInt(0);
					int safeInt2 = array[1].GetSafeInt(0);
                    where += " and (a.ffnf>" + safeInt + " or a.ffnf=" + safeInt + " and a.ffyf>=" + safeInt2 + ")";
				}
				if (!string.IsNullOrEmpty(gzzq2))
				{
					string[] array2 = gzzq2.Split(new char[]
					{
						'-'
					});
					int safeInt3 = array2[0].GetSafeInt(0);
					int safeInt4 = array2[1].GetSafeInt(0);
                    where += " and (a.ffnf<" + safeInt3 + " or a.ffnf=" + safeInt3 + " and a.ffyf<=" + safeInt4 + ")";
				}
				if (!string.IsNullOrEmpty(spsj1))
				{
					where = where + " and (a.shsj is not null and a.shsj>=convert(datetime,'" + spsj1 + "')) ";
				}
				if (!string.IsNullOrEmpty(spsj2))
				{
					where = where + " and (a.shsj is not null and a.shsj<=convert(datetime,'" + spsj2 + " 23:59:59')) ";
				}
				text += where + " order by a.cjsj desc";
				if (pagesize > 0)
				{
					list = this.CommonDao.GetPageData(text, pagesize, pageindex, out totalcount);
				}
				else
				{
					list = this.CommonDao.GetDataTable(text);
					totalcount = list.Count;
				}
                IList<IDictionary<string,string>> dt1 = CommonDao.GetDataTable("select sum(a.yfze) as s1, sum(a.sfze) as s2 from i_m_pay a left outer join h_fflx b on a.fflx=b.lxbh where " + where);
                t1 = dt1[0]["s1"].GetSafeDecimal();
                t2 = dt1[0]["s2"].GetSafeDecimal();

                List<string> yhpts = new List<string>();
                foreach (IDictionary<string, string> dictionary in list)
				{
					if (dictionary["cjsj"] != "")
					{
						dictionary["cjsj"] = dictionary["cjsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["shsj"] != "")
					{
						dictionary["shsj"] = dictionary["shsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					else if (dictionary["shsj0"] != "")
					{
						dictionary["shsj"] = dictionary["shsj0"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					if (dictionary["ffsj"] != "")
					{
						dictionary["ffsj"] = dictionary["ffsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm");
					}
					dictionary["ztms"] = PayStatus.GetDesc(dictionary["zt"].GetSafeInt(0));
                    dictionary["zhye"] = "";

                    var findYhpts = yhpts.Where(e => e.Equals(dictionary["yhpt"], StringComparison.OrdinalIgnoreCase));
                    if (findYhpts.Count() == 0)
                        yhpts.Add(dictionary["yhpt"]);
				}

                foreach (string yhpt in yhpts)
                {
                    List<string> list2 = new List<string>();
                    foreach (IDictionary<string, string> dictionary in list)
                    {
                        if (dictionary["yhpt"].Equals(yhpt, StringComparison.OrdinalIgnoreCase))
                            list2.Add(dictionary["ffzh"]);
                    }
                    CL_GetUserInfos cl_GetUserInfos = new CL_GetUserInfos();
                    cl_GetUserInfos.UserIds.AddRange(list2);
                    if (this.GetUserInfos(cl_GetUserInfos, usercode, yhpt, out msg))
                    {
                        VTransPayRespGetUserInfo vtransPayRespGetUserInfo = this.ParseResponse<VTransPayRespGetUserInfo>(msg);
                        if (vtransPayRespGetUserInfo.IsSucceed)
                        {
                            msg = "";
                            using (IEnumerator<IDictionary<string, string>> enumerator = list.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    IDictionary<string, string> row = enumerator.Current;
                                    VTransPayRespGetUserInfoDataItem vtransPayRespGetUserInfoDataItem = (from e in vtransPayRespGetUserInfo.data
                                                                                                         where e.UserId.Equals(row["ffzh"])
                                                                                                         select e).First<VTransPayRespGetUserInfoDataItem>();
                                    if (vtransPayRespGetUserInfoDataItem != null)
                                    {
                                        row["zhye"] = vtransPayRespGetUserInfoDataItem.AccountBalance.ToString();
                                    }
                                }
                                //goto IL_4CE;
                            }
                        }
                        //msg = vtransPayRespGetUserInfo.message;
                    }
                    //IL_4CE:;
                }
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
				msg = ex.Message;
			}
			return list;
		}
        #endregion

        #region 缓存
        protected const string KEY_SESSION_PREFIX = "PayService_Session_";
        protected const string CACHE_KEY_YHPT = "YHPT";
        IDictionary<string,string> GetCacheYhpt(string yhbh)
        {
            IDictionary<string,string> ret = null;
            try
            {
                ret = HttpRuntime.Cache.Get(KEY_SESSION_PREFIX + CACHE_KEY_YHPT+"_"+yhbh) as IDictionary<string,string>;
                if (ret == null || ret.Count == 0)
                {
                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select * from h_yhpt where yhbh='" + yhbh + "'");

                    if (dt.Count >0)
                    {
                        ret = dt[0];
                        HttpRuntime.Cache.Insert(KEY_SESSION_PREFIX + CACHE_KEY_YHPT+"_"+yhbh, ret);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                throw ex;
            }
            return ret;
        }
        #endregion
    }
}
