//**** 全局变量 ****
var conditionNum = 5;

//**** 定义对象 ****
function KeyParam() {
    this.key = "";
    this.value = "";
}

//**** 定义函数 ****
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

//**** 动态隐藏定义层 ****
//动态创建层
function CreateDiv(table, field, presuffix,suffix) {
    var ret = "";
    ret += "<div id='" + table + "Div'>";
    ret += CreateField(table, field, presuffix,suffix);
    ret += "</div>";
    return ret;
}

function CreateField(table, field, presuffix,suffix) {
    //当前动态生成的控件排号
    var ctrlNum = 0;
    var ret = "";
    ret += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='" + suffix + "'/>";
    ret += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>"; 
    ret += "<table style='width:100%'>";
    $(field).each(function (index) {
        //每个控件
        ctrlNum = ctrlNum + 1;
        if (ctrlNum == 1)
            ret += "<tr>";
        ret += "<td style='width: 90px; padding: 1px 5px 1px 5px;' align='right'>" + field[index].sy + "</td>";
        ret += "<td style='width: 90px; padding: 1px 5px 1px 5px;' align='left'>";

        //判断类型
        switch (field[index].kjlx) {
            case "TEXT":
                ret += CreateHtmlText(field[index], presuffix + suffix);
                break;
            case "CHECKBOX":
                ret += CreateHtmlText(field[index], presuffix + suffix);
                break;
            case "RADIO":
                ret += CreateHtmlText(field[index], presuffix + suffix);
                break;
            case "SELECT":
                ret += CreateHtmlSelect(field[index], presuffix + suffix);
                break;
            default:
                ret += CreateHtmlText(field[index], presuffix + suffix);
                break;
        }
        ret += "</td>";
        //判断是否已经到最后一列中
        if (ctrlNum == conditionNum) {
            ret += "</tr>";
            //当前列数清0
            ctrlNum = 0;
        }
        //判断是否到达最后一个控件
        else if (index == field.length - 1) {
            //判断是否需要填充
            var blankNum = conditionNum - ctrlNum;
            while (blankNum > 0) {
                ret += "<td style='width: 90px; padding: 1px 5px 1px 5px;' align='right'></td>";
                ret += "<td style='width: 90px; padding: 1px 5px 1px 5px;' align='left'></td>";

                blankNum = blankNum - 1;
            }
            ret += "</tr>";
        }
    });
    ret += "</table>";
    return ret;
}

//**** 创建控件 ****
//** HTML控件 **
//文本框TEXT
function CreateHtmlText(ctrl, suffix) {
    var ret = "";
    ret += "<input type='text' custom='custom' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' sy = '" + ctrl.sy + "' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' />";
    return ret;
}

//复选框CHECKBOX
function CreateHtmlCheckBox(ctrl, suffix) {
    var ret = "";
    ret += "<input type='text' custom='custom' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' sy = '" + ctrl.sy + "' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' />";
    return ret;
}

//单选框RADIO
function CreateHtmlRadio(ctrl, suffix) {

}

//下拉框SELECT
function CreateHtmlSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select custom='custom' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' sy = '" + ctrl.sy + "' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";

    //下拉框值
    var valueStr = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
        else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    if (funStr.length > 0)
        ret += "onchange=\"CtrlChange('" + funStr + "');\"";
    ret += ">";

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "' ";
        if (items[2] == "1")
            ret += " selected='selected' ";
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    return ret;
}

//大文本框TEXTAREA
function CreateHtmlTextarea(ctrl, suffix) {

}


//特殊控件
//** **




