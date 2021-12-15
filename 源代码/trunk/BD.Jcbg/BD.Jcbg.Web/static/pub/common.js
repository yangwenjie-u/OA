var layerNum = 0;
var layerLoad;

function ajaxTpl(url, params, handle) {
    layerNum++;
    if (typeof layer == "object" && typeof layer.load == "function") {
        layerLoad = layer.load({
            time:10000
        });
    }
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        success: function (data) {
            if (typeof handle == 'function') {
                handle(data);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {},
        complete: function (XMLHttpRequest, textStatus) {
            layerNum--;
            if (layerNum == 0 && typeof layer == "object" && typeof layer.close == "function") {
                layer.close(layerLoad);
            }
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