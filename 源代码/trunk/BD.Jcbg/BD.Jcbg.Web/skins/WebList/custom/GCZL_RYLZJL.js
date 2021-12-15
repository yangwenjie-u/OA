function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        reportfile = GetAvailableReportFile(reportfile, serial);
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial),
            end: function () {
                searchRecord();
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

// 获取实际可用的reportfile
// 主要考虑到，手机填写的监督记录，在后台没有生成模板文件，也就是reportfile为空
// 因此，如果在pc端查看手机填写的监督记录，需要到后台先生成模板文件
function GetAvailableReportFile(reportfile, serial) {
    var realReportFile = reportfile;

    if (realReportFile == null || realReportFile == '') {
        $.ajax({
            type: "POST",
            url: "/jdbg/GeneratePhoneJDJLTemplate",
            data: "template=" + encodeURIComponent("监督记录v1.docx") + "&serial=" + serial,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0")
                    realReportFile = data.msg;
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }

    return realReportFile;

}

function switchRecord(obj) {
    try {
        var strLocation = decodeURIComponent(window.location.href);
        // 所有
        if (obj.checked) {
            strLocation = strLocation.replace("NOT||CHECKBOX", "ALL||CHECKBOX").replace("所有|我的", "所有|所有");
        }
        else {
            strLocation = strLocation.replace("ALL||CHECKBOX", "NOT||CHECKBOX").replace("所有|所有", "所有|我的");
        }
        window.location = strLocation;
    } catch (ex) {
        alert(ex);
    }
}