function add() {
    try {
        var tabledesc = "质监站";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("H_ZJZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZJZBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮

        var s_tablename = encodeURIComponent("I_S_ZJZ_JCZX");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("ZJZBH,RECID");
        var s_title = encodeURIComponent("送样检测机构");


        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&type=leftright"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
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
        var tabledesc = "质监站"; 
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("H_ZJZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZJZBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.ZJZBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_ZJZ_JCZX");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("ZJZBH,RECID");
        var s_title = encodeURIComponent("送样检测机构");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&type=leftright" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
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
        var tabledesc = "质监站";   
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("H_ZJZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZJZBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.ZJZBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_ZJZ_JCZX");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("ZJZBH,RECID");
        var s_title = encodeURIComponent("送样检测机构");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&type=leftright" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
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
        var tabledesc = "质监站";      
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deletehzjz?recid=" + encodeURIComponent(selected.ZJZBH),
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