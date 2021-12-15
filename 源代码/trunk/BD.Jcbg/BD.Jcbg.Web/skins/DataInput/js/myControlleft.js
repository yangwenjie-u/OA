//判断是否启用只读文本模式
var isReadonlyLabel = false;
//******************************//
var all_k = 350;
var label_width = 110;
var input_width = 240;



//**** 动态隐藏定义层 ****
//动态创建JS层
function CreateJs(table, triggerfield, field, presuffix) {
    var ret = "";
    ret += "<script language='javascript'>";
    ret += "  function " + table + "InitEvent(record){";
    //顺序触发事件
    $(triggerfield).each(function (index) {
        ret += "    InitCtrlEvent('" + triggerfield[index].fieldname + presuffix + "' + record);";
    });

    //还原默认值事件
    $(field).each(function (index) {
        //初始化特殊控件
        ret += "    InitSpecialCtrl('" + field[index].fieldname + presuffix + "' + record);";
        //还原默认值
        ret += "    RecoveryDefValue('" + field[index].fieldname + presuffix + "' + record);";
        //触发改变事件
        ret += "    InitCtrlEvent('" + field[index].fieldname + presuffix + "' + record);"; 
        //特殊情况,组号
        if (field[index].fieldname.toUpperCase() == "S_BY.ZH" || field[index].fieldname.toUpperCase() == "S_DJ_BY.ZH")
            ret += "    SetCtrlValue('" + field[index].fieldname + presuffix + "' + record, parseInt($('#" + table + "Record').val()));";
    });
    ret += "  }";
    ret += "</script>";
    return ret;
}

//动态创建层
function CreateDiv(table, hiddenfield, copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    ret += "<div id='" + table + "Div'>";
    ret += CreateField1(table, hiddenfield,copyhiddenfield, field, presuffix, suffix);
    ret += "</div>";
    return ret;
}

//动态创建表单
function CreateField(table, hiddenfield,copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    //td,tr之前的所有代码
    var ret_s = "";

    ret_s += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
    ret_s += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
    //隐藏字段
    $(hiddenfield).each(function (index) {
        ret_s += "<input type='hidden' kjlx='hidden' id='" + hiddenfield[index].fieldname + suffix + "' name='" + hiddenfield[index].fieldname + suffix + "' value='" + hiddenfield[index].defval + "'/>";
    });
    //隐藏复制字段
    $(copyhiddenfield).each(function (index) {
        ret_s += "<input type='hidden' kjlx='hidden' id='" + copyhiddenfield[index].fieldname + suffix + "' name='" + copyhiddenfield[index].fieldname + suffix + "' value=''/>";
    });
    var temp2 = document.getElementsByClassName("wrapper wrapper-content").item(0).offsetWidth;
    var td_width = temp2 / conditionNum;
    //占整行的数据
    var fieldRowStr = "";
    $(field).each(function (index) {
        fieldRowStr += "<tr>";
        fieldRowStr += "<td class=\"stj_lr2_table_td";
        if (field[index].fzyc)
            fieldRowStr += " hiddentd";
        fieldRowStr += "\">";
        fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:34px;\">" + field[index].sy + "</label>";
        fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";

        //判断类型
        switch (field[index].kjlx) {
            case "LABEL":
                fieldRowStr += CreateHtmlLabel(field[index], presuffix + suffix);
                break;
            case "TEXT":
                fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
                break;
            case "TEXTAREA":
                fieldRowStr += CreateHtmlTextarea(field[index], presuffix + suffix);
                break;
            case "TEXTBUTTON":
                fieldRowStr += CreateHtmlTextButton(field[index], presuffix + suffix);
                break;
            case "CHECKBOX":
                fieldRowStr += CreateHtmlCheckBox(field[index], presuffix + suffix);
                break;
            case "RADIO":
                fieldRowStr += CreateHtmlRadio(field[index], presuffix + suffix);
                break;
            case "SELECT":
                fieldRowStr += CreateHtmlSelect(field[index], presuffix + suffix);
                break;
            case "COMBOBOX":
                fieldRowStr += CreateHtmlCombobox(field[index], presuffix + suffix);
                break;
            case "FILE":
            case "FILELABEL":
                fieldRowStr += CreateHtmlFile(field[index], presuffix + suffix);
                break;
            //日期              
            case "DATE":
                fieldRowStr += CreateHtmlDate(field[index], presuffix + suffix);
                break;
            //bootstrap插件               
            case "BOOTSTRAPSELECT":
                fieldRowStr += CreateBootstrapSelect(field[index], presuffix + suffix);
                break;
            //bootstrap multiselect 
            case "BOOTSTRAPMULSELECT":
                fieldRowStr += CreateBootstrapMulSelect(field[index], presuffix + suffix);
                break;
            case "BOOTSTRAPSINGSELECT":
                fieldRowStr += CreateBootstrapSingleSelect(field[index], presuffix + suffix);
                break;
            default:
                fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
                break;
        }
        fieldRowStr += "    </div> </td>";
        fieldRowStr += "</tr>";
        //添加行记录
        ret += fieldRowStr;
        fieldRowStr = "";
    });

    //前div
    ret_s += "<div class=\"col-sm-3 stj_shu\">";
    ret_s += "  <table class=\"stj_lr2_table\">";
    ret_s = ret_s + ret + "</table></div>";

    return ret_s;
}

//创建描述信息
function ctrlMsginfo(msg) {
    var ret = "<div><font color='red'>" + SwitchNRToBr(msg) + "</font></div>";

    return ret;
}

