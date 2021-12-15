var all_k = 350;
var label_width = 110;
var input_width = 240;


//**** 动态隐藏定义层 ****
function ctrlstring(str) {
    var obj = {};
    var ary = str.split('||');
    var tmp, key, val, list, m;
    for (var i = 0, len = ary.length; i < len; i++) {
        tmp = ary[i].split('--');
        if (tmp.length) {

            key = tmp.shift();
            val = tmp.join().split('|');
            list = [];
            if (key == 'value') {
                for (var item of val) {
                    m = item.split(',');
                    if (m.length == 3) {
                        list.push({
                            name: m[0],
                            key: m[1],
                            selected: m[2] == 1 ? true : false
                        });
                    }
                }
            } else if (key == 'ctrlChange') {
                list = val;

            }
            obj[key] = list;
        }
    }
    console.log(obj);
    return obj;
}
//创建描述信息
function ctrlMsginfo(msg) {
    msg = "<div class='zhushi'>" + msg + "</div>";
    return msg;
}

//**** 创建控件 ****
//** HTML控件 **

function CreateHtmlSelect2(ctrl) {
    var str = '<div class="select-type" ><input readOnly="readOnly" name="' + ctrl.fieldname + '" ';


    //是否必输项
    if (ctrl.mustin) {
        str += "mustin='mustin' ";
    }
    str += ' /><ul class="select-ul">';

    var obj = ctrlstring(ctrl.ctrlstring);
    //下拉框
    if (obj.value && obj.value.length) {
        for (var i = 0; i < obj.value.length; i++) {
            str += "<li value='" + obj.value[i].key + "'>" + obj.value[i].name + "</li>";
        }
    }

    str += '</ul></div>';
    return str;

}

function CreateHtmlText2(ctrl) {
    var ret = "";
    ret += "<input type='text'   name = '" + ctrl.fieldname + "' ";
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\" ";
    } 
    // else {
    //     ret += "onkeydown=\"ChangeEnterToTab(event);\" ";
    // }
    ret += "/>";

    return ret;
}

//标签框LABEL
function CreateHtmlLabel(ctrl) {
    var ret = "";
    ret += "<input type='text' readOnly='readOnly' class='form-control background_tr no_xing' custom='custom' name = '" + ctrl.fieldname + "' sy = '" + ctrl.sy + "' ";

    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin'";
    ret += " />";
    return ret;
}


