




function getWorkSum() {
    ajaxTpl("/workflow/getworktodocount", "", function (data) {
        $("#sp_work_sum").html(data.count);
        if (data.count) {
            $(".side_icon0").hide();
            $(".side_icon1").show();
        } else {
            $(".side_icon0").show();
            $(".side_icon1").hide();
        }
    });
}
function getNewMailSum() {
    var ret = 0;
    try {
        $.ajax({
            type: "POST",
            url: "/oa/getnewmailsum",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == 0)
                    ret = data.msg * 1;

                $("#sp_mail_sum").html(ret);

                if (ret) {
                    $(".side_mail0").hide();
                    $(".side_mail1").show();
                } else {
                    $(".side_mail0").show();
                    $(".side_mail1").hide();
                }
            },
            complete: function (XMLHttpRequest, textStatus) { },
            beforeSend: function (XMLHttpRequest) { }
        });
        setMail();
    } catch (e) { }
    return ret;
}

function getRYYJSum() {
    ajaxTpl("/info_yj/getRYYJSum", "", function (data) {
        $("#sp_ryyj_sum").html(data.count);
    });
}

function getXZYJSum() {
    ajaxTpl("/info_yj/getXZYJSum", "", function (data) {
        $("#sp_xzyj_sum").html(data.count);
    });
}
getWorkSum();
getNewMailSum();
getRYYJSum();
getXZYJSum();

$("#slidMenus").on("click", "span", function () {
    var val = $(this).attr("value");
    if (val == 0) {

        //收起
        $("#navList").find("span").hide();
        $("#navList").find('.nav-sed').hide();

        $("#slidMenus").animate({
            width: "80px"
        }, 1000);
        $("#navList").animate({
            width: "80px"
        }, 1000);


        var width = $("#mainPage").width();

        $("#mainPage").animate({
            width: width + 120
        }, 1000);
        $("#navMenus").animate({
            width: width + 120
        }, 1000);

        $(this).hide().siblings().show();

    } else {
        //展开
        var width = $("#mainPage").width();

        $("#mainPage").animate({
            width: width - 120
        }, 1000);
        $("#slidMenus").animate({
            width: "200px"
        }, 1000);

        $("#navMenus").animate({
            width: width - 120
        }, 1000);
        $("#navList").animate({
            width: "200px"
        }, 1000);
        setTimeout(function () {
            $("#navList").find("span").show();
        },
            1000);

        $(this).hide().siblings().show();
    }

});



function logout() {
    try {
        $.ajax({
            type: "POST",
            url: "/user/dologout",
            dataType: "json",
            success: function (data) {
                if (data.code == 0)
                    window.location = g_logouturl;
                else
                    layer.open({ title: '提示', content: data.msg, icon: 5 });
            },
            complete: function (XMLHttpRequest, textStatus) {
                layer.closeAll('loading');
            },
            beforeSend: function (XMLHttpRequest) {
                layer.load();;
            }
        });
    } catch (e) {
        layer.open({ title: '错误', content: e, icon: 5 });
    }
}


// 生成二维码
// function generateBar() {
//     try {
//         $("#barIos").qrcode({
//             render: "canvas", //table方式
//             width: 135, //宽度
//             height: 135, //高度
//             correctLevel: 0,
//             text: "@GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_APPIOS") " //任意内容
//         });
//         $("#barAndroid").qrcode({
//             render: "canvas", //table方式
//             width: 135, //宽度
//             height: 135, //高度
//             correctLevel: 0,
//             text: "@GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_APPANDROID") " //任意内容
//         });
//     } catch (e) {
//         alert(e);
//     }
// }
var MenusData;
//nav导航
function loadMenus() {
    ajaxTpl("/user/getmenusv2", "", function (data) {
        MenusData = data.one_caidan;
        var tmp, val, str = '',
            picClass2;
        for (var i = 0, len = MenusData.length; i < len; i++) {
            tmp = MenusData[i];

            picClass2 = tmp.one_caidan_pic_class;
            tmp.one_caidan_pic_class = tmp.one_caidan_pic_class.replace("-select", "");
            if (tmp.two_caidan.length) { //有二级节点（做了去掉.操作）
                str += '<li class="nav-fir" value="' + i + '" name="' + tmp.MenuId + '" ><img class="nav-image" src="/static/image' + tmp.one_caidan_pic_class + '" /><img class="nav-image2" src="/static/image' + picClass2 + '" /><span class="nav-text">' + tmp.one_caidan_name.replace('.', '') + '</span>';

                str += "<ul class='nav-sed'>";
                for (var j = 0, len2 = tmp.two_caidan.length; j < len2; j++) {
                    val = tmp.two_caidan[j];
                    str += '<li value="' + j + '" name="' + val.MenuId + '"><span class="nav-text">' + val.two_caidan_name.replace('.', '') + '</span></li>';
                }
                str += "</ul></li>";
            } else { //不存在二级节点
                str += '<li class="nav-fir" value="' + i + '"  name="' + tmp.MenuId + '" ><img class="nav-image" src="/static/image' + tmp.one_caidan_pic_class + '" /><img class="nav-image2" src="/static/image' + picClass2 + '" /><span class="nav-text">' + tmp.one_caidan_name.replace('.', '') + '</span></li>';
            }
        }
        $("#navList").append(str);
    });
}

