//数据加载成功后,自动判断调用
function CheckCustomLoadSuccessFun() {
    var ret = false;
    if ((typeof CustomLoadSuccessFun) === "function")
        ret = CustomLoadSuccessFun();
    else
        ret = true;
    return ret;
}

////编码
//function Base64Str(str) {
//    return btoa(str);
//}

////解码
//function Base64DecodeStr(str) {
//    return atob(str);
//}

//扩展
Array.prototype.removeByValue = function(val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            this.splice(i, 1);
            break;
        }
    }
}

//日期扩展
Date.prototype.Format = function(fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1, //月份   
        "d+": this.getDate(), //日   
        "h+": this.getHours(), //小时   
        "m+": this.getMinutes(), //分   
        "s+": this.getSeconds(), //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds() //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//************* datebox 过滤控件 自定义按钮 *********************
function getCustomizedButtons(field) {
    var buttons = $.extend([], $.fn.datebox.defaults.buttons);
    var handler1 = buttons[0].handler;
    buttons.splice(0, 1, {
        text: function(target) {
            return $(target).datebox("options").customizedCurrentText;
        },
        handler: function(target) {
            handler1.call($.fn.datebox.defaults, target);
            var v = $(target).datebox('getValue');
            var op = 'equal';
            var rule = dataGrid.datagrid('getFilterRule', field);
            if (rule != null) { op = rule.op; };
            dataGrid.datagrid('addFilterRule', {
                field: field,
                op: op,
                value: v
            });
            dataGrid.datagrid('doFilter');
        }
    });
    return buttons;
}


//**** 全局变量 ****
var conditionNum = 4;

//定义函数的数组字段
var hiddenZdzdField = new Array();
var hiddenZdzdMc = new Array();
//定义当前已选择的字段
var chooseZdzdField = new Array();

//********************************//
var cx_btn = false;
var cx_table;
//********************************//

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
    if (typeof(myObj) != 'object' || myObj == null) return myObj;
    var newObj = new Object();
    for (var i in myObj) {
        newObj[i] = clone(myObj[i]);
    }
    return newObj;
}

//序列化
jQuery.prototype.serializeObject = function() {
    var obj = new Object();
    $.each(this.serializeArray(), function(index, param) {
        if (!(param.name in obj)) {
            obj[param.name] = param.value;
        }
    });
    return obj;
};

//**** 绑定事件 ****
function bindEvent() {
    //条件回车查询
    $(".searchPress").bind("keypress", function(event) {
        if (event.keyCode == "13") {
            searchRecord();
        }
    });
}



