function view() {
    try {
        var selecteds = pubselects();
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        var selected = selecteds[0];
        var url = "/jc/viewht?recid=" + selected.Recid;
        parent.layer.open({
            type: 2,
            title: '',
            shadeClose: true,
            shade: 0.5,
            area: ['900px', '95%'],
            content: url,
            end: function () {
            }
        });
    } catch (err) {
        alert(err);
    }
}
function viewDjb() {
    try {
        var selecteds = pubselects();
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        var selected = selecteds[0];
        var url = "/jc/viewjcdjb?id=" + selected.Recid + "&file=" + encodeURIComponent(selected.JCDJBMB);
        parent.layer.open({
            type: 2,
            title: '',
            shadeClose: true,
            shade: 0.5,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    } catch (err) {
        alert(err);
    }
}

function add() {
    try {
        var url = "/workflow/startwork?processid=9&DlgId=1&_=" + Math.random();
        parent.layer.open({
            type: 2,
            title: '',
            shadeClose: true,
            shade: 0.5,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (err) {
        alert(err);
    }
}
function addSub() {
    var selecteds = pubselects();
    if (selecteds.length == 0) {
        return;
    }
    var selected = selecteds[0];
    if (selected.HTLX != "主合同") {
        alert("不是主合同，不能录入分包合同");
        return;
    }
    $.ajax({
        type: "POST",
        url: "/jc/getjchtinfo?recid=" + selected.Recid,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.code != "0") {
                alert(data.msg);
            } else {
                var htxx = data.result[0];
                var bcode = new Base64();
                var extrainfo1 = bcode.encode("i_m_ht|" + bcode.encode("recid=" + selected.Recid + ""));
                var url = "/workflow/startwork?processid=10&DlgId=1&extrainfo=" + encodeURIComponent(extrainfo1) + "&_=" + Math.random();
                parent.layer.open({
                    type: 2,
                    title: '',
                    shadeClose: true,
                    shade: 0.5,
                    area: ['95%', '95%'],
                    content: url,
                    end: function () {
                        searchRecord();
                    }
                });
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
function edit() {
    try {

    } catch (err) {
        alert(err);
    }
}
function del() {
    try {
        var selecteds = pubselects();
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        var selected = selecteds[0];

        var bcode = new Base64();
        var extrainfo1 = bcode.encode("i_m_ht|" + bcode.encode("recid=" + selected.Recid + ""));
        var url = "/workflow/startwork?processid=11&DlgId=1&extrainfo=" + encodeURIComponent(extrainfo1) + "&ParentSerial=" + encodeURIComponent(selected.SerialNo) + "&_=" + Math.random();
        parent.layer.open({
            type: 2,
            title: '',
            shadeClose: true,
            shade: 0.5,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    } catch (err) {
        alert(err);
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