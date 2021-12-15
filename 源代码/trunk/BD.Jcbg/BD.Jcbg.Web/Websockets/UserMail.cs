using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Websockets
{
    public class UserMail : WebSocketBehavior
    {
        private Timer timer = null;

        protected override void OnOpen()
        {
            timer = new Timer((obj) => { SendMsg(); }, null, 1000, 3000);
            
            
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data + "-- from server !");
        }

        protected override void OnError(ErrorEventArgs e)
        {

            Send("error -- from server !");

        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
        }

        private void SendMsg()
        {
            string msg = "";
            try
            {
                msg = CurrentUser.IsLogin ? "true" : "false";
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            Send(string.Format("{0}--{1}--{2}", "usermail", msg, "from server!"));
        }
    }
}