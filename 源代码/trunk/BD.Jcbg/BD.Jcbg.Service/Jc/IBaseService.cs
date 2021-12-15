using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Service.Jc
{
    public interface IBaseService
    {
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        ResultParam GetQrCode(IDictionary<string, string> dic);

        #region 个性化
        /// <summary>
        /// 上传非两块两材数据
        /// </summary>
        /// <param name="jgid">机构ID</param>
        /// <param name="jcqy">机构区域</param>
        /// <param name="xml">XML数据包</param>
        /// <returns></returns>
        ResultParam UploadFeiLiangKuai(IDictionary<string, string> dic);

        /// <summary>
        /// 上传两块两材
        /// </summary>
        /// <param name="jgid">机构ID</param>
        /// <param name="jgqy">机构区域</param>
        /// <param name="code">编号</param>
        /// <param name="xml">数据XML</param>
        /// <param name="jpjcXml">样品XML</param>
        /// <returns></returns>
        ResultParam UploadLiangKuai(IDictionary<string, string> dic);
        #endregion
    }
}
