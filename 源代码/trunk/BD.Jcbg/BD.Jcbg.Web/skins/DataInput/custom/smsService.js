
var smsService = {
    sendSpan:0,
    send: function () {
        var obj = document.getElementById("I_M_RY.SJHM");
        if (obj == null)
            obj = document.getElementById("I_M_QY.LXSJ")
        var phone = obj.value;
        
        if (phone == "") {
            alert("手机号码不能为空！");
            return;
        }
        var p1 = /^1\d{10}$/;

        if (!p1.test(phone)) {
            alert("手机号码格式错误，请输入正确的手机号码！");
            return;
        }

        this.setSendVCButtonId();

        $.ajax({
            type: "POST",
            url: "/sms/getverifycodespan",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code != "0") {
                    var msg = data.msg;
                    if (msg == "")
                        msg = "获取短信发送间隔失败";
                    alert(msg);
                } else {
                    smsService.sendSpan = data.msg * 1; 
                    $.ajax({
                        type: "POST",
                        url: "/sms/dosendverifycode?receiver=" + phone,
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.msg != "") {
                                alert(data.msg);
                            } else {
                                smsService.setButtonText();
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
    setSendVCButtonId:function(){
        try {
            var btns = document.getElementsByTagName("input");
            for (var i = 0; i < btns.length; i++) {
                if (btns[i].value == "发送验证码" && btns[i].type == "button") {
                    btns[i].id = "btn_send_vc";
                }
            }
        } catch (err) {
            alert(err);
        }
    },
    setButtonText: function () {
        if (smsService.sendSpan == 0) {
            document.getElementById("btn_send_vc").value = "发送验证码";
            document.getElementById("btn_send_vc").disabled = false;
        } else {
            
            document.getElementById("btn_send_vc").value = smsService.sendSpan+"秒后再次发送";
            document.getElementById("btn_send_vc").disabled = true;
            smsService.sendSpan = smsService.sendSpan - 1;
            window.setTimeout(smsService.setButtonText, 1000);
        }
    }
}