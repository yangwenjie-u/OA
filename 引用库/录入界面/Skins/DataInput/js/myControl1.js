//**** 全局变量 ****
var conditionNum = 4;

//******************************//
var all_k = 350;
var label_width = 110;
var input_width = 240;

//**** 其他事件 ****
//锚点跳转
function JumpHref(url) {
    location.hash = url;
}

//动态执行
function DynamicImplement(str) {
    eval(str);
}

//'替换为\'
function PointQuotes(str) {
    return str.replace(/'/g, "\\\'");
}

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

//.替换成空
function PointBlankRep(str) {
    return str.replace(".", "___");
}

//.替换
function PointFileRep(str) {
    return str.replace(".", "\\\\.");
}
//$替换'
function ParamRep(str) {
    return str.replace(/\$/g, "'");
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

////序列化
//jQuery.prototype.serializeObject = function () {
//    var obj = new Object();
//    $.each(this.serializeArray(), function (index, param) {
//        if (!(param.name in obj)) {
//            obj[param.name] = param.value;
//        }
//    });
//    return obj;
//};

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
    });
    ret += "  }";
    ret += "</script>";
    return ret;
}

//动态创建层
function CreateDiv(table, hiddenfield, field, presuffix, suffix) {
    var ret = "";
    ret += "<div id='" + table + "Div'>";
    ret += CreateField(table, hiddenfield, field, presuffix, suffix);
    ret += "</div>";
    return ret;
}

