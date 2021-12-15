// left导航hover
$(document).on("mouseover mouseleave", '.nav_item', function (event) {
    if (event.type == "mouseover") {
        //鼠标悬浮
        $(this).addClass('nav_name_select');
        $(this).children('.content_box').show();
    } else if (event.type == "mouseleave") {
        //鼠标离开
        $(this).removeClass('nav_name_select');
        $(this).children('.content_box').hide();
    }
})

// left导航点击
$(document).on("click", '.nav_name', function () {
    var flag = $(this).next('.content_box')[0]
    if (!flag) {
        $('.nav_name').removeClass('nav_name_select');
        $(this).addClass('nav_name_select');
        toggleHtml($(this).children('.nav_text').text(), $(this).attr('name'));
    }
    //openPage($(this).attr('name'));
    // var parent = $(this).parent();
    // var value = parent.attr('value');//index
    // if (!parent.children().hasClass("content_box")) {
    //     if (value == -1)
    //         openPage("homepage");
    //     else {
    //         // getPage(value);
    //         var text = $(this).children('span').text();//一级菜单标题，两个字
    //         toggleHtml(text, $(this).attr('name'));
    //     }
    // }
})

// 弹窗导航hover
$(document).on("mouseover mouseleave", '.content_item', function (event) {
    if (event.type == "mouseover") {
        $(this).children('.item_name').css({
            'color': '#0B87DD',
            'font-weight': 'bold'
        });
    } else if (event.type == "mouseleave") {
        $(this).children('.item_name').css({
            'color': '',
            'font-weight': ''
        });
    }
})

// 点击弹窗导航
$(document).on("click", '.content_item', function () {
    $(this).parent().children('.content_item').children('.item_name').removeClass('item_name_select');
    $('.nav_name').removeClass('nav_name_select');
    $(this).children('.item_name').addClass('item_name_select');
    $(this).parent().parent().prev('.nav_name').addClass('nav_name_select');
    $('.content_box').hide();
    var value = $(this).attr('value');
    var parent = $(this).parent().parent().parent();
    var pvalue = parent.attr('value');
    toggleHtml($(this).children('.item_name').text(), $(this).attr('name'));
    // getPage(pvalue, value);
    //openPage($(this).attr('name'));
})

// 点击左边导航，头部出现对应导航。最多出现4个
function toggleHtml(texts, idname) {
    var arr = $('.top_nav_list>ul>li')
    for (var i = 0; i < arr.length; i++) {
        if (arr.eq(i).children('span').text() === texts) {
            $('.top_nav_list>ul>li').removeClass('top_nav_list_select')
            arr.eq(i).addClass('top_nav_list_select')
            openPage(idname);
            return false
        }
    }
    $('.top_nav_list>ul>li').removeClass('top_nav_list_select')
    if (arr.length >= 5) {
        $('.top_nav_list>ul>li').eq(1).remove()
        $('.top_nav_list>ul').append(`<li class="top_nav_list_select" name="${idname}"><span>${texts}</span><i class="iconfont icon-quxiao"></i></li>`)
    } else {
        $('.top_nav_list>ul').append(`<li class="top_nav_list_select" name="${idname}"><span>${texts}</span><i class="iconfont icon-quxiao"></i></li>`)
    }
    openPage(idname);
};


// 点击头部导航
function topnavClick() {
    $('.top_nav_list>ul').on('click', 'li', function () {
        // var index = $(this).index();
        // $("#mainPage").children().removeClass('active').eq(index).addClass('active');
        event.stopPropagation();
        $('.top_nav_list>ul>li').removeClass('top_nav_list_select')
        $(this).addClass('top_nav_list_select')
        $('.nav_name').removeClass('nav_name_select');
        for (var i = 0; i < $('.nav_name').length; i++) {
            if ($('.nav_name').eq(i).next('.content_box')[0]) {
                $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').children('.item_name').removeClass('item_name_select');
                for (var j = 0; j < $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').length; j++) {
                    if ($('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').text() === $(this).children('span').text()) {
                        $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').addClass('item_name_select');
                        $('.nav_name').eq(i).addClass('nav_name_select')
                    }
                }
            } else {
                if ($('.nav_name').eq(i).children('.nav_text').text() === $(this).children('span').text()) {
                    $('.nav_name').eq(i).addClass('nav_name_select');
                }
            }
        }
        openPage($(this).attr('name'))
    })
}
topnavClick()