//初始化列表
function initTableForm(data) {
    //界面参数
    var listform = data.form;
    //初始化字段
    var fieldzdzd = data.zdzdList;
    //字段字符串
    var fieldStr = "";
    //过滤默认值字符串
    var defaultFilterStr = "";
    defaultFilterStr += "[";

    fieldStr += "[";
    //循环
    $(fieldzdzd).each(function(index) {
        //判断是否为普通列
        if (fieldzdzd[index].sfgd)
            return true;
        //过滤字段默认值
        if (fieldzdzd[index].filterDef != "") {
            defaultFilterStr += "{";
            defaultFilterStr += "   field: '" + fieldzdzd[index].zdname + "',";
            defaultFilterStr += "   op: '" + fieldzdzd[index].filterOpt + "',";
            defaultFilterStr += "   value: '" + fieldzdzd[index].filterDef + "'";
            defaultFilterStr += "},";
        }
        //显示字段
        fieldStr += "{";
        fieldStr += "   field: '" + fieldzdzd[index].zdname + "',";
        fieldStr += "   title: '" + fieldzdzd[index].zdsy + "',";
        fieldStr += "   sortable: " + fieldzdzd[index].sfpx + ",";
        fieldStr += "   hidden: " + fieldzdzd[index].sfhidden + ",";
        fieldStr += "   width: " + fieldzdzd[index].zdwidth + ",";
        //列颜色样式
        if (fieldzdzd[index].zdcolor != "")
            fieldStr += "   styler: " + fieldzdzd[index].zdcolor + ",";
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
        } else if (fieldzdzd[index].kjlx == "COMBOBOX") {
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
        }
        //格式化function(value,row,index)
        if (fieldzdzd[index].formatevent != "") {
            fieldStr += "   formatter:" + fieldzdzd[index].formatevent + ",";
        }
        fieldStr += "   align: '" + fieldzdzd[index].align + "',";
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
    $(fieldzdzd).each(function(index) {
        filterStr += "{";
        //帮助列表
        if (fieldzdzd[index].filtertype == "bdgrid") {
            filterStr += "   type: 'bdgrid',";
            filterStr += "   op: ['contains'],";
            filterStr += "   options: {";
            filterStr += "      helplink: '" + fieldzdzd[index].helplink + "'"
            filterStr += "   },";
        }
        else if (fieldzdzd[index].filtertype == "bdgrid2") {
            filterStr += "   type: 'bdgrid2',";
            filterStr += "   op: ['contains'],";
            filterStr += "   options: {";
            filterStr += "          editable:false,";
            filterStr += "          config: getCustomizedBdGridConfig('" + Base64Str(JSON.stringify(fieldzdzd[index])) + "')";
            filterStr += "   },";
        }
        //日期范围
        else if (fieldzdzd[index].filtertype == "dateRange") {
            filterStr += "   type: 'dateRange',";
            filterStr += "   op: ['range'],";
            filterStr += "   options: { ";
            //filterStr += "            editable:false,";
            filterStr += "          buttons: getCustomizedButtons('" + fieldzdzd[index].zdname + "'),";
            filterStr += "   },";
        }
        //日期范围
        else if (fieldzdzd[index].filtertype == "dateRange1") {
            filterStr += "   type: 'dateRange1',";
            filterStr += "   op: ['range'],";
            filterStr += "   options: { ";
            //filterStr += "            editable:false,";
            filterStr += "          buttons: getCustomizedButtons('" + fieldzdzd[index].zdname + "'),";
            filterStr += "   },";
        }
        //日期范围
        else if (fieldzdzd[index].filtertype == "dateRange2") {
            filterStr += "   type: 'dateRange2',";
            filterStr += "   op: ['range'],";
            filterStr += "   options: { ";
            filterStr += "          editable:false,";
            filterStr += "          buttons: getCustomizedDateRangeButtons('" + fieldzdzd[index].zdname + "'),";
            filterStr += "   },";
        }
        //日期
        else if (fieldzdzd[index].filtertype == "date") {
            filterStr += "   type: 'datebox',";
            filterStr += "   op: ['equal', 'notequal', 'less', 'lessorequal', 'greater', 'greaterorequal'],";
            filterStr += "   options: { ";
            filterStr += "          editable:false,";
            filterStr += "          buttons: getCustomizedButtons('" + fieldzdzd[index].zdname + "'),";
            filterStr += "          onSelect: function(date){       ";
            filterStr += "              var v = date.Format('yyyy-MM-dd');";
            filterStr += "              var op = 'equal';";
            filterStr += "              var rule = dataGrid.datagrid('getFilterRule', '" + fieldzdzd[index].zdname + "');";
            filterStr += "              if(rule!=null){ op = rule.op;};";
            filterStr += "              dataGrid.datagrid('addFilterRule', {";
            filterStr += "                  field: '" + fieldzdzd[index].zdname + "',";
            filterStr += "                  op: op,";
            filterStr += "                  value: v";
            filterStr += "              });";
            filterStr += "              dataGrid.datagrid('doFilter');";
            filterStr += "          }";
            filterStr += "   },";

        }
        //日期
        else if (fieldzdzd[index].filtertype == "yyyy") {
            filterStr += "   type: 'datebox',";
            filterStr += "   options:{ ";
            filterStr += "      formatter:myyyyyformatter,";
            filterStr += "      parser:myparser";
            filterStr += "   },";
            filterStr += "   op: ['equal', 'notequal', 'less', 'lessorequal', 'greater', 'greaterorequal'],";
        }
        //日期
        else if (fieldzdzd[index].filtertype == "yyyymm") {
            filterStr += "   type: 'datebox',";
            filterStr += "   options:{ ";
            filterStr += "      formatter:myyyyymmformatter,";
            filterStr += "      parser:myparser";
            filterStr += "   },";
            filterStr += "   op: ['equal', 'notequal', 'less', 'lessorequal', 'greater', 'greaterorequal'],";
        }
        //数值
        else if (fieldzdzd[index].filtertype == "number") {
            filterStr += "   type: 'numberbox',";
            filterStr += "   op: ['equal', 'notequal', 'less', 'lessorequal', 'greater', 'greaterorequal'],";
        }
        //下拉框
        else if (fieldzdzd[index].filtertype == "list") {
            filterStr += "   type: 'combobox',";
            filterStr += "   options:{ ";
            filterStr += "       panelHeight:200,";
            //下拉框值
            var valueStr = "";
            //值对
            var keyParam;
            var typeList = fieldzdzd[index].datatype.split("||");
            for (var i = 0; i < typeList.length; i++) {
                keyParam = StrSplit(typeList[i], "--");
                //固定值
                if (keyParam.key == "value") {
                    valueStr = keyParam.value;
                }
            }
            //filterStr += "       data:[{value:'',text:'全部'},{value:'1',text:'正常'},{value:'0',text:'禁用'}],";
            filterStr += "       data:[";
            //分析每一项
            var items = valueStr.split("|");
            var itemList;
            for (var iItem = 0; iItem < items.length; iItem++) {
                itemList = items[iItem].split(",");
                filterStr += "      {";
                filterStr += "          value:'" + itemList[1] + "',";
                filterStr += "          text:'" + itemList[0] + "'";
                filterStr += "      },";
            }
            //去掉最后一个,
            if (filterStr.length > 0 && filterStr.charAt(filterStr.length - 1) == ',')
                filterStr = filterStr.substring(0, filterStr.length - 1)
            filterStr += "       ],";

            filterStr += "       onChange:function(value){";
            filterStr += "          if (value == ''){";
            filterStr += "               dataGrid.datagrid('removeFilterRule', '" + fieldzdzd[index].zdname + "');";
            filterStr += "          } else { ";
            filterStr += "              dataGrid.datagrid('addFilterRule', {";
            filterStr += "                  field: '" + fieldzdzd[index].zdname + "',";
            //过程操作符
            if (fieldzdzd[index].filterOpt != "")
                filterStr += "                  op: '" + fieldzdzd[index].filterOpt + "',";
            else
                filterStr += "                  op: 'equal',";
            filterStr += "                  value: value ";
            filterStr += "              }); ";
            filterStr += "          }";
            filterStr += "          dataGrid.datagrid('doFilter');";
            filterStr += "       }";
            filterStr += "   },";
            filterStr += "   op: ['equal', 'notequal', 'contains'],";
        }
        //默认空或文本
        else if (fieldzdzd[index].filtertype == "input" || fieldzdzd[index].filtertype == "") {
            filterStr += "   type: 'text',";
            filterStr += "   op: ['equal','contains','notequal', 'beginwith', 'endwith'],"
        }
        //其他
        else {
            filterStr += "   type: 'text',";
            filterStr += "   op: ['equal','contains','notequal', 'beginwith', 'endwith'],"
        }


        //        filterStr += "   type: 'combobox',";
        //        filterStr += "   options:{ ";
        //        filterStr += "       panelHeight:'auto',";
        //        filterStr += "       data:[{value:'',text:'全部'},{value:'1',text:'正常'},{value:'0',text:'禁用'}],";
        //        filterStr += "       onChange:function(value){";
        //        filterStr += "          if (value == ''){";
        //        filterStr += "               dataGrid.datagrid('removeFilterRule', '" + fieldzdzd[index].zdname + "');";
        //        filterStr += "          } else { ";
        //        filterStr += "              dataGrid.datagrid('addFilterRule', {";
        //        filterStr += "                  field: '" + fieldzdzd[index].zdname + "',";
        //        filterStr += "                  op: 'equal',";
        //        filterStr += "                  value: value ";
        //        filterStr += "              }); ";
        //        filterStr += "          }";
        //        filterStr += "          dataGrid.datagrid('doFilter');";
        //        filterStr += "       }";
        //        filterStr += "   },";
        filterStr += "   ";
        //                    filterStr += "   type: 'text',";
        //                    //filterStr += "   options:{precision:1},";
        //                    op: 'contains',
        //filterStr += "   op: ['contains','equal', 'notequal', 'less', 'greater'],";
        filterStr += "   field: '" + fieldzdzd[index].zdname + "'";
        filterStr += "},";
    });
    //去掉最后一个逗号
    if (filterStr.length > 0 && filterStr.charAt(filterStr.length - 1) == ',')
        filterStr = filterStr.substring(0, filterStr.length - 1);
    filterStr += "]";



    //固定列
    var gdColumnStr = "[";
    if (listform.checkbox) {
        gdColumnStr += "{ field: 'ck', checkbox: true },";
    }
    //**************固定列*************************
    //循环
    $(fieldzdzd).each(function(index) {
        //判断是否为普通列
        if (!fieldzdzd[index].sfgd)
            return true;
        //过滤字段默认值
        if (fieldzdzd[index].filterDef != "") {
            defaultFilterStr += "{";
            defaultFilterStr += "   field: '" + fieldzdzd[index].zdname + "',";
            defaultFilterStr += "   op: '" + fieldzdzd[index].filterOpt + "',";
            defaultFilterStr += "   value: '" + fieldzdzd[index].filterDef + "'";
            defaultFilterStr += "},";
        }
        //显示字段
        gdColumnStr += "{";
        gdColumnStr += "   field: '" + fieldzdzd[index].zdname + "',";
        gdColumnStr += "   title: '" + fieldzdzd[index].zdsy + "',";
        gdColumnStr += "   sortable: " + fieldzdzd[index].sfpx + ",";
        gdColumnStr += "   hidden: " + fieldzdzd[index].sfhidden + ",";
        gdColumnStr += "   width: " + fieldzdzd[index].zdwidth + ",";
        //列颜色样式
        if (fieldzdzd[index].zdcolor != "")
            gdColumnStr += "   styler: " + fieldzdzd[index].zdcolor + ",";
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
        } else if (fieldzdzd[index].kjlx == "COMBOBOX") {
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
        }
        //格式化function(value,row,index)
        if (fieldzdzd[index].formatevent != "") {
            gdColumnStr += "   formatter:" + fieldzdzd[index].formatevent + ",";
        }
        gdColumnStr += "   align: '" + fieldzdzd[index].align + "',";
        gdColumnStr += "   halign: 'center'";
        gdColumnStr += "},";

    });
    //*********************************************
    //去掉最后一个逗号
    if (gdColumnStr.length > 0 && gdColumnStr.charAt(gdColumnStr.length - 1) == ',')
        gdColumnStr = gdColumnStr.substring(0, gdColumnStr.length - 1);
    gdColumnStr += "]";


    //过滤默认值
    if (defaultFilterStr.length > 0 && defaultFilterStr.charAt(defaultFilterStr.length - 1) == ',')
        defaultFilterStr = defaultFilterStr.substring(0, defaultFilterStr.length - 1);
    defaultFilterStr += "]";

    //初始列表
    dataGrid = $('#datagrid').datagrid({
        //url: "/WebList/SearchEasyUiFormData",
        //        queryParams: {
        //            param: $("#param").val(),
        //            $("#conditionform").serialize()
        //        },
        //queryParams: $.extend(true, { param: $("#param").val() }, $("#conditionform").serializeObject()),
        queryParams: { param: $("#param").val() },
        fit: true,
        pagination: true,
        rownumbers: true,
        remoteFilter: true,
        nowrap: gridwrap == "1" ? false : true,
        striped: true,
        showFooter: listform.summaryheight == 0 ? false : true,
        singleSelect: listform.singleselect,
        pageSize: listform.pagesize,
        pageList: eval(listform.limitsize),
        frozenColumns: [eval(gdColumnStr)],
        columns: [eval(fieldStr)],
        filterRules: eval(defaultFilterStr),
        rowStyler: function (index, row) {
            if (listform.rowstyle != "")
                return eval(listform.rowstyle + "(index, row);");
        },
        onDblClickRow: function (rowIndex, field, value) {
            eval(listform.dbClickFunName);
        },
        onLoadSuccess: function () {
            t1 = window.setTimeout(dataGridReflash, 4000);
            //加载自定义函数
            if (!CheckCustomLoadSuccessFun())
                return;
        },
        onBeforeLoad: function (param) {
            $.extend(true, param, $("#conditionform").serializeObject())
        },
        toolbar: '#toolbar'
    });
    var p = dataGrid.datagrid('getPager');
    $(p).pagination({
        displayMsg: '当前【 {from} 至 {to} 】 共【 {total} 】条记录',
        onBeforeRefresh: function(pageNumber, pageSize) {
            $(this).pagination('loading');
            $(this).pagination('loaded');
        }
    });
    //过滤规则
    if (data.filter)
        dataGrid.datagrid('enableFilter', eval(filterStr));

    // 手动加载数据, 防止多次加载
    dataGrid.datagrid('options').url = "/WebList/SearchEasyUiFormData";
    //判断是否自动加载
    if(autoloaddata)
        dataGrid.datagrid('load');
}

