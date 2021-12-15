function CustomFun() {
    var ret = false;
    var zh = GetCtrlValue("I_M_RY.ZH").trim();
    var sfz = GetCtrlValue("I_M_RY.SFZHM").trim();
    var yzm = GetCtrlValue("I_M_RY.SJYZM").trim();
    var sjhm = GetCtrlValue("I_M_RY.SJHM").trim();
    if (!checkZh(zh)) {
        return false;
    }
    var xm = GetCtrlValue("I_M_RY.RYXM").trim();
    if (!checkXm(xm)) {
        return false;
    }
    if (!checkSjhm(sjhm))
        return false;
    if (!checkSfz(sfz))
        return false;
    if (!checkRepeat(xm, sfz, sjhm))
        return false;
    if (GetCtrlValue("I_M_RY.TYYHXY") != '1') {
        alert('必须同意用户协议才能注册');
        return ret;
    }    
    
    $.ajax({
        type: "POST",
        url: "/ry/checkregister?username=" + encodeURIComponent(zh) + "&idno=" + encodeURIComponent(sfz)+"&yzm="+encodeURIComponent(yzm),
        dataType: "json",
        async: false,
        success: function (data) {
            ret = data.code == "0";
            if (data.msg != "") {
                alert(data.msg);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
    return ret;
}

function checkZh(zh) {

    zh = zh.trim();
    if (zh.length < 6 || zh.length > 32) {
        alert("账号长度无效");
        return false;
    }
    var reg = /^[a-zA-Z0-9_]*$/;
    if (!reg.test(zh)) {
        alert("账号包含非法的字符！");
        return false;
    }
    return true;
}
function checkXm(xm) {
    if (xm.length < 2 || xm.length > 15) {
        alert('请输入姓名，2~15个汉字！');
        return false;
    }
    var reg = /^[\u4E00-\u9FA5]+$/;
    if (!reg.test(xm)) {
        alert('姓名只允许输入汉字！');
        return false;
    }
    return true;
}

function checkSfz(sfzh) {
    var idcardObj = Idcard.createNew(sfzh);
    if (!idcardObj.IdCardValidate()) {
        alert('请输入正确的身份证号码！');
        return false;
    }
    return true;
}
function checkSjhm(sjhm) {
    var reg = /^1[0-9]{10}$/;

    if (!reg.test(sjhm)) {
        alert('请输入正确的手机号码！');
        return false;
    }
    return true;
}

function checkRepeat(realname, sfzh, sjhm) {
    var ret = false;
    // 校验用户信息
    $.ajax({
        type: "POST",
        url: "/user/checkuserregisterfirststep?realname=" + encodeURIComponent(realname) + "&sfzh=" + encodeURIComponent(sfzh)+"&sjhm="+encodeURIComponent(sjhm),
        dataType: "json",
        async: false,
        success: function (data) {
            try{
                ret = data.code == "0";
                    
                // 新注册
                if (!ret){
                    //姓名，手机，身份证一样，跳转到找回密码
                    if (data.msg == "1") {
                        openResetpassWindow(realname, sfzh, sjhm);
                    } else if (data.msg == "2") {
                        layer.alert('您的身份证已经注册账号，但是手机或者是姓名和您当前输入的不一致，无法重新注册。请携带相关证件到温州市建设工程质量监督站进行核验操作！', {
                            icon: 0,
                            skin: 'layer-ext-moon'
                        });
                    } else if (data.msg == "3") {
                        layer.alert('您的手机已经注册账号，但是身份证和您输入的不一致，无法重新注册。请携带相关证件到温州市建设工程质量监督站进行核验操作！', {
                            icon: 0,
                            skin: 'layer-ext-moon'
                        });
                    } else {
                        layer.alert(data.msg, {
                            icon: 0,
                            skin: 'layer-ext-moon'
                        });
                    }

                }
            } catch (err) {
                alert(err);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
    return ret;
}

function openResetpassWindow(realname,sfzh,sjhm) {
    try {
        var realname = realname.trim();
        var sfzh = sfzh.trim();
        var sjhm = sjhm.trim();
        parent.layer.closeAll();

        parent.layer.open({
            type: 2,
            title: '重置密码——第一步',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '500px'],
            content: '/user/resetpassfirststep?type=u&xm=' + encodeURIComponent(realname) + "&msg=" + encodeURIComponent("您的账号已注册，无法重复注册。现在为您跳转到密码重置页面，您可以找回密码。"),
            end: function () {
            }
        });
    }
    catch (err) {
        alert(err);
    }
}