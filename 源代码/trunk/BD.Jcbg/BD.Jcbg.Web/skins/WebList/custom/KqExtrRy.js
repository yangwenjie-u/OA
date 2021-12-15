

function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("KqExtrRy"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("考勤额外信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = "";
        var sufproc = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&fieldparam=" + fieldparam +
            "&sufproc=" + sufproc +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '人员添加',
            shadeClose: false,
            shade: 0.8,
            area: ['70%', '70%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (ex) {
        alert(ex);
    }
}



function del() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deletetable?ID=" + encodeURIComponent(selected.RECID) + "&name=RECID&table=KqExtrRy",
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