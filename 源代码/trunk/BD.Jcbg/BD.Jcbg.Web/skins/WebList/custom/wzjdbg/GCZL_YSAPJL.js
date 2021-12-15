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

function printjdjlreport() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var reportfile = '验收安排监督记录v1';
        var serial = selected.WorkSerial;
        var recid = selected.JDJLID;
        if (serial==null ||  serial=='') {
            alert('当前验收安排记录来自于老系统,无法打印监督记录表！');
            return;
        }
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&reporttype=YSAPJDJL",
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}