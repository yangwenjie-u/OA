//检验录入列表保存
function CheckListSaveFun() {
    var ret = false;
    if ((typeof ListSaveFun) === "function")
        ret = ListSaveFun();
    else
        ret = true;
    return ret;
}



//检验函数是否存在,提交之前判断
function CheckCustomFun() {
    var ret = false;
    if ((typeof CustomFun) === "function")
        ret = CustomFun();
    else
        ret = true;
    return ret;
}



//提交之后判断
function CheckAfterSubmitFun() {
    var ret = false;
    if ((typeof AfterSubmitFun) === "function")
        ret = AfterSubmitFun();
    else
        ret = true;
    return ret;
}

//回车跳转下一格事件
/* enter键转换成tab键 */

function ChangeEnterToTab(event) {
    if (event.keyCode == 13) {
        var obj = document.activeElement;
        var tagName = obj.tagName.toLowerCase();
        if (tagName == "input" &&
            (obj.getAttribute("type").toLowerCase() == "button" || obj.getAttribute("type").toLowerCase() == "submit")) {} else {
            event.keyCode = 9;
        }
    }
}

//检测控件检验类型
function MyContentValidate(ctrlName) {
    // 返回值
    var ret = true;
    //获取控件类型
    var mc = GetCtrlMc(ctrlName);
    var kjlx = GetCtrlType(ctrlName);
    var inputType = GetCtrlInputType(ctrlName);
    var value = GetCtrlValue(ctrlName);
    var mustin = GetCtrlMustIn(ctrlName);
    //错误提醒
    var err = "【" + mc + "】：";
    //判断是否为必输
    if (mustin != "mustin" && value == "")
        return ret;
    //如果是必输
    if (mustin != "mustin" && value == "") {
        err += "无效的身份证号。";
        alert(err);
        ret = false;
        return ret;
    }
    //判断检验类型
    switch (inputType) {
        //手机号
        case "PHONE":
            if (!PhoneValidate(value)) {
                err += "无效的手机号。";
                alert(err);
                ret = false;
            }
            break;
            //银行卡
        case "BANKCARD":
            if (!BankcardValidate(value)) {
                err += "无效的银行卡号。";
                alert(err);
                ret = false;
            }
            break;
            //身份证号
        case "IDCARD":
            if (!IdCardValidate(value)) {
                err += "无效的身份证号。";
                alert(err);
                ret = false;
            }
            break;
            //mm:ss
        case "TIME":
            //判断是否为时间
            if (!IsHourMinute(value)) {
                err += "无效的小时:分钟值，日期格式应该为：mm:ss。";
                alert(err);
                ret = false;
            }
            break;
            //日期
        case "DATE":
            if (!IsDate(value)) {
                err += "无效的日期值，日期格式应该为：yyyy-mm-dd、yy-mm-dd、yyyy/mm/dd、yy/mm/dd、yyyy\\mm\\dd、yy\\mm\\dd、yyyymmdd、yymmdd中的一种。";
                alert(err);
                ret = false;
            }
            break;
            //日期时间
        case "DATETIME":
            //是否日期
            if (!IsDateTime(value)) {
                err += "无效的日期时间值，日期格式应该为：yyyy-mm-dd hh:mm:ss。";
                alert(err);
                ret = false;
            }
            break;
            //整数
        case "INTEGER":
            if (!IsInt(value)) {
                err += "无效的整数。";
                alert(err);
                ret = false;
            }
            break;
            //数字
        case "NUMBER":
            //判断是否为数字
            if (!IsNumeric(value)) {
                err += "无效的数字。";
                alert(err);
                ret = false;
            }
            break;
    }
    return ret;
}

//********检验**********
//手机号判断
function PhoneValidate(phone) {
    var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(15[0-9]{1})||(17[0-9]{1})(18[0-9]{1}))+\d{8})$/;
    return myreg.test(phone);
}

