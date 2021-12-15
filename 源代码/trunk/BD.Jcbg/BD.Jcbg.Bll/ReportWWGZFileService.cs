using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class ReportWWGZfileService : IReportWWGZfileService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        IReportWWGZfileDao ReportWWGZfileDao { get; set; }
        #endregion

        public ReportWWGZfile Get(int attachID)
        {

            return ReportWWGZfileDao.Get(attachID);
        }

        public void Update(ReportWWGZfile itm)
        {
            ReportWWGZfileDao.Update(itm);
        }



        public bool deleteReportWWGZfile(ReportWWGZfile itm, out string msg)
        {
            msg = "删除成功!";
            try
            {
                ReportWWGZfileDao.Delete(itm);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }

        public bool deleteReportWWGZfile(int ReportWWGZfileid, out string msg)
        {
            msg = "删除成功!";
            try
            {
                ReportWWGZfileDao.Delete(ReportWWGZfileid);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }


        public bool saveReportWWGZfile(ReportWWGZfile ReportWWGZfile, out string msg)
        {
            msg = "上传成功!";
            try
            {
                ReportWWGZfileDao.Save(ReportWWGZfile);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;
        }


        public ReportWWGZfile Save(ReportWWGZfile ReportWWGZfile)
        {
            return ReportWWGZfileDao.Save(ReportWWGZfile);
        }
    }
}
