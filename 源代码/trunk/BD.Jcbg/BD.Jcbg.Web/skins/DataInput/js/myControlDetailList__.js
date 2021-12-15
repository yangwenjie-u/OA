
//******************************//
var all_k=350;
var label_width=110;
var input_width = 240;

//**** 明细表列表层 ****
//动态创建JSList层
function CreateListJs(ptable, table, triggerfield, field, presuffix, suffix) {
    var ret = "";
    ret += "<script language='javascript'>";
    ret += "    var " + table + presuffix + suffix + "SaveData = '';";
    ret += "    var " + table + presuffix + suffix + "dataGrid;";
    ret += "    var " + table + presuffix + suffix + "editIndex = undefined;";

    //**** 定义函数 ****
    //定义grid
    ret += "    function " + table + presuffix + suffix + "InitGrid() {";
    ret += "        " + table + presuffix + suffix + "dataGrid = $('#" + table + presuffix + suffix + "datagrid').datagrid({";
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
        fieldStr += "   field: '" + field[index].fieldname + "',";
        fieldStr += "   title: '" + field[index].sy + "',";
        fieldStr += "   width: 100,";
        fieldStr += "   align: 'left',";
        fieldStr += "   halign: 'center'";
        fieldStr += "},";
    });
    //去掉最后一个逗号
    if (fieldStr.length > 0 && fieldStr.charAt(fieldStr.length - 1) == ',')
        fieldStr = fieldStr.substring(0, fieldStr.length - 1);
    ret += "            columns: [[" + fieldStr + "]],";
    ret += "            frozenColumns: [[{field:'ck',checkbox:true}]],";
    ret += "            toolbar: '#" + table + presuffix + suffix + "_toolbar'";
    ret += "        });"
    ret += "    }";
    //获取DataGrid的JSON格式
    ret += "    function " + table + "SetSaveData() {";
    ret += "        var rows = " + table + "dataGrid.datagrid('getRows');";
    ret += "        var json = '';";
    ret += "        json += '{\"total\":' + rows.length + ',';";

    ret += "    }";
    ret += "</script>";
    //base编码
    $.base64.utf8encode = false; 
    return $.base64.btoa(ret, true);
}

//动态创建DivList层
function CreateListDiv(ptable, table, hiddenfield, copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    //加载网页层
    ret += "<div id='" + table + "Div'>";   
    ret += CreateGridListDiv(table, hiddenfield, copyhiddenfield, field, presuffix, suffix);
    ret += "</div>";
    //加载JS层
    ret += "<div id='" + table + "JsDiv'>";
    ret += CreateListJs(ptable,table, "", field, presuffix, suffix);
    ret += "</div>";
    return ret;
}

function CreateGridListDiv(table, hiddenfield, copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    ret += "";
    ret += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
    ret += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
    ret += "<table id='" + table + presuffix + suffix + "datagrid'></table>";
    ret += "<div id='" + table + presuffix + suffix + "_toolbar' style='display: none;'>";
    ret += "    <a href='javascript:void(0)' id='btn_new_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-add' plain='true' onclick='new" + table + presuffix + suffix + "()'>添加</a>";
    ret += "    <a href='javascript:void(0)' id='btn_modify_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-edit' plain='true' onclick='modify" + table + presuffix + suffix + "()'>修改</a>";
    ret += "    <a href='javascript:void(0)' id='btn_save_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-save' plain='true' onclick='save" + table + presuffix + suffix + "()' disabled='disabled'>保存</a>";
    ret += "    <a href='javascript:void(0)' id='btn_cancel_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-cancel' plain='true' onclick='cancel" + table + presuffix + suffix + "()' disabled='disabled'>取消</a>";
    ret += "    <a href='javascript:void(0)' id='btn_copy_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-copy' plain='true' onclick='copy" + table + presuffix + suffix + "()'>复制</a>";
    ret += "    <a href='javascript:void(0)' id='btn_del_" + table + presuffix + suffix + "' class='easyui-linkbutton' iconCls='icon-remove' plain='true' onclick='del" + table + presuffix + suffix + "()'>删除</a>";
    ret += "</div>";
    return ret;
}

