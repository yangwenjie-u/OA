$(function() {
    //设置高度
    $("body").height(CONFIG.clientHeight);

    $('#header').on("click", "span", function() {
        $(this).addClass("select").siblings().removeClass('select');
    })

    $("#userName").val(getCookie("user"));
    $("#passWord").val(getCookie("pass"));

    //$('#title1').html(CONFIG.title1);
    //$('#title2').html(CONFIG.title2);

    $("#page_down").hide();
});
document.onkeydown = function (e) {
    var ev = document.all ? window.event : e;
    if (ev.keyCode == 13) {
        $("#login").click();
    }
};

$('#login').click(function() {
    var user = $("#userName").val();
    var pass = $("#passWord").val();
    if (!user) {
        layer.msg('请输入用户名', {
            icon: 2,
            time: 2000
        });
        return;
    } else if (!pass) {
        layer.msg('请输入密码', {
            icon: 2,
            time: 2000
        });
        return;
    }

    setCookie("user", user, 3600000);
    setCookie("pass", pass, 3600000);

    ajaxTpl("/user/dologin", {
        login_name: escape(user),
        login_pwd: escape(pass),
        _: Math.random()
    }, function(data) {
        data = JSON.parse(data);
        try {
            //登录成功
            if (data.code == 0) {
                // 记住密码
                if ($('#remberPass').is(":checked")) {
                    setCookie("user", user, 3600000);
                    setCookie("pass", pass, 3600000);
                }
                window.location.replace("/user/main");
            } else {
                var msg = data.msg
                if (msg == "") {
                    msg = "用户名或密码错误，请重新登录";
                }
                $("#passWord").val('');

                layer.open({ title: '提示', content: msg, icon: 2 });

            }
        } catch (e) {
            layer.open({ title: '错误', content: e.message, icon: 2 });
        }
    });
});

$('.info3').click(function (event) {
    window.location = "/downs/标点浏览器.exe";
});

$('#page_down').click(function (event) {
    //window.location = "/user/downsv3";
});