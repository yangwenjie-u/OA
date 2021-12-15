using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BD.Jcbg.IBll
{
    public interface IFormAPIService
    {
        bool GetWebListData(HttpRequestBase Request, object controller, out int total, out List<Dictionary<string, object>> data, out string msg);
    }
}
