
define(function (require, exports, module) {

    //登录
    exports.BtnLogin = function () {
        var userName = $("#username").val(),
            password = $("#password").val();
        $.ajax({
            url: "/Login/CheckUser",
            type: "post",
            dataType: "json",
            data: { userName: userName, password: password },
            beforeSend: function () {
                layer.load();
            },
            success: function (json) {
                layer.closeAll("loading");
                switch (json.status) {
                    case "success":
                        window.location.href = "/Home/Index";
                        break;
                    default:
                        layer.alert(json.msg);
                        break;
                }
            },
            error: function () {
                layer.alert("登录出错,请重新登录!");
            }
        });
    };

});