//动态创建表单
function CreateField(table,hiddenfield, field, presuffix,suffix) {
    //当前动态生成的控件排号
    var ctrlNum = 0;
    var ret = "";
    //td,tr之前的所有代码
    var ret_s = "";
    //表格的宽度
    var table_width;
    //判断是否有大行的存在
    var dayg_live = false;

    ret_s += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
    ret_s += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
    //隐藏字段
    $(hiddenfield).each(function (index) {
        ret_s += "<input type='hidden' id='" + hiddenfield[index].fieldname + suffix + "' name='" + hiddenfield[index].fieldname + suffix + "' value='" + hiddenfield[index].defval + "'/>"; 
    });

    /*
    if(field.length>conditionNum)
    {
    ret_s += "<table class='stj_lr_tab' style='width:"+(conditionNum*450)+"px' >";
    table_width=conditionNum*450;
    //alert(field.length);
    }
    else
    {
		
    ret_s += "<table class='stj_lr_tab' style='width:"+(field.length*450)+"px' >";
    table_width=field.length*450;
    }
	
    */



    var temp2 = document.getElementsByClassName("wrapper wrapper-content animated fadeInRight").item(0).offsetWidth;
    var td_width = temp2 / conditionNum;
    //占整行的数据
    var fieldRowStr = "";
    $(field).each(function (index) {
        //判断是否占一行
        if (field[index].sfgd) {
            dayg_live = true;
            fieldRowStr += "<tr>";
            if (conditionNum == 4) {
                //style='width:" + temp2 + "px'
                if (field[index].sy.length > 8) {
                    fieldRowStr += "<td colspan='" + conditionNum + "' class='yyy' > <label class='col-sm-3 control-label stj_label' style='line-height:15px;'>" + field[index].sy + "</label>";
                }
                else {
                    fieldRowStr += "<td colspan='" + conditionNum + "' class='yyy' > <label class='col-sm-3 control-label stj_label' style='line-height:34px;'>" + field[index].sy + "</label>";
                }


            }
            else {
                //style='width:" + temp2 + "px'
                if (field[index].sy.length > 8)
                { fieldRowStr += "<td colspan='" + conditionNum + "' class='stj_lr_td' > <label class='col-sm-3 control-label stj_label'style='line-height:15px;'>" + field[index].sy + "</label>"; }
                else
                { fieldRowStr += "<td colspan='" + conditionNum + "' class='stj_lr_td' > <label class='col-sm-3 control-label stj_label'style='line-height:34px;'>" + field[index].sy + "</label>"; }


            }


            if ((field.length - 1) > conditionNum) {
                table_width = conditionNum * 350 - 110 + 5 - 56.5;


            }
            else {
                table_width = (field.length - 1) * 350 - 110 + 5 - 56.5;

            }
            fieldRowStr += "<div class='dayg' style='width:" + table_width + "px '>";


            /*
            if(table_width>$("#contentDiv").width())
            { fieldRowStr += "<div class='dayg' style='width:"+$("#contentDiv").width()+"px '>";
				
            }
            else
            {
            fieldRowStr += "<div class='dayg' style='width:"+table_width+"px '>";
            }
            */
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
                default:
                    fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
                    break;
            }
            fieldRowStr += "</div> </td>";
            fieldRowStr += "</tr>";
            //判断是否是第一个控件
            if (ctrlNum == 0) {
                //添加占整行数据行
                ret += fieldRowStr;
                fieldRowStr = "";
            }
            //判断是否到达最后一个控件
            else if (index == field.length - 1) {
                if (ctrlNum > 0) {
                    //判断是否需要填充
                    var blankNum = conditionNum - ctrlNum;
                    while (blankNum > 0) {
                        //style='width:" + td_width + "px'
                        ret += "<td  class='stj_lr_td' style='width:0px;padding:0px;margin:0px ' > <label class='col-sm-3 control-label stj_label' style='width:0px;padding:0px;margin:0px '></label>";
                        ret += "<div class='col-sm-9 date stj_input' style='width:0px;padding:0px;margin:0px ' ></div> </td>";

                        blankNum = blankNum - 1;
                    }
                    ret += "</tr>";
                }
                //添加占整行数据行
                ret += fieldRowStr;
                fieldRowStr = "";
            }
        }
        else {
            //每个控件
            ctrlNum = ctrlNum + 1;
            if (ctrlNum == 1)
                ret += "<tr>";
            //style='width:" + td_width + "px'
            if (field[index].sy.length > 8) {
                ret += "<td class='stj_lr_td' > <label class='col-sm-3 control-label stj_label' style='line-height:15px;'>" + field[index].sy + "</label>";
            }
            else {
                ret += "<td class='stj_lr_td' > <label class='col-sm-3 control-label stj_label'  style='line-height:34px;'>" + field[index].sy + "</label>";
            }

            ret += "<div class='col-sm-9 date stj_input'>";

            //判断类型
            switch (field[index].kjlx) {
                case "LABEL":
                    ret += CreateHtmlLabel(field[index], presuffix + suffix);
                    break;
                case "TEXT":
                    ret += CreateHtmlText(field[index], presuffix + suffix);
                    break;
                case "TEXTAREA":
                    fieldRowStr += CreateHtmlTextarea(field[index], presuffix + suffix);
                    break;
                case "TEXTBUTTON":
                    ret += CreateHtmlTextButton(field[index], presuffix + suffix);
                    break;
                case "CHECKBOX":
                    ret += CreateHtmlCheckBox(field[index], presuffix + suffix);
                    break;
                case "RADIO":
                    ret += CreateHtmlRadio(field[index], presuffix + suffix);
                    break;
                case "SELECT":
                    ret += CreateHtmlSelect(field[index], presuffix + suffix);
                    break;
                case "FILE":
                case "FILELABEL":
                    ret += CreateHtmlFile(field[index], presuffix + suffix);
                    break;
                //日期             
                case "DATE":
                    ret += CreateHtmlDate(field[index], presuffix + suffix);
                    break;
                //bootstrap插件              
                case "BOOTSTRAPSELECT":
                    ret += CreateBootstrapSelect(field[index], presuffix + suffix);
                    break;
                default:
                    ret += CreateHtmlText(field[index], presuffix + suffix);
                    break;
            }
            ret += "</div> </td>"; 
            //判断是否已经到最后一列中
            if (ctrlNum == conditionNum) {
                ret += "</tr>";
                //当前列数清0
                ctrlNum = 0;
                //添加占整行数据行
                ret += fieldRowStr;
                fieldRowStr = "";
            }
            //判断是否到达最后一个控件
            else if (index == field.length - 1) {
                //判断是否需要填充
                var blankNum = conditionNum - ctrlNum;
                while (blankNum > 0) {
                    //style='width:" + td_width + "px'
                    ret += "<td  class='stj_lr_td' style='width:0px;padding:0px;margin:0px ' > <label class='col-sm-3 control-label stj_label' style='width:0px;padding:0px;margin:0px '></label>";
                    ret += "<div class='col-sm-9 date stj_input' style='width:0px;padding:0px;margin:0px ' ></div> </td>";

                    blankNum = blankNum - 1;
                }
                ret += "</tr>";
                //添加占整行数据行
                ret += fieldRowStr;
                fieldRowStr = "";
            }
        }
    });

    if (dayg_live == false) {
        if (field.length > conditionNum) {
            ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:" + (conditionNum * 350) + "px' >";
            //table_width=conditionNum*450;
            //alert(field.length);
        }
        else {

            ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:" + (field.length * 350) + "px' >";
            // table_width=field.length*450;
        }

    }
    else {
        if ((field.length - 1) > conditionNum) {
            ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:" + (conditionNum * 350) + "px' >";
            //table_width=conditionNum*450;
            //alert(field.length);
        }
        else {

            ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:" + ((field.length - 1) * 350) + "px' >";
            // table_width=field.length*450;
        }


    }

    ret_s = ret_s + ret + "</table> </div>";

    // ret += "</table>";
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" ";
    }
    //检验类型
    ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');\" ";
    ret += "/>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" ";
    }
    //检验类型
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
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加元素
        ret += "<div class='checkbox checkbox-success checkbox-inline' style='margin: 0 5px 0 0;'>";
        ret += "<input type='checkbox' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
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
        ret += "<label for='inlineCheckbox2' style='padding-left: 2px;'>" + itemList[0] + "" + "</label>";
        ret += "</div>"
    }
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
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
    var items = valueStr.split("|");
    var itemList;
    for (var iItem = 0; iItem < items.length; iItem++) {
        itemList = items[iItem].split(",");
        //添加元素
        ret += "<div class='radio radio-info radio-inline' style='margin: 0 5px 0 0;'>";
        ret += "<input type='radio' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
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
        ret += "<label for=\"inlineRadio1\" style='padding-left: 2px;'>" + itemList[0] + "</label>";   
        ret += "</div>"
    }
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
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
        ret += "<option value='" + itemList[1] + "' ";
        if (itemList[2] == "1") {
            ret += " selected='selected' ";
        }
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    return ret;
}

