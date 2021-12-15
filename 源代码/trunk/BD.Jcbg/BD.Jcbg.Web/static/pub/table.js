/*
option = {
    doc : $('#doc'),
    page: 'page',
    pageSize:'rows',
    //data:'rows',
    count:'count',
    jump:jump
}
*/

function getTable(url, params, handle, option) {
    var doc, pageindex, pagesize, count;
    if (option) {
        doc = option.doc ? option.doc : '';
        pageindex = option.pageindex ? option.pageindex : 'page';
        pagesize = option.pageSize ? option.pageSize : 'pagesize';
        count = option.count ? option.count : 'count';
    }
    //layui 分页
    var page = function(data) {
        layui.use('laypage', function() {
            layui.laypage.render({
                elem: data.elem, //id 节点
                count: data.count, //数据总数，从服务端得到
                limit: data.pageSize || 10, //数据条数
                curr: data.page,
                jump: function(obj, first) {
                    //首次不执行
                    if (option.jump) {
                        option.jump(obj.curr);
                    }
                    if (!first) {
                        params[pageindex] = obj.curr; //当前页
                        option.doc = "";
                        getTable(url, params, handle, option);
                    }
                }
            });
        });
    }


    if (doc && doc.length && option.sure != 0) { //事件只执行一次
        doc.one("click", ".sure", function() {
            params[pageindex] = doc.find(".page-val").val();
            params[pagesize] = doc.find(".page-size").val();
            option.sure = 1;
            getTable(url, params, handle, option);
        });
    }

    //合并对象 默认页面条数和当前页
    var tmpObj = {};
    tmpObj[pageindex] = 1;
    tmpObj[pagesize] = 10;
    params = $.extend(tmpObj, params);
    ajaxTpl(url, params, function(data) {
        try{
             data.getParams = params;
        }catch(e){}

        handle(data);
        if ((doc && doc.length) || option.sure) {
            option.sure = 0;
            page({
                page: params[pageindex],
                pageSize: params[pagesize] || doc.find(".page-size").val(),
                elem: doc.find(".layer-page").attr("id"),
                count: data[count] || 10 //页面总条数
            });
            doc.find(".count").html("共 " + data[count] + " 条数据");
        }

    })
}