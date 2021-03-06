// 数组去重复 
Array.prototype.unique = function () {
    var newArr = [];
    for (var i = 0; i < this.length; i++) {
        if (newArr.indexOf(this[i]) == -1) {
            newArr.push(this[i]);
        }
    }
    return newArr;
}


// 发起撤销整改单处罚流程
function cancelCF() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var zgdbh = selected.ZGDBH;
    var qymc = selected.QYMC;
    var cfworkserial = selected.WORKSERIAL;
    if (selected.CFZTMC == "未处罚") {
        alert("未处罚的整改单无需撤销！");
        return;
    }
    if (zgdbh != "") {
        $.ajax({
            type: "POST",
            url: "/dwgxwz/CheckCXZGDCFJBR",
            data: { zgdbh: zgdbh },
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    var rdm = Math.random();
                    var bcode = new Base64();
                    var extrainfo1 = bcode.encode("view_jdbg_jdjl|" + bcode.encode("lx='zgd' and extrainfo4='" + zgdbh + "'") + "||view_jdbg_zgdcfjl|" + bcode.encode("workserial='" + cfworkserial + "' and zgdbh='" + zgdbh + "' and qymc='" + qymc + "'"));
                    var url = "/workflow/startwork?processid=41" +
                        // 加密的条件信息（表单中用到）
                        "&extrainfo=" + encodeURIComponent(extrainfo1) +
                        //// 流程中显示的主体信息
                        //"&extrainfo2=" + encodeURIComponent(extrainfo2) +
                        //// 流程中用到的跟工程关联的主键
                        //"&extrainfo3=" + encodeURIComponent(extrainfo3) +
                        //// 流程中用到的分工程主键
                        //"&extrainfo4=" + encodeURIComponent(extrainfo4) +
                        "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
                        "&_=" + rdm;

                    parent.layer.open({
                        type: 2,
                        title: '撤销整改单处罚',
                        shadeClose: false,
                        shade: 0.8,
                        area: ['95%', '95%'],
                        content: url,
                        end: function () {
                        }
                    });
                }
                else {
                    if (data.msg == "") {
                        data.msg = "操作失败！";
                    }
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });

    }

}

// 设置整改单处罚撤销结果
function setCX(success) {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var zgdbh = selected.ZGDBH;
    var cfworkserial = selected.WORKSERIAL;
    if (selected.CFZTMC != "处罚成功") {
        alert("未成功处罚的整改单无需撤销！");
        return;
    }
    if (zgdbh != "") {
        $.ajax({
            type: "POST",
            url: "/jdbg/docxzgdcf?success=" + encodeURIComponent(success) + '&zgdbhlist=' + encodeURIComponent(zgdbh) + "&cfworkserial=" + cfworkserial,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("撤销操作成功！");
                    searchRecord();
                }
                else {
                    if (data.msg == "") {
                        data.msg = "撤销操作失败！";
                    }
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
}

function sendmail() {
    try {
        var kjlx = JudgeKJ();
        var records = null;
        var workserial = '';
        var reportfile = '整改单逾期处罚结果';
        var ykfrq = '';
        if (kjlx == 'easyui') {
            records = dataGrid.datagrid('getData');
            if (records.total == 0) {
                $.messager.alert('提示', '当前不存在处罚记录，无法发送邮件！', 'info');
                return;
            }
            else {
                workserial = records.rows[0]['WORKSERIAL'];
                ykfrq = records.rows[0]['JZRQ'];

            }
        }
        else if (kjlx = 'jqx') {

        }
        if (workserial != "") {
            $.ajax({
                type: "POST",
                url: "/jdbg/zgdcfsendmail?serial=" + encodeURIComponent(workserial) + '&reportfile=' + encodeURIComponent(reportfile) + '&ykfrq=' + encodeURIComponent(ykfrq),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.code == "0") {
                        alert("发送成功！");
                        searchRecord();
                    }
                    else {
                        if (data.msg == "") {
                            data.msg = "发送失败！";
                            alert(data.msg);
                        }
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }

    } catch (e) {
        alert(ex);
    }

}


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

