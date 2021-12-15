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
using System.Web.UI;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using System.Text.RegularExpressions;
using Spring.Transaction.Interceptor;
using Newtonsoft.Json;

namespace BD.Jcbg.Web.Controllers
{
    public class DwgxWzLcController : Controller
    {
        #region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                }
                return _commonService;
            }
        }

        private IJdbgService _jdbgService = null;
        private IJdbgService JdbgService
        {
            get
            {
                if (_jdbgService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jdbgService = webApplicationContext.GetObject("JdbgService") as IJdbgService;
                }
                return _jdbgService;
            }
        }

        private IWorkFlowService _workflowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workflowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workflowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workflowService;
            }
        }

        private IRemoteUserService _remoteUserService = null;
        private IRemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as IRemoteUserService;
                }
                return _remoteUserService;
            }
        }
        #endregion
        #region 页面
        /// <summary>
        /// 监督站安监流程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult stformview()
        {
            ViewBag.serial = Request["serial"].GetSafeString();
         
            return View();
        }

        [Authorize]
        public ActionResult qylyry()
        {
            string parentid = Request["parentid"].GetSafeString();
            bool code = false;
            string msg="";
            try
            {
                string sql="select readertime from companyreader  where ParentEntity='" + CompanyEntityType.UserMail + "' and ParentId='" + parentid + "' and HasReader=1 and username='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>>  list= CommonService.GetDataTable(sql);
                if (list.Count > 0 && list[0]["readertime"]!="")
                {
                    DateTime rdtime = list[0]["readertime"].GetSafeDate();
                    if((DateTime.Now-rdtime).TotalHours>24)
                    {
                        code = false;
                        msg = "<script>alert('连接已失效')</script>";
                    }
                    else
                    {
                        code = true;
                    }
                }
                else
                {
                    code = false;
                    msg = "<script>alert('连接已失效')</script>";
                }
            }
            catch(Exception e)
            {
                msg= "<script>alert('"+e.Message+"')</script>";
            }
            if (code)
                return View();
            else
                return Content(msg);
        }
        [Authorize]
        public ActionResult SCRYZP()
        {
            try
            {
                string title = "人员照片上传";
                string buttons = Server.UrlEncode("保存|TJ| | ");
                string username = CurrentUser.UserName;
                string sql = "select rybh from i_m_ry where zh='" + username + "'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if (list.Count > 0)
                {
                    string url = "/datainput/Index?zdzdtable=zdzd_jc&t1_tablename=i_m_ry&&t1_pri=rybh&t1_title=" + title + "&button=" + buttons + "&rownum=1&LX=ZP&jydbh=" + list[0]["rybh"];
                    return new RedirectResult(url);
                }
                else
                    return null;
            }
            catch(Exception e)
            {
                return null;
            }
          
        }
        #endregion

        #region 设置获取数据
        /// <summary>
        /// 设置人员申请企业状态
        /// </summary>
        [Authorize]
        public void SetRySQZT()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                sqls.Add("update i_s_ry_sq set sfyx=1,sptg=1 where rybh in (" + rybh.FormatSQLInStr() + ") and qybh=(select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')");
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 获取工程流程号
        /// </summary>
        [Authorize]
        public void GetSTSerial()
        {
            string serialno = "";
            string msg = "";
            bool code = true;
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();
                string sql = "select SerialNo from STFORM where ExtraInfo3='" + gcbh + "' and DoState=0 and formname='开工整改流程'";
                IList<IDictionary<string, string>> list= CommonService.GetDataTable(sql);
                if(list.Count!=0)
                {
                    serialno = list[0]["serialno"].GetSafeString();
                    msg = serialno;
                }
                else
                {
                    code = false;
                    msg = "找不到该工程的流程";
                }
                
            }
            catch(Exception e)
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
        /// 获取常用语句
        /// </summary>
        [Authorize]
        public JsonResult GetSelfCYYJ()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string ryzh = Request["ryzh"].GetSafeString();
                string nrlx = Request["nrlx"].GetSafeString();
                string sql = "select * from h_CYYJ where 1=1 ";
                if (ryzh != "")
                    sql += " and ryzh='" + ryzh + "' ";
                if (nrlx != "")
                    sql += " and nrlx='" + nrlx + "' ";

                sql += " order by recid desc";
                SysLog4.WriteError(sql);
                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(ret);
        }

        [Authorize]
        public void GetGcinfo()
        {
            string msg = "";
            bool code = true;
            try
            {
                string zjdjh = Request["zjdjh"].GetSafeString();
                string sql = "select gcbh,gclxbh from I_m_gc where zjdjh='" + zjdjh + "'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if (list.Count != 0)
                {
                    msg = JsonConvert.SerializeObject(list);
                }
                else
                {
                    code = false;
                    msg = "找不到该工程的流程";
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


        #region 邮件公告用户树
        /// <summary>
        /// 获取部门用户树
        /// </summary>
        [Authorize]
        public void GetUserTree()
        {
            var depcode = Request["depcode"].GetSafeString();
            var nocompany = Request["nocompany"].GetSafeString();
            StringBuilder sb = new StringBuilder();
            try
            {
                string sqllx = "select * from H_QYLX";
                IList<IDictionary<string, string>> qylxs = CommonService.GetDataTable(sqllx);
                string sql = "select * from View_I_M_QY_WITH_ZZ";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                sb.Append("[");
                sb.Append("{\"id\":\"allchose\",\"text\":\"全选\",\"children\":");
                sb.Append("[");
                #region ///五方主体
                sb.Append("{\"id\":\"CP201703000001\",\"text\":\"五方主体\",\"children\":");
                sb.Append("[");
                foreach (IDictionary<string, string> qylx in qylxs)
                {
                    if (qylx["lxbh"] == "11" || qylx["lxbh"] == "12" || qylx["lxbh"] == "13" || qylx["lxbh"] == "14" || qylx["lxbh"] == "15")// 施工单位
                    {
                        if (qylx["lxbh"] == "11")// 施工单位
                        {
                            sb.Append("{\"id\":\"sgdw\",\"text\":\"施工单位\",\"children\":");

                        }
                        if (qylx["lxbh"] == "12")// 监理单位
                        {
                            sb.Append("{\"id\":\"jldw\",\"text\":\"监理单位\",\"children\":");
                        }
                        if (qylx["lxbh"] == "13")// 建设单位
                        {
                            sb.Append("{\"id\":\"jsdw\",\"text\":\"建设单位\",\"children\":");
                        }
                        if (qylx["lxbh"] == "14")// 设计单位
                        {
                            sb.Append("{\"id\":\"sjdw\",\"text\":\"设计单位\",\"children\":");
                        }
                        if (qylx["lxbh"] == "15")// 勘察单位
                        {
                            sb.Append("{\"id\":\"kcdw\",\"text\":\"勘察单位\",\"children\":");
                        }
                        sb.Append("[");
                        foreach (IDictionary<string, string> qyxx in list)
                        {
                            if (qyxx["qylxs"].Contains(qylx["lxbh"]))
                            {
                                sb.Append("{\"id\":\"" + qyxx["zh"] + "\",\"text\":\"" + qyxx["qymc"] + "\"},");

                            }
                        }
                        if (sb.ToString().EndsWith(","))
                            sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                        sb.Append("},");
                    }


                }
                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append("},");
                //sb.Append("]");
                #endregion

              
                // 获取部门
                IList<RemoteUserService.VUser> users = BD.Jcbg.Web.Remote.UserService.FileShareUsers;

                List<KeyValuePair<string, string>> deps = new List<KeyValuePair<string, string>>();
                foreach (RemoteUserService.VUser user in users)
                {
                    var q = from e in deps where e.Key.Equals(user.DEPCODE, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() == 0 && (depcode == "" || user.DEPCODE == depcode))
                        deps.Add(new KeyValuePair<string, string>(user.DEPCODE, user.DEPNAME));
                }
                if (nocompany == "")
                    sb.Append("{\"id\":\"" + CurrentUser.CompanyCode + "\",\"text\":\"" + CurrentUser.CompanyName + "\",\"children\":");
                sb.Append("[");
                foreach (KeyValuePair<string, string> dep in deps)
                {
                    sb.Append("{\"id\":\"" + dep.Key + "\",\"text\":\"" + dep.Value + "\"");
                    var q = from e in users where e.DEPCODE.Equals(dep.Key, StringComparison.OrdinalIgnoreCase) orderby e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
                    if (q.Count() > 0)
                    {
                        sb.Append(",\"children\":[");
                        foreach (RemoteUserService.VUser user in q)
                        {
                            if (user != q.First())
                                sb.Append(",");
                            sb.Append("{\"id\":\"" + user.USERCODE + "\",\"text\":\"" + user.REALNAME + "\"}");
                        }
                        sb.Append("]");
                    }
                    sb.Append("},");
                }
                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                if (nocompany == "")
                    sb.Append("}]");

                //////////////////////
                sb.Append("}");
                sb.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

                Response.Write(sb.ToString());
                Response.End();
            }
        }
        #endregion


        #region 获取今日代办工作
        [Authorize]
        public void GetTodayWorkTodoList()
        {
            IList<IDictionary<string, string>> tasks = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select workserial from JDBG_YSSQJL where CONVERT(varchar(100), yssj, 23)=CONVERT(varchar(100), GETDATE(), 23)";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if(list.Count>0)
                {
                    string serialno = list[0]["workserial"].GetSafeString();
                    sql = "select * from ViewTodoTask where serialno='" + serialno + "' and userid='" + CurrentUser.UserName + "'";
                    tasks = CommonService.GetDataTable(sql);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", tasks.Count, jss.Serialize(tasks)));
            }
        }
        #endregion

        #region 工程函数
        /// <summary>
        /// 提交工程
        /// </summary>
        [Authorize]
        public void SubmitGc()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_gc set zt='LR', ZJSPYX=1, AJYSWC=1  where gcbh='" + gcbh + "' and zt='YT'");

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region
        /// <summary>
        /// 企业录入人员
        /// </summary>
        [Authorize]
        public void SetRydw()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                string sql = "select yhzh,ryxm from View_I_M_RYZH where qybh in (" + rybh.FormatSQLInStr() + ") ";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                string usercodes = "";
                string ryxms = "";
                for (int i = 0; i < dt.Count; i++)
                {
                    usercodes +=dt[i]["yhzh"]+",";
                    ryxms += dt[i]["ryxm"] + ",";
                }
                usercodes=usercodes.Trim(',');
                ryxms = ryxms.Trim(',');
                string qymc="";
                string qybh="";
                sql="select qymc,qybh from i_m_qy where qybh=(select qybh from i_m_qyzh where yhzh='"+CurrentUser.UserName+"')";
                IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
                if(list.Count>0)
                {
                    qymc=list[0]["qymc"];
                    qybh=list[0]["qybh"];
                }
                if(qymc!=""&&qybh!="")
                {
                    string content = qymc + "想要录用你，请点击连接进行确认。";
                    string procstr = string.Format("FlowSetCommonMailRYLY('{0}', '{1}','{2}','{3}','{4}')", usercodes, ryxms,content, "企业人员录用", qybh);
                    CommonService.ExecProc(procstr, out msg);
                    code = true;
                }
               
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 企业录用人员，人员进行确认
        /// </summary>
        [Authorize]
        public void qylyryqr()
        {
            bool code = false;
            string msg = "";
            try
            {
                string qybh = Request["qybh"].GetSafeRequest();
                string checkoption = Request["checkoption"].GetSafeRequest();
                if(checkoption=="1")
                {
                    string sql = "select * from i_m_ry where rybh=(select qybh from I_m_QYZH where yhzh='" + CurrentUser.UserName + "') and (qybh is null or qybh='')";
                    IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql);
                    if(dt.Count>0)
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("update i_m_ry set qybh='" + qybh + "' where rybh = (select  qybh  from i_m_qyzh where yhzh='" + CurrentUser.UserName + "') and (qybh is null or qybh='')");
                        code = CommonService.ExecTrans(sqls);
                    }
                    else  //=0
                    {
                        msg = "你已有企业录入，不能重复确认";
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
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