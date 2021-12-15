
function AfterSubmitFun()
{
    var yhkyh = GetCtrlValue("I_M_WGRY.YHKYH");
    var yhkh = GetCtrlValue("I_M_WGRY.YHKH");
    var sfzhm = GetCtrlValue("I_M_WGRY.SFZHM");
    var ryxm = GetCtrlValue("I_M_WGRY.RYXM");
    var gcbh = GetCtrlValue("I_M_WGRY.JDZCH");
    var sjhm = GetCtrlValue("I_M_WGRY.SJHM");
     $.ajax({
         type: "POST",
         url: "/zj_info/saveYHK_NEW",
         data:{
             yhkyh:yhkyh,
             yhkh:yhkh,
             sfzhm: sfzhm,
             ryxm: ryxm,
             gcbh: gcbh,
             sjhm:sjhm
         },
         dataType: "json",
         async: false,
         success: function (data) {
         if (data.code == "0") {
       
         } else {
         if (data.msg == "")
             data.msg = "保存银行卡信息失败";
         alert(data.msg);
         }
         },
         complete: function (XMLHttpRequest, textStatus) {
         },
         beforeSend: function (XMLHttpRequest) {
         }
     });

	SetCtrlValue("I_M_WGRY.RYBH","");
	SetCtrlValue("I_M_WGRY.SFZHM","");
	SetCtrlValue("I_M_WGRY.RYXM","");
	SetCtrlValue("I_M_WGRY.XB", "");
	SetCtrlValue("I_M_WGRY.CSRQ", "");
	SetCtrlValue("I_M_WGRY.SJHM","");
	SetCtrlValue("I_M_WGRY.HM","");
	SetCtrlValue("I_M_WGRY.YHKYH","");
	SetCtrlValue("I_M_WGRY.YHKH","");
	SetCtrlValue("I_M_WGRY.SFZDZ","");
	SetCtrlValue("I_M_WGRY.QFJG","");
	SetCtrlValue("I_M_WGRY.SFZYXQ","");
	SetCtrlValue("I_M_WGRY.HMZL","");
	SetCtrlValue("I_M_WGRY.ZP","");
	

	 // $.ajax({
            // type: "POST",
            // url: "/jx_info/GetRYBH_New",
            // dataType: "json",
            // async: false,
            // success: function (data) {
                // if (data.code == "0") {
                   	// SetCtrlValue("I_M_RY.RYBH",data.msg);
                // } else {
                    // if (data.msg == "")
                        // data.msg = "获取人员编号失败，请刷新界面";
                    // alert(data.msg);
                // }
            // },
            // complete: function (XMLHttpRequest, textStatus) {
            // },
            // beforeSend: function (XMLHttpRequest) {
            // }
        // });
	
	return false;
}

function SetYHMC()
{
	 var yhkh=GetCtrlValue("I_M_WGRY.YHKH");
	 if(yhkh=="")
		 return;
	 $.ajax({
            type: "POST",
            url: "/zj_info/GetBankName?yhkh=" + encodeURIComponent(yhkh),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                   	SetCtrlValue("I_M_WGRY.YHKYH",data.msg);
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
function dowIris(rybh,hm,sfzhm,jdzch) {
    try {
		if (hm == "") {
			alert("当前人员没有登记虹膜，无法下发");
			return;
		}
		$.ajax({
			type: "POST",
			url: "/kqj/downryiris?rybh=" + encodeURIComponent(rybh),
			dataType: "json",
			async: false,
			success: function (data) {
				if (data.code == "0") {
					// alert("下发操作成功，等待考勤机下载！下发人员需要重启考勤机才能生效，请在考勤机界面重启考勤机。");

				} else {
					if (data.msg == "")
						data.msg = "下发失败";
					alert(data.msg);
				}
			},
			complete: function (XMLHttpRequest, textStatus) {
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
     
    } catch (e) {
        alert(e);
    }
}
function downWgryIris(rybh,hm,sfzhm,jdzch) {
    try {
		if (hm == "") {
			alert("当前人员没有登记虹膜，无法下发");
			return;
		}
		$.ajax({
			type: "POST",
			url: "/kqj/downwgryiris?rybh=" + encodeURIComponent(rybh),
			dataType: "json",
			async: false,
			success: function (data) {
				if (data.code == "0") {
					// alert("下发操作成功，等待考勤机下载！下发人员需要重启考勤机才能生效，请在考勤机界面重启考勤机。");

				} else {
					if (data.msg == "")
						data.msg = "下发失败";
					alert(data.msg);
				}
			},
			complete: function (XMLHttpRequest, textStatus) {
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
     
    } catch (e) {
        alert(e);
    }
}
function dowIris_V2(rybh,hm,sfzhm,jdzch) {
    try {

        $.ajax({
            type: "POST",
            url: "/zj_info/FlowCheckI_M_RY_INFO?sfzhm=" + encodeURIComponent(sfzhm) + "&jdzch=" + encodeURIComponent(jdzch),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    if (hm == "") {
                        alert("当前人员没有登记虹膜，无法下发");
                        return;
                    }
                    $.ajax({
                        type: "POST",
                        url: "/kqj/downryiris?rybh=" + encodeURIComponent(rybh),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.code == "0") {
                                // alert("下发操作成功，等待考勤机下载！下发人员需要重启考勤机才能生效，请在考勤机界面重启考勤机。");

                            } else {
                                if (data.msg == "")
                                    data.msg = "下发失败";
                                alert(data.msg);
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                } else {
                    if (data.msg == "")
                        data.msg = "更新人员库失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
     
    } catch (e) {
        alert(e);
    }
}