//大文本框TEXTAREA
function CreateHtmlTextarea(ctrl, suffix) {
    var ret = "";
    ret += "<textarea class='form-control mustinContent' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
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
    ret += ">";
    ret += "</textarea>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
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
    ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');\" ";
    ret += "/>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
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
        ret += "onchange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');\"";
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
    return ret;
}


//特殊控件
//** **
//文件框FILE
function CreateHtmlFile(ctrl, suffix) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&' "
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' def=''";
    ret += "id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' value='100'/>";
    ret += "<div style='width:120px;' type='file' id='" + PointBlankRep(ctrl.fieldname) + suffix + "' name = '" + PointBlankRep(ctrl.fieldname) + suffix + "'></div>";
	//ret += "<input type='file' id='" + PointBlankRep(ctrl.fieldname) + suffix + "' name = '" + PointBlankRep(ctrl.fieldname) + suffix + "'/>";
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
    return ret;
}


//******************控件后缀转换*********************
//CtrlString转换
function GetSwitchCtrlString(ctrlstring, suffix) {
    var newCtrlString = "";
    newCtrlString += "ctrlChange--";
    //值对
    var keyParam;
    var ctrlStringList = ctrlstring.split("|");
    for (var i = 0; i < ctrlStringList.length; i++) {
        keyParam = StrSplit(ctrlStringList[i], "-");
        //目标控件加后缀
        if (keyParam.key == "targetctrl") {
            newCtrlString += "targetctrl-";
            var targetCtrlList = keyParam.value.split(",");
            for (var j = 0; j < targetCtrlList.length; j++) {
                newCtrlString += targetCtrlList[j] + suffix + ",";
            }
            //去掉功能项后面的,
            if (newCtrlString.length > 0 && newCtrlString.charAt(newCtrlString.length - 1) == ',')
                newCtrlString = newCtrlString.substring(0, newCtrlString.length - 1);
            newCtrlString += '|';
        }
        //条件控件加后缀
        else if (keyParam.key == "wherectrl") {
            newCtrlString += "wherectrl-";
            var whereCtrlList = keyParam.value.split(",");
            for (var j = 0; j < whereCtrlList.length; j++) {
                newCtrlString += whereCtrlList[j] + suffix + ",";
            }
            //去掉功能项后面的,
            if (newCtrlString.length > 0 && newCtrlString.charAt(newCtrlString.length - 1) == ',')
                newCtrlString = newCtrlString.substring(0, newCtrlString.length - 1);
            newCtrlString += '|';
        }
        else {
            newCtrlString += ctrlStringList[i] + '|';
        }
    }
    //去掉功能项后面的,
    if (newCtrlString.length > 0 && newCtrlString.charAt(newCtrlString.length - 1) == '|')
        newCtrlString = newCtrlString.substring(0, newCtrlString.length - 1);
    //返回
    return newCtrlString;
}

