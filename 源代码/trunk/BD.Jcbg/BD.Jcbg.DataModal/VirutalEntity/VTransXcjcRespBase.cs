using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespBase
    {
        public const int Success = 0;               // 成功
        public const int ErrorException = 1;        // 异常
        public const int ErrorUserLoginError = 2;   // 登录失败
        public const int ErrorSaveSession = 3;      // 保存session失败
        public const int ErrorParamCheck = 4;       // 参数校验失败
        public const int ErrorUserNotLogin = 5;     // 用户未登录
        public const int ErrorBindSimCode = 6;      // 绑定sim卡号失败
        public const int ErrorGetCompanyCode = 7;   // 获取人员企业编号失败
        public const int ErrorGetSyxmList = 8;      // 获取试验项目异常
        public const int ErrorGetWtdList = 9;       // 获取试验列表异常
        public const int ErrorGetSybwList = 10;     // 获取部位列表异常
        public const int ErrorGetSxtList = 11;     // 获取摄像头列表异常        
        public const int CodeSxtNotOnline = 12;     // 摄像头不在线
        public const int ErrorGetSxtOnline = 13;     // 查询摄像头是否在线失败
        public const int ErrorStartExperment = 14;  // 开始试验异常
        public const int ErrorUploadImage = 15;     // 上传照片异常
        public const int ErrorStopExperment = 16;   // 停止试验异常
        public const int ErrorGetInSybh = 17;       // 获取试验中编号异常
        public const int ErrorInvalidImage = 18;       // 无效的图片
        public const int ErrorUploadToUserSystem = 19;       // 签名上传到用户系统失败
        public const int ErrorSaveSign = 20;       // 签名保存到数据库失败
        public const int ErrorGetUserType = 21;     // 获取人员类型异常
        public const int ErrorGetJzrySybh = 22;     // 见证人员获取见证试验编号异常
        public const int ErrorGetSyrySybh = 23;     // 送样人员获取见证试验编号异常
        public const int ErrorGetJzryGclb = 24;     // 见证人员获取工程列表异常
        public const int ErrorGetSyryGclb = 25;     // 送样人员获取工程列表异常
        public const int ErrorGetWtdJzInfo = 26;    // 获取委托单见证信息失败
        public const int ErrorSetJzqyzt = 27;       // 见证取样确认失败
        public const int ErrorGetJzqySyInfo = 28;       // 获取试验信息失败
        public const int ErrorSimUsed = 29;       // 手机已绑定其他用户
        public const int ErrorVideoNull = 30;       // 视频文件为空或者不正确的json数组
        public const int ErrorUploadVideo = 31;     // 上传视频失败
        public const int ErrorSimHasUse = 32;       // sim卡已绑定其他用户
        public const int ErrorUpdateCjsybh = 101;   // 设置厂家试验编号失败
        public const int ErrorGetWtdDevList = 1001;    // 获取试验设备列表异常
        public static string GetErrorInfo(int code)
        {
            string msg = "";
            switch (code)
            {
                case ErrorException:
                    msg = "异常";
                    break;
                case ErrorUserLoginError:
                    msg = "登录失败";
                    break;
                case ErrorSaveSession:
                    msg = "保存会话失败";
                    break;
                case ErrorParamCheck:
                    msg = "参数校验失败";
                    break;
                case ErrorUserNotLogin:
                    msg = "用户未登录";
                    break;
                case ErrorBindSimCode:
                    msg = "绑定SIM卡号失败，可能是当前账号非人员账号";
                    break;
                case ErrorGetCompanyCode:
                    msg = "获取人员企业信息失败，可能是当前人员未录用到企业";
                    break;
                case ErrorGetSyxmList:
                    msg = "获取试验项目异常";
                    break;
                case ErrorGetWtdList:
                    msg = "获取试验列表异常";
                    break;
                case ErrorGetSybwList:
                    msg = "获取部位列表异常";
                    break;
                case ErrorGetSxtList:
                    msg = "获取摄像头列表异常";
                    break;
                case CodeSxtNotOnline:
                    msg = "摄像头不在线";
                    break;
                case ErrorGetSxtOnline:
                    msg = "查询摄像头是否在线异常";
                    break;
                case ErrorStartExperment:
                    msg = "开始试验异常";
                    break;
                case ErrorUploadImage:
                    msg = "上传照片异常";
                    break;
                case ErrorStopExperment:
                    msg = "停止试验异常";
                    break;
                case ErrorGetInSybh:
                    msg = "获取试验中编号异常";
                    break;
                case ErrorInvalidImage:
                    msg = "无效的图片文件";
                    break;
                case ErrorUploadToUserSystem:
                    msg = "签名上传到用户系统失败";
                    break;
                case ErrorSaveSign:
                    msg = "签名保存到数据库失败";
                    break;
                case ErrorGetUserType:
                    msg = "获取人员类型异常";
                    break;
                case ErrorGetJzrySybh:
                    msg = "获取见证人员见证试验编号异常";
                    break;
                case ErrorGetSyrySybh:
                    msg = "获取收样人员见证试验编号异常";
                    break;
                case ErrorGetJzryGclb:
                    msg = "获取见证人员工程列表异常";
                    break;
                case ErrorGetSyryGclb:
                    msg = "获取送样人员工程列表异常";
                    break;
                case ErrorGetWtdJzInfo:
                    msg = "获取委托单见证信息失败";
                    break;
                case ErrorSetJzqyzt:
                    msg = "见证取样确认失败";
                    break;
                case ErrorGetJzqySyInfo:
                    msg = "获取试验信息失败";
                    break;
                case ErrorSimUsed:
                    msg = "手机已绑定其他用户";
                    break;
                case ErrorVideoNull:
                    msg = "视频文件为空或者不正确的json数组";
                    break;
                case ErrorUploadVideo:
                    msg = "上传视频失败";
                    break;
                case ErrorSimHasUse:
                    msg = "sim卡已绑定其他用户";
                    break;
            }
            return msg;
        }
        public int code { get; set; }
        public string msg { get; set; }
    }
}
