using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.threads
{
    public class SchedulerConfiguration
    {
        //时间间隔
        public List<int> Intervals = new List<int>();
        //任务列表
        public List<ISchedulerJob> Jobs = new List<ISchedulerJob>();

        public int GetInterval(int nIndex)
        {
            int nInterval = 10000; //ms
            try
            {
                nInterval = Convert.ToInt32(Intervals[nIndex]);
            }
            catch
            {
            }
            return nInterval;
        }

        //调度配置类的构造函数
        public SchedulerConfiguration()
        {

        }
    }
}