//helplink转换
function GetSwitchHelplink(helplink,suffix){
    var newHelplink = "";
    newHelplink += "helplink--";
     //值对
    var keyParam;
    var helplinkList = helplink.split("|");
    for (var i = 0; i < helplinkList.length; i++) {
        keyParam = StrSplit(helplinkList[i], "-");
        //目标控件加后缀
        if (keyParam.key == "targetctrl") {
            newHelplink += "targetctrl-";
            var targetCtrlList = keyParam.value.split(",");
            for (var j = 0; j < targetCtrlList.length; j++) {
                newHelplink += targetCtrlList[j] + suffix + ",";
            }
            //去掉功能项后面的,
            if (newHelplink.length > 0 && newHelplink.charAt(newHelplink.length - 1) == ',')
                newHelplink = newHelplink.substring(0, newHelplink.length - 1);
            newHelplink += '|';
        }
        //条件控件加后缀
        else if (keyParam.key == "wherectrl") {
            newHelplink += "wherectrl-";
            var whereCtrlList = keyParam.value.split(",");
            for (var j = 0; j < whereCtrlList.length; j++) {
                newHelplink += whereCtrlList[j] + suffix + ",";
            }
            //去掉功能项后面的,
            if (newHelplink.length > 0 && newHelplink.charAt(newHelplink.length - 1) == ',')
                newHelplink = newHelplink.substring(0, newHelplink.length - 1);
            newHelplink += '|';
        }
        else {
            newHelplink += helplinkList[i] + '|';
        }
    }

    //返回
    return newHelplink;
}
//***************************************************

