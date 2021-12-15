using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;
namespace BD.Jcbg.IDao
{
    public interface ISysycdytablerelationDao:IBaseDao<SysYcdyTableRelation, int>
    {
        IList<SysYcdyTableRelation> Gets(string callid);
    }
}
