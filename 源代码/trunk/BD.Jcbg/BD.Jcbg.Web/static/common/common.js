function ajaxTpl(url, params, handle, complete) {
    //var index = layer.load(2, { time: 10000 });
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        success: function (data) {
            //layer.close(index);
            handle(data);
        },
        fail: function (err) {
            //layer.close(index);
            //console.log(err);
        },
        complete: function () {
            if (complete) {
                complete();
            }
        }
    });
}

function ajaxGet(url, params, handle) {
    //var index = layer.load(2, { time: 10000 });
    $.ajax({
        url: url,
        type: 'get',
        data: params,
        dataType: 'json',
        success: function (data) {
            // layer.close(index);
            handle(data);
        },
        fail: function (err) {
            // layer.close(index);
            //console.log(err);
        }
    });
}

function getParams() {
    var url = location.search; //获取url中"?"符后的字串
    var obj = {};
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        var strS = str.split("&");
        for (var i = 0; i < strS.length; i++) {
            obj[strS[i].split("=")[0]] = (strS[i].split("=")[1]);
        }
    }
    return obj;
}


/*

function getTable(url, params, handle, doc) {
    
    //layui 分页
    var page = function (data) {
        layui.use('laypage', function () {
            layui.laypage.render({
                elem: data.elem,//id 节点
                count: data.count,//数据总数，从服务端得到
                limit: data.pageSize || 10,//数据条数
                jump: function (obj, first) {
                    //首次不执行
                    if (!first) {
                        params.page = obj.curr;//当前页
                        getTable(url, params, handle);
                    }
                }
            });
        });
    }

    if (doc && doc.length) {//事件只执行一次
        doc.one("click", ".sure", function () {
            params.page = 1;
            params.limit = doc.find(".page-size").val();
            getTable(url, params, handle, doc);
        });
    }

    //合并对象 默认页面条数和当前页
    params = Object.assign({
        page: 1,
        limit: 10
    }, params);
    
    ajaxTpl(url, params, function (data) {
        if (doc && doc.length) {
            page({
                page: 1,
                pageSize: doc.find(".page-size").val(),
                elem: doc.find(".layer-page").attr("id"),
                count: data.count || 100 //页面总条数
            });
            doc.find(".count").html("共 "+ data.count +" 条数据");
        }
        handle(data);
    })
}
*/
function formatYesNo(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是" || value == "1")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}


function getDateString(value, row, index) {
    var ret = "";
    try {
        var ttDate = value.match(/\d{4}.\d{1,2}.\d{1,2}/mg).toString();
        ttDate = ttDate.replace(/[^0-9]/mg, '-');
        if (ttDate.indexOf("1900") == -1)
            ret = ttDate;
    } catch (e) {
        ret = value;
    }
    return ret;
}


function DateFormat(value, row, index) { //author: meizz

    var ret = new Date();
    try {
        if (value.length == 0) {
            ret = new Date("1900-01-01 00:00:00");
        }
        else {
            var ttDate = new Date(value.replace(/-/g, "/"));
            ret = ttDate;
        }

    } catch (e) {
        ret = new Date("1900-01-01 00:00:00");

    }

    var o = {
        "M+": ret.getMonth() + 1,                 //月份
        "d+": ret.getDate(),                    //日
        "h+": ret.getHours(),                   //小时
        "m+": ret.getMinutes(),                 //分
        "s+": ret.getSeconds(),                 //秒
        "q+": Math.floor((ret.getMonth() + 3) / 3), //季度
        "S": ret.getMilliseconds()             //毫秒
    };
    var fmt = "yyyy-MM-dd hh:mm:ss";

    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (ret.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
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
                searchRecord();
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