//动态创建表单
function CreateField1(table, hiddenfield,copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    //td,tr之前的所有代码
    var ret_s = "";

    ret_s += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
    ret_s += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
    //隐藏字段
    $(hiddenfield).each(function (index) {
        ret_s += "<input type='hidden' id='" + hiddenfield[index].fieldname + presuffix + suffix + "' name='" + hiddenfield[index].fieldname + presuffix + suffix + "' value='" + hiddenfield[index].defval + "'/>";
    });
    //隐藏复制字段
    $(copyhiddenfield).each(function (index) {
        ret_s += "<input type='hidden' id='" + copyhiddenfield[index].fieldname + presuffix + suffix + "' name='" + copyhiddenfield[index].fieldname + presuffix + suffix + "' value=''/>";
    });

    var temp2 = document.getElementsByClassName("wrapper wrapper-content").item(0).offsetWidth;
    var td_width = temp2 / conditionNum;
    //占整行的数据
    var fieldRowStr = "";
    $(field).each(function (index) {
        fieldRowStr += "<tr>";
        fieldRowStr += "<td class=\"stj_lr2_table_td";
        if (field[index].fzyc)
            fieldRowStr += " hiddentd";
        fieldRowStr += "\">";
        fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:34px;\">" + field[index].sy + "</label>";
        fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";

        //判断类型
        switch (field[index].kjlx) {
            case "LABEL":
                fieldRowStr += CreateHtmlLabel(field[index], presuffix + suffix);
                break;
            case "TEXT":
                fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
                break;
            case "TEXTAREA":
                fieldRowStr += CreateHtmlTextarea(field[index], presuffix + suffix);
                break;
            case "TEXTBUTTON":
                fieldRowStr += CreateHtmlTextButton(field[index], presuffix + suffix);
                break;
            case "CHECKBOX":
                fieldRowStr += CreateHtmlCheckBox(field[index], presuffix + suffix);
                break;
            case "RADIO":
                fieldRowStr += CreateHtmlRadio(field[index], presuffix + suffix);
                break;
            case "SELECT":
                fieldRowStr += CreateHtmlSelect(field[index], presuffix + suffix);
                break;
            case "COMBOBOX":
                fieldRowStr += CreateHtmlCombobox(field[index], presuffix + suffix);
                break;
            case "FILE":
            case "FILELABEL":
                fieldRowStr += CreateHtmlFile(field[index], presuffix + suffix);
                break;
            //日期               
            case "DATE":
                fieldRowStr += CreateHtmlDate(field[index], presuffix + suffix);
                break;
            //bootstrap插件                
            case "BOOTSTRAPSELECT":
                fieldRowStr += CreateBootstrapSelect(field[index], presuffix + suffix);
                break;
            //bootstrap multiselect  
            case "BOOTSTRAPMULSELECT":
                fieldRowStr += CreateBootstrapMulSelect(field[index], presuffix + suffix);
                break;
            case "BOOTSTRAPSINGSELECT":
                fieldRowStr += CreateBootstrapSingleSelect(field[index], presuffix + suffix);
                break;
            default:
                fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
                break;
        }
        fieldRowStr += "    </div> </td>";
        fieldRowStr += "</tr>";
        //添加行记录
        ret += fieldRowStr;
        fieldRowStr = "";
    });

    //前div
    ret_s += "<div class=\"table_wai\">";
    ret_s += "  <table class=\"stj_lr2_table\">";
    ret_s = ret_s + ret + "</table></div>";

    return ret_s;
}
//**** 创建控件 ****
//** HTML控件 **
//标签框LABEL
function CreateHtmlLabel(ctrl, suffix) {
    var ret = "";
    ret += "<div type='text' class='form-control' custom='custom' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' sy = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' >";
    ret += "</div>";
    return ret;
}

//文本框TEXT
function CreateHtmlText(ctrl, suffix) {
    var ret = "";
    ret += "<input type='text' class='form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //判断只读
    if (ctrl.zdsx)
        ret += "readOnly='true' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    //判断是否包含ctrlstring
    var ctrlstring = "";
    //值对
    if (ctrl.ctrlstring.length > 0) {
        var keyParam;
        var typeList = ctrl.ctrlstring.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //改变其他控件ctrlChange
            if (keyParam.key == "ctrlChange") {
                //ctrlstring = ctrlstring + keyParam.value + "||";
                ctrlstring += GetSwitchCtrlString(keyParam.value, suffix) + "||";
            }
        }
        //去掉功能项后面的||
        if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
            ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
        //添加事件
        //ret += " onChange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');\" ";
    }
    //判断是否包含helplink
    if (ctrl.helplink.length > 0) {
        //值对
        var keyParam;
        var helplink = "";
        var typeList = ctrl.helplink.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "helplink") {
                helplink = keyParam.value;
                break;
            }
        }
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');ShowHelpLinkFormData('" + GetSwitchHelplink(helplink, suffix) + "');ChangeEnterToTab();\" ";
    }
    else {
        ret += "onkeydown=\"ChangeEnterToTab();\" ";
    }
    //检验类型
    var js = "";
    if (ctrl.validproc.length > 0) {
        //值对
        var keyParam;   
        var typeList = ctrl.validproc.split("|||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "js") {
                js = keyParam.value;
                break;
            }
        }
    }

    if (js != "") {
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');CtrlChange('" + ctrlstring + "');" + GetSwitchValidProc(js, suffix) + "\" ";
    }
    else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');CtrlChange('" + ctrlstring + "');\" ";

    //添加文本ctrlstring
    if (ctrlstring.length > 0)
        ret += "onchange=\"CtrlChange('" + ctrlstring + "');\" ";
    ret += "/>";
    //**特殊情况**
    if (!dataReadonly && ctrl.mustin && ctrl.helplink.length > 0) {
        //值对
        var keyParam;
        var helplink = "";
        var typeList = ctrl.helplink.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "helplink") {
                helplink = keyParam.value;
                break;
            }
        }
        ret += "<input type='button' value='选择' class='tonghang_btn_input' onclick=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\"/><font class='mustin'>*</font>";
    }
    else {
        //是否必输项
        if (!dataReadonly && ctrl.mustin)
            ret += "<font class='mustin'>*</font>";
        if (ctrl.helplink.length > 0) {
            //值对
            var keyParam;
            var helplink = "";
            var typeList = ctrl.helplink.split("||");
            for (var i = 0; i < typeList.length; i++) {
                keyParam = StrSplit(typeList[i], "--");
                //固定值
                if (keyParam.key == "helplink") {
                    helplink = keyParam.value;
                    break;
                }
            }
            ret += "<input type='button' class='tonghang_btn_input' value='选择' onclick=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" />";
        }
    }
