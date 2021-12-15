using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
	public interface IYcbaService
	{
        IList<SysYcdyStation> GetStations();
        SysYcdyUrl GetUrl(string callid, string version);

        IList<SysYcdyParam> GetParams(string callid, string version);

        IList<SysYcdyTable> GetTables(string callid, string version);

        IList<SysYcdyTableRelation> GetTableRelations(string callid, string version);

        IList<SysYcdyField> GetFields(string callid, string version);

        IList<SysYcdyPrimaryKey> GetPrimaryKeys(string callid, string version);

        bool SaveData(IList<IDictionary<string, object>> sqls, out string msg);
        IList<SysYcdyUrl> GetAllUrl();
        IList<SysYcdyTable> GetAllTable();
        IList<SysYcdyTableRelation> GetAllRelations();
        IList<SysYcdyParam> GetAllParams();
        IList<SysYcdyField> GetAllFields();
    }
}
