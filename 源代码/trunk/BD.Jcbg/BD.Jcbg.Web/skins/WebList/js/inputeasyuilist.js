
//**** 公共函数 ****
//定义参数对象
function KeyParam() {
    this.key = "";
    this.value = "";
}

//.替换
function PointRep(str) {
    return str.replace(".", "\\.");
}


//分隔字符串
function StrSplit(str, sp) {
    //定义对象
    var keyParam = new KeyParam();
    if (str == "")
        return keyParam;
    //分组
    var strList = str.split(sp);
    if (strList.length == 1)
        return keyParam;
    //获取KEY与值
    keyParam.key = strList[0];
    keyParam.value = str.substr(keyParam.key.length + sp.length);
    return keyParam;
}


//复制对象
function clone(myObj) {
    if (typeof (myObj) != 'object' || myObj == null) return myObj;
    var newObj = new Object();
    for (var i in myObj) {
        newObj[i] = clone(myObj[i]);
    }
    return newObj;
}

//序列化
jQuery.prototype.serializeObject = function () {
    var obj = new Object();
    $.each(this.serializeArray(), function (index, param) {
        if (!(param.name in obj)) {
            obj[param.name] = param.value;
        }
    });
    return obj;
};

//**** 绑定事件 ****
function bindEvent() {
    //条件回车查询
    $(".searchPress").bind("keypress", function (event) {
        if (event.keyCode == "13") {
            searchRecord();
        }
    });
}



//初始化toolbar
function initToolbar(data) {
    var dataStr = "";
    dataStr += "<br/>";
    $(data).each(function (index) {
        dataStr += "<button type='button' class='btn btn-default stj_tab_btn_table' onclick='" + data[index].funname + "'>";
        dataStr += "    <i class='" + data[index].icon + "'>" + "</i>" + data[index].mc;
        dataStr += "</button>";
    });
    dataStr += "<br/><br/>";
    $("#toolmenu").append(dataStr);
    $.parser.parse($("#toolmenu"));
}

