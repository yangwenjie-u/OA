using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ISysycdytableDao:IBaseDao<SysYcdyTable, int>
    {
        IList<SysYcdyTable> Gets(string callid);
    }
}