//**** 查询 ****
function searchRecord() {
    //$('#dataGridPic').datagrid('fixRowHeight');
    dataGrid.datagrid('reload');
}

//**** 导出 ****
function exportRecord() {
    //定义过滤
    $("#exportfilter").val(JSON.stringify(dataGrid.datagrid("options").filterRules));
    $("#exportsort").val(dataGrid.datagrid("options").sortName);
    $("#exportorder").val(dataGrid.datagrid("options").sortOrder);
    $("#exportcustomzdzd").val(chooseZdzdField.toString());

    var frm = $("#dataExportForm");
    frm.attr("action", "/WebList/ExportExcelData");
    frm.submit();
}

//**** 加密导出 ****
function exportEncRecord() {
    //定义过滤
    $("#exportfilter").val(JSON.stringify(dataGrid.datagrid("options").filterRules));
    $("#exportsort").val(dataGrid.datagrid("options").sortName);
    $("#exportorder").val(dataGrid.datagrid("options").sortOrder);
    $("#exportcustomzdzd").val(chooseZdzdField.toString());

    var frm = $("#dataExportForm");
    frm.attr("action", "/WebList/ExportEncData");
    frm.submit();
}

//初始化隐藏字段
function initHiddenField(data) {
    //初始化字段
    var fieldzdzd = data.zdzdList;
    //循环
    $(fieldzdzd).each(function(index) {
        //记录隐藏字段
        if (fieldzdzd[index].sfhidden && fieldzdzd[index].sfwitch) {
            hiddenZdzdField.push(fieldzdzd[index].zdname);
            //hiddenZdzdMc.push(fieldzdzd[index].fieldzdzd[index].zdsy);
            hiddenZdzdMc.push(fieldzdzd[index].zdsy);
            //hiddenZdzdMc[hiddenZdzdMc.length] = fieldzdzd[index].fieldzdzd[index].zdsy;
        }
    });
}



