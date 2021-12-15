using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ISysSessionDao : IBaseDao<SysSession, string>
    {
        void DeleteExpire(int expirehours);
        SysSession GetByUser(string username);
    }
}
