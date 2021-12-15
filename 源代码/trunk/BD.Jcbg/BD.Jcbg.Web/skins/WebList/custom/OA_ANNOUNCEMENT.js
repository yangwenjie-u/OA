function add() {
    try {
        var tabledesc = "常用短信";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("OA_SMS_CYDX"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");  // 按钮

        var rdm = Math.random();

        var js = "";
        var callback = "";

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
            title: '新增' + tabledesc,
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
        var tabledesc = "常用短信";

        var selected = pubselect();
        if (selected == undefined)
            return;


        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("OA_SMS_CYDX"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.RECID)   // 键值

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
            "&jydbh=" + jydbh +
            "&LX=N" +
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


function deleteDX() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var id = selected.RECID;
        $.ajax({
            type: "POST",
            url: "/delete/deletetable?ID=" + encodeURIComponent(id) + "&name=RECID&table=OA_SMS_CYDX",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    parent.layer.alert('删除成功！', {
                        icon: 0,
                        skin: 'layer-ext-moon'
                    });
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
                    layer.alert(data.msg, {
                        icon: 0,
                        skin: 'layer-ext-moon'
                    });
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