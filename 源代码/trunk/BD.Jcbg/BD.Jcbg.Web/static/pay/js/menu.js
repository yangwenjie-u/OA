
function loadMenus() {
    $.ajax({
        type: "POST",
        url: "/user/getmenus",
        dataType: "json",
        async: false,
        success: function (data) {
            initMenus();
            var menus = data.filter(function(p){
                return !p.IsGroup;
            });
            $.each(menus, function (i, item) {
                if (i < 5) {
                    $("<li onclick=\"gotoPage('" + item.MenuUrl + "')\">" + item.MenuName + "</li>").insertBefore($("#btnExit"));
                } else if (i == 5) {
                    $("<li id=\"moreCon\">更多<ul class=\"more-info\"></ul></li>").insertBefore($("#btnExit"));
                    $(".more-info").append("<li onclick=\"gotoPage('" + item.MenuUrl + "')\">" + item.MenuName + "</li>");
                } else {
                    $(".more-info").append("<li onclick=\"gotoPage('" + item.MenuUrl + "')\">" + item.MenuName + "</li>");
                }
            });
            $("#moreCon").hover(function () {
                $(this).find("ul").show();
            }, function () {
                $(this).find("ul").hide();
            });
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
function gotoPage(url) {
    window.location.replace(url);
}
function initMenus() {
    $(".head-ul").children().each(function (i, item) {
        if ($(item).attr("id") != "btnExit") {
            $(item).remove();
        }
    });
}