function add() {
    try {
        var tabledesc = "短信通知名单";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("MessageInfoList"); 			// 表名
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
        var tabledesc = "短信通知名单";
        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            return;
        }

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("MessageInfoList"); 			// 表名
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
        var tabledesc = "短信通知名单";

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);

     //   var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        var selecteds = pubselects();
        if (!selecteds) {
            return;
        }

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        var recids = "";
        for (i = 0; i < selecteds.length; i++) {
            recids += selecteds[i].RECID+",";
        }

        $.ajax({
            type: "POST",
            url: "/delete/deleteidxtzmd?recids=" + encodeURIComponent(recids),
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