//文本框TEXT
function CreateHtmlText(ctrl) {
    var ret = "";
    ret += "<input type='text' class='form-control mustinContent";
    if (!dataReadonly && ctrl.helplink.length > 0) {
        ret += " tonghang_text_input";
    }
    ret += "' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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
                //ctrlstring = keyParam.value;
                ctrlstring += GetSwitchCtrlString(keyParam.value) + "||";
            }
        }
        //去掉功能项后面的||
        if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
            ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\" ";
    } 
    // else {
    //     ret += "onkeydown=\"ChangeEnterToTab(event);\" ";
    // }

    //检验类型
    var js = "";
    if (ctrl.validproc.length > 0) {
        //值对
        var keyParam;
        var typeList = ctrl.validproc.split("||");
        for (var i = 0; i < typeList.length; i++) {
            keyParam = StrSplit(typeList[i], "--");
            //固定值
            if (keyParam.key == "js") {
                js = keyParam.value;
                break;
            }
        }
    }

    if (js != "")
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');CtrlChange('" + ctrlstring + "',event);" + GetSwitchValidProc(js) + "\" ";
    else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');CtrlChange('" + ctrlstring + "',event);\" ";
    //添加文本ctrlstring
    if (ctrlstring.length > 0)
        ret += "onchange=\"CtrlChange('" + ctrlstring + "',event);\" ";
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
        ret += "<input type='button' value='选择' class='tonghang_btn_input' onclick=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\"/><font class='mustin'>*</font>";
    } else {
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
            ret += "<input type='button' class='tonghang_btn_input' value='选择' onclick=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\" />";
        }
    }
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//文本按钮框TEXTBUTTON
function CreateHtmlTextButton(ctrl) {
    var ret = "";
    ret += "<input type='text' class='form-control mustinContent' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\" ";
    }
    //检验类型
    ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');\" ";
    ret += "/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
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
            ret += "<input class='sousuo_btn_k_r' type='button' value='" + strFun[0] + "' onclick=\"DynamicImplement('" + PointQuotes(strFun[1]) + "',event);\"/>";
        }
    }
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//文本按钮框TEXTBUTTON2
function CreateHtmlTextButton2(ctrl) {
    var ret = "";
    ret += "<input type='text' class='form-control mustinContent' custom='custom' datain='&&datain&&' name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

    //是否查看
    if (dataReadonly)
        ret += "readonly='readonly' ";
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink) + "',event);\" ";
    }
    //检验类型
    ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');\" ";
    ret += "/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
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
            ret += "<input class='sousuo_btn_k_r' type='button' value='" + strFun[0] + "' onclick=\"DynamicImplement('" + PointQuotes(strFun[1]) + "',event);\"/>";
        }
    }
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//文本按钮框TEXTIMAGEBUTTON
function CreateHtmlTextImageButton(ctrl) {
    var ret = "";
    ret += "<input type='hidden'  class='form-control mustinContent' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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

    ret += "<img  ";

    ret += "src='data:image/png;base64,";
    ret += "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";
    ret += "' />";

    //是否必输项
    if (!dataReadonly && ctrl.mustin)
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
            ret += "<input class='sousuo_btn_k_r' type='button' value='" + strFun[0] + "' onclick=\"DynamicImplement('" + PointQuotes(strFun[1]) + "',event);\"/>";
        }
    }
    return ret;
}

//图形文本
function CreateHtmlTextImage(ctrl) {
    var ret = "";
    ret += "<input type='hidden' class='form-control mustinContent";
    ret += "' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";
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
    ret += "<img  class='form-control mustinContent' ";

    ret += "src='data:image/png;base64,";
    ret += "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";

    ret += "' />";
    return ret;
}

//复选框CHECKBOX
function CreateHtmlCheckBox(ctrl) {
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
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加元素
        ret += "<label><div class='checkbox checkbox-success checkbox-inline' style='margin: 0 5px 0 0;'>";
        ret += "<input type='checkbox' custom='custom' datain='&&datain&&' name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";
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
        ret += "/>";
        //标签
        ret +=  itemList[0] + "" + "</label>";
        ret += "</div>"
    }
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//单选框RADIO
function CreateHtmlRadio(ctrl) {
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
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加元素
        ret += "<div class='radio radio-info radio-inline'>";
        ret += "<label><input type='radio' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";
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
        ret += "/>";
        ret +=   itemList[0] + "</label>";
        ret += "</div>"
    }
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//下拉框SELECT
function CreateHtmlSelect(ctrl) {
    var ret = "";
    ret += "<select class='form-control mustinContent' custom='custom' datain='&&datain&&' name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";
    //判断只读
    if (ctrl.zdsx)
        ret += "onfocus='this.defaultIndex=this.selectedIndex;' onchange='this.selectedIndex=this.defaultIndex;'";
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
            ctrlstring += GetSwitchCtrlString(keyParam.value) + "||";
        } else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //去掉功能项后面的||
    if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
        ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + ctrlstring + "',event);\"";
    }
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
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}



//可输入下拉框
function CreateHtmlCombobox(ctrl) {
    var ret = "";
    ret += "<div class='combox' style='position:relative;'>";
    ret += "<input type='text' class='form-control mustinContent shuang-top-input' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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

    var obj = ctrlstring(ctrl.ctrlstring);
    //下拉框
    ret += "<ul class='box-ul'>";
    if (obj.value && obj.value.length) {
        for (var i = 0; i < obj.value.length; i++) {
            ret += "<li value='" + obj.value[i].key + "'>" + obj.value[i].name + "</li>";
        }
    }
    ret += "</ul>";

    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);

    ret += "</div>"
    return ret;
}

