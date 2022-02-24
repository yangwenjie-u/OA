using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    /// <summary>
    /// 人员技术档案详情
    /// </summary>
    public class ArchivesDetails
    {
        public string UserRecid { get; set; }

        public List<ArchivesData> ArchivesData = new List<ArchivesData>();

    }

    /// <summary>
    /// 档案信息
    /// </summary>
    public class ArchivesData
    {
        /// <summary>
        /// 档案唯一号
        /// </summary>
        public int Recid { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int ArchivesIndex { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ArchivesType { get; set; }

        /// <summary>
        /// 档案名称
        /// </summary>
        public string ArchivesName { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public  List<AnnexData> AnnexData = new List<AnnexData>();

        public string Remark { get; set; }

    }

    public class AnnexData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int FileName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string OssUrl { get; set; }

    }

}
