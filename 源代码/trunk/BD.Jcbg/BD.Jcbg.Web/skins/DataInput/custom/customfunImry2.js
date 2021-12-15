function CustomFun() {
    var ret = false;
    if (GetCtrlValue("I_M_RY.TYYHXY") != '1') {
        alert('必须同意用户协议才能注册');
        return ret;
    }
    var zh = GetCtrlValue("I_M_RY.ZH");
    var sfz = GetCtrlValue("I_M_RY.SFZHM");
    var yzm = GetCtrlValue("I_M_RY.SJYZM");
    $.ajax({
        type: "POST",
        url: "/ry/checkregister2?username=" + encodeURIComponent(zh) + "&idno=" + encodeURIComponent(sfz)+"&yzm="+encodeURIComponent(yzm),
        dataType: "json",
        async: false,
        success: function (data) {
            ret = data.code == "0";
            if (data.msg != "") {
                alert(data.msg);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
    return ret;
}

