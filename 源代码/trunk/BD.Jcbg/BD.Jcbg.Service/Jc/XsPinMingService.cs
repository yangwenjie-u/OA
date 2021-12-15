using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Service.XsPinMingWebService;
using BD.Jcbg.DataModal.VirutalEntity.Jc;
using BD.Jcbg.Common;

namespace BD.Jcbg.Service.Jc
{
    public class XsPinMingService
    {
        #region 属性
        /// <summary>
        /// 程序是否运行
        /// </summary>
        public static bool isRun = false;

        /// <summary>
        /// 序列化类
        /// </summary>
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// 接口服务类
        /// </summary>
        private static SupplyProjectInfo service = new SupplyProjectInfo();
        #endregion

        #region 函数
        /// <summary>
        /// 下载品铭工程信息
        /// </summary>
        /// <returns></returns>
        public static ResultParam GetProjectInfo(string startRq, string endRq)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string data = service.GetProjectInfo(startRq, endRq);
                //解析数据包
                //string data = "{ \"total\": 10, \"msg\": \"success\", \"rows\": [ { \"ProjectName\": \"年组装10万辆童车项目（车间一）\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191002033\", \"Coordinate\": \"120.2712,30.18843\", \"BuildingArea\": \"1231.00\", \"Supervisor\": \"李炜，周壁隽，来维新\", \"BuildManager\": \"章国良\", \"AddUserName\": \"13567188218\", \"AddTrueName\": \"章国良\", \"AddDate\": \"2019-03-06\", \"Units\": [ { \"UnitCode\": \"52145789-9\", \"UnitName\": \"杭州小帅哥儿童用品有限公司\", \"LegalName\": \"无\", \"LegalPhone\": \"13654784514\", \"UnitType\": 5 }, { \"UnitCode\": \"91421381MA490FEH1G\", \"UnitName\": \"广水市正新建筑工程有限公司\", \"LegalName\": \"张小华\", \"LegalPhone\": \"13600000000\", \"UnitType\": 1 }, { \"UnitCode\": \"45625147-o\", \"UnitName\": \"湘湖设计单位\", \"LegalName\": \"湘湖设计单位\", \"LegalPhone\": \"13241254124\", \"UnitType\": 4 } ] }, { \"ProjectName\": \"年产60万台微型电机产品项目(1#车间扩建）\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191003117\", \"Coordinate\": \",\", \"BuildingArea\": \"4644.00\", \"Supervisor\": \"来春燕，高志法，瞿俊平 \", \"BuildManager\": \"徐志明\", \"AddUserName\": \"18757571968\", \"AddTrueName\": \"颜宝明\", \"AddDate\": \"2019-06-04\", \"Units\": [ { \"UnitCode\": null, \"UnitName\": \"杭州钱塔涂料玻璃有限公司\", \"LegalName\": null, \"LegalPhone\": null, \"UnitType\": 5 }, { \"UnitCode\": \"91330109738443406T\", \"UnitName\": \"杭州天仁建设环境有限公司\", \"LegalName\": \"项建儿\", \"LegalPhone\": \"13588878684\", \"UnitType\": 1 } ] }, { \"ProjectName\": \"年生产8万件金属工艺品项目\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191001118\", \"Coordinate\": \",\", \"BuildingArea\": \"9998.60\", \"Supervisor\": \"瞿红峰，周张浩，史佳\", \"BuildManager\": \"魏利娟\", \"AddUserName\": \"13567121716\", \"AddTrueName\": \"魏利娟\", \"AddDate\": \"2019-06-04\", \"Units\": [ { \"UnitCode\": \"913301093418616182\", \"UnitName\": \"杭州乐麦特工贸有限公司\", \"LegalName\": \"李刚\", \"LegalPhone\": \"13255693897\", \"UnitType\": 5 }, { \"UnitCode\": \"14345768-1\", \"UnitName\": \"杭州萧山城市建筑有限公司\", \"LegalName\": \"盛建康\", \"LegalPhone\": \"13706506191\", \"UnitType\": 1 } ] }, { \"ProjectName\": \"萧政储出[2018]17号地块居住用地项目——配建兴议路（建设一路—兴五路）1#，2#--&6#\", \"ProjectCategory\": \"2\", \"RegNumber\": \"20191004119\", \"Coordinate\": \"120.27,30.18619\", \"BuildingArea\": \"308.00\", \"Supervisor\": \"邱国华，莫中华，孔柳红，高军，王哲\", \"BuildManager\": \"王小忠\", \"AddUserName\": \"13958010666\", \"AddTrueName\": \"王小忠\", \"AddDate\": \"2019-06-05\", \"Units\": [ { \"UnitCode\": \"74512457-h\", \"UnitName\": \"杭州南兴房地产开发有限公司\", \"LegalName\": \"想小华\", \"LegalPhone\": \"13521234124\", \"UnitType\": 5 }, { \"UnitCode\": \"91330109747164978F\", \"UnitName\": \"浙江嘉润建设有限公司\", \"LegalName\": \"丁建刚\", \"LegalPhone\": \"13906717333\", \"UnitType\": 1 }, { \"UnitCode\": \"91330104662322199G\", \"UnitName\": \"浙江元正工程管理有限公司\", \"LegalName\": \"张建华\", \"LegalPhone\": \"0571-86974302\", \"UnitType\": 2 }, { \"UnitCode\": \"45625147-o\", \"UnitName\": \"湘湖设计单位\", \"LegalName\": \"湘湖设计单位\", \"LegalPhone\": \"13241254124\", \"UnitType\": 4 } ] }, { \"ProjectName\": \"年产3000万只汽车轮毂轴承单元精密锻车件智能化工厂建设项目1#，2#,333#&gg【】\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191002120\", \"Coordinate\": \"120.2703,30.18819\", \"BuildingArea\": \"78603.00\", \"Supervisor\": \"李炜，周壁隽，来维新\", \"BuildManager\": \"付海兵\", \"AddUserName\": \"18857193033\", \"AddTrueName\": \"付海兵\", \"AddDate\": \"2019-06-06\", \"Units\": [ { \"UnitCode\": \"12345678-g\", \"UnitName\": \"浙江兆丰机电股份有限公司\", \"LegalName\": \"孔爱祥\", \"LegalPhone\": \"13524124523\", \"UnitType\": 5 }, { \"UnitCode\": \"68583780-6\", \"UnitName\": \"杭州恒联建设有限公司\", \"LegalName\": \"翁建甫\", \"LegalPhone\": \"13605708785\", \"UnitType\": 1 }, { \"UnitCode\": \"45625147-o\", \"UnitName\": \"湘湖设计单位\", \"LegalName\": \"湘湖设计单位\", \"LegalPhone\": \"13241254124\", \"UnitType\": 4 } ] }, { \"ProjectName\": \"空港新城城市示范村（南阳区块）三期\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191002121\", \"Coordinate\": \",\", \"BuildingArea\": \"177277.88\", \"Supervisor\": \"李炜，周壁隽，来维新\", \"BuildManager\": \"许建芳\", \"AddUserName\": \"13018981270\", \"AddTrueName\": \"许建芳\", \"AddDate\": \"2019-06-12\", \"Units\": [ { \"UnitCode\": \"\", \"UnitName\": \"杭州空港投资开发有限公司\", \"LegalName\": \"姚进\", \"LegalPhone\": \"13906710828\", \"UnitType\": 5 }, { \"UnitCode\": \"14347807-9\", \"UnitName\": \"浙江宝盛建设集团有限公司\", \"LegalName\": \"诸黎明\", \"LegalPhone\": \"82811552\", \"UnitType\": 1 }, { \"UnitCode\": \"78238052-9\", \"UnitName\": \"浙江鑫润工程管理有限公司\", \"LegalName\": \"郑永达\", \"LegalPhone\": \"13575733835\", \"UnitType\": 2 } ] }, { \"ProjectName\": \"年产30万套五金机械及配件项目\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191003122\", \"Coordinate\": \",\", \"BuildingArea\": \"5299.00\", \"Supervisor\": \"来春燕，高志法，瞿俊平 \", \"BuildManager\": \"丁小明\", \"AddUserName\": \"13806501780\", \"AddTrueName\": \"丁小明\", \"AddDate\": \"2019-06-13\", \"Units\": [ { \"UnitCode\": null, \"UnitName\": \"杭州萧山伟达灯业有限公司\", \"LegalName\": null, \"LegalPhone\": null, \"UnitType\": 5 }, { \"UnitCode\": \"91330109770836058E\", \"UnitName\": \"浙江伟丰建设有限公司\", \"LegalName\": \"华庆生\", \"LegalPhone\": \"82216999\", \"UnitType\": 1 } ] }, { \"ProjectName\": \"年产500吨起重链条项目\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191001123\", \"Coordinate\": \",\", \"BuildingArea\": \"9838.00\", \"Supervisor\": \"瞿红峰，周张浩，史佳\", \"BuildManager\": \"俞介平\", \"AddUserName\": \"13357135608\", \"AddTrueName\": \"俞介平\", \"AddDate\": \"2019-06-13\", \"Units\": [ { \"UnitCode\": \"\", \"UnitName\": \"杭州浙中链条有限公司\", \"LegalName\": \"俞鸿\", \"LegalPhone\": \"13336117333\", \"UnitType\": 5 }, { \"UnitCode\": \"\", \"UnitName\": \"浙江中南建设集团钢结构有限公司\", \"LegalName\": \"刘吾明\", \"LegalPhone\": \"86243888\", \"UnitType\": 1 } ] }, { \"ProjectName\": \"萧政储出(2018)17号地块居住用地项目——配建兴五路小学与社会停车场\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191002124\", \"Coordinate\": \",\", \"BuildingArea\": \"28603.68\", \"Supervisor\": \"李炜，周壁隽，来维新\", \"BuildManager\": \"王小忠\", \"AddUserName\": \"13958010666\", \"AddTrueName\": \"王小忠\", \"AddDate\": \"2019-06-13\", \"Units\": [ { \"UnitCode\": \"74512457-h\", \"UnitName\": \"杭州南兴房地产开发有限公司\", \"LegalName\": \"想小华\", \"LegalPhone\": \"13521234124\", \"UnitType\": 5 }, { \"UnitCode\": \"91330783147520019P\", \"UnitName\": \"中天建设集团有限公司\", \"LegalName\": \"楼永良\", \"LegalPhone\": \"28860018\", \"UnitType\": 1 }, { \"UnitCode\": \"91330104662322199G\", \"UnitName\": \"浙江元正工程管理有限公司\", \"LegalName\": \"张建华\", \"LegalPhone\": \"0571-86974302\", \"UnitType\": 2 } ] }, { \"ProjectName\": \"年组装500台套脱硫（除臭）脱硝除尘环保设备项目\", \"ProjectCategory\": \"1\", \"RegNumber\": \"20191003125\", \"Coordinate\": \",\", \"BuildingArea\": \"4672.00\", \"Supervisor\": \"来春燕，高志法，瞿俊平 \", \"BuildManager\": \"钱丰标\", \"AddUserName\": \"13967110545\", \"AddTrueName\": \"钱丰标\", \"AddDate\": \"2019-06-17\", \"Units\": [ { \"UnitCode\": null, \"UnitName\": \"杭州荣丰环保科技有限公司\", \"LegalName\": null, \"LegalPhone\": null, \"UnitType\": 5 }, { \"UnitCode\": \"14358606-1\", \"UnitName\": \"浙江联谊建筑工程有限公司\", \"LegalName\": \"姚正东\", \"LegalPhone\": \"13806503308\", \"UnitType\": 1 } ] } ] }";
                InterfaceXspmProject xspmProjectData = null;
                try
                {
                    xspmProjectData = jss.Deserialize<InterfaceXspmProject>(data);
                }
                catch (Exception e)
                {
                    SysLog4.WriteError(String.Format("工程信息格式不正确，原因：{0}，数据包：{1}", e.Message, data));
                    ret.msg = String.Format("工程信息格式不正确，原因：{0}，数据包：{1}", e.Message, data);
                }       
                //返回数据包
                ret.data = xspmProjectData;
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("下载品铭工程信息出错，原因：{0}", ex.Message));
                ret.msg = String.Format("下载品铭工程信息出错，原因：{0}", ex.Message);
            }
            return ret;
        }
        #endregion
    }
}