//    //是否必输项
//    if (ctrl.mustin)
//        ret += "<font class='mustin'>*</font>";
//    if (ctrl.helplink.length > 0)
//        ret += "按F2选择";
//    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//文本按钮框TEXTBUTTON
function CreateHtmlTextButton(ctrl, suffix) {
    var ret = "";
    ret += "<input type='text' class='form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //判断只读
    if (ctrl.zdsx)
        ret += "readOnly='true' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    //判断是否包含helplink
    if (ctrl.helplink.length > 0) {
        //值对
        var keyParam;
        var helplink = "";
        var typeList = ctrl.helplink.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "helplink") {
                helplink = keyParam.value;
                break;
            }
        }
        ret += "onkeydown=\"ChangeEnterToTab();ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" ";
    }
    else
        ret += "onkeydown=\"ChangeEnterToTab();\" ";
    //检验类型
    if (ctrl.validproc.length > 0) {
        //值对
        var keyParam;
        var js = "";
        var typeList = ctrl.validproc.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "js") {
                js = keyParam.value;
                break;
            }
        }
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');" + js + "\" ";
    }
    else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');\" ";
    ret += "/>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //按钮
    if (ctrl.validproc.length > 0) {
        //值对
        var keyParam;
        var validproc = "";
        var typeList = ctrl.validproc.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "validproc") {
                validproc = keyParam.value;
                break;
            }
        }
        //分析值
        var buttonList = validproc.split("|");
        for (var i = 0; i < buttonList.length; i++) {
            var strFun = buttonList[i].split(",");
            ret += "<input type='button' value='" + strFun[0] + "' onclick=\"DynamicImplement('" + PointQuotes(strFun[1]) + "');\"/>";
        }
    }
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//复选框CHECKBOX
function CreateHtmlCheckBox(ctrl, suffix) {
    var ret = "";
    //复选框值
    var valueStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
            break;
        }
    }

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    
    //**** 添加隐藏Input ****
    var hiddenStr = "<div id='" + ctrl.fieldname + suffix + "HiddenDiv' style='display:none'>";
    hiddenStr += "<div class='checkbox checkbox-success checkbox-inline' style='margin: 0 5px 0 0;'>";

    hiddenStr += "<input type='checkbox' custom='&&custom&&' datain='datain' id='" + ctrl.fieldname + suffix + "&&iItem&&' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //控件类型
    hiddenStr += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //是否必输项
    if (ctrl.mustin)
        hiddenStr += "mustin='mustin' ";
    //默认值
    hiddenStr += "def='" + ctrl.defval + "' ";
    //值
    hiddenStr += "value='&&itemvalue&&' ";
    //只读
    if (ctrl.zdsx)
        hiddenStr += "onclick='return false;' ";
    hiddenStr += "onkeydown=\"ChangeEnterToTab();\" ";
    hiddenStr += "/>";
    //标签
    hiddenStr += "<label for='" + ctrl.fieldname + suffix + "&&iItem&&' style='padding-left: 2px;'>&&itemmc&&</label>";
    hiddenStr += "</div>"
    hiddenStr += "</div>";
    //**** ****
    //其他隐藏域添加
    //判断ID是否已经存在
    //if ($("#" + ctrl.fieldname + "HiddenDiv").size() == 0)
    //$("#hiddenControlDiv").append(hiddenStr);
    ret += hiddenStr;
    //**** ****
    
    ret += "<div id='" + ctrl.fieldname + suffix + "Div' class='mustinContent'>";
    //**** 添加隐藏Input ****
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //
        //添加元素
        ret += "<div class='checkbox checkbox-success checkbox-inline' style='margin: 0 5px 0 0;'>";

        ret += "<input type='checkbox' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + iItem + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
        //控件类型
        ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
        //是否必输项
        if (ctrl.mustin)
            ret += "mustin='mustin' ";
        //默认值
        ret += "def='" + ctrl.defval + "' ";
        //值
        ret += "value='" + itemList[1] + "' ";
        //只读
        if (ctrl.zdsx)
            ret += "onclick='return false;' ";
        //判断是否选中
        if (itemList[2] == "1")
            ret += "checked='checked' ";
        else if (ctrl.defval == itemList[1])
            ret += "checked='checked' ";
        ret += "onkeydown=\"ChangeEnterToTab();\" ";
        ret += "/>";
        //标签
        ret += "<label for='" + ctrl.fieldname + suffix + iItem + "' style='padding-left: 2px;'>" + itemList[0] + "" + "</label>";
        ret += "</div>"
    }
    ret += "</div>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//单选框RADIO
function CreateHtmlRadio(ctrl, suffix) {
    var ret = "";
    //单选框值
    var valueStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
            break;
        }
    }

    //分析每一项
    ret += "<div class='mustinContent'>";
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加元素
        ret += "<div class='radio radio-info radio-inline' style='margin: 0 5px 0 0;'>";
        ret += "<input type='radio' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + iItem + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
        //控件类型
        ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
        //是否必输项
        if (ctrl.mustin)
            ret += "mustin='mustin' ";
        //默认值
        ret += "def='" + ctrl.defval + "' ";
        //值
        ret += "value='" + itemList[1] + "' ";
        //判断是否选中
        if (itemList[2] == "1")
            ret += "checked='checked' ";
        else if (ctrl.defval == itemList[1])
            ret += "checked='checked' ";
        ret += "onkeydown=\"ChangeEnterToTab();\" ";
        ret += "/>";
        ret += "<label for='" + ctrl.fieldname + suffix + iItem + "' style='padding-left: 2px;'>" + itemList[0] + "</label>";
        ret += "</div>"
    }
    ret += "</div>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//下拉框SELECT
function CreateHtmlSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select class='form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";

    //下拉框值
    var valueStr = "";
    var ctrlstring = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
        //改变其他控件ctrlChange
        else if (keyParam.key == "ctrlChange") {
            ctrlstring += GetSwitchCtrlString(keyParam.value, suffix) + "||";
        }
        else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2);
    //去掉功能项后面的||
    if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
        ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + ctrlstring + "');\"";
    }
    ret += "onkeydown=\"ChangeEnterToTab();\" ";
    ret += ">";

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
        else if (ctrl.defval == itemList[1]) {
            ret += " selected='selected' ";
        }
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//下拉框输入框
function CreateHtmlCombobox(ctrl, suffix) {
    var ret = "";
    ret += "<div style='position:relative;'>";

    ret += "<input type='text' class='form-control mustinContent shuang-top-input' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px;position:absolute;left:0px;' ";
    else
        ret += "style='position:absolute;left:0px;' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //判断只读
    if (ctrl.zdsx)
        ret += "readOnly='true' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    ret += "/>";
    //隐藏下拉框
    ret += "<span style='width:18px;overflow:hidden;'>";
    ret += "<select class='form-control mustinContent' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + (ctrl.xscd + 32) + "px' ";
    //控件ID及名称
    ret += " id='" + ctrl.fieldname + suffix + "_combobox' name = '" + ctrl.fieldname + suffix + "_combobox' ";
    //控件信息
    ret += "kjlx='COMBOBOX_TMP' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";

    //下拉框值
    var valueStr = "";
    var ctrlstring = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
            break;
        }
    }

    //改变事件
    ret += " onchange='this.parentNode.previousSibling.value=this.value' ";
    ret += ">";

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    //遍历节点项,判断是否默认值包含在选项中
    var itemContent = false;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        if (ctrl.defval == itemList[1]) {
            itemContent = true;
            break;
        }
    }
    //不包含在下拉框中
    if (!itemContent) {
        ret += "<option value='" + ctrl.defval + "' selected='selected'>" + ctrl.defval + "</option>";
    }
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "' ";
        //默认值在节点中
        if (itemContent && ctrl.defval == itemList[1]) {
            ret += " selected='selected' ";
        }
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    ret += "</span>";   

    ret += "</div>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//大文本框TEXTAREA
