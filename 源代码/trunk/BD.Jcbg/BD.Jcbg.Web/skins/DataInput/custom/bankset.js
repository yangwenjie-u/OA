function SetYHMC()
{
	 var yhkh=GetCtrlValue("I_M_RY_INFO.YHKH");
	 if(yhkh=="")
	     return;
	 $.ajax({
            type: "POST",
            url: "/jx_info/GetBankName?yhkh=" + encodeURIComponent(yhkh) ,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                   	SetCtrlValue("I_M_RY_INFO.YHKYH",data.msg);
                } else {
                    if (data.msg == "")
                        data.msg = "获取银行失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
}