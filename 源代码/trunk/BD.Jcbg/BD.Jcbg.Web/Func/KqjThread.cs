using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using System.Threading;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.Web.Func
{
    public class KqjThread
    {
        #region 服务
        private static ISystemService _systemService = null;
        private static ISystemService SystemService
        {
            get
            {
                if (_systemService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                }
                return _systemService;
            }
        }

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

        #endregion

        #region 消息推送现成属性
        private static Thread thread;                                   //定时器
        private readonly static int threadTime = 3 * 60 * 1000;      //间隔发送时间
        private static bool threadflag = false;                         //线程循环标记

        public static void StopSendMsg()
        {
            threadflag = false;         //停止线程
            thread.Abort();
        }

        /// <summary>
        /// 开始启动发送线程
        /// </summary>
        public static void StartSendMsg()
        {


            threadflag = true;          //开始线程循环标记
            thread = new Thread(new ThreadStart(SendThreadOpt));
            thread.Start();
        }

        /// <summary>
        /// 发送线程
        /// </summary>
        private static void SendThreadOpt()
        {
            string a = "";

            //循环
            while (threadflag)
            {
                try
                {

                    dokqcount();

                }
                catch (Exception ex)
                {
                    a = ex.Message;
                }
                Thread.Sleep(threadTime);
            }
        }


        public static bool dokqcount()
        {
            bool ret = true;
            try
            {
                //说明 Stype，考勤时间类型
                //null 没有考勤记录
                //-1，超过考勤时间，无效考勤，
                //-10，迟到或者早退10分钟以内
                //-30，迟到或早退10到30分钟
                //-60， 迟到或者早退30分钟到一个小时
                //-100，迟到或者早退一个小时以上的

                string S1 = "08:30";
                string S2 = "11:30";
                string S3 = "14:00";
                string S4 = "18:00";
                //获取每天四次的考勤时间，
                IList<IDictionary<string, string>> timelist = CommonService.GetDataTable("select * from KqgzPeriod where getdate()>=convert(datetime,convert(nvarchar(20),year(getdate()))+'-'+KqStart) and getdate()<=convert(datetime,convert(nvarchar(20),year(getdate()))+'-'+Kqend) ");
                foreach (IDictionary<string, string> kqtime in timelist)
                {
                    if (kqtime["kqtype"].GetSafeString().Trim() == "1")
                    {
                        S1 = kqtime["kqtime"].GetSafeString();
                    }
                    if (kqtime["kqtype"].GetSafeString().Trim() == "2")
                    {
                        S2 = kqtime["kqtime"].GetSafeString();
                    }
                    if (kqtime["kqtype"].GetSafeString().Trim() == "3")
                    {
                        S3 = kqtime["kqtime"].GetSafeString();
                    }
                    if (kqtime["kqtype"].GetSafeString().Trim() == "4")
                    {
                        S4 = kqtime["kqtime"].GetSafeString();
                    }
                }





                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select usercode,kqdate,realname,kqextr from userinfo where needkq=1");
                for (int i = 0; i < dt.Count; i++)
                {
                    //这里获取的是所有用户
                    IList<string> sqls = new List<string>();

                    //这里需要做一个循环，查询下，处理日期到今天为止，每天增加一个考勤记录，这里还要做一个排除，周末和节假日不加，
                    DateTime predate = dt[i]["kqdate"].GetSafeDate(DateTime.Now).AddDays(-1).ToString("yyyy-MM-dd").GetSafeDate();
                    DateTime nowdate = DateTime.Now.ToString("yyyy-MM-dd").GetSafeDate();
                    TimeSpan sp = DateTime.Now.Subtract(predate);
                    for (int j = 0; j < sp.Days; j++)
                    {
                        DateTime tdate = predate.AddDays(j + 1);
                        bool needadd = false;
                        if (tdate.DayOfWeek == DayOfWeek.Sunday || tdate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            IList<IDictionary<string, string>> dt0 = CommonService.GetDataTable("select count(*) as sum from KqExtrIn where convert(varchar(10),KQDate,23)=convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23)");
                            if (dt0[0]["sum"].GetSafeInt() > 0)
                            {
                                //这里判断周末是不是上班
                                needadd = true;
                            }
                        }
                        else
                        {
                            IList<IDictionary<string, string>> dt0 = CommonService.GetDataTable("select count(*) as sum from KqExtrOut where convert(varchar(10),KQDate,23)=convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23)");
                            if (dt0[0]["sum"].GetSafeInt() == 0)
                            {
                                //这里判断上班时间是不是休假
                                needadd = true;
                            }
                        }
                        //要做一个测试，没有生成统计信息，但是不用考勤人的考勤记录有了，统计信息直接调用是报错

                        if (dt[i]["kqextr"].GetSafeInt() == 1)
                        {
                            //还要增加一个判断，这个人是不是有外出记录了，如果有了，就不要增加了
                            //这里要增加一个考勤记录，如果是某个人在咧外立面的，考勤数据直接输入,判断下，是不是超过考勤时间了，如果是的，就看下是不是这个人没记录，没记录就加一个记录
                            DateTime kqsj1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S1);
                            DateTime kqsj4 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S4);




                            if (DateTime.Compare(DateTime.Now, kqsj1) > 0)
                            {
                                bool needextr = true;
                                IList<IDictionary<string, string>> dt0 = CommonService.GetDataTable("select count(recid) as sum from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and wcsqsjstart<='" + kqsj1.ToString("yyyy-MM-dd HH:mm") + "' and wcsqsjend>='" + kqsj1.ToString("yyyy-MM-dd HH:mm") + "'");
                                if (dt0[0]["sum"].GetSafeInt() > 0)
                                {
                                    needextr = false;
                                }
                                dt0 = CommonService.GetDataTable("select count(recid) as sum from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and qjsjstart<='" + kqsj1.ToString("yyyy-MM-dd HH:mm") + "' and qjsjend>='" + kqsj1.ToString("yyyy-MM-dd HH:mm") + "'");
                                if (dt0[0]["sum"].GetSafeInt() > 0)
                                {
                                    needextr = false;
                                }


                                if (needextr && needadd)
                                {
                                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select count(*) as sum from KqjUserLog where userid='" + dt[i]["usercode"].GetSafeString() + "' and  logdate<='" + kqsj1.ToString("yyyy-MM-dd HH:mm") + "' and  logdate>='" + kqsj1.ToString("yyyy-MM-dd") + "'");
                                    if (dt1[0]["sum"].GetSafeInt() == 0)
                                    {


                                        string mintime = "";
                                        string maxtime = "";
                                        DateTime kqt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S1).AddMinutes(-30);
                                        DateTime kqt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S1);
                                        //获取这个人以前考勤的最早时间和最晚时间
                                        IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select max(CONVERT(varchar(100), logdate, 8)) as t1,min(CONVERT(varchar(100), logdate, 8)) as t2 from KqjUserLog where userid='" + dt[i]["usercode"].GetSafeString() + "' and CONVERT(varchar(100), logdate, 8)<'" + S1 + "'");
                                        if (dt2.Count > 0)
                                        {
                                            mintime = dt2[0]["t2"].GetSafeString();
                                            maxtime = dt2[0]["t1"].GetSafeString();
                                        }
                                        if (mintime != maxtime)
                                        {
                                            if (mintime != "")
                                            {
                                                try
                                                {
                                                    kqt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + mintime);
                                                }
                                                catch { }
                                            }
                                            if (maxtime != "")
                                            {
                                                try
                                                {
                                                    kqt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + maxtime);
                                                }
                                                catch { }
                                            }
                                        }

                                        TimeSpan ts = kqt2 - kqt1;
                                        double timeint = ts.TotalMinutes;
                                        Random rd = new Random();
                                        double tadd = rd.NextDouble() * timeint;

                                        DateTime logdate = kqt1.AddMinutes(tadd);


                                        IList<string> sqls1 = new List<string>();
                                        sqls1.Add("insert into KqjUserLog (UserId,Serial,LogDate,HasDeal) select top 1 '" + dt[i]["usercode"].GetSafeString() + "',KQJBH,'" + logdate.ToString("yyyy-MM-dd HH:mm:ss") + "',0 from I_M_KQJ ");
                                        CommonService.ExecTrans(sqls1);
                                    }
                                }
                            }

                            if (DateTime.Compare(DateTime.Now, kqsj4.AddMinutes(30)) > 0)
                            {
                                bool needextr = true;
                                IList<IDictionary<string, string>> dt0 = CommonService.GetDataTable("select count(recid) as sum from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and wcsqsjstart<='" + kqsj4.ToString("yyyy-MM-dd HH:mm") + "' and wcsqsjend>='" + kqsj4.ToString("yyyy-MM-dd HH:mm") + "'");
                                if (dt0[0]["sum"].GetSafeInt() > 0)
                                {
                                    needextr = false;
                                }
                                dt0 = CommonService.GetDataTable("select count(recid) as sum from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and qjsjstart<='" + kqsj4.ToString("yyyy-MM-dd HH:mm") + "' and qjsjend>='" + kqsj4.ToString("yyyy-MM-dd HH:mm") + "'");
                                if (dt0[0]["sum"].GetSafeInt() > 0)
                                {
                                    needextr = false;
                                }


                                if (needextr && needadd)
                                {
                                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select count(*) as sum from KqjUserLog where userid='" + dt[i]["usercode"].GetSafeString() + "' and  logdate>='" + kqsj4.ToString("yyyy-MM-dd HH:mm") + "' and logdate<'" + kqsj4.AddDays(1).ToString("yyyy-MM-dd") + "' ");
                                    if (dt1[0]["sum"].GetSafeInt() == 0)
                                    {


                                        string mintime = "";
                                        string maxtime = "";
                                        DateTime kqt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S4);
                                        DateTime kqt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + S4).AddMinutes(30);
                                        //获取这个人以前考勤的最早时间和最晚时间
                                        IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select max(CONVERT(varchar(100), logdate, 8)) as t1,min(CONVERT(varchar(100), logdate, 8)) as t2 from KqjUserLog where userid='" + dt[i]["usercode"].GetSafeString() + "' and CONVERT(varchar(100), logdate, 8)>'" + S4 + "'");
                                        if (dt2.Count > 0)
                                        {
                                            mintime = dt2[0]["t2"].GetSafeString();
                                            maxtime = dt2[0]["t1"].GetSafeString();
                                        }
                                        if (mintime != maxtime)
                                        {
                                            if (mintime != "")
                                            {
                                                try
                                                {
                                                    kqt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + mintime);
                                                }
                                                catch { }
                                            }
                                            if (maxtime != "")
                                            {
                                                try
                                                {
                                                    kqt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + maxtime);
                                                }
                                                catch { }
                                            }
                                        }

                                        TimeSpan ts = kqt2 - kqt1;
                                        double timeint = ts.TotalMinutes;
                                        Random rd = new Random();
                                        double tadd = rd.NextDouble() * timeint;

                                        DateTime logdate = kqt1.AddMinutes(tadd);


                                        IList<string> sqls1 = new List<string>();
                                        sqls1.Add("insert into KqjUserLog (UserId,Serial,LogDate,HasDeal) select top 1 '" + dt[i]["usercode"].GetSafeString() + "',KQJBH,'" + logdate.ToString("yyyy-MM-dd HH:mm:ss") + "',0 from I_M_KQJ ");
                                        CommonService.ExecTrans(sqls1);
                                    }
                                }
                            }
                        }


                        if (needadd)
                        {
                            IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select count(*) as sum from KqUserSign where UserCode='" + dt[i]["usercode"].GetSafeString() + "' and convert(varchar(10),SignDate,23)=convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23)");
                            if (dt1[0]["sum"].GetSafeInt() == 0)
                            {
                                KqUserSign sign = new KqUserSign();
                                sign.UserCode = dt[i]["usercode"].GetSafeString();
                                sign.SignDate = tdate.ToString("yyyy-MM-dd");
                                sign.S1Type = "-1";
                                sign.S2Type = "-1";
                                sign.S3Type = "-1";
                                sign.S4Type = "-1";
                                sign.S1Text = "无考勤记录";
                                sign.S2Text = "无考勤记录";
                                sign.S3Text = "无考勤记录";
                                sign.S4Text = "无考勤记录";

                                //if (dt[i]["kqextr"].GetSafeInt() == 1)
                                //{
                                //    sign.S1Type = "0";
                                //    sign.S2Type = "0";
                                //    sign.S3Type = "0";
                                //    sign.S4Type = "0";
                                //    sign.S1Text = "正常";
                                //    sign.S2Text = "正常";
                                //    sign.S3Text = "正常";
                                //    sign.S4Text = "正常";
                                //}

                                SystemService.updateUserSign(sign);
                                /*
                                IList<string> sqls1 = new List<string>();
                                sqls1.Add("insert into KqUserSign(UserCode,SignDate) values('" + dt[i]["usercode"].GetSafeString() + "',convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23))");
                                CommonService.ExecTrans(sqls1);*/
                            }
                        }

                    }


                }
                


                IList<IDictionary<string, string>> dr2 = CommonService.GetDataTable("select * from KqjUserLog where (Hasdeal is null or Hasdeal=0)");
                IList<string> tsqls = new List<string>();
                for (int j = 0; j < dr2.Count; j++)
                {
                    int recid = dr2[j]["recid"].GetSafeInt();
                    DateTime tdate = dr2[j]["logdate"].GetSafeDate();
                    string usercode = dr2[j]["userid"].GetSafeString();
                    IList<IDictionary<string, string>> dr1 = CommonService.GetDataTable("select count(*) as sum from KqUserSign where UserCode='" + usercode + "' and convert(varchar(10),SignDate,23)=convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23)");
                    if (dr1[0]["sum"].GetSafeInt() == 0)
                    {

                        KqUserSign ksign = new KqUserSign();
                        ksign.UserCode = usercode;
                        ksign.SignDate = tdate.ToString("yyyy-MM-dd");
                        ksign.S1Type = "-1";
                        ksign.S2Type = "-1";
                        ksign.S3Type = "-1";
                        ksign.S4Type = "-1";
                        ksign.S1Text = "无考勤记录";
                        ksign.S2Text = "无考勤记录";
                        ksign.S3Text = "无考勤记录";
                        ksign.S4Text = "无考勤记录";
                        SystemService.updateUserSign(ksign);
                        /*
                        IList<string> sqls1 = new List<string>();
                        sqls1.Add("insert into KqUserSign(UserCode,SignDate) values('" + usercode + "',convert(varchar(10),'" + tdate.ToString("yyyy-MM-dd") + "',23))");
                        CommonService.ExecTrans(sqls1);*/
                    }

                    //string sql = "insert into userinfo(username,Realname,Departmentid,departmentname,usercode)values ('" + rvu.USERNAME + "','" + rvu.REALNAME + "','" + rvu.DEPCODE + "','" + rvu.DEPNAME + "','" + rvu.USERCODE + "')";

                    string sql = "";

                    string SignDate = tdate.ToString("yyyy-MM-dd");
                    DateTime dt1 = DateTime.Parse(SignDate + " " + S1);
                    DateTime dt2 = DateTime.Parse(SignDate + " " + S2);
                    DateTime dt3 = DateTime.Parse(SignDate + " " + S3);
                    DateTime dt4 = DateTime.Parse(SignDate + " " + S4);

                    int deadline = 90;


                    KqUserSign sign = SystemService.getUserSign(usercode, SignDate);
                    if (sign == null)
                    {
                        sign = new KqUserSign();
                        sign.UserCode = usercode;
                        sign.SignDate = SignDate;
                        sign.S1Type = "-1";
                        sign.S2Type = "-1";
                        sign.S3Type = "-1";
                        sign.S4Type = "-1";
                        sign.S1Text = "无考勤记录";
                        sign.S2Text = "无考勤记录";
                        sign.S3Text = "无考勤记录";
                        sign.S4Text = "无考勤记录";

                    }

                    if (tdate < dt1.AddMinutes(1))
                    {

                        //这里待会把他改回去，直接把sign实例化掉算了

                        //sql = "update KqUserSign set S1='" + tdate.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "'  ";
                        sign.S1 = tdate;
                        sign.S1Type = "0";
                        sign.S1Text = "正常";
                    }
                    else if (tdate < dt2.AddMinutes(0))
                    {
                        if (sign.S1.GetSafeDate(DateTime.MinValue) == DateTime.MinValue)
                        {
                            sign.S1 = tdate;
                            if (dt1.AddMinutes(10) >= tdate)
                            {
                                sign.S1Type = "-10";
                                sign.S1Text = "迟到（10分钟以内）";
                            }
                            else if (dt1.AddMinutes(30) >= tdate)
                            {
                                sign.S1Type = "-30";
                                sign.S1Text = "迟到（30分钟以内）";
                            }
                            else if (dt1.AddMinutes(60) >= tdate)
                            {
                                sign.S1Type = "-60";
                                sign.S1Text = "迟到（一小时以内）";
                            }
                            else
                            {
                                sign.S1Type = "-100";
                                sign.S1Text = "迟到（一小时以上）";
                            }
                        }
                        else
                        {
                            sign.S2 = tdate;
                            if (dt2.AddMinutes(-10) <= tdate)
                            {
                                sign.S2Type = "-10";
                                sign.S2Text = "早退（10分钟以内）";
                            }
                            else if (dt2.AddMinutes(-30) <= tdate)
                            {
                                sign.S2Type = "-30";
                                sign.S2Text = "早退（30分钟以内）";
                            }
                            else if (dt2.AddMinutes(-60) <= tdate)
                            {
                                sign.S2Type = "-60";
                                sign.S2Text = "早退（一小时以内）";
                            }
                            else
                            {
                                sign.S2Type = "-100";
                                sign.S2Text = "早退（一小时以上）";
                            }
                        }

                    }
                    else if (tdate < dt3.AddMinutes(1))
                    {
                        if ((sign.S2.GetSafeDate(DateTime.MinValue) == DateTime.MinValue && tdate <= dt2.AddMinutes(deadline)) || tdate <= dt2.AddMinutes(deadline))
                        {
                            sign.S2 = tdate;
                            if (tdate.AddMinutes(-deadline) >= dt2)
                            {
                                sign.S2Type = "-1";
                            }
                            else
                            {
                                sign.S2Type = "0";
                                sign.S2Text = "正常";
                            }
                        }
                        else
                        {
                            sign.S3 = tdate;
                            sign.S3Type = "0";
                            sign.S3Text = "正常";
                        }
                    }
                    else if (tdate < dt4.AddMinutes(0))
                    {
                        if (sign.S3.GetSafeDate(DateTime.MinValue) == DateTime.MinValue && tdate <= dt4.AddMinutes(-100))
                        {
                            sign.S3 = tdate;
                            if (dt3.AddMinutes(10) >= tdate)
                            {
                                sign.S3Type = "-10";
                                sign.S3Text = "迟到（10分钟以内）";
                            }
                            else if (dt3.AddMinutes(30) >= tdate)
                            {
                                sign.S3Type = "-30";
                                sign.S3Text = "迟到（30分钟以内）";
                            }
                            else if (dt3.AddMinutes(60) >= tdate)
                            {
                                sign.S3Type = "-60";
                                sign.S3Text = "迟到（一小时以内）";
                            }
                            else
                            {
                                sign.S3Type = "-100";
                                sign.S3Text = "迟到（一小时以上）";
                            }
                        }
                        else
                        {
                            sign.S4 = tdate;
                            if (dt4.AddMinutes(-10) <= tdate)
                            {
                                sign.S4Type = "-10";
                                sign.S4Text = "早退（10分钟以内）";
                            }
                            else if (dt4.AddMinutes(-30) <= tdate)
                            {
                                sign.S4Type = "-30";
                                sign.S4Text = "早退（30分钟以内）";
                            }
                            else if (dt4.AddMinutes(-60) <= tdate)
                            {
                                sign.S4Type = "-60";
                                sign.S4Text = "早退（一小时以内）";
                            }
                            else
                            {
                                sign.S4Type = "-100";
                                sign.S4Text = "早退（一小时以上）";
                            }
                        }
                    }
                    else
                    {
                        sign.S4 = tdate;
                        sign.S4Type = "0";
                        sign.S4Text = "正常";
                    }

                    SystemService.updateUserSign(sign);
                    sql = "update KqjUserLog set Hasdeal=1 where recid=" + recid.ToString();
                    tsqls.Add(sql);
                }

                CommonService.ExecTrans(tsqls);



                //考勤时间需要设置有效期时间段，录入调休的增加一个判断是否已经重复录入的

                //这里要改，开始时间大于今天的，就开始处理，结束大于今天的更新状态，
                //这里只处理没有今天之前的请假，不然可能导致请假了，
                IList<string> wcsqls = new List<string>();

                //因公外出
                IList<IDictionary<string, string>> leaves = CommonService.GetDataTable("select recid,wcry, wcryzh, wcsqsjstart, wcsqsjend,spzt,wcbzsm,nr from View_QJ_YGWCJL where SPZT=1 and (IsDeal=0 or IsDeal is null)");
                foreach (IDictionary<string, string> leave in leaves)
                {
                    string msg = "";
                    string upsql = "update KqUserSign set S1Type='0',S1Text='" + leave["nr"].GetSafeString() + "' where S1Type!='0' and UserCode='" + leave["wcryzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S1 + "') between convert(datetime,'" + leave["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S2Type='0',S2Text='" + leave["nr"].GetSafeString() + "' where S2Type!='0' and UserCode='" + leave["wcryzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S2 + "') between convert(datetime,'" + leave["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S3Type='0',S3Text='" + leave["nr"].GetSafeString() + "' where S3Type!='0' and UserCode='" + leave["wcryzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S3 + "') between convert(datetime,'" + leave["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S4Type='0',S4Text='" + leave["nr"].GetSafeString() + "' where S4Type!='0' and UserCode='" + leave["wcryzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S4 + "') between convert(datetime,'" + leave["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update QJ_YGWCJL set IsDeal=1 where recid in( select recid from View_QJ_YGWCJL where  wcsqsjend< DATEADD(day,0,getdate()) and recid=" + leave["recid"].GetSafeInt().ToString() + " )";
                    wcsqls.Add(upsql);

                }

                CommonService.ExecTrans(wcsqls);
                wcsqls.Clear();
                //因私外出
                IList<IDictionary<string, string>> leaves2 = CommonService.GetDataTable("select recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx,qjyy from View_QJ_YSWCJL where SPZT=1 and (IsDeal=0 or IsDeal is null)");
                foreach (IDictionary<string, string> leave in leaves2)
                {
                    string msg = "";
                    string upsql = "update KqUserSign set S1Type='0',S1Text='" + leave["qjyy"].GetSafeString() + "'  where S1Type!='0' and UserCode='" + leave["qjrzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S1 + "') between convert(datetime,'" + leave["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S2Type='0',S2Text='" + leave["qjyy"].GetSafeString() + "' where S2Type!='0' and UserCode='" + leave["qjrzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S2 + "') between convert(datetime,'" + leave["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S3Type='0',S3Text='" + leave["qjyy"].GetSafeString() + "' where S3Type!='0' and UserCode='" + leave["qjrzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S3 + "') between convert(datetime,'" + leave["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update KqUserSign set S4Type='0',S4Text='" + leave["qjyy"].GetSafeString() + "' where S4Type!='0' and UserCode='" + leave["qjrzh"].GetSafeString() + "' and (convert(datetime,SignDate+' " + S4 + "') between convert(datetime,'" + leave["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "') and convert(datetime,'" + leave["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "')) ";
                    wcsqls.Add(upsql);
                    upsql = "update QJ_YSWCJL set IsDeal=1 where qjsjend< DATEADD(day,0,getdate()) and recid=" + leave["recid"].GetSafeInt().ToString();
                    wcsqls.Add(upsql);

                }


                //


                CommonService.ExecTrans(wcsqls);
                /*
                dt = CommonService.GetDataTable("select usercode,kqdate,realname,kqextr from userinfo where needkq=1 and kqextr=1");
                for (int i = 0; i < dt.Count; i++)
                {
                    
                }*/





            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret = false;
            }

            return ret;
        }


        #endregion
    }
}