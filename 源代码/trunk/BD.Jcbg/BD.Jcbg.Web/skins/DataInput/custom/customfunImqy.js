function CustomFun() {
    var ret = false;
    var zh = GetCtrlValue("I_M_QY.ZH").trim();
    if (!checkZh(zh)) {
        return false;
    }
    
    if (GetCtrlValue("I_M_QY.TYYHXY")!='1') {
        alert('必须同意用户协议才能注册');
        return ret;
    }
    
    var zzjgdm = GetCtrlValue("I_M_QY.ZZJGDM").trim();
    
    console.log("1");
    if (zzjgdm.length != 9 && zzjgdm.length != 10 && zzjgdm.length != 18) {
        alert("组织机构代码或社会统一信用代码长度错误！");
        return false;
    }
    console.log("2");
    if (zzjgdm.length == 18 && !checkCode18(zzjgdm))
        return false;
    console.log("3");
    if (zzjgdm.length != 18 && !checkCode9(zzjgdm))
        return false;
    console.log("4");
    var sjhm = GetCtrlValue("I_M_QY.LXSJ").trim();
    return false;
    if (!checkSjhm(sjhm))
        return false;
    var qymc = GetCtrlValue("I_M_QY.QYMC");
    
    $.ajax({
        type: "POST",
        url: "/qy/checkqyvalid",
        data: "qymc=" + encodeURIComponent(qymc) + "&qydm=" + encodeURIComponent(zzjgdm),
        dataType: "json",
        async: false,
        success: function (data) {
            ret = data.code == "0";
            if (ret) {
                
                var yzm = GetCtrlValue("I_M_QY.SJYZM");
                $.ajax({
                    type: "POST",
                    url: "/qy/checkregister?username=" + encodeURIComponent(zh) + "&yzm=" + encodeURIComponent(yzm),
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
            } else {
                if (data.toreset == "0") {
                    ret = false;
                    parent.layer.closeAll();

                    parent.layer.open({
                        type: 2,
                        title: '重置密码——第一步',
                        shadeClose: true,
                        shade: 0.8,
                        area: ['800px', '500px'],
                        content: '/user/resetpassfirststep?type=q&xm=' + encodeURIComponent(data.msg) + "&msg=" + encodeURIComponent("企业名称，无法重复注册。现在为您跳转到密码重置页面，您可以找回密码。"),
                        end: function () {
                        }
                    });
                } else {
                    alert(data.msg);
                    ret = false;
                }
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
function checkSjhm(sjhm) {
    var reg = /^1[0-9]{10}$/;

    if (!reg.test(sjhm)) {
        alert('请输入正确的手机号码！');
        return false;
    }
    return true;
}
function checkCode18(Code) {
    Code = Code.trim();

    var patrn = /^[0-9A-Z]+$/;
    //18位校验及大写校验
    if ((Code.length != 18) || (patrn.test(Code) == false)) {
        alert("不是有效的统一社会信用编码！");
        return false;
    }
    else {
        var Ancode;//统一社会信用代码的每一个值
        var Ancodevalue;//统一社会信用代码每一个值的权重 
        var total = 0;
        var weightedfactors = [1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28];//加权因子 
        var str = '0123456789ABCDEFGHJKLMNPQRTUWXY';
        //不用I、O、S、V、Z 
        for (var i = 0; i < Code.length - 1; i++) {
            Ancode = Code.substring(i, i + 1);
            Ancodevalue = str.indexOf(Ancode);
            total = total + Ancodevalue * weightedfactors[i];
            //权重与加权因子相乘之和 
        }
        var logiccheckcode = 31 - total % 31;

        if (logiccheckcode == 31) {
            logiccheckcode = 0;
        }
        var Str = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,T,U,W,X,Y";
        var Array_Str = Str.split(',');
        logiccheckcode = Array_Str[logiccheckcode];

        var checkcode = Code.substring(17, 18);
        if (logiccheckcode != checkcode) {
            alert("组织机构代码或社会统一信用代码校验错误！");
            return false;
        }
    }
    return true;
}

function checkCode9(value) {
    value = value.trim();
    var ret = false;
    if (value.length != 9 && value.length != 10) {
        alert("组织机构代码或社会统一信用代码长度错误！");
        return ret;
    }
    if (value == 9)
        value = value.substr(0, 8) + '-' + value.substr(8, 1);

    var values = value.split("-");
    var ws = [3, 7, 9, 10, 5, 8, 4, 2];
    var str = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    var reg = /^([0-9A-Z]){8}$/;
    if (!reg.test(values[0])) {
        alert("组织机构代码或社会统一信用代码校验错误！");
        return ret
    }
    var sum = 0;
    for (var i = 0; i < 8; i++) {
        sum += str.indexOf(values[0].charAt(i)) * ws[i];
    }
    var C9 = 11 - (sum % 11);
    var YC9 = values[1] + '';
    if (C9 == 11) {
        C9 = '0';
    } else if (C9 == 10) {
        C9 = 'X';
    } else {
        C9 = C9 + '';
    }
    ret = YC9 == C9;
    if (!ret) {
        alert("无效的组织机构代码或社会统一信用代码！");
    }

    return ret;
}