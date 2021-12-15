using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.threads
{
    public interface ISchedulerJob
    {
        void Execute();
        void SetInterval(int seconds);
    }
}