using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace BD.Jcbg.Web.Controllers
{
    public class JX_InfoController : Controller
    {

        #region 服务
        private IKqjCmdService _kqjService;
        private IKqjCmdService KqjService
        {
            get
            {
                if (_kqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _kqjService = webApplicationContext.GetObject("KqjService") as IKqjCmdService;
                }
                return _kqjService;
            }
        }

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
        #endregion

        #region 页面
        [Authorize]
        public ActionResult Agreement()
        {
            return View();
        }
        /// <summary>
        /// 劳资手动设置考勤
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SetKqView()
        {
            try
            {
                string sfzhms = Request["sfzhms"].GetSafeString();
                ViewData.Add("sfzhms", sfzhms);
            }
            catch(Exception e)
            { }
            return View();
        }

        [Authorize]
        public ActionResult SetKqViewDate()
        {
            string kqtimes = "1";
            try
            {
                string sql = "select kqtimes from i_m_gc_kgba where jdzch='" + CurrentUser.Jdzch + "' and qybh='" + CurrentUser.Qybh + "'";
                IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql);
                if(dt.Count>0)
                {
                    kqtimes = dt[0]["kqtimes"];
                }
            }
            catch(Exception e)
            { }
            string jdzch = CurrentUser.Jdzch;
            ViewData.Add("jdzch", jdzch);
            ViewData.Add("kqtimes", kqtimes);
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取班组负责人
        /// </summary>
        [Authorize]
        public void getbzfzr()
        {
            IList<IDictionary<string, string>> datas = KqjService.GetBzfzrs();
            IDictionary<string, string> row = new Dictionary<string, string>();
            row.Add("sfzhm", "");
            row.Add("ryxm", "不限");
            datas.Insert(0, row);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.ContentType = "text/plain";
            Response.Write(jss.Serialize(datas));
            Response.End();
        }
        /// <summary>
        /// 获取工种
        /// </summary>
        [Authorize]
        public void getGZ()
        {
            IList<IDictionary<string, string>> datas = KqjService.GetGzs();
            IDictionary<string, string> row = new Dictionary<string, string>();
            row.Add("recid", "");
            row.Add("gzname", "");
            //datas.Insert(0, row);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.ContentType = "text/plain";
            Response.Write(jss.Serialize(datas));
            Response.End();
        }
        /// <summary>
        /// 获取工程名称
        /// </summary>
        [Authorize]
        public void getGCMC()
        {
            
        }
        /// <summary>
        /// 获取人员库的人员信息
        /// </summary>
        [Authorize]
        public void getryinfo()
        {
            string msg = "[]";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                string sfzhm=Request["sfzhm"].GetSafeString();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from i_m_ry_info where sfzhm='" + sfzhm+"'");
                if (dt.Count == 0)
                {
                    string s = "查不到该人员信息";
                    code = false;
                    msg = JsonFormat.GetRetString(code, s);

                }
                else
                    msg = new JavaScriptSerializer().Serialize(dt[0]);
			
            }
            catch (Exception e)
            {
                msg = new JavaScriptSerializer().Serialize(e.Message); 
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(msg);
            }
        }
        /// <summary>
        /// 获取工程监督注册号
        /// </summary>
        public void getgcjdzch()
        {
            string msg = "";
            bool code = true;
            try
            {
                msg = CurrentUser.Jdzch;

            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        } 
        /// <summary>
        /// 根据卡号获取银行名
        /// </summary>
        public void GetBankName()
        {
            string msg = "";
            bool code = true;
            try
            {
                
                string yhkh = Request["yhkh"].GetSafeString();
                string yhkyh = BankInfo.GetBankName(yhkh.ToCharArray(), 0);  //获取银行卡的信息;
                msg = yhkyh;
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void FlowCheckI_M_RY_INFO()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string jdzch = Request["jdzch"].GetSafeString();
                string sql = "select * from I_M_RY_INFO where sfzhm='" + sfzhm + "'";
                IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
                if(list.Count==0)
                {
                    sql = "select * from I_m_ry where sfzhm='" + sfzhm + "' and jdzch='"+jdzch+"'";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if(dt.Count!=0)
                    {
                        string rybh = dt[0]["rybh"].ToString();
                        string ryxm = dt[0]["ryxm"].ToString();
                        string sjhm = dt[0]["sjhm"].ToString();
                        string xb = dt[0]["xb"].ToString();
                        string sfzdz = dt[0]["sfzdz"].ToString();
                        string qfjg = dt[0]["qfjg"].ToString();
                        string sfzyxq = dt[0]["sfzyxq"].ToString();
                        string zp = dt[0]["zp"].ToString();
                        string hm = dt[0]["hm"].ToString();
                        string hmzl = dt[0]["hmzl"].ToString();                    
                        string yhkh = dt[0]["yhkh"].ToString();
                        string yhkyh = dt[0]["yhkyh"].ToString();
                        //string yhkyh =BankInfo.GetBankName(yhkh.ToCharArray(),0);  //获取银行卡的信息;

                        IList<string> sqls = new List<string>();

                        sql = "Insert INTO i_m_ry_info (rybh,ryxm,sfzhm,sjhm,xb,sfzdz,qfjg,sfzyxq,zp,hm,hmzl,yhkyh,yhkh) values('" + rybh + "','" +
                            ryxm + "','" +
                            sfzhm + "','" +
                            sjhm + "','" +
                            xb + "','" +
                            sfzdz + "','" +
                            qfjg + "','" +
                            sfzyxq + "','" +
                            zp + "','" +
                            hm + "','" +
                            hmzl + "','" +
                            yhkyh + "','" +
                            yhkh + "')";
                        sqls.Add(sql);

                        sql = "update i_m_ry set yhkyh='" + yhkyh + "' where sfzhm='" + sfzhm + "'and jdzch='" + jdzch + "'";
                        sqls.Add(sql);

                        code = CommonService.ExecSqls(sqls);
                                         
                    }
              
                }
            }
            catch(Exception  e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 获取新建的人员编号
        /// </summary>
        public void GetRYBH_New()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sql = "SELECT ZDBH from PR_M_BHMS WHERE BHMSJZ='I_M_RY__RYBH'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if(list.Count!=0)
                {
                    string rybh_old = list[0]["zdbh"].GetSafeString();
                    string rybh_temp = (rybh_old.Substring(1).GetSafeInt() + 1).ToString().PadLeft(6, '0');
                    string rybh_new = "R" + rybh_temp;
                    msg = rybh_new;
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        #endregion

        #region 任务数据获取
        /// <summary>
        /// 获取开工备案审批数量
        /// </summary>
        [Authorize]
        public void getkgbasl()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from i_m_gc_kgba where sptg=0 and tjsp=1");
                msg = dt[0]["sum"];
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        } 
        #endregion

        #region 

        /// <summary>
        /// 设置工程是用考勤机还是人工设置考勤
        /// </summary>
        [Authorize]
        public void setgckqtype()
        {
            string msg = "";
            bool code = true;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string jdzch = Request["jdzch"].GetSafeString();
                string kqlx = Request["kqlx"].GetSafeString();
                string kqtype="";
                if(kqlx=="虹膜")
                    kqtype = "0";
                else
                    kqtype = "1";
                string sql = "update i_m_gc_kgba set kqtype='" + kqtype + "' where qybh='" + qybh + "' and jdzch='" + jdzch + "'";
                code=CommonService.Execsql(sql);
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void getgckqtype()
        {
            string msg = "";
            bool code = false;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string jdzch = Request["jdzch"].GetSafeString();

                string sql = "select kqtype from i_m_gc_kgba  where  qybh='" + CurrentUser.Qybh + "' and jdzch='" + CurrentUser.Jdzch + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    if (dt[0]["kqtype"].GetSafeString() == "False") //False为手动
                        code = true;
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        [Authorize]
        public void SetGcryKq()
        {
            string msg = "";
            bool code = false;
            try
            {
                string sfzhms = Request["sfzhms"].GetSafeString();
                DateTime datetime = Request["date"].GetSafeDate();
                sfzhms = sfzhms.Trim(',');
                if (Request["date"].GetSafeString()=="")
                {
                    datetime = DateTime.Now;
                }
                IList<string> sqls = new List<string>();
                string sql = "";
                
                string[] sfzhm_list = sfzhms.Split(',');
                for (int i = 0; i < sfzhm_list.Length; i++)
                {
                    sql = "select * from kqjuserlog where userid='" + sfzhm_list[i] + "' and  datediff(dd,logdate,'" + datetime + "')=0 and companyid='" + CurrentUser.Qybh + "' and placeid='" + CurrentUser.Jdzch + "'";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if(dt.Count>0)
                    {
                        
                    }
                    else
                    {
                        sql = "INSERT INTO kqjuserlog (userid,LogDate,companyid,placeid,hasdeal,dealtype,logtype) values ('" + sfzhm_list[i] + "','" + datetime + "','"+CurrentUser.Qybh+ "','"+CurrentUser.Jdzch+"',1,1,'1')";
                        sqls.Add(sql);
                    }
                }
                if (sqls.Count != 0)
                    code = CommonService.ExecSqls(sqls);
                else
                    msg = "选择的人员已考勤过";
                if(code)
                {
                     for (int i = 0; i < sfzhm_list.Length; i++)
                     {
                         KqjService.SaveUserLogWithOUTkqj(sfzhm_list[i], datetime, CurrentUser.Qybh, CurrentUser.Jdzch);
                     }
                  
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 判断工种名是否已被使用
        /// </summary>
        [Authorize]
        public void getGcGzUse()
        {
            string msg = "";
            bool code = false;
            try
            {
                string jdzch = Request["jdzch"].GetSafeString();
                string gzname = Request["gzname"].GetSafeString();
                string sql = "select * from i_m_ry where gz='" + gzname + "' and jdzch='"+jdzch+"'";
                IList<IDictionary<string, string>> dt =CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    code = false;
                    msg = "该工种已被人员使用,无法修改";
                }
                else
                    code = true;
            }
            catch (Exception e)
            {
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