using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.Service.Jc;

namespace BD.Jcbg.Service.Jc
{
    public class ServiceManager
    {
        //基础服务类
        public static IBaseService _baseService;

        /// <summary>
        /// 根据配置获取基础服务
        /// </summary>
        /// <returns></returns>
        public static IBaseService GetBaseService()
        {
            IBaseService baseService;
            //判断服务类是否存在
            if (_baseService == null)
            {
                //判断服务接口类型
                //萧册协会接口
                if (GlobalVariableConfig.GLOBAL_SERVICE_GENERATE == ServiceEnum.XSXH.ToString())
                    baseService = new XsXhService();       
                //浙江标点接口
                else if (GlobalVariableConfig.GLOBAL_SERVICE_GENERATE == ServiceEnum.ZJBD.ToString())
                    baseService = new BDService();
                //浙江标点接口
                else
                    baseService = new BDService();
            }
            else
            {
                baseService = _baseService;
            }
            return baseService;
        }
    }
}
