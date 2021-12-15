using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IDao;
using Spring.Data.NHibernate.Generic.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DaoSqlServer
{
    public class SysLogPicDao : HibernateDaoSupport, ISysLogPicDao
    {
        public IList<SysLogPic> Gets(string userCode)
        {
            string hql = "from SysLogPic ";

            if (!string.IsNullOrEmpty(userCode))
                hql += " where userCode='" + userCode + "' order by createTime ";

            return HibernateTemplate.Find<SysLogPic>(hql);
        }

        public SysLogPic Save(SysLogPic sysLogPic)
        {
            try
            {
                HibernateTemplate.Save(sysLogPic);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return sysLogPic;
        }
    }
}