function CreateHtmlTextarea(ctrl, suffix) {
    var ret = "";
    ret += "<textarea class='form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //行数
    ret += "rows=5 ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //判断只读
    if (ctrl.zdsx)
        ret += "readOnly='true' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    ret += ">";
    ret += "</textarea>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//文本框TEXT
function CreateHtmlDate(ctrl, suffix) {
    var ret = "";
    ret += "<input type='text' class='Wdate form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='width:" + ctrl.xscd + "px' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";

    ret += "onFocus='WdatePicker({isShowClear:false,readOnly:true})' ";
    //检验类型
    if (ctrl.validproc.length > 0) {
        //值对
        var keyParam;
        var js = "";
        var typeList = ctrl.validproc.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "js") {
                js = keyParam.value;
                break;
            }
        }
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');" + GetSwitchValidProc(js, suffix) + "\" ";
    }
    else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');\" ";
    ret += "/>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//bootstrap插件控件
//多选下拉
function CreateBootstrapSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select data-placeholder='请选择' class='chosen-select form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    /* style='width:350px;'*/
    ret += "multiple tabindex='0' ";
    //下拉框值
    var valueStr = "";
    var ctrlstring = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
        //改变其他控件ctrlChange
        else if (keyParam.key == "ctrlChange") {
            ctrlstring = keyParam.value;
        }
        else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');ChangeEnterToTab();\"";
    }
    ret += ">";

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "' hassubinfo='true'>" + itemList[0] + "</option>";
    }
    ret += "</select>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin1'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//下拉过滤
function CreateBootstrapMulSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' multiple='multiple' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    /* style='width:350px;'*/
    ret += "multiple tabindex='0' ";
    //下拉框值
    var valueStr = "";
    var ctrlstring = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
        //改变其他控件ctrlChange
        else if (keyParam.key == "ctrlChange") {
            ctrlstring = keyParam.value;
        }
        else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');\"";
    }
    ret += ">";

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "'>" + itemList[0] + "</option>";
    }
    ret += "</select>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin1'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//下拉过滤
function CreateBootstrapSingleSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    /* style='width:350px;'*/
    ret += " tabindex='0' ";
    //下拉框值
    var valueStr = "";
    var ctrlstring = "";
    var funStr = "";
    //值对
    var keyParam;
    var typeList = ctrl.ctrlstring.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        //固定值
        if (keyParam.key == "value") {
            valueStr = keyParam.value;
        }
        //改变其他控件ctrlChange
        else if (keyParam.key == "ctrlChange") {
            ctrlstring = keyParam.value;
        }
        else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');\"";
    }
    ret += ">";

    //分析每一项
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加Select元素
        ret += "<option value='" + itemList[1] + "'>" + itemList[0] + "</option>";
    }
    ret += "</select>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin1'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//特殊控件
//** **
//文件框FILE
function CreateHtmlFile(ctrl, suffix) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&'  mc = '" + ctrl.sy + "' "
    if (ctrl.mustin) {
        ret += "mustin='mustin' ";
    }
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' def=''";
    ret += "id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' value='100'/>";
    ret += "<div style='width:120px;' type='file' id='" + PointBlankRep(ctrl.fieldname) + suffix + "' name = '" + PointBlankRep(ctrl.fieldname) + suffix + "'></div>";
    //ret += "<input type='file' id='" + PointBlankRep(ctrl.fieldname) + suffix + "' name = '" + PointBlankRep(ctrl.fieldname) + suffix + "'/>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin_file'>*</font>";
    ret += "<div id='FILEFIELD_DIV_" + PointBlankRep(ctrl.fieldname) + suffix + "' class='wfa_all_div'></div>";
    //    //对应JS语句
    //    ret += "<script language='javascript' type='text/javascript'>";
    //    ret += "    function UPLOADFILE_" + PointBlankRep(ctrl.fieldname) + suffix + "(){";
    //    ret += "        $('#" + PointBlankRep(ctrl.fieldname) + suffix + "').uploadify({";
    //    ret += "            'swf':'/skins/DataInput/pub/uploadify/uploadify.swf',";
    //    ret += "            'uploader':'/DataInput/FileService?method=UploadFile',";
    //    ret += "            'cancelImg':'/skins/DataInput/pub/uploadify/uploadify-cancel.png',";
    //    ret += "            'buttonText':'请选择文件',";
    //    ret += "            'fileTypeExts':'*.*',";
    //    ret += "            'fileTypeDesc':'有效文档',";
    //    ret += "            'fileSizeLimit':'2048000KB',";
    //    //ret += "            'width':'',";
    //    ret += "            'height':'25',";
    //    ret += "            'onSelect':function (file) {";
    //    ret += "                this.addPostParam(\"file_name\", encodeURIComponent(file.name));";
    //    ret += "            },";
    //    ret += "            'onUploadSuccess': function (file, data, response) {";
    //    ret += "                var val = $.parseJSON(data);";
    //    ret += "                if(!val.success){";
    //    ret += "                    alert(val.msg);";
    //    ret += "                    return;";
    //    ret += "                }";
    //    ret += "                var filevalue = $(\"#" + PointFileRep(ctrl.fieldname) + suffix + "\").val();";
    //    ret += "                filevalue += val.fileid + \",\" + val.filename + \"|\";";
    //    ret += "                $(\"#" + PointFileRep(ctrl.fieldname) + suffix + "\").val(filevalue);"; 
    //    //控件名，当前选择的文件名及上传后返回的文件ID
    //    ret += "                showUpFile(\"" + PointBlankRep(ctrl.fieldname) + "\", val.fileid,val.filename);";                         
    //    ret += "            },";
    //    ret += "            'onUploadError': function (file, errorCode, errorMsg, errorString) {"
    //    ret += "                alert(file.name + '上传失败，' + errorString + '，请稍后再试');";
    //    ret += "            }";
    //    ret += "        });";
    //    ret += "     }";
    //    ret += "</script>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font color='red'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}



