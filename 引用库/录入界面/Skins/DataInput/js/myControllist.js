//****** 动态执行 ******
//在全局环境中执行
//function evalGlobal(strScript) {
//    with (window) eval(strScript);
//} 
//****** 动态执行 ******

//****** 动态创建从表结构 ******
//创建从表Grid
function CreateGridToolbar(table) {
    var ret = "";
    ret += "<input type=\"hidden\" id=\"" + table + "index\" name=\"" + table + "index\" />";
    ret += "<input type=\"hidden\" id=\"" + table + "SaveData\" name=\"" + table + "SaveData\" />";
    ret += "<table id=\"" + table + "_datagrid\"></table>";
    ret += "<div id=\"" + table + "_toolbar\" style=\"display: none;\">";
    ret += "    <a href=\"javascript:void(0)\" id=\"btn_new_" + table + "\" class=\"easyui-linkbutton\" iconCls=\"icon-add\" plain=\"true\" onclick=\"new" + table + "()\">添加</a>";
    ret += "    <a href=\"javascript:void(0)\" id=\"btn_modify_" + table + "\" class=\"easyui-linkbutton\" iconCls=\"icon-edit\" plain=\"true\" onclick=\"modify" + table + "()\">修改</a>";
    ret += "    <a href=\"javascript:void(0)\" id=\"btn_save_" + table + "\" class=\"easyui-linkbutton\" iconCls=\"icon-save\" plain=\"true\" onclick=\"save" + table + "()\" disabled=\"disabled\">保存</a>";
    ret += "    <a href=\"javascript:void(0)\" id=\"btn_copy_" + table + "\" class=\"easyui-linkbutton\" iconCls=\"icon-copy\" plain=\"true\" onclick=\"copy" + table + "()\">复制</a>";
    ret += "    <a href=\"javascript:void(0)\" id=\"btn_del_" + table + "\" class=\"easyui-linkbutton\" iconCls=\"icon-remove\" plain=\"true\" onclick=\"del" + table + "()\">删除</a>";
    ret += "</div>";
    return ret;
}
//构造列表
function CreateGrid(table, field) {
    var ret = "";
    ret +=  table + "dataGrid = $('#" + table + "_datagrid').datagrid({";
    ret += "    fit: true,";
    ret += "    collapsible: false,";
    ret += "    border: false,";
    ret += "    pagination: false,";
    ret += "    autoRowHeight: false,";
    ret += "    singleSelect: true,";
    ret += "    rownumbers: true,";
    ret += "    frozenColumns: [[{field:'ck',checkbox:true}]],";
    ret += "    columns: [[";
    $(field).each(function (index) {
        ret += "    {";
        ret += "        field:'" + field[index].fieldname + "',";
        ret += "        title:'" + field[index].sy + "',";
        //编辑信息
        ret += "        " + CreateListCtrl(field[index]);
        if (field[index].xscd > 0)
            ret += "    width:" + field[index].xscd + ",";
        else
            ret += "    width:50,";
        ret += "        align:'center'";
        ret += "    },";
    });
    //去掉最后一个,号
    if (ret.length > 0 && ret.charAt(ret.length - 1) == ',')
        ret = ret.substring(0, ret.length - 1)
    ret += "    ]],";
    ret += "    toolbar: '#" + table + "_toolbar'";
    ret += "});";
    eval(ret);
    $.parser.parse($("#contentDiv"));
}
//****** 动态创建从表结构 ******

//****** 动态创建控件类型 ******
function CreateListCtrl(ctrl) {
    
    var ret = "";
    switch (ctrl.kjlx) {
        case "TEXT":
            ret = CreateListText(ctrl);
            break;
        case "SELECT":
            ret = CreateListSelect(ctrl);
            break;
        default:
            ret = "";
            break;
    }
    return ret;
}

//列表文本框
function CreateListText(ctrl) {
    var ret = "";
    ret += "editor: {";
    ret += "    type:\"validatebox\",";
    ret += "    options:{";
    ret += "        required: " + (ctrl.mustin ? "true" : "false");
    ret += "    }";
    ret += "},";
    return ret;
}
//****** 动态创建控件类型 ******

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
function CreateField(table, hiddenfield, field, presuffix, suffix) {
    //	alert(field.length);
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
                //bootstrap multiselect   
                case "BOOTSTRAPMULSELECT":
                    ret += CreateBootstrapMulSelect(field[index], presuffix + suffix);
                    break;
                case "BOOTSTRAPSINGSELECT":
                    ret += CreateBootstrapSingleSelect(field[index], presuffix + suffix);
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
        ret += "<input type='radio' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + iItem +"' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
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
        ret += "<label for='" + ctrl.fieldname + suffix + iItem + "' style='padding-left: 2px;'>" + itemList[0] + "</label>";
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

//多选下拉过滤
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
    return ret;
}


//单选下拉过滤
function CreateBootstrapSingleSelect(ctrl, suffix) {
    var ret = "";
    ret += "<select custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "'  ";
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

    //是否必输项
    if (ctrl.mustin)
        ret += "<font color='red'>*</font>";
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
        $.messager.alert('提示', err, 'info');
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
        $.messager.alert('提示', err, 'info');
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