//**** 界面 ****
//** 从表 **
//从表Tab项模板
function GetT2Tab(table, sy, hasDetail, detail, detailsy) {
    //var regS = new RegExp("&amp;&amp;table&amp;&amp;", "g");
    //var html = $("#hiddenTemplateDiv").html();
    //html = html.replace(regS, table);
    var html = "";
    html += "<input type=\"hidden\" id=\"" + table + "Record\" name=\"" + table + "Record\" value=\"0\"/>";
    html += "<div class=\"tabs-container\">";
    html += "   <input type=\"button\" id=\"" + table + "btn\" name=\"" + table + "btn\" value='添加' onclick=\"AddT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\" />";
    html += "   <input type=\"button\" value='删除' onclick=\"DelT2Page('" + table + "');\" />";
    html += "   <div id=\"" + table + "Nav\">";
    html += "       <ul class=\"nav nav-tabs\"></ul>";
    html += "   </div>";
    html += "   <div id=\"" + table + "Tab\" class=\"tab-content\"></div>";
    html += "</div>";
    return html;
}

//添加Tab项
//table:表名
//sy:释义
//hasDetail:是否有明细表
//detail:明细表名
//rec:当前从表记录数
function AddT2Page(table,sy,hasDetail,detail,detailsy) {
    //清除class
    $("#" + table + "Nav ul li").each(function(index){
        $(this).removeClass("active");
    });

    $("#" + table + "Tab div").each(function (index) {
        $(this).removeClass("active");
    });

    //当前记录数
    var record = parseInt($("#" + table + "Record").val()) + 1;
    //定义添加的tab标签
    var lab = "";
    lab += "<li class='active'>";
    lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true'>" + sy + record + "</a>";
    lab += "</li>";
    $("#" + table + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "Tab-" + record + "' class='tab-pane active'>";
    content += "    <div class='panel-body'>";
    //字段内容
    var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    var fieldcontent = $("#" + table + "Div").html();
    fieldcontent = fieldcontent.replace(regField, record) 
    content += fieldcontent;
    //判断是否包含明细表项
    //****************************************
    if (hasDetail)
        content += GetT3Tab(table,detail, detailsy, record);
    //****************************************
    content += "    </div>";
    content += "</div>";
    $("#" + table + "Tab").append(content);
    //记录数
    $("#" + table + "Record").val(record);
}

//删除Tab项
function DelT2Page(table) {
    //删除当前选中的标签项
    $("#" + table + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active"))
            $(this).remove();
    });
    //删除当前选中的内容项
    $("#" + table + "Tab div").each(function (index) {
        if ($(this).hasClass("tab-pane active"))
            $(this).remove();
    });
    //当前记录数减1
    //$("#" + table + "Record").val(parseInt($("#" + table + "Record").val()) - 1);
    //激活第一个标签项
    $("#" + table + "Nav ul li:first-child").addClass("active");
    //激活第一个内容项
    $("#" + table + "Tab div:first-child").addClass("active");
}

//** 明细 **
//从表Tab项模板
function GetT3Tab(tablab,table, sy, rec) {
    var html = "";
    //当前Tab项记录数
    html += "<input type=\"hidden\" id=\"" + tablab + "_" + rec + "Record\" name=\"" + tablab + "_" + rec + "Record\" value=\"0\"/>";
    html += "<div class=\"tabs-container\">";
    html += "   <input type=\"button\" id=\"" + table + "btn\" name=\"" + table + "btn\" value='添加' onclick=\"AddT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\" />";
    html += "   <input type=\"button\" value='删除' onclick=\"DelT3Page('" + table + "'," + rec + ");\" />";
    html += "   <div id=\"" + table + "_" + rec + "Nav\">";
    html += "       <ul class=\"nav nav-tabs\"></ul>";
    html += "   </div>";
    html += "   <div id=\"" + table + "_" + rec + "Tab\" class=\"tab-content\"></div>";
    html += "</div>";
    return html;
}

