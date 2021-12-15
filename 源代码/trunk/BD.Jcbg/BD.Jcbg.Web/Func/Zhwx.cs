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

namespace BD.Jcbg.Web.Func
{
    public class Zhwx
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



        private static ISmsService _smsService = null;
        private static ISmsService SmsService
        {
            get
            {
                if (_smsService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                }
                return _smsService;
            }
        }




        #endregion

        #region 定义


        private static Thread thread;                                   //定时器
        private readonly static int threadTime = 2 * 60 * 60 * 1000;      //24 * 60 * 60 * 1000;         //间隔发送时间
        private static bool threadflag = false;                         //线程循环标记
        // private readonly static int bddays = 30;         //最小标定天数
        //  private readonly static int gzdays = 30;         //最小外借跟踪天数
        //  private readonly static int zhwx_equiprecord_tmp_days = 14;         //zhwx_equiprecord_tmp存在天数


        private const string zhwx_equip_bd_days_ = "ZHWX_EQUIP_BD_DAYS";         //最小标定天数
        private const string zhwx_equip_gz_days_ = "ZHWX_EQUIP_GZ_DAYS";         //最小外借跟踪天数
        private const string zhwx_equiprecord_tmp_days_ = "ZHWX_EQUIPRECORD_TMP_DAYS";         //zhwx_equiprecord_tmp存在天数  
        private const string zhwx_yzm_equiprecord_tmp_gx_hour_ = "ZHWX_YZM_EQUIPRECORD_TMP_GX_HOUR";
        private const string zhwx_equiprecord_gz_bd_gx_hour_strat_ = "ZHWX_EQUIPRECORD_GZ_BD_GX_HOUR_START";
        private const string zhwx_equiprecord_gz_bd_gx_hour_end_ = "ZHWX_EQUIPRECORD_GZ_BD_GX_HOUR_END";


        private const string wx_push_unitname_ = "WX_PUSH_UNITNAME";
        private const string bd_wx_equip_status_template_ = "BD_WX_EQUIP_STATUS_TEMPLATE";
        private const string zhwx_bd_sms_base_appid_ = "ZHWX_BD_SMS_BASE_APPId";
        private const string bd_wx_universal_template_ = "BD_WX_UNIVERSAL_TEMPLATE";
        private static Dictionary<string, string> dic_ = new Dictionary<string, string>();



        private static Boolean isPushMessage = false;


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

