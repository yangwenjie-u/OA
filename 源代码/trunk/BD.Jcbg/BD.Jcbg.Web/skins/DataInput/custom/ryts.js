function ryts(a) {
	 var jdzch=GetCtrlValue("I_M_RY_TS.JDZCH");
	 $.ajax({
		type: "POST",
		url: "/jdbg/setryts?jdzch=" +encodeURIComponent(jdzch),
		dataType: "json",
		async: false,
		success: function (data) {
			if (data.msg != "")
				alert(data.msg);
			parent.layer.closeAll();
		},
		complete: function (XMLHttpRequest, textStatus) {
		},
		beforeSend: function (XMLHttpRequest) {
		}
	});
}


function tsfk()
{
	
}