function check(row) {
    var rybh = row.RYBH;
    //var yxrq = row.YXRQ;
    var yxrq = '2099-01-01';
    var rylx = row.RYLXMC;
    if (rylx == "建设人员")
        return true;
    if (yxrq == "") {
        alert("该证书有效期无效！");
        return false;
    }
    var flag = true;
    $.ajax({
        type: "POST",
        url: "/ry/getryinfo?rybh=" + encodeURIComponent(rybh) + "&yxrq=" + encodeURIComponent(yxrq),
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.code != "0") {
                var msg = data.msg;
                alert(msg);
                flag = false;
            }
            else {

            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });

    return flag;
}