//一处头部导航
function removeLi() {
    $('.top_nav_list>ul').on('click', 'li>i', function (e) {
        if ($(this).parent().children('span').text() === '首页') {
            return false
        }
        // var index = $(this).parent().index();
        // $("#mainPage").children().eq(index).remove();
        var name = $(this).parent().attr('name')
        $(`#${name}`).remove()
        $(this).parent().remove()
        if ($(this).parent().attr('class') === 'top_nav_list_select') {
            $('.top_nav_list>ul>li:last').addClass('top_nav_list_select')
            $('.nav_name').removeClass('nav_name_select');
            for (var i = 0; i < $('.nav_name').length; i++) {
                if ($('.nav_name').eq(i).next('.content_box')[0]) {
                    $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').children('.item_name').removeClass('item_name_select');
                    for (var j = 0; j < $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').length; j++) {
                        if ($('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').text() === $('.top_nav_list>ul>li:last').children('span').text()) {
                            $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').addClass('item_name_select');
                            $('.nav_name').eq(i).addClass('nav_name_select')
                        }
                    }
                } else {
                    if ($('.nav_name').eq(i).children('.nav_text').text() === $('.top_nav_list>ul>li:last').children('span').text()) {
                        $('.nav_name').eq(i).addClass('nav_name_select')
                    }
                }
            }
            openPage($('.top_nav_list>ul>li:last').attr('name'))
        }
        e.stopPropagation();
    })
}
removeLi()




