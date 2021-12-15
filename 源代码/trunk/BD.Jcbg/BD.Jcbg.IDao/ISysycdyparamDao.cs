using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ISysycdyparamDao:IBaseDao<SysYcdyParam, int>
    {
        IList<SysYcdyParam> Gets(string callid);
    }
}