//**** 界面 ****
//** 从表 **
//从表Tab项模板
function GetT2Tab(data, table, sy, subsy, hasDetail, detail, detailsy) {
    //修改申请隐藏按钮
    var xgsqNewHidden = "";
    var xgsqDelHidden = "";
    var xgsqStr = "false";
    if (data.sham) {
        //修改申请是否允许添加记录
        if (data.shnew != "1")
            xgsqNewHidden = "style=\"display :none\"";
        //修改申请是否允许删除记录
        if (data.shdel != "1")
            xgsqDelHidden = "style=\"display :none\"";
        xgsqStr = "true";
    }
    var html = "";
    //添加锚点
    html += "<input type=\"hidden\" id=\"" + table + "Record\" name=\"" + table + "Record\" value=\"0\"/>";
    html += "<div class=\"col-sm-9 animated fadeInRight stj_shu\" id=\"table_right\" style=\"background-color:#fff;\">";
    html += "  <div class=\"panel panel-success\">";
    html += "    <div class=\"panel-heading\">";
    html += "          <h3 class=\"panel-title\">" + sy + "</h3>";
    html += "    </div>";

    html += "    <div class=\"panel-body stj_lr2_mianban\">";

    html += "       <div style='min-height:38px;' class=\"stj_lr2_mianban_title\">";
    html += "           <button " + xgsqNewHidden + " class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\"  onclick=\"AddT2Page('" + table + "','" + sy + "','" + subsy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "'," + xgsqStr + ",'" + data.shdel + "');\"><i class=\"fa fa-plus\"></i>&nbsp;添加</button>";
    html += "           <button " + xgsqNewHidden + " class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\"   onclick=\"CopyT2Page('" + table + "','" + sy + "','" + subsy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "'," + xgsqStr + ",'" + data.shdel + "');\"><i class=\"fa fa-copy\"></i>&nbsp;复制</button>";
    html += "           <button " + xgsqNewHidden + " class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyNumsBtn\" name=\"" + table + "CopyNumsBtn\"   onclick=\"CopyNumsT2Page('" + table + "','" + sy + "','" + subsy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "'," + xgsqStr + ",'" + data.shdel + "');\"><i class=\"fa fa-copy\"></i>&nbsp;复制多组</button>";
    html += "           <div class=\"btn_right\">";

    $(data.custombutton).each(function (index) {
        html += "<button class=\"btn btn-primary stj_ti\" type=\"button\"    name='" + data.custombutton[index].name + "' value='" + data.custombutton[index].title + "' onclick='" + data.custombutton[index].event + "(\"" + data.custombutton[index].url + "\");' ><i class=\"" + data.custombutton[index].css + "\" ></i>" + data.custombutton[index].title + "</button>";
    });

    $(data.btndata).each(function (index) {
        html += "<button class=\"btn btn-primary stj_ti\" type=\"button\"    name='" + data.btndata[index].name + "' value='" + data.btndata[index].title + "' onclick='" + data.btndata[index].event + "(\"" + data.btndata[index].url + "\");' ><i class=\"" + data.btndata[index].css + "\" ></i>" + data.btndata[index].title + "</button>";
    });

   

    html += "       </div>";
    html += "       </div>";
    html += "       <div class=\"panel-body\">";
    html += "           <div class=\"tabs-container\">";
    html += "               <div class='tab_control' id=\"" + table + "Nav\">";
    html += " <div class=\"scrollfir display_no\" onclick='scrollfir_click()'>";
    html += " <i class='fa fa-fast-backward '></i>";
    html += "  </div>";
    html += " <div class=\"scrollLeft display_no\" onclick='scrollLeft_click()'>";
    html += "<i class='fa fa-backward '></i>";
    html += "</div>";


    html += "  <div class='scrollmid'><ul class=\"nav nav-tabs stj_nav tab_control_ul \"></ul></div>";
    html += "  <div class=\"scrollRight display_no\" onclick='scrollRight_click()'>";
    html += " <i class='fa fa-forward '></i>";
    html += " </div>";
    html += " <div class=\"scrolllast display_no\"  onclick='scrolllast_click()'>";
    html += "<i class='fa fa-fast-forward '></i>";
    html += "  </div>";

    html += "               </div>";
    html += "               <div id=\"" + table + "Tab\" class=\"tab-content\"></div>";
    html += "           </div>";
    html += "       </div>";

    html += "    </div>";

//    html += "    <div class=\"panel-body stj_lr2_mianban\">";

//    html += "       <div class=\"stj_lr2_mianban_title\">";
//    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\"  onclick=\"AddT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"><i class=\"fa fa-plus\"></i>&nbsp;添加</button>";
//    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\"   onclick=\"CopyT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"><i class=\"fa fa-copy\"></i>&nbsp;复制</button>";
//    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyNumsBtn\" name=\"" + table + "CopyNumsBtn\"   onclick=\"CopyNumsT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"><i class=\"fa fa-copy\"></i>&nbsp;复制多组</button>";
//    html += "       </div>";

//    html += "       <div class=\"panel-body\">";
//    html += "           <div class=\"tabs-container\">";
//    html += "               <div id=\"" + table + "Nav\">";
//    html += "                   <ul class=\"nav nav-tabs stj_nav\"></ul>";
//    html += "               </div>";
//    html += "               <div id=\"" + table + "Tab\" class=\"tab-content\"></div>";
//    html += "           </div>";
//    html += "       </div>";

//    html += "    </div>";

    html += "</div>";
    html += "</div>";
    return html;
}