                try
                {
                    int bddays = getValue(zhwx_equip_bd_days_).GetSafeInt();
                    int gzdays = getValue(zhwx_equip_gz_days_).GetSafeInt();
                    int zhwx_yzm_equiprecord_tmp_gx_hour = getValue(zhwx_yzm_equiprecord_tmp_gx_hour_).GetSafeInt();
                    int zhwx_equiprecord_gz_bd_gx_hour_strat = getValue(zhwx_equiprecord_gz_bd_gx_hour_strat_).GetSafeInt();
                    int zhwx_equiprecord_gz_bd_gx_hour_end = getValue(zhwx_equiprecord_gz_bd_gx_hour_end_).GetSafeInt();
                    string zhwx_equiprecord_tmp_days = getValue(zhwx_equiprecord_tmp_days_);

                    int hour = DateTime.Now.Hour;
                    IList<string> sqls = new List<string>();
                    string sql = "";

                    if (hour >= zhwx_equiprecord_gz_bd_gx_hour_strat && hour <= zhwx_equiprecord_gz_bd_gx_hour_end && isPushMessage)
                    {

                         int pagesize = 1000;
                         int pageindex = 1;
                         int totalCount = 1001;

                        IDictionary<string, string[]> bd_dics = new Dictionary<string, string[]>();
                        IDictionary<string, string[]> gz_dics = new Dictionary<string, string[]>();
                        bd_dics.Clear();
                        gz_dics.Clear();

                        IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

                        ret = CommonService.GetDataTable(sql);

                        while (Math.Ceiling(totalCount * 1.0 / pagesize) >= pageindex )
                        {
                            Thread.Sleep(500);
                            sql = "select t1.sfjl,t4.sfgz,CONVERT(varchar, t1.xcbdrq, 120 ) as xcbdrq ,t1.lrrxm,t1.lrrzh"
                                + " ,t1.recid, DATEDIFF(day, getdate(),t1.xcbdrq) as days "
                                + " ,t2.equipid as equipidbd,t3.equipid as equipidgz,t5.phone ,t1.sbmc,t4.statusname"
                                + " from i_m_sb  t1 left join zhwx_equipremind_bd t2 on t1.recid = t2.equipid  "
                                + " left join zhwx_equipremind_gz t3 on t1.recid = t3.equipid "
                                + " left join  zhwx_equipstatus t4 on t1.status = t4.status"
                                + " left join  zhwx_user t5 on t1.lrrzh = t5.usercode where t1.status <> 4";

                            ret = CommonService.GetPageData(sql, pagesize, pageindex, out totalCount);
                            pageindex++;

                            for (int i = 0; i < ret.Count; i++)
                            {
                                string pushername = ret[i]["lrrxm"].GetSafeString();
                                string pusher = ret[i]["lrrzh"].GetSafeString();
                                string days = ret[i]["days"].GetSafeString();
                                string equipid = ret[i]["recid"].GetSafeString();
                                string equipidbd = ret[i]["equipidbd"].GetSafeString();
                                string equipidgz = ret[i]["equipidgz"].GetSafeString();
                                string xcbdrq = ret[i]["xcbdrq"].GetSafeString();
                                string sfgz = ret[i]["sfgz"].GetSafeString();
                                string phone = ret[i]["phone"].GetSafeString();
                                string sbmc = ret[i]["sbmc"].GetSafeString();
                                string statusname = ret[i]["statusname"].GetSafeString();
                                string sfjl = ret[i]["sfjl"].GetSafeString();

                                // Boolean ispush = false;
                                //  Boolean issendmessage = false;

                                //标定
                                if (sfjl.Equals("是") && days != "" && int.Parse(days) <= bddays)
                                {

                                    sqls.Clear();
                                    //下次标定时间存在且小于相应天数则推送给相关保管员
                                    if (equipidbd.Equals(""))
                                    {
                                        //zhwx_equipremind_bd中未存在该设备，需要添加进去
                                        sql = " insert into  zhwx_equipremind_bd(equipid)  values ('" + equipid + "') ";
                                        sqls.Add(sql);
                                        sql = "insert into zhwx_pushmessages(title,context,pusher,pushername,datetime,type,issure,isneedsure,isread,ispush,equipid)"
                                            + " values ('标定提醒','设备【" + sbmc + "】下次标定时间为【" + xcbdrq + "】,请及时标定!','" + pusher + "','" + pushername + "',CONVERT(varchar, getdate(), 120 ),'2','0','0','0','1','" + equipid + "')";
                                        sqls.Add(sql);


                                        //////////////////////////临时使用////////////////////////////////
                                        string unitname = getValue(wx_push_unitname_).GetSafeString();
                                        string info = sbmc + "距离下次标定不到一个月";
                                        // issendmessage = bduniversalpush(phone, unitname + "设备标定提醒", info);
                                        //////////////////////////////////////////////////////////

                                        //issendmessage = bdequipstatuspush(phone, sbmc, statusname, "下次标定时间为(" + xcbdrq + ")");

                                        // if (issendmessage) { }
                                        sql = "update zhwx_bd_message_total set  total = total+1  where phone = '" + phone + "'";
                                        sqls.Add(sql);
                                        CommonService.ExecTrans(sqls);

                                        if (!bd_dics.ContainsKey(phone))
                                        {
                                            bd_dics.Add(phone, new string[] { phone, unitname + "设备标定提醒", "您有设备距离下次标定不到一个月" });

                                        }

                                    }
                                }
                                else
                                {
                                    sqls.Clear();
                                    //大于天数则删除对应zhwx_equipremind_bd表中的设备
                                    sql = " delete from zhwx_equipremind_bd where equipid = '" + equipid + "'";
                                    sqls.Add(sql);
                                    CommonService.ExecTrans(sqls);
                                }

                                //跟踪
                                Boolean notgz = false;
                                if (sfgz.Equals("1"))
                                {
                                    sql = "select  DATEDIFF(day,min(t1.datetime), getdate() ) as gzdays,CONVERT(varchar, min(t1.datetime), 120)  as datetime  from zhwx_equiprecord t1"
                                        + " left join  zhwx_equipstatus t2 on t1.status = t2.status"
                                        + " where t1.datetime> (select case when max(datetime) is null then '1970-01-01 00:00:00' else max(datetime) end "
                                        + " from zhwx_equiprecord where status = '7' and equipid = '" + equipid + "' ) and t2.sfgz = '1'  and  t1.equipid = '" + equipid + "'";
                                    IList<IDictionary<string, string>> gzret = new List<IDictionary<string, string>>();
                                    gzret = CommonService.GetDataTable(sql);
                                    if (gzret.Count > 0)
                                    {
                                        string igzdays = gzret[0]["gzdays"].GetSafeString();
                                        string idatetime = gzret[0]["datetime"].GetSafeString();
                                        if (igzdays != "" && int.Parse(igzdays) >= gzdays)
                                        {
                                            if (equipidgz.Equals(""))
                                            {
                                                sqls.Clear();
                                                //zhwx_equipremind_bd中未存在该设备，需要添加进去
                                                sql = " insert into  zhwx_equipremind_gz(equipid)  values ('" + equipid + "') ";
                                                sqls.Add(sql);
                                                sql = "insert into zhwx_pushmessages(title,context,pusher,pushername,datetime,type,issure,isneedsure,isread,ispush,equipid)"
                                                    + " values ('跟踪提醒','设备【" + sbmc + "】开始租借时间为【" + idatetime + "】,已超一个月，请及时跟踪!','" + pusher + "','" + pushername + "',CONVERT(varchar, getdate(), 120 ),'3','0','0','0','1','" + equipid + "')";
                                                sqls.Add(sql);


                                                //////////////////////////临时使用////////////////////////////////
                                                string unitname = getValue(wx_push_unitname_).GetSafeString();
                                                string info = sbmc + "借出已超一个月，请及时跟踪.";
                                                //issendmessage = bduniversalpush(phone, unitname + "设备跟踪提醒", info);
                                                //////////////////////////////////////////////////////////

                                                // issendmessage = bdequipstatuspush(phone, sbmc, statusname, "开始租借时间为(" + idatetime + "),已超一个月");

                                                //if (issendmessage) { }

                                                sql = "update zhwx_bd_message_total set  total = total+1  where phone = '" + phone + "'";
                                                sqls.Add(sql);
                                                CommonService.ExecTrans(sqls);



                                                if (!gz_dics.ContainsKey(phone))
                                                {
                                                    gz_dics.Add(phone, new string[] { phone, unitname + "设备跟踪提醒", "您有设备借出已超一个月" });

                                                }
                                            }

                                        }
                                        else
                                        {
                                            notgz = true;
                                        }
                                    }
                                    else
                                    {
                                        notgz = true;
                                    }
                                }
                                else
                                {
                                    notgz = true;
                                }

                                if (notgz)
                                {
                                    sqls.Clear();
                                    //大于天数则删除对应zhwx_equipremind_gz表中的设备
                                    sql = " delete from zhwx_equipremind_gz where equipid = '" + equipid + "'";
                                    sqls.Add(sql);
                                    CommonService.ExecTrans(sqls);
                                }


                            }


                            //执行发短信
                            //执行发短信
                            //标定
                            if (false)
                            {
                                foreach (KeyValuePair<string, string[]> kvp in bd_dics)
                                {
                                    bduniversalpush(kvp.Value[0], kvp.Value[1], kvp.Value[2]);
                                    isPushMessage = false;
                                }
                                //跟踪
                                foreach (KeyValuePair<string, string[]> kvp in gz_dics)
                                {
                                    bduniversalpush(kvp.Value[0], kvp.Value[1], kvp.Value[2]);
                                    isPushMessage = false;
                                }

                            }

                        }
                        ////////////while结束///////////////////////////////
                        

                    }



                    if (hour <= zhwx_yzm_equiprecord_tmp_gx_hour)
                    {
                        isPushMessage = true;
                        sqls.Clear();
                        //大于天数则删除对应zhwx_equipremind_gz表中的设备
                        sql = " update zhwx_bd_message_total set yzmday = 0 ";
                        sqls.Add(sql);
                        sql = "delete from zhwx_equiprecord_tmp  where DATEDIFF(day,datetime, getdate() ) >= " + zhwx_equiprecord_tmp_days;
                        sqls.Add(sql);
                        CommonService.ExecTrans(sqls);
                    }

                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                finally
                {
                    Thread.Sleep(threadTime);
                }
            }
        }





        #region 推送消息

        /// <summary>
        /// 发送设备状态提醒
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="shebei"></param>
        /// <param name="status"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static bool bdequipstatuspush(string phone, string shebei, string status, string act)
        {

            bool code = false;
            string msg = "";
            try
            {
                //  string receiver = Request["receiver"].GetSafeString();
                string zhwx_bd_sms_base_appid = getValue(zhwx_bd_sms_base_appid_);
                string unitname = getValue(wx_push_unitname_).GetSafeString();
                if (phone == "")
                    code = false;
                else if (!phone.IsMobile())
                    code = false;
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = getValue(bd_wx_equip_status_template_).GetSafeString();

                    ZhwxWxEquipStatusBdMessage zhwxWxEquipStatusBdMessage = new ZhwxWxEquipStatusBdMessage()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = phone,
                        templateCode = vctemplate,
                        contentVar = new EquipStatusBdMessage()
                        {
                            unitname = unitname,
                            shebei = shebei,
                            status = status,
                            act = act

                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(zhwxWxEquipStatusBdMessage);

                    code = SmsService.SendMessage(zhwx_bd_sms_base_appid, Guid.NewGuid().ToString(), phone, contents, out msg);

                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                return code;
            }

            return code;

        }



        /// <summary>
        /// 百度通用短信推送模板
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="client"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool bduniversalpush(string phone, string client, string info)
        {

            bool code = false;
            string msg = "";
            try
            {
                //  string receiver = Request["receiver"].GetSafeString();
                string zhwx_bd_sms_base_appid = getValue(zhwx_bd_sms_base_appid_);
                if (phone == "")
                    code = false;
                else if (!phone.IsMobile())
                    code = false;
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = getValue(bd_wx_universal_template_);

                    ZhwxUniversalMessage zhwxUniversalMessage = new ZhwxUniversalMessage()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = phone,
                        templateCode = vctemplate,
                        contentVar = new UniversalMessage()
                        {
                            client = client,
                            info = info

                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(zhwxUniversalMessage);

                    code = SmsService.SendMessage(zhwx_bd_sms_base_appid, Guid.NewGuid().ToString(), phone, contents, out msg);

                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                return code;
            }

            return code;

        }

        #endregion

        #region 公共方法
        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);

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
                sql = "select * from   zhwx_syssetting where SettingCode='" + key + "'";
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