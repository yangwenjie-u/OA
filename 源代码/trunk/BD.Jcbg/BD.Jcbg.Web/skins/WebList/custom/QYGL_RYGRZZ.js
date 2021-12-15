function add() {
    try {
        $.ajax({
            type: "POST",
            url: "/ry/getrybh",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    var rybh = data.msg;
                    var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
                    var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
                    var tablerecid = encodeURIComponent("RECID"); 	// 表主键 
                    var title = encodeURIComponent("人员证书"); 	// 标题
                    var buttons = encodeURIComponent("保存|TJ| "); // 按钮
                    var fieldparam = encodeURIComponent("I_S_RY_RYZZ,RYBH," + rybh);
                    var rdm = Math.random();

                    var js = encodeURIComponent("userService.js");
                    var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");

                    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                        "&t1_tablename=" + tablename +
                        "&t1_pri=" + tablerecid +
                        "&t1_title=" + title +
                        "&button=" + buttons +
                        "&rownum=2" +
                        "&fieldparam=" + fieldparam +
                        "&js=" + js +
                        "&callback=" + callback +
                        "&_=" + rdm;
                    parent.layer.open({
                        type: 2,
                        title: '录入证书信息',
                        shadeClose: false,
                        shade: 0.8,
                        area: ['95%', '95%'],
                        content: url,
                        end: function () {
                            searchRecord();
                        }
                    });
                } else {                    
                    alert("获取当前账号对应的人员信息失败，请联系管理员");
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
function copy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("资质证书"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_RY_RYZZ,RYBH," + selected.RYBH);
        var rdm = Math.random();
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");


        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&js=" + js +
            "&callback=" + callback +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '复制证书信息',
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
function edit() {
    try {
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;
        /*
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");


        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&js=" + js +
            "&callback=" + callback +
            "&_=" + rdm;
            */

        var rdm = Math.random();
        var bcode = new Base64();
        var extrainfo1 = bcode.encode("I_S_RY_RYZZ|" + bcode.encode("RECID='" + selected.RECID + "'"));
        var extrainfo2 = bcode.encode('[' + selected.ZZZSBH+']证书修改');
        var extrainfo3 = bcode.encode(selected.RECID);
        var url = "/workflow/startwork?processid=47" +
        // 加密的条件信息（表单中用到）
       "&extrainfo=" + encodeURIComponent(extrainfo1) +
        // 流程中显示的主体信息
        "&extrainfo2=" + encodeURIComponent(extrainfo2) +
         // 流程中用到的跟工程关联的主键
         "&extrainfo3=" + encodeURIComponent(extrainfo3) +
         // 流程中用到的分工程主键
         "&extrainfo4=" +
         "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
         "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
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
function del() {
    try {
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteisryryzs?recid=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    $.ajax({
                        type: "POST",
                        url: "/ry/updateuserrole?rybh=" + encodeURIComponent(selected.RYBH),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.msg != "") {
                                alert(data.msg);
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
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

function view() {
    try {
        var tabledesc = "证书信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&view=true"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看' + tabledesc,
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