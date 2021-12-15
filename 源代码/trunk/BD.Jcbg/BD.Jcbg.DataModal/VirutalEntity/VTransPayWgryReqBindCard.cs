using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryReqBindCard: VTransPayWgryReqBase
    {
        public VTransPayWgryReqBindCardMain RootData
        {
            get
            {
                if (data == null)
                    return null;
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                VTransPayWgryReqBindCardMain ret = jsonSerializer.Deserialize<VTransPayWgryReqBindCardMain>(data);
                return ret;
            }
        }
    }

    public class VTransPayWgryReqBindCardMain
    {
        public string bankplatform { get; set; } // 银行平台，不传默认为华夏HXB
        public string paycompanycode { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string idnumber { get; set; }
        public string cardnumber { get; set; }
        public string banknumber { get; set; }

        public bool IsValid(out string msg)
        {
            if (string.IsNullOrEmpty(bankplatform))
                bankplatform = "HXB";
            paycompanycode = paycompanycode.GetValidPayString();
            name = name.GetValidPayString();
            phone = phone.GetValidPayString();
            string decodeIdnumber = CryptFun.LrDecode(idnumber).GetValidPayString();
            idnumber = CryptFun.LrEncode(decodeIdnumber);
            string decodeCardnumber = CryptFun.LrDecode(cardnumber).GetValidPayString();
            cardnumber = CryptFun.LrEncode(decodeCardnumber);
            banknumber = banknumber.GetValidPayString();
            bool ret = true;
            msg = "";
            if (string.IsNullOrEmpty(name))
                msg += "姓名不能为空，";
            if (string.IsNullOrEmpty(decodeIdnumber))
                msg += "身份证号码不能为空，";
            else if (!IDCardValidation.CheckIDCard(decodeIdnumber))
                msg += "身份证号码格式不对，";
            if (string.IsNullOrEmpty(decodeCardnumber))
                msg += "银行卡不能为空，";
            msg = msg.Trim(new char[] { '，' });
            return msg.Length == 0;
        }
    }
}
