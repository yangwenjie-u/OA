using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 委托书异常状态
    /// </summary>
    public class WtsYczt
    {
        public const int Normal = 0;        // 无异常状态
        public const int WtsModify = 1;     // 委托单有修改
        public const int WtsFieldsMiss = 2; // 委托书字段未全部上传
        public const int DataModify = 4;    // 自动采集数据有修改
        public const int DataUnsave = 8;    // 自动采集有未保存数据
        public const int DataRedo = 16;     // 自动采集有重做数据
        public const int DataRepeat = 32;   // 自动采集有重复试验
        public const int ReportRepeat = 64; // 有重复报告
        public const int PersonNotFind = 128;// 试验员未登记
        public const int PersonLogNotFind = 256;//试验员未到场
        public const int DataUpExceed = 512;    // 数据上传时间超差
        public const int ReportUpExceed = 1024;// 报告上传超差
        public const int NoJzPic = 2048;       //无见证照片
        public const int ReportJyrq = 4096;     //报告接样日期异常
        //public const int D

        private int mStatus = 0;
        public int Status { get { return mStatus; } }

        public WtsYczt()
        {
            mStatus = 0;
        }

        public int AddStatus(int status)
        {
            mStatus |= status;
            return mStatus;
        }

    }
}