//**** 界面 ****
//** 从表 **
//从表Tab项模板
function GetT2Tab(table, sy, hasDetail, detail, detailsy) {
    //var regS = new RegExp("&amp;&amp;table&amp;&amp;", "g");
    //var html = $("#hiddenTemplateDiv").html();
    //html = html.replace(regS, table);
    var html = "";
    //添加锚点
    html += "<a id=\"" + table + "Anchor\" name=\"" + table + "Anchor\"></a>";
    html += "<input type=\"hidden\" id=\"" + table + "Record\" name=\"" + table + "Record\" value=\"0\"/>";
    html += "<div class=\"tabs-container\">";
    html += "   <div class=\"add_del_stj\">"
    /*
	
    html += "       <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\" value='添加' onclick=\"AddT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\" />";
    html += "       <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\" value='复制' onclick=\"CopyT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\" />";
    html += "       <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "DelBtn\" name=\"" + table + "DelBtn\" value='删除' onclick=\"DelT2Page('" + table + "');\" />";
    */

    html += "<div class=\"ryxx\" > 人员信2息</div>";
    html += "<div class='btn-group'>";
    html += " <button type=\"button\" class=\"btn btn-default\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\"  onclick=\"AddT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"><i class=\"fa fa-plus\"></i>添加</button>";
    html += " <button type=\"button\" class=\"btn btn-default\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\"   onclick=\"CopyT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"> <i class='fa fa-copy'> </i>复制</button>";
    /*	html +=" <button style='display:none' type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "DelBtn\" name=\"" + table + "DelBtn\"  onclick=\"DelT2Page('" + table + "');\" >删除</button>";
    */



    /*
    <div class="btn-group">
    <button type="button" class="btn btn-default">按钮 1</button>
    <button type="button" class="btn btn-default">按钮 2</button>
    <button type="button" class="btn btn-default">按钮 3</button>
    */
    html += "</div>";







    html += "   </div>"
    html += "   <div id=\"" + table + "Nav\">";
    html += "       <ul class=\"nav nav-tabs stj_nav\"></ul>";
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
function AddT2Page(table, sy, hasDetail, detail, detailsy) {
    //layer.load(2); 
    //清除class
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
    //style='border-width:0px'
    lab += "<li class='active' >";
    //lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true'>" + sy + record + "<a onclick='$(\"#" + table + "DelBtn\").click()'>*</a></a>";
    lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true' style='background-color:transparent;border-width:0px;'>" + sy + record + "</a>";
    //</a>
    lab += "<a  onclick='alert(1)' onfocus=\"this.blur();\" style='padding: 0px;border-width: 0px;  background-color:transparent;'><i class='fa fa-close'></i></a>";
    lab += "</li>";
    $("#" + table + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "Tab-" + record + "' class='tab-pane active'>";
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
    //判断是否包含明细表项
    //****************************************
    if (hasDetail)
        content += GetT3Tab(table, detail, detailsy, record);
    //****************************************
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
function CopyT2Page(table, sy, hasDetail, detail, detailsy) {
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
            AddT2Page(table, sy, hasDetail, detail, detailsy);
            //获取当前最大标签数
            var record = parseInt($("#" + table + "Record").val());
            //获取当前选中项数
            var regS = new RegExp(sy, "g");
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

//删除Tab项
function DelT2Page(table) {
    //layer.load(2);
    var titleTip = "";
    var delStatus = false;
    //判断是否还有当前选中项
    $("#" + table + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active")) {
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
            //关闭
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });     
    }
    //layer.closeAll('loading');
}

//** 明细 **
//从表Tab项模板
function GetT3Tab(tablab, table, sy, rec) {
    var html = "";
    //当前Tab项记录数
    //添加锚点
    html += "<a id=\"" + table + "Anchor\" name=\"" + table + "Anchor\"></a>";
    html += "<input type=\"hidden\" id=\"" + tablab + "_" + rec + "Record\" name=\"" + tablab + "_" + rec + "Record\" value=\"0\"/>";
    html += "<div class=\"tabs-container\">";

    html += "   <div class=\"add_del_stj\">"


    html += "<div class=\"ryxx\" > 人员信息</div>";
    html += "<div class='btn-group'>";
    html += " <button type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "_" + rec + "btn\" name=\"" + table + "_" + rec + "btn\"  onclick=\"AddT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\"><i class=\"fa fa-plus\"></i>添加</button>";
    html += " <button type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "_" + rec + "CopyBtn\" name=\"" + table + "CopyBtn\"    onclick=\"CopyT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\"> <i class='fa fa-copy'> </i>复制</button>";


    /*
    html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "_" + rec + "btn\" name=\"" + table + "_" + rec + "btn\" value='添加' onclick=\"AddT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\" />";
    html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "_" + rec + "CopyBtn\" name=\"" + table + "_" + rec + "CopyBtn\" value='复制' onclick=\"CopyT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\" />";

    html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" value='删除' onclick=\"DelT3Page('" + table + "'," + rec + ");\" />";
	
    */
    html += "   </div>"

    html += "</div>";
    html += "   <div id=\"" + table + "_" + rec + "Nav\">";
    html += "       <ul class=\"nav nav-tabs stj_nav_2\" style='border-width: 0px;'></ul>";
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
function AddT3Page(tablab, table, sy, rec) {
    //layer.load(2);
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
    //style='border-width:0px;'
    lab += "<li class='active' >";
    lab += "    <a data-toggle='tab' style='background-color: transparent;border-width: 0px;' href='#" + table + "_" + rec + "Tab-" + record + "' aria-expanded='true'>" + sy + record + "</a>";
    lab += "<a  onclick='alert(1)' onfocus=\"this.blur();\" style='padding: 0px;border-width: 0px; background-color:transparent;'><i class='fa fa-close'></i></a>";

    lab += "</li>";
    $("#" + table + "_" + rec + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "_" + rec + "Tab-" + record + "' class='tab-pane active'>";
    content += "    <div class='panel-body' style='background-color:#dff0fa'>";
    //字段内容
    var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    var fieldcontent = $("#" + table + "Div").html();
    fieldcontent = fieldcontent.replace(regField, rec + "_" + record)
    //替换数据属性
    var dataField = new RegExp("&amp;&amp;datain&amp;&amp;", "g");
    fieldcontent = fieldcontent.replace(dataField, "datain")
    //赋值字符串
    content += fieldcontent;
    content += "    </div>";
    content += "</div>";
    $("#" + table + "_" + rec + "Tab").append(content);
    //记录数加1
    $("#" + tablab + "_" + rec + "Record").val(record);
    //初始化函数
    eval(table + "InitEvent('" + rec + "_" + record + "');");
    //layer.closeAll('loading');
}

function CopyT3Page(tablab, table, sy, rec) {
    var copyStatus = false;
    $("#" + table + "_" + rec + "Nav ul li").each(function (index) {
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
            //添加新建项
            AddT3Page(tablab, table, sy, rec);
            //获取当前最大标签数
            var record = parseInt($("#" + tablab + "_" + rec + "Record").val()); 
            //获取当前选中项数
            var regS = new RegExp(sy, "g");
            var recordCopy = titleTip.replace(regS, "");
            //还原对象字段集
            var filedListObject = JSON.parse(fieldArray[tablab + "_" + table]);
            //复制记录
            $(filedListObject).each(function (index) {
                SetCtrlValue(filedListObject[index].fieldname + "_" + rec + "_" + record, GetCtrlValue(filedListObject[index].fieldname + "_" + rec + "_" + recordCopy));
                //触发改变事件
                InitCtrlEvent(filedListObject[index].fieldname + "_" + rec + "_" + record);
            });
            //关闭
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });
    }
}

//删除Tab项
function DelT3Page(table, rec) {
    //layer.load(2);
    var delStatus = false;
    $("#" + table + "_" + rec + "Nav ul li").each(function (index) {
        if ($(this).hasClass("active")){
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
            //关闭
            layer.close(index);
        }, function (index) {
            layer.close(index);
        }); 
    }
    //layer.closeAll('loading');
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
    if (kjlx == "LABEL") {
        value = $("#" + name).html();
    }
    else if (kjlx == "TEXT") {
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
    //LABEL
    if (kjlx == "LABEL") {
        if (value instanceof Array) {
            if (value.length == 0)
                $("#" + name).html("");
            else
                $("#" + name).html(value[0].value);
        }
        else
            $("#" + name).html(value);
    }
    //TEXT
    else if (kjlx == "TEXT") {
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
    //文件控件
    else if (kjlx == "FILE") {
        $("#" + name).val(value);
        name = name.replace("\\.", "___");
        var arrItems = value.split("|");
        if (arrItems.length == 0)
            return;
        //显示内容
        for (i = 0; i < arrItems.length; i++) {
            if (arrItems[i] == "")
                continue;
            var itemlist = arrItems[i].split(",");
            showUpFile(name, itemlist[0], itemlist[1]);
        }
    }
    //展示文件控件
    else if (kjlx == "FILELABEL") {
        $("#" + name).val(value);
        name = name.replace("\\.", "___");
        var arrItems = value.split("|");
        if (arrItems.length == 0)
            return;
        //显示内容
        for (i = 0; i < arrItems.length; i++) {
            if (arrItems[i] == "")
                continue;
            var itemlist = arrItems[i].split(",");
            showUpFile(name, itemlist[0], itemlist[1]);
        }
    }
    //**** BOOTSTRAP ****
    else if (kjlx == "BOOTSTRAPSELECT") {
        //分析每一项
        var items = value.split(",");
        for (var iItem = 0; iItem < items.length; iItem++) {
            $("#" + name + " option[value='" + items[iItem] + "']").attr("selected", true);
            $("#" + name).trigger("chosen:updated");
        }
        //$("#" + name).val(value);
        $("#" + name).trigger("chosen:updated");
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

//初始化特殊控件
function InitSpecialCtrl(name) {
    //获取控件类型
    var kjlx = GetCtrlType(name);
    if (kjlx == "FILE") {
        //eval("UPLOADFILE_" + PointBlankRep(name) + "();");
        UploadFileInit(name);
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
//检验数据
function IsSubmitFormValid() {
    var ret = true;
    //触发事件
    $("input[custom='custom']").each(function (i) {
        var ctrl = $("input[custom='custom']").eq(i);
        var ctrlhtml = $("input[custom='custom']").eq(i).parent();
        var strhtml = ctrlhtml.html();
        if (strhtml != undefined && strhtml.toLowerCase().indexOf("onblur") > -1) {
            var var1 = strhtml;
            var var2 = "onblur";
            var index1 = var1.indexOf(var2) + var2.length + 1;

            var1 = strhtml;
            var index2 = var1.indexOf("\"", index1);
            var index3 = var1.indexOf(";", index2 + 1);
            var varPar1 = var1.substr(index2 + 1, index3 - index2);
            //var index3 = var1.indexOf("\"", index2 + 1);
            //var varPar1 = var1.substr(index2 + 1, index3 - index2 - 1); 
            if (varPar1.indexOf("&") == -1) {
                if (!eval(varPar1)) {
                    ret = false;
                    return ret;
                }
            }
        }
    });

    if (ret) {
        //检测必输项
        $("[custom='custom'][mustin='mustin'][datain='datain']").each(function (i) {
            var ctrl = $("[custom='custom'][mustin='mustin'][datain='datain']").eq(i);
            //获取值
            var value = GetCtrlValue(GetNameByCtrl(ctrl));
            if (value == undefined || value == null || value == "") {
                alert(ctrl.attr("mc") + "不能为空！");
                //ctrl.focus().select();
                ret = false;
                return ret;
            }
        });
    }
    //返回
    return ret;
}

//暂存
function btn_opt_zc(url) {
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function (val) {
            //遮罩
            parent.layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            }
            else {
                alert('保存成功!');
                parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
            //遮罩
            parent.layer.load();
        }
    });
}

//保存
function btn_opt_tj(url) {
    //检验数据
    if (!IsSubmitFormValid())
        return;

    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function (val) {
            //遮罩
            parent.layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            }
            else {
                alert('保存成功!');
                //执行回调函数
                if (val.data.callback != undefined) {
                    eval(val.data.callback);
                }
                //关闭窗口
                parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
            //遮罩
            parent.layer.load();
        }
    });
}


//返回
function btn_opt_fh(url) {
    window.location = url;
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
    $.post("/DataInput/CtrlStringData", { ctrlString: ctrlstring }, function (data, textStatus) {
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
        content: '/DataInput/Helplink?HelpLink=' + helpStr + '&Math.random()' //iframe的url
    });
}
//*************************************Helplink**************************************


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
        if (confirm("确定要删除文件：" + filename + "吗？")){
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
