function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = '竣工验收通知书';
        var serial = selected.WORKSERIAL;
        if (reportfile == null || reportfile == "" || serial == null || serial=="") {
            parent.layer.alert("当前记录来自于老系统，无法查看通知书！");
        }
        else {
            parent.parent.layer.open({
                type: 2,
                title: '报表查看',
                shadeClose: false,
                shade: 0.8,
                area: ['95%', '95%'],
                content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&reporttype=JGYSTZS",
                end: function () {
                    searchRecord();
                }
            });
        }
        
    } catch (ex) {
        alert(ex);
    }
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