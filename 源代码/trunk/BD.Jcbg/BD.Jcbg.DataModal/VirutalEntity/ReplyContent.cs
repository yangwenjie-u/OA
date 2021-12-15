using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class ReplyContent
    {
        /// <summary>
        ///  接入号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Callmdn { get; set; }


        /// <summary>
        ///  回复 手机号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Mdn { get; set; }


        /// <summary>
        ///  回复内容
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Content { get; set; }


        /// <summary>
        /// 回复时间
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Reply_time { get; set; }


        /// <summary>
        ///  回复Id编号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Id { get; set; }
    }
}