//大文本框TEXTAREA
function CreateHtmlTextarea(ctrl) {
    var ret = "";
    ret += "<textarea class='form-control mustinContent' custom='custom' datain='&&datain&&' name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//日期DATE
function CreateHtmlDate(ctrl) {
    var ret = "";
    ret += "<input type='text' class='Wdate form-control mustinContent' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

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
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');" + GetSwitchValidProc(js) + "\" ";
    } else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');\" ";
    ret += "/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//日期DATETIME
function CreateHtmlDateTime(ctrl) {
    var ret = "";
    ret += "<input type='text' class='Wdate form-control mustinContent' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";

    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' ";
    //默认值
    ret += "def='" + ctrl.defval + "' ";
    //是否必输项
    if (ctrl.mustin)
        ret += "mustin='mustin' ";

    ret += "onFocus='WdatePicker({isShowClear:false,readOnly:true,dateFmt:\"yyyy-MM-dd HH:mm:ss\"})' ";
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
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');" + GetSwitchValidProc(js) + "\" ";
    } else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + "');\" ";
    ret += "/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//bootstrap插件控件
//多选下拉
function CreateBootstrapSelect(ctrl) {
    var ret = "";
    ret += "<select data-placeholder='请选择' class='chosen-select form-control mustinContent' custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' ";
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
        } else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring) + "',event);\"";
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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//多选下拉过滤
function CreateBootstrapMulSelect(ctrl) {
    var ret = "";
    ret += "<select custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "' multiple='multiple' ";
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
        } else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring) + "',event);\"";
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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}


//单选下拉过滤
function CreateBootstrapSingleSelect(ctrl) {
    var ret = "";
    ret += "<select custom='custom' datain='&&datain&&'  name = '" + ctrl.fieldname + "' mc = '" + ctrl.sy + "'  ";
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
        } else {
            funStr += typeList[i] + "||";
        }
    }
    //去掉功能项后面的||
    if (funStr.length > 0 && funStr.charAt(funStr.length - 1) == '|' && funStr.charAt(funStr.length - 2) == '|')
        funStr = funStr.substring(0, funStr.length - 2)
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring) + "',event);\"";
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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//特殊控件
//** **
//文件框FILE
function CreateHtmlFile(ctrl) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&'  mc = '" + ctrl.sy + "' "
    if (!dataReadonly && ctrl.mustin) {
        ret += "mustin='mustin' ";
    }
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' def=''";
    ret += " name = '" + ctrl.fieldname + "' value='100'/>";
    //是否必输项

    if (!dataReadonly)
        ret += "<div style='width:120px;' type='file'  name = '" + PointBlankRep(ctrl.fieldname) + "></div>";
        //ret += "<input type='file'  name = '" + PointBlankRep(ctrl.fieldname) + "'/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin_file'>*</font>";
    ret += "<div class='wfa_all_div'></div>";

    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}


//图片文件IMAGEFILE
function CreateHtmlImageFile(ctrl) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&'  mc = '" + ctrl.sy + "' "
    if (!dataReadonly && ctrl.mustin) {
        ret += "mustin='mustin' ";
    }
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' def=''";
    ret += "name = '" + ctrl.fieldname + "' value='100'/>";
    //是否必输项

    if (!dataReadonly)
        ret += "<input type='file' name = '" + PointBlankRep(ctrl.fieldname) + "'/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin_file'>*</font>";
    ret += "<div id='FILEFIELD_DIV_" + PointBlankRep(ctrl.fieldname) + "' class='wfa_all_div'></div>";



    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
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
//        'uploader': '/DataInput/FileService?method=UploadFile',
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
//            $("#" + PointRep(fieldname)).val(filevalue);
//            //控件名，当前选择的文件名及上传后返回的文件ID
//            showUpFile(PointBlankRep(fieldname), val.fileid, val.filename);
//        },
//        'onUploadError': function (file, errorCode, errorMsg, errorString) {
//            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
//        }
//    });
}

