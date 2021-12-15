using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using BD.Jcbg.Common;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BD.Jcbg.Web.Func
{
    public  class WebSocket
    {
        private static WebSocketServer wssv = null;
        /// <summary>
        /// 初始化websocket服务
        /// </summary>
        public static  void InitSocket()
        {
            if (wssv == null )
            {
                int port = GetPort();
                wssv = new WebSocketServer(port);
                wssv.ReuseAddress = true;
                wssv.AddWebSocketService<Websockets.UserMail>("/UserMail");
                wssv.AddWebSocketService<Websockets.WelcomeInfo>("/WelcomeInfo");
                wssv.Start();
            }
            


        }

        /// <summary>
        /// 停止websocket服务
        /// </summary>
        public static void StopSocket()
        {
            if (wssv != null)
            {
                wssv.Stop();
            }
        }

        /// <summary>
        ///  重启websocket服务
        /// </summary>
        public static void RebootSocket()
        {
            StopSocket();
            InitSocket();
        }

        /// <summary>
        /// 获取配置文件中的websocket服务端口号
        /// </summary>
        /// <returns></returns>
        public static int GetPort()
        {
            int port = 0;
            try
            {
                port = Configs.GetConfigItem("port", "WebSocketConfig.xml").GetSafeInt();
            }
            catch (Exception e)
            {
                throw e;
            }
            return port;
        }

        
    }
}