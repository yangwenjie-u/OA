using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class MessageActService : IMessageActService
    {
        #region 用到的Dao
        IMessageActDao MessageActDao { get; set; }
        #endregion
    }
}
