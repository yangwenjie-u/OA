function add() {
    try {
        var tabledesc = "人员信息";
        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的工程信息");
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();
        if (selected == undefined)
            return;
        var tablename = getTableName(selected.QYLXBH);
        if (tablename == "") {
            alert(selected.QYLXMC + "不需要录入人员");
            return;
        }
        var gcbh = selected.GCBH;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var buttons = encodeURIComponent("保存|TJ| | ");
        var rdm = Math.random();
        var fieldparam = tablename + ",GCBH," + selected.GCBH + "|" + tablename + ",QYBH," + selected.GCQYID; //工程区域
        fieldparam = encodeURIComponent(fieldparam);

        var js = "";//encodeURIComponent("jdbgService.js");
        var js = encodeURIComponent("userService.js");
        // var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");
        var callback = encodeURIComponent("userService.updateUserRole2('$$RYBH$$','" + gcbh + "')");
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&js=" + js +
            "&callback=" + callback +
            "&button=" + buttons +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function viewGC() {
    try {
        var tabledesc = "工程信息";

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的工程信息");
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();
        if (selected == undefined)
            return;
        var jydbh = encodeURIComponent(selected.GCBH);
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = ""; // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY||I_S_GC_TSDW|I_S_GC_TSRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员|图审人员");

        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
            "&view=true" +
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function view() {
    try {
        var tabledesc = "人员信息";
        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的工程信息");
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/WebList/EasyUiIndex?FormDm=QYGL_GCRYCK&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(selected.GCQYID)

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function getTableName(qylx) {
    var tablename = "";
    if (qylx == "13")
        tablename = "I_S_GC_JSRY";
    else if (qylx == "12")
        tablename = "I_S_GC_JLRY";
    else if (qylx == "11")
        tablename = "I_S_GC_SGRY";
    else if (qylx == "14")
        tablename = "I_S_GC_SJRY";
    else if (qylx == "15")
        tablename = "I_S_GC_KCRY";
    else if (qylx == "21")
        tablename = "I_S_GC_TSRY";
    return tablename;
}

function changeRY() {
    try {

        var tabledesc = "工程";
        var selected = pubselect();
        if (selected == undefined)
            return;
        var workurl = getQueryString("workurl");
        var gcbh = selected.GCBH;
        var bcode = new Base64();
        var title = selected.GCMC;
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='" + gcbh + "'"));
        var extrainfo2 = bcode.encode('[' + selected.ZJDJH + ']' + selected.GCMC);
        var extrainfo3 = bcode.encode(gcbh);
        
        //var fgc = selected.FGC;
        //var multiFgc = fgc.indexOf("||") > -1;
        //if (multiFgc) {
        //    var layerObj = undefined;
        //    parent.layer.open({
        //        type: 2,
        //        title: '分工程选择-' + title,
        //        shadeClose: false,
        //        shade: 0.5,
        //        area: ['600px', '400px'],
        //        content: "/jdbg/fgcxz?fgc=" + encodeURIComponent(fgc),
        //        btn: ["保存", "关闭"],
        //        yes: function (index) {
        //            var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
        //            var values = obj.fgcid;

        //            parent.layer.closeAll();
        //            var extrainfo4 = "";
        //            if (values.length > 0)
        //                extrainfo4 = bcode.encode(values);

        //            gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, obj.fgcmc);
        //        },
        //        success: function (layero, index) {
        //            layerObj = layero;
        //        },
        //        btn2: function (index) {
        //            parent.layer.closeAll();
        //        },
        //        end: function () {
        //        }
        //    });

        //}
        //else
        //    gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");

        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");


    } catch (e) {
        alert(e);
    }
}



function gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc) {
    try {
        var rdm = Math.random();
        var url = workurl +
            // 加密的条件信息（表单中用到）
            "&extrainfo=" + encodeURIComponent(extrainfo1) +
            // 流程中显示的主体信息
            "&extrainfo2=" + encodeURIComponent(extrainfo2) +
            // 流程中用到的跟工程关联的主键
            "&extrainfo3=" + encodeURIComponent(extrainfo3) +
            // 流程中用到的分工程主键
            "&extrainfo4=" + encodeURIComponent(extrainfo4) +
            "&fgcmc=" + encodeURIComponent(fgcmc) +
            "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });

    }
    catch (ex) {
        alert(ex);
    }
}


function getQueryString(paramName) {

    var reg = new RegExp("(^|&)" + paramName + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}

/**
*
*  Base64 encode / decode
*
*  @author haitao.tu
*  @date   2010-04-26
*  @email  tuhaitao@foxmail.com
*
*/

function Base64() {

    // private property
    _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    // public method for encoding
    this.encode = function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;
        input = _utf8_encode(input);
        while (i < input.length) {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output +
            _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
            _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
        }
        return output;
    }

    // public method for decoding
    this.decode = function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;
        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
        while (i < input.length) {
            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));
            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;
            output = output + String.fromCharCode(chr1);
            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = _utf8_decode(output);
        return output;
    }

    // private method for UTF-8 encoding
    _utf8_encode = function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }
        return utftext;
    }

    // private method for UTF-8 decoding
    _utf8_decode = function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;
        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
}