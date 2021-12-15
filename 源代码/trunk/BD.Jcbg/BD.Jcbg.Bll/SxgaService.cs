using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using Spring.Context;
using Spring.Context.Support;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{

    public class SxgaService : ISxgaService
    {
        #region 用到的Dao

        ICommonDao CommonDao { get; set; }
        #endregion



        public IList<IDictionary<string, string>> getTbDataByPro(DateTime dt, string BS)
        {
            IList<IDictionary<string, string>> tblist = new List<IDictionary<string, string>>();
            try
            {

                tblist = CommonDao.GetTbData(BS,dt.ToString("yyyy-MM-dd"));

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                tblist.Clear();
            }

            return tblist;
        }


        public IList<IDictionary<string, string>> getTbData(DateTime dt, string BS)
        {
            IList<IDictionary<string, string>> tblist = new List<IDictionary<string, string>>();
            try
            {
                
                string sql = "";
                sql = "select * from MessageTJ "
                    + "where bs='" + BS + "' and startdate<='" + dt.ToString("yyyy-MM-dd") + "' and enddate>='" + dt.ToString("yyyy-MM-dd") + "' order by orderno";
                tblist = CommonDao.GetDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                tblist.Clear();
            }

            return tblist;
        }

        public IList<IDictionary<string, string>> getBmyData(string type,string dtstr){
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

            try
            {
                string dt1 = "";
                string dt2 = "";
                string date1_ = System.Text.RegularExpressions.Regex.Replace(dtstr, @"[^0-9]+", "");
                if (date1_.Equals(""))
                {
                    dt1 = "getdate()";
                    dt2 = "getdate()-1";
                }
                else
                {
                    DateTime dt = DateTime.ParseExact(date1_, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    dt1 = "CONVERT(varchar(10) , CONVERT(datetime,'" + dt.ToString("yyyy-MM-dd") + "',120), 120 )";
                    dt2 = "CONVERT(varchar(10) , CONVERT(datetime,'" + dt.ToString("yyyy-MM-dd") + "',120), 120 )";
                }

                string sql;
                if (type.ToUpper().Equals("X"))
                {
                    //下午不满意
                    sql = "select * from MessagePush where  issatisfied in ('2','3') and "
                        + "  updatetime >=CONVERT(varchar(10) , " + dt2 + ", 120 )+' 12:00:00' "
                        + " and updatetime <=CONVERT(varchar(10) , " + dt2 + ", 120 )+' 23:59:59' ";
                }
                else//上午
                {
                    //上午不满意
                    sql = "select * from MessagePush where  issatisfied in ('2','3') and "
                        + "  updatetime >=CONVERT(varchar(10) ," + dt1 + ", 120 )+' 00:00:00' "
                        + " and updatetime <CONVERT(varchar(10) , " + dt1 + ", 120 )+' 12:00:00' ";
                }

                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                throw new Exception(ex.Message.GetSafeString());

            }


            return ret;
        }


        [Transaction(ReadOnly = false)]
        public bool CreateData(DateTime dt, string BS)
        {
            IList<string> sqls = new List<string>();
            IList<IDictionary<string, string>> tblist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            string sql = "";
            try
           {
               DateTime dqdt;
               if (dt == null)
                   dqdt = DateTime.Now;  //当前时间
               else
                   dqdt = dt;

               int dayOfWeek = Convert.ToInt32(dqdt.DayOfWeek.ToString("d"));
               int dayOfMonth = Convert.ToInt32(dqdt.Day);

               DateTime weekStart = dqdt.AddDays(1 - ((dayOfWeek == 0) ? 7 : dayOfWeek));   //本周周一
               DateTime weekEnd = weekStart.AddDays(6);  //本周周日
               DateTime beforelastWeekStart = weekStart.AddDays(-7);  //上周周一
               DateTime beforelastWeekEnd = weekEnd.AddDays(-7);  //上周周日
               DateTime MouthStartDay = DateTime.Parse(dqdt.ToString("yyyy-MM-01"));//本月1号
               DateTime MouthEndDay = DateTime.Parse(dqdt.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1);//本月最后31号
               string startdate = "";
               string enddate = "";
               if (BS.ToUpper().Equals("Y"))
               {
                   BS = "Y";
                   startdate = MouthStartDay.ToString("yyyy-MM-dd");
                   enddate = MouthEndDay.ToString("yyyy-MM-dd");
               }
               else {
                   BS = "Z";
                   startdate = weekStart.ToString("yyyy-MM-dd");
                   enddate = weekEnd.ToString("yyyy-MM-dd");
               }
               

               sql = "SELECT * FROM MessageTJ where bs='" + BS + "' and  startdate='" + startdate + "' and enddate='" +enddate + "'";
               ret = CommonDao.GetDataTable(sql);
               if (ret.Count==0)
               {
                   sqls.Clear();
                   tblist = CommonDao.GetTbData(BS, dqdt.ToString("yyyy-MM-dd"));
                   for (int i = 0; i < tblist.Count; i++)
                   {
                       sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy, hf, istartdate, ienddate, bstext, ititle) "
                               + "values('" + tblist[i]["startdate"].GetSafeString() + "',"
                               + "'" + tblist[i]["enddate"].GetSafeString() + "',"
                               + "'" + tblist[i]["bs"].GetSafeString() + "',"
                               + "'" + tblist[i]["clum"].GetSafeString() + "',"
                               + "'" + tblist[i]["clumvalue"].GetSafeString() + "',"
                               + "'" + tblist[i]["title"].GetSafeString() + "',"
                               + "" + tblist[i]["orderno"].GetSafeInt() + ","
                               + "'" + tblist[i]["yb"].GetSafeString() + "',"
                               + "'" + tblist[i]["my"].GetSafeString() + "',"
                               + "'" + tblist[i]["bmy"].GetSafeString() + "',"
                               + "'" + tblist[i]["hf"].GetSafeString() + "',"
                               + "'" + tblist[i]["istartdate"].GetSafeString() + "',"
                               + "'" + tblist[i]["ienddate"].GetSafeString() + "',"
                               + "'" + tblist[i]["bstext"].GetSafeString() + "',"
                               + "'" + tblist[i]["ititle"].GetSafeString() + "');";
                       sqls.Add(sql);
                   }

               }


               foreach (string str in sqls)
                   CommonDao.ExecCommand(str, CommandType.Text);

           }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                return false;
            }

            return true;


            /**以下是老方式，作废
           IList<string> sqls = new List<string>();

           try
           {
               
               DateTime dqdt;
               if (dt==null)
                   dqdt = DateTime.Now;  //当前时间
               else
                   dqdt = dt;
                  
               int dayOfWeek = Convert.ToInt32(dqdt.DayOfWeek.ToString("d"));
               int dayOfMonth = Convert.ToInt32(dqdt.Day);

               DateTime weekStart = dqdt.AddDays(1 - ((dayOfWeek == 0) ? 7 : dayOfWeek));   //本周周一
               DateTime weekEnd = weekStart.AddDays(6);  //本周周日
               DateTime beforelastWeekStart = weekStart.AddDays(-7);  //上周周一
               DateTime beforelastWeekEnd = weekEnd.AddDays(-7);  //上周周日
               DateTime beforeMouthStartDay = DateTime.Parse(dqdt.ToString("yyyy-MM-01")).AddMonths(-1);//上月1号
               DateTime beforeMouthEndDay = DateTime.Parse(dqdt.ToString("yyyy-MM-01")).AddDays(-1);//上月最后31号

                
               //StringBuilder sqls = new StringBuilder();
               sqls.Clear();
               string sql = "";
               IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
               IList<IDictionary<string, string>> z_ret = new List<IDictionary<string, string>>();
               string clum = "";
               string clumvalue = "";
               string yb = "";
               string my = "";
               string bmy = "";
               if (dayOfWeek == 1 || true)
               {
                   sql = "SELECT * FROM MessageTJ where bs='Z' and  startdate='" + beforelastWeekStart.ToString("yyyy-MM-dd") + "' and enddate='" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "'";
                   ret = CommonDao.GetDataTable(sql);

                   if (ret.Count == 0)
                   {
                       sql = " SELECT    AREANAME,  ZS, CONVERT(decimal(18, 2), my * 100.0 / ZS)  AS MYL,yb,my,bmy "
                       + " FROM        (select COUNT(*) AS ZS,                                       "
                       + "  SUM(CASE IsSatisfied WHEN '1' THEN 1 ELSE 0 END) AS MY,                  "
                       + "  SUM(CASE IsSatisfied WHEN '2' THEN 1 ELSE 0 END) AS YB,                  "
                       + "   SUM(CASE IsSatisfied WHEN '3' THEN 1 ELSE 0 END) AS BMY, areaname       "
                       + " from MessageReceive where DATASTATE = '1' AND (LEN(LSH) > 0)              "
                       + " and updatetime>='" + beforelastWeekStart.ToString("yyyy-MM-dd") + "' and updatetime<='" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "'                 "
                       + " GROUP BY     AREANAME ) as t    order by AREANAME";

                       ret = CommonDao.GetDataTable(sql);
                      // sqls.Clear();
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
                               + "values('" + beforelastWeekStart.ToString("yyyy-MM-dd") + "',"
                               + "'" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "','Z','" + clum + "',"
                               + "'" + clumvalue + "','" + "绍兴市" + beforelastWeekStart.ToString("yyyy年MM月dd日") + "至" + beforelastWeekEnd.ToString("yyyy年MM月dd日") + "周统计'"
                               + ",'0','" + yb + "','" + my + "','" + bmy + "');";
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
                       z_ret = CommonDao.GetDataTable(sql);
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
                           else
                           {

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
                                       + ",'1','" + yb + "','" + my + "','" + bmy + "');";
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

                       if (z_ret.Count > 0)
                       {
                           clum = clum.Trim(',');
                           clumvalue = clumvalue.Trim(',');
                           yb = yb.Trim(',');
                           bmy = bmy.Trim(',');
                           my = my.Trim(',');
                           sql = "insert into MessageTJ(startdate, enddate, bs, clum, clumvalue, title, orderno,yb,my,bmy) "
                               + "values('" + beforelastWeekStart.ToString("yyyy-MM-dd") + "',"
                               + "'" + beforelastWeekEnd.ToString("yyyy-MM-dd") + "','Z','" + clum + "',"
                               + "'" + clumvalue + "','" + z_ret[z_ret.Count - 1]["policetype"].GetSafeString() + beforelastWeekStart.ToString("yyyy年MM月dd日") + "至" + beforelastWeekEnd.ToString("yyyy年MM月dd日") + "周统计'"
                               + ",'1','" + yb + "','" + my + "','" + bmy + "');";
                           sqls.Add(sql);



                          // CommonDao.execSql(sqls.ToString());
                          // foreach (string str in sqls)
                         //      CommonDao.ExecCommand(str, CommandType.Text);
                         //  CommonDao.ExecCommand(sqls.ToString(), CommandType.Text);
                       }


                   }


               }
               if (dayOfMonth == 1 || true)
               {


                   sql = "SELECT * FROM MessageTJ where bs='Y' and  startdate='" + beforeMouthStartDay.ToString("yyyy-MM-dd") + "' and enddate='" + beforeMouthEndDay.ToString("yyyy-MM-dd") + "'";
                   ret = CommonDao.GetDataTable(sql);

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

                       ret = CommonDao.GetDataTable(sql);
                     //  sqls.Clear();
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
                               + "'" + clumvalue + "','" + "绍兴市" + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "月统计'"
                               + ",'0','" + yb + "','" + my + "','" + bmy + "');";
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
                       z_ret = CommonDao.GetDataTable(sql);
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
                                       + "'" + clumvalue + "','" + z_ret[j - 1]["policetype"].GetSafeString() + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "月统计'"
                                       + ",'1','" + yb + "','" + my + "','" + bmy + "');";
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
                               + "'" + clumvalue + "','" + z_ret[z_ret.Count - 1]["policetype"].GetSafeString() + beforeMouthStartDay.ToString("yyyy年MM月dd日") + "至" + beforeMouthEndDay.ToString("yyyy年MM月dd日") + "月统计'"
                               + ",'1','" + yb + "','" + my + "','" + bmy + "');";
                           sqls.Add(sql);



                         //  foreach (string str in sqls)
                           //    CommonDao.ExecCommand(str, CommandType.Text);
                           //  CommonDao.execSql(sqls.ToString());
                   
                          //CommonDao.ExecCommand(sqls.ToString(), CommandType.Text);
                       }


                   }

               }


               foreach (string str in sqls)
                   CommonDao.ExecCommand(str, CommandType.Text);

           }
           catch (Exception ex)
           {
               SysLog4.WriteLog(ex);
               return false;
           }

           return true;
                **/
        }
    }
}
