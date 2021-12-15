function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_GC_Monitor"); // 表名
        var tablerecid = encodeURIComponent("Id"); // 表主键
        var title = encodeURIComponent("绑定设备"); // 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&preproc=Sp_SaveGcMonitorInfo|$I_S_GC_Monitor.DeviceID|$I_S_GC_Monitor.TypeCode" +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '绑定设备',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function() {
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
        var tablename = encodeURIComponent("I_S_GC_Monitor"); // 表名
        var tablerecid = encodeURIComponent("Id"); // 表主键
        var title = encodeURIComponent("更换设备"); // 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.Id); // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&preproc=Sp_UpdateGcMonitorInfo|$JYDBH$|$I_S_GC_Dust.DeviceID|$I_S_GC_Dust.TypeCode" +
            "&rownum=2" +
            "&LX=E" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '更换设备',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function() {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function selectGc() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    top.layer.tab({
        area: ['95%', '95%'],
        tab: [{
            title: "统计图",
            content: "<iframe id='iframe2' width='980px' height='540px' frameborder='0' src='/DwgxWzOh/DeviceChart?deviceCode=" + selected.DeviceCode + "&gcmc=" + selected.GCMC+ "'></iframe>"

            // content: "<iframe id='iframe2' width='980px' height='540px' frameborder='0' src='/DwgxWzOh/dindex?deviceCode=" + selected.DeviceCode + "'></iframe>"
        }, {
            title: "统计表",
            content: "<iframe id='iframe2' width='980px' height='540px' frameborder='0' src='/DwgxWzOh/PageDustList?deviceCode=" + selected.DeviceCode + "'></iframe>"

            // content: "<iframe id='iframe2' width='980px' height='540px' frameborder='0' src='/WebList/EasyUiIndex?FormDm=I_M_Dust&FormStatus=1&FormParam=PARAM--" + selected.DeviceCode + "'></iframe>"
        }]
    });
}




function UpdateIOT() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;


        var RECID = selected.Id;
        $.ajax({
            type: "POST",
            url: "/DwgxWzOh/AddOrUpdateVideo?id=" + encodeURIComponent(RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("同步成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "同步失败";
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


function DeleteIOT() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        var RECID = selected.Id;
        $.ajax({
            type: "POST",
            url: "/DwgxWzOh/DeleteVideoDevice?id=" + encodeURIComponent(RECID),
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
                searchRecord();
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}


function zjlist() {

        try {
            var tabledesc = "在建工程扬尘设备";
            /*
            var rowindex = dataGrid.jqxGrid('getselectedrowindex');
        
            if (rowindex == -1) {
                alert("请选择要操作的" + tabledesc);
                return;
            }*/

            var url = "/WebList/EasyUiIndex?FormDm=I_S_GC_Dust&FormStatus=2";
            parent.layer.open({
                type: 2,
                title: '查看' + tabledesc,
                shadeClose: true,
                shade: 0.8,
                area: ['100%', '100%'],
                content: url,
                end: function () {
                    searchRecord();
                }
            });
        } catch (e) {
            alert(e);
        }

}