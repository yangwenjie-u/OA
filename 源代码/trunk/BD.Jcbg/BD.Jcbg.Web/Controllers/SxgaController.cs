using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD.Jcbg.Common;
using BD.Jcbg.Web.zm;
using BD.Jcbg.IBll;
using Newtonsoft.Json;
using BD.Jcbg.DataModal.Entities;
using System.IO;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Controllers
{
    public class SxgaController : Controller
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



        private ISxgaService _sxgaService = null;
        private ISxgaService SxgaService
        {
            get
            {
                if (_sxgaService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _sxgaService = webApplicationContext.GetObject("SxgaService") as ISxgaService;
                }
                return _sxgaService;
            }
        }
        #endregion 服务


        //转换过的xml数据
        private string _xmlData;
        //传输方式(post/get)
        private string _httpMethod;


        /// <summary>
        /// 微信界面
        /// </summary>
        /// <returns></returns>
        public void Home()
        {
            //ProduceImage();
            _httpMethod = Request.HttpMethod.ToLower();


            if (_httpMethod == "post")
            {
                //将xml数据转换
                using (Stream stream = HttpContext.Request.InputStream)
                {
                    byte[] byteData = new byte[stream.Length];
                    stream.Read(byteData, 0, (Int32)stream.Length);
                    _xmlData = Encoding.UTF8.GetString(byteData);
                }
                //判断传输过来的xml是否有数据
                Response.Write(_xmlData.GetSafeString());
                Response.End();
            }
            else//get方式
            {
                string echoString = Request.QueryString["echostr"];
                Response.Write(echoString.GetSafeString());
                Response.End();
            }

        }

        public ActionResult index()
        {

            return View("Index");
        }

        /// <summary>
        /// 满意率统计
        /// </summary>
        /// <returns></returns>
        public ActionResult lookTb()
        {
            string date1 = Request["date1"].GetSafeString();
            string BS = "Z";
            if (date1.StartsWith("Y"))//月开头,默认周
            {
               BS = "Y";
            }
            ViewData["BS"] = BS;
            ViewData["date1"] = date1;
            ViewData["title"] = BS.Equals("Y") ? "满意率月统计" : "满意率周统计";
            return View("lookTbPhone");
        }

        /// <summary>
        /// 用于获取图表数据
        /// </summary>
        /// <returns></returns>
        public string getTbData()
        {
            string date1 = Request["date1"].GetSafeString();
            string pro = Request["pro"].GetSafeString();
            string date1_ = "";
            string BS = "Z";
            if (date1.StartsWith("Y"))//月开头,默认周
            {
                BS = "Y";
            }

            IList<IDictionary<string, string>> tblist = new List<IDictionary<string, string>>();

            try
            {

                date1_ = System.Text.RegularExpressions.Regex.Replace(date1, @"[^0-9]+", "");
                DateTime dt = DateTime.Now;
                if (!date1_.Equals(""))
                {
                    dt = DateTime.ParseExact(date1_, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                }


                if (pro.Equals("true"))
                {
                    tblist = SxgaService.getTbDataByPro(dt, BS);
                }
                else {
                    tblist = SxgaService.getTbData(dt, BS);
                }
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);

                return "{\"success\":false,\"msg\":\""+ex.Message.GetSafeString()+"\"}";
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(tblist) + "}";
           
        }


        /// <summary>
        /// 获取不满意的短信
        /// </summary>
        /// <returns></returns>
        public string getBmyData() {
            string type = Request["T"].GetSafeString();
            string date1 = Request["date1"].GetSafeString();
            StringBuilder sb = new StringBuilder();

            try
            {



                IList<IDictionary<string, string>> ret = SxgaService.getBmyData(type, date1);

                for (int i = 0; i < ret.Count; i++)
                {
                    sb.Append("地区:" + ret[i]["areaname"].GetSafeString() + ",事项:" + ret[i]["policetype"].GetSafeString() + ",办理结果:" + ret[i]["transactresult"].GetSafeString() + ",用户:" + ret[i]["applyname"].GetSafeString() + ",电话:" + ret[i]["telphone"].GetSafeString() + ",回复内容:" + ret[i]["replycontext"].GetSafeString() + "," + "<br/>");
                }
                if (ret.Count == 0)
                {
                    sb.Append("没有不满意的数据");
                }
            }
            catch (Exception ex) {
                SysLog4.WriteLog(ex);
                sb.Append(ex.Message.GetSafeString());
                
            }

            return sb.ToString();
        }


        /*
        public string send()
        {

            string id = Request["id"].GetSafeString();


            tools tl = new tools();
            WebserivceJson responseJson = new WebserivceJson();
            responseJson.Code = "fail";
            string msg = "";
            try
            {
                var messageReceiveService = (IMessageReceiveService)ServiceManager.GetService("MessageReceiveService");

                
                //DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
                //
                //long startTime = timemillis.GetSafeLong();
                //long endTime = Convert.ToInt64((DateTime.Now - DateStart).TotalMilliseconds);
                //
                //string newSecurity = MD5Util.StringToMD5Hash(timemillis + "bdsoft");
                
                //校验验证字段是否正确
                if (tl.CheckWebservice2("81234567", "7e63ef5ffcaccead7c24edd1f14783be"))
                {
                    responseJson.Code = "verify";
                    responseJson.Msg = "校验失败，code不符合要求，请检查！";
                    return JsonConvert.SerializeObject(responseJson);
                }

                string requestJson = id;

                if (requestJson.GetSafeString().Equals(""))
                {
                    responseJson.Code = "fail";
                    responseJson.Msg = "SMSdata数据不能为空!";
                    return JsonConvert.SerializeObject(responseJson);
                }

                //List<MessageReceive> messageReceiveList = JsonConvert.DeserializeObject<List<MessageReceive>>(requestJson);
                List<MessageReceive> messageReceiveList = new List<MessageReceive>();
                MessageReceive messageReceive = JsonConvert.DeserializeObject<MessageReceive>(requestJson);
                messageReceiveList.Add(messageReceive);
                int i = messageReceiveService.InsertData(messageReceiveList, "81234567", out msg);
                if (i > 0)
                {
                    responseJson.Code = "success";
                    responseJson.Msg = "上传并保存成功！";
                }
                else if (i == 0)
                {
                    responseJson.Code = "success";
                    responseJson.Msg = "数据为空！";
                }
                else
                {
                    responseJson.Code = "fail";
                    responseJson.Msg = msg;
                }
            }
            catch (JsonReaderException exception)
            {
                SysLog4.WriteError(exception.Message);
                responseJson.Code = "analyse";
                responseJson.Msg = "Json解析出错，请检查json格式!";

            }
            catch (JsonSerializationException exception)
            {
                SysLog4.WriteError(exception.Message);
                responseJson.Code = "analyse";
                responseJson.Msg = "Json解析出错，请检查json格式!";
            }
            catch (Exception exception)
            {
                SysLog4.WriteError(exception.Message);
                responseJson.Code = "error";
                responseJson.Msg = "程序出错，请联系管理员!";

            }
            return JsonConvert.SerializeObject(responseJson);


        }

        */

        public string tj() {

            string date1 = Request["date1"].GetSafeString();
            string ret;
            DateTime dt;

            try
            {
                string date1_ = System.Text.RegularExpressions.Regex.Replace(date1, @"[^0-9]+", "");
                if (date1_.Equals(""))
                {
                    dt = DateTime.Now;
                }
                else {
                    dt = DateTime.ParseExact(date1_, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                }

                ret = SxgaService.CreateData(dt, "Y").GetSafeString().ToLower();
                if (ret.Equals("false"))
                    SxgaService.CreateData(dt, "Z").GetSafeString().ToLower();
                else
                    ret = SxgaService.CreateData(dt, "Z").GetSafeString().ToLower();
                

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);

                return "false";
            }


            return ret;
        
        }

        /*
        public string ztj()
        {
            try
            {
               
            DateTime dqdt = DateTime.Now;  //当前时间
            int dayOfWeek = Convert.ToInt32(dqdt.DayOfWeek.ToString("d"));
            int dayOfMonth = Convert.ToInt32(dqdt.Day);

            DateTime weekStart = dqdt.AddDays(1 - ((dayOfWeek == 0) ? 7 : dayOfWeek));   //本周周一
            DateTime weekEnd = weekStart.AddDays(6);  //本周周日
            DateTime beforelastWeekStart = weekStart.AddDays(-7);  //上周周一
            DateTime beforelastWeekEnd = weekEnd.AddDays(-7);  //上周周日
            DateTime beforeMouthStartDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);//上月1号
            DateTime beforeMouthEndDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);//上月最后31号

            IList<string> sqls = new List<string>();
            string sql = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> z_ret = new List<IDictionary<string, string>>();
            string clum = "";
            string clumvalue = "";
            string yb = "";
            string my = "";
            string bmy = "";
            if (dayOfWeek == 1||true)
            {
                sql = "SELECT * FROM MessageTJ where bs='Z' and  startdate='" + beforelastWeekStart.ToString("yyyy-MM-dd") + "' and enddate='" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "'";
                ret = CommonService.GetDataTable(sql);
                
                if (ret.Count==0)
                {
                    sql = " SELECT    AREANAME,  ZS, CONVERT(decimal(18, 2), my * 100.0 / ZS)  AS MYL,yb,my,bmy "
                    + " FROM        (select COUNT(*) AS ZS,                                       "
                    + "  SUM(CASE IsSatisfied WHEN '1' THEN 1 ELSE 0 END) AS MY,                  "
                    + "  SUM(CASE IsSatisfied WHEN '2' THEN 1 ELSE 0 END) AS YB,                  "
                    + "   SUM(CASE IsSatisfied WHEN '3' THEN 1 ELSE 0 END) AS BMY, areaname       "
                    + " from MessageReceive where DATASTATE = '1' AND (LEN(LSH) > 0)              "
                    + " and updatetime>='" + beforelastWeekStart.ToString("yyyy-MM-dd") + "' and updatetime<='" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "'                 "
                    + " GROUP BY     AREANAME ) as t    order by AREANAME";

                    ret = CommonService.GetDataTable(sql);
                    sqls.Clear();
                    clum = "";
                    clumvalue = "";
                    yb = "";
                    bmy = "";
                    my = "";
                    for (int i = 0; i < ret.Count;i++ )
                    {
                        clum += ret[i]["areaname"] + ",";
                        clumvalue += ret[i]["myl"] + ",";
                        yb += ret[i]["yb"] + ",";
                        bmy += ret[i]["bmy"] + ",";
                        my += ret[i]["my"] + ",";
                    }

                    if (ret.Count>0)
                    {
                        clum = clum.Trim(',');
                        clumvalue = clumvalue.Trim(',');
                        yb = yb.Trim(',');
                        bmy = bmy.Trim(',');
                        my = my.Trim(',');
                        sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                            + "values('" + beforelastWeekStart.ToString("yyyy-MM-dd") + "',"
                            + "'" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "','Z','" + clum + "',"
                            + "'" + clumvalue + "','" + "绍兴市"+ beforelastWeekStart.ToString("yyyy年MM月dd日") + "至" + beforelastWeekEnd.ToString("yyyy年MM月dd日") + "周统计'"
                            + ",'0','" + yb + "','" + my + "','" + bmy + "')";
                        sqls.Add(sql);

                       
                    }


                    string z_policetype = "";
                    clum = "";
                    clumvalue = "";
                    yb = "";
                    bmy = "";
                    my = "";
                    sql = " SELECT    POLICETYPE,AREANAME,  ZS, CONVERT(decimal(18, 2), my * 100.0 / ZS)   AS MYL ,yb,my,bmy   "
                        + " FROM        (select COUNT(*) AS ZS,                                                   "
                        + "  SUM(CASE IsSatisfied WHEN '1' THEN 1 ELSE 0 END) AS MY,                              "
                        + "  SUM(CASE IsSatisfied WHEN '2' THEN 1 ELSE 0 END) AS YB,                              "
                        + "   SUM(CASE IsSatisfied WHEN '3' THEN 1 ELSE 0 END) AS BMY, AREANAME  ,POLICETYPE               "
                        + " from MessageReceive where DATASTATE = '1' AND (LEN(LSH) > 0)     "
                        + " and updatetime>='" + beforelastWeekStart.ToString("yyyy-MM-dd") + "' and updatetime<='" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "'                 "
                        + " GROUP BY   AREANAME,POLICETYPE ) as t  order by POLICETYPE  ";
                    z_ret = CommonService.GetDataTable(sql);
                    for (int j = 0; j < z_ret.Count; j++)
                    {
                        if (z_policetype.Equals(z_ret[j]["policetype"]) && !z_policetype.Equals(""))
                        {
                            clum += z_ret[j]["areaname"] + ",";
                            clumvalue += z_ret[j]["myl"] + ",";
                            yb += z_ret[j]["yb"] + ",";
                            bmy += z_ret[j]["bmy"] + ",";
                            my += z_ret[j]["my"] + ",";
                        }
                        else {

                            if (!z_policetype.Equals(""))
                            {
                                clum = clum.Trim(',');
                                clumvalue = clumvalue.Trim(',');
                                yb = yb.Trim(',');
                                bmy = bmy.Trim(',');
                                my = my.Trim(',');
                                sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                                    + "values('" + beforelastWeekStart.ToString("yyyy-MM-dd") + "',"
                                    + "'" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "','Z','" + clum + "',"
                                    + "'" + clumvalue + "','" + z_ret[j - 1]["policetype"].GetSafeString() + beforelastWeekStart.ToString("yyyy年MM月dd日") + "至" + beforelastWeekEnd.ToString("yyyy年MM月dd日") + "周统计'"
                                    + ",'1','" + yb + "','" + my + "','" + bmy + "')";
                                sqls.Add(sql);
                            }
                            z_policetype = z_ret[j]["policetype"].GetSafeString();
                            clum = z_ret[j]["areaname"] + ",";
                            clumvalue = z_ret[j]["myl"] + ",";
                            yb = z_ret[j]["yb"] + ",";
                            bmy = z_ret[j]["bmy"] + ",";
                            my = z_ret[j]["my"] + ",";
                        }

                    }

                    if (z_ret.Count > 0) {
                        clum = clum.Trim(',');
                        clumvalue = clumvalue.Trim(',');
                        yb = yb.Trim(',');
                        bmy = bmy.Trim(',');
                        my = my.Trim(',');
                        sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                            + "values('" + beforelastWeekStart.ToString("yyyy-MM-dd") + "',"
                            + "'" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "','Z','" + clum + "',"
                            + "'" + clumvalue + "','" + z_ret[z_ret.Count - 1]["policetype"].GetSafeString() + beforelastWeekStart.ToString("yyyy年MM月dd日") + "至" + beforelastWeekEnd.ToString("yyyy年MM月dd日") + "周统计'"
                            + ",'1','" + yb + "','" + my + "','" + bmy + "')";
                        sqls.Add(sql);

                        CommonService.ExecTrans(sqls);
                    }

                   
                }


            }
            if (dayOfMonth == 1||true)
            {


                sql = "SELECT * FROM MessageTJ where bs='Y' and  startdate='" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "' and enddate='" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "'";
                ret = CommonService.GetDataTable(sql);

                if (ret.Count == 0)
                {
                    sql = " SELECT    AREANAME,  ZS, CONVERT(decimal(18, 2), my * 100.0 / ZS)  AS MYL,yb,my,bmy "
                    + " FROM        (select COUNT(*) AS ZS,                                       "
                    + "  SUM(CASE IsSatisfied WHEN '1' THEN 1 ELSE 0 END) AS MY,                  "
                    + "  SUM(CASE IsSatisfied WHEN '2' THEN 1 ELSE 0 END) AS YB,                  "
                    + "   SUM(CASE IsSatisfied WHEN '3' THEN 1 ELSE 0 END) AS BMY, areaname       "
                    + " from MessageReceive where DATASTATE = '1' AND (LEN(LSH) > 0)              "
                    + " and updatetime>='" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "' and updatetime<='" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "'                 "
                    + " GROUP BY     AREANAME ) as t    order by AREANAME";

                    ret = CommonService.GetDataTable(sql);
                    sqls.Clear();
                    clum = "";
                    clumvalue = "";
                    yb = "";
                    bmy = "";
                    my = "";
                    for (int i = 0; i < ret.Count; i++)
                    {
                        clum += ret[i]["areaname"] + ",";
                        clumvalue += ret[i]["myl"] + ",";
                        yb += ret[i]["yb"] + ",";
                        bmy += ret[i]["bmy"] + ",";
                        my += ret[i]["my"] + ",";
                    }

                    if (ret.Count > 0)
                    {
                        clum = clum.Trim(',');
                        clumvalue = clumvalue.Trim(',');
                        yb = yb.Trim(',');
                        bmy = bmy.Trim(',');
                        my = my.Trim(',');
                        sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                            + "values('" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "',"
                            + "'" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "','Y','" + clum + "',"
                            + "'" + clumvalue + "','" + "绍兴市" + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "周统计'"
                            + ",'0','" + yb + "','" + my + "','" + bmy + "')";
                        sqls.Add(sql);


                    }


                    string y_policetype = "";
                    clum = "";
                    clumvalue = "";
                    yb = "";
                    bmy = "";
                    my = "";
                    sql = " SELECT    POLICETYPE,AREANAME,  ZS, CONVERT(decimal(18, 2), my * 100.0 / ZS)   AS MYL ,yb,my,bmy   "
                        + " FROM        (select COUNT(*) AS ZS,                                                   "
                        + "  SUM(CASE IsSatisfied WHEN '1' THEN 1 ELSE 0 END) AS MY,                              "
                        + "  SUM(CASE IsSatisfied WHEN '2' THEN 1 ELSE 0 END) AS YB,                              "
                        + "   SUM(CASE IsSatisfied WHEN '3' THEN 1 ELSE 0 END) AS BMY, AREANAME  ,POLICETYPE               "
                        + " from MessageReceive where DATASTATE = '1' AND (LEN(LSH) > 0)     "
                        + " and updatetime>='" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "' and updatetime<='" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "'                 "
                        + " GROUP BY   AREANAME,POLICETYPE ) as t  order by POLICETYPE  ";
                    z_ret = CommonService.GetDataTable(sql);
                    for (int j = 0; j < z_ret.Count; j++)
                    {
                        if (y_policetype.Equals(z_ret[j]["policetype"]) && !y_policetype.Equals(""))
                        {
                            clum += z_ret[j]["areaname"] + ",";
                            clumvalue += z_ret[j]["myl"] + ",";
                            yb += z_ret[j]["yb"] + ",";
                            bmy += z_ret[j]["bmy"] + ",";
                            my += z_ret[j]["my"] + ",";
                        }
                        else
                        {

                            if (!y_policetype.Equals(""))
                            {
                                clum = clum.Trim(',');
                                clumvalue = clumvalue.Trim(',');
                                yb = yb.Trim(',');
                                bmy = bmy.Trim(',');
                                my = my.Trim(',');
                                sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                                    + "values('" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "',"
                                    + "'" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "','Y','" + clum + "',"
                                    + "'" + clumvalue + "','" + z_ret[j - 1]["policetype"].GetSafeString() + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "周统计'"
                                    + ",'1','" + yb + "','" + my + "','" + bmy + "')";
                                sqls.Add(sql);
                            }
                            y_policetype = z_ret[j]["policetype"].GetSafeString();
                            clum = z_ret[j]["areaname"] + ",";
                            clumvalue = z_ret[j]["myl"] + ",";
                            yb = z_ret[j]["yb"] + ",";
                            bmy = z_ret[j]["bmy"] + ",";
                            my = z_ret[j]["my"] + ",";
                        }

                    }

                    if (z_ret.Count > 0)
                    {
                        clum = clum.Trim(',');
                        clumvalue = clumvalue.Trim(',');
                        yb = yb.Trim(',');
                        bmy = bmy.Trim(',');
                        my = my.Trim(',');
                        sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                            + "values('" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "',"
                            + "'" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "','Y','" + clum + "',"
                            + "'" + clumvalue + "','" + z_ret[z_ret.Count - 1]["policetype"].GetSafeString() + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "周统计'"
                            + ",'1','" + yb + "','" + my + "','" + bmy + "')";
                        sqls.Add(sql);

                        CommonService.ExecTrans(sqls);
                    }


                }

            }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                return "-1";
            }

            return "1";
        }

        */


        #region 处理完成MessagePush中不满意的短信回访，标记为是
        [Authorize]
        public void completeMsgPush()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recids = Request["RECIDS"].GetSafeString();
                recids = recids.Trim(',');
                IList<string> sqls = new List<string>();
                sqls.Add("update MessagePush set sfcl='是' where RECID in(" + recids + ") ");
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

        #endregion  



    }
}