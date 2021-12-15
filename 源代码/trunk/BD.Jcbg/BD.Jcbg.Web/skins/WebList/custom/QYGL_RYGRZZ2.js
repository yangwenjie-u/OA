function add() {
    try {
        $.ajax({
            type: "POST",
            url: "/ry/getrybh",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    var rybh = data.msg;
                    var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
                    var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
                    var tablerecid = encodeURIComponent("RECID"); 	// 表主键 
                    var title = encodeURIComponent("人员证书"); 	// 标题
                    var buttons = encodeURIComponent("保存|TJ| "); // 按钮
                    var fieldparam = encodeURIComponent("I_S_RY_RYZZ,RYBH," + rybh);
                    var rdm = Math.random();

                    var js = encodeURIComponent("userService.js");
                    var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");

                    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                        "&t1_tablename=" + tablename +
                        "&t1_pri=" + tablerecid +
                        "&t1_title=" + title +
                        "&button=" + buttons +
                        "&rownum=2" +
                        "&fieldparam=" + fieldparam +
                        "&js=" + js +
                        "&callback=" + callback +
                        "&_=" + rdm;
                    parent.layer.open({
                        type: 2,
                        title: '录入证书信息',
                        shadeClose: false,
                        shade: 0.8,
                        area: ['95%', '95%'],
                        content: url,
                        end: function () {
                            searchRecord();
                        }
                    });
                } else {                    
                    alert("获取当前账号对应的人员信息失败，请联系管理员");
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
function copy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("资质证书"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_RY_RYZZ,RYBH," + selected.RYBH);
        var rdm = Math.random();
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");


        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&js=" + js +
            "&callback=" + callback +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '复制证书信息',
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
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");


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
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteisryryzs?recid=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    $.ajax({
                        type: "POST",
                        url: "/ry/updateuserrole?rybh=" + encodeURIComponent(selected.RYBH),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.msg != "") {
                                alert(data.msg);
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
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
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
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
            "&button=" +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&view=true"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看' + tabledesc,
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