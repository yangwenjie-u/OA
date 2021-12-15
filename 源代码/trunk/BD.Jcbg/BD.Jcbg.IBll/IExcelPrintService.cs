using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.IBll
{
    public interface IExcelPrintService
    {
        byte[] FormatWts(string filepath, IDictionary<string, string> wheres, MyDelegates.FuncGetUserSign funcGetUserSign);
    }
}