//银行卡判断
function BankcardValidate(bankno) {
    bankno = bankno.replace(/ /g, "").trim(); //去掉字符串头尾空格 

    var lastNum = bankno.substr(bankno.length - 1, 1); //取出最后一位（与luhm进行比较）

    var first15Num = bankno.substr(0, bankno.length - 1); //前15或18位
    var newArr = new Array();
    for (var i = first15Num.length - 1; i > -1; i--) { //前15或18位倒序存进数组
        newArr.push(first15Num.substr(i, 1));
    }
    var arrJiShu = new Array(); //奇数位*2的积 <9
    var arrJiShu2 = new Array(); //奇数位*2的积 >9

    var arrOuShu = new Array(); //偶数位数组
    for (var j = 0; j < newArr.length; j++) {
        if ((j + 1) % 2 == 1) { //奇数位
            if (parseInt(newArr[j]) * 2 < 9)
                arrJiShu.push(parseInt(newArr[j]) * 2);
            else
                arrJiShu2.push(parseInt(newArr[j]) * 2);
        } else //偶数位
            arrOuShu.push(newArr[j]);
    }

    var jishu_child1 = new Array(); //奇数位*2 >9 的分割之后的数组个位数
    var jishu_child2 = new Array(); //奇数位*2 >9 的分割之后的数组十位数
    for (var h = 0; h < arrJiShu2.length; h++) {
        jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
        jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
    }

    var sumJiShu = 0; //奇数位*2 < 9 的数组之和
    var sumOuShu = 0; //偶数位数组之和
    var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
    var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
    var sumTotal = 0;
    for (var m = 0; m < arrJiShu.length; m++) {
        sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
    }

    for (var n = 0; n < arrOuShu.length; n++) {
        sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
    }

    for (var p = 0; p < jishu_child1.length; p++) {
        sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
        sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
    }
    //计算总和
    sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

    //计算Luhm值
    var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
    var luhm = 10 - k;

    if (lastNum == luhm) {
        return true;
    } else {
        return false;
    }

}

//身份证
/* 判断字符串特性 */
/* 判断是否为身份证号 */
var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1]; // 加权因子   
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2]; // 身份证验证位值.10代表X   
function IdCardValidate(idCard) {
    idCard = idCard.replace(/ /g, "").trim(); //去掉字符串头尾空格                     
    if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard); //进行15位身份证的验证    
    } else if (idCard.length == 18) {
        var a_idCard = idCard.split(""); // 得到身份证数组   
        if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) { //进行18位身份证的基本验证和第18位的验证
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
/**  
 * 判断身份证号码为18位时最后的验证位是否正确  
 * @param a_idCard 身份证号码数组  
 * @return  
 */
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0; // 声明加权求和变量   
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10; // 将最后位为x的验证码替换为10方便后续操作   
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i]; // 加权求和   
    }
    valCodePosition = sum % 11; // 得到验证码所位置   
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    } else {
        return false;
    }
}
/**  
 * 验证18位数身份证号码中的生日是否是有效生日  
 * @param idCard 18位书身份证字符串  
 * @return  
 */
