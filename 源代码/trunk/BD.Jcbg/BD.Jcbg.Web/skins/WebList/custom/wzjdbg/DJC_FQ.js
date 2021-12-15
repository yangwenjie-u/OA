function showInfo() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var djcfqbh = selected.DJCFQBH;
    layer.open({
        type: 2,
        title: '查看工程和人员信息',
        shadeClose: false,
        shade: 0.8,
        area: ['1000px', '600px'],
        content: '/jdbg/showmajorinspectioninfo?djcfqbh=' + encodeURIComponent(djcfqbh),
        end: function () {
        }
    });
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

function del() {
    try {
        var tabledesc = "记录删除";                // 表格描述

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}

        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();

        if (!selected) {
            return;
        }


        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeleteDJC?RECID=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
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
