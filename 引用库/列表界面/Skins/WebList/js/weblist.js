
//**** 全局变量 ****
var conditionNum = 4;

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

//**** 查询 ****
function searchRecord() {
    dataGrid.jqxGrid('updatebounddata');
}

//**** 导出 ****
function exportRecord() {
    var frm = $("#dataExportForm");
    frm.attr("action", "/WebList/ExportData");
    frm.submit();
}

//**** 加密导出 ****
function exportEncRecord() {
    var frm = $("#dataExportForm");
    frm.attr("action", "/WebList/ExportEncData");
    frm.submit();
}

/*
//初始化按钮
function iniButton(data) {
    var ret = "";
    $(data).each(function (index) {
        ret += "<a ";
        ret += "class='btn btn-w-m btn-primary stj_btn_y_f'";
        ret += "href='javascript:void(0);' ";
        ret += "onclick='" + data[index].funname + "' ";
        ret += ">";
        ret += data[index].mc;
        ret += "</a>";
    });
    return ret;
}
*/

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
        }
        else if (data[index].type == "radio") {
            for (var i = 0; i < mcList.length; i++) {
                ret += "<input type='radio' name='radio" + radioNum + "' value='" + mcList[i] + "' ";
                if (mcList[i] == data[index].def)
                    ret += "checked='checked' ";
                ret += "onclick='" + data[index].funname + "' ";
                ret += "/>" + mcList[i];
            }
        }
        //单选框
        else if (data[index].type == "checkbox") {
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
    });
    return ret;
}

