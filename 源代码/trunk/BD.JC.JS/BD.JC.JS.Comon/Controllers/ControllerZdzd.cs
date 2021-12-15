using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.JC.JS.Common.Entities;
using BD.JC.JS.Common;

namespace BD.JC.JS.Common.Controllers
{
    /// <summary>
    /// zdzd操作类
    /// </summary>
    public static class ControllerZdzd
    {
        static IDictionary<string, CollectionZdzd> mZdzds = new Dictionary<string, CollectionZdzd>();
        /// <summary>
        /// 根据byzbrecid获取zdzd
        /// </summary>
        /// <param name="byzbrecid"></param>
        /// <returns></returns>
        public static CollectionZdzd GetZdzds(string syxmbh, out string msg)
        {
            msg = "";
            CollectionZdzd ret = new CollectionZdzd();
            syxmbh = syxmbh.ToLower();
            try
            {
                if (!mZdzds.TryGetValue(syxmbh, out ret))
                {
                    ret = new CollectionZdzd();
                    // 必有表
                    string sql = "select * from xtzd_by order by sjbmc, xssx";
                    List<IDictionary<string,string>> dt = (List<IDictionary<string,string>>)SqlHelper.GetDataTable(sql, out msg);
                    // 项目表
                    sql = "select * from zdzd_"+syxmbh+" where sjbmc like 'M_%' or sjbmc like 'S_%' order by sjbmc, xssx";
                    List<IDictionary<string, string>> dt2 = (List<IDictionary<string,string>>)SqlHelper.GetDataTable(sql, out msg);
                    dt.AddRange(dt2);
                    // 单位表
                    sql = "select * from dwzd_" + syxmbh + " where sjbmc like 'M_D_%' or sjbmc like 'S_D_%' order by sjbmc, xssx";
                    dt2 = (List<IDictionary<string, string>>)SqlHelper.GetDataTable(sql, out msg);
                    dt.AddRange(dt2);

                    foreach (IDictionary<string, string> row in dt)
                    {
                        EntityZdzd zdzd = new EntityZdzd();
                        if (zdzd.Load(row))
                            ret.Add(zdzd);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }

    }
}
