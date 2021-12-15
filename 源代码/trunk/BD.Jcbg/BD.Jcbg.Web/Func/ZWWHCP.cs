using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atg.Api;
using Atg.Api.Request;
using Atg.Api.Util;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Func
{
    /// <summary>
    /// 绍兴市建筑业企业资质管理系统使用
    /// 政务网好差评接口调用
    /// </summary>
    public class ZWWHCP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affairId">办件编号，如 330601190620108066200</param>
        /// <param name="affairType">办件类型 1 即办件 2承诺件</param>
        /// <param name="affairStatus">办件状态 1 收件 2 办结</param>
        /// <param name="affairTime"> 办件状态为收件时传收件时间，办件类型为办结时传办结时间</param>
        /// <param name="identityType">用户证件类型 证件类型编码 身份证 31  营业执照 51</param>
        /// <param name="userIdentityNum">证件号码 用户类型为“个人”时，填写身份证号或其他身份证件号码，用户类型为“法人”时，填写各类证照中的统一社会信用代码</param>
        /// <param name="userType">用户类型 个人，填：person ;法人，填：legal</param>
        /// <param name="matterId">填写事项库的事项唯一码</param>
        /// <param name="operatorName">经办人姓名</param>
        /// <param name="url">返回的评价地址</param>
        /// <param name="msg">返回的错误信息</param>
        /// <returns></returns>
        public static bool GetPJUrl(
            string affairId, long affairType,
            long affairStatus, string affairTime,
            string identityType,string userIdentityNum,
            string userType,string matterId,
            string operatorName,
            string areaCode, string areaName,
            out string url, out string msg)
        {
            bool ret = true;
            url = "";
            msg = "";
            try
            {
                // 接口地址
                string gatewayUrl =Configs.GetConfigItem("zwpj_gatewayUrl");
                //应用ID
                string appId = Configs.GetConfigItem("zwpj_appId");
                //商户私钥
                AtgBusSecretKey secretKey = new AtgBusSecretKey(Configs.GetConfigItem("zwpj_keyId"), Configs.GetConfigItem("zwpj_secretKey"));
                //请求客户端
                IAtgBusClient client = new DefaultAtgBusClient(gatewayUrl, appId, secretKey);

                var request = new AtgBizComAlibabaGovEvaluationQueryPublishurlRequest();
                request.affairId = affairId; // 赋码平台提供的统一办件ID， 有问题
                request.affairType = affairType; // 办件类型 1 即办件 2承诺件
                request.affairStatus = affairStatus; // 办件状态 1 收件 2 办结
                                          // 办件状态为收件时传收件时间，办件类型为办结时传办结时间
                request.affairTime = affairTime; // 收件时间或者办件时间
                // 证件类型： 
                //身份证 31  营业执照 51                                                                   
                request.identityType = identityType;
                // 用户身份证号 / 社会统一信用码
                // 用户类型为个人时传身份证号，用户类型为法人时传社会统一信用码
                request.userIdentityNum = userIdentityNum;
                // 用户类型 
                //个人：person
                //法人：legal
                request.userType = userType;
                // 事项ID
                // 事项库的事项ID，
                request.matterId = matterId;
                // 区域代码
                //绍兴市本级	330600
                //越城区（高新区、袍江开发区）	330602
                //柯桥区	330603
                //上虞区	330604
                //新昌县	330624
                //诸暨市	330681
                //嵊州市	330683
                //袍江开发区管委会	330651
                //滨海新城管委会	330652
                //绍兴高新区管委会	330653
                //镜湖新区开发办	330654
                request.areaCode = areaCode;
                request.areaName = areaName;

                // 经办部门Code
                // 经办部门编码，需要和统一部门编码库一致
                request.departCode = Configs.GetConfigItem("zwpj_departCode"); // "001008006007014";
                // 经办部门名称
                request.departName = Configs.GetConfigItem("zwpj_departName"); //"绍兴市市建设局";
                // 经办部门统一社会信用代码
                request.departIdentityCode = Configs.GetConfigItem("zwpj_departIdentityCode"); ;// "113306000025771029";

                // 经办人姓名
                request.operatorName = operatorName;
                // 评价渠道，默认为二维码，
                //线上网厅: PC_ONLINE
                request.channel = "PC_ONLINE";

                var response = client.Execute(request);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string jsonstring = jss.Serialize(response);
                SysLog4.WriteError(jsonstring);
                ret = response.success;
                if (ret)
                {
                    url = response.data;
                }
                else
                {
                    msg = response.errorMsg;
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
                
            }
            return ret;
        }
    }
}