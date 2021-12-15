using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface ISxtptService
    {
        /// <summary>
        /// 根据我们软件的摄像头编号往平台注册摄像头
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Register(string sxtid, out string msg);
        /// <summary>
        /// 根据我们软件的摄像头编号，查询摄像头是否在线
        /// </summary>
        /// <param name="deviceserial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool QueryOnline(string sxtid, out string sxtwyh, out string sxtmc, out string msg);
        /// <summary>
        /// 获取摄像头播放地址
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool GetPlayUrl(string sxtid, out string msg);
        /// <summary>
        /// 查询摄像头是否在商家平台注册
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool IsRegister(string sxtid, out string msg);
        /// <summary>
        /// 根据我们软件的摄像头编号移除平台摄像头
        /// </summary>
        /// <param name="deviceserial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Remove(string sxtid, out string msg);
        /// <summary>
        /// 抓拍图片保存到数据库，返回图片id
        /// </summary>
        /// <param name="sxtid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CaptuerImage(string sxtid, string usercode, string realname, out string msg, string tranType = "", string wtdbh = "", string zh = "");
        /// <summary>
        /// 获取某个时间开始的图片
        /// </summary>
        /// <param name="fromTime"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetCaptureImages(string sxtid, string fromTime, out string msg);
        /// <summary>
        /// 获取抓拍的图片内容
        /// </summary>
        /// <param name="tpid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        byte[] GetCaptureImageContent(string tpid, out string msg);

        /// <summary>
        /// 开启摄像头抓拍线程
        /// </summary>
        /// <param name="useSxts"></param>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        void ThreadStartSxt(object useSxts, string username, string realname, string wtdwyh, string zh, bool ck = false);

        /// <summary>
        /// 关闭摄像头抓拍线程
        /// </summary>
        void DropSxtThread(string wtdwyh, string zh);

        //系统启动加载摄像头
        void InitThread();
    }
}
