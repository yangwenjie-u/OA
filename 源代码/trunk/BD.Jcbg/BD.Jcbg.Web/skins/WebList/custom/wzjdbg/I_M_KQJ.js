function add() {
    try {
        var tabledesc = "考勤机";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_KQJ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=2" +
            "&preproc=InputCheckImkqj|$i_m_kqj.KQJBH" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['800px', '400px'],
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
        var tabledesc = "考勤机";
        var selected = pubselect();
        if (selected == undefined)
            return;


        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_KQJ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&preproc=InputCheckImkqj|$i_m_kqj.KQJBH" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['800px', '400px'],
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
        var tabledesc = "考勤机";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_KQJ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['800px', '400px'],
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
        var tabledesc = "考勤机";

        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteimkqj?recid=" + encodeURIComponent(selected.RECID),
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

function dowIris(){
    try {
        var tabledesc = "考勤机";

        var selected = pubselect();
        if (selected == undefined)
            return;

        
        if (selected.QYBH == "") {
            alert("考勤机不属于企业，无法关联人员");
            return;
        }
        
        $.ajax({
            type: "POST",
            url: "/kqj/downkqjiris?kqjbh=" + encodeURIComponent(selected.KQJBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("下发操作成功，等待考勤机下载！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "下发失败";
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
function dowIrisInner() {
    try {
        var tabledesc = "考勤机";

        var selected = pubselect();
        if (selected == undefined)
            return;

        /*
        if (selected.QYBH != "") {
            alert("考勤机属于企业，不能下发内部人员模板");
            return;
        }
        */
        $.ajax({
            type: "POST",
            url: "/kqj/downkqjiris?kqjbh=" + encodeURIComponent(selected.KQJBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("下发操作成功，等待考勤机下载！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "下发失败";
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
function init() {
    try {
        var tabledesc = "考勤机";

        var selected = pubselect();
        if (selected == undefined)
            return;


        $.ajax({
            type: "POST",
            url: "/kqj/initkqj?kqjbh=" + encodeURIComponent(selected.KQJBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("设置初始化命令成功，等待考勤机执行！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "设置失败";
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

function restart() {
    try {
        var tabledesc = "考勤机";

        var selected = pubselect();
        if (selected == undefined)
            return;


        $.ajax({
            type: "POST",
            url: "/kqj/restartkqj?kqjbh=" + encodeURIComponent(selected.KQJBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("设置重启命令成功，等待考勤机执行！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "设置失败";
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