using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryRespBase
    {
        public const string ErrorSuccess = "0000";
        public const string ErrorException = "0001";
        public const string ErrorCheckCode = "0002";
        public const string ErrorParam = "0003";
        public const string ErrorExists = "0004";
        public const string ErrorBindCardNotComplete = "0005";
        public const string ErrorBindCardException = "0006";
        public const string ErrorBindCardComplete = "0007";
        public const string ErrorBindCardSaveDbError = "0008";
        public const string ErrorBindCardBind = "0009";
        public const string ErrorNotExists = "0010";
        public const string ErrorGetPayList = "0011";
        public const string ErrorGetPayListDetail = "0012";
        public const string ErrorReSubmitDraw = "0013";
        public const string ErrorRepert = "0014";

        public VTransPayWgryRespBase()
        {
            success = ErrorSuccess;
            message = "";
        }
        public static string GetErrorInfo(string code, string extrainfo = "")
        {
            string msg = "";
            switch (code)
            {
                case ErrorException:
                    msg = "系统异常";
                    break;
                case ErrorCheckCode:
                    msg = "密钥验证失败";
                    break;
                case ErrorParam:
                    msg = "参数无效";
                    break;
                case ErrorExists:
                    msg = "纪录已存在";
                    break;
                case ErrorBindCardNotComplete:
                    msg = "正在调用接口绑卡";
                    break;
                case ErrorBindCardException:
                    msg = "绑卡异常";
                    break;
                case ErrorBindCardComplete:
                    msg = "绑卡已成功";
                    break;
                case ErrorBindCardSaveDbError:
                    msg = "保存到本地数据库失败";
                    break;
                case ErrorBindCardBind:
                    msg = "绑卡错误";
                    break;
                case ErrorNotExists:
                    msg = "记录不存在";
                    break;
                case ErrorGetPayList:
                    msg = "获取支付列表失败";
                    break;
                case ErrorGetPayListDetail:
                    msg = "获取支付列表详情失败";
                    break;
                case ErrorReSubmitDraw:
                    msg = "修改提现卡号失败";
                    break;
                case ErrorRepert:
                    msg = "记录重复";
                    break;

            }
            if (extrainfo != "")
            {
                if (msg != "")
                    msg += "：";
                msg += extrainfo;
            }
            return msg;
        }
        public string success { get; set; }
        public string message { get; set; }
        
        public bool IsSuccess
        {
            get { return success == "0000"; }
        }
    }
}
