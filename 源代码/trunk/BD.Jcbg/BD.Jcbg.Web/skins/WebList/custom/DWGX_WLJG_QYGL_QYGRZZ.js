
function copy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_QY_QYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZZBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.ZZBH)   // 键值
        var title = encodeURIComponent("企业资质信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_QY_QYZZ,QYBH," + selected.QYBH)
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制企业资质信息',
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

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_QY_QYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZZBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.ZZBH)   // 键值
        var title = encodeURIComponent("资质信息"); 	// 标题
        var buttons = encodeURIComponent("保存|TJ| | |保存成功"); // 按钮
        var fieldparam = encodeURIComponent("I_S_QY_QYZZ,QYBH," + selected.QYBH + "|I_S_QY_QYZZ,SPTG,1|I_S_QY_QYZZ,SFYX,1");
        var sufproc = "";//encodeURIComponent("QYZZSPSendMail|" + jydbh + "|" + selected.QYBH + "|0");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&sufproc=" + sufproc +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '企业资质修改申请',
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
        /*
        var sptg = selected.SPTG;
        if (sptg == "True") {
            alert("资质申请已审批，无法删除");
            return;
        }*/
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteisqyqyzzanyway?ZZBH=" + encodeURIComponent(selected.ZZBH),
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

function add() {
    try {
        $.ajax({
            type: "POST",
            url: "/qy/getqybh",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    var qybh = data.msg;

                    var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
                    var tablename = encodeURIComponent("I_S_QY_QYZZ"); 			// 表名
                    var tablerecid = encodeURIComponent("ZZBH"); 	// 表主键
                    var title = encodeURIComponent("资质信息"); 	// 标题
                    var buttons = encodeURIComponent("保存|TJ| | |保存成功"); // 按钮
                    var fieldparam = encodeURIComponent("I_S_QY_QYZZ,QYBH," + qybh);
                    var sufproc = "";//encodeURIComponent("QYZZSPSendMail|$JYDBH$|"+qybh+"|1");
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
                        title: '企业资质申请',
                        shadeClose: true,
                        shade: 0.8,
                        area: ['95%', '95%'],
                        content: url,
                        end: function () {
                            searchRecord();
                        }
                    });

                } else {
                    alert("获取当前账号对应的企业信息失败，请联系管理员");
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