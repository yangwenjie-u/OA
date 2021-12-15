function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮

        var fieldparam = "I_M_QY,SPTG,0";

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=2" +
            "&LX=N" +
            "&fieldparam=" + fieldparam +
            "&_=" + rdm;

        
        parent.layer.open({
            type: 2,
            title: '录入企业信息',
            shadeClose: true,
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

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = "I_M_QY,SPTG,0";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&LX=N" +
            "&fieldparam=" + fieldparam +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制企业信息',
            shadeClose: true,
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

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
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
            title: '修改企业信息',
            shadeClose: true,
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
            url: "/delete/deleteiqy?qybh=" + encodeURIComponent(selected.QYBH),
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

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = ""; // 按钮
        var rdm = Math.random();

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&view=true" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看企业信息',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                //searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