//初始化toolbar
function initToolbar(data) {
    var dataStr = "";
    //radio记数
    var radioNum = 0;
    dataStr += "<div class='padding_10'>";
    $(data).each(function (index) {
        //判断类型
        //按钮
        if (data[index].type == "" || data[index].type == "button") {
            dataStr += "<button type='button' class='btn btn-default stj_tab_btn_table' onclick='";
            if (data[index].check) {
                dataStr += "if(checkButtonData(" + data[index].id + ")) { " + data[index].funname + " }";
                dataStr += "'>";
            }
            else {
                dataStr += "" + data[index].funname + "'>";
            }

            dataStr += "    <i class='" + data[index].icon + "'>" + "</i>" + data[index].mc;
            dataStr += "</button>";
        } else if (data[index].type == "radio") {
            for (var i = 0; i < mcList.length; i++) {
                dataStr += "<input type='radio' name='radio" + radioNum + "' value='" + mcList[i] + "' ";
                if (mcList[i] == data[index].def)
                    dataStr += "checked='checked' ";
                dataStr += "onclick='" + data[index].funname + "' ";
                dataStr += "/>" + mcList[i];
            }
        }
        //单选框
        else if (data[index].type == "checkbox") {

            var mcList = data[index].mc.split(",");
            dataStr += "<div class='ondisplay' style='float:left;'>";
            dataStr += "<section title='.slideOne'>";
            dataStr += "<div class='slideOne'>";
            dataStr += "<input type='checkbox' value='None' id='slideOne' name='check' ";

            if (mcList[1] == data[index].def)
                dataStr += "checked ";
            dataStr += "onclick='" + data[index].funname + "' ";
            dataStr += " />";
            dataStr += "<label for='slideOne'></label>";
            dataStr += "</div>";
            dataStr += "<span class='qie_left'>" + mcList[0] + "</span>";
            dataStr += "<span class='qie_right'>" + mcList[1] + "</span>";


            dataStr += "</section>";
            dataStr += "</div>";
        }
        //单选框
        else if (data[index].type == "checkbox1") {


            var mcList = data[index].mc.split(",");
            dataStr += "<p class='field switch'>";
            if (mcList[0] == data[index].def) {
                checkbox1value = mcList[0];
                dataStr += "<label class='cb-enable selected'><span>" + mcList[0] + "</span></label>";
            } else
                dataStr += "<label class='cb-enable'><span>" + mcList[0] + "</span></label>";
            if (mcList[1] == data[index].def) {
                checkbox1value = mcList[1];
                dataStr += "<label class='cb-disable selected'><span>" + mcList[1] + "</span></label>";
            } else
                dataStr += "<label class='cb-disable'><span>" + mcList[1] + "</span></label>";

            //dataStr += "onclick='" + data[index].funname + "' ";

            dataStr += "</p>";
        }
        //下拉框
        else if (data[index].type == "select") {
            var mcList = data[index].mc.split(",");
            dataStr += "<select class='btn btn-default stj_tab_btn_table' style='float:none;padding-right:0' onchange='" + data[index].funname + "'>";
            for (var i = 0; i < mcList.length; i++) {
                dataStr += " <option value ='" + mcList[i] + "' ";
                if (mcList[i] == data[index].def)
                    dataStr += "selected = 'selected' ";
                dataStr += ">" + mcList[i] + "</option>";
            }
            dataStr += "</select>";
        }
    });

    //添加下拉选择隐藏字段
    if (hiddenZdzdField.length > 0) {
        dataStr += "<div class='shuaixuan_div'>";
        dataStr += "    <font class='shuaixuan_div_font'>筛选：</font><input id='zdzdChoose' style='width:100px' />";
        //hiddenZdzdField[i] + "'>" + hiddenZdzdMc[i]
        //        dataStr += "<input class=\"easyui-combobox\" "
        //        dataStr += "    data-options=\""
        //        dataStr += "        data:[";
        //        for (var i = 0; i < hiddenZdzdField.length; i++) {
        //            dataStr += "            {id:'" + hiddenZdzdField[i] + "',text:'" + hiddenZdzdMc[i] + "'},";
        //        }
        //        //去掉最后一个逗号
        //        if (dataStr.length > 0 && dataStr.charAt(dataStr.length - 1) == ',')
        //            dataStr = dataStr.substring(0, dataStr.length - 1);
        //        dataStr += "        ],";
        //        dataStr += "        multiple:true,";
        //        dataStr += "        valueField:'id',";
        //        dataStr += "        textField:'text'";
        //        dataStr += "    \"";
        //        dataStr += ">";
        dataStr += "</div>";
    }
    dataStr += "</div>";
    if (data.length > 0) {
        $("#toolmenu").append(dataStr);
        $.parser.parse($("#toolmenu"));
    }
}