//初始化查询条件
function initCondition(data) {
    //当前动态生成的控件排号
    //$("#conditionDiv").width(1920);

    var ctrlNum = 0;
    //var ret = "<div id='table_cuhhhhhgg' style='height:auto;'></div>";
    //$("#table_cuhhhhhgg").width(1920);
    // var  ret_s= "<div id='table_cd'>";
    //********************************//
    var ret_s = "";
    var ret = "";
    //ret += "<table class='stj_lr_tab'" + "style='float:left; width:" + conditionNum * 450 + "px'" + " >";
    if ((data.length + 1) * 450 < ($(window).width() - 50)) {
        //
        ret += "<table class='stj_lr_tab'" + "style='float:left; width:" + ($(window).width() - 50) + "px'" + " >";
        conditionNum = data.length + 1;
    }
    else {
        ret += "<table class='stj_lr_tab'" + "style='float:left; width:" + conditionNum * 450 + "px'" + " >";
    }
    //********************************//
    // var temp2 = document.getElementsByClassName("wrapper wrapper-content animated fadeInRight").item(0).offsetWidth;
    // var td_width = temp2 / conditionNum;
    $(data).each(function (index) {
        //判断控件
        //判断是否为占多列控件
        cx_table = data.length;
        //alert(data.length);
        if (data[index].type == "NUMBERS") {
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr>";
            //style='width:" + td_width + "px'  colspan='2'
            ret += "<td  class='stj_lr_td' style='width:450px' >";

            if (data[index].sy.length > 6) {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:23px;'>" + data[index].sy + "</label>";
            }
            else {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:46px;'>" + data[index].sy + "</label>";
            }
            // <label class='col-sm-3 control-label stj_label'>" + data[index].sy + "</label>";

            var obj1 = clone(data[index]);
            obj1.name = obj1.name + "1";
            //style='width:35% ; float:left'
            ret += "<div class='col-sm-9 date stj_input2' >" + CreateHtmlText(obj1) + "</div>";
            ret += "<label class='col-sm-3 control-label stj_label2' style='float:left;line-height:46px;'>至</label>";
            var obj2 = clone(data[index]);
            obj2.name = obj2.name + "2";
            //'width:35% ;
            ret += "<div class='col-sm-9 date stj_input2' style=' float:left'>" + CreateHtmlText(obj2) + "</div></td>";
        }
        //日期范围
        else if (data[index].type == "DATES") {
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr>";
            //style='width:" + td_width + "px'  colspan='2'
            ret += "<td class='stj_lr_td' style='width:450px'>";

            if (data[index].sy.length > 6) {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:23px;'>" + data[index].sy + "</label>";
            }
            else {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:46px;'>" + data[index].sy + "</label>";
            }

            // <label class='col-sm-3 control-label stj_label'>" + data[index].sy + "</label>";
            var obj1 = clone(data[index]);
            obj1.name = obj1.name + "1";
            // style='width:35% ; float:left'
            ret += "<div class='col-sm-9 date stj_input2'>" + CreateHtmlDate(obj1) + "</div>";
            //width:auto
            ret += "<label class='col-sm-3 control-label stj_label2' style='float:left; line-height:46px;'>至</label>";
            var obj2 = clone(data[index]);
            obj2.name = obj2.name + "2";
            //width:35% ; 
            ret += "<div class='col-sm-9 date stj_input2' style='float:left'>" + CreateHtmlDate(obj2) + "</div></td>";
        }
        //日期控件
        else if (data[index].type == "DATE") {
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr>";
            //style='width:" + td_width + "px'
            ret += "<td  class='stj_lr_td' style='width:450px' >";

            if (data[index].sy.length > 6) {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:23px;'>" + data[index].sy + "</label>";
            }
            else {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:46px;'>" + data[index].sy + "</label>";
            }

            //alert(data[index].sy.length);
            ret += "<div class='col-sm-9 date stj_input' >" + CreateHtmlDate(data[index]) + "</div></td>";
        }
        //下拉控件
        else if (data[index].type == "SELECT") {
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr class='stj_lr_tr'>";
            //style='width:" + td_width + "px'
            ret += "<td class='stj_lr_td' style='width:450px' >"

            if (data[index].sy.length > 6) {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:23px;'>" + data[index].sy + "</label>";
            }
            else {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:46px;'>" + data[index].sy + "</label>";
            }

            // <label class='col-sm-3 control-label stj_label'>" + data[index].sy + "</label>";
            ret += "<div class='col-sm-9 stj_input'>" + CreateHtmlSelect(data[index]) + "<div> </td>";
        }
        //单列控件
        else {
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr class='stj_lr_tr'>";
            //style='width:" + td_width + "px'
            ret += "<td class='stj_lr_td' style='width:450px' >";

            if (data[index].sy.length > 6) {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:23px;'>" + data[index].sy + "</label>";
            }
            else {
                ret += " <label class='col-sm-3 control-label stj_label' style='line-height:46px;'>" + data[index].sy + "</label>";
            }

            // <label class='col-sm-3 control-label stj_label'>" + data[index].sy + "</label>";
            ret += "<div class='col-sm-9 stj_input'>" + CreateHtmlText(data[index]) + "</div></td>";
        }

        //判断是否已经到最后一列中
        if (ctrlNum == conditionNum && index < data.length - 1) {
            ret += "</tr>";
            //当前列数清0
            ctrlNum = 0;
        }

        //判断是否已经到最后一行的最后一列中
        else if (ctrlNum == conditionNum && index == data.length - 1) {
            //ret += 
            ret += "</tr>";
            //当前列数清0
            ctrlNum = 0;

            /************************************************************************************************/
            /*
            for (i=0;i<conditionNum;i++)
            {
            if(i==0){ret += "<tr>";
            ret +="<td style='width:450px;'></td>";
				
            }
            else if(i==conditionNum-1)
            {  ret +="<td style='width:450px;'>";
            ret += "<a class='btn btn-w-m btn-primary stj_btn_y_f' href='javascript:void(0);' onclick='searchRecord();' style='float:right;font-size: 20px; padding-top: 3px; padding-bottom: 3px;'>查询</a>";
            ret +="</td></tr>"; 
            }
            else
            {
            ret +="<td style='width:450px;'></td>";
            }
				
            }
            */
            /************************************************************************************************/
        }
        //判断是否到达最后一个控件
        else if (index == data.length - 1) {
            //判断是否需要填充
            ret += "<td class='stj_lr_td'   >"
            //float:left;
            ret += "<a class='btn btn-w-m btn-primary stj_btn_y_f' href='javascript:void(0);' onclick='searchRecord();' style=' font-size: 20px;padding-top: 3px; padding-bottom: 3px;'>查询</a>";
            ret += "</td>"
            var blankNum = conditionNum - ctrlNum - 1;
            cx_btn = true;

            while (blankNum > 0) {
                //style='width:" + td_width + "px'
                ret += "<td class='stj_lr_td' style='width:450px'  > ";
                ret += "<div class='col-sm-9 stj_input'></div> </td>";

                blankNum = blankNum - 1;
            }
            ret += "</tr>";

        }
    });

    ret += "</table>";

    if (cx_btn == false) {
        var cx_width = $(window).width() - conditionNum * 450 - 20 - 66;
        if (cx_width < 66) { ret += "<table class=\"stj_lr_tab\" style=\"width:66px;float: left;\">"; }
        //
        else { ret += "<table class=\"stj_lr_tab\" style=\"min-width:50px;float: left;width:" + cx_width + "px \">"; }
        ret += "<tbody>";
        var cx_table_num = Math.ceil(cx_table / conditionNum);
        for (i = 0; i < cx_table_num; i++) {
            if (i == cx_table_num - 1) {
                ret += "<tr> <td style='padding:0px;padding:12px 0px;'>";
                //float:left;
                ret += "<a class=\"btn btn-primary \" href=\"javascript:void(0);\" onclick=\"searchRecord();\" style=\"font-size: 20px;height:36px;\">查询</a>";
                ret += "</td>  </tr>"
            }
            else { ret += "<tr><td style='height:56px;'><td></tr>"; }
        }
        ret += "</tbody></table>";
        if (cx_btn == true) {
            ret_s = "<div style='width:" + (conditionNum * 450 + 20) + "px'>";
            // $("#table_cd").width(conditionNum*450+20); 
        }
        else {
            if (cx_width > 90) {
                ret_s = "<div style='width:" + (conditionNum * 450 + 20 + cx_width) + "px'>";

                //$("#table_cd").width(conditionNum*450+20+cx_width);
            }
            else {
                ret_s = "<div style='width:" + (conditionNum * 450 + 20 + 66) + "px'>";

                // $("#table_cd").width(conditionNum*450+20+90); alert("ok");
            }
            //conditionNum*450+20+90
        }
    }
    //else
    //{$("#table_cd").width(conditionNum*450+20);}
    var ret_s = ret_s + ret + "</div>";
    //ret +=
    return ret_s;
    //********************************//
}




