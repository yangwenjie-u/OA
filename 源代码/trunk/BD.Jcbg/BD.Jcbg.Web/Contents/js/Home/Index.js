define(function (require, exports, module) {



    //退出系统公共方法
    exports.LoginOutFunc = function () {
        $.ajax({
            url: "/Login/LoginOut",
            success: function () {
                top.window.location.href = "/Login/Index";
            },
            error: function () {
                layer.alert("退出系统出错!");
            }
        });
    }

    //退出
    exports.LoginOut = function () {
        layer.confirm("您确定要退出本次登录吗?", function () {
            exports.LoginOutFunc();
        });
    };

    //修改密码页面
    exports.ChangePassWord = function () {
        layer.open({
            type: 2,
            title: ["修改密码", "font-size:16px;font-weight:900;"],
            shadeClose: false,
            shade: 0.8,
            moveType: 1, //拖拽风格，0是默认，1是传统拖动
            shift: 1, //0-6的动画形式，-1不开启
            area: ["500px", "400px"],
            content: "/Home/ChangePwd"
        });
    };

    //修改密码的表单验证
    exports.ValidatePwd = function () {
        //设置样式
        $.validator.setDefaults({
            highlight: function (e) {
                $(e).closest(".form-group").removeClass("has-success").addClass("has-error");
            },
            success: function (e) {
                e.closest(".form-group").removeClass("has-error").addClass("has-success");
            },
            errorElement: "span",
            errorPlacement: function (e, r) {
                e.appendTo(r.is(":radio") || r.is(":checkbox") ? r.parent().parent().parent() : r.parent());
            },
            errorClass: "help-block m-b-none",
            validClass: "help-block m-b-none"
        });
        //验证表单并提示信息
        var e = "<i class='fa fa-times-circle'></i> ";
        $("#ChangePwdForm").validate({
            rules: {
                oldPwd: { required: true, minlength: 5 },
                newPwd: { required: true, minlength: 5 },
                confirmPwd: { required: true, minlength: 5, equalTo: "#newPwd" }
            },
            messages: {
                oldPwd: { required: e + "请输入您的旧密码", minlength: e + "密码必须5个字符以上" },
                newPwd: { required: e + "请输入您的新密码", minlength: e + "密码必须5个字符以上" },
                confirmPwd: { required: e + "请再次输入新密码", minlength: e + "密码必须5个字符以上", equalTo: e + "两次输入的密码不一致" }
            },
            submitHandler: function () {
                exports.UpdatePw();
            }
        });
    };

    //更新密码
    exports.UpdatePw = function () {
        $("#ChangePwdForm").ajaxSubmit({
            url: "/Home/UpdatePwd",
            type: "post",
            dataType: "json",
            beforeSubmit: function () {
                layer.load();
            },
            success: function (data) {
                layer.closeAll("loading");
                switch (data.status) {
                    case "success":
                        layer.msg(data.msg, {
                            icon: 1,
                            time: 2000 //2秒关闭（默认3秒）
                        }, function () {
                            exports.LoginOutFunc();
                        });
                        break;
                    default:
                        layer.alert(data.msg);
                        break;
                }
            },
            error: function () {
                layer.alert("修改密码出错!");
            }
        });
    };

});