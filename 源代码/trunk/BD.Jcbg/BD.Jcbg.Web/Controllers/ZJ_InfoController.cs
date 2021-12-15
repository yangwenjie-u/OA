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
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using BD.Common;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ReportPrintService.OpenXmlSdk;
using DocumentFormat.OpenXml;

namespace BD.Jcbg.Web.Controllers
{
    public class ZJ_InfoController : Controller
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

        [Authorize]
        public ActionResult WgryXCZP()
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
            catch (Exception e)
            { }
            return View();
        }

        [Authorize]
        public ActionResult SetKqViewDate()
        {
            string kqtimes = "1";
            try
            {
                string sql = "select kqtimes from i_m_gc where gcbh='" + CurrentUser.Jdzch + "' ";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    kqtimes = dt[0]["kqtimes"];
                }
                if (kqtimes != "2")
                    kqtimes = "1";
            }
            catch (Exception e)
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
        /// 获取岗位
        /// </summary>
        [Authorize]
        public void GetGzGw()
        {
            string gz = DataFormat.GetSafeString(Request["gz"]);
            IList<IDictionary<string, string>> datas = KqjService.GetGzGws(gz);
            IDictionary<string, string> row = new Dictionary<string, string>();
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
                string sfzhm = Request["sfzhm"].GetSafeString();
             //   string encode_sfzhm = EncryUtil.Encode(sfzhm);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from i_m_ry_info where sfzhm='" + sfzhm + "'");
               // IDictionary<string, string> srcsfzhm=new Dictionary<string, string>();
                //srcsfzhm.Add("srcsfzhm",sfzhm);
                //dt.Add(srcsfzhm);
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
        /// 获取工程监督注册号/工程编号
        /// </summary>
        [Authorize]
        public void getgcbh_jdzch()
        {
            string msg = "";
            bool code = true;
            string json = "";
            try
            {
                string jdzch = CurrentUser.Jdzch;
                string gcbh = CurrentUser.GCBH;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("code", code ? "0" : "1");
                row.Add("jdzch", jdzch);
                row.Add("gcbh", gcbh);
                json=jss.Serialize(row);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(json);
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
        /// <summary>
        /// 判断人员库是否有，没有则插入，目前没有使用
        /// </summary>
        [Authorize]
        public void FlowCheckI_M_RY_INFO()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string jdzch = Request["jdzch"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string sql = "select * from I_M_RY_INFO where sfzhm='" + sfzhm + "'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if (list.Count == 0)
                {
                    sql = "select * from i_m_wgry where sfzhm='" + sfzhm + "' and jdzch='" + jdzch + "'";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count != 0)
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

                        sql = "update i_m_wgry set yhkyh='" + yhkyh + "' where sfzhm='" + sfzhm + "'and jdzch='" + jdzch + "'";
                        sqls.Add(sql);

                        code = CommonService.ExecSqls(sqls);

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
                if (list.Count != 0)
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
        /// 设置工程是用考勤机还是人工设置考勤 ,不使用
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
                string kqtype = "";
                if (kqlx == "虹膜")
                    kqtype = "0";
                else
                    kqtype = "1";
                string sql = "update i_m_gc_kgba set kqtype='" + kqtype + "' where qybh='" + qybh + "' and jdzch='" + jdzch + "'";
                code = CommonService.Execsql(sql);
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
        /// 判断考勤是否允许手动
        /// </summary>
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

        /// <summary>
        /// 手动插入kqjuserlog 
        /// </summary>
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
                if (Request["date"].GetSafeString() == "")
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
                    if (dt.Count > 0)
                    {

                    }
                    else
                    {
                        sql = "INSERT INTO kqjuserlog (userid,LogDate,companyid,placeid,hasdeal,dealtype,logtype) values ('" + sfzhm_list[i] + "','" + datetime + "','" + CurrentUser.Qybh + "','" + CurrentUser.Jdzch + "',1,1,'1')";
                        sqls.Add(sql);
                    }
                }
                if (sqls.Count != 0)
                    code = CommonService.ExecSqls(sqls);
                else
                    msg = "选择的人员已考勤过";
                if (code)
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
                string gcbh = Request["gcbh"].GetSafeString();
                string gzname = Request["gzname"].GetSafeString();
                string where="";
                if (jdzch != "")
                    where += " and jdzch='" + jdzch + "'";
                if (gcbh != "")
                    where += " and gcbh='" + gcbh + "'";
                string sql = "select * from i_m_wgry where gz='" + gzname + "'" +where;// and jdzch='" + jdzch + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
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

        /// <summary>
        /// 保存工资发放审批时的recid
        /// </summary>
        [Authorize]
        public void saveGZffRecid()
        {
            string msg = "";
            bool code = false;
            try
            {
                string recids = Request["recids"].GetSafeString();
                string Jdzch = CurrentUser.Jdzch;
                string guid = Guid.NewGuid().ToString();

                string sql = "select ryxm from View_KQJUSERMONTHPAY where (yhkh is null or yhkh='') and recid in (" + recids + ")";
                IList<IDictionary<string, string>> notyh =CommonService.GetDataTable(sql);
                for(int i=0;i<notyh.Count;i++)
                {
                    msg += notyh[i]["ryxm"] + ",";
                }
                if(msg!="")
                {
                    msg = msg.Substring(0, msg.Length - 1);
                    msg += "没有设置银行卡,请先设置";
                }
                if(msg=="")
                {
                    sql = "select ryxm from View_KQJUSERMONTHPAY where recid in (" + recids + ") and tjsp=1";
                    IList<IDictionary<string, string>> havetjdata = CommonService.GetDataTable(sql);
                    for (int i = 0; i < havetjdata.Count; i++)
                    {
                        msg += havetjdata[i]["ryxm"] + ",";
                    }
                    if (msg != "")
                    {
                        msg = msg.Substring(0, msg.Length - 1);
                        msg += "已经提交审批,不能重复提交";
                    }
                }
                if(msg=="")
                {
                    IList<string> sqls = new List<string>();
                   // sql = "update KqjUserMonthPay set tjsp=1 where recid in (" + recids + ")";
                   // sqls.Add(sql);
                    sql = "INSERT INTO INFO_XZFF ([RGuid],[PayRecids] ,[JDZCH] ,[LRRZH],[LRSJ],[Hasdeal]) VALUES('" + guid + "','" + recids + "','" + Jdzch + "','" + CurrentUser.UserName + "',getdate(),0)";
                    sqls.Add(sql);
                    //  code = CommonService.Execsql(sql);
                    code=CommonService.ExecSqls(sqls);
                    msg = guid;
                }
                else
                {
                    code = false;
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
        /// 获取工资发放表的recid
        /// </summary>
        [Authorize]
        public void getGZffRecid()
        {
            string msg = "";
            bool code = false;
            try
            {
                string guid = Request["guid"].GetSafeString();
                string Jdzch = CurrentUser.Jdzch;
                string sql = "select PayRecids from INFO_XZFF where RGuid='" + guid + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    msg = dt[0]["payrecids"];
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
        /// <summary>
        /// 保存新的银行卡信息
        /// </summary>
        [Authorize]
        public void saveYHK_NEW()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yhkyh = Request["yhkyh"].GetSafeString();
                string yhkh = Request["yhkh"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                string ryxm = Request["ryxm"].GetSafeString();
                string jdzch = Request["gcbh"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                if(yhkh!="")
                {
                    string Jdzch = CurrentUser.Jdzch;
                    string sql = "select * from I_S_GC_YHKH where sfzhm='" + sfzhm + "' and yhkh='" + yhkh + "'";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        code = true;
                    }
                    else
                    {
                        sql = "INSERT INTO [dbo].[I_S_GC_YHKH] ([JDZCH] ,[SFZHM],[YHKYH],[YHKH],[RYXM] ,[SJHM]) values('" + jdzch + "','" +
                        sfzhm + "','" +
                        yhkyh + "','" +
                        yhkh + "','" +
                        ryxm + "','" +
                        sjhm + "')";
                        code = CommonService.Execsql(sql);
                    }
                }
                else
                {
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
        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject2(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter(); return formatter.Deserialize(ms);
            }
        }
        public static object BytesToObject(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(data);
            data = null;
            return formatter.Deserialize(rems);
        }
        [Authorize]
        public void SaveSFZFYJBase64()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                if (gcbh != "")
                {
                    string sql = "select sfzdz,csrq,qfjg,ryxm,sfzhm,mz,xb,sfzyxq,zp from i_m_wgry where jdzch='" + gcbh + "' and (yhkh='' or yhkh is null) and sfzdz !='' and csrq !='' and qfjg!='' and ryxm !='' and sfzhm !='' and mz !='' and xb  !='' and sfzyxq ! ='' and CAST(zp as nvarchar(max))   !=''";
                    IList<IDictionary<string, string>> rydatas = CommonService.GetDataTable(sql);
                    IList<IDCardInfo> infos = new List<IDCardInfo>();

                    for (int i = 0; i < rydatas.Count; i++)
                    {
                        IDCardInfo info = new IDCardInfo()
                        {
                            Address = rydatas[i]["sfzdz"], //地址
                            BirtyDay = rydatas[i]["csrq"],
                            Department = rydatas[i]["qfjg"],
                            FullName = rydatas[i]["ryxm"],
                            IdNo = rydatas[i]["sfzhm"],
                            Nation = rydatas[i]["mz"],
                            Sex = rydatas[i]["xb"],
                            ValidDate = rydatas[i]["sfzyxq"],
                            Thumb = rydatas[i]["zp"],
                        };
                        infos.Add(info);
                    }
                    IList<IDictionary<string, byte[]>> idcards = IDCardOperation.GetIDCardList(infos, out msg);

                    for (int j = 0; j < idcards.Count; j++)
                    {
                        byte[] a = idcards[j]["front"];
                        byte[] b = idcards[j]["back"];
                        string front = Convert.ToBase64String(a);
                        string back = Convert.ToBase64String(b);
                        sql = "update i_m_ry_info set SFZFYJ_FRONT='" + front + "',SFZFYJ_BACK='" + back + "' where sfzhm='" + rydatas[j]["sfzhm"]+"'";
                        CommonService.Execsql(sql);                      
                    }
                   
                }
            }
            catch (Exception e)
            {

            }

        }
        /// <summary>
        /// 批量打印身份证
        /// </summary>
        [Authorize]
        public ActionResult ExportIDCardByBase64()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                if (gcbh != "")
                {
                    string sql = "select SFZFYJ_FRONT,SFZFYJ_BACK from i_m_ry_info where SFZFYJ_FRONT is not null and SFZFYJ_BACK is not null and  sfzhm in (select sfzhm from i_m_wgry where jdzch='" + gcbh + "' and (yhkh='' or yhkh is null) and sfzdz !='' and csrq !='' and qfjg!='' and ryxm !='' and sfzhm !='' and mz !='' and xb  !='' and sfzyxq ! ='' and CAST(zp as nvarchar(max))   !='')";
                    IList<IDictionary<string, string>> rydatas = CommonService.GetDataTable(sql);
                 
                    IList<IDictionary<string, object>> opendata = new List<IDictionary<string, object>>();
                    for (int j = 0; j < rydatas.Count; j++)
                    {
                        //using (FileStream fsWrite = new FileStream(@"D:\1.jpg", FileMode.Create))
                        //{
                        //    fsWrite.Write(a, 0, a.Length);
                        //};
                        object c = rydatas[j]["sfzfyj_front"] as object;
                        object d = rydatas[j]["sfzfyj_back"] as object;

                        IDictionary<string, object> list = new Dictionary<string, object>();
                        list.Add("front", c);
                        opendata.Add(list);
                        list = new Dictionary<string, object>();
                        list.Add("front", d);
                        opendata.Add(list);
                    }
                    if (opendata.Count > 0)
                    {
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.fileindex = "0";
                        c.filename = "身份证复印件";
                        c.table = "sfzfy";
                        c.where = "1=1";
                        c.openType = ReportPrint.OpenType.FileDown;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>();
                        data.Add("sfzfy", opendata);
                        c.data = data;
                        c.signindex = 0;
                        c.AllowVisitNum = 1;
                        var guid = g.Add(c);

                        string url = "/reportPrint/Index?" + guid;
                        return new RedirectResult(url);
                    }

                }

            }
            catch (Exception e)
            {

            }
            return null;
        }
        /// <summary>
        /// 批量打印身份证
        /// </summary>
        [Authorize]
        public ActionResult ExportIDCard()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                if(gcbh!="")
                {
                    string sql = "select sfzdz,csrq,qfjg,ryxm,sfzhm,mz,xb,sfzyxq,zp from i_m_wgry where jdzch='" + gcbh + "' and (yhkh='' or yhkh is null) and sfzdz !='' and csrq !='' and qfjg!='' and ryxm !='' and sfzhm !='' and mz !='' and xb  !='' and sfzyxq ! ='' and CAST(zp as nvarchar(max))   !=''";
                    IList<IDictionary<string, string>> rydatas=CommonService.GetDataTable(sql);
                    IList<IDCardInfo> infos = new List<IDCardInfo>();
                    for (int i = 0; i < rydatas.Count; i++)
                    {
                        IDCardInfo info = new IDCardInfo()
                        {
                            Address = rydatas[i]["sfzdz"], //地址
                            BirtyDay = rydatas[i]["csrq"],
                            Department =  rydatas[i]["qfjg"],
                            FullName = rydatas[i]["ryxm"],
                            IdNo = rydatas[i]["sfzhm"],
                            Nation = rydatas[i]["mz"],
                            Sex = rydatas[i]["xb"],
                            ValidDate = rydatas[i]["sfzyxq"],
                            Thumb = rydatas[i]["zp"],
                        };
                        infos.Add(info);
                    }
                    IList<IDictionary<string, byte[]>> idcards = IDCardOperation.GetIDCardList(infos, out msg);
                    IList<IDictionary<string, object>> opendata=new List<IDictionary<string, object>>();
                    for (int j = 0; j < idcards.Count; j++)
                    {
                        byte[] a =idcards[j]["front"];
                        byte[] b = idcards[j]["back"];
                        //using (FileStream fsWrite = new FileStream(@"D:\1.jpg", FileMode.Create))
                        //{
                        //    fsWrite.Write(a, 0, a.Length);
                        //};
                        object c =Convert.ToBase64String(a) as object;
                        object d =Convert.ToBase64String(b) as object;

                        IDictionary<string, object> list =  new Dictionary<string, object>();
                        list.Add("front", c);
                        opendata.Add(list);
                        list = new Dictionary<string, object>();
                        list.Add("front", d);
                        opendata.Add(list);
                    }
                    if (opendata.Count > 0)
                    {
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.fileindex = "0";
                        c.filename = "身份证复印件";
                        c.table = "sfzfy";
                        c.where = "1=1";
                        c.openType = ReportPrint.OpenType.FileDown;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>();
                        data.Add("sfzfy", opendata);
                        c.data = data;
                        c.signindex = 0;
                        c.AllowVisitNum = 1;
                        var guid = g.Add(c);

                        string url = "/reportPrint/Index?" + guid;
                        return new RedirectResult(url);
                    }
                   
                }
            
            }
            catch (Exception e)
            {

            }
            return null;
        }
        /// <summary>
        /// 导出人员复印件doc
        /// </summary>
        [Authorize]
        public void ExportIDCardDoc()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            MemoryStream ms = new MemoryStream();
            try
            {
                if (gcbh != "")
                {
                    string sql = "select sfzdz,csrq,qfjg,ryxm,sfzhm,mz,xb,sfzyxq,zp from i_m_wgry where hasdelete=0 and jdzch='" + gcbh + "' and (yhkh='' or yhkh is null) and sfzdz !='' and csrq !='' and qfjg!='' and ryxm !='' and sfzhm !='' and mz !='' and xb  !='' and sfzyxq ! ='' and CAST(zp as nvarchar(max))   !=''";
                    IList<IDictionary<string, string>> rydatas = CommonService.GetDataTable(sql);
                    IList<IDCardInfo> infos = new List<IDCardInfo>();
                    for (int i = 0; i < rydatas.Count; i++)
                    {
                        IDCardInfo info = new IDCardInfo()
                        {
                            Address = rydatas[i]["sfzdz"], //地址
                            BirtyDay = rydatas[i]["csrq"],
                            Department = rydatas[i]["qfjg"],
                            FullName = rydatas[i]["ryxm"],
                            IdNo = rydatas[i]["sfzhm"],
                            Nation = rydatas[i]["mz"],
                            Sex = rydatas[i]["xb"],
                            ValidDate = rydatas[i]["sfzyxq"],
                            Thumb = rydatas[i]["zp"],
                        };
                        infos.Add(info);
                    }
                    IList<IDictionary<string, byte[]>> idcards = IDCardOperation.GetIDCardList(infos, out msg);

                    using (WordprocessingDocument doc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart objMainDocumentPart = doc.AddMainDocumentPart();
                        Document objDocument = new Document();
                        Body objBody = new Body();
                        objDocument.Append(objBody);
                        objMainDocumentPart.Document = objDocument;
                        for (int j = 0; j < idcards.Count; j++)
                        {
                            byte[] front = idcards[j]["front"];
                            byte[] back = idcards[j]["back"];
                            //front

                            Paragraph p = new Paragraph();
                            DocumentFormat.OpenXml.Wordprocessing.Run r = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            r.AddPicture(doc, front);
                            p.Append(r);
                            //front
                            Paragraph tp = new Paragraph();
                            objBody.Append(p);
                            objBody.Append(tp);
                            objBody.Append(tp.CloneNode(false));
                            objBody.Append(tp.CloneNode(false));
                            objBody.Append(tp.CloneNode(false));
                            //back
                            Paragraph p2 = new Paragraph();
                            DocumentFormat.OpenXml.Wordprocessing.Run r2 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            r2.AddPicture(doc, back);
                            p2.Append(r2);
                            objBody.Append(p2);

                            if(j != idcards.Count-1)
                            {
                                //换页
                                Paragraph pp = new Paragraph();
                                DocumentFormat.OpenXml.Wordprocessing.Run rr = new DocumentFormat.OpenXml.Wordprocessing.Run();
                                DocumentFormat.OpenXml.Wordprocessing.Break br = new DocumentFormat.OpenXml.Wordprocessing.Break();
                                br.Type = BreakValues.Page;
                                br.Clear = BreakTextRestartLocationValues.All;
                                pp.ParagraphProperties = new ParagraphProperties();
                                pp.ParagraphProperties.WidowControl = new WidowControl();
                                pp.ParagraphProperties.WidowControl.Val = new OnOffValue();
                                pp.ParagraphProperties.WidowControl.Val.Value = true;
                                pp.ParagraphProperties.Justification = new Justification();
                                pp.ParagraphProperties.Justification.Val = JustificationValues.Left;
                                rr.AppendChild(br);
                                pp.AppendChild(rr);
                                objBody.Append(pp);
                            }                           
                        }
                    }               
                    //using (FileStream fsWrite = new FileStream(@"D:\1.docx", FileMode.Create))
                    //{
                    //    fsWrite.Write(ms.ToArray(), 0, ms.ToArray().Length);
                    //};

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
               
                msg = e.Message;
            }
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //设置输出流
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            //防止中文乱码
            string fileName = HttpUtility.UrlEncode("复印件打印");
            //设置输出文件名
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".docx");
            //输出
            Response.BinaryWrite(ms.ToArray());
        }

        #endregion

        #region 黑名单
        [Authorize]
        public void AddRYHMD()
        {

            bool ret = true;
            string msg = "";
            string guid = "";

            try
            {
                string sfzhmlist = Request["sfzhmlist"].GetSafeString();
                if (sfzhmlist != "")
                {
                    guid = Guid.NewGuid().ToString("N");
                    string sql = string.Format("insert into INFO_RYHMD (GUID, SFZHMLIST) VALUES ('{0}','{1}')", guid, sfzhmlist);
                    ret = CommonService.Execsql(sql);
                    if (!ret) msg = "插入身份证列表失败！";
                }
                else
                {
                    ret = false;
                    msg = "人员不能为空！";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\",\"guid\":\"{2}\"}}", ret ? "0" : "1", msg, guid));
                Response.End();
            }
        }
        [Authorize]
        public void GETRYHMD()
        {

            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string guid = Request["guid"].GetSafeString();
                if (guid != "")
                {
                    string sql = string.Format("SELECT  ryxm, sfzhm, sjhm,xb  from view_I_M_RY_INFO WHERE sfzhm in (select item from dbo.SplitString((select sfzhmlist from info_ryhmd where guid='{0}'),',',1)) ", guid);
                    dt = CommonService.GetDataTable(sql);
                }
                else
                {
                    ret = false;
                    msg = "获取人员列表失败！";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\",\"rows\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        [Authorize]
        public void AddQYHMD()
        {

            bool ret = true;
            string msg = "";
            string guid = "";

            try
            {
                string qybhlist = Request["qybhlist"].GetSafeString();
                if (qybhlist != "")
                {
                    guid = Guid.NewGuid().ToString("N");
                    string sql = string.Format("insert into INFO_QYHMD (GUID, QYBHLIST) VALUES ('{0}','{1}')", guid, qybhlist);
                    ret = CommonService.Execsql(sql);
                    if (!ret) msg = "插入企业编号列表失败！";
                }
                else
                {
                    ret = false;
                    msg = "企业不能为空！";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\",\"guid\":\"{2}\"}}", ret ? "0" : "1", msg, guid));
                Response.End();
            }
        }
        [Authorize]
        public void GETQYHMD()
        {

            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string guid = Request["guid"].GetSafeString();
                if (guid != "")
                {
                    string sql = string.Format("SELECT SY_WDQY,LXMC,QYBH,QYMC,QYFZR,LXSJ,QYFR,QYFRSJ from view_i_m_qy WHERE qybh in (select item from dbo.SplitString((select qybhlist from info_qyhmd where guid='{0}'),',',1)) ", guid);
                    dt = CommonService.GetDataTable(sql);
                }
                else
                {
                    ret = false;
                    msg = "获取企业列表失败！";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\",\"rows\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        #endregion
    }
}