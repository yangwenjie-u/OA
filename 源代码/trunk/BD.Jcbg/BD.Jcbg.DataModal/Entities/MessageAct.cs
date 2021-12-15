using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.Entities
{
    public class MessageAct
    {


        /// <summary>
        ///  流水号(主键)
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Lsh { get; set; }


        /// <summary>
        ///  手机号码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PhoneNumber { get; set; }


        /// <summary>
        ///  发送内容
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Context { get; set; }


        /// <summary>
        ///  状态
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Status { get; set; }


        /// <summary>
        ///  回复内容
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string ReplyContext { get; set; }


        /// <summary>
        ///  uuid
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Uuid { get; set; }


        /// <summary>
        ///  创建日期
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string CreateDate { get; set; }


        /// <summary>
        ///  办理事务code
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string ServiceCode { get; set; }

         

    }
}
