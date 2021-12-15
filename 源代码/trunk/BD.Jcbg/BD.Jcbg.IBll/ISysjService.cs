using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;
using System.Web;

namespace BD.Jcbg.IBll
{
	public interface ISysjService
	{
		
		SysjBak GetSysj(int recid);
		IList<DcLogRedo> GetDclogRedos(string uniqcode);
		DcLogRedo GetDclogRedo(int recid);
		IList<Sysjsd> GetSysjsds(string commsylb = "");
		IList<Xcsj> GetXcsjs();
		Xcsj GetXcsj(string commsylb);
        DcLog GetDclog(int recid);
        BanHeZhanSection GetSection(int id);
        void InsertSection(BanHeZhanSection section);
        void UpdateSection(BanHeZhanSection section);
        IList<BanHeZhanSection> GetAllSection();
        void InsertBHZData(BanHeZhanData bhzdata);
        BanHeZhanData GetBHZData(int id);
	}
}