//点击显示第二节点
$("#navList").on('click', '.nav-fir', function () {
    $(this).addClass('active').siblings().removeClass('active').find('.nav-sed').hide();
    var sed = $(this).find('.nav-sed');

    if (sed.length) {
        if (sed.is(":hidden")) {
            sed.show();
        } else {
            sed.hide();
        }
        ////页面跳转
        var i = $(this).attr("value");
        if (i != -1) { //跳转到首页
            getFirstPage(i);
        }
    } else {
        //页面跳转
        var i = $(this).attr("value");
        if (i == -1) { //跳转到首页
            openPage("", "homepage");
            return;
        }
        getPage(i);
    }
});
//点击第二节点 跳转页面
$("#navList").on('click', '.nav-sed li', function () {
    $(this).parents('nav-fir').addClass('active').siblings().removeClass('active');
    $(this).addClass('active').siblings().removeClass('active');
    var j = $(this).attr("value");
    var i = $(this).parents(".nav-fir").attr("value");
    getPage(i, j);
    //页面跳转
    return false;
});

//一级
function getFirstPage(i) {
    try {
        var tmp, name;
        tmp = MenusData[i];
        //去掉.
        name = tmp.one_caidan_name.replace('.', '');
        if (tmp.MenuUrl != "") {
            openPage(tmp.MenuUrl, tmp.MenuId, name);
        }
    } catch (e) {
        layer.msg("页面跳转失败,请稍后再试", {
            icon: 2,
            time: 2000
        });
    }
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
        openPage(tmp.MenuUrl, tmp.MenuId, name);

    } catch (e) {
        layer.msg("页面跳转失败,请稍后再试", {
            icon: 2,
            time: 2000
        });
    }
}

function openPage(url, id, name) {

    var tmp = $("#" + id);
    var navMenus = $("#navMenus");
    //已经存在的页面
    if (tmp.length) {
        //tmp.show().siblings().hide();
        tmp.addClass('active').siblings().removeClass('active');
        navMenus.find("[value='" + id + "']").addClass("active").siblings().removeClass("active");
        return;
    }

    var mainPage = $("#mainPage")
    var child = mainPage.children();


    if (child.length < 5) { //不存在的页面 且标签少于5个
    } else {
        // child.hide().eq(1).remove();
        child.removeClass('active').eq(1).remove();
        navMenus.children().eq(1).remove();
    }
    navMenus.children().removeClass('active').parent().append('<span class="active" value="' + id + '">' + name + '<i class="close-icon"></i></span>');
    child.removeClass('active').parent().append('<iframe class="active" id="' + id + '" src="' + url + '" ></iframe>');
}


$("#navMenus").on("click", "span", function () {

    // var i = $(this).index();
    // $(this).addClass("active").siblings().removeClass("active");
    // $("#mainPage").children().hide().eq(i).show();


    var val = $(this).index();
    var doc = $("#navList").children().eq(val);
    //var val = $(this).attr("value");
    //var doc = $("#navList").find("[name = '" + val + "']");

    var par = doc.parent();
    if (par.hasClass('nav-sed')) { //二级节点
        par.show().parents(".nav-fir").addClass('active').siblings().removeClass("active").find('.nav-sed').hide();
        // doc.click();
        doc.addClass('active').siblings().removeClass("active");
    } else {
        //一级节点
        // doc.click();
        doc.addClass('active').siblings().removeClass("active");
    }

    if (doc.length) {
        var i = $(this).index();
        $(this).addClass('active').siblings().removeClass("active");
        // $("#mainPage").children().hide().eq(i).show();
        $("#mainPage").children().removeClass('active').eq(i).addClass('active');
    }

}).on("click", ".close-icon", function () {
    var that = $(this).parent();
    var i = that.index();
    if (that.hasClass("active")) {
        that.prev().click();
    }
    that.remove();
    $("#mainPage").children().eq(i).remove();
    return false;
});


//点击退出
function logout() {
    ajaxTpl("/user/dologout", "", function (data) {
        if (data.code == 0) {
            window.location = g_logouturl;
        } else {
            layer.open({ title: '提示', content: data.msg, icon: 5 });
        }
    })
}