//初始化toolbar
function initToolbarButton(data) {
    var ret = "";
    //radio记数
    var radioNum = 0;
    $(data).each(function (index) {
        //判断类型
        //按钮
        if (data[index].type == "" || data[index].type == "button") {
            ret += "<button type='button' class='btn btn-default stj_tab_btn_table' onclick='" + data[index].funname + "'>";
            ret += "    <i class='" + data[index].icon + "'>" + "</i>" + data[index].mc;
            ret += "</button>";
        } else if (data[index].type == "radio") {
            for (var i = 0; i < mcList.length; i++) {
                ret += "<input type='radio' name='radio" + radioNum + "' value='" + mcList[i] + "' ";
                if (mcList[i] == data[index].def)
                    ret += "checked='checked' ";
                ret += "onclick='" + data[index].funname + "' ";
                ret += "/>" + mcList[i];
            }
        }
        //单选框
        else if (data[index].type == "checkbox1") {
            var mcList = data[index].mc.split(",");
            ret += "<div class='ondisplay'>";
            ret += "<section title='.slideOne'>";
            ret += "<div class='slideOne'>";
            ret += "<input type='checkbox' value='None' id='slideOne' name='check' ";

            if (mcList[1] == data[index].def)
                ret += "checked ";
            ret += "onclick='" + data[index].funname + "' ";
            ret += " />";
            ret += "<label for='slideOne'></label>";
            ret += "</div>";
            ret += "<span class='qie_left'>" + mcList[0] + "</span>";
            ret += "<span class='qie_right'>" + mcList[1] + "</span>";


            ret += "</section>";
            ret += "</div>";
        }
        //下拉框
        else if (data[index].type == "select") {
            ret += "<select>";
            for (var i = 0; i < mcList.length; i++) {
                ret += "<option value ='" + mcList[i] + "'>" + mcList[i] + "</option>";
            }
            ret += "</select>";
            alert(ret);
        }
    });
    return ret;
}

