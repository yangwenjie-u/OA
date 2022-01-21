using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.Entities
{
    public class MateriaInfo
    {
        ///<summary>
        ///
        ///</summary>
        public int ID { get; set; }

        ///<summary>
        ///材料唯一号
        ///</summary>
        public string Recid { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string MaterialID { get; set; }

        ///<summary>
        ///材料名称
        ///</summary>
        public string MaterialName { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string MaterialSpecID { get; set; }

        ///<summary>
        ///规格型号
        ///</summary>
        public string MaterialSpecName { get; set; }

        ///<summary>
        ///单价
        ///</summary>
        public decimal? Price { get; set; }

        ///<summary>
        ///采购价格
        ///</summary>
        public decimal? PurchasePrice { get; set; }

        ///<summary>
        ///数量
        ///</summary>
        public decimal? Quantity { get; set; }

        ///<summary>
        ///用途
        ///</summary>
        public string Purpose { get; set; }

        ///<summary>
        ///技术要求
        ///</summary>
        public string TechnicalRequirement { get; set; }

        ///<summary>
        ///供应商
        ///</summary>
        public string Supplier { get; set; }

        ///<summary>
        ///生产厂家
        ///</summary>
        public string Manufacturer { get; set; }

        ///<summary>
        ///申购人
        ///</summary>
        public string Requisitioner { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string JCJGBH { get; set; }

        ///<summary>
        ///
        ///</summary>
        public DateTime? CreateTime { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string Creator { get; set; }

        ///<summary>
        ///
        ///</summary>
        public DateTime? UpdateTime { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string Updater { get; set; }

        ///<summary>
        ///-1 弃用  
        ///</summary>
        public string Status { get; set; }

        ///<summary>
        ///最新进价
        ///</summary>
        public decimal? LastPrice { get; set; }

        ///<summary>
        ///单位
        ///</summary>
        public string MaterialUnit { get; set; }

        ///<summary>
        ///(编号)
        ///</summary>
        public string MaterialBH { get; set; }

    }
}
