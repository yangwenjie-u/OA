var userService = {
    createUser: function (userType, userCode) {

        $.ajax({
            type: "POST",
            url: "/remoteuser/adduser?usertype="+encodeURIComponent(userType)+"&usercode="+encodeURIComponent(userCode),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                } else {
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    },
    createUserWithSms: function (userType, userCode) {

        $.ajax({
            type: "POST",
            url: "/remoteuser/adduser?usertype=" + encodeURIComponent(userType) + "&usercode=" + encodeURIComponent(userCode),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                } else {
                    $.ajax({
                        type: "POST",
                        url: "/sms/dosenduserinfo?usercode=" + encodeURIComponent(userCode),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.msg != "") {
                                alert(data.msg);
                            } else {
                                alert("账号密码已经以短信方式发送到您的手机，请查收");
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    },

    createUserWithSmsAndDefaultRoles: function (userType, userCode) {

        $.ajax({
            type: "POST",
            url: "/remoteuser/adduserwithdefaultroles?usertype=" + encodeURIComponent(userType) + "&usercode=" + encodeURIComponent(userCode),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                } else {
                    $.ajax({
                        type: "POST",
                        url: "/sms/dosenduserinfo?usercode=" + encodeURIComponent(userCode),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.code=="0") {
                                alert("账号密码已经以短信方式发送到您的手机，请查收");
                            }
                            else {
                                if (data.msg == "") {
                                    data.msg = "添加账号失败！";
                                }
                                alert(data.msg);
                            }
                            
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    },
    updateUserRole: function (rybh) {
        $.ajax({
            type: "POST",
            url: "/ry/updateuserrole?rybh=" + encodeURIComponent(rybh),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    },
    updateQyRole: function (qybh) {
        $.ajax({
            type: "POST",
            url: "/qy/UpdateQyRole?qybh=" + encodeURIComponent(qybh),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
}