//初始化列表
function initTableForm(data) {
    //界面参数
    var listform = data.form;
    //初始化字段
    var fieldzdzd = data.zdzdList;
    var fieldStr = "";
    fieldStr += "[";
    //循环
    $(fieldzdzd).each(function (index) {
        fieldStr += "{";
        fieldStr += "   field: '" + fieldzdzd[index].zdname + "',";
        fieldStr += "   title: '" + fieldzdzd[index].zdsy + "',";
        fieldStr += "   sortable: true,";
        fieldStr += "   hidden: " + fieldzdzd[index].sfhidden + ",";
        fieldStr += "   width: " + fieldzdzd[index].zdwidth + ",";
        //编辑
        //判断控件类型
        if (fieldzdzd[index].kjlx == "SELECT") {
            //组装数据包
            //数据包值
            var datastr = "";
            //控件改变值
            var ctrlChange = "";
            if (fieldzdzd[index].ctrlstring != "") {
                //ctrlstring值
                var valueStr = "";
                //值对
                var keyParam;
                var typeList = fieldzdzd[index].ctrlstring.split("||");
                for (var iType = 0; iType < typeList.length; iType++) {
                    keyParam = StrSplit(typeList[iType], "--");
                    //值
                    if (keyParam.key == "value") {
                        valueStr = keyParam.value;
                    }
                    //控件改变
                    else if (keyParam.key == "ctrlChange") {
                        ctrlChange += "ctrlChange--" + keyParam.value + "||";
                    }
                }
                //修整ctrlChange
                //去掉功能项后面的||
                if (ctrlChange.length > 0 && ctrlChange.charAt(ctrlChange.length - 1) == '|' && ctrlChange.charAt(ctrlChange.length - 2) == '|')
                    ctrlChange = ctrlChange.substring(0, ctrlChange.length - 2);

                //判断是否有下拉值
                if (valueStr != "") {
                    //分析每一项
                    var items = valueStr.split("|");
                    var itemList;
                    //组成数据
                    datastr = "[";
                    for (var iItem = 0; iItem < items.length; iItem++) {
                        itemList = items[iItem].split(",");
                        datastr += "{";
                        if (itemList.length > 1)
                            datastr += "\"id\":\"" + itemList[1] + "\",";
                        else
                            datastr += "\"id\":\"" + itemList[0] + "\",";
                        datastr += "\"text\":\"" + itemList[0] + "\"";
                        datastr += "},";
                    }
                    //去掉最后一个逗号
                    if (datastr.length > 0 && datastr.charAt(datastr.length - 1) == ',')
                        datastr = datastr.substring(0, datastr.length - 1);
                    datastr += "]";
                }
            }

            fieldStr += "   editor: {";
            fieldStr += "       type:'combobox',";
            fieldStr += "       options:{";
            if (datastr != "")
                fieldStr += "           data:" + datastr + ",";
            if (fieldzdzd[index].mustin)
                fieldStr += "           required: true,";
            fieldStr += "           valueField: 'id',";
            fieldStr += "           textField: 'text',";
            fieldStr += "           editable: false,";
            //改变事件
            if (ctrlChange != "") {
                fieldStr += "           onChange: function(newValue,oldValue){";
                //判断如果有内容值改变时,才触发
                fieldStr += "               if(oldValue != \"\")";
                fieldStr += "                   changeOtherContrl(dataGrid,editIndex,\"" + ctrlChange + "\");";
                fieldStr += "           },";
            }
            fieldStr += "           panelHeight: 200"; //'auto'
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else if (fieldzdzd[index].kjlx == "COMBOBOX") {
            //组装数据包
            //数据包值
            var datastr = "";
            //控件改变值
            var ctrlChange = "";
            if (fieldzdzd[index].ctrlstring != "") {
                //ctrlstring值
                var valueStr = "";
                //值对
                var keyParam;
                var typeList = fieldzdzd[index].ctrlstring.split("||");
                for (var iType = 0; iType < typeList.length; iType++) {
                    keyParam = StrSplit(typeList[iType], "--");
                    //值
                    if (keyParam.key == "value") {
                        valueStr = keyParam.value;
                    }
                    //控件改变
                    else if (keyParam.key == "ctrlChange") {
                        ctrlChange += "ctrlChange--" + keyParam.value + "||";
                    }
                }
                //修整ctrlChange
                //去掉功能项后面的||
                if (ctrlChange.length > 0 && ctrlChange.charAt(ctrlChange.length - 1) == '|' && ctrlChange.charAt(ctrlChange.length - 2) == '|')
                    ctrlChange = ctrlChange.substring(0, ctrlChange.length - 2);

                //判断是否有下拉值
                if (valueStr != "") {
                    //分析每一项
                    var items = valueStr.split("|");
                    var itemList;
                    //组成数据
                    datastr = "[";
                    for (var iItem = 0; iItem < items.length; iItem++) {
                        itemList = items[iItem].split(",");
                        datastr += "{";
                        if (itemList.length > 1)
                            datastr += "\"id\":\"" + itemList[1] + "\",";
                        else
                            datastr += "\"id\":\"" + itemList[0] + "\",";
                        datastr += "\"text\":\"" + itemList[0] + "\"";
                        datastr += "},";
                    }
                    //去掉最后一个逗号
                    if (datastr.length > 0 && datastr.charAt(datastr.length - 1) == ',')
                        datastr = datastr.substring(0, datastr.length - 1);
                    datastr += "]";
                }
            }

            fieldStr += "   editor: {";
            fieldStr += "       type:'combobox',";
            fieldStr += "       options:{";
            if (datastr != "")
                fieldStr += "           data:" + datastr + ",";
            if (fieldzdzd[index].mustin)
                fieldStr += "           required: true,";
            fieldStr += "           valueField: 'id',";
            fieldStr += "           textField: 'text',";
            fieldStr += "           editable: true,";
            //改变事件
            if (ctrlChange != "") {
                fieldStr += "           onChange: function(newValue,oldValue){";
                //判断如果有内容值改变时,才触发
                fieldStr += "               if(oldValue != \"\")";
                fieldStr += "                   changeOtherContrl(dataGrid,editIndex,\"" + ctrlChange + "\");";
                fieldStr += "           },";
            }
            fieldStr += "           panelHeight: 200";
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else {
            fieldStr += "   editor: {";
            fieldStr += "       type:'textbox',"; //textbox，validatebox
            fieldStr += "       options:{";
            //fieldStr += "           required: true";
            fieldStr += "       }";
            fieldStr += "   },";
        }
//        fieldStr += "   styler: function(value,row,index){";
//        fieldStr += "       return 'vertical-align:middle;';";
//        fieldStr += "   },";
        fieldStr += "   align: 'left',";
        fieldStr += "   halign: 'center'";
        fieldStr += "},";

    });
    //去掉最后一个逗号
    if (fieldStr.length > 0 && fieldStr.charAt(fieldStr.length - 1) == ',')
        fieldStr = fieldStr.substring(0, fieldStr.length - 1);
    fieldStr += "]";
    //初始化过滤规则
    var filterStr = "";
    filterStr += "[";
    //循环
    $(fieldzdzd).each(function (index) {
        filterStr += "{";
        //                    if (fieldzdzd[index].kjlx == "") {
        //                        filterStr += "   type: 'numberbox',";
        //                        //filterStr += "   options:{precision:1},";
        //                        filterStr += "   op: ['equal', 'notequal', 'less', 'greater'],";
        //                    }
        //                    else {
        //                        filterStr += "   type: 'numberbox',";
        //                        //filterStr += "   options:{precision:1},";
        //                        filterStr += "   op: ['equal', 'notequal', 'less', 'greater'],";

        //                    }
        filterStr += "   type: 'combobox',";
        filterStr += "   options:{ ";
        filterStr += "       panelHeight:'auto',";
        filterStr += "       data:[{value:'',text:'全部'},{value:'1',text:'正常'},{value:'0',text:'禁用'}],";
        filterStr += "       onChange:function(value){";
        filterStr += "          if (value == ''){";
        filterStr += "               dataGrid.datagrid('removeFilterRule', '" + fieldzdzd[index].zdname + "');";
        filterStr += "          } else { ";
        filterStr += "              dataGrid.datagrid('addFilterRule', {";
        filterStr += "                  field: '" + fieldzdzd[index].zdname + "',";
        filterStr += "                  op: 'equal',";
        filterStr += "                  value: value ";
        filterStr += "              }); ";
        filterStr += "          }";
        filterStr += "          dataGrid.datagrid('doFilter');";
        filterStr += "       }";
        filterStr += "   },";
        filterStr += "   ";
        //                    filterStr += "   type: 'text',";
        //                    //filterStr += "   options:{precision:1},";
        //                    op: 'contains',
        //filterStr += "   op: ['contains','equal', 'notequal', 'less', 'greater'],";
        filterStr += "   field: '" + fieldzdzd[index].zdname + "',";
        filterStr += "},";
    });
    //去掉最后一个逗号
    if (filterStr.length > 0 && filterStr.charAt(filterStr.length - 1) == ',')
        filterStr = filterStr.substring(0, filterStr.length - 1);
    filterStr += "]";
    //初始列表
    dataGrid = $('#datagrid').datagrid({
        url: "/WebList/SearchInputEasyUiFormData",
        queryParams: {
            param: $("#param").val()
        },
        fit: true,
        pagination: true,
        rownumbers: true,
        remoteFilter: true,
        striped: true,
        singleSelect: listform.singleselect,
        pageList: eval(listform.limitsize),
        frozenColumns: [[
                        { field: 'ck', checkbox: true }
                    ]],
        columns: [eval(fieldStr)],
        onClickCell: onClickCell,
        onBeforeLoad: function (param) {
            editIndex = undefined;
        },
        toolbar: '#toolbar'
    });
    var p = dataGrid.datagrid('getPager');
    $(p).pagination({
        displayMsg: '当前【 {from} 至 {to} 】 共【 {total} 】条记录',
        onBeforeRefresh: function (pageNumber, pageSize) {
            $(this).pagination('loading');
            $(this).pagination('loaded');
        }
    });
    //过滤规则
    dataGrid.datagrid('enableFilter', []);
}

//事件
//列表点击事件
function onClickCell(index, field) {
    if ((editIndex != index && editIndex != undefined) || dataGrid.datagrid('getChanges').length > 0) {
        $.messager.confirm('询问', '记录还处于编辑状态，是否撤销编辑信息?', function (r) {
            if (r) {
                dataGrid.datagrid('rejectChanges');
                editIndex = undefined;
            }
        });
    }
}

//查询
function searchRecord() {
    dataGrid.datagrid('reload');
}

//新建
function addRecord() {
    try {
        //判断是否还在编辑状态
        if (editIndex != undefined) {
            $.messager.alert("提示", "记录还处于编辑状态！", "warning");
            return;
        }
        var newRow = {};
        $.extend(newRow, defRow);
        dataGrid.datagrid("insertRow", {
            index: 0,
            row: newRow
        });
        //设置编辑行
        editIndex = 0;
        dataGrid.datagrid('selectRow').datagrid('beginEdit', editIndex);
        var editors = dataGrid.datagrid('getEditors', editIndex);
    }
    catch (e) {
        alert("addRecord出错，原因：" + e.Message);
    }
}

//修改
function modifyRecord() {
    try {
        //判断是否还在编辑状态
        if (editIndex != undefined) {
            $.messager.alert("提示", "记录还处于编辑状态！", "warning");
            return;
        }
        //判断是否已经选择
        var rows = dataGrid.datagrid('getSelected');
        if (!rows || rows.length == 0) {
            $.messager.alert("提示", "请先择需要操作的记录！", "warning");
            return;
        }
        //设置编辑行
        editIndex = dataGrid.datagrid('getRowIndex', rows)
        dataGrid.datagrid('selectRow').datagrid('beginEdit', editIndex);
        //var editors = dataGrid.datagrid('getEditors', editIndex);
    }
    catch (e) {
        alert("modifyRecord出错，原因：" + e.Message);
    }
}

//保存
function saveRecord() {
    try {
        //判断是否还在编辑状态 
        if (editIndex == undefined && dataGrid.datagrid('getChanges').length == 0) {
            $.messager.alert("提示", "记录还处于编辑状态或无修改记录！", "warning");
            return;
        }

        if (editIndex != undefined && !dataGrid.datagrid('validateRow', editIndex)) {
            $.messager.alert("提示", "请输入必输项！", "warning");
            return;
        }
        //结束编辑
        if (editIndex != undefined) {
            dataGrid.datagrid('endEdit', editIndex);
            editIndex = undefined;
        }
        //判断是否有提交数据
        var rows = dataGrid.datagrid('getChanges');
        //判断是否有提交数据
        if (rows.length == 0) {
            $.messager.alert("提示", "没有需要保存的数据！", "warning");
            return;
        }
        //组装数据
        var data = "{";
        //循环遍历
        for (var i = 0; i < rows.length; i++) {
            jQuery.each(rows[i], function (key, val) {
                data = data + "\"" + key + "\":\"" + val + "\",";
            });
        }
        //去掉最后一个字符|
        if (data.length > 0 && data.charAt(data.length - 1) == ',')
            data = data.substring(0, data.length - 1);
        data += "}";

        $.ajax({
            type: "POST",
            url: "/WebList/SaveEasyUIData",
            dataType: "json",
            data: { param: $("#param").val(), jsonData: data },
            success: function (val) {
                //遮罩
                layer.closeAll("loading");
                if (!val.success) {
                    alert(val.msg);
                    return;
                }
                $.messager.alert('提示', val.msg, 'info', function () {
                    //重新加载数据
                    searchRecord();
                });
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
                //遮罩
                layer.load();
            }
        });
    }
    catch (e) {
        alert("saveRecord出错，原因：" + e.Message);
    }
}

//删除
function delRecord() {
    var rows = dataGrid.datagrid('getSelected');
    if (!rows || rows.length == 0) {
        $.messager.alert("提示", "请先择需要操作的记录！", "warning");
        return;
    }
    $.messager.confirm('提示', '是否删除选中数据?', function (r) {
        if (!r) {
            return;
        }
        //组装数据
        var data = "{";
        //循环遍历
        jQuery.each(rows, function (key, val) {
            data = data + "\"" + key + "\":\"" + val + "\",";
        });
        //去掉最后一个字符|
        if (data.length > 0 && data.charAt(data.length - 1) == ',')
            data = data.substring(0, data.length - 1);
        data += "}";
        $.ajax({
            type: "POST",
            url: "/WebList/DelEasyUIData",
            dataType: "json",
            data: { param: $("#param").val(), jsonData: data },
            success: function (val) {
                //遮罩
                layer.closeAll("loading");
                if (!val.success) {
                    alert(val.msg);
                    return;
                }
                $.messager.alert('提示', val.msg, 'info', function () {
                    //重新加载数据
                    searchRecord();
                });
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
                //遮罩
                layer.load();
            }
        });
    });
}

//界面根据ID获取控件值
function GetListControlText(dataGrid, editIndex, name) {
    var val = "";
    var cellEditor = dataGrid.datagrid('getEditor', { index: editIndex, field: name });
    if (cellEditor.type == "combobox") {
        var dataList = cellEditor.target.combobox('getText');
        if (dataList instanceof Array)
            val = dataList[0];
        else
            val = dataList;
        return val;
    }
    //验证框
    else if (cellEditor.type == "validatebox") {
        val = cellEditor.target.val();
        return val;
    }
    //文本
    else if (cellEditor.type == "textbox") {
        val = cellEditor.target.val();
        return val;
    }
    else {
        alert("未启用控件:" + cellEditor.type);
    }
}

//界面根据ID获取控件值
function GetListControlValue(dataGrid, editIndex, name) {
    var val = "";
    var cellEditor = dataGrid.datagrid('getEditor', { index: editIndex, field: name });
    if (cellEditor.type == "combobox") {
        var dataList = cellEditor.target.combobox('getValue');
        if (dataList instanceof Array)
            val = dataList[0];
        else
            val = dataList;
        return val;
    }
    //文本,验证框
    else if (cellEditor.type == "validatebox") {
        val = cellEditor.target.val();
        return val;
    }
    else {
        alert("未启用控件:" + cellEditor.type);
    }
}
//界面根据ID获取控件类型
function GetListControlType(dataGrid, editIndex, name) {
    var cellEditor = dataGrid.datagrid('getEditor', { index: editIndex, field: name });
    return cellEditor.type;
}
//界面根据ID获取对象控件
function GetListControl(dataGrid, editIndex, name) {
    return dataGrid.datagrid('getEditor', { index: editIndex, field: name });
}
//改变另外控件
function changeOtherContrl(dataGrid, editIndex, changeStr) {
    //如果没有改变字符串
    if (changeStr == "")
        return;
    //分析
    //值对
    var keyParam;
    var typeList = changeStr.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        if (keyParam.key == "ctrlChange") {
            var ctrlstring = "ctrlChange--";
            //分析每一项
            var items = keyParam.value.split("|");
            for (var iItem = 0; iItem < items.length; iItem++) {
                keyParam = StrSplit(items[iItem], "-");
                if (keyParam.key == "wherectrl") {
                    ctrlstring += keyParam.key + '-'
                    var ctrlList = keyParam.value.split(",");
                    for (var iCtrlList = 0; iCtrlList < ctrlList.length; iCtrlList++) {
                        ctrlstring += GetListControlValue(dataGrid, editIndex, ctrlList[iCtrlList]) + ",";
                    }
                    //去掉最后一个,
                    if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == ',')
                        ctrlstring = ctrlstring.substring(0, ctrlstring.length - 1);
                    ctrlstring += "|";
                }
                //获取目标控件
                else if (keyParam.key == "targetctrl") {
                    targetctrl = keyParam.value;
                    ctrlstring += items[iItem] + "|";
                }
                else {
                    ctrlstring += items[iItem] + "|";
                }
            }
            //去掉最后一个字符|
            if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|')
                ctrlstring = ctrlstring.substring(0, ctrlstring.length - 1);

            //请求数据
            //获取权限分组列表
            $.ajax({
                type: "POST",
                url: "/WebList/CtrlStringDataEasyUI",
                dataType: "json",
                async: false,
                data: { ctrlString: ctrlstring },
                success: function (val) {
                    if (val.success) {
                        //获取目标控件
                        var ctrlObj = GetListControl(dataGrid, editIndex, targetctrl);
                        //判断控件类型
                        if (ctrlObj.type == "combobox") {
                            //ctrlObj.target.combobox("clear");
                            ctrlObj.target.combobox("loadData", val.data.rows);
                        }
                        //文本验证框
                        else if (ctrlObj.type == "validatebox") {
                            //$("#" + Rep(arrDCols[i])).val(val.msg[0].content);
                        }
                    }
                    else {
                        alert(val.msg);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("访问出错");
                }
            });
        }
        //结束循环ctrlstring  
    }
}