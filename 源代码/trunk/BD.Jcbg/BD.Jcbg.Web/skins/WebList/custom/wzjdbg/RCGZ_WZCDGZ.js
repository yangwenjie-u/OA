function startWork() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/workflow/startwork?processid=" + selected.ProcessId + "&serial=" + selected.SerialNo + "&preurldone=true&DlgId=1&_=" + Math.random();

        parent.layer.open({
            type: 2,
            shadeClose: false,
            title:'',
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function cancelWork() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要取消所选的工作吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/workflow/canceltask?serial=" + selected.SerialNo,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("取消成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "取消失败";
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