//*************************

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
function CreateDiv(table, hiddenfield,copyhiddenfield, field, presuffix, suffix) {
    var ret = "";
    ret += "<div id='" + table + "Div'>";
    ret += CreateField(table, hiddenfield,copyhiddenfield, field, presuffix, suffix);
    ret += "</div>";
    return ret;
}

//创建描述信息
function ctrlMsginfo(msg) {
	    msg = "<div class='zhushi'>" + msg + "</div>";
    return msg;
}


//动态创建表单
function CreateField(table,hiddenfield,copyhiddenfield, field, presuffix,suffix) {
//	alert(field.length);
    //当前动态生成的控件排号
    var ctrlNum = 0;
    var ret = "";
	//td,tr之前的所有代码
	var ret_s="";
	//表格的宽度
	var table_width;
	//判断是否有大行的存在
	var dayg_live=false;
	
    ret_s += "<input type='hidden' id='" + table + "CHECKDATA_" + suffix + "' name='" + table + "CHECKDATA_" + suffix + "' value='1'/>";
    ret_s += "<input type='hidden' id='" + table + "PRIDATA_" + suffix + "' name='" + table + "PRIDATA_" + suffix + "' value=''/>";
    //隐藏字段
    $(hiddenfield).each(function (index) {
        ret_s += "<input type='hidden' kjlx='hidden' id='" + hiddenfield[index].fieldname + presuffix + suffix + "' name='" + hiddenfield[index].fieldname + presuffix + suffix + "' value='" + hiddenfield[index].defval + "'/>"; 
    });
    //隐藏复制字段
    $(copyhiddenfield).each(function (index) {
        ret_s += "<input type='hidden' kjlx='hidden' id='" + copyhiddenfield[index].fieldname + presuffix + suffix + "' name='" + copyhiddenfield[index].fieldname + presuffix + suffix + "' value=''/>";
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
            //判断是否占一行前,判断是否前面已经有列,如果有,则自动先填充满
            if (ctrlNum > 0) {
                while (ctrlNum < conditionNum) {
                    ctrlNum = ctrlNum + 1;
                    //style='width:" + td_width + "px'
                    ret += "<td  class='stj_lr_td' style='width:0px;padding:0px;margin:0px ' > <label class='col-sm-3 control-label stj_label' style='width:0px;padding:0px;margin:0px '></label>";
                    ret += "<div class='col-sm-9 date stj_input' style='width:0px;padding:0px;margin:0px ' ></div> </td>";
                }
                ret += "</tr>";
                //当前列数清0
                ctrlNum = 0;
                //添加占整行数据行
                ret += fieldRowStr;
                fieldRowStr = "";
            }
            
            //************************************************************

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
                table_width = conditionNum * 350 - 110;
				if(table_width<590){table_width=590;}


            }
            else {
                table_width = (field.length - 1) * 350 - 110;
				if(table_width<590){table_width=590;}

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
                    ret += CreateHtmlTextarea(field[index], presuffix + suffix);
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
                case "COMBOBOX":
                    ret += CreateHtmlCombobox(field[index], presuffix + suffix);
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
	
	
	if(dayg_live==false)
	{
		if(field.length>conditionNum)
	{
    ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:"+(conditionNum*350)+"px;min-width:700px;' >";
	//table_width=conditionNum*450;
	//alert(field.length);
	}
	else
	{
		
		 ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:"+(field.length*350)+"px;min-width:700px;' >";
		// table_width=field.length*450;
		}
		
	}
	else
	{
		if((field.length-1)>conditionNum)
	{
    ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:"+(conditionNum*350)+"px;min-width:700px;' >";
	//table_width=conditionNum*450;
	//alert(field.length);
	}
	else
	{
		
		 ret_s += "<div class=\"table_wai\"><table class='stj_lr_tab' style='width:"+((field.length-1)*350)+"px;min-width:700px;' >";
		// table_width=field.length*450;
		}
		
		
		}
	
	ret_s=ret_s+ret+ "</table> </div>";

	// ret += "</table>";
    return ret_s;
}


//**** 创建控件 ****
//** HTML控件 **
//标签框LABEL
function CreateHtmlLabel(ctrl, suffix) {
    var ret = "";
    ret += "<div type='text' class='form-control background_tr no_xing' custom='custom' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' sy = '" + ctrl.sy + "' ";
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
                //ctrlstring = keyParam.value;
                ctrlstring += GetSwitchCtrlString(keyParam.value, suffix) + "||";
            }
        }
        //去掉功能项后面的||
        if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
            ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
        //添加事件
       // ret += " onChange=\"CtrlChange('" + GetSwitchCtrlString(ctrlstring, suffix) + "');\" ";
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" ";
    }
    else {
        ret += "onkeydown=\"ChangeEnterToTab();\" ";
    }

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
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');CtrlChange('" + ctrlstring + "');" + GetSwitchValidProc(js, suffix) + "\" ";
    else
        ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');CtrlChange('" + ctrlstring + "');\" ";
    //添加文本ctrlstring
    if (ctrlstring.length > 0)
        ret += "onchange=\"CtrlChange('" + ctrlstring + "');\" ";
    ret += "/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
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
        ret += "onkeydown=\"ShowHelpLinkForm('" + GetSwitchHelplink(helplink, suffix) + "');\" ";
    }
    //检验类型
    ret += "onblur=\"MyContentValidate('" + ctrl.fieldname + suffix + "');\" ";
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
            ret += "<input class='sousuo_btn_k_r' type='button' value='" + strFun[0] + "' onclick=\"DynamicImplement('" + PointQuotes(strFun[1]) + "');\"/>";
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
        ret += "<label for='" + ctrl.fieldname + suffix + iItem + "' style='padding-left: 2px;'>" + itemList[0] + "" + "</label>";
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
        ret += "<div class='radio radio-info radio-inline'>";
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
        ret += "/>";
        ret += "<label for='" + ctrl.fieldname + suffix + iItem + "' style='padding-left: 2px;'>" + itemList[0] + "</label>";   
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
        funStr = funStr.substring(0, funStr.length - 2)
    //去掉功能项后面的||
    if (ctrlstring.length > 0 && ctrlstring.charAt(ctrlstring.length - 1) == '|' && ctrlstring.charAt(ctrlstring.length - 2) == '|')
        ctrlstring = ctrlstring.substring(0, ctrlstring.length - 2);
    //添加事件
    //分析ctrlstring
    if (ctrlstring.length > 0) {
        ret += "onchange=\"CtrlChange('" + ctrlstring + "');\"";
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
function CreateHtmlCombobox(ctrl, suffix) {
    var ret = "";
    ret += "<div style='position:relative;'>";
    ret += "<input type='text' class='form-control mustinContent shuang-top-input' custom='custom' datain='&&datain&&' id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' mc = '" + ctrl.sy + "' ";
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
    ret += "/>";
    //隐藏下拉框
    ret += "<span style='width:18px;overflow:hidden;'>";
    ret += "<select class='form-control mustinContent' mc = '" + ctrl.sy + "' ";
    //判断控件长度
    if (ctrl.xscd != 0)
        ret += "style='margin-left:-100px;width:" + (ctrl.xscd + 32) + "px' ";
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
    //添加事件
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
    if (!dataReadonly && ctrl.mustin)
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
    if (!dataReadonly && ctrl.mustin)
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
    if (!dataReadonly && ctrl.mustin)
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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
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
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin'>*</font>";
    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
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
function CreateHtmlFile(ctrl, suffix) {
    var ret = "";
    ret += "<input type='hidden' custom='custom' datain='&&datain&&' "
    //控件信息
    ret += "kjlx='" + ctrl.kjlx + "' inputlx='" + ctrl.inputlx + "' def=''";
    ret += "id='" + ctrl.fieldname + suffix + "' name = '" + ctrl.fieldname + suffix + "' value='100'/>";
    //是否必输项
    if (!dataReadonly && ctrl.mustin)
        ret += "<font class='mustin_file'>*</font>";
    if (!dataReadonly)
        ret += "<input type='file' id='" + PointBlankRep(ctrl.fieldname) + suffix + "' name = '" + PointBlankRep(ctrl.fieldname) + suffix + "'/>";
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


    //描述说明
    if (ctrl.msginfo.length > 0)
        ret += ctrlMsginfo(ctrl.msginfo);
    return ret;
}




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

    html += "<div class=\"ryxx\" >" + sy + "</div>";
    //判断是否为只读,只读时不显示
    if(dataReadonly)
        html += "<div class='btn-group' style = 'display:none;'>";
    else
        html += "<div class='btn-group'>";
    html += " <button type=\"button\" class=\"btn btn-default\" id=\"" + table + "AddBtn\" name=\"" + table + "AddBtn\"  onclick=\"AddT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"><i class=\"fa fa-plus\"></i>添加</button>";
    html += " <button type=\"button\" class=\"btn btn-default\" id=\"" + table + "CopyBtn\" name=\"" + table + "CopyBtn\"   onclick=\"CopyT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"> <i class='fa fa-copy'> </i>复制</button>";
    /*html += " <button type=\"button\" class=\"btn btn-default\" id=\"" + table + "CopyNumsBtn\" name=\"" + table + "CopyNumsBtn\"   onclick=\"CopyNumsT2Page('" + table + "','" + sy + "'," + hasDetail + ",'" + detail + "','" + detailsy + "');\"> <i class='fa fa-copy'> </i>复制多组</button>";*/
    /*html += " <button type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "DelBtn\" name=\"" + table + "DelBtn\"  onclick=\"alert('" + table + "');DelT2Page('" + table + "');\" >删除</button>";*/




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
	//style='border-width:0px'
    lab += "<li class='active' mc='" + table + "Tab_" + record + "Item'>";
    //lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true'>" + sy + record + "<a onclick='$(\"#" + table + "DelBtn\").click()'>*</a></a>";
    lab += "    <a data-toggle='tab' href='#" + table + "Tab-" + record + "' aria-expanded='true' style='background-color:transparent;border-width:0px;'>" + sy + record +"</a>";
    //</a>
    //不只读时才显示删除
    if (!dataReadonly)
        lab += "    <a  onclick=\"DelT2Page('" + table + "'," + record + ");\" onfocus=\"this.blur();\" style='padding: 0px;border-width: 0px;  background-color:transparent;'><i class='fa fa-close'></i></a>";
    lab += "</li>";
    $("#" + table + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "Tab-" + record + "' mc='" + table + "Tab_" + record + "Content' class='tab-pane active'>";
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
        content += GetT3Tab(table,detail, detailsy, record);
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

//复制多组Tab项
function CopyNumsT2Page(table, sy, hasDetail, detail, detailsy) {
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
                //if ($(this).hasClass("tab-pane active"))
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

//** 明细 **
//从表Tab项模板
function GetT3Tab(tablab,table, sy, rec) {
    var html = "";
    //当前Tab项记录数
    //添加锚点
    html += "<a id=\"" + table + "Anchor\" name=\"" + table + "Anchor\"></a>";
    html += "<input type=\"hidden\" id=\"" + tablab + "_" + rec + "Record\" name=\"" + tablab + "_" + rec + "Record\" value=\"0\"/>";
    html += "<div class=\"tabs-container\">";
	
	 html += "   <div class=\"add_del_stj\">"


	 html += "<div class=\"ryxx\" >" + sy + "</div>";
	 if (dataReadonly) 
        html += "<div class='btn-group' style = 'display:none;'>";
     else
	    html += "<div class='btn-group'>";
	html += " <button type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "_" + rec + "btn\" name=\"" + table + "_" + rec + "btn\"  onclick=\"AddT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\"><i class=\"fa fa-plus\"></i>添加</button>";
	html += " <button type=\"button\" class=\"btn btn-default btn-sm\" id=\"" + table + "_" + rec + "CopyBtn\" name=\"" + table + "CopyBtn\"    onclick=\"CopyT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\"> <i class='fa fa-copy'> </i>复制</button>";


	/*
	html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "_" + rec + "btn\" name=\"" + table + "_" + rec + "btn\" value='添加' onclick=\"AddT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\" />";
	html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" id=\"" + table + "_" + rec + "CopyBtn\" name=\"" + table + "_" + rec + "CopyBtn\" value='复制' onclick=\"CopyT3Page('" + tablab + "','" + table + "','" + sy + "'," + rec + ");\" />";

	html += "   <input class=\"btn btn-primary btn-sm\" style=\" margin-right: 10px;\" type=\"button\" value='删除' onclick=\"DelT3Page('" + table + "'," + rec + ");\" />";
	
	*/
	html += "   </div>"
	
	
	  html +="</div>";
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
	lab += "<a  onclick=\"DelT3Page('" + table + "'," + rec + ");\" onfocus=\"this.blur();\" style='padding: 0px;border-width: 0px; background-color:transparent;'><i class='fa fa-close'></i></a>";
	
    lab += "</li>";
    $("#" + table + "_" + rec + "Nav ul").append(lab);
    //加tab内容项
    var content = "";
    content += "<div id='" + table + "_" + rec + "Tab-" + record + "' class='tab-pane active'>";
    content += "    <div class='panel-body' style='background-color:#dff0fa'>";
    //字段内容
    var fieldcontent = $("#" + table + "Div").html();
    var regField = new RegExp("&amp;&amp;record&amp;&amp;", "g");
    fieldcontent = fieldcontent.replace(regField, rec + "_" + record);
    var regcodeField = new RegExp("&&record&&", "g");
    fieldcontent = fieldcontent.replace(regcodeField, rec + "_" + record);
    //替换数据属性
    var dataField = new RegExp("&amp;&amp;datain&amp;&amp;", "g");
    fieldcontent = fieldcontent.replace(dataField, "datain")
    //赋值字符串
    content += fieldcontent;
    content += "    </div>";
    content += "</div>";
    //添加JS
    var jscontent = $("#" + table + "JsDiv").html();
    //解码
    $.base64.utf8encode = false;
    jscontent = $.base64.atob(jscontent, true); 
    jscontent = jscontent.replace(regField, rec + "_" + record);
    jscontent = jscontent.replace(regcodeField, rec + "_" + record);
    content += jscontent;
    $("#" + table + "_" + rec + "Tab").append(content);
    //记录数加1
    $("#" + tablab + "_" + rec + "Record").val(record);
    //初始化函数
    eval(table + "_" + rec + "_" + record + "InitGrid();");
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






//*************************************文件*****************************************
//构建文件函数
function UploadFileInit(fieldname) {
    $('#' + PointBlankRep(fieldname)).uploadify({
        'swf': '/skins/DataInput/pub/uploadify/uploadify.swf',
        'uploader': '/DataInput/FileService?method=UploadFile',
        'cancelImg': '/skins/DataInput/pub/uploadify/uploadify-cancel.png',
        'buttonText': '请选择文件',
        'fileTypeExts': '*.*',
        'fileTypeDesc': '有效文档',
        'fileSizeLimit': '2048000KB',
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
            $("#" + PointRep(fieldname)).val(filevalue); ;
            //控件名，当前选择的文件名及上传后返回的文件ID
            showUpFile(PointBlankRep(fieldname), val.fileid, val.filename);
        },
        'onUploadError': function (file, errorCode, errorMsg, errorString) {
            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
        }
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
        }
        else {
            if (dataReadonly)
                newcontent = "<div class='wfa_frame_div2' ";
            else
                newcontent = "<div class='wfa_frame_div' ";
        }
        newcontent = newcontent + "id='file_div_" + fileid + "'><span class='wfa_text'><a href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + ctrlid + "_" + tail + "' title='" + filename + "'>";
        //判断是否为图片
        if (isImage(filename.toLowerCase())) {
            newcontent = newcontent + "<img height='100px' width='100px' src='/DataInput/FileService?method=DownloadFile&type=small&fileid=" + fileid + "'/>";
        }
        else {
            newcontent = newcontent + filename;
        }
        newcontent = newcontent + "</a></span>";
        //只读时不显示删除图标
        if (!dataReadonly)
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
    }
    catch (err) {
        $.messager.alert('提示', err, 'info');
    }
}

//判断图片
function isImage(filename) {
    var ret = false;
    if (filename.indexOf(".jpg") || filename.indexOf(".jpg"))
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
