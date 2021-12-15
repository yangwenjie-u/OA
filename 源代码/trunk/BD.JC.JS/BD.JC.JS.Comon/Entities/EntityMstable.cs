using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common.Entities
{
    /// <summary>
    /// 祝从表实体类
    /// </summary>
    public class EntityMstable
    {
        // 必有主表
        public IDictionary<string, string> MBydata = new Dictionary<string, string>();
        // 单位主表
        public IDictionary<string, string> MDwdata = new Dictionary<string, string>();
        // 项目主表
        public IDictionary<string, string> MXmdata = new Dictionary<string, string>();
        // 必有从表
        public IList<IDictionary<string, string>> SBydata = new List<IDictionary<string, string>>();
        // 单位从表
        public IList<IDictionary<string, string>> SDwdata = new List<IDictionary<string, string>>();
        // 项目从表
        public IList<IDictionary<string, string>> SXmdata = new List<IDictionary<string, string>>();

        // 委托单唯一号
        public string Wtdwyh
        {
            get
            {
                string ret = "";
                if (MBydata != null)
                {
                    ret = MBydata["recid"].GetSafeString();
                }
                return ret;
            }
        }
        // 试验项目编号
        public string Syxmbh 
        {
            get
            {
                string ret = "";
                if (MBydata != null)
                {
                    ret = MBydata["syxmbh"].GetSafeString();
                }
                return ret;
            }
        }
        /// <summary>
        /// 单位主表
        /// </summary>
        public string MDW
        {
            get
            {
                string ret = "";
                if (Syxmbh != "")
                {
                    ret = "m_d_" + Syxmbh;
                }
                return ret;
            }
        }
        /// <summary>
        /// 单位从表
        /// </summary>
        public string SDW
        {
            get
            {
                string ret = "";
                if (Syxmbh != "")
                {
                    ret = "s_d_" + Syxmbh;
                }
                return ret;
            }
        }
        /// <summary>
        /// 项目主表
        /// </summary>
        public string MXM
        {
            get
            {
                string ret = "";
                if (Syxmbh != "")
                {
                    ret = "m_" + Syxmbh;
                }
                return ret;
            }
        }
        /// <summary>
        /// 项目从表
        /// </summary>
        public string SXM
        {
            get
            {
                string ret = "";
                if (Syxmbh != "")
                {
                    ret = "s_" + Syxmbh;
                }
                return ret;
            }
        }
        /// <summary>
        /// 必有主表
        /// </summary>
        public string MBY
        {
            get
            {
                return "m_by";
            }
        }
        /// <summary>
        /// 必有从表
        /// </summary>
        public string SBY
        {
            get
            {
                return "s_by";
            }
        }
        // 状态
        public string Zt {
            get
            {
                string ret = "";
                if (MBydata != null)
                {
                    ret = MBydata["zt"].GetSafeString();
                }
                return ret;
            }
        }

        public bool IsValid { get; set; }

        public bool Load(IDictionary<string, string> mdwdata, IDictionary<string, string> mxmdata,
            IList<IDictionary<string, string>> sdwdata, IList<IDictionary<string, string>> sxmdata)
        {
            IsValid = false;
            try
            {
                MDwdata = mdwdata;
                MXmdata = mxmdata;
                SDwdata = sdwdata;
                SXmdata = sxmdata;

                IsValid = true;
            }
            catch (Exception ex)
            {

            }
            return IsValid;
        }

        public bool Load(IDictionary<string, string> mbydata, IList<IDictionary<string, string>> sbydata)
        {
            IsValid = false;
            try
            {
                MBydata = mbydata;
                SBydata = sbydata;

                IsValid = true;
            }
            catch (Exception ex)
            {

            }
            return IsValid;
        }
    }
}
