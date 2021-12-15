using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.JC.JS.Common.Entities;
using BD.JC.JS.Common.Controllers;

namespace BD.JC.JS.Common
{
    /// <summary>
    /// 祝从表业务类
    /// </summary>
    public static class BillMstable
    {
        /// <summary>
        /// 把字符串类型的空或者NULL的字段设置成----
        /// </summary>
        /// <returns></returns>
        public static bool NullToOther(string byzbrecid, string destString, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 获取数据信息
                EntityMstable mstable = ControllerMstable.GetByxx(byzbrecid, out msg);
                if (mstable == null){
                    msg = "获取必有主表信息失败，详细信息："+msg;
                    return ret;
                }
                // 获取zdzd
                CollectionZdzd zdzds = ControllerZdzd.GetZdzds(mstable.Syxmbh, out msg);
                if (msg != "")
                {
                    msg = "获取zdzd失败，详细信息：" + msg;
                    return ret;
                }
                // 排除的字段
                IList<KeyValuePair<string, string>> execludes = new List<KeyValuePair<string, string>>();
                /*
                execludes.Add(new KeyValuePair<string, string>("M_BY", "SYDWBH"));
                execludes.Add(new KeyValuePair<string, string>("M_BY", "SYDWMC"));
                execludes.Add(new KeyValuePair<string, string>("S_BY", "ZT"));*/
                // 设置值
                ret = ControllerMstable.SetNullToLine(mstable, zdzds, destString, execludes, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
    }
}
