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
    /// 排班操作
    /// </summary>
    public class InfoScheduleDao : HibernateDaoSupport, IInfoScheduleDao
    {
        /// <summary>
        /// 获取一个单位一个工地的排班设置
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public IList<InfoSchedule> Gets(string companyid, string projectid)
        {
            return HibernateTemplate.Find<InfoSchedule>("from InfoSchedule where ProjectId='" + projectid + "' and HasDelete=0 order by StartTime asc");
            //return HibernateTemplate.Find<InfoSchedule>("from InfoSchedule order by StartTime asc");
        }
        /// <summary>
        /// 获取排班
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public InfoSchedule Get(string recid)
        {
            return HibernateTemplate.Get<InfoSchedule>(recid);
        }

    }
}