//添加Tab项
//table:表名
//sy:释义
//hasDetail:是否有明细表
//detail:明细表名
//rec:当前从表记录数
//sham:虚假保存
function AddT2Page(table, sy, subsy, hasDetail, detail, detailsy, sham, shdel) {
    //layer.load(2AddT2Page 
    //清除class
    width_ul_zong = $(".scrollmid").width(); //ul总长度，
    if ($(".scrollfir").hasClass("display_no")) { $(".scrollfir").removeClass("display_no"); }
    if ($(".scrollLeft").hasClass("display_no")) { $(".scrollLeft").removeClass("display_no"); }
    if ($(".scrollRight").hasClass("display_no")) { $(".scrollRight").removeClass("display_no"); }
    if ($(".scrolllast").hasClass("display_no")) { $(".scrolllast").removeClass("display_no"); }


    $("#" + table + "Nav ul li").each(function (index) {
        $(this).removeClass("active");
    });

    $("#" + table + "Tab div").each(function (index) {
        $(this).removeClass("active");
    });

    //当前记录数
    var record = parseInt($("#" + table + "Record").val()) + 1;
    //定义添加的tab标签
    var lab = "";
    //lab += "<li class='active' mc='" + table + "Tab_" + record + "Item' >";
    //lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true' style='background-color:transparent;border-width:0px;'>" + sy + record + "</a>";
    //lab += "    <a  onclick=\"DelT2Page('" + table + "'," + record + ");\" onfocus=\"this.blur();\" style='padding: 0px;border-width: 0px;  background-color:transparent;'><i class='fa fa-close'></i></a>";
    //lab += "</li>";
    lab += "<li class='active tab_control_li' mc='" + table + "Tab_" + record + "Item' >";
    lab += " <div class='tab_control_font'>   <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true' style='background-color:transparent;border-width:0px;'>" + subsy + record + "</a> </div>";
    //修改申请，不显示删除
    if(!(sham && shdel != "1"))
      lab += " <div class='tab_control_a'>   <a  onclick=\"DelT2Page('" + table + "'," + record + ");\" onfocus=\"this.blur();\"><i class='fa fa-times-circle'></i></a></div>";
    
    lab += "</li>";
    $("#" + table + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "Tab-" + record + "' mc='" + table + "Tab_" + record + "Content' class='tab-pane stj_lr2_neimianban active'>";
    content += "    <div class='panel-body' style='border-width:0px; background-color:#f0f7fd'>";
    //字段内容
    var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    var fieldcontent = $("#" + table + "Div").html();
    fieldcontent = fieldcontent.replace(regField, record);
    //替换数据属性
    var dataField = new RegExp("&amp;&amp;datain&amp;&amp;", "g");
    fieldcontent = fieldcontent.replace(dataField, "datain");
    //赋值字符串
    content += fieldcontent;
    content += "    </div>";
    content += "</div>";
    $("#" + table + "Tab").append(content);
    //记录数
    $("#" + table + "Record").val(record);
    //初始化函数
    eval(table + "InitEvent('" + record + "');");
    //layer.closeAll('loading');
}

//复制Tab项
//sham:修改申请
//shdel:修改申请时,是否允许删除
function CopyT2Page(table, sy, subsy, hasDetail, detail, detailsy, sham, shdel) {
    var titleTip = "";
    var copyStatus = false;
    //判断是否还有当前选中项
    $("#" + table + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active")) {
            copyStatus = true;
            titleTip = $("a", $(this)).html();
        }
    });
    //如果有复制项
    if (copyStatus) {
        //询问框
        layer.confirm('您确定要复制【' + titleTip + '】吗？', {
            btn: ['确定', '取消'] //按钮
        }, function (index) {
            //新建标签页
            AddT2Page(table, sy, subsy, hasDetail, detail, detailsy, sham, shdel);
            //获取当前最大标签数
            var record = parseInt($("#" + table + "Record").val());
            //获取当前选中项数
            //var regS = new RegExp(sy, "g");
            var regS = new RegExp(subsy, "g");
            var recordCopy = titleTip.replace(regS, ""); 
            //还原对象字段集
            var filedListObject = JSON.parse(fieldArray[table]);
            //复制记录
            $(filedListObject).each(function (index) {
                SetCtrlValue(filedListObject[index].fieldname + "_" + record, GetCtrlValue(filedListObject[index].fieldname + "_" + recordCopy));
                //触发改变事件
                InitCtrlEvent(filedListObject[index].fieldname + "_" + record);
            });
            //关闭
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });
    }
}

//复制多组Tab项
function CopyNumsT2Page(table, sy, subsy, hasDetail, detail, detailsy, sham, shdel) {
    var titleTip = "";
    var copyStatus = false;
    //判断是否还有当前选中项
    $("#" + table + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active")) {
            copyStatus = true;
            titleTip = $("a", $(this)).html();
        }
    });
    //如果有复制项
    if (copyStatus) {
        //询问框
        layer.prompt({ title: '请输入复制【' + titleTip + '】数量？', formType: 0, value: '1' }, function (num, index) {
            //判断是否为正整数
            if (!isPositiveNum(num)) {
                alert("内容【" + num + "】为无效整数！");
                return;
            }

            var copyNum = parseInt(num);
            for (var i = 1; i <= copyNum; i++) {
                //新建标签页
                AddT2Page(table, sy, subsy, hasDetail, detail, detailsy, sham, shdel);
                //获取当前最大标签数
                var record = parseInt($("#" + table + "Record").val());
                //获取当前选中项数
                //var regS = new RegExp(sy, "g");
                var regS = new RegExp(subsy, "g");
                var recordCopy = titleTip.replace(regS, "");
                //还原对象字段集
                var filedListObject = JSON.parse(fieldArray[table]);
                //复制记录
                $(filedListObject).each(function (index) {
                    SetCtrlValue(filedListObject[index].fieldname + "_" + record, GetCtrlValue(filedListObject[index].fieldname + "_" + recordCopy));
                    //触发改变事件
                    InitCtrlEvent(filedListObject[index].fieldname + "_" + record);
                });
            }
            //关闭
            layer.close(index);
        });
    }
}
//判断是否为正整数
function isPositiveNum(s) {//是否为正整数  
    var re = /^[0-9]*[1-9][0-9]*$/;
    return re.test(s)
}

