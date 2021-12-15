using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ISysycdyfieldDao : IBaseDao<SysYcdyField, int>
    {
        IList<SysYcdyField> Gets(string callid);
    }
}
