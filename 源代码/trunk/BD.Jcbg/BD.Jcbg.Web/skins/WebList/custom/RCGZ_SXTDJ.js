function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_SXT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("摄像头信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";
        var callback = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入摄像头信息',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function copy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_SXT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("摄像头信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";
        var callback = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制摄像头信息',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_SXT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("摄像头信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改摄像头信息',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
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
            url: "/delete/deletesxt?id=" + encodeURIComponent(selected.RECID),
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


function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/hkws/play?url=" + encodeURIComponent(selected.SXTDZ);

        parent.layer.open({
            type: 2,
            title: '查看摄像头',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });

    } catch (e) {
        alert(e);
    }
}