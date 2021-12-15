//.替换
function PointRep(str) {
    return str.replace(".", "\\.");
}


//**** 查询 ****
function searchRecord() {
    loadData(1);
}

//**** 数据包 ****
//查询数据包
function loadData(page) {
    //获取参数
    var col = $.trim($("#dlg_search").val());
    var val = $.trim($("#edt_search").val());

    var helplink = $.trim($("#helplink").val());
    //设置分页参数
    var rows = dataGrid.bootstrapTable('getOptions').pageSize;
    var sort = dataGrid.bootstrapTable('getOptions').sortName;
    var order = dataGrid.bootstrapTable('getOptions').sortOrder;
    //请求数据
    $.ajax({
        type: "post",
        url: "/WebList/HelplinkData",
        dataType: "json",
        data: { col: col, val: val, helplink: helplink, page: page, rows: rows, sort: sort, order: order },
        async: true,
        success: function (jsonData) {
            if (!jsonData.success) {
                //$.messager.alert("错误", jsonData.msg, 'error');
                return;
            }
            //刷新
            dataGrid.bootstrapTable('resetView');
            //加载数据
            dataGrid.bootstrapTable("load", jsonData.data);
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}



//******** 控件信息 ******
//获取控件类型
function GetCtrlType(name) {
    return parent.$("#" + PointRep(name)).attr("kjlx");
}

//设置控件值
function SetCtrlValue(name, value) {
    var kjlx = GetCtrlType(name);
    name = PointRep(name);
    //****判断控件*****
    //****HTML****
    //TEXT
    if (kjlx == "TEXT") {
        if (value instanceof Array) {
            if (value.length == 0)
                parent.$("#" + name).val("");
            else
                parent.$("#" + name).val(value[0].value);
        }
        else
            parent.$("#" + name).val(value);
    }
    else {
        if (value instanceof Array)
            parent.$("#" + name).val(value[0].value);
        else
            parent.$("#" + name).val(value);
    }
}


