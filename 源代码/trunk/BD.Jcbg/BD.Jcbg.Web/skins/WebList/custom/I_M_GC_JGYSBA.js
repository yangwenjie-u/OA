function add() {
    try {
		//var selected = pubselect();
        //if (selected == undefined)
        //    return;
		//var gcmc=selected.GCMC;
		
        var tabledesc = "竣工验收备案";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_YSBA"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| "); // 按钮
        var para = "I_M_GC_YSBA,YSBALX,JGYS";
        var fieldparam = encodeURIComponent(para)
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&fieldparam=" + fieldparam+
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: true,
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
function edit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var tabledesc = "修改";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_YSBA"); 			// 表名
        var tablerecid = encodeURIComponent("recid"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh="+jydbh+
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: tabledesc,
            shadeClose: true,
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

function apply() {
    try {
        var tabledesc = "工程";                // 表格描述
        var selected = pubselect();// dataGrid.jqxGrid('getrowdata', rowindex);
        if (selected == undefined)
            return;
        if (selected.SY_ZT == "通过") {
            alert("该工程已经通过");
            return;
        }
        if (!confirm("确定要提交吗？")) {
            return;
        }
        var gcmc = selected.GCMC;
        $.ajax({
            type: "POST",
            url: "/qy/submitysba?recid=" + encodeURIComponent(selected.RECID)+"&ysbalx="+encodeURIComponent("JGYS"),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("提交成功！");         
                    $.ajax({
                       type: "POST",
                       url: "/jdbg/SetInfoMail?mailcon=" + encodeURIComponent(gcmc + "进行工程竣工验收备案") + "&maintitle=" + encodeURIComponent("竣工验收备案"),
                       dataType: "json",
                       async: false,
                       success: function (data) {
                           // if (data.code != 0)
                           //alert("发送邮件失败，详细信息：" + data.msg);
                           // else {
                           // alert("发送邮件成功");
                           // }
                       },
                       complete: function (XMLHttpRequest, textStatus) {
						      searchRecord();
                       },
                       beforeSend: function (XMLHttpRequest) {
                       }
                    });
                } else {
                    if (data.msg == "")
                        data.msg = "提交失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}
function selectGc() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var table = "View_I_M_GC_YSBA";
        var recid = selected.RECID;
        var bcode = new Base64();
        var tablename = bcode.encode(table);

        var reportname = encodeURIComponent("工程验收工资支付情况备案v1");;

        var workurl = "/ReportPrint/Index?type=word&opentype=pdf&table=" + tablename + "&where=recid%3d%27" + recid + "%27&filename=" + reportname;       

        var title = selected.GCMC;
        gotoStarkWork(workurl, title);


    } catch (e) {
        alert(e);
    }
}
function gotoStarkWork(workurl, title) {
    try {
        var rdm = Math.random();
        var url = workurl;

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: true,
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