function ajaxTpl(url, params, handle) {
    //var index = layer.load(2, { time: 10000 });
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        success: function(data) {
            //layer.close(index);
            handle(data);
        },
        fail: function(err) {
            //layer.close(index);
            console.log(err);
        }
    });
}

function getTable(url, params, handle, doc) {

    //layui 分页
    var page = function(data) {
        layui.use('laypage', function() {
            layui.laypage.render({
                elem: data.elem, //id 节点
                count: data.count, //数据总数，从服务端得到
                limit: data.pageSize || 10, //数据条数
                jump: function(obj, first) {
                    //首次不执行
                    if (!first) {
                        params.page = obj.curr; //当前页
                        getTable(url, params, handle);
                    }
                }
            });
        });
    }

    if (doc && doc.length) { 
        //选择分页条数//事件只执行一次
        doc.one("click", ".sure", function() {
            params.page = 1;
            params.rows = doc.find(".page-size").val();
            getTable(url, params, handle, doc);
        });
    }

    //合并对象 默认页面条数和当前页
    params = $.extend({
        page: 1,
        rows: 10
    }, params);

    ajaxTpl(url, params, function(data) {
        if (doc && doc.length) {
            page({
                page: 1,
                pageSize: doc.find(".page-size").val(),
                elem: doc.find(".layer-page").attr("id"),
                count: data.total || 10 //页面总条数
            });
            doc.find(".count").html("共 " + data.total + " 条数据");
        }
        handle(data);
    })
}