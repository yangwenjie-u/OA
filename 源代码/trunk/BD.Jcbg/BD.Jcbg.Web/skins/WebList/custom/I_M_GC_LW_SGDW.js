function add()
{
	try{
		var selected = pubselect();
		if (selected == undefined)
			return;
		var yhzh=selected.YHZH;
		var qymc=selected.QYMC;
		
		var lwsgdwzh="#"+$('#lwsgdwzh').val();
		var lwsgdw="#"+$('#lwsgdw').val();
		
		var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
		parent.$(lwsgdwzh).val("");
		parent.$(lwsgdw).val("");
		parent.$(lwsgdwzh).val(parent.$(lwsgdwzh).val()+yhzh);
		parent.$(lwsgdw).val(parent.$(lwsgdw).val()+qymc);
		parent.layer.close(index);
	}
	catch(e)
	{alert(e);}
	
}

function selectGc() {
    try {
        
        var tabledesc = "工程";
        var selected = pubselect();
        if (selected == undefined)
            return;
        		
        var workurl="/workflow/startwork?processid=17";
        var gcbh = selected.GCBH;
        var bcode = new Base64();
        var title = selected.GCMC;
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='" + gcbh + "'"));
        var extrainfo2 = bcode.encode('[' + selected.GCMC + ']');
        var extrainfo3 = bcode.encode(gcbh);
       
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
            "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();")+
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




var g_stations = [];
function getFromPlatform() {
    $.ajax({
        type: "POST",
        url: "/ycba/GetStations",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length == 0) {
                alert("请配置正确的调用服务器");
            } else {
                g_stations = data;
                if (data.length > 0) {
                    var contents = "<center>";
                    $.each(data, function (i, item) {
                        contents += "<button style='width:380px;color: #fff;background-color: #f0ad4e;border-color: #eea236;padding:6px 12px;display:inline-block;font-size:14px;line-height:20px;margin-top:10px;border: 1px solid transparent; border-radius: 4px;' onclick=\"showPlatformDiv(" + i + ")\">" + item.StationName + "</button><br>";
                    });
                    contents += "<center>";
                    layer.open({
                        type: 1,
                        title: "请选择平台",
                        area: ['420px', '300px'], //宽高
                        content: contents
                    });
                }
                if (data.length == 1) {
                    showPlatformDiv(0);
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });

}

function showPlatformDiv(index) {
    layer.closeAll();
    var station = g_stations[index];
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/ycba/apipagelist?callid=ApiPageProjectList&version=" + encodeURIComponent(station.VersionNo) + "&url=" + encodeURIComponent(station.RootUrl),
        end: function () {
            searchRecord();
        }
    });
}



var g_stations_qy = [];
function getFromPlatformQY() {
    $.ajax({
        type: "POST",
        url: "/ycba/GetStations",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length == 0) {
                alert("请配置正确的调用服务器");
            } else {
                g_stations_qy = data;
                if (data.length > 0) {
                    var contents = "<center>";
                    $.each(data, function (i, item) {
                        contents += "<button style='width:380px;color: #fff;background-color: #f0ad4e;border-color: #eea236;padding:6px 12px;display:inline-block;font-size:14px;line-height:20px;margin-top:10px;border: 1px solid transparent; border-radius: 4px;' onclick=\"showPlatformDiv(" + i + ")\">" + item.StationName + "</button><br>";
                    });
                    contents += "<center>";
                    layer.open({
                        type: 1,
                        title: "请选择平台",
                        area: ['420px', '300px'], //宽高
                        content: contents
                    });
                }
                if (data.length == 1) {
                    showPlatformDivQY(0);
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });

}

function showPlatformDivQY(index) {
    layer.closeAll();
    var station = g_stations_qy[index];
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/ycba/apipagelist?callid=ApiPageQyList&version=" + encodeURIComponent(station.VersionNo) + "&url=" + encodeURIComponent(station.RootUrl),
        end: function () {
            searchRecord();
        }
    });
}