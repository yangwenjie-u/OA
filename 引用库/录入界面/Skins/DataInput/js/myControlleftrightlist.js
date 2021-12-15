
//初始化数据
$(function () {
    //扩展
    $.extend($.fn.datagrid.defaults.editors, {
        numberspinner: {
            init: function (container, options) {
                var input = $('<input type="text">').appendTo(container);
                return input.numberspinner(options);
            },
            destroy: function (target) {
                $(target).numberspinner('destroy');
            },
            getValue: function (target) {
                return $(target).numberspinner('getValue');
            },
            setValue: function (target, value) {
                $(target).numberspinner('setValue', value);
            },
            resize: function (target, width) {
                $(target).numberspinner('resize', width);
            }
        }
    });

    $.extend($.fn.textbox.defaults.rules, {
        cusnumber: {
            validator: function (value, param) {
                return IsNumeric(value);
            },
            message: "请输入数字"
        },
        cusint: {
            validator: function (value, param) {
                return IsInt(value);
            },
            message: "请输入整数"
        },
        cusdate: {
             validator : function(value, param) {
                return IsDate(value);
            },
            message : "请输入日期"
        },
        cusdatetime: {
            validator: function (value, param) {
                return IsDateTime(value);
            },
            message: "请输入日期时间"
        }
    });
//        number : {
//            validator : function(value, param) {
//                return /^[0-9]*$/.test(value);
//            },
//            message : "请输入数字"
//        },
//        chinese : {
//            validator : function(value, param) {
//                var reg = /^[\u4e00-\u9fa5]+$/i;
//                return reg.test(value);
//            },
//            message : "请输入中文"
//        },
//        checkLength: {
//            validator: function(value, param){
//                return param[0] >= get_length(value);
//            },
//            message: '请输入最大{0}位字符'
//        },
//        specialCharacter: {
//            validator: function(value, param){
//                var reg = new RegExp("[`~!@#$^&*()=|{}':;'\\[\\]<>~！@#￥……&*（）——|{}【】‘；：”“'、？]");
//                return !reg.test(value);
//            },
//            message: '不允许输入特殊字符'
//        }
//    });  
});

//******************************//
var all_k = 350;
var label_width = 110;
var input_width = 240;



//**** 动态隐藏定义层 ****


////动态创建表单
//function CreateField(table, hiddenfield,copyhiddenfield, field, presuffix, suffix) {
//    var ret = "";
//    //td,tr之前的所有代码
//    var ret_s = "";

//    ret_s += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
//    ret_s += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
//    //隐藏字段
//    $(hiddenfield).each(function (index) {
//        ret_s += "<input type='hidden' kjlx='hidden' id='" + hiddenfield[index].fieldname + suffix + "' name='" + hiddenfield[index].fieldname + suffix + "' value='" + hiddenfield[index].defval + "'/>";
//    });
//    //隐藏复制字段
//    $(copyhiddenfield).each(function (index) {
//        ret_s += "<input type='hidden' kjlx='hidden' id='" + copyhiddenfield[index].fieldname + suffix + "' name='" + copyhiddenfield[index].fieldname + suffix + "' value=''/>";
//    });
//    var temp2 = document.getElementsByClassName("wrapper wrapper-content").item(0).offsetWidth;
//    var td_width = temp2 / conditionNum;
//    //分析各字段对象
//    var fieldLeftList = new Array();
//    var fieldRightList = new Array();
//    $(field).each(function (index) {
//        //左则
//        if (field[index].position == 0)
//            fieldLeftList.push(field[index]);
//        else
//            fieldRightList.push(field[index]);
//    });

//    var rowNum = fieldLeftList.length > fieldRightList.length ? fieldLeftList.length : fieldRightList.length;
//    //占整行的数据
//    var fieldRowStr = "";
//    for (var i = 1; i <= rowNum; i++) {
//        if (fieldLeftList.length >= i) {
//            //左行
//            fieldRowStr += "<tr>";
//            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
//            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + fieldLeftList[i - 1].sy + "</label>";
//            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
//            //判断类型
//            switch (fieldLeftList[i - 1].kjlx) {
//                case "LABEL":
//                    fieldRowStr += CreateHtmlLabel(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXT":
//                    fieldRowStr += CreateHtmlText(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXTAREA":
//                    fieldRowStr += CreateHtmlTextarea(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXTBUTTON":
//                    fieldRowStr += CreateHtmlTextButton(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "CHECKBOX":
//                    fieldRowStr += CreateHtmlCheckBox(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "RADIO":
//                    fieldRowStr += CreateHtmlRadio(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "SELECT":
//                    fieldRowStr += CreateHtmlSelect(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "COMBOBOX":
//                    fieldRowStr += CreateHtmlCombobox(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "FILE":
//                case "FILELABEL":
//                    fieldRowStr += CreateHtmlFile(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                //日期                  
//                case "DATE":
//                    fieldRowStr += CreateHtmlDate(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                //bootstrap插件                   
//                case "BOOTSTRAPSELECT":
//                    fieldRowStr += CreateBootstrapSelect(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                //bootstrap multiselect     
//                case "BOOTSTRAPMULSELECT":
//                    fieldRowStr += CreateBootstrapMulSelect(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                case "BOOTSTRAPSINGSELECT":
//                    fieldRowStr += CreateBootstrapSingleSelect(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//                default:
//                    fieldRowStr += CreateHtmlText(fieldLeftList[i - 1], presuffix + suffix);
//                    break;
//            }
//            fieldRowStr += "    </div> </td><td class='zhushi_shang'><div class='zhushi_shang_div' title=''></div></td>";
//        }
//        else {
//            //左行
//            fieldRowStr += "<tr>";
//            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
//            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\"></label>";
//            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
//            fieldRowStr += "    </div> </td><td class='zhushi_shang'><div class='zhushi_shang_div' title=''></div></td>";
//        }    
//        //添加行记录
//        ret += fieldRowStr;
//        fieldRowStr = "";

