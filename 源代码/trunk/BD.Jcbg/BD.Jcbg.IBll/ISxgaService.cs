using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
   public interface ISxgaService
    {


       /// <summary>
        ///  用存储过程获取图表数据(Y按月Z按周)
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="BS"></param>
       /// <returns></returns>
       IList<IDictionary<string, string>> getTbDataByPro(DateTime dt, string BS);

       /// <summary>
       /// 获取图表数据(Y按月Z按周)
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="BS"></param>
       /// <returns></returns>
       IList<IDictionary<string, string>> getTbData(DateTime dt, string BS);

       /// <summary>
       /// 查询不满意短信
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       IList<IDictionary<string, string>> getBmyData(string type, string dtstr);

       /// <summary>
       /// 生成周统计、月统计数据
       /// </summary>
       /// <param name="dt"></param>
       /// <returns></returns>
       bool CreateData(DateTime dt, string BS);
    }
}