var MenusData;
//获取菜单
function loadMenusNew() {
    $.ajax({
        type: "POST",
        url: "/user/getmenusv2",
        data: {
        },
        dataType: "json",
        //async: false,
        success: function (data) {
            MenusData = data.one_caidan;
            var tmp, val, str = '', picClass2;
            //菜单两种颜色渐变
            var towcolor = '<div style="width: 100%;height: 100%;"><div style = "width: 100%;height: 25px;background:#525357;" ></div> <div style="width: 100%;height: 25px;background:#45464B;"></div> </div >';
            for (var i = 0, len = MenusData.length; i < len; i++) {
                tmp = MenusData[i];
                picClass2 = tmp.one_caidan_pic_class;
                tmp.one_caidan_pic_class = tmp.one_caidan_pic_class.replace("-select", "");
                if (tmp.two_caidan.length) { //有二级节点（做了去掉.操作）
                    str += '<li class="nav_item" value="' + i + '"  name="' + tmp.MenuId + '" >' + towcolor + '<div class="nav_name" > <i class="iconfont '+ (tmp.one_caidan_pic_class ? tmp.one_caidan_pic_class : "icon-daibangongzuo") +'" style="font-size: 18px;"></i>' +
                        '<span class="nav_text"> ' + tmp.one_caidan_name.replace('.', '').substring(0, 2) + '</span ></div>';

                    str += '<div class="content_box"><div class="content_list_title"><div>' + tmp.one_caidan_name.replace('.', '') + '</div></div><ul class="content_list">';
                    for (var j = 0, len2 = tmp.two_caidan.length; j < len2; j++) {
                        val = tmp.two_caidan[j];
                        str += '<li class="content_item" value="' + j + '" name="' + val.MenuId + '"><div class="iconfont item_icon '+ (val.two_caidan_pic_class ? val.two_caidan_pic_class : "icon-daibangongzuo") +'"></div><div class="item_name">'
                            + val.two_caidan_name.replace('.', '') + '</div></li>';
                    }
                    str += "</ul></div></li>";
                } else { //不存在二级节点
                    str += '<li class="nav_item" value="' + i + '"  name="' + tmp.MenuId + '" >' + towcolor + '<div class="nav_name"><i class="iconfont '+ (tmp.one_caidan_pic_class ? tmp.one_caidan_pic_class : "icon-daibangongzuo") +'"  style="font-size: 18px;"></i>' +
                        '<span class="nav_text"> ' + tmp.one_caidan_name.replace('.', '').substring(0, 2) + '</span ></div></li > ';
                }
            }
            $("#ui_left_nav").append(str);
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}

function openPage(name) {
    $('#mainPage').children().removeClass('active')
    if ($(`#${name}`).attr('id')) {
        $(`#${name}`).addClass('active')
        return false
    }
    var url = ''
    for (var i = 0; i < MenusData.length; i++) {
        if (name === MenusData[i].MenuId) {
            url = MenusData[i].MenuUrl
        } else {
            for (var j = 0; j < MenusData[i].two_caidan.length; j++) {
                if (name === MenusData[i].two_caidan[j].MenuId) {
                    url = MenusData[i].two_caidan[j].MenuUrl
                }
            }
        }
    }
    if ($('#mainPage').children().length > 5) {
        $('#mainPage').children().eq(1).remove()
        $('#mainPage').append('<iframe class="active" id="' + name + '" src="' + url + '" ></iframe>')
    } else {
        $('#mainPage').append('<iframe class="active" id="' + name + '" src="' + url + '" ></iframe>')
    }
    // child.removeClass('active').parent().append('<iframe class="active" id="' + name + '" src="' + url + '" ></iframe>');
}

//获取待办事项数量
function getWorkSum() {
    $.ajax({
        type: "POST",
        url: "/workflow/getworktodocount",
        data: {
        },
        dataType: "json",
        //async: false,
        success: function (data) {
            $("#dbsxCount").html(data.count);
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}

//获取邮件数量
function getNewMailSum() {
    $.ajax({
        type: "POST",
        url: "/oa/getnewmailsum",
        data: {
        },
        dataType: "json",
        //async: false,
        success: function (data) {
            if (data.code == "0")
                $("#mailCount").html(data.msg);
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}

//二级
function getPage(i, j) {
    try {
        var tmp, name;
        //去掉.
        if (j && 0 <= j) {
            tmp = MenusData[i].two_caidan[j];
            name = tmp.two_caidan_name.replace('.', '');
        } else {
            tmp = MenusData[i];
            name = tmp.one_caidan_name.replace('.', '');
        }
        openPage(tmp.MenuUrl, tmp.MenuId, name, i);

    } catch (e) {
        layer.msg("页面跳转失败,请稍后再试", {
            icon: 2,
            time: 2000
        });
    }
}

//点击下载
$("#down").click(function () {
    generateBar();
    generateBar = function () { }
    layer.open({
        type: 1,
        content: $('#layer')
    });

});
function generateBar() {
    $("#barIos").qrcode({
        render: "canvas", //table方式
        width: 135, //宽度
        height: 135, //高度
        correctLevel: 0,
        text: '@GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_APPIOS ")' //任意内容
    });
    $("#barAndroid").qrcode({
        render: "canvas", //table方式
        width: 135, //宽度
        height: 135, //高度
        correctLevel: 0,
        text: '@GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_APPANDROID ")' //任意内容
    });
}

//点击退出
$(".top_nav_return").click(function () {
    $.ajax({
        type: "post",
        url: "/User/DoLogout",
        dataType: "json",
        success: function (data) {
            window.location.href = '/';
        }
    });
});

$('#opentask').click(function(){
    $('.nav_name').removeClass('nav_name_select');
    for (var i = 0; i < $('.nav_name').length; i++) {
        if ($('.nav_name').eq(i).next('.content_box')[0]) {
            $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').children('.item_name').removeClass('item_name_select');
            for (var j = 0; j < $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').length; j++) {
                if ($('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').text() === '待办工作') {
                    $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').addClass('item_name_select');
                    $('.nav_name').eq(i).addClass('nav_name_select')
                }
            }
        } else {
            if ($('.nav_name').eq(i).children('.nav_text').text() === '待办工作') {
                $('.nav_name').eq(i).addClass('nav_name_select');
            }
        }
    }
    toggleHtml('待办工作', 'DBGZ')
    openPage('DBGZ')
})
$('#openEmail').click(function(){
    $('.nav_name').removeClass('nav_name_select');
    for (var i = 0; i < $('.nav_name').length; i++) {
        if ($('.nav_name').eq(i).next('.content_box')[0]) {
            $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').children('.item_name').removeClass('item_name_select');
            for (var j = 0; j < $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').length; j++) {
                if ($('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').text() === '内部邮件') {
                    $('.nav_name').eq(i).next('.content_box').children('.content_list').children('.content_item').eq(j).children('.item_name').addClass('item_name_select');
                    $('.nav_name').eq(i).addClass('nav_name_select')
                }
            }
        } else {
            if ($('.nav_name').eq(i).children('.nav_text').text() === '内部邮件') {
                $('.nav_name').eq(i).addClass('nav_name_select');
            }
        }
    }
    toggleHtml('内部邮件', 'NBYJ')
    openPage('NBYJ')
})