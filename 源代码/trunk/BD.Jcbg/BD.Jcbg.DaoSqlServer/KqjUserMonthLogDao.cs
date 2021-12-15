using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.IDao;
using NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.DaoSqlServer
{
	/// <summary>
	/// 用户月考勤记录操作
	/// </summary>
	public class KqjUserMonthLogDao : HibernateDaoSupport, IKqjUserMonthLogDao
	{
		/// <summary>
		/// 身份证+单位+工种+班组负责人+年+月 唯一确定一条记录
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="companyid"></param>
		/// <param name="gzid"></param>
		/// <param name="bzfzr"></param>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		public KqjUserMonthLog Get(string userid, string companyid, string gzid,  int year, int month,string ScheduleId)
		{
            string hql = string.Format("from KqjUserMonthLog where UserId='{0}' and CompanyId='{1}' and GzId='{2}'  and LogYear={3} and LogMonth={4} and ScheduleId='{5}'",
                userid, companyid, gzid, year, month, ScheduleId);
			IList<KqjUserMonthLog> logs = HibernateTemplate.Find<KqjUserMonthLog>(hql);
			if (logs.Count == 0)
				return null;
			return logs[0];
		}
		/// <summary>
		/// 保存某天的考勤记录
		/// </summary>
		/// <param name="recid"></param>
		/// <param name="day"></param>
		/// <param name="logtype"></param>
		/// <returns></returns>
		public bool SetLog(int recid, int day, string logtype, DateTime dt)
		{
			string field = "LogDay";
            if (logtype == UserLogType.In)
                field += "In";
            else 
                field += "Out";
			field += day;
        
			bool ret = true;
			IDataReader reader = null;
			ISession session = this.SessionFactory.OpenSession();
			try
			{
				bool needupdate = true;

				IDbCommand cmd = session.Connection.CreateCommand();
				cmd.CommandType = CommandType.Text;

				string hql = "select "+field+" from KqjUserMonthLog where Recid=" + recid;
				cmd.CommandText = hql;
				reader = cmd.ExecuteReader();
				if (reader.Read())
				{                  
					DateTime oldDt = DataFormat.GetSafeDate(reader[0]);
                    if (logtype == UserLogType.In)
					{
						if (oldDt.Year != 1900 && oldDt.TimeOfDay.CompareTo(dt.TimeOfDay) < 0)
							needupdate = false;
					}
                    if (logtype == UserLogType.Out)
					{
						if (oldDt.Year != 1900 && oldDt.TimeOfDay.CompareTo(dt.TimeOfDay) > 0)
							needupdate = false;
					}
               
				}
				reader.Close();
				if (needupdate)
				{
					hql = "update KqjUserMonthLog set " + field + "=convert(datetime,'" + dt.ToString() + "') where Recid=" + recid;
					cmd.CommandText = hql;
					ret = cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{
				if (reader != null)
					reader.Close();
				session.Close();
			}
			return ret;
		}
		/// <summary>
		/// 插入一个考勤记录
		/// </summary>
		/// <param name="KqjUserDayLog"></param>
		/// <returns></returns>
		public void SaveLog(KqjUserMonthLog log)
		{
			HibernateTemplate.Save(log);
		}
		/// <summary>
		/// 获取考勤列表
		/// </summary>
		/// <param name="sfzhm"></param>
		/// <param name="y1"></param>
		/// <param name="m1"></param>
		/// <param name="y2"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public IList<KqjUserMonthLog> Gets(string sfzhm, string y1, string m1, string y2, string m2)
		{
			string hql = string.Format("from KqjUserMonthLog where (UserId='{0}' or Bzfzr='{0}') and LogYear>={1} and LogYear<={2} and LogMonth>={3} and LogMonth<={4} order by ProjectName,UserId,LogYear,LogMonth",
				sfzhm, y1, y2,m1,m2);
			return HibernateTemplate.Find<KqjUserMonthLog>(hql);
		}
        /// <summary>
        /// 更新月工资册
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="realpay"></param>
        /// <param name="advance"></param>
        /// <param name="paid"></param>
        /// <returns></returns>
        public bool UpdateUserMonthPay(string gcbh, string userid, string realname, string year, string month, string gzgz, string shouldpay, string realpay, string advance,string worknum)
        {
            bool ret = true;
            if (gzgz==""&& shouldpay==""&&realpay == "" && advance == "") return ret;
            string HaveKQJL = "";
            if (worknum != "0")
                HaveKQJL = "1"; //有考勤
            else
                HaveKQJL = "2";//没考勤记录
            shouldpay = shouldpay == "" ? "0" : shouldpay;
            realpay = realpay == "" ? "0" : realpay;
            advance = advance == "" ? "0" : advance;
            float paid = float.Parse(shouldpay) - float.Parse(realpay) - float.Parse(advance);

            gzgz = gzgz == "" ? "0" : gzgz;
            string companyid = (string)CurrentUser.GetSession("INFODWBH");
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                string hql = "select userid  from InfoUserMonthPay where logyear=" + year+
                    " and logmonth="+month+
                    " and userid ='"+userid+
                    "' and companyid='" + companyid +"' and projectid='"+ gcbh +"';";
                cmd.CommandText = hql;
                if(cmd.ExecuteScalar()==null)
                {
                    string sql = "Insert INTO InfoUserMonthPay  (userid,realname,companyid,projectid,pricegz,shouldpay,realpay,advance,paid,HaveKQJL,logyear,logmonth) VALUES( '" + 
                        userid + "','"
                      + realname + "','"
                      + companyid + "','"
                      + gcbh + "','"
                      + gzgz + "','"
                      + shouldpay + "','"
                      + realpay + "','" 
                      + advance + "','"
                      + paid.ToString() + "','"
                      + HaveKQJL + "','"
                      + year + "','" 
                      + month + "');";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }
                else
                {
                    string sql = "Update InfoUserMonthPay  SET realpay='" + realpay 
                        + "',advance='" + advance 
                        + "',paid='" + paid.ToString()
                        + "',pricegz='" + gzgz
                        + "',shouldpay='" + shouldpay
                        + "' where userid='" + userid + "'and logyear='" + year + "' and logmonth='" + month + "' and companyid='" + companyid + "' and projectid='" + gcbh +  "';";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                return false;
            }
            finally
            {
                session.Close();
            }
            return ret;

        }

        public   bool JudgeMonthPayLog(string gcbh, string userid,string realname, string year, string month,out string msg)
        {
            bool ret = false;
            msg = "";
            string companyid = (string)CurrentUser.GetSession("INFODWBH");
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                string sql = "select * from Infowgry where GCBH='" + gcbh + "'" +
                        "and sfzhm='" + userid + "'" +
                        "and realname='" + realname + "'";
                cmd.CommandText = sql;
                if (cmd.ExecuteScalar() == null)
                {
                    ret = false;
                    msg = "该工程没有该人员,请先录入";
                }
                if(msg=="")
                {
                    string hql = "select userid  from ViewWgryMonthPay where logyear=" + year +
                   " and logmonth=" + month +
                   " and userid ='" + userid +
                   "' and companyid='" + companyid + "' and projectid='" + gcbh + "';";
                    cmd.CommandText = hql;
                    if (cmd.ExecuteScalar() != null)
                    {
                        ret = false;
                        msg = "该人员有考勤记录,无法添加";
                    }
                    else
                        ret = true;
                }       
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = "保存失败";
            }
            finally
            {
                session.Close();
            }
            return ret;
        }
        /// <summary>
        /// 设置工资册明细
        /// </summary>
        /// <typeparam name="IDictionary"></typeparam>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool SetMonthDetails(string recid,string gcbh,string realname, string GzName, string logyear, string logmonth, string Bc, string workcontent, string classprice, string areanum, string unitprice, string summoney)
        {
            bool ret = true;
            string companyid = (string)CurrentUser.GetSession("INFODWBH");
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //string hql = "select recid  from InfoMonthDetails where logyear=" + logyear+"and logmonth="+logmonth+" and RealName ='"+realname+"' and WorkContent='"+workcontent+"';";
                string hql = "select recid  from InfoMonthDetails where recid='"+recid+ "';";
                cmd.CommandText = hql;
                object res=cmd.ExecuteScalar();
                if ( res== null)
                {
                    string sql = "Insert INTO InfoMonthDetails  (CompanyId,ProjectId,RealName,GzName,LogYear,LogMonth,Bc,WorkContent,Classprice,AreaNum,UnitPrice,SumMoney) VALUES ( '"
                        + companyid + "','"
                        + gcbh + "','"
                        + realname + "','" 
                        + GzName + "','" 
                        + logyear + "','" 
                        + logmonth + "','" 
                        + Bc + "','" 
                        + workcontent + "','" 
                        + classprice + "','" 
                        + areanum + "','" 
                        + unitprice + "','" 
                        + summoney 
                        + "');";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }
                else
                {
                    string sql = "Update InfoMonthDetails  SET Bc='" + Bc + "',WorkContent='" + workcontent + "',Classprice='" + classprice 
                        + "',AreaNum='" + areanum + "',UnitPrice='" + unitprice + "',SumMoney='" + summoney +
                        "' where recid='" + recid +  "';";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                return false;
            }
            finally
            {
                session.Close();
            }
            return ret;
        }
       /// <summary>
       /// 设置单价
       /// </summary>
       /// <param name="recid"></param>
       /// <param name="priceGz"></param>
       /// <param name="userid"></param>
       /// <param name="realname"></param>
       /// <param name="logyear"></param>
       /// <param name="logmonth"></param>
       /// <param name="shouldpay"></param>
       /// <returns></returns>
        public bool SetMonthDetailsPrice(string recid,string gcbh, string priceGz, string userid, string realname, string logyear, string logmonth, string shouldpay)
        {
            bool ret = true;

            string companyid = (string)CurrentUser.GetSession("INFODWBH");
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                string hql = "select *  from InfoUserMonthPay where logyear=" + logyear + "and logmonth=" + logmonth 
                    + " and userid ='" + userid
                    + "' and companyid='"+companyid
                    +"' and projectid='" + gcbh +"';";
                cmd.CommandText = hql;
                IDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string _realpay = reader["realpay"].ToString() == "" ? "0" : reader["realpay"].ToString();
                    string _advance = reader["advance"].ToString() == "" ? "0" : reader["advance"].ToString();
                    float paid = float.Parse(shouldpay) - float.Parse(_realpay) - float.Parse(_advance);
                    reader.Close();
                    reader = null;
                    string Strpaid = paid.ToString();
                    string sql = "Update InfoUserMonthPay  SET pricegz='" + priceGz + 
                        "',shouldpay='" + shouldpay +
                        "',paid='" + Strpaid +
                        "' where userid='" + userid + "'and logyear='" + logyear + "' and logmonth='" + logmonth 
                        + "' and companyid='" + companyid
                        + "' and projectid='" + gcbh + "';";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }
                else
                {
                    reader.Close();
                    reader = null;
                    string sql = "Insert INTO InfoUserMonthPay  (userid,realname,companyid,projectid,priceGz,shouldpay,logyear,logmonth) VALUES( '" +
                      userid + "','"
                    + realname + "','"
                    + companyid + "','"
                    + gcbh + "','"
                    + priceGz + "','"
                    + shouldpay + "','"
                    + logyear + "','"
                    + logmonth + "');";
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery() > 0;
                }
                if(reader!=null)
                   reader.Close();

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                return false;
            }
            finally
            {
                session.Close();
            }
            return ret;
        }
	}
}
