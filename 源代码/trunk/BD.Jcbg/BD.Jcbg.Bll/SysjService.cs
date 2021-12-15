using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IDao;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using System.Data;
using System.Web;
using Spring.Transaction.Interceptor;

namespace BD.Jcbg.Bll
{
	public class SysjService : ISysjService
	{
		#region 用到的Dao
        private IDcLogDao DcLogDao { get; set; }
		private IDcLogRedoDao DcLogRedoDao { get; set; }
		private ISysjBakDao SysjBakDao { get; set; }
		private ISysjsdDao SysjsdDao { get; set; }
		private IXcsjDao XcsjDao { get; set; }
        private IBanHeZhanDataDao BanHeZhanDataDao { get; set; }
        private IBanHeZhanSectionDao BanHeZhanSectionDao { get; set; }
		#endregion
		#region 服务
		public SysjBak GetSysj(int recid)
		{
			return SysjBakDao.Get(recid);
		}
		public IList<DcLogRedo> GetDclogRedos(string uniqcode)
		{
			return DcLogRedoDao.Gets(uniqcode);
		}
		public DcLogRedo GetDclogRedo(int recid)
		{
			return DcLogRedoDao.Get(recid);
		}
		public IList<Sysjsd> GetSysjsds(string commsylb="")
		{
			return SysjsdDao.Gets(commsylb);
		}
		public IList<Xcsj> GetXcsjs()
		{
			return XcsjDao.Gets();
		}
		public Xcsj GetXcsj(string commsylb)
		{
			return XcsjDao.Get(commsylb);
		}
        public DcLog GetDclog(int recid)
        {
            return DcLogDao.Get(recid);
        }

        public BanHeZhanSection GetSection(int id)
        {
            return BanHeZhanSectionDao.Get(id);
        }
        public void InsertSection(BanHeZhanSection section)
        {
            BanHeZhanSectionDao.Save(section);
        }
        public void UpdateSection(BanHeZhanSection section)
        {
            BanHeZhanSectionDao.Update(section);
        }
        public IList<BanHeZhanSection> GetAllSection()
        {
           return BanHeZhanSectionDao.GetAll();
        }
        public void InsertBHZData(BanHeZhanData bhzdata)
        {
            BanHeZhanDataDao.Save(bhzdata);
        }
        public BanHeZhanData GetBHZData(int id)
        {
           return BanHeZhanDataDao.Get(id);
        }
		#endregion
	}
}