//删除Tab项
function DelT2Page(table, record) {
    //layer.load(2);
    var titleTip = "";
    var delStatus = false;
    //判断是否还有当前选中项
    $("#" + table + "Nav ul li").each(function (index) {
        //if ($(this).hasClass("active")) {
        if ($(this).attr("mc") == table + "Tab_" + record + "Item") {
            delStatus = true;
            titleTip = $("a", $(this)).html();
        }
    });
    //如果有删除项
    if (delStatus) {
        //询问框
        layer.confirm('您确定要删除【' + titleTip + '】吗？', {
            btn: ['确定', '取消'] //按钮
        }, function (index) {
            //删除当前选中的标签项
            $("#" + table + "Nav ul li").each(function (index) {
                //if ($(this).hasClass("active"))
                if ($(this).attr("mc") == table + "Tab_" + record + "Item")
                    $(this).remove();
            });
            //删除当前选中的内容项
            $("#" + table + "Tab div").each(function (index) {
                //if ($(this).hasClass("tab-pane stj_lr2_neimianban active"))
                if ($(this).attr("mc") == table + "Tab_" + record + "Content")
                    $(this).remove();
            });
            //当前记录数减1
            //$("#" + table + "Record").val(parseInt($("#" + table + "Record").val()) - 1);
            //清除class
            $("#" + table + "Nav ul li").each(function (index) {
                $(this).removeClass("active");
            });

            $("#" + table + "Tab div").each(function (index) {
                $(this).removeClass("active");
            });
            //激活第一个标签项
            $("#" + table + "Nav ul li:first-child").addClass("active");
            //激活第一个内容项
            $("#" + table + "Tab div:first-child").addClass("active");
            //关闭
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });
    }
    //layer.closeAll('loading');
}

//第一Tab项
function FirstT2Page(table) {
    //清除class
    $("#" + table + "Nav ul li").each(function (index) {
        $(this).removeClass("active");
    });

    $("#" + table + "Tab div").each(function (index) {
        $(this).removeClass("active");
    });
    //激活第一个标签项
    $("#" + table + "Nav ul li:first-child").addClass("active");
    //激活第一个内容项
    $("#" + table + "Tab div:first-child").addClass("active");
}

//*************************************文件*****************************************
//构建文件函数
function UploadFileInit(fieldname) {
    //文件属性
    //对象存储类型
    var localStorageType = storagetype;
    //缩放类型
    var localScaleType = "";
    //宽度
    var localWidth = "";
    //高度
    var localHeight = "";
    //文件大小
    var localSize = "20480KB";
    var kjfzcs = GetCtrlKjfzcs(fieldname);
    if (kjfzcs != "") {
        var keyParam;
        var typeList = kjfzcs.split("|");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "-");
            //存储类型
            if (keyParam.key == "storagetype")
                localStorageType = keyParam.value;
            //缩放类型
            else if (keyParam.key == "scaletype")
                localScaleType = keyParam.value;
            //宽度
            else if (keyParam.key == "imgwidth")
                localWidth = keyParam.value;
            //高度
            else if (keyParam.key == "imgheight")
                localHeight = keyParam.value;
            //文件大小
            else if (keyParam.key == "size")
                localSize = keyParam.value;
        }
    }

    $('#' + PointBlankRep(fieldname)).Huploadify({
        auto: true,
        multi: false,
        fileObjName: 'Filedata',
        fileSizeLimit: 10240,
        buttonText: '请选择文件',

        'fileTypeExts': '*.*',
        'fileTypeDesc': '有效文档',

        'uploader': '/DataInput/FileService?method=UploadFile&type=' + localStorageType + '&scaletype=' + localScaleType + '&imgwidth=' + localWidth + '&imgheight=' + localHeight,
        'onSelect': function (file) {
            this.addPostParam("file_name", encodeURIComponent(file.name));
        },

        onUploadSuccess: function (file, data) {
            var val = $.parseJSON(data);
            if (!val.success) {
                alert(val.msg);
                return;
            }
            var filevalue = $("#" + PointRep(fieldname)).val();
            filevalue += val.fileid + "," + val.filename + "|";
            $("#" + PointRep(fieldname)).val(filevalue); ;
            //控件名，当前选择的文件名及上传后返回的文件ID
            showUpFile(PointBlankRep(fieldname), val.fileid, val.filename);
        },
        'onUploadError': function (file, errorCode, errorMsg, errorString) {
            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
        }
    });

//    $('#' + PointBlankRep(fieldname)).uploadify({
//        'swf': '/skins/DataInput/pub/uploadify/uploadify.swf',
//        'uploader': '/DataInput/FileService?method=UploadFile&type=' + storagetype,
//        'cancelImg': '/skins/DataInput/pub/uploadify/uploadify-cancel.png',
//        'buttonText': '请选择文件',
//        'fileTypeExts': '*.*',
//        'fileTypeDesc': '有效文档',
//        'fileSizeLimit': '2048000KB',
//        //'width':'',
//        'height': '25',
//        'onSelect': function (file) {
//            this.addPostParam("file_name", encodeURIComponent(file.name));
//        },
//        'onUploadSuccess': function (file, data, response) {
//            var val = $.parseJSON(data);
//            if (!val.success) {
//                alert(val.msg);
//                return;
//            }
//            var filevalue = $("#" + PointRep(fieldname)).val();
//            filevalue += val.fileid + "," + val.filename + "|";
//            $("#" + PointRep(fieldname)).val(filevalue); ;
//            //控件名，当前选择的文件名及上传后返回的文件ID
//            showUpFile(PointBlankRep(fieldname), val.fileid, val.filename);
//        },
//        'onUploadError': function (file, errorCode, errorMsg, errorString) {
//            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
//        }
//    });
}

// 展示上传文件（文件控件ID，选择的文件名及后台返回的文件ID号）
function showUpFile(ctrlid, fileid, filename) {
    try {
        var divid = "FILEFIELD_DIV_" + ctrlid;
        var tail = (new Date()).getTime();
        var content = $("#" + divid).html();
        var newcontent = "<div class='wfa_frame_div' id='file_div_" + fileid + "'><span class='wfa_text'><a href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>" + filename + "</a></span>";
        newcontent += "<div class='wfa_image'><a title='删除文件' id='FILEFIELD_FILE_DEL_" + ctrlid + "_" + tail + "' href='javascript:delFile(\"" + ctrlid + "\",\"" + fileid + "\",\"" + filename + "\")'><img src='/skins/DataInput/images/wfa_close1.png' name='" + fileid + "_pic' width='18' height='18' id='" + fileid + "_pic' border='0' onmouseover='wfa_swapImage(\"" + fileid + "_pic\",\"\",\"/skins/DataInput/images/wfa_close2.png\",1)' onmouseout='wfa_swapImgRestore()'  /></a></div>";
        newcontent += "</div>";
        $("#" + divid).html(content + newcontent);
    }
    catch (err) {
        alert(err);
        //$.messager.alert('提示', err, 'info');
    }
}