//添加Tab项
//table:表名
//sy:释义
//hasDetail:是否有明细表
//detail:明细表名
//rec:当前从表记录数
function AddT3Page(tablab,table, sy, rec) {
    //清除class
    $("#" + table + "_" + rec + "Nav ul li").each(function (index) {
        $(this).removeClass("active");
    });

    $("#" + table + "_" + rec + "Tab div").each(function (index) {
        $(this).removeClass("active");
    });


    //当前记录数
    var record = parseInt($("#" + tablab + "_" + rec + "Record").val()) + 1; 
    //定义添加的tab标签
    var lab = "";
    lab += "<li class='active'>";
    lab += "    <a data-toggle='tab' href='#" + table + "_" + rec + "Tab-" + record + "' aria-expanded='true'>" + sy + record + "</a>";
    lab += "</li>";
    $("#" + table + "_" + rec + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "_" + rec + "Tab-" + record + "' class='tab-pane active'>";
    content += "    <div class='panel-body'>";
    //字段内容
    var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    var fieldcontent = $("#" + table + "Div").html();
    fieldcontent = fieldcontent.replace(regField, rec + "_" + record)
    content += fieldcontent;
    content += "    </div>";
    content += "</div>";
    $("#" + table + "_" + rec + "Tab").append(content);
    //记录数加1
    $("#" + tablab + "_" + rec + "Record").val(record);
}

//删除Tab项
function DelT3Page(table, rec) {
    //删除当前选中的标签项
    $("#" + table + "_" + rec + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active"))
            $(this).remove();
    });
    //删除当前选中的内容项
    $("#" + table + "_" + rec + "Tab div").each(function (index) {
        if ($(this).hasClass("tab-pane active"))
            $(this).remove();
    });
    //当前记录数减1
    //$("#" + table + "_" + rec + "Record").val(parseInt($("#" + table + "_" + rec + "Record").val()) - 1);
    //激活第一个标签项
    $("#" + table + "_" + rec + "Nav ul li:first-child").addClass("active");
    //激活第一个内容项
    $("#" + table + "_" + rec + "Tab div:first-child").addClass("active");
}



//**** 分析控件 ****
//获取控件必输项
function GetCtrlMustIn(name) {
    return $("#" + PointRep(name)).attr("mustin");
}
//获取控件类型
function GetCtrlType(name) {
    return $("#" + PointRep(name)).attr("kjlx");
}
//获取控件检验类型
function GetCtrlInputType(name) {
    return $("#" + PointRep(name)).attr("inputlx");
}

//获取控件默认值
function GetDefValue(name) {
    return $("#" + PointRep(name)).attr("def");
}

//获取控件名称
function GetCtrlMc(name) {
    return $("#" + PointRep(name)).attr("mc");
}

//获取控件获取name
function GetNameByCtrl(ctrl) {
    var kjlx = ctrl.attr("kjlx");
    var name = "";
    switch (kjlx) {
        case "RADIO":
        case "CHECKBOX":
            name = ctrl.attr("name");
            break;
        default:
            name = ctrl.attr("id");
            break;
    }
    return name;
}

