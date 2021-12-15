function show() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var url = "/WebList/EasyUiIndex?FormDm=DWYDKQBB&FormStatus=1&MenuCode=DWYDKQBB&FormParam=PARAM--" + encodeURIComponent(selected.GCBH) + "|" + encodeURIComponent(selected.QYBH) + "|" + encodeURIComponent(selected.RQ) + "\"";
        parent.layer.open({
            type: 2,
            title: selected.QYMC + '的人员考勤信息',
            shadeClose: true,
            shade: 0.4,
            area: ['60%', '60%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function exportRow() {
    $.ajax({
        url: "/Kqj/ExportKqj",
        dataType: "json",
        data: { exportfilter: JSON.stringify(dataGrid.datagrid("options").filterRules) },
        type: "post",
        beforeSend: function () {
            $.messager.progress({
                msg: "正在导出中......"
            });
        },
        success: function (json) {
            $.messager.progress("close");
            if (json.Status === "success") {
                window.location.href = "/Common/ExportExcel.ashx?fileName=" + encodeURIComponent(json.FileName) + "&url=" + encodeURIComponent(json.Url);
            } else {
                $.messager.alert('提示', json.Msg, 'info');
            }
        },
        error: function () {
            $.messager.progress("close");
            $.messager.alert('提示', '导出出错,请稍后重试!', 'info');
        }
    });
}