define(function (require, exports, module) {

    //加载列表
    exports.LoadDataTables = function (id, url, columns, isSingle) {
        var table = $("#" + id).DataTable({
            searching: false
            , lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]]
            , processing: true//显示进度条
            , autoWidth: false//自适应宽度
            , serverSide: true//服务器端分页
            , sort: false
            , ajax: {
                url: url
                , async: false
            }
            , pagingType: "full_numbers"//分页类型
            , columns: columns
            , language: {
                processing: "正在加载中......",
                lengthMenu: "每页显示 _MENU_ 条记录",
                zeroRecords: "表中无数据存在！",
                info: "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
                paginate: {
                    sFirst: "首页",
                    sPrevious: "上一页",
                    sNext: "下一页",
                    sLast: "末页"
                }
            }
        });
        //单选和多选
        $("#" + id + " tbody").on("click", "tr", function () {
            if (isSingle || isSingle === undefined || isSingle === null) {
                if ($(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                }
                else {
                    table.$("tr.selected").removeClass("selected");
                    $(this).addClass("selected");
                }
            } else {
                $(this).toggleClass("selected");
            }
        });
    }

    //加载权限按钮
    exports.LoadButton = function (id, url) {
        $.ajax({
            url: url
            , type: "get"
            , dataType: "json"
            , async: false
            , success: function (json) {
                var html = "";
                for (var i = 0; i < json.length; i++) {
                    html += "<a id='" + json[i].MENUCODE + "' class='btn btn-primary' style='margin-left: 10px; float: left;'>" + json[i].MENUNAME + "</a>";
                }
                $("#" + id).html(html);
            }
        });
    }

})