//初始化查询栏
function initCondition(data,cols) {
    var dataStr = "";
    var keynum = 0;
    dataStr += "<div>";
    dataStr += "<form id='conditionform'>";
    $(data).each(function(index) {
        //控件类型
        if (data[index].FieldType == "" || data[index].FieldType == "TEXT") {
            dataStr += "<div class='inputTEXT inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionText(data[index]) + "</div>";
            keynum++;
        }
        //下拉框
        else if (data[index].FieldType == "SELECT") {
            dataStr += "<div class='inputSELECT inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionSelect(data[index]) + "</div>";
            keynum++;
        }
        //日期年
        else if (data[index].FieldType == "DATEYYYY") {
            dataStr += "<div class='inputDATES inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDateYyyy(data[index]) + "</div>";
            keynum++;
        }
        //日期年月
        else if (data[index].FieldType == "DATEYYYYMM") {
            dataStr += "<div class='inputDATES inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDateYyyyMm(data[index]) + "</div>";
            keynum++;
        }
        //日期月
        else if (data[index].FieldType == "DATEMM") {
            dataStr += "<div class='inputDATES inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDateMm(data[index]) + "</div>";
            keynum++;
        }
        //日期月
        else if (data[index].FieldType == "DATEM") {
            dataStr += "<div class='inputDATES inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDateMm(data[index]) + "</div>";
            keynum++;
        }
        //日期日
        else if (data[index].FieldType == "DATEDD") {
            dataStr += "<div class='inputDATES inputForm'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDateDd(data[index]) + "</div>";
            keynum++;
        }
        //日期范围
        else if (data[index].FieldType == "DATES") {
            var obj1 = clone(data[index]);
            obj1.FieldName = obj1.FieldName + "1";
            var obj2 = clone(data[index]);
            obj2.FieldName = obj2.FieldName + "2";
            dataStr += "<div class='inputDATES inputForm'><div class='fir'><label>" + data[index].FieldSy + ":</label>" + CreateConditionDate(obj1) + "</div><div class='sed'><span calss='between'>至</span>" + CreateConditionDate(obj2) + "</div></div>";
            keynum += 2;
        }
        //数值范围
        else if (data[index].FieldType == "NUMBERS") {
            var obj1 = clone(data[index]);
            obj1.FieldName = obj1.FieldName + "1";
            var obj2 = clone(data[index]);
            obj2.FieldName = obj2.FieldName + "2";
            dataStr += "<div class='inputNUMBERS inputForm'><div class='fir'><label>" + data[index].FieldSy + ":</label>" + CreateConditionNumber(obj1) + "</div><div class='sed'><span class='between'>至</span>" + CreateConditionNumber(obj2) + "</div></div>";
            keynum += 2;
        }

        if (cols != 0 && (keynum == cols || keynum == cols + 1)) {
            keynum = 0;
            dataStr += "<br/>";
        }
    });
    dataStr += "</form>";
    dataStr += "</div>";
    if (dataStr.length > 0) {
        $("#toolcondition").append(dataStr);
        $.parser.parse($("#toolcondition"));
    }
    if (cols) {
        return;
    }
    var maxWdith = 0,
        tmp, tmp;

    var ary = $('#conditionform').find('.inputSELECT,.inputTEXT,.fir,.sed');

    for (var i = 0, len = ary.length; i < len; i++) {
        tmp = ary.eq(i).outerWidth(true);
        if (tmp > maxWdith) {
            maxWdith = tmp;
        }
    }
    //console.log(maxWdith + "px");
    ary.width(maxWdith + "px");

}



