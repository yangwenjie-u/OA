using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Common;
using BD.Jcbg.Service.JyJzqyWebService;

namespace BD.Jcbg.Service.Jc
{
    public class JyJzqyService
    {
        private readonly static InterfaceService InterfaceService = new InterfaceService();

        /// <summary>
        /// 工程信息同步
        /// </summary>
        /// <param name="projectNum">工程编码（必填）</param>
        /// <param name="projectName">工程名称（必填）</param>
        /// <param name="constractionUnit">施工单位</param>
        /// <param name="inspectUnit">监理单位</param>
        /// <param name="slpeopleJson">取样员信息（必填）</param>
        /// <param name="spnpeopleJson">取样员信息（必填）</param>
        /// <returns></returns>
        public static ResultParam SynProjectInfo(
            string projectNum,
            string projectName,
            string constractionUnit,
            string inspectUnit,
            string slpeopleJson,
            string spnpeopleJson,
            string createunit)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.SynProjectInfo(projectNum, projectName, constractionUnit, inspectUnit, slpeopleJson, spnpeopleJson, createunit);

                ret = HandleResponse(response);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.success = false;                
                ret.msg = ex.Message;
            }

            return ret;
        }
        
        /// <summary>
        /// 删除工程
        /// </summary>
        /// <param name="projectNum"></param>
        /// <returns></returns>
        public static ResultParam DelProjectInfo(string projectNum)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.DelProjectInfo(projectNum);

                ret = HandleResponse(response);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.success = false;
                ret.msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 人员信息变更
        /// </summary>
        /// <param name="peopleNo">人员编码（必填）</param>
        /// <param name="peopleName">人员姓名（必填）</param>
        /// <param name="peopleType">人员身份（必填）</param>
        /// <returns></returns>
        public static ResultParam UpdatePeopleInfo(
            string peopleNo,
            string peopleName,
            string peopleType)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.UpdatePeopleInfo(peopleNo, peopleName, peopleType);

                ret = HandleResponse(response);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.success = false;
                ret.msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 见证取样记录查询
        /// </summary>
        /// <param name="projectNum">工程编码</param>
        /// <param name="itemCode">项目代号</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="syyNum">取样员编码</param>
        /// <param name="jzyNum">见证员编码</param>
        /// <param name="recordId">记录唯一编码</param>
        /// <param name="qrCode">二维码</param>
        /// <param name="orderStatus">委托状态</param>
        /// <returns></returns>
        public static ResultParam UpLoadDataList(
            string projectNum,
            string gcbw,
            string itemCode,
            string startTime,
            string endTime,
            string syyNum,
            string jzyNum,
            string recordId,
            string qrCode,
            int orderStatus = 0
        )
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.UpLoadDataList(projectNum, gcbw, itemCode,
                                  startTime, endTime, syyNum, jzyNum, recordId, qrCode, orderStatus);

                ret = HandleResponse(response, 1);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 见证取样记录查询
        /// </summary>
        /// <param name="projectNum">工程编码</param>
        /// <param name="itemCode">项目代号</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="syyNum">取样员编码</param>
        /// <param name="jzyNum">见证员编码</param>
        /// <param name="recordId">记录唯一编码</param>
        /// <param name="qrCode">二维码</param>
        /// <param name="orderStatus">委托状态</param>
        /// <returns></returns>
        public static ResultParam UpLoadDataListPage(
            string projectNum,
            string gcbw,
            string itemCode,
            string startTime,
            string endTime,
            string jstartTime,
            string jendTime,
            string syyNum,
            string jzyNum,
            string syyname, 
            string jzyname, 
            string recordId,
            string qrCode,
            int orderStatus,
            int page,
            int limit,
            out int total
        )
        {
            ResultParam ret = new ResultParam();
            total = 0;

            try
            {
                string response = InterfaceService.UpLoadDataListPage(projectNum, gcbw, itemCode,
                                  startTime, endTime, jstartTime, jendTime, syyNum, jzyNum, syyname, jzyname, recordId, qrCode, orderStatus, page, limit);

                string msg = string.Empty;
                var responseResult = JsonSerializer.Deserialize<JzqyUpLoadDataListPageResponse>(response, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var reason = responseResult.Reason.GetSafeString();

                if (responseResult.Results == "成功")
                {
                    ret.success = true;
                    ret.msg = "获取数据成功";
                    ret.data = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(reason);
                    total = responseResult.Count;
                }
                else
                {
                    ret.msg = reason;
                }

                return ret;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取工程修改记录
        /// </summary>
        /// <param name="projectNum">工程编码(必填)</param>
        /// <returns></returns>
        public static ResultParam GetProjectUpdateData(string projectNum)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.GetProjectUpdateData(projectNum);

                ret = HandleResponse(response);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取见证取样修改记录
        /// </summary>
        /// <param name="recordId">见证取样记录唯一编码（必填）</param>
        /// <returns></returns>
        public static ResultParam GetIteminfoUpdateData(string recordId)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.GetIteminfoUpdateData(recordId);

                ret = HandleResponse(response, 1);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 变更见证取样委托状态
        /// </summary>
        /// <param name="recordList">见证取样记录唯一编码集合（必填）</param>
        /// <param name="orderstatus">委托状态（0，未委托：1，已委托）</param>
        /// <param name="ordernum">委托单编码（已委托状态，必填）</param>
        /// <returns></returns>
        public static ResultParam UpdateOrderStatus(List<string> recordList, int orderStatus, string orderNum)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string[] records = recordList.ToArray<string>();
                string response = InterfaceService.UpdateOrderStatus(records, orderStatus, orderNum);

                ret = HandleResponse(response);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取样品照片Url路径
        /// </summary>
        /// <param name="recordId">见证取样记录唯一编码（必填）</param>
        /// <returns></returns>
        public static ResultParam GetDataReocrdPhotoUrl(string recordId)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.GetDataReocrdPhotoUrl(recordId);

                ret = HandleResponse(response, 2);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取样品照片base64格式
        /// </summary>
        /// <param name="recordId">见证取样记录唯一编码（必填）</param>
        /// <returns></returns>
        public static ResultParam GetDataReocrdPhotoBase64(string recordId)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.GetDataReocrdPhotoBase64(recordId);

                ret = HandleResponse(response, 2);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 解除当前人员登录状态
        /// </summary>
        /// <param name="peopleNum">人员编码（必填）</param>
        /// <returns></returns>
        public static ResultParam RelievePeopleStatus(string peopleNum)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string response = InterfaceService.RelievePeopleStatus(peopleNum);

                ret = HandleResponse(response);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.success = false;
                ret.msg = e.Message;
            }

            return ret;
        }

        /// <summary>
        /// 处理Response响应结果
        /// </summary>
        /// <param name="response"></param>
        /// <param name="type">默认data 0 - 不返回, 1 - 序列化list, 2 - 序列化dict </param>
        /// <returns></returns>
        public static ResultParam HandleResponse(string response, int type = 0)
        {
            ResultParam ret = new ResultParam
            {
                success = false,
                msg = string.Empty
            };

            string msg = string.Empty;
            var responseResult = JsonSerializer.Deserialize<JzqyServiceResponseBase>(response, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                ret.msg = msg;
                return ret;
            }

            var reason = responseResult.Reason.GetSafeString();

            if (responseResult.Results == "成功")
            {
                ret.success = true;
                ret.msg = "获取数据成功";

                if (type == 1)
                {
                    if (!string.IsNullOrEmpty(reason) && reason.IndexOf("[") != -1)
                        ret.data = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(reason);
                    else
                        ret.data = new List<Dictionary<string, string>>();
                }
                else if (type == 2)
                {
                    if (!string.IsNullOrEmpty(reason) && reason.IndexOf("{") != -1)
                        ret.data = JsonSerializer.Deserialize<Dictionary<string, object>>(reason);
                    else
                        ret.data = new Dictionary<string, object>();
                }
            }
            else
            {
                ret.msg = reason;
            }

            return ret;
        }  
    }
}