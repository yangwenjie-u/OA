using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Service.Jc
{
    /// <summary>
    /// 2019-07-03
    /// 杨鑫钢
    /// 用于浙江标点服务接口
    /// </summary>
    public class BDService : IBaseService
    {
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public ResultParam GetQrCode(IDictionary<string, string> dic)
        {
            ResultParam ret = new ResultParam();
            ret.data = Guid.NewGuid().ToString();
            ret.success = true;
            return ret;
        }


        public ResultParam UploadFeiLiangKuai(IDictionary<string, string> dic)
        {
            throw new NotImplementedException();
        }

        public ResultParam UploadLiangKuai(IDictionary<string, string> dic)
        {
            throw new NotImplementedException();
        }
    }
}
