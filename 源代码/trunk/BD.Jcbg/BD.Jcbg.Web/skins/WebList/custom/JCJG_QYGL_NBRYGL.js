function add() {
    try {
        var tabledesc = "管理人员";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_NBRY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService.js");
        var callback = encodeURIComponent("userService.createUser('n','$$RYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
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
        var tabledesc = "管理人员";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_NBRY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var js = encodeURIComponent("userService.js,idcardService.js,irisService.js");
        var callback = encodeURIComponent("userService.createUser('n','$$RYBH$$')");
        var fieldparam = "I_M_NBRY,ZH,";
        fieldparam = encodeURIComponent(fieldparam);
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&fieldparam=" + fieldparam +
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
        var tabledesc = "管理人员";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_NBRY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var js = encodeURIComponent("idcardService.js,irisService.js,userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");
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
            "&js=" + js +
	    "&callback=" + callback +
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
        var tabledesc = "管理人员";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteimnbry?rybh=" + encodeURIComponent(selected.RYBH),
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