//获取控件值
function GetCtrlValue(name) {
    var value = "";
    var kjlx = GetCtrlType(name);
    name = PointRep(name);
    //****判断控件*****
    //HTML
    if (kjlx == "TEXT") {
        value = $("#" + name).val();
    }
    else if (kjlx == "SELECT") {
        value = $("#" + name).val();
    }
    else if (kjlx == "CHECKBOX") {
        var chk_value = [];
        $('input[name="' + name + '"]:checked').each(function () {
            chk_value.push($(this).val());
        });
        value = chk_value;
    }
    else if (kjlx == "RADIO") {
        value = $("input[name='" + name + "']:checked").val();
    }
    //EasyUI
    else if (kjlx == "EASYCOMBOGRID") {
        value = $("#" + name).combogrid("getValue");
    }
    else if (kjlx == "EASYCOMBOBOX") {
        value = $("#" + name).combobox("getValue");
    }
    else if (kjlx == "EASYDATE") {
        value = $("#" + name).datebox("getValue");
    }
    else if (kjlx == "EASYDATETIME") {
        value = $("#" + name).datetimebox("getValue");
    }
    else
        value = $("#" + name).val();
    return value;
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
                $("#" + name).val("");
            else
                $("#" + name).val(value[0].value);
        }
        else
            $("#" + name).val(value);
    }
    else if (kjlx == "CHECKBOX" || kjlx == "C") {
        //清除原来选择的内容
        //$('input[name="' + name + '"]').removeAttr("checked"); //取消全选  
        if (value instanceof Array) {
            $("input[name='" + zdmc + "']").each(function () {
                if ($.inArray($(this).val(), value) == -1)
                    $(this).attr("checked", false);
                else
                    $(this).attr("checked", true);
            });
        }
        else {
            if (value.split(",").length > 1) {
                value = value.split(",");
                $("input[name='" + name + "']").each(function () {
                    if ($.inArray($(this).val(), value) == -1)
                        $(this).attr("checked", false);
                    else
                        $(this).attr("checked", true);
                });
            }
            else {
                //遍历选中
                $('input[name="' + name + '"]').each(function () {
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
                $("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value[0].value)
                        $(this).attr("checked", "checked");
                });
            }
        }
        else {
            if (value.split(",").length > 1) {
                $("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value[0])
                        $(this).attr("checked", "checked");
                });
            }
            else {
                $("input[name='" + name + "']").each(function () {
                    if ($(this).val() == value)
                        $(this).attr("checked", "checked");
                });
            }

        }
    }
    //****EasyUI****
    else if (kjlx == "EASYCOMBOGRID" || kjlx == "Z") {
        if (value instanceof Array)
            $("#" + name).combogrid("setValues", value);
        else if (value.split(",").length > 1)
            $("#" + name).combogrid("setValues", value.split(","));
        else
            $("#" + name).combogrid("setValue", value);
    }
    else if (kjlx == "EASYCOMBOBOX") {
        if (value instanceof Array)
            $("#" + name).combobox("setValues", value);
        else if (value.split(",").length > 1)
            $("#" + name).combobox("setValues", value.split(","));
        else
            $("#" + name).combobox("setValue", value);
    }
    else if (kjlx == "EASYDATE") {
        if (value instanceof Array)
            $("#" + name).datebox("setValue", value[0].value);
        else
            $("#" + name).datebox("setValue", value);
    }
    else if (kjlx == "EASYDATETIME") {
        if (value instanceof Array)
            $("#" + name).datetimebox("setValue", value[0].value);
        else
            $("#" + name).datetimebox("setValue", value);
    }
    else {
        if (value instanceof Array)
            $("#" + name).val(value[0].value);
        else
            $("#" + name).val(value);
    }
}

//初始化控件原型数据
function SetCtrlMode(name, value) {
    var kjlx = GetCtrlType(name);
    name = PointRep(name);
    //下拉框
    if (kjlx == "SELECT") {
        //清空原有内容
        $("#" + name).empty();
        $(value).each(function (inde) {
            $("#" + name).append("<option value='" + value[inde].value + "'>" + value[inde].content + "</option>");
        });
    }
}

//初始化触发控件改变事件
function InitCtrlEvent(name) {
    name = PointRep(name);
    //==var ctrlhtml = $("input[name='" + name + "']").parent();
    var ctrlhtml = $("#" + name).parent();
    var strhtml = ctrlhtml.html();
    if (strhtml != undefined && strhtml.toLowerCase().indexOf("onchange") > -1) {
        var var1 = strhtml;
        var var2 = "onchange";
        var index1 = var1.indexOf(var2) + var2.length + 1;

        var1 = strhtml;
        var index2 = var1.indexOf("\"", index1);
        var index3 = var1.indexOf("\"", index2 + 1);
        var varPar1 = var1.substr(index2 + 1, index3 - index2 - 1);
        if (!eval(varPar1))
            ret = false;
    }
}

//还原控件默认值
function RecoveryDefValue(name) {
    //排除没有默认值控件
    var kjlx = GetCtrlType(name);
    if (kjlx == "CHECKBOX" || kjlx == "RADIO") {
        return;
    }
    var value = GetDefValue(name);
    SetCtrlValue(name, value);
}


//********************保存数据********************
//暂存
function btn_opt_zc() { 
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serializeObject(),
        success: function (val) {
            if (!val.success) {
                alert(val.msg);
                return;
            }
            else
                alert(val.msg);
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
       
        }
    });
}