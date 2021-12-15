var userid = "wgryuserC005-7D5B-4231-8CEA-16939BEACD67";
var pwdid = "wgrypwdC005-7D5B-4231-8CEA-16939BEACD67";
var wgryid = "wgryC005-7D5B-4231-8CEA-16939BEACD67";

$(function () {
    //设置高度
    $(".fir-page").height(CONFIG.clientHeight - 60);

    GetLastUser();
    //$("#userName").val(getCookie("user"));
    //$("#passWord").val(getCookie("pass"));

     $('#title1').html(CONFIG.title1);
    $('#title2').html(CONFIG.title2);
});
document.onkeydown = function (e) {
    var ev = document.all ? window.event : e;
    if (ev.keyCode == 13) {
        $("#login").click();
    }
};

function GetLastUser() {
    if (typeof (CallCSharpMethodByResult) != 'undefined') {
        var username = CallCSharpMethodByResult("GetValue", userid);
        document.getElementById('userName').value = username;
        var pwd = CallCSharpMethodByResult("GetValue", pwdid);
        if (pwd == "") {
            document.getElementById('remberPass').checked = false;
        }
        else {
            document.getElementById('passWord').value = pwd;
            document.getElementById('remberPass').checked = true;
        }
    }
    else {
        var id = wgryid;//GUID标识符
        var usr = getCookie(id);
        if (usr != null) {
            document.getElementById('userName').value = usr;
            GetPwdAndChk();
        }
    }
}
function GetPwdAndChk() {
    var usr = document.getElementById('userName').value;
    var pwd = getCookie(usr);
    if (pwd != "") {
        document.getElementById('remberPass').checked = true;
        document.getElementById('passWord').value = pwd;
    } else {
        document.getElementById('remberPass').checked = false;
        document.getElementById('passWord').value = "";
    }
}
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
    if (typeof (CallCSharpMethodByResult) != 'undefined')
        CallCSharpMethodByResult("SetValue", userid + "|" + user);
    else
        setCookie(wgryid, user, 3600000);
	
	layer.load();
	
    ajaxTpl("/user/dologin", {
        login_name: user,
        login_pwd: pass,
        _: Math.random()
    }, function(data) {
		layer.closeAll('loading');
        data = JSON.parse(data);
        try {
            //登录成功
            if (data.code == 0) {
                // 记住密码
                if ($('#remberPass').is(":checked")) {
                    if (typeof (CallCSharpMethodByResult) != 'undefined') {
                        CallCSharpMethodByResult("SetValue", pwdid + "|" + pass);
                    }
                    else
                    {
                        setCookie(user, pass, 3600000);
                    }             
                } else {
                    if (typeof (CallCSharpMethodByResult) != 'undefined') {
                        CallCSharpMethodByResult("SetValue", pwdid + "|");
                    }
                    else
                    {
                        setCookie(user, "", 3600000);
                       // delCookie(user);
                       // delCookie(pwdid);
                    }				
				}
                window.location = "/user/mainwz";
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

$('.info3').click(function(event) {
    //window.location = "/downs/专用浏览器.exe";
});

$('#header').on("click", "span", function() {
    $(this).addClass("select").siblings().removeClass('select');

    if ($(this).attr("value") == "down") {
        $(".fir-page").hide();
        $(".down-page").show();
    } else {
        $(".fir-page").show();
        $(".down-page").hide();
    }
})