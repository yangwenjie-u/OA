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
    public class AlertService : IAlertService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        IAlertDao AlertDao { get; set; }
        #endregion

        public Alert Get(int attachID) {

            return AlertDao.Get(attachID);
        }

        public void Update(Alert itm)
        {
            AlertDao.Update(itm);
        }



        public bool deleteAlert(Alert itm, out string msg)
        {
            msg = "删除成功!";
            try
            {
                AlertDao.Delete(itm);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }

        public bool deleteAlert(int alertid, out string msg)
        {
            msg = "删除成功!";
            try
            {
                AlertDao.Delete(alertid);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }


        public bool saveAlert(Alert Alert, out string msg)
        {
            msg = "上传成功!";
            try
            {
                AlertDao.Save(Alert);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;
        }


        public Alert  Save(Alert Alert)
        {
            return AlertDao.Save(Alert);
        }
    }
}