//        //右行
//        //判断类型
//        if (fieldRightList.length >= i) {
//            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
//            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + fieldRightList[i - 1].sy + "</label>";
//            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
//            //判断类型
//            switch (fieldRightList[i - 1].kjlx) {
//                case "LABEL":
//                    fieldRowStr += CreateHtmlLabel(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXT":
//                    fieldRowStr += CreateHtmlText(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXTAREA":
//                    fieldRowStr += CreateHtmlTextarea(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "TEXTBUTTON":
//                    fieldRowStr += CreateHtmlTextButton(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "CHECKBOX":
//                    fieldRowStr += CreateHtmlCheckBox(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "RADIO":
//                    fieldRowStr += CreateHtmlRadio(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "SELECT":
//                    fieldRowStr += CreateHtmlSelect(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "COMBOBOX":
//                    fieldRowStr += CreateHtmlCombobox(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "FILE":
//                case "FILELABEL":
//                    fieldRowStr += CreateHtmlFile(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                //日期                   
//                case "DATE":
//                    fieldRowStr += CreateHtmlDate(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                //bootstrap插件                    
//                case "BOOTSTRAPSELECT":
//                    fieldRowStr += CreateBootstrapSelect(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                //bootstrap multiselect      
//                case "BOOTSTRAPMULSELECT":
//                    fieldRowStr += CreateBootstrapMulSelect(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                case "BOOTSTRAPSINGSELECT":
//                    fieldRowStr += CreateBootstrapSingleSelect(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//                default:
//                    fieldRowStr += CreateHtmlText(fieldRightList[i - 1], presuffix + suffix);
//                    break;
//            }
//        }
//        else {
//            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
//            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\"></label>";
//            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
//        }

//        fieldRowStr += "    </div> </td>";
//        fieldRowStr += "</tr>";
//        //添加行记录
//        ret += fieldRowStr;
//        fieldRowStr = "";
//    }


////    $(field).each(function (index) {
////        //0为左边不修改 1为右边修改

////        //一列两行
////        if (index % 2 == 0) {
////            fieldRowStr += "<tr>";
////            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
////            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + field[index].sy + "</label>";
////            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";

////            //判断类型
////            switch (field[index].kjlx) {
////                case "LABEL":
////                    fieldRowStr += CreateHtmlLabel(field[index], presuffix + suffix);
////                    break;
////                case "TEXT":
////                    fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
////                    break;
////                case "TEXTAREA":
////                    fieldRowStr += CreateHtmlTextarea(field[index], presuffix + suffix);
////                    break;
////                case "TEXTBUTTON":
////                    fieldRowStr += CreateHtmlTextButton(field[index], presuffix + suffix);
////                    break;
////                case "CHECKBOX":
////                    fieldRowStr += CreateHtmlCheckBox(field[index], presuffix + suffix);
////                    break;
////                case "RADIO":
////                    fieldRowStr += CreateHtmlRadio(field[index], presuffix + suffix);
////                    break;
////                case "SELECT":
////                    fieldRowStr += CreateHtmlSelect(field[index], presuffix + suffix);
////                    break;
////                case "COMBOBOX":
////                    fieldRowStr += CreateHtmlCombobox(field[index], presuffix + suffix);
////                    break;
////                case "FILE":
////                case "FILELABEL":
////                    fieldRowStr += CreateHtmlFile(field[index], presuffix + suffix);
////                    break;
////                //日期                
////                case "DATE":
////                    fieldRowStr += CreateHtmlDate(field[index], presuffix + suffix);
////                    break;
////                //bootstrap插件                 
////                case "BOOTSTRAPSELECT":
////                    fieldRowStr += CreateBootstrapSelect(field[index], presuffix + suffix);
////                    break;
////                //bootstrap multiselect   
////                case "BOOTSTRAPMULSELECT":
////                    fieldRowStr += CreateBootstrapMulSelect(field[index], presuffix + suffix);
////                    break;
////                case "BOOTSTRAPSINGSELECT":
////                    fieldRowStr += CreateBootstrapSingleSelect(field[index], presuffix + suffix);
////                    break;
////                default:
////                    fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
////                    break;
////            }
////            fieldRowStr += "    </div> </td><td class='zhushi_shang'><div class='zhushi_shang_div' title=''></div></td>";
////            //添加行记录
////            ret += fieldRowStr;
////            fieldRowStr = "";
////        }
////        else {
////            fieldRowStr += "<td class=\"stj_lr2_table_td\">";
////            fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + field[index].sy + "</label>";
////            fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
////            //判断类型
////            switch (field[index].kjlx) {
////                case "LABEL":
////                    fieldRowStr += CreateHtmlLabel(field[index], presuffix + suffix);
////                    break;
////                case "TEXT":
////                    fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
////                    break;
////                case "TEXTAREA":
////                    fieldRowStr += CreateHtmlTextarea(field[index], presuffix + suffix);
////                    break;
////                case "TEXTBUTTON":
////                    fieldRowStr += CreateHtmlTextButton(field[index], presuffix + suffix);
////                    break;
////                case "CHECKBOX":
////                    fieldRowStr += CreateHtmlCheckBox(field[index], presuffix + suffix);
////                    break;
////                case "RADIO":
////                    fieldRowStr += CreateHtmlRadio(field[index], presuffix + suffix);
////                    break;
////                case "SELECT":
////                    fieldRowStr += CreateHtmlSelect(field[index], presuffix + suffix);
////                    break;
////                case "COMBOBOX":
////                    fieldRowStr += CreateHtmlCombobox(field[index], presuffix + suffix);
////                    break;
////                case "FILE":
////                case "FILELABEL":
////                    fieldRowStr += CreateHtmlFile(field[index], presuffix + suffix);
////                    break;
////                //日期                 
////                case "DATE":
////                    fieldRowStr += CreateHtmlDate(field[index], presuffix + suffix);
////                    break;
////                //bootstrap插件                  
////                case "BOOTSTRAPSELECT":
////                    fieldRowStr += CreateBootstrapSelect(field[index], presuffix + suffix);
////                    break;
////                //bootstrap multiselect    
////                case "BOOTSTRAPMULSELECT":
////                    fieldRowStr += CreateBootstrapMulSelect(field[index], presuffix + suffix);
////                    break;
////                case "BOOTSTRAPSINGSELECT":
////                    fieldRowStr += CreateBootstrapSingleSelect(field[index], presuffix + suffix);
////                    break;
////                default:
////                    fieldRowStr += CreateHtmlText(field[index], presuffix + suffix);
////                    break;
////            }
////            fieldRowStr += "    </div> </td>";
////            fieldRowStr += "</tr>";
////            //添加行记录
////            ret += fieldRowStr;
////            fieldRowStr = "";
////        }
////    });

