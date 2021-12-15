using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

using BD.Jcbg.Common;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BD.Jcbg.Web.Func
{
    public class SXGA
    {



        #region 服务
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
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



        private static ISxgaService _sxgaService = null;
        private static ISxgaService SxgaService
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


        #endregion

        #region 定义


        private static Thread thread;                                   //定时器
        private readonly static int threadTime = 60 * 1000;      //24 * 60 * 60 * 1000;         //间隔发送时间
        private static bool threadflag = false;                         //线程循环标记
 


        private const string lt_companyid_ = "LT_COMPANYID";         //联通企业编号
        private const string lt_username_ = "LT_USERNAME";         //联通用户名
        private const string lt_password_ = "LT_PASSWORD";         //联通密码
        private const string lt_send_ = "LT_SEND";             //联通发送短信url
        private const string lt_report_ = "LT_REPORT";          //联通回执接口url
        private const string lt_reply_ = "LT_REPLY";           //联通上行回复内容查询url
        private const string lt_replyconfirm_ = "LT_REPLYCONFIRM";    //联通上行回复内容确认接口url
        private const string bqts_url_ = "BQTS_URL";    //短信发送错误时推送给开发者
        private const string bqts_qy_ = "BQTS_QY";    //0表示恢复启用,其他则无操作


        private static Dictionary<string, string> dic_ = new Dictionary<string, string>();
        private static Boolean BQTS_QY_Flag_ = true;

        private static bool swtsflag_ = true;                         //上午推送标记
        private static bool xwtsflag_ = true;                         //下午推送标记

        private static bool weekflag_ = true;                         //周推送标记
        private static bool monthflag_ = true;                         //月推送标记

       

        #endregion

        /// <summary>
        /// 停止线程
        /// </summary>
        public static void StopSend()
        {
            threadflag = false;         //停止线程
            thread.Abort();
        }


        /// <summary>
        /// 开始启动发送线程
        /// </summary>
        public static void StartSend()
        {
            threadflag = true;          //开始线程循环标记
            thread = new Thread(new ThreadStart(ThreadOpt));
            thread.Start();
        }






        /// <summary>
        /// 发送线程
        /// zhwx_equipremind表中去判断是否已跟踪和检定
        /// </summary>
        public static void ThreadOpt()
        {


            //循环
            while (threadflag)
            {
                  string bqts_url_2="";

                try
                {
                    int hour = DateTime.Now.Hour;
                    int min = DateTime.Now.Minute;

                    string bqts_url = getValue(bqts_url_).GetSafeString();
                    bqts_url_2 = bqts_url;
                    string lt_companyid = getValue(lt_companyid_).GetSafeString();
                    string lt_username = getValue(lt_username_).GetSafeString();
                    string lt_password = getValue(lt_password_).GetSafeString();
                    string lt_data = "SpCode=" + lt_companyid + "&LoginName=" + lt_username + "&Password=" + lt_password;
                    string lt_send = getValue(lt_send_).GetSafeString();
                    string lt_report = getValue(lt_report_).GetSafeString();
                    string lt_reply = getValue(lt_reply_).GetSafeString();
                    string lt_replyconfirm = getValue(lt_replyconfirm_).GetSafeString();

                    IList<string> sqls = new List<string>();
                    string sql = "";
                    //处理短信发送事务
                    if (hour >= 8 && hour <= 18) {

                        sql = "SELECT  top 5000   t1.uuid,t1.lsh, t1.phonenumber, t1.context,t1.servicecode,t2.updatetime,t2.recid  FROM     MessageAct  t1 "
                            + " left join  sxga_send_phone_xz t2 on t1.phonenumber = t2.phone and t1.servicecode = t2.code WHERE    status = '0' order by createdate asc";
                        
                        
                        IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> sxga_send_phone_xz_ret = new List<IDictionary<string, string>>();
                        ret = CommonService.GetDataTable(sql);
                        for (int i = 0; i < ret.Count; i++) {

                            if (i % 1000 == 0 && i > 0)
                            {
                                //每隔1000休息三秒
                                Thread.Sleep(3000);
                            }

                            string uuid = ret[i]["uuid"].GetSafeString();
                            string lsh = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                            string send_updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string phonenumber = ret[i]["phonenumber"].GetSafeString();
                            string context = ret[i]["context"].GetSafeString();

                            string servicecode = ret[i]["servicecode"].GetSafeString();

                            string sxga_send_phone_xz_updatetime = ret[i]["updatetime"].GetSafeString();
                            string sxga_send_phone_xz_recid = ret[i]["recid"].GetSafeString();


                            sql = "select top 1  updatetime, recid  from sxga_send_phone_xz  where phone='" + phonenumber + "' and code = '" + servicecode + "'";
                            sxga_send_phone_xz_ret = CommonService.GetDataTable(sql);
                            if (sxga_send_phone_xz_ret.Count > 0)
                            {
                                sxga_send_phone_xz_updatetime = sxga_send_phone_xz_ret[0]["updatetime"].GetSafeString();
                                sxga_send_phone_xz_recid = sxga_send_phone_xz_ret[0]["recid"].GetSafeString();
                            }

                            //sxga_send_phone_xz_updatetime更新日期大于等于当前发送日期短信不发送并作废
                            if (!sxga_send_phone_xz_updatetime.Equals("") && Convert.ToDateTime(send_updatetime).Day <= Convert.ToDateTime(sxga_send_phone_xz_updatetime).Day)
                            {
                                sqls.Clear();
                                sql = "delete from MessageAct   WHERE    status = '0'  and uuid='" + uuid + "';";
                                sqls.Add(sql);
                                sql = "update MessageReceive set  DATASTATE = '0'   WHERE    uuid='" + uuid + "';";
                                sqls.Add(sql);
                                CommonService.ExecTrans(sqls);
                            }
                            else {
                                //发送短信 
                                IDictionary<string, string> send_ret = tools2.AnalyticParam(tools2.getPost(lt_send, lt_data + "&MessageContent=" + context + "&UserNumber=" + phonenumber + "&SerialNumber=" + lsh + "&ExtendAccessNum=&ScheduleTime=&f=1"));

                                if (getPSValue(send_ret, "result").Equals("0"))
                                {
                                    sqls.Clear();
                                    
                                    if (sxga_send_phone_xz_recid.Equals(""))
                                    {//新增到sxga_send_phone_xz
                                        sql = "delete from  sxga_send_phone_xz where  code = '" + servicecode + "' and phone = '" + phonenumber + "' ;";
                                        sqls.Add(sql);
                                        sql = "INSERT INTO sxga_send_phone_xz (code,phone ,updatetime) VALUES ('" + servicecode + "','" + phonenumber + "','" + send_updatetime + "')";
                                    }
                                    else
                                    {//修改sxga_send_phone_xz
                                        sql = "UPDATE sxga_send_phone_xz set updatetime = '" + send_updatetime + "' where recid = " + sxga_send_phone_xz_recid + "";
                                    }
                                    sqls.Add(sql);

                                    sql = "update MessageAct set  status = '1', lsh='" + lsh + "'  WHERE    status = '0'  and uuid='" + uuid + "';";
                                    sqls.Add(sql);

                                    sql = "update MessageReceive set   lsh='" + lsh + "',updatetime='" + send_updatetime + "'  WHERE    uuid='" + uuid + "';";
                                    sqls.Add(sql);
                                    sql = "delete from sxga_receiveys where phone = '" + phonenumber + "';";
                                    sqls.Add(sql);
                                    sql = "INSERT INTO sxga_receiveys(phone,lsh) values('" + phonenumber + "','" + lsh + "');";
                                    sqls.Add(sql);

                                    //发送短信后将信息写入MessageSendBak表
                                    sql = "INSERT INTO  MessageSendBak (lsh,phonenumber ,context,type,replycontext,sendTime) VALUES ('" + lsh + "'  ,'" + phonenumber + "' ,'" + context + "'  ,'1'   ,''  ,CONVERT(varchar(100), GETDATE(), 120))";
                                    sqls.Add(sql);
                                    CommonService.ExecTrans(sqls);
                                }
                                else
                                {
                                    //出错时写入数据库
                                    sqls.Clear();
                                    sql = "update MessageAct set  status = '4'  WHERE    status = '0'  and uuid='" + uuid + "';";
                                    sqls.Add(sql);
                                    sql = "INSERT INTO sxga_error_message(type,result,description,createdate)values('send','" + getPSValue(send_ret, "result") + "','" + getPSValue(send_ret, "description") + "--UUID为:" + uuid + ",号码为:" + phonenumber + "',CONVERT(varchar(100), GETDATE(), 120) );";
                                    sqls.Add(sql);
                                    CommonService.ExecTrans(sqls);
                                    if (BQTS_QY_Flag_)
                                    {
                                        if ((!getPSValue(send_ret, "result").Equals("6") && !getPSValue(send_ret, "result").Equals("7"))) {
                                            tools2.PostJsonData(bqts_url, getPSValue(send_ret, "description") + "--UUID为:" + uuid);
                                            BQTS_QY_Flag_ = false;
                                            sqls.Clear();
                                            sql = "update sxga_syssetting set  settingvalue = '1'  WHERE  SettingCode = '" + bqts_qy_ + "';";
                                            sqls.Add(sql);
                                            CommonService.ExecTrans(sqls);
                                        }

                                    }
                                    else
                                    {
                                        IList<IDictionary<string, string>> BQTS_QY_Flag_ret = new List<IDictionary<string, string>>();
                                        BQTS_QY_Flag_ret = CommonService.GetDataTable("SELECT   settingvalue FROM    sxga_syssetting WHERE  SettingCode = '" + bqts_qy_ + "'");
                                        if (BQTS_QY_Flag_ret[0]["settingvalue"].GetSafeString().Equals("0"))
                                        {
                                            BQTS_QY_Flag_ = true;
                                        }
                                    }

                                }

                                //每次发送短信间隔为1秒
                                Thread.Sleep(1000);
                            }



                        }
                    }


                    //处理短信回复事务
                    if ((hour >= 7 && hour <= 20))
                    {
                        //获取回执
                        IDictionary<string, string> report_ret = tools2.AnalyticParam(tools2.getPost(lt_report, lt_data));
                        if (getPSValue(report_ret, "result").Equals("0"))
                        {
                            string[] outs =  getPSValue(report_ret, "out").Split(';');
                            sqls.Clear();
                            for (int i = 0; i < outs.Length;i++ )
                            {
                                string[] outs_tmp = outs[i].GetSafeString().Split(',');
                                if (outs_tmp.Length==3) {
                                    //回执不成功的标记3--回执不成功的需要重新发送--目前采用不重新发送
                                    if (!outs_tmp[2].GetSafeString().Equals("0"))
                                    {
                                        //sql = "update MessageAct set  status='0' where status='1' and lsh+'_'+phonenumber in (select( lsh+'_'+phone) from sxga_receiveys where lsh='" + outs_tmp[0] + "' and phone='" + outs_tmp[1] + "' );";
                                        //sqls.Add(sql);
                                        sql = "update MessageAct set  status='3' where status='1' and lsh+'_'+phonenumber in (select( lsh+'_'+phone) from sxga_receiveys where lsh='" + outs_tmp[0] + "' and phone='" + outs_tmp[1] + "' );";
                                        sqls.Add(sql);
                                        //回执不成功时，需要删除发送的短信
                                        //sql = "delete from MessageSendBak  where lsh+'_'+phonenumber in (select( lsh+'_'+phone) from sxga_receiveys where lsh='" + outs_tmp[0] + "' and phone='" + outs_tmp[1] + "' );";
                                        //sqls.Add(sql);
                                        //由于受到回执失败时，无法区分来自于哪个警种业务，只能删除全部的 sxga_send_phone_xz，这个还需要考虑
                                        //sql = "delete from  sxga_send_phone_xz where  phone = '" + outs_tmp[1] + "' ;";
                                        //sqls.Add(sql);
                                        sql = "update MessageSendBak set issuccess='False'  where lsh+'_'+phonenumber in (select( lsh+'_'+phone) from sxga_receiveys where lsh='" + outs_tmp[0] + "' and phone='" + outs_tmp[1] + "' );";
                                        sqls.Add(sql);
                                    }
                                    else {
                                        sql = "update MessageSendBak set issuccess='True'  where lsh+'_'+phonenumber in (select( lsh+'_'+phone) from sxga_receiveys where lsh='" + outs_tmp[0] + "' and phone='" + outs_tmp[1] + "' );";
                                        sqls.Add(sql);
                                    }
                                }
                            }
                            CommonService.ExecTrans(sqls);
                        }
                        else {
                            //出错时写入数据库
                            sqls.Clear();
                            sql = "INSERT INTO sxga_error_message(type,result,description,createdate)values('report','" + getPSValue(report_ret, "result") + "','" + getPSValue(report_ret, "description") + "',CONVERT(varchar(100), GETDATE(), 120) );";
                            sqls.Add(sql);
                            CommonService.ExecTrans(sqls);
                        }


                        //上行回复内容查询
                        IDictionary<string, string> reply_ret = tools2.AnalyticParam(tools2.getPost(lt_reply, lt_data));
                        //最后一条回复信息所对应的ID
                        string reply_id = getPSValue(reply_ret, "id");

                        if (getPSValue(reply_ret, "result").Equals("0"))
                        {
                            //获取回复的内容
                            if (!getPSValue(reply_ret, "replys").Equals("")) {
                                sqls.Clear();
                                List<ReplyContent> replyContentList = JsonConvert.DeserializeObject<List<ReplyContent>>(getPSValue(reply_ret, "replys"));
                                for (int i = 0; i < replyContentList.Count;i++ )
                                {
                                    replyContentList[i].Content = replyContentList[i].Content.GetSafeString().Replace("'", "''").Replace("--", "");
                                   
                                    sql = "update MessageAct set  status='2',replycontext='" + replyContentList[i].Content + "' where lsh in (select  lsh from sxga_receiveys where phone='" + replyContentList[i].Mdn + "' );";
                                    sqls.Add(sql);
                                    //假如某个事件添加代码，当这个事件发送二次时则仍不能区分是回复哪个的
                                    //回复1表示满意，2表示不满意，3表示不满意且有话反馈，默认满意
                                    //满意请回复1,一般请回复2,不满意请回复3，有内容反馈请回复4加内容(最新)
                                    string IsSatisfied = "1";//表示是否满意；1满意3不满意2一般
                                    string SatisfiedCode = "1";//用户回复代码
                                  
                                    if (replyContentList[i].Content.GetSafeString().StartsWith("1")) {

                                        IsSatisfied = "1";
                                        SatisfiedCode = "1";
                                    }
                                    else if (replyContentList[i].Content.GetSafeString().StartsWith("2"))
                                    {

                                        IsSatisfied = "2";
                                        SatisfiedCode = "2";
                                    }
                                    else if (replyContentList[i].Content.GetSafeString().StartsWith("3"))
                                    {

                                        IsSatisfied = "3";
                                        SatisfiedCode = "3";
                                    }
                                    else 
                                    {

                                        
                                        if (replyContentList[i].Content.GetSafeString().IndexOf("不满意") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("不好") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("不佳") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("不合格") > -1)
                                        {
                                            IsSatisfied = "3";
                                        }
                                        else if (replyContentList[i].Content.GetSafeString().IndexOf("满意") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("好") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("不差") > -1 || replyContentList[i].Content.GetSafeString().IndexOf("不一般") > -1)
                                        {
                                            IsSatisfied = "1";
                                        }
                                        else if (replyContentList[i].Content.GetSafeString().IndexOf("一般") > -1)
                                        {
                                            IsSatisfied = "2";
                                        }

                                        if (replyContentList[i].Content.GetSafeString().StartsWith("4"))
                                        {
                                            SatisfiedCode = "4";
                                        }
                                        else { 
                                            SatisfiedCode = IsSatisfied;
                                        }
                                       
                                    }

                                    /*
                                    if ((replyContentList[i].Content.GetSafeString().StartsWith("4") || replyContentList[i].Content.GetSafeString().IndexOf("不满意") > -1) && !replyContentList[i].Content.GetSafeString().StartsWith("3") && !replyContentList[i].Content.GetSafeString().StartsWith("2"))
                                    {
                                        IsSatisfied = "4";
                                        SatisfiedCode = "4";
                                    }
                                    else if (replyContentList[i].Content.GetSafeString().StartsWith("2"))
                                    {
                                        IsSatisfied = "2";
                                    }
                                    else if (replyContentList[i].Content.GetSafeString().StartsWith("3"))
                                    {
                                        IsSatisfied = "3";
                                    }
                                    else 
                                    {
                                        IsSatisfied = "1";
                                    }
                                    */

                                    string reply_updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    sql = "update MessageReceive set  updatetime='" + reply_updatetime + "' , IsSatisfied='" + IsSatisfied + "',SatisfiedCode='" + SatisfiedCode + "' ,REPLYCONTEXT='" + replyContentList[i].Content + "'  where lsh in (select lsh from sxga_receiveys where phone='" + replyContentList[i].Mdn + "' )";
                                    sqls.Add(sql);
                                    //不满意的短信加入不满意推送表
                                    if (IsSatisfied.Equals("3") || IsSatisfied.Equals("2"))
                                    {
                                        sql = "insert MessagePush( PROJID, HanderDeptname, HanderDeptid, AREACODE, AREANAME, ACCEPTMAN, ACCEPTTIME, PROMISEVALUE, PROMISETYPE, PROMISEETIME,             "
                                            + "  TRANSACTUSER, TRANSACTTIME, TRANSACTRESULT, SERVICECODE, SERVICENAME, INFOTYPE, APPLYNAME, APPLYCARDTYPE, APPLYCARDNUMBER,          "
                                            + "  CONTACTMAN, CONTACTMANCARDTYPE, CONTACTMANCARDNUMBER, TELPHONE, POSTCODE, ADDRESS, LEGALMAN, DATASTATE, BELONGSYSTEM,               "
                                            + "  POLICETYPECODE, POLICETYPE, EXTEND, LSH, CREATEDATE, IsSatisfied,SatisfiedCode, CONTEXT, REPLYCONTEXT, CODE, UUID, UPDATETIME)                    "
                                            + " select  PROJID, HanderDeptname, HanderDeptid, AREACODE, AREANAME, ACCEPTMAN, ACCEPTTIME, PROMISEVALUE, PROMISETYPE, PROMISEETIME,                         "
                                            + "   TRANSACTUSER, TRANSACTTIME, TRANSACTRESULT, SERVICECODE, SERVICENAME, INFOTYPE, APPLYNAME, APPLYCARDTYPE, APPLYCARDNUMBER,          "
                                            + "   CONTACTMAN, CONTACTMANCARDTYPE, CONTACTMANCARDNUMBER, TELPHONE, POSTCODE, ADDRESS, LEGALMAN, DATASTATE, BELONGSYSTEM,               "
                                            + "  POLICETYPECODE, POLICETYPE, EXTEND, LSH, CREATEDATE,'" + IsSatisfied + "' as  IsSatisfied,'" + SatisfiedCode + "' as  SatisfiedCode, CONTEXT,'" + replyContentList[i].Content + "' as  REPLYCONTEXT,"
                                            + " CODE, UUID,'" + reply_updatetime + "' as UPDATETIME from MessageReceive where DATASTATE ='1' and lsh in (select lsh from sxga_receiveys where phone='" + replyContentList[i].Mdn + "' );";
                                        sqls.Add(sql);
                                    }

                                   //用户回复时，更新回复内容
                                   sql = "update MessageSendBak set REPLYCONTEXT='" + replyContentList[i].Content + "'  where lsh in (select lsh from sxga_receiveys where phone='" + replyContentList[i].Mdn + "' )";
                                   sqls.Add(sql);

                                   sql = "delete from sxga_receiveys where phone = '"  + replyContentList[i].Mdn +  "'";
                                   sqls.Add(sql);


                                }
                                CommonService.ExecTrans(sqls);
                            }    
                            
                        }
                        else
                        {
                            //出错时写入数据库
                            sqls.Clear();
                            sql = "INSERT INTO sxga_error_message(type,result,description,createdate)values('reply','" + getPSValue(reply_ret, "result") + "','" + getPSValue(reply_ret, "description") + "',CONVERT(varchar(100), GETDATE(), 120) );";
                            sqls.Add(sql);
                            CommonService.ExecTrans(sqls);
                        }


                        if (!reply_id.Equals(""))
                        {
                            //上行回复内容确认接口,如果不执行，短信无法获取新的信息
                            IDictionary<string, string> replyconfirm_ret = tools2.AnalyticParam(tools2.getPost(lt_replyconfirm, lt_data + "&id=" + reply_id));
                            if (!getPSValue(replyconfirm_ret, "result").Equals("0")) {
                                //出错时写入数据库
                                sqls.Clear();
                                sql = "INSERT INTO sxga_error_message(type,result,description,createdate)values('replyconfirm','" + getPSValue(replyconfirm_ret, "result") + "','" + getPSValue(replyconfirm_ret, "description") + "',CONVERT(varchar(100), GETDATE(), 120) );";
                                sqls.Add(sql);
                                //删除时间久的数据
                                // sql = "delete from MessageAct   where (status = '2' or CONVERT(varchar(10),createdate)<CONVERT(varchar(10) , getdate()-30, 120 )) and status   in ('1','2')";
                              //  sqls.Add(sql);

                                CommonService.ExecTrans(sqls);
                            }
                        }



                    }



                    if (hour == 1 && min <= 10)
                    {

                        //删除时间久的数据或发送成功的数据
                        // sql = "delete from MessageAct   where (status = '2' or CONVERT(varchar(10),createdate)<CONVERT(varchar(10) , getdate()-30, 120 )) and status   in ('1','2')";
                        sql = "delete from MessageAct   where (status = '2' or CONVERT(varchar(10),createdate)<CONVERT(varchar(10) , getdate()-15, 120 )) and status   in ('1','2')";
                        sqls.Add(sql);

                        CommonService.ExecTrans(sqls);
                    }


                    //上午的推送
                    if (((hour >= 12 && min >= 30) || hour>=13) && hour <= 15 && swtsflag_)
                    {

                        IList<IDictionary<string, string>> sw_bmy_ret = new List<IDictionary<string, string>>();
                        sw_bmy_ret = SxgaService.getBmyData("S","");
                        if (sw_bmy_ret .Count> 0)
                        {
                            swtsflag_ = false;

                            //推送消息
                        }


                    }



                    //下午的推送
                    if (((hour >= 8 && min >= 30) || hour >= 9) && hour <= 11 && xwtsflag_)
                    {
                       
                        IList<IDictionary<string, string>> xw_bmy_ret = new List<IDictionary<string, string>>();
                        xw_bmy_ret = SxgaService.getBmyData("X", "");
                        if (xw_bmy_ret.Count > 0)
                        {
                            xwtsflag_ = false;
                            //推送消息
                           
                        }
                    }

                    //不满意短信推送重新启用
                    if (hour >= 17) {
                        swtsflag_ = true;
                        swtsflag_ = true;
                    }


                    DateTime dqdt = DateTime.Now;  //当前时间
                    int dayOfWeek = Convert.ToInt32(dqdt.DayOfWeek.ToString("d"));
                    int dayOfMonth = Convert.ToInt32(dqdt.Day);

                    IList<IDictionary<string, string>> tblist = new List<IDictionary<string, string>>();
                    bool flag = true;
                    DateTime tjdt = DateTime.Now.AddDays(-4);  //统计时间，统计上月的或者上周的，所以减日期1-7
                    //周推送
                    if (dayOfWeek == 1 && weekflag_)
                    {

                        flag = SxgaService.CreateData(tjdt, "Z");
                        if (flag)
                        {
                            weekflag_ = false;
                            tblist = SxgaService.getTbData(tjdt, "Z");
                            if (tblist.Count>0)
                            {
                                //推送消息
                            }
                        }
                        else {
                            tools2.PostJsonData(bqts_url, "【" + dqdt.ToString("yyyy-MM-dd") + "】" + "统计周视图出错,时间:" + tjdt.ToString("yyyy-MM-dd"));
                        }
                    }

                    //月推送

                    if (dayOfMonth == 1 && monthflag_)
                    {

                        flag = SxgaService.CreateData(tjdt, "Y");
                        if (flag)
                        {
                            monthflag_ = false;
                            tblist = SxgaService.getTbData(tjdt, "Y");
                            if (tblist.Count > 0)
                            {
                                //推送消息
                            }
                        }
                        else
                        {
                            tools2.PostJsonData(bqts_url, "【" + dqdt.ToString("yyyy-MM-dd") + "】" + "统计月视图出错,时间:" + tjdt.ToString("yyyy-MM-dd"));
                        }
                    }

                    //周推送恢复
                    if(dayOfWeek>1)
                        weekflag_ = true;
                    //月推送恢复
                    if(dayOfMonth>1)
                        monthflag_ = true;



                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                    tools2.PostJsonData(bqts_url_2, e.Message);
                                           
                }
                finally
                {
                    Thread.Sleep(threadTime);
                }
            }
        }











        #region 推送消息



        #endregion

        #region 公共方法




        /// <summary>
        /// 用于防止Dictionary中没有相应的key而报错
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getPSValue(IDictionary<string, string> dic, string key)
        {
            string ret = "";
            try
            {
                ret = dic[key].GetSafeString();
            }
            catch
            {
                ret = "";
            }
            return ret;
        }

        public static string getValue(string key)
        {
            Boolean flag = dic_.ContainsKey(key);
            string ret = "";
            if (flag)
            {
                ret = dic_[key].GetSafeString();
            }

            string sql = "";
            if (ret.Equals(""))
            {
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                sql = "select * from   sxga_syssetting where SettingCode='" + key + "'";
                list = CommonService.GetDataTable(sql);
                if (list.Count > 0)
                {
                    ret = list[0]["settingvalue"].GetSafeString();
                    dic_.Add(key, ret);
                }
            }
            return ret;
        }
        #endregion

    }


}