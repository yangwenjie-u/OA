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