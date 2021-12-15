function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial),
            end: function () {
                searchRecord();
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function showInfo() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var wclx = selected.WCLX;
    var content = "";
    content = "<table cellspacing=\"0\" border=\"1\" cellpadding=\"2\" style=\"border-collapse:collapse;width:90%; margin:20px;\" >" +
                    "<tbody>" +
                        "<tr>" +
                            "<td width=\"30%\">外出类型</td>" +
                            "<td>" + wclx + "</td>" +
                        "</tr>";
        
    if (wclx=='出差') {
        content += "<tr>" +
                        "<td>出差人员</td>" +
                        "<td>" + selected.WCRY + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>出差时间</td>" +
                        "<td>" + selected.WCSJMS + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>出差城市</td>" +
                        "<td>" + selected.CCCITY + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>出差事由</td>" +
                        "<td>" + selected.CCSY + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>预计差旅费</td>" +
                        "<td>" + selected.CCYJCLF + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>差旅费说明</td>" +
                        "<td>" + selected.CCCLFSM + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>记录添加时间</td>" +
                        "<td>" + selected.LRSJ + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>审批状态</td>" +
                        "<td>" + selected.SPZTMS + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>审批人</td>" +
                        "<td>" + selected.SPRXM + "</td>" +
                   "</tr>" +
                   "<tr>" +
                        "<td>审批时间</td>" +
                        "<td>" + selected.SPSJ + "</td>" +
                   "</tr>"
    }
    else {
        content += "<tr>" +
                        "<td>人员姓名</td>" +
                        "<td>" + selected.WCRY + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>外出时间</td>" +
                        "<td>" + selected.WCSJMS + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>备注说明</td>" +
                        "<td>" + selected.NR + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>记录添加时间</td>" +
                        "<td>" + selected.LRSJ + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>审批状态</td>" +
                        "<td>" + selected.SPZTMS + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>审批人</td>" +
                        "<td>" + selected.SPRXM + "</td>" +
                   "</tr>"+
                   "<tr>" +
                        "<td>审批时间</td>" +
                        "<td>" + selected.SPSJ + "</td>" +
                   "</tr>"
                   
    }
    
    content += "</tbody>";
    content += "</table>";

    parent.layer.open({
        type: 1,
        title: '因公外出查看',
        shadeClose: false,
        shade: 0.8,
        area: ['500px', '400px'],
        content: content,
        end: function () {
        }
    });
    
}

function startYGWC() {
    var processid = 35;
    var rdm = Math.random();
    var url = "";

    if (processid !=null) {
        url = "/workflow/startwork?processid=" + processid +
              "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
              "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '因公外出申请',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    }


}

function startYGWCXJ() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    if (selected.SPZT == '0') {
        alert('请假申请未获批准，不能销假！');
        return;
    }

    if (selected.XJSPZT != '') {
        alert('不能重复销假！');
        return;
    }



    var bcode = new Base64();
    var processid = 37;
    var rdm = Math.random();
    var url = "";
    var extrainfo1 = bcode.encode("view_qj_ygwcjl|" + bcode.encode("workserial='" + selected.WORKSERIAL + "'"));

    if (processid != null) {
        url = "/workflow/startwork?processid=" + processid +
              "&extrainfo=" + encodeURIComponent(extrainfo1) +
              "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
              "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '因公外出销假填写',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    }
}


function switchRecord(obj) {
    try {
        var strLocation = decodeURIComponent(window.location.href);
        // 所有
        if (obj.checked) {
            strLocation = strLocation.replace("NOT||CHECKBOX", "ALL||CHECKBOX").replace("所有|我的", "所有|所有");
        }
        else {
            strLocation = strLocation.replace("ALL||CHECKBOX", "NOT||CHECKBOX").replace("所有|所有", "所有|我的");
        }
        window.location = strLocation;
    } catch (ex) {
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