function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 这里用getFullYear()获取年份，避免千年虫问题   
    if (temp_date.getFullYear() != parseFloat(year) ||
        temp_date.getMonth() != parseFloat(month) - 1 ||
        temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
/**  
 * 验证15位数身份证号码中的生日是否是有效生日  
 * @param idCard15 15位书身份证字符串  
 * @return  
 */
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
    if (temp_date.getYear() != parseFloat(year) ||
        temp_date.getMonth() != parseFloat(month) - 1 ||
        temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}

//是否整型
function IsInt(ctlcontent) {
    var reg = /^-?(\d+)$/;
    return reg.exec(ctlcontent);
}

//是否数字
function IsNumeric(ctlcontent) {
    var reg = /^-?(\d+\.?\d*)$/;
    return reg.exec(ctlcontent);
}

//判断是否为时间
function IsHourMinute(ctlcontent) {
    var reg = /^(([01]?[0-9])|(2[0-3])):[0-5]?[0-9]$/;
    return reg.test(ctlcontent);
}

//判断日期时间
function IsDateTime(ctlcontent) {
    var reg = /^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s((([0-1][0-9])|(2?[0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$/;
    return reg.test(ctlcontent);
}

//是否日期
function IsDate(ctlcontent) {
    var ret = false;

    var strOrg = ctlcontent;
    var arrDate = null;
    var nYear;
    var nMonth;
    var nDay;
    do {
        if (strOrg.length == 0)
            break;
        // yyyy-mm-dd or yy-mm-dd
        if (strOrg.indexOf("-") > -1) {
            var arrDate = strOrg.split("-");
            if (arrDate.length != 3)
                break;
            if (arrDate[0].length == 2) {
                arrDate[0] = "20" + arrDate[0];
            }

        }
        // yyyy/mm/dd or yy/mm/dd
        else if (strOrg.indexOf("/") > -1) {
            var arrDate = strOrg.split("/");
            if (arrDate.length != 3)
                break;
            if (arrDate[0].length == 2) {
                arrDate[0] = "20" + arrDate[0];
            }
        }
        // yyyy\mm\dd or yy\mm\dd
        else if (strOrg.indexOf("\\") > -1) {
            var arrDate = strOrg.split("\\");
            if (arrDate.length != 3)
                break;
            if (arrDate[0].length == 2) {
                arrDate[0] = "20" + arrDate[0];
            }
        }
        // yyyymmdd or yymmdd
        else if (strOrg.length == 6 || strOrg.length == 8) {
            arrDate = new Array();
            if (strOrg.length == 6) {
                strOrg = "20" + strOrg;
            }
            arrDate.push(strOrg.substr(0, 4));
            arrDate.push(strOrg.substr(4, 2));
            arrDate.push(strOrg.substr(6, 2));
        } else
            break;

        if (arrDate == null)
            break;
        nYear = arrDate[0] * 1;
        nMonth = arrDate[1] * 1;
        nDay = arrDate[2] * 1;
        if (isNaN(nYear) || isNaN(nMonth) || isNaN(nDay))
            break;

        var a = new Date(nYear, nMonth - 1, nDay);
        var y = a.getFullYear();
        var m = a.getMonth() + 1;
        var d = a.getDate();

        if (!(y == nYear && m == nMonth && d == nDay))
            break;

        strOrg = y + "-" + m + "-" + d;

        ret = true;
    } while (false);

    if (event.srcElement.type == "button")
        return ret;
    if (ret)
        event.srcElement.value = strOrg;
    else {
        var d = new Date();
        var strD = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
        event.srcElement.value = strD;
    }

    return ret;
}

//****************** 共用部分 **************************
//**** 全局变量 ****
var conditionNum = 3;
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



//******************控件后缀转换*********************
//CtrlString转换
function GetSwitchCtrlString(ctrlstring) {
    return 'ctrlChange--' + ctrlstring;
}

//helplink转换
function GetSwitchHelplink(helplink) {
    return helplink;
}


//validProc焦点移开事件
function GetSwitchValidProc(js, suffix) {

    var regField = new RegExp("&&record&&", "g");
    return js.replace(regField, suffix);
}
//***************************************************
//**** 分析控件 ****
//获取控件必输项
function GetCtrlMustIn(name) {
    return $("#" + PointRep(name)).attr("mustin");
}
//获取控件类型

// parent.SetCtrlValue('I_S_GC_JLDW.QYBH', row.QYBH);
// parent.InitCtrlEvent('I_S_GC_JLDW.QYBH');


function GetCtrlType(name) {
    if ($("#" + PointRep(name)).attr("kjlx") == undefined)
        return $('input[name="' + PointRep(name) + '"]').attr("kjlx");
    else
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

//重新组装没有前缀的字段的表名
function SwitchFieldName(name) {
    $("input[name$='." + name + "']").each(function(i, o) {
        name = $(o).attr("id");
    });
    return name;
}

//获取控件值
function GetCtrlValue(name) {
    //重新获取名称
    if (name.indexOf(".") == -1)
        name = SwitchFieldName(name);
    var value = "";
    var kjlx = GetCtrlType(name);
    name = PointRep(name);
    //****判断控件*****
    //HTML
    if (kjlx == "LABEL") {
        value = $("#" + name).html();
    } else if (kjlx == "TEXT") {
        value = $("#" + name).val();
    } else if (kjlx == "SELECT") {
        value = $("#" + name).val();
    } else if (kjlx == "CHECKBOX") {
        var chk_value = [];
        $('input[name="' + name + '"]:checked').each(function() {
            chk_value.push($(this).val());
        });
        value = chk_value;
    } else if (kjlx == "RADIO") {
        value = $("input[name='" + name + "']:checked").val();
    }
    //EasyUI
    else if (kjlx == "EASYCOMBOGRID") {
        value = $("#" + name).combogrid("getValue");
    } else if (kjlx == "EASYCOMBOBOX") {
        value = $("#" + name).combobox("getValue");
    } else if (kjlx == "EASYDATE") {
        value = $("#" + name).datebox("getValue");
    } else if (kjlx == "EASYDATETIME") {
        value = $("#" + name).datetimebox("getValue");
    }
    //**** BOOTSTRAP ****
    else if (kjlx == "BOOTSTRAPSELECT") {
        value = $("#" + name).val();
    } else if (kjlx == "BOOTSTRAPMULSELECT" || kjlx == "BOOTSTRAPSINGSELECT") {
        value = $("#" + name).val();
    } else
        value = $("#" + name).val();
    return value;
}

//设置控件值
function SetCtrlValue(name, value) {

    console.log("key:" + name);
    console.log(value);

    //2级从表
    var tmp = $('#main').find('[name="' + name + '" ]:visible');
    if (tmp.parents('.table').length) { //3级明细表
        $('#main').find('.active:visible').find('[name="' + name + '" ]').val(value);
    } else { //2级从表
        tmp.val(value);
    }
}


//初始化控件原型数据
function SetCtrlMode(name, value) {
    var kjlx = GetCtrlType(name);
    var sourename = name;
    name = PointRep(name);
    //复选框
    if (kjlx == "CHECKBOX") {
        //清空原有内容
        $("#" + name + "Div").empty();
        var cbStr = $("#" + name + "HiddenDiv").html();
        //Custom
        var customField = new RegExp("&amp;&amp;custom&amp;&amp;", "g");
        //Item
        var itemField = new RegExp("&amp;&amp;iItem&amp;&amp;", "g");
        //名称
        var itemmcField = new RegExp("&amp;&amp;itemmc&amp;&amp;", "g");
        //值
        var itemvalueField = new RegExp("&amp;&amp;itemvalue&amp;&amp;", "g");
        var index = 0;
        $(value).each(function(inde) {
            index++;
            var cbStrTmp = cbStr;
            cbStrTmp = cbStrTmp.replace(customField, "custom");
            cbStrTmp = cbStrTmp.replace(itemField, index);
            cbStrTmp = cbStrTmp.replace(itemmcField, value[inde].content);
            cbStrTmp = cbStrTmp.replace(itemvalueField, value[inde].value);
            $("#" + name + "Div").append(cbStrTmp);
        });
        //如果有多值,则设置默认值
        if (value.length > 0) {
            //设置下拉框默认值
            SetCtrlValue(sourename, GetDefValue(sourename + "1"));
        }
    }
    //下拉框
    else if (kjlx == "SELECT") {
        //清空原有内容
        $("#" + name).empty();
        $(value).each(function(inde) {
            $("#" + name).append("<option value='" + value[inde].value + "'>" + value[inde].content + "</option>");
        });
        //判断是否长度大于0
        if (value.length > 0) {
            SetCtrlValue(sourename, value[0].value);
            //$("#" + name + " option").eq(0).attr("selected", true);
        }
    }
    //下拉多选框
    else if (kjlx == "BOOTSTRAPSELECT") {
        $("#" + name).html("");
        $("#" + name).chosen("destroy");
        $(value).each(function(inde) {
            $("#" + name).append("<option value='" + value[inde].value + "'>" + value[inde].content + "</option>");
        });
        $("#" + name).attr("multiple", true);
        $("#" + name).chosen({
            no_results_text: "未找到此选项!"
        });
    }
    //下拉过滤多选框
    else if (kjlx == "BOOTSTRAPMULSELECT") {
        //清空原有内容
        $("#" + name).empty();
        $(value).each(function(inde) {
            $("#" + name).append("<option value='" + value[inde].value + "'>" + value[inde].content + "</option>");
        });
        $("#" + name).multiselect('rebuild');
    }
}

//初始化特殊控件
function InitSpecialCtrl(name) {
    //获取控件类型
    var kjlx = GetCtrlType(name);
    if (kjlx == "FILE") {
        //eval("UPLOADFILE_" + PointBlankRep(name) + "();");
        UploadFileInit(name);
    } else if (kjlx == "IMAGEFILE") {
        UploadImageFileInit(name);
    }
    //下拉多选过滤
    else if (kjlx == "BOOTSTRAPMULSELECT") {
        BootstrapMulSelectInit(name);
    }
    //下拉单选过滤
    else if (kjlx == "BOOTSTRAPSINGSELECT") {
        BootstrapSingleSelectInit(name);
    }
}

//初始化触发控件改变事件
function InitCtrlEvent(name) {
    //判断控件类型
    var kjlx = GetCtrlType(name);
    if (kjlx == "hidden") {
        return;
    }
    name = PointRep(name);
    //==var ctrlhtml = $("input[name='" + name + "']").parent();
    var ctrlhtml = $("#" + name).parent();
    var strhtml = ctrlhtml.html();
    if (strhtml != undefined && strhtml.toLowerCase().indexOf("onchange") > -1 && strhtml.toLowerCase().indexOf("combobox_tmp") == -1) {
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
    if (kjlx == "RADIO") {
        return;
    }
    var value = "";
    if (kjlx == "CHECKBOX")
        value = GetDefValue(name + "0");
    else
        value = GetDefValue(name);
    //如果是下拉框,默认值为空,不处理
    if (kjlx == "SELECT" && value == "") {
        return;
    }
    SetCtrlValue(name, value);
}


//********************保存数据********************
//检验数据
function IsSubmitFormValid() {
    var ret = true;
    //触发事件
    $("input[custom='custom']").each(function(i) {
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
        $("[custom='custom'][mustin='mustin'][datain='datain']").each(function(i) {
            var ctrl = $("[custom='custom'][mustin='mustin'][datain='datain']").eq(i);
            //获取值
            var value = GetCtrlValue(GetNameByCtrl(ctrl)); //alert(GetNameByCtrl(ctrl) + ' ' + ctrl.attr("mc") + ' ' + value + ' ' + GetNameByCtrl(ctrl));
            if (value == undefined || value == null || value == "") {
                alert(ctrl.attr("mc") + "不能为空！");
                ctrl.focus().select();
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
    //执行录入表列保存函数
    if (!CheckListSaveFun())
        return;
    //记录DataGrid数据
    DGSetSaveData();
    //定义提交按钮
    $("#btnContent").val("ZC");
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function(val) {
            //遮罩
            layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            } else {
                alert(val.msg);
                parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function(XMLHttpRequest, textStatus) {},
        beforeSend: function(XMLHttpRequest) {
            //遮罩
            layer.load();
        }
    });
}

//保存
function btn_opt_bc(url) {
    //检验数据
    if (!IsSubmitFormValid())
        return;
    //执行自定义函数
    if (!CheckCustomFun())
        return;
    //执行录入表列保存函数
    if (!CheckListSaveFun())
        return;
    //记录DataGrid数据
    DGSetSaveData();
    $("#btnContent").val("BC");
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function(val) {
            //遮罩
            layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            } else {
                alert(val.msg);
                //关闭窗口
                parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function(XMLHttpRequest, textStatus) {},
        beforeSend: function(XMLHttpRequest) {
            //遮罩
            layer.load();
        }
    });
}

//提交
function btn_opt_tj(url) {
    //检验数据
    if (!IsSubmitFormValid())
        return;
    //执行自定义函数
    if (!CheckCustomFun())
        return;
    //执行录入表列保存函数
    if (!CheckListSaveFun())
        return;
    //记录DataGrid数据
    DGSetSaveData();
    $("#btnContent").val("TJ");
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function(val) {
            //遮罩
            layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            } else {
                //判断值是否为空
                if (val.msg.trim() != "")
                    alert(val.msg);
                //执行回调函数
                if (val.data.callback != undefined) {
                    eval(val.data.callback);
                }
                //关闭窗口
                if (CheckAfterSubmitFun())
                    parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function(XMLHttpRequest, textStatus) {},
        beforeSend: function(XMLHttpRequest) {
            //遮罩
            layer.load();
        }
    });
}

//单组提交
function btn_opt_dztj(url) {
    //检验数据
    if (!IsSubmitFormValid())
        return;
    //执行自定义函数
    if (!CheckCustomFun())
        return;
    //执行录入表列保存函数
    if (!CheckListSaveFun())
        return;
    //记录DataGrid数据
    DGSetSaveData();
    $("#btnContent").val("DZTJ");
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    $.ajax({
        type: "POST",
        url: "/DataInput/SaveData",
        dataType: "json",
        data: $("#form1").serialize(), //$("#form1").serializeObject(),
        success: function(val) {
            //遮罩
            layer.closeAll("loading");
            if (!val.success) {
                alert(val.msg);
                return;
            } else {
                //判断值是否为空
                if (val.msg.trim() != "")
                    alert(val.msg);
                //执行回调函数
                if (val.data.callback != undefined) {
                    eval(val.data.callback);
                }
                //关闭窗口
                if (CheckAfterSubmitFun())
                    parent.layer.close(index);
                //parent.searchRecord();
            }
        },
        complete: function(XMLHttpRequest, textStatus) {},
        beforeSend: function(XMLHttpRequest) {
            //遮罩
            layer.load();
        }
    });
}

//返回
function btn_opt_fh(url) {
    window.location = url;
}


//*************************************CtrlString************************************
//改变第三控件的值
function CtrlChange(str, event) {

    //判断是否需要改变其他控件值
    if (str == "")
        return;
    //值对
    var keyParam;
    var keyValue;
    var typeList = str.split("||");
    for (var i = 0; i < typeList.length; i++) {
        keyParam = StrSplit(typeList[i], "--");
        if (keyParam.key == "ctrlChange") {
            //获取值
            keyValue = keyParam.value;
            //判断是否包含创建隐藏字段Div内容
            if (keyValue.indexOf("hiddendiv-") != -1) {
                //分析每一项
                var items = keyValue.split("|");
                for (var iItem = 0; iItem < items.length; iItem++) {
                    keyParam = StrSplit(items[iItem], "-");
                    if (keyParam.key == "hiddendiv") {
                        var hiddenDivList = keyParam.value.split(",");
                        for (var iHiddenDivList = 0; iHiddenDivList < hiddenDivList.length; iHiddenDivList++) {
                            //判断隐藏字段是否存在,不存在则创建
                            if ($("#" + PointRep(hiddenDivList[iHiddenDivList])).length == 0) {
                                var fieldDivStr = "<input type='hidden' id='" + hiddenDivList[iHiddenDivList] + "' name='" + hiddenDivList[iHiddenDivList] + "' value=''/>";
                                $("#incFieldDiv").append(fieldDivStr);
                            }
                        }
                        break;
                    }
                }
            }
            CtrlChangeContent(keyValue, event);
        }
    }
}

//改变每一组控件
function CtrlChangeContent(str, event) {

    var ary = str.split('|');
    var tmp, targetctrl,
        val = $(event.target).val();

    for (var i = 0, len = ary.length; i < len; i++) {
        tmp = ary[i].split('-');
        if (tmp[0] == 'wherectrl') { //当前控件的值
            tmp[1] = val;
            ary[i] = tmp.join('-');
        } else if (tmp[0] == 'targetctrl') { //目标控件
            targetctrl = tmp[1];
        }
    }


    ajaxTpl('/DataInput/CtrlStringData', {
        ctrlString: ary.join('|')
    }, function(data) {
        if (data.success) {

            //判断控件类型,根据类型是初始化控件模型还是设置控件值
            // var kjlx = GetCtrlType(targetctrl);
            var tmp = $(event.target).parents('tr');

            if (tmp.length) { //在3级表格中
                setChangeData(tmp.find('[name="' + targetctrl + '"]'), data.data.rows);
                return;
            }

            tmp = $(event.target).parents('.u-sed-info');
            if (tmp.length) { //在2级从表中

                setChangeData(tmp.find('[name="' + targetctrl + '"]'), data.data.rows);
                return;

            }
            tmp = $(event.target).parents('.m-info');
            if (tmp.length) { //在主表中
                setChangeData(tmp.find('[name="' + targetctrl + '"]'), data.data.rows);
                return;

            }


            // targetctrl
            // if (kjlx == "SELECT") {
            //     SetCtrlMode(targetctrl, data.data.rows);
            // } else if (kjlx == "BOOTSTRAPSELECT") {
            //     SetCtrlMode(targetctrl, data.data.rows);
            // } else if (kjlx == "BOOTSTRAPMULSELECT") {
            //     SetCtrlMode(targetctrl, data.data.rows);
            // } else if (kjlx == "CHECKBOX") {
            //     SetCtrlMode(targetctrl, data.data.rows);
            // } else {
            //     SetCtrlValue(targetctrl, data.data.rows);
            // }


            //触发改变事件
            InitCtrlEvent(targetctrl);

        } else {
            alert(data.msg);
        }
    }, 'sync');
}

function setChangeData(jq, ary) {

    // if (kjlx == "SELECT") {
    //     SetCtrlMode(targetctrl, data.data.rows);
    // } else if (kjlx == "BOOTSTRAPSELECT") {
    //     SetCtrlMode(targetctrl, data.data.rows);
    // } else if (kjlx == "BOOTSTRAPMULSELECT") {
    //     SetCtrlMode(targetctrl, data.data.rows);
    // } else if (kjlx == "CHECKBOX") {
    //     SetCtrlMode(targetctrl, data.data.rows);
    // } else {
    //     SetCtrlValue(targetctrl, data.data.rows);
    // }
    var str = '';
    var kjlx = jq.attr('kjlx') ;
    if (kjlx == 'SELECTS') {
        for (var i = 0, len = ary.length; i < len; i++) {
            str += '<option value="' + ary[i].value + '">' + ary[i].content + '</option>';
        }
        jq.html(str);
    }else if(kjlx == 'COMBOBOX'){
         for (var i = 0, len = ary.length; i < len; i++) {
            str += '<li value="' + ary[i].value + '">' + ary[i].content + '</li>';
        }
        jq.parent().find('.box-ul').html(str);

    }


    //触发改变事件
    // InitCtrlEvent(jq.attr('name'));

}


//*************************************CtrlString************************************

//*************************************Helplink**************************************
//显示helplink
function ShowHelpLinkForm(helplink, event) {
    //判断按下F2时,触发
    if (!(event.keyCode == 113 || event.type == "click"))
        return;

    //iframe层
    layer.open({
        type: 2,
        title: '数据信息',
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: '/DataInput/Helplink?HelpLink=' + helplink + '&Math.random()' //iframe的url
    });
}

//*************************************Helplink**************************************

//*************************************DetailHelplink********************************
/* List界面弹出HelpLink对话框 */
function ShowListHelpLink(dataGrid, editors, editIndex, event, helplink, prestr) {
    if (event.keyCode != 113)
        return;
    //分析helplink
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

    //分析值
    //把当前行赋值给从表公共定义编辑行
    editGrid = dataGrid;
    editGridEditingIndex = editIndex;
    var editors = editGrid.datagrid('getEditors', editGridEditingIndex);

    //传输helpStr
    var helpStr = "";
    helpStr = helpStr + "helplink--";
    //控件项列表
    var items = helplinkValue.split("|");
    for (var i = 0; i < items.length; i++) {
        keyParam = StrSplit(items[i], "-");
        //目标控件
        if (keyParam.key == "wherectrl") {
            helpStr = helpStr + "wherectrl-";
            var ctrlList = keyParam.value.split(",");
            for (var iCtrlList = 0; iCtrlList < ctrlList.length; iCtrlList++) {
                helpStr = helpStr + GetListControlValue(editGrid, editGridEditingIndex, ctrlList[iCtrlList]) + ",";
            }
            //去掉最后一个,
            if (helpStr.length > 0 && helpStr.charAt(helpStr.length - 1) == ',')
                helpStr = helpStr.substring(0, helpStr.length - 1)
            helpStr = helpStr + "|";
        } else {
            helpStr = helpStr + items[i] + "|";
        }
    }

    //去掉最后一个字符|
    if (helpStr.length > 0 && helpStr.charAt(helpStr.length - 1) == '|')
        helpStr = helpStr.substring(0, helpStr.length - 1);



    //iframe层
    layer.open({
        type: 2,
        title: '数据信息',
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: '/DataInput/Helplink?Type=list&PreStr=' + prestr + '&Index=' + editIndex + '&HelpLink=' + helpStr + '&Math.random()' //iframe的url
    });
}



//**********************************下拉多选单选过滤***********************************
function BootstrapMulSelectInit(fieldname) {
    $('#' + PointRep(fieldname)).multiselect({
        nonSelectedText: "请选择",
        checkAllText: "全选",
        uncheckAllText: '全不选',
        buttonWidth: "210px",
        includeSelectAllOption: true,
        enableFiltering: true
    });
}

function BootstrapSingleSelectInit(fieldname) {
    $('#' + PointRep(fieldname)).multiselect({
        nonSelectedText: "请选择",
        checkAllText: "全选",
        uncheckAllText: '全不选',
        buttonWidth: "210px",
        includeSelectAllOption: true,
        enableFiltering: true
    });
}
//**********************************下拉多选单选过滤***********************************

//**********************************回车换行转换***************************************
//回车转br
function SwitchNRToBr(str) {
    return str.replace(/\r\n/g, "<br>").replace(/\n/g, "<br>");
}
//*************************************************************************************