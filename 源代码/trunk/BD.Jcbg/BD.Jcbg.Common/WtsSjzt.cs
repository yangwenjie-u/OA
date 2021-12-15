using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 委托书数据状态
    /// </summary>
    public class WtsSjzt
    {
        public const int NoData = 0;        // 无数据
        public const int HasData = 1;       // 有采集数据
        public const int HasCurve = 2;      // 有曲线
        public const int HasVideo = 4;      // 有视频
        public const int XcHasData = 8;     // 现场数据
        public const int XcHasVideo = 16;   // 现场视频
        public const int XcHasImage = 32;   // 现场图片
        //public const int XcHasCamera = 64;  // 现场摄像头
        public const int XcjkVideo = 128;    //现场监控
        public const int XcjkTpl = 256;      //图片链


        private int mStatus = 0;
        public int Status { get { return mStatus; } }

        public WtsSjzt()
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
