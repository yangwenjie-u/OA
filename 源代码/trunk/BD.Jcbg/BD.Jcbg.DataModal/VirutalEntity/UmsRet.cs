using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class UmsRet
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool compress { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public object data { get; set; }

    }
}
