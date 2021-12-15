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
    public class ReportWWGZService : IReportWWGZService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        IReportWWGZDao ReportWWGZDao { get; set; }
        #endregion

        public ReportWWGZ Get(int attachID) {

            return ReportWWGZDao.Get(attachID);
        }

        public void Update(ReportWWGZ itm)
        {
            ReportWWGZDao.Update(itm);
        }



        public bool deleteReportWWGZ(ReportWWGZ itm, out string msg)
        {
            msg = "删除成功!";
            try
            {
                ReportWWGZDao.Delete(itm);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }

        public bool deleteReportWWGZ(int ReportWWGZid, out string msg)
        {
            msg = "删除成功!";
            try
            {
                ReportWWGZDao.Delete(ReportWWGZid);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }


        public bool saveReportWWGZ(ReportWWGZ ReportWWGZ, out string msg)
        {
            msg = "上传成功!";
            try
            {
                ReportWWGZDao.Save(ReportWWGZ);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;
        }


        public ReportWWGZ  Save(ReportWWGZ ReportWWGZ)
        {
            return ReportWWGZDao.Save(ReportWWGZ);
        }
    }
}