//**** 基本控件定义 ****
//** HTML控件 **
//文本框TEXT
function CreateHtmlText(data) {
    var ret = "";
    ret += "<input type='text' custom='custom' id='" + data.name + "' name = '" + data.name + "' class='form-control searchPress' ";
    //控件信息
    ret += "kjlx='" + data.type + "' ";
    //默认值
    ret += "value='" + data.defval + "' ";
    //判断是否包含helplink
    if (data.helplink.length > 0)
        ret += "onkeydown=\"ShowHelpLinkForm('" + data.helplink + "');\" ";
    ret += "/>";
    return ret;
}
//下拉框SELECT
function CreateHtmlSelect(data) {
    var ret = "";
    ret += "<select class='form-control' custom='custom' id='" + data.name + "' name = '" + data.name + "' ";
    //长度
    ret += "style='width:" + data.width + "%' ";
    //控件信息
    ret += "kjlx='" + data.type + "' ";
    //默认值
    ret += "value='" + data.defval + "' ";


    //下拉框值
    var valueStr = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = data.ctrlstring.split("||");
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

//日期框DATE
function CreateHtmlDate(data) {
    var ret = "";
    ret += "<input type='text' custom='custom' id='" + data.name + "' name = '" + data.name + "' class='Wdate form-control searchPress' onFocus='WdatePicker({isShowClear:false,readOnly:false})' ";
    //控件信息
    ret += "kjlx='" + data.type + "' ";
    //默认值
    ret += "value='" + data.defval + "' ";
    ret += "/>";
    return ret;
}

//数字范围NUMBER
function CreateHtmlNumber(data) {
    var ret = "";
    ret += "<input type='text' custom='custom' id='" + data.name + "' name = '" + data.name + "' class='searchPress' ";
    //控件信息
    ret += "kjlx='" + data.type + "' ";
    //默认值
    ret += "value='" + data.defval + "' ";
    ret += "/>";
    return ret;
}




//**** 绑定控件值 ****
//**** 初始化控件****
function SetCtrlMode(name, value) {
    //清空原有内容
    $("#" + name).empty();
    $(value).each(function (inde) {
        $("#" + name).append("<option value='" + value[inde].value + "'>" + value[inde].content + "</option>");
    });
}

//获取控件类型
function GetCtrlType(name) {
    return $("#" + PointRep(name)).attr("kjlx");
}

//获取控件默认值
function GetDefValue(name) {
    return $("#" + PointRep(name)).attr("def");
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
    else {
        if (value instanceof Array)
            $("#" + name).val(value[0].value);
        else
            $("#" + name).val(value);
    }
}

//初始化触发控件改变事件
function InitCtrlEvent(name) {
    name = PointRep(name);
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


//*************************************CtrlString************************************
//改变第三控件的值
function CtrlChange(str) {
    //判断是否需要改变其他控件值
    if (str == "")
        return;
    //值对
    var keyParam;
    var typeList = str.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        if (keyParam.key == "ctrlChange") {
            CtrlChangeContent(keyParam.value);
        }
    }
}

//改变每一组控件
function CtrlChangeContent(str) {
    var ctrlstring = "";
    ctrlstring += "ctrlChange--";
    //值对
    var keyParam;
    //目标控件
    var targetctrl = "";
    //分析每一项
    var items = str.split("|");
    for (var iItem = 0; iItem < items.length; iItem++) {
        keyParam = StrSplit(items[iItem], "-");
        if (keyParam.key == "wherectrl") {
            ctrlstring += keyParam.key + '-'
            var ctrlList = keyParam.value.split(",");
            for (var iCtrlList = 0; iCtrlList < ctrlList.length; iCtrlList++) {
                ctrlstring += GetCtrlValue(ctrlList[iCtrlList]) + ",";
            }
            //去掉最后一个,
            if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == ',')
                ctrlstring = ctrlstring.substring(0, ctrlstring.length - 1)
            ctrlstring += "|";
        }
        //获取目标控件
        else if (keyParam.key == "targetctrl") {
            targetctrl = keyParam.value;
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
    $.ajaxSetup({
        async: false
    });
    $.post("/WebList/CtrlStringData", { ctrlString: ctrlstring }, function (data, textStatus) {
        if (textStatus == "success") {
            var val = $.parseJSON(data);
            if (val.success) {
                //判断控件类型,根据类型是初始化控件模型还是设置控件值
                var kjlx = GetCtrlType(targetctrl);
                if (kjlx == "SELECT")
                    SetCtrlMode(targetctrl, val.data.rows);
                else
                    SetCtrlValue(targetctrl, val.data.rows);
                //触发改变事件
                InitCtrlEvent(targetctrl);
            }
            else {
                alert(val.msg);
            }
        }
        else
            alert("访问出错");
    });
    $.ajaxSetup({
        async: false
    });
}
//*************************************CtrlString************************************

//*************************************Helplink**************************************
//显示helplink
function ShowHelpLinkForm(helplink) {
    //判断按下F2时,触发
    if (event.keyCode != 113)
        return;

    var helplinkValue = "";
    //值对
    var keyParam;
    var typeList = helplink.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        if (keyParam.key == "helplink") {
            helplinkValue = keyParam.value;
            break;
        }
    }

    //判断是否包含helplink
    if (helplinkValue == "")
        return;

    //传输helpStr
    var helpStr = "";
    helpStr = helpStr + "helplink--";
    //控件项列表
    var items = helplinkValue.split("|");
    for (var i = 0; i < items.length; i++) {
        keyParam = StrSplit(items[i], "-");
        if (keyParam.key == "wherectrl") {
            helpStr = helpStr + "wherectrl-";
            var ctrlList = keyParam.value.split(",");
            for (var iCtrlList = 0; iCtrlList < ctrlList.length; iCtrlList++) {
                helpStr = helpStr + GetCtrlValue(ctrlList[iCtrlList]) + ",";
            }
            //去掉最后一个,
            if (helpStr.length > 0 && helpStr.charAt(helpStr.length - 1) == ',')
                helpStr = helpStr.substring(0, helpStr.length - 1)
            helpStr = helpStr + "|";
        }
        else {
            helpStr = helpStr + items[i] + "|";
        }
    }

    //去掉最后一个字符|
    if (helpStr.length > 0 && helpStr.charAt(helpStr.length - 1) == '|')
        helpStr = helpStr.substring(0, helpStr.length - 1)

    //iframe层
    layer.open({
        type: 2,
        title: '数据信息',
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: '/WebList/Helplink?HelpLink=' + helpStr + '&Math.random()' //iframe的url
    });
}
//*************************************Helplink**************************************
























//if (ctl.substring(0, 2) == "$$")

//$detail.html(res.replace(/\n/g, '<br>'));

//            $(window).resize(function () {
//                $table.bootstrapTable('resetView', {
//                //height: getHeight()
//            });
//        });


function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row.id
    });
}

//    function responseHandler(res) {
//        $.each(res.rows, function (i, row) {
//            row.state = $.inArray(row.id, selections) !== -1;
//        });
//        return res;
//    }


function getHeight() {
    return $(window).height() - $('h1').outerHeight(true);
}
