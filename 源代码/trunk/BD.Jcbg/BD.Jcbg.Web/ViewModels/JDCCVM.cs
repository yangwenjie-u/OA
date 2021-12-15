using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.ViewModels
{
    public class JDCCVM
    {
        /// <summary>
        /// 是监督抽测  否则违法违规
        /// </summary>
        public bool IsJDCC { get; set; }

        /// <summary>
        /// 抽测内容
        /// </summary>
        public string CCNR { get; set; }

        /// <summary>
        /// 抽测内容原Id
        /// </summary>
        public string CCNRID { get; set; }

        /// <summary>
        /// 抽测总组数
        /// </summary>
        public string ZZS { get; set; }

        /// <summary>
        /// 不合格组数
        /// </summary>
        public string BHGZS { get; set; }

        /// <summary>
        /// 合格组数
        /// </summary>
        public string HGZS { get; set; }

        /// <summary>
        /// 违法违规内容
        /// </summary>
        public string WFWGContent { get; set; }
    }
}