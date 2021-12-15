using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
    /// <summary>
    /// 工程、企业、人员、设备 基础数据接口
    /// </summary>
    public interface IBasicDataService
    {
        bool GetAllData(BasicDataType type, BasicDataBase where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);

        bool GetDataList(BasicDataType type, BasicDataBase where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);
        bool GetGcs(VBasicDataGetGc where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);
        bool GetGcs(VBasicDataGetGc where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);

        bool GetQys(VBasicDataGetQy where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);
        bool GetQys(VBasicDataGetQy where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);

        bool GetRys(VBasicDataGetRy where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);
        bool GetRys(VBasicDataGetRy where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);

        bool GetSbs(VBasicDataGetSb where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);
        bool GetSbs(VBasicDataGetSb where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg);


    }
}
