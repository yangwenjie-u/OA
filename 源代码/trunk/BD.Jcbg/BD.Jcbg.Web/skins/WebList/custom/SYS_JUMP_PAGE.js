function add() {
    try {
        var tabledesc = "页面跳转";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD"); // zdzd名
        var tablename = encodeURIComponent("SysJumpSrcPage"); 			// 表名
        var tablerecid = encodeURIComponent("PageId"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("SysJumpDestPage");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("SourcePageId,PageId");
        var s_title = encodeURIComponent("跳转记录");


        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");
        var sufproc = "";//encodeURIComponent("KPCalculate|$JYDBH$");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&type=leftrightlist" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&LX=N" +
            "&sufproc=" + sufproc +
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
function copy() {
    try {
        var tabledesc = "页面跳转";
        var selected = pubselect();

        if (!selected) {
            return;
        }
        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD"); // zdzd名
        var tablename = encodeURIComponent("SysJumpSrcPage"); 			// 表名
        var tablerecid = encodeURIComponent("PageId"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("SysJumpDestPage");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("SourcePageId,PageId");
        var s_title = encodeURIComponent("跳转记录");

        var copyjydbh = encodeURIComponent(selected.PageId)   // 键值

        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");
        var sufproc = "";//encodeURIComponent("KPCalculate|$JYDBH$");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&js=" + js +
            "&callback=" + callback +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&type=leftrightlist" +
            "&LX=N" +
            "&sufproc=" + sufproc +
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
function edit() {
    try {
        var tabledesc = "页面跳转";
        var selected = pubselect();

        if (!selected) {
            return;
        }
        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD"); // zdzd名
        var tablename = encodeURIComponent("SysJumpSrcPage"); 			// 表名
        var tablerecid = encodeURIComponent("PageId"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("SysJumpDestPage");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("SourcePageId,PageId");
        var s_title = encodeURIComponent("跳转记录");

        var jydbh = encodeURIComponent(selected.PageId)   // 键值

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");
        var sufproc = "";//encodeURIComponent("KPCalculate|$JYDBH$");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&type=leftrightlist" +
            "&sufproc=" + sufproc +
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
function del() {
    try {
        var selected = pubselect();

        if (!selected) {
            return;
        }


        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeleteJumpPage",
            dataType: "json",
            data:"key="+encodeURIComponent(selected.PageId),
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