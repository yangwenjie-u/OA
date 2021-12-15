using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BD.Jcbg.IBll
{
    public interface IJcjgBgService
    {
        int GetBgsl(string zjdjh, string jcjg, string ptbh, string lszjdjh = "");

        bool GetBHGBG(string ptbh, Dictionary<string, string> param, out List<Dictionary<string, object>> data, out int total, out string msg);

        bool GetBGList(string ptbh, Dictionary<string, string> param, out List<Dictionary<string, object>> data, out string msg);
    }
}
