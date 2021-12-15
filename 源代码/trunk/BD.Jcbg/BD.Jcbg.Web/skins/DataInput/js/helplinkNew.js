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
        url: "/DataInput/HelplinkData",
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
    //SELECT
    if (kjlx == "SELECT") {
        if (value instanceof Array) {
            if (value.length == 0)
                parent.$("#" + name).val("");
            else
                parent.$("#" + name).val(value[0].value);
        }
        else
            parent.$("#" + name).val(value);
    }
    else if (kjlx == "CHECKBOX" || kjlx == "C") {
        //清除原来选择的内容
        //$('input[name="' + name + '"]').removeAttr("checked"); //取消全选  
        if (value instanceof Array) {
            parent.$("input[name='" + zdmc + "']").each(function () {
                if ($.inArray($(this).val(), value) == -1)
                    $(this).attr("checked", false);
                else
                    $(this).attr("checked", true);
            });
        }
        else {
            if (value.split(",").length > 1) {
                value = value.split(",");
                parent.$("input[name='" + name + "']").each(function () {
                    if ($.inArray($(this).val(), value) == -1)
                        $(this).attr("checked", false);
                    else
                        $(this).attr("checked", true);
                });
            }
            else {
                //遍历选中
                parent.$('input[name="' + name + '"]').each(function () {
                    if ($(this).val() == value)
                        $(this).attr("checked", true)
                    else
                        $(this).attr("checked", false)
                });
            }
        }
    }
    else if (kjlx == "RADIO" || kjlx == "R") {
        if (value instanceof Array) {
            if (value.length != 0) {
                parent.$("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value[0].value)
                        $(this).attr("checked", "checked");
                });
            }
        }
        else {
            if (value.split(",").length > 1) {
                parent.$("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value[0])
                        $(this).attr("checked", "checked");
                });
            }
            else {
                parent.$("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value)
                        $(this).attr("checked", "checked");
                });
            }

        }
    }
}