//    //前div
//    ret_s += "<div class=\"col-sm-3 stj_shu\">";
//    ret_s += "  <table class=\"stj_lr2_table\">";
//    ret_s = ret_s + ret + "</table></div>";

//    return ret_s;
//}

//动态创建表单
function CreateField(table, hiddenfield, copyhiddenfield, field, presuffix, suffix) {
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
    //分析各字段对象
    var fieldLeftList = new Array();
    var fieldRightList = new Array();
    $(field).each(function (index) {
        //左则
        if (field[index].position == 0)
            fieldLeftList.push(field[index]);
        else
            fieldRightList.push(field[index]);
    });

    var rowNum = fieldLeftList.length > fieldRightList.length ? fieldLeftList.length : fieldRightList.length;
    //占整行的数据
    var ret_s_left = "";
    var fieldRowStr = "";
    for (var i = 1; i <= fieldLeftList.length; i++) {
        //左行
        fieldRowStr += "<tr>";
        fieldRowStr += "<td class=\"stj_lr2_table_td\">";
        fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + fieldLeftList[i - 1].sy + "</label>";
        fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
        //判断类型
        switch (fieldLeftList[i - 1].kjlx) {
            case "LABEL":
                fieldRowStr += CreateHtmlLabel(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "TEXT":
                fieldRowStr += CreateHtmlText(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "TEXTAREA":
                fieldRowStr += CreateHtmlTextarea(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "TEXTBUTTON":
                fieldRowStr += CreateHtmlTextButton(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "CHECKBOX":
                fieldRowStr += CreateHtmlCheckBox(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "RADIO":
                fieldRowStr += CreateHtmlRadio(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "SELECT":
                fieldRowStr += CreateHtmlSelect(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "COMBOBOX":
                fieldRowStr += CreateHtmlCombobox(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "FILE":
            case "FILELABEL":
                fieldRowStr += CreateHtmlFile(fieldLeftList[i - 1], presuffix + suffix);
                break;
            //日期                   
            case "DATE":
                fieldRowStr += CreateHtmlDate(fieldLeftList[i - 1], presuffix + suffix);
                break;
            //bootstrap插件                    
            case "BOOTSTRAPSELECT":
                fieldRowStr += CreateBootstrapSelect(fieldLeftList[i - 1], presuffix + suffix);
                break;
            //bootstrap multiselect      
            case "BOOTSTRAPMULSELECT":
                fieldRowStr += CreateBootstrapMulSelect(fieldLeftList[i - 1], presuffix + suffix);
                break;
            case "BOOTSTRAPSINGSELECT":
                fieldRowStr += CreateBootstrapSingleSelect(fieldLeftList[i - 1], presuffix + suffix);
                break;
            default:
                fieldRowStr += CreateHtmlText(fieldLeftList[i - 1], presuffix + suffix);
                break;
        }
        fieldRowStr += "    </div> </td><td class='zhushi_shang'><div class='zhushi_shang_div' title=''>";
        //描述说明
        if (fieldLeftList[i - 1].msginfo.length > 0)
            fieldRowStr += ctrlMsginfo(fieldLeftList[i - 1].msginfo);
        fieldRowStr += "</div></td></tr>";
     } 
    //添加行记录
    ret_s_left = fieldRowStr;
    var ret_s_right = "";
    fieldRowStr = "";
    for (var i = 1; i <= fieldRightList.length; i++) {
        //右行
        fieldRowStr += "<td class=\"stj_lr2_table_td\">";
        fieldRowStr += "    <label class=\"col-sm-3 control-label\" style=\"line-height:30px;\">" + fieldRightList[i - 1].sy + "</label>";
        fieldRowStr += "    <div class=\"col-sm-9 date stj_input\">";
        //判断类型
        switch (fieldRightList[i - 1].kjlx) {
            case "LABEL":
                fieldRowStr += CreateHtmlLabel(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "TEXT":
                fieldRowStr += CreateHtmlText(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "TEXTAREA":
                fieldRowStr += CreateHtmlTextarea(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "TEXTBUTTON":
                fieldRowStr += CreateHtmlTextButton(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "CHECKBOX":
                fieldRowStr += CreateHtmlCheckBox(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "RADIO":
                fieldRowStr += CreateHtmlRadio(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "SELECT":
                fieldRowStr += CreateHtmlSelect(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "COMBOBOX":
                fieldRowStr += CreateHtmlCombobox(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "FILE":
            case "FILELABEL":
                fieldRowStr += CreateHtmlFile(fieldRightList[i - 1], presuffix + suffix);
                break;
            //日期                    
            case "DATE":
                fieldRowStr += CreateHtmlDate(fieldRightList[i - 1], presuffix + suffix);
                break;
            //bootstrap插件                     
            case "BOOTSTRAPSELECT":
                fieldRowStr += CreateBootstrapSelect(fieldRightList[i - 1], presuffix + suffix);
                break;
            //bootstrap multiselect       
            case "BOOTSTRAPMULSELECT":
                fieldRowStr += CreateBootstrapMulSelect(fieldRightList[i - 1], presuffix + suffix);
                break;
            case "BOOTSTRAPSINGSELECT":
                fieldRowStr += CreateBootstrapSingleSelect(fieldRightList[i - 1], presuffix + suffix);
                break;
            default:
                fieldRowStr += CreateHtmlText(fieldRightList[i - 1], presuffix + suffix);
                break;
        }
        fieldRowStr += "    </div> </td><td class='zhushi_shang'><div class='zhushi_shang_div' title=''>";
//        if (fieldLeftList[i - 1].helplink.length > 0)
//            fieldRowStr += "按F2选择";
        //描述说明
        if (fieldRightList[i - 1].msginfo.length > 0)
            fieldRowStr += ctrlMsginfo(fieldRightList[i - 1].msginfo);
        fieldRowStr += "</div></td></tr>";
    }
    //添加行记录
    ret_s_right = fieldRowStr;

    //前div
    ret_s += "<div class=\"col-sm-3 stj_shu\">";
    ret_s += "<div class='luru_left_z'>  <table class=\"stj_lr2_table\">";
    ret_s += ret_s_left;
    ret_s += "</table></div>"
    ret_s += "<div class='luru_right_z'>  <table class=\"stj_lr2_table\">";
    ret_s += ret_s_right;
    ret_s += "</table></div>"
    ret_s += "</div>"


    return ret_s;
}

//创建描述信息
function ctrlMsginfo(msg) {
    var ret = msg;

    return ret;
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
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
//    if (ctrl.helplink.length > 0)
//        ret += "按F2选择";
    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
        ret += ">" + itemList[0] + "</option>";
    }
    ret += "</select>";
    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
    ret += "</span>";   

    ret += "</div>";

    //是否必输项
    if (ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}

//特殊控件
//** **
//文件框FILE
function CreateHtmlFile(ctrl, suffix) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&' mc = '" + ctrl.sy + "' "
    if (ctrl.mustin) {
        ret += "mustin='mustin' ";
    }
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
//    //描述说明
//    if (ctrl.msginfo.length > 0)
//        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}



//**** 界面 ****
//** 从表 **
//从表Tab项模板
function GetT2Tab(table, sy, data) {
    var html = "";
    //添加锚点
    html += "<input type=\"hidden\" id=\"" + table + "SaveData\" name=\"" + table + "SaveData\" value=\'\'/>";
    html += "<div class=\"col-sm-9 animated fadeInRight stj_shu\" id=\"table_right\" style=\"background-color:#fff;\">";
    html += "  <div class=\"panel panel-success\">";
    html += "    <div class=\"panel-heading\">";
    html += "          <h3 class=\"panel-title\">" + sy + "</h3>";
    html += "    </div>";
    html += "    <div class=\"panel-body stj_lr2_mianban\">";

    html += "       <div class=\"stj_lr2_mianban_title\">";
    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\"  onclick=\"" + table + "New();\"><i class=\"fa fa-plus\"></i>&nbsp;添加</button>";
    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "ModifyBtn\" name=\"" + table + "ModifyBtn\"  onclick=\"" + table + "Modify();\"><i class=\"fa fa-plus\"></i>&nbsp;修改</button>";
//    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "SaveBtn\" name=\"" + table + "SaveBtn\"   onclick=\"" + table + "Save();\"><i class=\"fa fa-copy\"></i>&nbsp;保存</button>";
    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "DelBtn\" name=\"" + table + "DelBtn\"   onclick=\"" + table + "Del();\"><i class=\"fa fa-copy\"></i>&nbsp;删除</button>"; 
    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\"   onclick=\"" + table + "Copy();\"><i class=\"fa fa-copy\"></i>&nbsp;复制</button>";
    html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + table + "CopyNumsBtn\" name=\"" + table + "CopyNumsBtn\"   onclick=\"" + table + "CopyNum();\"><i class=\"fa fa-copy\"></i>&nbsp;复制多组</button>";
    html += "           <div class=\"btn_right\">";
    //固定按钮
    $(data.btndata).each(function (index) {
        html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + data.btndata[index].name + "\" name=\"" + data.btndata[index].name + "\"   onclick='" + data.btndata[index].event + "(\"" + data.btndata[index].url + "\");'><i class=\"fa fa-copy\"></i>&nbsp;" + data.btndata[index].title + "</button>";
        //$("#btnDiv").append("<button class=\"btn btn-primary stj_ti\" type=\"button\"    name='" + val.data.btndata[index].name + "' value='" + val.data.btndata[index].title + "' onclick='" + val.data.btndata[index].event + "(\"" + val.data.btndata[index].url + "\");' ><i class=\"" + val.data.btndata[index].css + "\" ></i>" + val.data.btndata[index].title + "</button>");
    });
    //自定义按钮
    $(data.custombutton).each(function (index) {
        html += "           <button class=\"btn btn-success btn-outline btn-sm \" type=\"button\" id=\"" + data.custombutton[index].name + "\" name=\"" + data.custombutton[index].name + "\"   onclick='" + ParamRep(val.data.custombutton[index].event) + "'><i class=\"fa fa-copy\"></i>&nbsp;" + data.custombutton[index].title + "</button>";
        //$("#btnDiv").append("<button class=\"btn btn-primary stj_ti\" type=\"button\"    name='" + val.data.custombutton[index].name + "' value='" + val.data.custombutton[index].title + "' onclick=\"" + ParamRep(val.data.custombutton[index].event) + "\" ><i class=\"" + val.data.custombutton[index].css + "\" ></i>" + val.data.custombutton[index].title + "</button>");
    });
    html += "       </div>";
    html += "       </div>";

    html += "       <div class=\"panel-body\" style=\"height:" + ($(document.body).height() * 0.4 - 50) + "px\">";
    html += "           <table id=\"" + table + "datagrid\" ></table>";
    html += "       </div>";

    html += "    </div>";

    html += "</div>";
    html += "</div>";
    return html;
}


//判断是否为正整数
function isPositiveNum(s) {//是否为正整数  
    var re = /^[0-9]*[1-9][0-9]*$/;
    return re.test(s)
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
//创建整体控制提交函数
function SaveAllCtroller(t2fields) {
    var ret = "";
    ret += "<script language='javascript'>";
    ret += "    function ListSaveFun() {"
    ret += "        var ret = true;";
    $(t2fields).each(function (index) {
        ret += "    ret = ret && " + t2fields[index].t2table + "Save();";
    });
    ret += "        return ret;";
    ret += "    }";
    ret += "</script>";

    return ret;
}
//*************************************创建从表JS**********************************
//创建从表JS
function CreateListJs(table, field) {
    var ret = "";
    ret += "<script language='javascript'>";
    ret += "    var " + table + "dataGrid;";
    ret += "    var " + table + "nums = 0;";
    ret += "    var " + table + "editIndex = undefined;";
    //新建默认值
    var defrow = "";
    defrow += "    var " + table + "defRow = ";
    defrow += "    {";
    defrow += "        \"opt\":\"new\",";
    //循环
    $(field).each(function (index) {
        defrow += "    \"" + field[index].fieldname + "\":\"" + field[index].defval + "\",";
    });
    //去掉最后一个逗号
    if (defrow.length > 0 && defrow.charAt(defrow.length - 1) == ',')
        defrow = defrow.substring(0, defrow.length - 1);
    defrow += "    };";
    ret += defrow;
    //**** 定义函数 ****
    //定义grid
    ret += "    function " + table + "InitGrid() {";
    ret += "        " + table + "dataGrid = $('#" + table + "datagrid').datagrid({";
    ret += "            fit: true,";
    ret += "            collapsible: false,";
    ret += "            border: false,";
    ret += "            pagination: false,";
    ret += "            autoRowHeight: false,";
    ret += "            singleSelect: true,";
    ret += "            showFooter: true,";
    ret += "            rownumbers: true,";
    ret += "            ";
    ret += "            ";
    //定义字段
    var fieldStr = "";
    //循环
    $(field).each(function (index) {
        fieldStr += "{";
        if (field[index].xscd == 0)
            fieldStr += "   width: 100,";
        else
            fieldStr += "   width: " + field[index].xscd + ",";
        fieldStr += "   field: '" + field[index].fieldname + "',";
        fieldStr += "   title: '" + field[index].sy + "',";
        //判断控件类型
        if (field[index].kjlx == "SELECT") {
            //组装数据包
            //数据包值
            var datastr = "";
            //ctrlstring值
            var valueStr = "";
            //控件改变值
            var ctrlChange = "";
            if (field[index].ctrlstring != "") {
                //值对
                var keyParam;
                var typeList = field[index].ctrlstring.split("||");
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
            fieldStr += "           required: " + field[index].mustin + ",";
            fieldStr += "           valueField: 'id',";
            fieldStr += "           textField: 'text',";
            fieldStr += "           editable: false,";
            //改变事件
            if (ctrlChange != "") {
                fieldStr += "           onChange: function(newValue,oldValue){";
                //判断如果有内容值改变时,才触发
                fieldStr += "               if(newValue != \"\" || oldValue != \"\")";
                fieldStr += "                   changeOtherContrl(" + table + "dataGrid," + table + "editIndex,\"" + ctrlChange + "\");";
                fieldStr += "           },";
            }

            //fieldStr += "           panelHeight: 'auto'";
            if (valueStr != "") {
                //分析每一项
                var items = valueStr.split("|");
                if (items.length * 25 > 200)
                    fieldStr += "           panelHeight: 200";
                else
                    fieldStr += "           panelHeight: " + items.length * 25;
            }
            else
                fieldStr += "           panelHeight: 200";
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else if (field[index].kjlx == "SELECTS" || field[index].kjlx == "CHECKBOX") {
            //组装数据包
            //数据包值
            var datastr = "";
            //ctrlstring值
            var valueStr = "";
            //控件改变值
            var ctrlChange = "";
            if (field[index].ctrlstring != "") {
                //值对
                var keyParam;
                var typeList = field[index].ctrlstring.split("||");
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
            fieldStr += "           required: true,";
            fieldStr += "           multiple:true,";
            fieldStr += "           valueField: 'id',";
            fieldStr += "           textField: 'text',";
            fieldStr += "           editable: false,";
            //改变事件
            if (ctrlChange != "") {
                fieldStr += "           onChange: function(newValue,oldValue){";
                //判断如果有内容值改变时,才触发
                fieldStr += "               if(newValue != \"\" || oldValue != \"\")";
                fieldStr += "                   changeOtherContrl(" + table + "dataGrid," + table + "editIndex,\"" + ctrlChange + "\");";
                fieldStr += "           },";
            }
            //fieldStr += "           panelHeight: 'auto'";
            if (valueStr != "") {
                //分析每一项
                var items = valueStr.split("|");
                if (items.length * 25 > 200)
                    fieldStr += "           panelHeight: 200";
                else
                    fieldStr += "           panelHeight: " + items.length * 25;
            }
            else
                fieldStr += "           panelHeight: 200";
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else if (field[index].kjlx == "COMBOBOX") {
            //组装数据包
            //数据包值
            var datastr = "";
            //ctrlstring值
            var valueStr = "";
            //控件改变值
            var ctrlChange = "";
            if (field[index].ctrlstring != "") {

                //值对
                var keyParam;
                var typeList = field[index].ctrlstring.split("||");
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
            //fieldStr += "               validType:['cusdate'],";
            if (datastr != "")
                fieldStr += "           data:" + datastr + ",";
            fieldStr += "           required: " + field[index].mustin + ",";
            fieldStr += "           valueField: 'id',";
            fieldStr += "           textField: 'text',";
            fieldStr += "           editable: true,";
            //改变事件
            if (ctrlChange != "") {
                fieldStr += "           onChange: function(newValue,oldValue){";
                //判断如果有内容值改变时,才触发
                fieldStr += "               if(newValue != \"\" || oldValue != \"\")";
                fieldStr += "                   changeOtherContrl(" + table + "dataGrid," + table + "editIndex,\"" + ctrlChange + "\");";
                fieldStr += "           },";
            }
            //fieldStr += "           panelHeight: 'auto'";
            if (valueStr != "") {
                //分析每一项
                var items = valueStr.split("|");
                if (items.length * 25 > 200)
                    fieldStr += "           panelHeight: 200";
                else
                    fieldStr += "           panelHeight: " + items.length * 25;
            }
            else
                fieldStr += "           panelHeight: 200";
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else if (field[index].kjlx == "DATE") {
            fieldStr += "   editor: {";
            fieldStr += "       type:'datebox',";
            fieldStr += "       formatter:function(date) {alert('gg');";
            fieldStr += "           var y = date.getFullYear();";
            fieldStr += "           var m = date.getMonth()+1;";
            fieldStr += "           var d = date.getDate();";
            fieldStr += "           return y + '-' + m + '-' + d;";
            fieldStr += "       },";
            fieldStr += "       parser:function() {";
            fieldStr += "           ";
            fieldStr += "       }";
            fieldStr += "   },";
        }
        else {
            if (!field[index].zdsx) {
                fieldStr += "   editor: {";
                fieldStr += "       type:'textbox',"; //textbox，validatebox
                fieldStr += "       options:{";
                //检验类型
                var js = "";
                if (field[index].validproc.length > 0) {
                    //值对
                    var keyParam;
                    var typeList = field[index].validproc.split("|||");
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
                    fieldStr += "           onChange: function(newValue,oldValue){";
                    //fieldStr += "               " + GetSwitchListValidProc(js,table);
                    fieldStr += "           },";
                }

                if (field[index].inputlx != "") {
                    fieldStr += "           validType: '" + GetListCheckData(field[index].inputlx.toLowerCase()) + "',";
                }
                fieldStr += "           required: " + field[index].mustin;
                fieldStr += "       }";
                fieldStr += "   },";
            }
        }
        fieldStr += "   align: 'left',";
        fieldStr += "   halign: 'center'";
        fieldStr += "},";
    });
    //去掉最后一个逗号
    if (fieldStr.length > 0 && fieldStr.charAt(fieldStr.length - 1) == ',')
        fieldStr = fieldStr.substring(0, fieldStr.length - 1);
    ret += "            columns: [[" + fieldStr + "]],";
    ret += "            onClickCell: " + table + "onClickCell,";
    ret += "            onDblClickRow: " + table + "onDblClickRow,";
    //ret += "            onClickRow: " + table + "onClickRow,";
    ret += "            frozenColumns: [[{field:'ck',checkbox:true}]]";
    ret += "        });"
    //渲染
    ret += "    }";

    //事件
    ret += "    function " + table + "onClickRow(index,row) {";
    ret += "            if (" + table + "editIndex != undefined) {";
    ret += "                if(!" + table + "Save()) {";
    //ret += "                    " + table + "dataGrid.datagrid('selectRow'," + table + "editIndex);";
    ret += "                    return;";
    ret += "                }";
    ret += "            }";
    //ret += "            " + table + "dataGrid.datagrid('selectRow',index);";
    ret += "            " + table + "Modify();";
    ret += "    }";

    ret += "    function " + table + "onDblClickRow(index,row) {";
    ret += "            " + table + "Modify();";
    ret += "    }";

    ret += "    function " + table + "onClickCell(index,field,value) {";
    ret += "            if (" + table + "editIndex != undefined) {";
    ret += "                if(!" + table + "Save())";
    ret += "                    return;";
    ret += "            }";
    ret += "    }";

    //**** 获取数据JSON包 ****/
    ret += "    function " + table + "SaveData() {";
    ret += "        var rows = " + table + "dataGrid.datagrid('getRows');";
    ret += "        var json = '';";
    ret += "        json += '{\"total\":' + rows.length + ',';";
    //组装数据
    ret += "        json += '\"data\":[';";
    ret += "        for (var i = 0; i < rows.length; i++) {";
    ret += "            json += '{';";
    ret += "            if (rows[i].datapri == undefined)";
    ret += "                json += '\"datapri\":\"\",';";
    ret += "            else";
    ret += "                json += '\"datapri\":\"' + rows[i].datapri + '\",';";
    $(field).each(function (index) {
        ret += "            json += '\"" + field[index].fieldname + "\":\"' + rows[i]." + field[index].fieldname + " + '\",';";
    });
    ret += "            if(json.charAt(json.length - 1) == ',')";
    ret += "                json = json.substring(0,json.length-1);";
    ret += "            json += '},';";
    ret += "        }";
    ret += "        if(json.charAt(json.length - 1) == ',')";
    ret += "            json = json.substring(0,json.length-1);";
    ret += "        json += ']';";
    ret += "        json += '}';";
    ret += "        $('#" + table + "SaveData').val(json);";
    ret += "        " + table + "nums  = rows.length;";
    ret += "    }";

    //**** 新建 ****/
    ret += "    function " + table + "New() {";
    ret += "        try {";
    //判断是否还在编辑状态
    ret += "            if (" + table + "editIndex != undefined) {";
    //ret += "                alert('记录还处于编辑状态！', 'warning');";
    //ret += "                return;";
    ret += "                if(!" + table + "Save())";
    ret += "                    return;";
    ret += "            }";
    ret += "            var newRow = {};";
    ret += "            " + table + "defRow." + table + "__ZH = " + table + "nums + 1;";
    ret += "            $.extend(true, newRow, " + table + "defRow);";
    ret += "            " + table + "dataGrid.datagrid('appendRow', newRow);";
//    ret += "            $.extend(true, newRow, " + table + "defRow);";
//    ret += "            " + table + "dataGrid.datagrid('insertRow', {";
//    ret += "                index: 0,";
//    ret += "                row: newRow";
//    ret += "            });";
    //设置编辑行
    ret += "            " + table + "editIndex = " + table + "dataGrid.datagrid('getRowIndex',newRow);";
    ret += "            " + table + "dataGrid.datagrid('selectRow').datagrid('beginEdit', " + table + "editIndex);";
    ret += "            var editors = " + table + "dataGrid.datagrid('getEditors', " + table + "editIndex);";
    //定义事件
    //循环
    $(field).each(function (index) {
        if (field[index].helplink.length > 0) {
            ret += "    editors[" + index + "].target.bind('keydown',function(event) {";
            ret += "        ShowListHelpLink(" + table + "dataGrid,editors," + table + "editIndex,event,'" + field[index].helplink + "','" + table + "')";
            ret += "    });";
        }
    });
    //ret += "            $('#btn_new_" + table + presuffix + suffix + "').attr('disabled','disabled');";
    //ret += "            $('#btn_save_" + table + presuffix + suffix + "').attr('disabled','');";
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('new" + table + "出错，原因：' + e.Message);";
    ret += "        }";
    ret += "    }";

    //**** 修改 ****/
    //修改
    ret += "    function " + table + "Modify() {";
    ret += "        try {";
    //判断是否还在编辑状态
    ret += "            if (" + table + "editIndex != undefined) {";
    //ret += "                alert('记录还处于编辑状态！');";
    //ret += "                return;";
    ret += "                if(!" + table + "Save())";
    ret += "                    return;";
    ret += "            }";
    //判断是否已经选择
    ret += "            var rows = " + table + "dataGrid.datagrid('getSelected');";
    ret += "            if (!rows || rows.length == 0) {";
    ret += "                $.messager.alert('提示', '请先择需要操作的记录！', 'warning');";
    ret += "                return;";
    ret += "            }";
    //设置编辑行
    ret += "            " + table + "editIndex = " + table + "dataGrid.datagrid('getRowIndex', rows);";
    ret += "            " + table + "dataGrid.datagrid('selectRow').datagrid('beginEdit', " + table + "editIndex);";
    ret += "            var editors = " + table + "dataGrid.datagrid('getEditors', " + table + "editIndex);";
    //定义事件
    //循环
    $(field).each(function (index) {
        if (field[index].helplink.length > 0) {
            ret += "    editors[" + index + "].target.bind('keydown',function(event) {";
            ret += "        ShowListHelpLink(" + table + "dataGrid,editors," + table + "editIndex,event,'" + field[index].helplink + "','" + table + "')";
            ret += "    });";
        }
    });
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('modify" + table + "出错，原因：' + e.Message);";
    ret += "        }";
    ret += "    }";

    //**** 删除 ****/
    ret += "    function " + table + "Del() {";
    ret += "        try {";
    ret += "            if (" + table + "editIndex != undefined) {";
    ret += "                $.messager.confirm('询问', '记录还处于编辑状态，是否撤销编辑信息?', function (r) {";
    ret += "                    if (r) {";
    ret += "                        " + table + "dataGrid.datagrid('rejectChanges');";
    ret += "                        " + table + "editIndex = undefined;";
    ret += "                    }";
    ret += "                });";
    ret += "            }";
    ret += "            else {";
    ret += "                var row = " + table + "dataGrid.datagrid('getSelected');";
    ret += "                if (!row || row.length == 0) {";
    ret += "                    alert('请先择需要操作的记录！');";
    ret += "                    return;";
    ret += "                }";
    ret += "                $.messager.confirm('提示', '是否删除选中数据?', function (r) {";
    ret += "                    if (!r) {";
    ret += "                        return;";
    ret += "                    }";
    ret += "                var rowIndex = " + table + "dataGrid.datagrid('getRowIndex', row);";
    ret += "                " + table + "dataGrid.datagrid('deleteRow', rowIndex);";
    ret += "                " + table + "SaveData();";
    ret += "                });";
    ret += "            }";
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('del" + table  + "出错，原因：' + e.Message);";
    ret += "        }";
    ret += "    }";

    //**** 保存 ****/
    //保存
    ret += "    function " + table + "Save() {";
    ret += "        try {";
    //判断是否还在编辑状态 
//    ret += "            if (" + table + "editIndex == undefined) {";
//    ret += "                alert('记录未处于编辑状态！', 'warning');";
//    ret += "                return false;";
//    ret += "            }";

    ret += "            if (!" + table +"dataGrid.datagrid('validateRow', " + table + "editIndex)) {";
    ret += "                alert('请输入必输项！', 'warning');";
    ret += "                return false;";
    ret += "            }";
    //结束编辑
    ret += "            " + table + "dataGrid.datagrid('endEdit', " + table + "editIndex);";
    ret += "            " + table + "dataGrid.datagrid('acceptChanges');";
    ret += "            " + table + "editIndex = undefined;";
    ret += "            " + table + "SaveData();";
    ret += "            return true";
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('" + table + "Save出错，原因：' + e.Message);";
    ret += "            return false";
    ret += "        }";
    ret += "    }";

    //复制
    ret += "    function " + table + "Copy() {";
    ret += "        try {";
    ret += "            if (" + table + "editIndex != undefined) {";
    //ret += "                alert('记录还处于编辑状态！');";
    //ret += "                return;";
    ret += "                if(!" + table + "Save())";
    ret += "                    return;";
    ret += "            }";
    ret += "            var rows = " + table + "dataGrid.datagrid('getSelected');";
    ret += "            if (!rows || rows.length == 0) {";
    ret += "                $.messager.alert('提示', '请先择需要操作的记录！', 'warning');";
    ret += "                return;";
    ret += "            }";
    ret += "            var newRow = {};";
    ret += "            $.extend(true, newRow, rows);";
    ret += "            newRow." + table + "__ZH = " + table + "nums + 1;";
    //清除原有主键
    ret += "            newRow.datapri = \"\";";
    ret += "            " + table + "dataGrid.datagrid('appendRow',newRow);";
//    ret += "            " + table + "dataGrid.datagrid('insertRow', {";
//    ret += "                index: 0,";
//    ret += "                row: newRow";
//    ret += "            });";
    ret += "            " + table + "SaveData();";
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('copy" + table + "出错，原因：' + e.Message);";
    ret += "        }";
    ret += "    }";

    //复制多组
    ret += "    function " + table + "CopyNum() {";
    ret += "        try {";
    ret += "            if (" + table + "editIndex != undefined) {";
    //ret += "                alert('记录还处于编辑状态！');";
    //ret += "                return;";
    ret += "                if(!" + table + "Save())";
    ret += "                    return;";
    ret += "            }";
    ret += "            var rows = " + table + "dataGrid.datagrid('getSelected');";
    ret += "            if (!rows || rows.length == 0) {";
    ret += "                $.messager.alert('提示', '请先择需要操作的记录！', 'warning');";
    ret += "                return;";
    ret += "            }";
    //输入复制数量
    ret += "            layer.prompt({ title: '请输入复制数量？', formType: 0, value: '1' }, function (num, index) {";
    ret += "                if (!isPositiveNum(num)) {";
    ret += "                    alert('内容【'+ num + '】为无效整数！');"; 
    ret += "                    return;";   
    ret += "                }";
    ret += "                var copyNum = parseInt(num);";
    ret += "                var newRow;";
    ret += "                for (var i = 1; i <= copyNum; i++) {";
    ret += "                    newRow = {};";
    ret += "                    $.extend(true, newRow, rows);";
    ret += "                    newRow." + table + "__ZH = " + table + "nums + i;";
    //清除原有主键
    ret += "                    newRow.datapri = \"\";";
    ret += "                    " + table + "dataGrid.datagrid('appendRow', newRow);";
//    ret += "                    " + table + "dataGrid.datagrid('insertRow', {";
//    ret += "                        index: 0,";
//    ret += "                        row: newRow";
//    ret += "                    });";
    //ret += "                    alert(JSON.stringify(newRow));";
    ret += "                }";
    ret += "                " + table + "SaveData();";
    ret += "                alert('复制成功！');";
    ret += "                layer.close(index);";
    ret += "            });";
    ret += "        }";
    ret += "        catch (e) {";
    ret += "            alert('copy" + table + "出错，原因：' + e.Message);";
    ret += "        }";
    ret += "    }";

    ret += "</script>";

    return ret;
}

//*************************************创建从表JS**********************************
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
                url: "/DataInput/GetCtrlStringLeftListData",
                dataType: "json",
                async: false,
                data: { ctrlString: ctrlstring },
                success: function (val) {
                    if (val.success) {
                        //获取目标控件
                        var ctrlObj = GetListControl(dataGrid, editIndex, targetctrl);
                        //判断控件类型
                        if (ctrlObj.type == "combobox") {
                            ctrlObj.target.combobox("clear");
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

//界面根据ID获取控件类型
function GetListControlType(dataGrid, editIndex, name) {
    name = name.replace(".", "__");
    var cellEditor = dataGrid.datagrid('getEditor', { index: editIndex, field: name });
    return cellEditor.type;
}

//界面根据ID获取对象控件
function GetListControl(dataGrid, editIndex, name) {
    name = name.replace(".", "__");
    return dataGrid.datagrid('getEditor', { index: editIndex, field: name });
}

//界面根据ID设置控件值
function SetListControlValue(dataGrid, editIndex, name, value) {
    name = name.replace(".", "__");
    var cellEditor = dataGrid.datagrid('getEditor', { index: editIndex, field: name });
    if (cellEditor.type == "combobox") {

    }
    //文本,验证框
    else if (cellEditor.type == "validatebox") {
        cellEditor.target.val(value);
    }
    else {
        alert("未启用控件:" + cellEditor.type);
    }
}

//界面根据ID获取控件值
function GetListControlText(dataGrid, editIndex, name) {
    var val = "";
    name = name.replace(".", "__");
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
    name = name.replace(".", "__");
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

function GetSwitchListValidProc(js, suffix) {
    //&&record&&
    //var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    var regField = new RegExp("&&table&&", "g");
    return js.replace(regField, suffix);
}

//列表校验类型
function GetListCheckData(inputlx) { 
    var ret = "";
    if (inputlx == "date")
        ret = "cusdate";
    else if (inputlx == "datetime")
        ret = "cusdatetime";
    else if (inputlx == "int")
        ret = "cusint";
    else if (inputlx == "number")
        ret = "cusnumber";
    return ret;
}