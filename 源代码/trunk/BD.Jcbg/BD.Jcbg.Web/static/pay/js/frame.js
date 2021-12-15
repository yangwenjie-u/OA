$(".tab-title").on("click", "li", function () {    
    var that = $(this)
    if (!that.hasClass('active')) {
        that.addClass('active').siblings().removeClass('active');
        var n = that.index();
        that.parent().next().children().eq(n).show().siblings().hide();
    }
});

$("#moreCon").hover(function () {
    $(this).find("ul").show();
}, function () {
    $(this).find("ul").hide();
});
$("#btnExit").on("click", function () {
    try {
        $.ajax({
            type: "POST",
            url: "/user/dologout",
            dataType: "json",
            success: function (data) {
                if (data.code == 0)
                    window.location = "/";
                else
                    layer.open({ title: '提示', content: data.msg, icon: 5 });
            },
            complete: function (XMLHttpRequest, textStatus) {
                //$("body").unmask();
                layer.closeAll('loading');
            },
            beforeSend: function (XMLHttpRequest) {
                //$("body").mask("正在退出系统...")
                layer.load();;
            }
        });
    } catch (e) {
        layer.open({ title: '错误', content: e, icon: 5 });
    }
});

$(function () {
    $(".NumDecText").keyup(function(){    
        $(this).val($(this).val().replace(/[^0-9.]/g,''));    
    }).bind("paste",function(){  //CTR+V事件处理    
        $(this).val($(this).val().replace(/[^0-9.]/g,''));     
    }).css("ime-mode", "disabled"); //CSS设置输入法不可用    

    loadMenus();
});

