using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class ResultParam
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        private string _code = String.Empty;

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        private bool _success = false;

        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        /// <summary>
        /// 消息
        /// </summary>
        private string _msg = "";

        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        /// <summary>
        /// 数据包
        /// </summary>
        private object _data = null;

        public object data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