//**********************查询条件控件*************************
//创建查询文本框控件
function CreateConditionText(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}
//创建查询下拉框控件
function CreateConditionSelect(ctrl) {
    var ret = "";
    //控件ID
    ret += "<select name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //长度
    if (ctrl.FieldWidth != 0)
        ret += "style='width:" + ctrl.FieldWidth + "px' ";
    ret += ">";
    //下拉框值
    var valueStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.CtrlString.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
    }
    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "' ";
        if (itemList[2] == "1") {
            ret += " selected='selected' ";
        }
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    return ret;
}
//创建查询日期控件
function CreateConditionDateYyyy(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false,dateFmt:\"yyyy\"})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}

//创建查询日期控件
function CreateConditionDateYyyyMm(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false,dateFmt:\"yyyy-MM\"})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}

//创建查询日期控件
function CreateConditionDateMm(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false,dateFmt:\"MM\"})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}

//创建查询日期控件
function CreateConditionDateM(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false,dateFmt:\"M\"})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}

//创建查询日期控件
function CreateConditionDateDd(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false,dateFmt:\"dd\"})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}

//创建查询日期控件
function CreateConditionDate(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //类型
    ret += "class='Wdate' onFocus='WdatePicker({isShowClear:false,readOnly:false})' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}
//创建查询数值控件
function CreateConditionNumber(ctrl) {
    var ret = "";
    //控件ID
    ret += "<input type='text' name='" + ctrl.FieldName + "' id='" + ctrl.FieldName + "' ";
    //控件值
    ret += "value='" + ctrl.DefVal + "' ";
    ret += "/>";
    return ret;
}
//***********************************************************

//初始化字段字典选择框
function initZdzdChoose() {
    var json = "[";
    for (var i = 0; i < hiddenZdzdField.length; i++) {
        json += "{id:'" + hiddenZdzdField[i] + "',text:'" + hiddenZdzdMc[i] + "'},";
    }
    //去掉最后一个逗号
    if (json.length > 0 && json.charAt(json.length - 1) == ',')
        json = json.substring(0, json.length - 1);
    json += "]";

    $('#zdzdChoose').combobox({
        editable: false,
        valueField: 'id',
        textField: 'text',
        data: eval(json),
        multiple: true,
        onChange: function(newValue, oldValue) {
            AnalysisValue(newValue, oldValue);
        }
    });
}

//分析值
function AnalysisValue(newList, oldList) {
    //多了
    if (newList.length > oldList.length) {
        for (var i = 0; i < newList.length; i++) {
            if (oldList.indexOf(newList[i]) == -1) {
                showGridColumn(true, newList[i]);
                //添加
                chooseZdzdField.push(newList[i]);
                break;
            }
        }
    }
    //少了
    else {
        for (var i = 0; i < oldList.length; i++) {

            if (newList.indexOf(oldList[i]) == -1) {
                showGridColumn(false, oldList[i]);
                //添加
                chooseZdzdField.removeByValue(oldList[i]);
                break;
            }
        }
    }
}

//显示或隐藏字段
function showGridColumn(showstate, name) {
    if (showstate)
        dataGrid.datagrid('showColumn', name);
    else
        dataGrid.datagrid('hideColumn', name);
}

var checkbox1value = "";

/* 新切换 */
function initCheckbox1() {
    $(".cb-enable").click(function() {
        if (checkbox1value == this.innerText)
            return;
        checkbox1value = this.innerText;
        var parent = $(this).parents('.switch');
        $('.cb-disable', parent).removeClass('selected');
        $(this).addClass('selected');
        $('.checkbox', parent).attr('checked', true);

        switchRecord(this);
    });

    $(".cb-disable").click(function() {
        if (checkbox1value == this.innerText)
            return;
        checkbox1value = this.innerText;
        var parent = $(this).parents('.switch');
        $('.cb-enable', parent).removeClass('selected');
        $(this).addClass('selected');
        $('.checkbox', parent).attr('checked', false);
        switchRecord(this);
    });
}


//************* 选择年-月 YYYY-MM *********************

function myyyyyformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y;
}


function myyyyymmformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '-' + (m < 10 ? ('0' + m) : m);
}



function myparser(s) {
    return;
    if (!s) return new Date();
    var ss = (s.split('-'));
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}
//************* 选择年-月 YYYY-MM *********************

//****************************************************
//检验按钮有效性
function checkButtonData(id) {
    var ret = false;
    //初始化界面数据包
    $.ajax({
        type: "POST",
        url: "/WebList/CheckButtonData",
        dataType: "json",
        data: { id: id, param: $("#param").val() },
        async: false,
        success: function (val) {
            try {
                if (!val.success) {
                    alert(val.msg);
                }
                else {
                    ret = true;
                }
            }
            catch (e) {
                alert("JS执行出错，原因：" + e.Message);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {

        }
    });
    //判断结果
    return ret;
}
//****************************************************
//图片
function showImage(url) {
    $('#imagecontent').html("<img id='easyImgField' height='100px' width='100px' src='" + url + "'/>");
    $('#easyImgField').viewer({
        hidden: function () {
            $('#imagecontent').html("");
        },
        shown: function () {

        }
    });
    $('#easyImgField').click();
}

//判断是否为图片
function checkImage(filename) {
    var ret = false;
    var exts = [".jpg", ".jpeg", ".png", ".bmp", ".gif"];
    var idx = filename.lastIndexOf(".");
    if (idx > -1) {
        var ext = filename.substring(idx).toLowerCase();
        if (exts.indexOf(ext) > -1) {
            ret = true;
        }
    }
    return ret;
}
//****************************************************
//************* daterange 过滤控件 自定义按钮 *********************
function getCustomizedDateRangeButtons(field) {
    var buttons = [
        {
            handler: function (target) {
                var v = $(target).combo('getValue');
                var op = 'range';
                var rule = dataGrid.datagrid('getFilterRule', field);
                if (rule != null) { op = rule.op; };
                dataGrid.datagrid('addFilterRule', {
                    field: field,
                    op: op,
                    value: v
                });
                dataGrid.datagrid('doFilter');
            }
        },
        {
            handler: function (target) {
                var v = $(target).combo('getValue');
                var op = 'range';
                var rule = dataGrid.datagrid('getFilterRule', field);
                if (rule != null) { op = rule.op; };
                dataGrid.datagrid('addFilterRule', {
                    field: field,
                    op: op,
                    value: v
                });
                dataGrid.datagrid('doFilter');
            }
        }
    ];
    //var handler1 = buttons[0].handler;
    //buttons.splice(0, 1, {
    //    text: function (target) {
    //        return $(target).datebox("options").customizedCurrentText;
    //    },
    //    handler: function (target) {
    //        handler1.call($.fn.datebox.defaults, target);
    //        var v = $(target).datebox('getValue');
    //        var op = 'equal';
    //        var rule = dataGrid.datagrid('getFilterRule', field);
    //        if (rule != null) { op = rule.op; };
    //        dataGrid.datagrid('addFilterRule', {
    //            field: field,
    //            op: op,
    //            value: v
    //        });
    //        dataGrid.datagrid('doFilter');
    //    }
    //});
    return buttons;
}

//判断是否图片
function isImage(filename) {
    var ret = false;
    if (filename.indexOf(".jpg") != -1 || filename.indexOf(".jpeg") != -1 || filename.indexOf(".bmp") != -1 || filename.indexOf(".png") != -1 || filename.indexOf(".gif") != -1)
        ret = true;
    return ret;
}

//缩略图对象
function ImageObj(thumb, origin) {
    return { thumb: thumb, origin: origin };
}

//多个图片
//urls：JSON格式的URL数组
//initindex:默认显示的图片索引值
function showImageList(urls, initindex) {
    var s = "";
    var urllist = JSON.parse(Base64decode(urls));
    if (urllist.length > 0) {
        $.each(urllist, function (index, u) {
            s += "<img id='easyImgField" + index.toString() + "' height='100px' width='100px' src='" + u.thumb + "' data-original='" + u.origin + "'/>";
        });
    }
    if (s != "") {
        $('#imagecontent').html(s);
        var v = $('#imagecontent').viewer({
            hidden: function () {
                $('#imagecontent').html("");
                $('#imagecontent').viewer('destroy');
            },
            shown: function () {
            },
            initialViewIndex: initindex,
            url: 'data-original'
        });
        $('#easyImgField' + initindex.toString()).click();
    }
}

function getCustomizedBdGridConfig(colzdzd) {
    var col = JSON.parse(Base64decode(colzdzd));
    var helplink = col.helplink;
    var field = col.zdname;
    var ret = {
        field: field,
        onselect: function (target) {
            var v = $(target).combo('getValue');
            var op = 'contains';
            var rule = dataGrid.datagrid('getFilterRule', field);
            if (rule != null) { op = rule.op; };
            dataGrid.datagrid('addFilterRule', {
                field: field,
                op: op,
                value: v
            });
            dataGrid.datagrid('doFilter');
        },
        oncancel: function (target) {
            var v = $(target).combo('getValue');
            var op = 'contains';
            var rule = dataGrid.datagrid('getFilterRule', field);
            if (rule != null) { op = rule.op; };
            dataGrid.datagrid('addFilterRule', {
                field: field,
                op: op,
                value: v
            });
            dataGrid.datagrid('doFilter');
        }
    };
    var plist = [];
    // 解析helplink配置参数
    if (helplink != null && helplink != "") {
        var arr = helplink.split('|||');
        if (arr.length > 0) {
            $.each(arr, function (i, p) {
                var idx = p.indexOf('-');
                if (idx > -1) {
                    var pn = p.substring(0, idx);
                    var pv = p.substring(idx + 1);
                    if (pv != "") {
                        plist.push({ name: pn, value: pv });
                    }
                }
            });
        }
    }
    if (plist.length > 0) {
        $.each(plist, function (i, p) {
            ret[p.name] = p.value;
        });
    }
    return ret;
}
//****************************************************


function ajaxTpl(url, params, handle, complete) {
    //var index = layer.load(2, { time: 10000 });
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        success: function (data) {
            //layer.close(index);
            handle(data);
        },
        fail: function (err) {
            //layer.close(index);
            //console.log(err);
        },
        complete: function () {
            if (complete) {
                complete();
            }
        }
    });
}

//JS获取url参数
function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}