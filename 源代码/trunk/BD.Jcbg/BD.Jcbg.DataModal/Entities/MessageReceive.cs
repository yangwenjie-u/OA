using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.Entities
{
    public class MessageReceive
    {

        /// <summary>
        ///  主键recid
        /// </summary>
        /// Author  :Napoleon
        /// Created :2017-08-10 10:36:22
        public virtual int Recid { get; set; }
         

        /// <summary>
        ///  申报信息的唯一编码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string ProjId { get; set; }
        

        /// <summary>
        ///  办结人所属部门名称,申报地点（部门）
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string HanderDeptName { get; set; }


        /// <summary>
        ///  办结人所属部门编码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string HanderDeptId { get; set; }


        /// <summary>
        ///  办结人所属部门的所在行政区划编码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string AreaCode { get; set; }


        /// <summary>
        ///  办结人所属部门的所在行政区划名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string AreaName { get; set; }


        /// <summary>
        ///  受理人员
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string AcceptMan { get; set; }


        /// <summary>
        ///  受理时间: 时间格式 yyyy-mm-dd hh24:mi:ss
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string AcceptTime { get; set; }


        /// <summary>
        ///  承诺期限
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PromiseValue { get; set; }


        /// <summary>
        ///  承诺期限单位: 工作日、工作小时
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PromiseType { get; set; }


        /// <summary>
        ///  承诺办结时间: 日期格式yyyy-mm-dd hh24:mi:ss
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PromiseEtime { get; set; }


        /// <summary>
        ///  办理人员姓名
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string TransactUser { get; set; }


        /// <summary>
        ///  办结日期
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string TransactTime { get; set; }


        /// <summary>
        ///  办理结果 : 准予许可、不予许可、转报',    办理结果
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  TransactResult{ get; set; }


        /// <summary>
        ///  审批事项编号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ServiceCode{ get; set; }


        /// <summary>
        ///  权力事项名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ServiceName{ get; set; }


        /// <summary>
        ///  办件类型: 即办件、承诺件
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  InfoType{ get; set; }


        /// <summary>
        ///  申报者名称：如为个人，则填写姓名；如为法人，则填写单位名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ApplyName{ get; set; }


        /// <summary>
        ///  申报者证件类型: 身份证、组织机构代码证等
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ApplyCardType{ get; set; }


        /// <summary>
        ///  申报者证件号码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ApplyCardNumber{ get; set; }


        /// <summary>
        ///  联系人/代理人姓名
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ContactMan{ get; set; }


        /// <summary>
        ///  联系人/代理人证件类型
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ContactManCardType{ get; set; }


        /// <summary>
        ///  联系人/代理人证件号码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  ContactManCardNumber{ get; set; }


        /// <summary>
        ///  联系人手机号码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  TelPhone{ get; set; }


        /// <summary>
        ///  邮编
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  PostCode{ get; set; }


        /// <summary>
        ///  通讯地址
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  Address{ get; set; }


        /// <summary>
        ///  法人代表
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string  LegalMan{ get; set; }


        /// <summary>
        ///  数据状态: 0=作废1=有效
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string DataState { get; set; }


        /// <summary>
        ///  所属系统
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string BelongSystem { get; set; }


        /// <summary>
        ///  警种代码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PoliceTypeCode { get; set; }


        /// <summary>
        ///  警种名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string PoliceType { get; set; }


        /// <summary>
        ///  备用字段
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Extend { get; set; }


        /// <summary>
        ///  流水号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Lsh { get; set; }


        /// <summary>
        ///  创建日期
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string CreateDate { get; set; }


        /// <summary>
        ///  是否满意
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string IsSatisfied { get; set; }


        /// <summary>
        ///  满意代码
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string SatisfiedCode { get; set; }


        /// <summary>
        ///  发送短信内容
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Context { get; set; }


        /// <summary>
        ///  回复短信内容
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string ReplyContext { get; set; }


        /// <summary>
        ///  验证code
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Code { get; set; }


        /// <summary>
        ///  uuid
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string Uuid { get; set; }


        /// <summary>
        ///  更新时间
        /// </summary>
        /// Author  : Napoleon
        /// Created : 22017-08-10 15:59:58
        public virtual string UpdateTime { get; set; }

         
        
    }
}
