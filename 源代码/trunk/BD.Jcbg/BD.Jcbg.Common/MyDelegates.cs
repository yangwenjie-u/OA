using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class MyDelegates
    {
        public delegate bool FuncGetUserSign(string username, out string sign);
    }
}
