function add() {
    try {
        var tabledesc = "检测设备";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
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
        var tabledesc = "检测设备";   
        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}
        // var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            return;
        }

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
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
        var tabledesc = "检测设备";   

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
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
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
            //"&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&_=" + rdm;
       

        $.ajax({
            type: "POST",
            url: "/zhwx/getpower?recid=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.success) {
                    url += "&button=" + buttons;
                } 

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


            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        })




        
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var tabledesc = "检测设备";   

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

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteimsb?recid=" + encodeURIComponent(selected.RECID),
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


//生成二维码
function createvcode() {
    try {
        var tabledesc = "检测设备";

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}
       // var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            return;
        }

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/ReportPrint/Index?filename=SBEWM&table=zhwx_sbewm&where=id="+jydbh+"&opentype=print";


        $.ajax({
            type: "POST",
            url: "/zhwx/createidentifycode?equipid=" + encodeURIComponent(jydbh),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.success) {
                    parent.layer.open({
                        type: 2,
                        title: '生成二维码' + tabledesc + "(" + jydbh + ")",
                        shadeClose: false,
                        shade: 0.8,
                        area: ['95%', '95%'],
                        content: url
                    });
                } else {
                    alert("二维码生成失败！");
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



 //导出到excel
function exportexcel() {
    try {

        var url = "/zhwx/exportexcel";

        parent.layer.open({
            type: 2,
            title: '导出到Excel',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });



    } catch (e) {
        alert(e);
    }
}



//检测设备管理台账导出
function celiangexcel() {
    try {

        var url = "/zhwx/celiangexcel";

        parent.layer.open({
            type: 2,
            title: '检测设备管理台账导出到Excel',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });



    } catch (e) {
        alert(e);
    }
}

//周期溯源计划以及实施记录表
function suyuanexcel() {
    try {

        var url = "/zhwx/suyuanexcel";

        parent.layer.open({
            type: 2,
            title: '周期溯源计划以及实施记录表导出到Excel',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });



    } catch (e) {
        alert(e);
    }
}