//只查看文件
function showUpFileView(ctrlid, fileid, filename) {
    try {
        var divid = "FILEFIELD_DIV_" + ctrlid
        var tail = (new Date()).getTime();
        var content = $("#" + divid).html();
        var newcontent = "<div class='wfa_frame_div' id='file_div_" + fileid + "'><span class='wfa_text'><a href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>" + filename + "</a></span>";
        newcontent += "<div class='wfa_image'><img src='/skins/DataInput/images/wfa_close1.png' name='" + fileid + "_pic' width='DataInputt='18' id='" + fileid + "_pic' border='0'/></div>";
        newcontent += "</div>";
        $("#" + divid).html(content + newcontent);
    }
    catch (err) {
        alert(err);
        //$.messager.alert('提示', err, 'info');
    }
}

// 删除上传文件展示的div
function delUpFileDiv(fileid) {
    var fileid = fileid.replace(/\./g, "\\.");
    $("#file_div_" + fileid).remove();
}

// 删除上传的文件
function delFile(ctrlid, fileid, filename) {
    try {
        if (confirm("确定要删除文件：" + filename + "吗？")) {
            //$.messager.confirm("文件删除提示", "确定要删除文件：" + filename + "吗？", function (b) {
            //if (b) {
            //                    $.ajax({
            //                        type: "POST",
            //                        url: "/DataEntry/DeleteFile",
            //                        data: "fileid=" + fileid,
            //                        dataType: "json",
            //                        success: function (val) {
            //                            //判断是否成功
            //                            if (!val.success) {
            //                                $.messager.alert('提示', '删除文件失败，错误信息：' + val.msg, 'info');
            //                                return;
            //                            }
            //                            var hidecontent = $("#" + ctrlid).val().replace(fileid + "," + filename + "|", "");
            //                            $("#" + ctrlid).val(hidecontent);
            //                            delUpFileDiv(fileid);
            //                        },
            //                        complete: function (XMLHttpRequest, textStatus) {
            //                        },
            //                        beforeSend: function (XMLHttpRequest) {
            //                        }
            //                    });
            var ctrlidname = ctrlid.replace("___", "\\.");
            var hidecontent = $("#" + ctrlidname).val().replace(fileid + "," + filename + "|", "");
            $("#" + ctrlidname).val(hidecontent);
            delUpFileDiv(fileid);
        }
        //});
    }
    catch (err) {
        alert(err);
        //$.messager.alert('提示', err, 'info');
    }
}

function wfa_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}

function wfa_findObj(n, d) { //v4.01
    var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = wfa_findObj(n, d.layers[i].document);
    if (!x && d.getElementById) x = d.getElementById(n); return x;
}

function wfa_swapImage() { //v3.0
    var i, j = 0, x, a = wfa_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
        if ((x = wfa_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
}
//*************************************文件****************************************

//*************************************从表滚动开始************************************
var first_li = 0   ///当前第一个li的序列
var last_li = 0;    ///当前最后一个li的序列
var width_ul_zong //ul总长度，
var left_position = new Array();
var zongchangdu = 75; //li的宽度值，目前固定~

function scrollRight_click() {
    move_right();
    $(".tab_control_ul").css("left", -1 * first_li * zongchangdu);
}

function scrollLeft_click() {
    move_left();
    $(".tab_control_ul").css("left", -1 * first_li * zongchangdu);
}

function scrollfir_click() {
    move_fir();
    $(".tab_control_ul").css("left", -1 * first_li * zongchangdu);
}

function scrolllast_click() {
    move_last();
    $(".tab_control_ul").css("left", -1 * first_li * zongchangdu);
}

function move_right() {
    var total_num = 0;   //显示总长度+临近的一个
    var total_num_before = 0;  ///显示总长度
    // alert(last_li);
    for (i = first_li; i < $(".tab_control_li").length; i++) {
        var value_temp = $(".tab_control_li").eq(i).outerWidth(true);
        total_num_before = total_num;
        total_num = total_num + value_temp;
        if (total_num > width_ul_zong) {
            total_num = total_num_before

            first_li = i;

            var total_num_2 = 0;   //显示总长度+临近的一个
            var total_num_before_2 = 0;  ///显示总长度

            for (j = first_li; j < $(".tab_control_li").length; j++) {
                var value_temp_2 = $(".tab_control_li").eq(j).outerWidth(true);
                total_num_before_2 = total_num_2;
                total_num_2 = total_num_2 + value_temp_2;
                if (total_num_2 > width_ul_zong) {
                    last_li = j - 1;
                    // alert(last_li + "\n" + first_li);
                    break;
                }

            }
            break;
        }
    }
}

function move_left() {
    if (first_li == 0) {
    }
    else {
        last_li = first_li - 1;
        var total_before_3 = 0;
        var total_num_3 = 0;   //显示总长度+临近的一个
        var total_num_before_3 = 0;  ///显示总长度
        // alert(first_li);
        for (k = last_li; k >= 0; k--) {
            var value_temp_3 = $(".tab_control_li").eq(k).outerWidth(true);
            total_num_before_3 = total_num_3;
            total_num_3 = total_num_3 - value_temp_3;
            if (total_num_3 <= total_before_3 - width_ul_zong) { first_li = k + 1; break }
            else if (total_num_3 > total_before_3 - width_ul_zong && k == 0) {
                first_li = 0; break;
            }
        }
    }
}

function move_fir() {
    first_li = 0;
    var total_num_2 = 0;   //显示总长度+临近的一个
    var total_num_before_2 = 0;  ///显示总长度

    for (j = first_li; j < $(".tab_control_li").length; j++) {
        var value_temp_2 = $(".tab_control_li").eq(j).outerWidth(true);
        total_num_before_2 = total_num_2;
        total_num_2 = total_num_2 + value_temp_2;
        if (total_num_2 > width_ul_zong) { last_li = j - 1; break; }
    }
}

function move_last() {
    last_li = $(".tab_control_li").length - 1;
    var total_before_3 = last_li * zongchangdu;
    var total_num_3 = last_li * zongchangdu;
    var total_num_before_3 = 0;  ///显示总长度
    // alert(first_li);
    for (k = last_li; k >= 0; k--) {
        var value_temp_3 = $(".tab_control_li").eq(k).outerWidth(true);
        total_num_before_3 = total_num_3;
        total_num_3 = total_num_3 - value_temp_3;
        if (total_num_3 <= total_before_3 - width_ul_zong) { first_li = k + 1; break }
        else if (total_num_3 > total_before_3 - width_ul_zong && k == 0) {
            first_li = 0; break;
        }
    }
}
//*************************************从表滚动结束************************************
