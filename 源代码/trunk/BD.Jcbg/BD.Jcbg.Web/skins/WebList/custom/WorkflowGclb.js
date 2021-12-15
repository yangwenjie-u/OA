function selectGcCheckKSCS() {
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
        
        var fgc = selected.FGC;
        var multiFgc = fgc.indexOf("||") > -1;
        if (multiFgc) {
            var layerObj = undefined;
            parent.layer.open({
                type: 2,
                title: '分工程选择-' + title,
                shadeClose: false,
                shade: 0.5,
                area: ['600px', '400px'],
                content: "/jdbg/fgcxz?fgc=" + encodeURIComponent(fgc),
                btn: ["保存", "关闭"],
                yes: function (index) {
                    var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                    var values = obj.fgcid;
                    
                    parent.layer.closeAll();
                    var extrainfo4 = "";
                    if (values.length > 0)
                        extrainfo4 = bcode.encode(values);
                    
                    showCssz(gcbh,workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, obj.fgcmc);
                },
                success: function (layero, index) {
                    layerObj = layero;
                },
                btn2: function (index) {
                    parent.layer.closeAll();
                },
                end: function () {
                }
            });

        }
        else
            showCssz(gcbh,workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");

        
    } catch (e) {
        alert(e);
    }
}

function showCssz(gcbh, workurl,title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc) {
    try{
        $.ajax({
            type: "POST",
            url: "/jdbg/getgccs",
            data:"gcbh="+gcbh+"&fgcbh=",
            dataType: "json",
            async: false,
            success: function (data) {
                var isBlank = false;
                $.each(data.records, function (i, item) {
                    if (item.kscs == "" || item.jscs == "") {
                        isBlank = true;
                        return false;
                    }
                });
                if (isBlank) {
                    parent.layer.open({
                        type: 2,
                        title: '工程开始结束层数设置（只能设置一次，并且不能修改）',
                        shadeClose: false,
                        shade: 0.5,
                        area: ['600px', '300px;'],
                        content: "/jdbg/gccssz?gcbh="+gcbh,
                        btn: ["确定", "关闭"],
                        yes: function (index) {
                            var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                            if (obj.msg != "") {
                                alert(obj.msg);
                            } else {
                                parent.layer.closeAll();
                                var poststr = "";
                                if (obj.items.length > 0){
                                    $.each(obj.items, function (i, item) {
                                        if (poststr != "")
                                            poststr += "|";
                                        poststr += item.gcbh + "," + item.zcb + "," + item.kscs + "," + item.jscs;
                                    });
                                    $.ajax({
                                        type: "POST",
                                        url: "/jdbg/setgccs?gccss="+encodeURIComponent(poststr),
                                        dataType: "json",
                                        async: false,
                                        success: function (data) {
                                            if (data.code == 0)
                                                gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc)
                                            else
                                                alert(data.msg);
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {
                                        },
                                        beforeSend: function (XMLHttpRequest) {
                                        }
                                    });
                                }
                                else{
                                    gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc);
                                }
                            }

                        },
                        success: function (layero, index) {
                            layerObj = layero;
                        },
                        btn2: function (index) {
                            parent.layer.closeAll();
                        },
                        end: function () {
                        }
                    });
                }
                else {
                    gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc)
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (err) {
        alert(err);
    }
}



function selectGc() {
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
        
        var fgc = selected.FGC;
        var multiFgc = fgc.indexOf("||") > -1;
        if (multiFgc) {
            var layerObj = undefined;
            parent.layer.open({
                type: 2,
                title: '分工程选择-' + title,
                shadeClose: false,
                shade: 0.5,
                area: ['600px', '400px'],
                content: "/jdbg/fgcxz?fgc=" + encodeURIComponent(fgc),
                btn: ["保存", "关闭"],
                yes: function (index) {
                    var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                    var values = obj.fgcid;
                    
                    parent.layer.closeAll();
                    var extrainfo4 = "";
                    if (values.length > 0)
                        extrainfo4 = bcode.encode(values);
                    
                    gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, obj.fgcmc);
                },
                success: function (layero, index) {
                    layerObj = layero;
                },
                btn2: function (index) {
                    parent.layer.closeAll();
                },
                end: function () {
                }
            });

        }
        else
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


function switchRecord(obj) {
    try {
        var strLocation = decodeURI(window.location.href);
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