//构建图片文件函数
function UploadImageFileInit(fieldname) {
    $('#' + PointBlankRep(fieldname)).uploadify({
        'swf': '/skins/DataInput/pub/uploadify/uploadify.swf',
        'uploader': '/DataInput/FileService?method=UploadFile',
        'cancelImg': '/skins/DataInput/pub/uploadify/uploadify-cancel.png',
        'buttonText': '请选择文件',
        'fileTypeExts': '*.jpg;*.jpeg;*.png;*.bmp',
        'fileTypeDesc': '有效文档',
        'fileSizeLimit': '5012KB',
        //'width':'',
        'height': '25',
        'onSelect': function (file) {
            this.addPostParam("file_name", encodeURIComponent(file.name));
        },
        'onUploadSuccess': function (file, data, response) {
            var val = $.parseJSON(data);
            if (!val.success) {
                alert(val.msg);
                return;
            }
            var filevalue = $("#" + PointRep(fieldname)).val();
            filevalue += val.fileid + "," + val.filename + "|";
            $("#" + PointRep(fieldname)).val(filevalue);;
            //控件名，当前选择的文件名及上传后返回的文件ID
            showUpFile(PointBlankRep(fieldname), val.fileid, val.filename);
        },
        'onUploadError': function (file, errorCode, errorMsg, errorString) {
            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
        },
        'onSelectError': function (file, errorCode, errorMsg) {
            switch (errorCode) {
                case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED:
                    //this.queueData.errorMsg = "每次最多上传 " + this.settings.queueSizeLimit + "个文件";  
                    msgText += "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
                    break;
                case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                    msgText += "文件大小超过限制( " + this.settings.fileSizeLimit + " )";
                    break;
                case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                    msgText += "文件大小为0";
                    break;
                case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                    msgText += "文件格式不正确，仅限 " + this.settings.fileTypeExts;
                    break;
                default:
                    msgText += "错误代码：" + errorCode + "\n" + errorMsg;
            }
            alert(msgText);
        }
    });
}


// 展示图片

