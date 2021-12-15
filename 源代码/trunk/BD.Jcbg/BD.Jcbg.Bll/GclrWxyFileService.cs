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
    public class GclrWxyFileService : IGclrWxyFileService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        IGclrWxyFileDao GclrWxyFileDao { get; set; }
        #endregion

        public GclrWxyFile Get(int fileid) {

            return GclrWxyFileDao.Get(fileid);
        }


        public bool deleteGclrWxyFile(int fileid, out string msg)
        {
            msg = "删除成功!";
            try
            {
                GclrWxyFileDao.Delete(fileid);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }


        public bool saveGclrWxyFile(GclrWxyFile GclrWxyFile, out string msg)
        {
            msg = "上传成功!";
            try
            {
                GclrWxyFileDao.Save(GclrWxyFile);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;
        }


        public GclrWxyFile  Save(GclrWxyFile GclrWxyFile)
        {
            return GclrWxyFileDao.Save(GclrWxyFile);
        }
    }
}
