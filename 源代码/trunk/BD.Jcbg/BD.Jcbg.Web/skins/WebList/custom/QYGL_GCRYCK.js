function del() {
    try {
        var tabledesc = "人员";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;
        if (!confirm("确定要删除吗？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: "/delete/deletegcry?id=" + encodeURIComponent(selected.RECID)+"&sjbmc="+encodeURIComponent(selected.SJBMC),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败！";
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
