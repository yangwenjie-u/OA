
//公出
function  GCAdd() {
    try {

        var workurl = "/workflow/startwork?processid=40";
        var gcbh = "222";
        var bcode = new Base64();
        var title = "检测派遣";
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='2323'"));
        var extrainfo2 = bcode.encode('[' + "42432" + ']');
        var extrainfo3 = bcode.encode(gcbh);
        console.log();
        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
    } catch (e) {
        alert(e);
    }

}

//审核
function GCReview() {
    try {

      
    } catch (e) {
        alert(e);
    }

}

//公出
function QJAdd() {
    try {

        var workurl = "/workflow/startwork?processid=39";
        var gcbh = "222";
        var bcode = new Base64();
        var title = "检测派遣";
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='2323'"));
        var extrainfo2 = bcode.encode('[' + "42432" + ']');
        var extrainfo3 = bcode.encode(gcbh);
        console.log();
        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
    } catch (e) {
        alert(e);
    }

}

//审核
function QJReview() {
    try {


    } catch (e) {
        alert(e);
    }

}


function JBAdd() {
    try {

        var workurl = "/workflow/startwork?processid=39";
        var gcbh = "222";
        var bcode = new Base64();
        var title = "检测派遣";
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='2323'"));
        var extrainfo2 = bcode.encode('[' + "42432" + ']');
        var extrainfo3 = bcode.encode(gcbh);
        console.log();
        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
    } catch (e) {
        alert(e);
    }

}

//审核
function JBReview() {
    try {


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




//查看使用记录
function UseRecord(id, code, name) {
    var url = "/WebList/ElementIndex?FormDm=YZGL_YZGL&FormStatus=1&FormParam=PARAM--" + encodeURIComponent(mid);
    parent.layer.open({
        type: 2,
        title: code + "-" + name + "用章记录",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    })
}

function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='Edit(" + value + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + "<a onclick='UseRecord((\"" + value + "\",\"" + row.SignatureCode + "\",\"" + row.SignatureName + "\")' style='cursor:pointer;color:#169BD5;'> 查看使用记录 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


