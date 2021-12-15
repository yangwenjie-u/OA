using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.Service;
using BD.Jcbg.Service.Jc;

namespace BD.Jcbg.Web.Controllers
{
    public class DeleteController : Controller
    {
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
            }
        }
        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                try
                {
                    if (_jcService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcService;
            }
        }
        private static ISxtptService _sxtptService = null;
        private static ISxtptService SxtptService
        {
            get
            {
                if (_sxtptService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _sxtptService = webApplicationContext.GetObject("SxtptService") as ISxtptService;
                }
                return _sxtptService;
            }
        }
        
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除企业，同时删除企业主账号、企业资质
        /// </summary>
        [Authorize]
        public void DeleteIQy()
        {
            bool code = true;
            string msg = "";
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                var result = JcService.DeleteIQy(qybh);

                code = result.success;
                msg = result.msg;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除企业资质
        /// </summary>
        [Authorize]
        public void DeleteISQyQyzz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeString();
                string sql = "select sptg from I_S_QY_QYZZ where zzbh='" + zzbh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt[0]["sptg"].GetSafeBool())
                {
                    code = false;
                    msg = "资质申请已审批，无法删除";
                }
                else
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from I_S_QY_QYZZ where zzbh='" + zzbh + "' and sptg=0 ");
                    code = CommonService.ExecTrans(sqls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除企业资质，不管是否审批
        /// </summary>
        [Authorize]
        public void DeleteISQyQyzzAnyway()
        {
            bool code = true;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_S_QY_QYZZ where zzbh='" + zzbh + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除人员，同时删除人员账号
        /// </summary>
        [Authorize]
        public void DeleteIRy()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();

                IList<IDictionary<string, string>> rycount = CommonService.GetDataTable("select count(*) as c1 from view_gc_ry where rybh='" + rybh + "'");
                if (rycount.Count > 0 && rycount[0]["c1"].GetSafeInt() > 0)
                {
                    msg = "人员已有工程，无法删除";
                    code = false;
                }
                else
                {

                    IList<IDictionary<string, string>> rydt = CommonService.GetDataTable("select zh,sfzhm from i_m_ry where rybh='" + rybh + "'");
                    string zh = "", sfzhm = "";
                    if (rydt.Count > 0)
                    {
                        zh = rydt[0]["zh"];
                        sfzhm = rydt[0]["sfzhm"];
                    }
                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_M_RY where rybh='" + rybh + "' ");
                sqls.Add("delete from I_M_QYZH where qybh='" + rybh + "' and SFQYZZH=0");
                    sqls.Add("delete from i_s_ry_ryzz where rybh='" + rybh + "' ");
                    if (sfzhm != "")
                        sqls.Add("insert into kqjdevicecommand(serial,command,userid) select kqjbh,22,'" + rybh + "' from i_m_kqj ");

                code = CommonService.ExecTrans(sqls);
                    if (zh != "")
                        Remote.UserService.DeleteUser(zh, out msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除试验项目显示分类
        /// </summary>
        [Authorize]
        public void DeletePrmSyxmxsfl()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_SYXMXSFL where RECID='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除试验项目，同时删除试验项目明细
        /// </summary>
        [Authorize]
        public void DeletePrmSyxm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string ssdwbh = Request["ssdwbh"].GetSafeString();
                string syxmbh = Request["syxmbh"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_SYXM where RECID='" + recid + "' ");
                sqls.Add("delete from PR_M_SYXMMX where syxmbh='" + syxmbh + "' and SSDWBH='"+ssdwbh+"'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除基础指标
        /// </summary>
        [Authorize]
        public void DeletePrmZb()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_ZB where RECID='" + recid + "' ");
                sqls.Add("delete from PR_S_CP_ZB where ZBBH='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        [Authorize]
        public void DeletePrmCp()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_CP where RECID='" + recid + "' ");
                sqls.Add("delete from PR_S_CP_ZB where CPBH='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除检测设备
        /// </summary>
        public void DeleteImSb()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_M_SB where RECID='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除资质证书分类
        /// </summary>
        public void DeleteHZzzsfl()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_ZZZSFL where RECID='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除资质证书
        /// </summary>
        public void DeleteHZzzs()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_ZZZS where RECID='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除任命书
        /// </summary>
        public void DeleteISryRyzs()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["RECID"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_S_RY_RYZZ where RECID='" + recid + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除工程区域
        /// </summary>
        public void DeleteHGcqy()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_GCQY where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除地基基础类型
        /// </summary>
        public void DeleteHDjjclx()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_DJJCLX where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除质监站
        /// </summary>
        public void DeleteHZjz()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_ZJZ where ZJZBH=" + recid + " ");
                //sqls.Add("delete from I_S_ZJZ_JCZX where zjzbh='" + recid + "'");
                sqls.Add("delete from I_S_ZJZ_JYJCJG where zjzbh='" + recid + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除工程类型
        /// </summary>
        public void DeleteHGclx()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from H_GCLX where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        /// <summary>
        /// 删除编号模式
        /// </summary>
        public void DeletePrmBhms()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from PR_M_BHMS where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除工程，同时删除从表
        /// </summary>
        [Authorize]
        public void DeleteIMGc()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zt,ssjcjgbh from i_m_gc where gcbh='" + gcbh + "'");

                if (dt.Count == 0)
                {
                    msg = "无效的工程信息";
                }
                else
                {
                    string str = dt[0]["zt"].GetSafeString();
                    if (!GcStatus.CanDelete(str))
                        msg = "工程不允许删除";
                    else
                    {
                        IList<string> sqls = new List<string>();

                        //监督工程ssjcjgbh字段为空,非监督工程ssjcjgbh字段不为空
                        if (string.IsNullOrEmpty(dt[0]["ssjcjgbh"].GetSafeString()))
                        {
                            sqls.Add("update i_m_gc set sjgcbh = '' where sjgcbh = '" + gcbh + "'");
                        }
                        else
                        {
                            int count = CommonService.GetSingleData("select count(1) from i_m_jcht where sfyx = 1 and gcbh = '" + gcbh + "'").GetSafeInt();

                            if (count > 0)
                            {
                                code = false;
                                msg = "工程下已存在合同,不允许删除";
                            }
                        }

                        if (code)
                        {
                            sqls.Add("delete from i_m_gc where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_jlry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_sgry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_kcry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_sjry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_jldw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_sgdw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_kcdw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_sjdw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_fgc where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_jzry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_syry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_jsdw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_jsry where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_tsdw where gcbh='" + gcbh + "' ");
                            sqls.Add("delete from i_s_gc_tsry where gcbh='" + gcbh + "' ");

                            code = CommonService.ExecTrans(sqls);

                            if (code && JcService.GetSysWzJgJyNewJzqy() == (int)SysWzJgJyNewJzqyEnum.Enabled)
                            {
                                JyJzqyService.DelProjectInfo(gcbh);
                            }
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

		/// <summary>
        /// 删除务工人员系统工程，同时删除从表
        /// </summary>
        [Authorize]
        public void DeleteIMGc_WGRY()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zt,gcbh_yc from i_m_gc where gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的工程信息";
                }
                else
                {
                    string gcbh_yc = dt[0]["gcbh_yc"].GetSafeString();
                    string _gcbh = gcbh;
                    //if (string.IsNullOrEmpty(gcbh_yc))
                    //    _gcbh = gcbh;
                    //else
                    //    _gcbh = gcbh_yc;
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from i_m_gc where gcbh='" + gcbh + "' ");
                    sqls.Add("delete from i_s_gc_jlry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_sgry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_kcry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_sjry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_jldw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_sgdw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_kcdw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_sjdw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_fgc where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_jzry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_syry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_jsdw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_jsry where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_tsdw where gcbh='" + _gcbh + "' ");
                    sqls.Add("delete from i_s_gc_tsry where gcbh='" + _gcbh + "' ");


                    code = CommonService.ExecTrans(sqls);
                    
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除委托单
        /// </summary>
        [Authorize]
        public void DeleteWtd()
        {
            bool code = false;
            string msg = "";
            try
            {                
                string recid = Request["recid"].GetSafeString();
                code = JcService.DeleteWtds(recid, CurrentUser.UserCode, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 删除考勤机
        /// </summary>
        [Authorize]
        public void DeleteIMKqj()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["RECID"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from i_m_kqj where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除工程人员
        /// </summary>
        [Authorize]
        public void DeleteGcry()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["id"].GetSafeInt();
                string sjbmc = Request["sjbmc"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from " + sjbmc + " where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        [Authorize]
        public void DeleteSxt()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["id"].GetSafeInt();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from pr_m_sxt where RECID=" + recid + " ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 删除考评，同时删除考评扣分项
        /// </summary>
        [Authorize]
        public void DeleteIMKP()
        {
            bool code = true;
            string msg = "";
            try
            {
                string KPID = Request["KPID"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_M_KP where KPID='" + KPID + "' ");
                sqls.Add("delete from I_S_KP_KF where FKID='" + KPID + "' ");
                sqls.Add("delete from I_S_KP_DF where FKID='" + KPID + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }



        /// <summary>
        /// 删除工程监督记录，复制到删除表，记录表删除
        /// </summary>
        [Authorize]
        public void DeleteJDJL()
        {
            bool code = true;
            string msg = "";
            try
            {
                string RECID = Request["RECID"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("INSERT INTO [dbo].[JDBG_JDJL_Delete] ([RECID] ,[LX] ,[GCBH] ,[ZJDJH] ,[GCMC] ,[CJRY] ,[WorkSerial] ,[ReportFile] ,[LRRZH] ,[LRRXM] ,[LRSJ] ,[JDJLSJ] ,[BH] ,[ExtraInfo1] ,[ExtraInfo2] ,[ExtraInfo3] ,[ExtraInfo4] ,[ExtraInfo5] ,[CJRYXM] ,[ExtraInfo6] ,[ExtraInfo7] ,[ExtraInfo8] ,[ExtraInfo9] ,[ExtraInfo10] ,[ExtraInfo11] ,[ExtraInfo12] ,[ExtraInfo13] ,[ExtraInfo14] ,[ExtraInfo15] ,[ExtraInfo16] ,[deletetime]) select [RECID] ,[LX] ,[GCBH] ,[ZJDJH] ,[GCMC] ,[CJRY] ,[WorkSerial] ,[ReportFile] ,[LRRZH] ,[LRRXM] ,[LRSJ] ,[JDJLSJ] ,[BH] ,[ExtraInfo1] ,[ExtraInfo2] ,[ExtraInfo3] ,[ExtraInfo4] ,[ExtraInfo5] ,[CJRYXM] ,[ExtraInfo6] ,[ExtraInfo7] ,[ExtraInfo8] ,[ExtraInfo9] ,[ExtraInfo10] ,[ExtraInfo11] ,[ExtraInfo12] ,[ExtraInfo13] ,[ExtraInfo14] ,[ExtraInfo15] ,[ExtraInfo16] ,getdate() from [JDBG_JDJL] where RECID=" + RECID.ToString());
                code = CommonService.ExecTrans(sqls);


                if (code)
                {
                    IList<string> sqls2 = new List<string>();
                    sqls2.Add("delete from JDBG_JDJL where  RECID=" + RECID.ToString());
                    code = CommonService.ExecTrans(sqls2);
                }
                else
                {
                    msg = "删除失败！";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        /// <summary>
        /// 删除工程报告，复制到删除表，记录表删除
        /// </summary>
        [Authorize]
        public void DeleteJDBG()
        {
            bool code = true;
            string msg = "";
            try
            {
                string RECID = Request["RECID"].GetSafeString();
                IList<string> sqls = new List<string>();
                string gcbh = "";
                sqls.Add("INSERT INTO [dbo].[JDBG_JDJL_Delete] ([RECID] ,[LX] ,[GCBH] ,[ZJDJH] ,[GCMC] ,[CJRY] ,[WorkSerial] ,[ReportFile] ,[LRRZH] ,[LRRXM] ,[LRSJ] ,[JDJLSJ] ,[BH] ,[ExtraInfo1] ,[ExtraInfo2] ,[ExtraInfo3] ,[ExtraInfo4] ,[ExtraInfo5] ,[CJRYXM] ,[ExtraInfo6] ,[ExtraInfo7] ,[ExtraInfo8] ,[ExtraInfo9] ,[ExtraInfo10] ,[ExtraInfo11] ,[ExtraInfo12] ,[ExtraInfo13] ,[ExtraInfo14] ,[ExtraInfo15] ,[ExtraInfo16] ,[deletetime]) select [RECID] ,[LX] ,[GCBH] ,[ZJDJH] ,[GCMC] ,[CJRY] ,[WorkSerial] ,[ReportFile] ,[LRRZH] ,[LRRXM] ,[LRSJ] ,[JDJLSJ] ,[BH] ,[ExtraInfo1] ,[ExtraInfo2] ,[ExtraInfo3] ,[ExtraInfo4] ,[ExtraInfo5] ,[CJRYXM] ,[ExtraInfo6] ,[ExtraInfo7] ,[ExtraInfo8] ,[ExtraInfo9] ,[ExtraInfo10] ,[ExtraInfo11] ,[ExtraInfo12] ,[ExtraInfo13] ,[ExtraInfo14] ,[ExtraInfo15] ,[ExtraInfo16] ,getdate() from [JDBG_JDJL] where RECID=" + RECID.ToString());
                sqls.Add("update i_m_gc set zt='JGYS' where gcbh=(select gcbh from JDBG_JDJL where recid=" + RECID + ")");

                code = CommonService.ExecTrans(sqls);

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select gcbh from JDBG_JDJL where recid=" + RECID);
                if (dt.Count>0)
                {
                    gcbh = dt[0]["gcbh"].GetSafeString();
                }
                string procstr = string.Format("ImportRyyjToGc('{0}')", gcbh);
                code = CommonService.ExecProc(procstr, out msg);

                if (code)
                {
                    IList<string> sqls2 = new List<string>();
                    sqls2.Add("delete from JDBG_JDJL where  RECID=" + RECID.ToString());
                    code = CommonService.ExecTrans(sqls2);
                }
                else
                {
                    msg = "删除失败！";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        /// <summary>
        /// 删除工程报告，复制到删除表，记录表删除
        /// </summary>
        [Authorize]
        public void DeleteAPJL()
        {
            bool code = true;
            string msg = "";
            try
            {
                code = false;
                return;
                string RECID = Request["RECID"].GetSafeString();
                IList<string> sqls = new List<string>();
                string gcbh = "";
                sqls.Add("INSERT INTO [dbo].[JDBG_YSAPJL_delete] ([APID] ,[GCBH] ,[GCMC] ,[ZJDJH] ,[YSRY] ,[YSSJ] ,[YSBW] ,[YSLX] ,[JDJLID] ,[WorkSerial] ,[APRZH] ,[APRXM] ,[APSJ] ,[ReportFile] ,[YSLXMS] ,[deletetime])  SELECT [APID] ,[GCBH] ,[GCMC] ,[ZJDJH] ,[YSRY] ,[YSSJ] ,[YSBW] ,[YSLX] ,[JDJLID] ,[WorkSerial] ,[APRZH] ,[APRXM] ,[APSJ] ,[ReportFile] ,[YSLXMS],GETDATE() FROM [dbo].[JDBG_YSAPJL] where [APID]='" + RECID.ToString() + "' ");
                code = CommonService.ExecTrans(sqls);
                
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select gcbh from JDBG_JDJL where recid=" + RECID);
                if (dt.Count > 0)
                {
                    gcbh = dt[0]["gcbh"].GetSafeString();
                }
                string procstr = string.Format("ImportRyyjToGc('{0}')", gcbh);
                code = CommonService.ExecProc(procstr, out msg);

                if (code)
                {
                    IList<string> sqls2 = new List<string>();
                    sqls2.Add("delete from JDBG_JDJL where  RECID=" + RECID.ToString());
                    code = CommonService.ExecTrans(sqls2);
                }
                else
                {
                    msg = "删除失败！";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }



        /// <summary>
        /// 删除大检查，复制到删除表，记录表删除
        /// </summary>
        [Authorize]
        public void DeleteDJC()
        {
            bool code = true;
            string msg = "";
            try
            {
                string RECID = Request["RECID"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("INSERT INTO [dbo].[DJC_FQ_delete] ([RECID] ,[DJCFQBH] ,[CCBT] ,[CCSJSTART] ,[CCSJEND] ,[FQRZH] ,[FQRXM] ,[FQSJ] ,[GCBH] ,[GCMC] ,[AZJDYZH] ,[AZJDYXM] ,[TJJDYZH] ,[TJJDYXM] ,[ZZZH] ,[ZZXM] ,[ZJLRSJ] ,[ZJ] ,[ZJFJ] ,[deletetime]) SELECT [RECID] ,[DJCFQBH] ,[CCBT] ,[CCSJSTART] ,[CCSJEND] ,[FQRZH] ,[FQRXM] ,[FQSJ] ,[GCBH] ,[GCMC] ,[AZJDYZH] ,[AZJDYXM] ,[TJJDYZH] ,[TJJDYXM] ,[ZZZH] ,[ZZXM] ,[ZJLRSJ] ,[ZJ] ,[ZJFJ],getdate() FROM [dbo].[DJC_FQ] where RECID=" + RECID.ToString());
                code = CommonService.ExecTrans(sqls);


                if (code)
                {
                    IList<string> sqls2 = new List<string>();
                    sqls2.Add("delete from DJC_FQ where  RECID=" + RECID.ToString());
                    code = CommonService.ExecTrans(sqls2);
                }
                else
                {
                    msg = "删除失败！";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }


        /// <summary>
        /// 通用删除，记录表删除
        /// </summary>
        [Authorize]
        public void DeleteTable()
        {
            bool code = true;
            string msg = "";
            try
            {
                string ID = Request["ID"].GetSafeRequest();
                string idname = Request["name"].GetSafeRequest();
                string table = Request["table"].GetSafeRequest();
                if (table != "")
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from " + table + " where " + idname + "='" + ID + "' ");
                    code = CommonService.ExecTrans(sqls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

		/// <summary>
        /// 删除检测资质试验项目对应
        /// </summary>
        [Authorize]
        public void DeleteZzsyxm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from pr_s_zzsyxm where parentid='" + recid + "' ");
                sqls.Add("delete from pr_m_zzsyxm where recid='" + recid + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		/// <summary>
        /// 删除设备标定信息
        /// </summary>
        [Authorize]
        public void DeleteBdxx()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from i_s_sb_bd where recid='" + recid + "' and sfyx=1 ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除检测摄像头
        /// </summary>
        [Authorize]
        public void DeleteIMJcsxt()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeRequest();
                code = SxtptService.Remove(recid, out msg);
                //if (code)
                {

                    IList<string> sqls = new List<string>();
                    sqls.Add("update i_m_jcsxt set sfyx=0 where recid='" + recid + "'");
                    code = CommonService.ExecTrans(sqls);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 把检测机构部门sfyx设置成0
        /// </summary>
        [Authorize]
        public void DeleteJcjgbm()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeRequest();

                    IList<string> sqls = new List<string>();
                    sqls.Add("update h_jcjgbm set sfyx=0 where recid='" + recid + "'");
                    code = CommonService.ExecTrans(sqls);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 把内部合同sfyx设置成0
        /// </summary>
        [Authorize]
        public void DeleteNbht()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["id"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_jcht set sfyx=0 ,sxsj=getdate(),xgrzh='"+CurrentUser.UserName+"',xgrxm='"+CurrentUser.RealName+"' where recid='" + recid + "'");
                code = CommonService.ExecTrans(sqls);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除人员企业申请
        /// </summary>
        [Authorize]
        public void DeleteRYSQ()
        {
            bool code = true;
            string msg = "";
            try
            {
                code = CommonService.Delete("I_S_RY_SQ", "RECID", Request["id"].GetSafeString());
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }
		/// <summary>
        /// 删除内部人员
        /// </summary>
        [Authorize]
        public void DeleteImnbry()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();

                IList<IDictionary<string, string>> rydt = CommonService.GetDataTable("select zh from i_m_nbry where rybh='" + rybh + "'");
                string zh = "";
                if (rydt.Count > 0)
                {
                    zh = rydt[0]["zh"];
                }
                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_M_NBRY where rybh='" + rybh + "' ");
                sqls.Add("delete from I_M_QYZH where qybh='" + rybh + "' and zhlx='N' ");
                code = CommonService.ExecTrans(sqls);
                if (zh != "")
                    Remote.UserService.DeleteUser(zh, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除结算人
        /// </summary>
        [Authorize]
        public void DeleteImjsr()
        {
            bool code = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from i_m_jsr where recid='" + id + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除检测章
        /// </summary>
        [Authorize]
        public void DeleteHJcz()
        {
            bool code = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();

                IList<string> sqls = new List<string>();
                sqls.Add("delete from h_jcz where recid='" + id + "' ");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		/// <summary>
        /// 删除投诉记录
        /// </summary>
        [Authorize]
        public void DeleteRYTS()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();

                code = CommonService.Delete("I_M_RY_TS", "RECID", recid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        /// <summary>
        /// 删除劳务员函数
        /// </summary>
        [Authorize]
        public void DeletelZZGY()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                code = CommonService.Delete("I_M_LZZGY_ZH", "RECID", recid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }
        /// <summary>
        /// 删除班次函数
        /// </summary>
        [Authorize]
        public void DeleteKQBC()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string sql = "update infoschedule set hasdelete=1 where recid='" + recid + "'";
                code = CommonService.Execsql(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        [Authorize]
        public JsonResult DeleteJumpPage(string key)
        {
            string msg = "";
            bool code = true;
            try
            {
                IList<string> sqls = new List<string>();
                sqls.Add("delete from sysjumpdestpage where sourcepageid='" + key + "'");
                sqls.Add("delete from sysjumpsrcpage where pageid='" + key + "'");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        /// <summary>
        /// 删除帮助文档
        /// </summary>
        [Authorize]
        public void DeleteHelpDoc()
        {
            bool code = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();

                //string querySql = string.Format("select count(0) count from H_HelpDoc where id='{0}' and userCode = '{1}'", id, CurrentUser.UserCode);
                //var result = CommonService.GetDataTable(querySql);
                //if (result.Count() > 0)
                //{
                //    if (result[0]["count"].GetSafeInt() == 0)
                //    {
                //        code = false;
                //        msg = "只有自己添加的记录才能删除";
                //    }
                //}

                //if (code)
                //{
                    string delSql = "delete from H_HelpDoc where id='" + id + "'";
                    code = CommonService.Execsql(delSql);
                //}
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 通用删除操作
        /// </summary>
        [Authorize]
        public void CurrencyDelete()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string tablename = Request["tablename"].GetSafeString();
                code = CommonService.Delete(tablename, "recid", recid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        #endregion
    }
}