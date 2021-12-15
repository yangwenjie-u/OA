using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IDwgxwzService
    {
        #region 获取整改单回复详情
        IList<IDictionary<string, object>> GetZgdHfxq(string zgdbh);
        #endregion
    }
}