function showImageFile(fileid, url) {
    //var viewer = new Viewer(document.getElementById('#' + id));
    $('#imgcontent').html("<img id='" + fileid + "' height='100px' width='100px' src='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "'/>");
    $('#' + fileid).viewer({
        hidden: function () {
            $('#imgcontent').html("");
        },
        shown: function () {

        }
    });
    $('#' + fileid).click();
}


function showImgFile(url) {
    layer.open({
        type: 2,
        title: '图片',
        area: ['50%', '80%'],
        //content: '<img src="' + url + '"/>'
        content: url
    });
}

// 展示上传文件（文件控件ID，选择的文件名及后台返回的文件ID号）
function showUpFile(ctrlid, fileid, filename) {
    try {
        var divid = "FILEFIELD_DIV_" + ctrlid;
        var tail = (new Date()).getTime();
        var content = $("#" + divid).html();
        var newcontent = "";
        //判断是否为图片
        if (isImage(filename.toLowerCase())) {
            if (dataReadonly)
                newcontent = "<div class='wfa_frame_div2_image' ";
            else
                newcontent = "<div class='wfa_frame_div_image' ";
        } else {
            if (dataReadonly)
                newcontent = "<div class='wfa_frame_div2' ";
            else
                newcontent = "<div class='wfa_frame_div' ";
        }
        newcontent = newcontent + "id='file_div_" + fileid + "'><span class='wfa_text'>";
        //判断是否为图片
        if (isImage(filename.toLowerCase())) {

            newcontent = newcontent + "<a onclick=\"showImageFile('" + fileid + "',event)\" id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>";
            newcontent = newcontent + "<img height='100px' width='100px' src='/DataInput/FileService?method=DownloadFile&type=small&fileid=" + fileid + "'/>";
        } else {
            newcontent = newcontent + "<a href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>";
            newcontent = newcontent + filename;
        }
        newcontent = newcontent + "</a>";
        //newcontent = newcontent + "<a target='_blank' href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "'>打印</a>";
        newcontent = newcontent + "</span>";
        //只读时不显示删除图标
        if (!dataReadonly)
            newcontent += "<div class='wfa_image'><a title='删除文件' id='FILEFIELD_FILE_DEL_" + ctrlid + "_" + tail + "' href='javascript:delFile(\"" + ctrlid + "\",\"" + fileid + "\",\"" + filename + "\")'><img src='/skins/DataInput/images/wfa_close1.png' name='" + fileid + "_pic' width='18' height='18' id='" + fileid + "_pic' border='0' onmouseover='wfa_swapImage(\"" + fileid + "_pic\",\"\",\"/skins/DataInput/images/wfa_close2.png\",1)' onmouseout='wfa_swapImgRestore()'  /></a></div>";
        if (isImage(filename.toLowerCase()))
            newcontent = newcontent + "<a class='print_a' target='_blank' href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "'>打印</a>";
        newcontent += "</div>";
        $("#" + divid).html(content + newcontent);
    } catch (err) {
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
        var newcontent = "";
        if (dataReadonly)
            newcontent = "<div class='wfa_frame_div2' ";
        else
            newcontent = "<div class='wfa_frame_div' ";
        newcontent = newcontent + "id='file_div_" + fileid + "'><span class='wfa_text'><a href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>" + filename + "";
        //判断是否为图片
        if (isImage(filename.toLowerCase())) {
            newcontent = newcontent + "&nbsp;&nbsp;&nbsp;<img height='100px' width='100px' src='/DataInput/FileService?method=DownloadFile&type=small&fileid=" + fileid + "'/>";
        }
        newcontent = newcontent + "</a></span>";
        newcontent += "<div class='wfa_image'><img src='/skins/DataInput/images/wfa_close1.png' name='" + fileid + "_pic' width='DataInputt='18' id='" + fileid + "_pic' border='0'/></div>";
        newcontent += "</div>";
        $("#" + divid).html(content + newcontent);
    } catch (err) {
        alert(err);
        //$.messager.alert('提示', err, 'info');
    }
}

//判断图片
function isImage(filename) {
    var ret = false;
    if (filename.indexOf(".jpg") != -1 || filename.indexOf(".jpeg") != -1 || filename.indexOf(".bmp") != -1 || filename.indexOf(".png") != -1 || filename.indexOf(".gif") != -1)
        ret = true;
    return ret;
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
            var ctrlidname = ctrlid.replace("___", "\\.");
            var hidecontent = $("#" + ctrlidname).val().replace(fileid + "," + filename + "|", "");
            $("#" + ctrlidname).val(hidecontent);
            delUpFileDiv(fileid);
        }
    } catch (err) {
        alert(err);
    }
}

function wfa_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr;
    for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}

function wfa_findObj(n, d) { //v4.01
    var p, i, x;
    if (!d) d = document;
    if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document;
        n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all) x = d.all[n];
    for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = wfa_findObj(n, d.layers[i].document);
    if (!x && d.getElementById) x = d.getElementById(n);
    return x;
}

function wfa_swapImage() { //v3.0
    var i, j = 0,
        x, a = wfa_swapImage.arguments;
    document.MM_sr = new Array;
    for (i = 0; i < (a.length - 2); i += 3)
        if ((x = wfa_findObj(a[i])) != null) {
            document.MM_sr[j++] = x;
            if (!x.oSrc) x.oSrc = x.src;
            x.src = a[i + 2];
        }
}
//*************************************文件****************************************