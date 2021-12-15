function add() {
    try {
        var tabledesc = "内部合同";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_JCHT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮

        var s_tablename = encodeURIComponent("I_S_JCHT_JZRY");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("JCHTRECID,RECID");
        var s_title = encodeURIComponent("见证人员");
        var sufproc = encodeURIComponent("CheckJcNbhtRy|$JYDBH$");

        var fieldparam = encodeURIComponent("I_M_JCHT,HTLX,QYHT");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=3" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&sufproc=" + sufproc +
            "&fieldparam=" + fieldparam +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
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

function add2() {
    try {
        var tabledesc = "内部合同";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_JCHT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮


        var fieldparam = encodeURIComponent("I_M_JCHT,HTLX,JDHT");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=3" +
            "&fieldparam=" + fieldparam +
            "&LX=J" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
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

        var tabledesc = "内部合同";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名            
        var lx = "N";
        var tablename = encodeURIComponent("I_M_JCHT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var s_tablename = encodeURIComponent("I_S_JCHT_JZRY");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("JCHTRECID,RECID");
        var s_title = encodeURIComponent("见证人员");
        var sufproc = encodeURIComponent("CheckJcNbhtRy|$JYDBH$");
        if (selected.HTLX == 'JDHT') {
            zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
            lx = "J"; // zdzd名
            s_pri = "";
            s_title = ""
            s_tablename = "";
            sufproc = "";
        }
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=3" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&sufproc=" + sufproc +
            "&jydbh=" + jydbh +
            "&LX=" +lx+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
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

        var tabledesc = "内部合同";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_JCHT"); 			// 表名
        var lx = "N";
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var s_tablename = encodeURIComponent("I_S_JCHT_JZRY");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("JCHTRECID,RECID");
        var s_title = encodeURIComponent("见证人员");
        var sufproc = encodeURIComponent("CheckJcNbhtRy|$JYDBH$");
        if (selected.HTLX == 'JDHT') {
            zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
            lx = "J"; // zdzd名
            s_pri = "";
            s_title = ""
            s_tablename = "";
            sufproc = "";
        }
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=3" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&sufproc=" + sufproc +
            "&copyjydbh=" + jydbh +
            "&LX=" +lx+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
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
            url: "/delete/deletenbht?id=" + encodeURIComponent(selected.RECID),
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
    } catch (err) {
        alert(err);
    }
}
function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var tabledesc = "内部合同";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_JCHT"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var s_tablename = encodeURIComponent("I_S_JCHT_JZRY");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("JCHTRECID,RECID");
        var s_title = encodeURIComponent("见证人员");
        var sufproc = encodeURIComponent("CheckJcNbhtRy|$JYDBH$");
        var lx = "N";
        if (selected.HTLX == 'JDHT') {
            zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
            lx = "J"; // zdzd名
            s_pri = "";
            s_title = ""
            s_tablename = "";
            sufproc = "";
        }

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=3" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&sufproc=" + sufproc +
            "&jydbh=" + jydbh +
            "&LX=" +lx+
            "&view=true"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看' + tabledesc,
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

