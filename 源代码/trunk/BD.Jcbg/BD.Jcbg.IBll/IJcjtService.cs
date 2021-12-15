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
    public interface IJcjtService
    {
        /// <summary>
        /// 获取全部检测机构资质和试验项目
        /// </summary>
        /// <returns></returns>
        IList<JcjtJcjgZZ> GetJcjgZZ();

        /// <summary>
        /// 获取某检测机构的资质和试验项目
        /// </summary>
        /// <returns></returns>
        IList<JcjtJcjgZZ> GetJcjgZZxx_Byqybh(string qybh,int status=0);

        /// <summary>
        /// 获取检测机构资质及分类
        /// </summary>
        /// <returns></returns>
        IList<JcjtJcjgZZ> GetJcjgZZFL();

        /// <summary>
        /// 初始化检测机构
        /// </summary>
        /// <returns></returns>
        bool InitJcjg(JcjtInitJcjg obj, out CreateZh obj_zh);


        bool SaveJcjgXX(string qybh, JcjtInitJcjg obj_jg, string Json);

        bool CreateJcjg(JcjtInitJcjg obj_jg, string yhzh);
        /// <summary>
        /// 初始化检测机构资质
        /// </summary>
        /// <returns></returns>
        bool InitQyzz(string qybh, string qymc);
        /// <summary>
        /// 获取检测机构编号模式方式
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJcbhmsfs();
        /// <summary>
        /// 设置试验项目编号代码
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="syxmmc"></param>
        /// <param name="bh1">登记单编号代码</param>
        /// <param name="bh2">委托单编号代码</param>
        /// <param name="bh3">试验编号代码</param>
        /// <param name="bh4">试验报告单编号代码</param>
        /// <returns></returns>
        bool SetSyxmbhms(string qybh, string syxmbh, string syxmmc, string bh1, string bh2, string bh3, string bh4);
        /// <summary>
        /// 获取试验项目各编号代码
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="syxmbh"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetSyxmbhms(string qybh, string syxmbh);
        /// <summary>
        /// 获取试验项目产品、指标、标准
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <returns></returns>
        IList<IDictionary<string, IList<IDictionary<string, string>>>> GetJcjgSyxmbz(string qybh,string syxmbh);


        IList<IDictionary<string, IList<IDictionary<string, string>>>> GetJcjgSyxmbz_Byqybh(string qybh,string syxmbh);
        /// <summary>
        /// 设置试验项目产品、指标、标准
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="cprows"></param>
        /// <param name="zbrows"></param>
        /// <param name="bzrows"></param>
        /// <returns></returns>
        bool SetJcjgSyxmsj(string qybh, string syxmbh, string cprows, string zbrows, string bzrows);

        /// <summary>
        /// 获取检测机构信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJcjgxx(string qybh);
        /// <summary>
        /// 获取检测机构列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetJcjglb(string key,int pagesize,int pageindex,out int totalCount);
        /// <summary>
        /// 创建新资质
        /// </summary>
        /// <param name="ZZname"></param>
        /// <param name="fl1"></param>
        /// <param name="fl2"></param>
        /// <param name="fl3"></param>
        /// <param name="fl4"></param>
        /// <param name="xsxx"></param>
        /// <returns></returns>
        bool CreateJcZZ(string ZZname, string fl1, string fl2, string fl3, string fl4, string xssx);

        /// <summary>
        /// 创建试验项目
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CreateSyxm(jcjtSyxm obj, out string msg);

        /// <summary>
        /// 获取检测机构配置表信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetH_JCJG(string usercode);

        IList<IDictionary<string, string>> GetH_JCKS(string qybh);

        IList<IDictionary<string, string>> GetH_JCSYRY(string qybh,string ksbh);

        string AddUser(string username, string realname, string password, string sfzh, string xb, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string rolecodelist, string json);
        string CheckUserBySfzh(string sfzhm);

        string ModifyUserStatusByUsercode(string usercode, string userstatus);
        string GetOwnerRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename);
        string GetRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename);
        string GetRoleList(string page, string rows, string usercode, string cpcode, string cpname, string procode, string proname, string rolename, string rolecode);
        string AddRoleInfo(string cpcode, string procode, string rolename, string memo);
        string GetOwnerUserListByRolecode(string page, string rows, string rolecode, string cpcode, string username, string realname);
        string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sjhm, string cpcode, string depcode,string ksbh, string postdm, string procode, string rolecodelist, string clearrole, string sfzhm);
        string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sfsyr, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string procode, string rolecodelist, string clearrole, string sfzhm, string type);

        string ModifyUserRoleByRolecodeAndUsercodeList(string rolecode, string usercodelist);

        string ModifyUserRoleByUsercodeAndRolecodeList(string usercode, string rolecodelist);

        string GetOwnerPowerListByRolecode(string rolecode);
        string SavePowerByRolecode(string rolecode, string menulist);
        string SavePowerByRolecode(string rolecode, string menulist, string butlist);

        string GetPowerListByRolecode(string rolecode);
        string GetUserListByMenucode(string page, string rows, string procode, string cpcode, string menucode);
        string GetRoleListByMenucode(string page, string rows, string procode, string menucode);

        string GetUserListByRolecode(string page, string rows, string rolecode, string cpcode, string realname);
        string GetUserListBySfzh(string sfzhm);

        string GetProcodeAndMenuByUsercode(string usercode);

        /// <summary>
        /// 保存人员的实验项目
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        bool SaveJCRYSYXM(string rybh, string json);

        /// <summary>
        /// 获取检测人员的实验项目
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<JcjtJcjgZZ> GetJCRYZZxx_Byrybh(string rybh, int status = 1);


        /// <summary>
        /// 试验项目报备
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="jsondata"></param>
        /// <param name="status"></param>
        /// <param name="bbid"></param>
        /// <returns></returns>
        bool SetGcsyxmbb(string gcbh, string jsondata, int status, string bbid);
        /// <summary>
        /// 获取试验项目报备
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="bbid"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetGcsyxmbb(string gcbh, string bbid);

        /// <summary>
        /// 获取试验项目报备的内容
        /// </summary>
        /// <param name="bbid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<JcjtJcjgZZ> GetGCBBSyxm(string bbid, int status = 1);
        /// <summary>
        /// 获取试验类别内容
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="jchtbh"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        bool SetSyxmht(string gcbh, string jchtbh, string jsondata);

        /// <summary>
        /// 删除登记单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        bool DeleteDjds(string ids, out string msg);
        /// <summary>
        /// 判断登记单主表m_dj_by 是否存在该合同记录
        /// </summary>
        /// <param name="htrecid"></param>
        /// <returns></returns>
        bool GetDjdht(string htrecid, out string recid);

        /// <summary>
        /// 登记单提交委托
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DJDTjWt(string recids, out string msg);

        /// <summary>
        /// 设置试验科室、试验人
        /// </summary>
        /// <param name="syks"></param>
        /// <param name="syrxm"></param>
        /// <param name="syrbh"></param>
        /// <param name="s_byrecids"></param>
        /// <returns></returns>
        bool SetS_BYsyks(string syks, string syrxm, string syrbh, string s_byrecids, DateTime SYKSSJ, DateTime SYJSSJ, out string msg);
        //获取分配详情
        IList<IDictionary<string, string>> GetS_BYsyks(string s_byrecids, out string msg);

        /// <summary>
        /// 设置人员单位
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="czr"></param>
        /// <param name="czrxm"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetJCRydw(string recids, string czr, string czrxm, out string msg);

        /// <summary>
        /// 辞退检测系统人员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="czr"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CtJCRy(string usercode, string czr, out string msg);

        /// <summary>
        /// 录用业务员
        /// </summary>
        /// <param name="rybhs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetYwy(string rybhs, out string msg);

        /// <summary>
        /// 辞退业务员
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="czr"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CtYwy(string usercode, string czr, out string msg);

        /// <summary>
        /// 获取检测机构或者检测人员的检测机构编号和名称
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> getJcryJCJG(string usercode, out string msg);

        string AddUser(string jcjgbh, string username, string realname, string password, string sfzh, string xb, string sfsyr, string sjhm, string cpcode, string depcode, string ksbh, string postdm, string rolecodelist, string json, string type = "");



        /// <summary>
        /// 检测机构科室
        /// </summary>
        /// <returns></returns>
        bool CreateJcks(string type, string ksbh, string qybh, string ksmc, string ksdz, string lxdh, string ksys, string kszcode, string kszxm, string jsfzrcode, string jsfzrxm, string zlfzrcode, string zlfzrxm);

    }
}
