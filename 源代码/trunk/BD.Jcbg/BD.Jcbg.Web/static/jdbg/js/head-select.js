
function setYear(ctrlName, nulltitle) {
    var curYear = (new Date()).getFullYear();
    $("#" + ctrlName).empty();
    for (i = 2010; i <= curYear; i++) {
        $("#" + ctrlName).prepend("<option value='"+i+"'>"+i+"年</option>");
    }
    if (nulltitle == "")
        nulltitle = "请选择";
    $("#" + ctrlName).prepend("<option value=''>"+nulltitle+"</option>");
    $("#" + ctrlName).val("");
}

function setGclx(ctrlName) {
    ajaxPost("/jdbg/getgclx", "", function (data) {
        var gclxs = data.records;

        var str = "";
        if (gclxs.length > 1 )
            str += "<option value=''>请选择工程类型</option>";
        $.each(gclxs, function (i, item) {
            str += "<option value='" + item.lxbh + "'>" + item.lxmc + "</option>";
        });

        $("#gclx").html(str);

    });
}