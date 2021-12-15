using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace BD.Jcbg.Web.threads
{
    public class Scheduler
    {
        private SchedulerConfiguration configuration = null;
        Thread[] m_LastThreads = null;

        public Scheduler(SchedulerConfiguration config)
        {
            configuration = config;
        }

        public void Start()
        {
            int[] arrJobTimes = new int[configuration.Jobs.Count];
            for (int i = 0; i < arrJobTimes.Length; i++)
                arrJobTimes[i] = -1;
            int nSleepS = 10;
            if (m_LastThreads == null)
            {
                m_LastThreads = new Thread[configuration.Jobs.Count];
                for (int i = 0; i < m_LastThreads.Length; i++)
                    m_LastThreads[i] = null;
            }

            while (true)
            {
                //执行每一个任务
                for (int i = 0; i < configuration.Jobs.Count; i++)
                {
                    int nInterval = configuration.GetInterval(i);
                    arrJobTimes[i]++;
                    if (arrJobTimes[i] == 0 || arrJobTimes[i] * nSleepS >= nInterval)
                    {
                        ISchedulerJob job = configuration.Jobs[i] as ISchedulerJob;
                        if (job != null)
                        {
                            if (m_LastThreads[i] == null || !m_LastThreads[i].IsAlive)
                            {
                                ThreadStart myThreadDelegate = new ThreadStart(job.Execute);
                                Thread myThread = new Thread(myThreadDelegate);
                                myThread.IsBackground = true;
                                myThread.Start();
                                m_LastThreads[i] = myThread;
                            }
                        }

                        arrJobTimes[i] = 0;
                    }
                    else
                    {
                        arrJobTimes[i]++;
                    }

                }
                Thread.Sleep(nSleepS * 1000);
            }
        }
    }
}