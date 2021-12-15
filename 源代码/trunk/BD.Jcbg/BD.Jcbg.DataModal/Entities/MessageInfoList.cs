using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.Entities
{
    public class MessageInfoList
    {

        /// <summary>
        ///  主键recid
        /// </summary>
        /// Author  :Napoleon
        /// Created :2017-08-10 10:36:22
        public virtual int Recid { get; set; }
         

        /// <summary>
        ///  账号code
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string UserCode { get; set; }
        

        /// <summary>
        ///  用户真实名
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string UserName { get; set; }


        /// <summary>
        ///  手机号码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Phone { get; set; }


        /// <summary>
        ///  微信唯一openid
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string WxOpenId { get; set; }


        /// <summary>
        ///  是否通知
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string IsNotice { get; set; }


        /// <summary>
        ///  允许地区列表
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string AreaCodeList { get; set; }


        /// <summary>
        ///  允许警种列表
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PoliceTypeList { get; set; }
         

    }
}
