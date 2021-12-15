
$("#btnMoreSub").on("click", function () {
    window.location.replace("/pay/moneytrans");
});

$("#btnEdhb1").on("click", function () {
    window.location.replace("/pay/moneytrans");
});
$("#btnRefresh").on("click", function () {
    setAccountInfo();
    loadData();
});
$("#btnSearch1").on("click", function () {
    getUncheckPays();
});
$("#btnSearch2").on("click", function () {
    getPayHistorys();
});
var g_accounts = [];
var g_accountmain = {};
var g_candomoneytrans = false;
$(function () {
    initEdhb();
    setAccountInfo();    
    loadData();
    initSearch();
    
});
function initEdhb() {
    $.ajax({
        type: "POST",
        url: "/pay/CanDoMoneyTrans",
        dataType: "json",
        success: function (data) {
            g_candomoneytrans = data.code == 0;
            if (g_candomoneytrans) {
                $("#btnEdhb1").show();
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
function setAccountInfo() {
    $.ajax({
        type: "POST",
        url: "/pay/GetAccountInfo",        
        dataType: "json",
        success: function (data) {
            if (data.code == 0) {
                if (data.records.length > 0) {
                    g_accountmain = data.records[0];
                    if (g_accountmain.yhzh == "") {
                        //$("#money1").text("");
                        $("#zhxm").text("未创建");
                    } else {
                        $("#money1").text(g_accountmain.zhye + "元");
                        $("#zhmc").text(g_accountmain.yhzh);
                        $("#zhxm").text(g_accountmain.yhzhmc);
                    }
                    
                }
            }
            else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
    $.ajax({
        type: "POST",
        url: "/pay/GetSubAccounts",
        data:"gcbh=&qybh=&pagesize=10&pageindex=1",
        dataType: "json",
        success: function (data) {
            if (data.code == 0) {
                g_accounts = data.records;// 分账号
                $(".h-other-ul").html("");
                var submoney = 0;
                $.each(g_accounts, function (i, item) {
                    submoney += item.zhye * 1;

                    if (i < 3) {
                        //+ "&nbsp;" + item.sgqymc
                        var btnEdStr = "";
                        if (g_candomoneytrans)
                            btnEdStr = "<span class=\"h-btn\" onclick=\"addEd('" + item.zhid + "')\">增加额度</span><span class=\"h-btn\" onclick=\"minEd('" + item.zhid + "')\">额度回拨</span>";
                        $(".h-other-ul").append("<li>"+btnEdStr+"<span class=\"h-btn\" onclick=\"viewDetail('" + item.zhid + "')\">查看记录</span><span class='text-button-padding'>" + item.zhmc  + "&nbsp;分账户额度&nbsp;" + item.zhye + "&nbsp;元</span></li>");
                    }
                });
                $("#money2").text(submoney.toFixed(2));
            }
            else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
function loadData() {   
    getUncheckPays();
    getPayHistorys();    
}
function addEd(zhid) {
    $("#tbEd").val("");
    $("#errEd").text("");
    $("#tbEdbz").val("额度划拨");
    layer.open({
        type: 1,
        title: '增加额度',
        skin: 'layui-layer-lan',
        shadeClose: true,
        shade: 0.8,
        area: ['320px', '250px'],
        content: $("#divEd"),
        btn: ["确定"],
        anim: 4,
        yes: function (index) {
            $("#errEd").text("");
            var ed = $("#tbEd").val();
            if (ed == "") {
                $("#errEd").text("请输入要划拨的额度");
                return;
            }
            if (ed > g_accountmain.zhye * 1) {
                $("#errEd").text("划拨额度不能大于当前余额:" + g_accountmain.zhye + "元");
                return;
            }
            var dlgIndex = layer.load(1, { shade: [0.6, '#eee'] });
            $.ajax({
                type: "POST",
                url: "/pay/domoneyout",
                data: "tozhid=" + encodeURIComponent(zhid) + "&ed=" + ed + "&bz=" + encodeURIComponent($("#tbEdbz").val()),
                dataType: "json",
                success: function (data) {
                    if (data.code == 0) {
                        layer.closeAll();
                        setAccountInfo();
                        layer.alert("划拨成功", {
                            title: '提示',
                            icon: 1
                        });
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
    });
}
function minEd(zhid) {
    $("#tbEd").val("");
    $("#tbEdbz").val("额度回拨");
    $("#errEd").text("");
    layer.open({
        type: 1,
        title: '减少额度',
        skin: 'layui-layer-lan',
        shadeClose: true,
        shade: 0.8,
        area: ['320px', '250px'],
        content: $("#divEd"),
        btn: ["确定"],
        anim: 4,
        yes: function (index) {
            $("#errEd").text("");
            var ed = $("#tbEd").val();
            if (ed == "") {
                $("#errEd").text("请输入要减少的额度");
                return;
            }
            var finds = g_accounts.filter(function(p){
                return p.zhid == zhid;
            });
            if (finds.length == 0) {
                $("#errEd").text("获取工程账户失败");
                return;
            }
            if (ed > finds[0].zhye * 1) {
                $("#errEd").text("划拨额度不能大于当前余额:" + finds[0].zhye + "元");
                return;
            }
            var dlgIndex = layer.load(1, { shade: [0.6, '#eee'] });
            $.ajax({
                type: "POST",
                url: "/pay/domoneyin",
                data: "fromzhid=" + encodeURIComponent(zhid) + "&ed=" + ed + "&bz=" + encodeURIComponent($("#tbEdbz").val()),
                dataType: "json",
                success: function (data) {
                    if (data.code == 0) {
                        layer.closeAll();
                        setAccountInfo();
                        layer.alert("回拨成功", {
                            title: '提示',
                            icon: 1
                        });
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
    });
}
function viewDetail(zhid) {
    var flag = true;
    getTable('/pay/GetMoneyTransList', {
        hbzh: zhid, pagesize: 10, pageindex: 1
    }, function (data) {
        if (data.code == 0) {
            $("#tbody2").html("");
            $.each(data.records, function (i, item) {
                var hblx = item.zfqyzh != g_accountmain.zhid ? "+" : "-";
                $("#tbody2").append("<tr><td>" + item.hbsj + "</td><td>" + item.gcmc + "</td><td>" + item.sgqymc + "</td><td class=\"warning\">" + hblx+ item.hbje + "元</td><td>" + item.mark + " </td></tr>");

            });
            if (flag) {
                layer.open({
                    type: 1,
                    title: '',
                    skin: 'layui-layer-lan',
                    shadeClose: true,
                    shade: 0.8,
                    area: ['95%', '600px'],
                    content: $("#divList"),
                    anim: 4
                });
                flag = false;
            }
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test3"));
}
var g_payid = "";

function showPayDetailDlg(recid, yfze, sfze) {
    g_payid = recid;
    $("#xm3").val("");
    layer.open({
        type: 1,
        title: '',
        skin: 'layui-layer-lan',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '600px'],
        content: $("#divPayDetail"),
        anim: 4
    });
    loadPayDetailData(g_payid, "",yfze,sfze);
}

function loadPayDetailData(payid, xm, yfze, sfze) {
    setClick(payid, "1");
    getTable('/pay/getpaydetail', {
        recid: payid
    }, function (data) {
        if (data.code == 0) {
            $("#tbody3").html("");
            $.each(data.records, function (i, item) {
                var kkjd = "";
                if (item.bkzt == "1")
                    kkjd += "绑卡完成";
                else
                    kkjd = item.kkcwxx;

                var ffjd = "";
                if (item.sfje * 1 < item.yfje*1)
                    ffjd += "待发";
                else
                    ffjd = "已发";
                var sfzhm = item.sfzhm;
                if (sfzhm != null && sfzhm.length>8)
                sfzhm = sfzhm.substr(0, 4) + "****" + sfzhm.substr(sfzhm.length - 4, 4);
                var jskh = item.jskh;
                if (jskh != null && jskh.length>8)
                jskh = jskh.substr(0, 4) + "****" + jskh.substr(jskh.length - 4, 4);
                var infoClass = "";
                if (item.sfje * 1 < item.yfje * 1)
                    infoClass = "info-exception";
                $("#tbody3").append("<tr class='"+infoClass+"'><td>"+(i+1)+"</td><td>" + item.ryxm + "</td><td>" + item.sjhm + "</td><td>" + sfzhm + "</td><td>" + jskh + "</td><td>" + item.yfje + "</td><td>" + item.sfje + "</td><td>" + item.bz + "</td><td title='" + kkjd + "'>" + kkjd + "</td><td>" + ffjd + " </td><td title='" + item.cwxx + "'>" + item.cwxx + "</td></tr>");

            });
            $("#tbody3").append("<tr><td colspan='5'>合计</td><td>" + yfze + "</td><td>"+sfze+"</td><td></td><td></td><td></td><td></td></tr>");
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, null);
}

function showAgreePayDlg(recid, zt, fjsum) {
    if (!canCheck(recid,fjsum))
        return;
    layer.confirm('您确定同意发放吗？', {
        btn: ['同意', '再想想'] //按钮
    }, function () {
        layer.closeAll();
        if (zt == 0) {
            layer.load(1, {
                shade: [0.6, '#eee'] //0.1透明度的白色背景
            });
            $.ajax({
                type: "POST",
                url: "/pay/DoCheckPay",
                data: "recid=" + encodeURIComponent(recid) + "&status=1",
                dataType: "json",
                success: function (data) {
                    if (data.code == 0) {
                        layer.closeAll();
                        loadData();
                        layer.alert("审批成功", {
                            title: '提示',
                            icon: 1
                        });
                        setTimeout(function () {
                            layer.closeAll();
                        }, 2000);
                    }
                    else {
                        layer.closeAll();
                        layer.alert(data.msg, {
                            title: '提示',
                            icon: 5
                        });
                        setTimeout(function () {
                            layer.closeAll();
                        }, 2000);
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
        else
            showPayCodeDialog(recid);
    }, function () {
    });
}
function showDisagreePayDlg(recid,fjsum) {
    try {
        if (!canCheck(recid,fjsum))
            return;
        var index = layer.open({
            type: 1,
            shadeClose: true,
            shade: 0.8,
            area: ['360px', '220px'],
            content: "<div style='padding:10px 10px 10px 10px;font-size:16px;line-height:30px;'>请输入拒绝原因：<span id='tmpError' style='color:red'>*</span><br/><textarea cols='30' rows='3' id='disagreeReason'/></div>",
            btn: ['拒绝'],
            yes: function (index, layero) {
                $("#tmpError").text("");
                var message = $("#disagreeReason").val();
                if (message == "") {
                    $("#tmpError").text("请输入原因");
                    return;
                }
                $("#tmpError").text("*");
                $.ajax({
                    type: "POST",
                    url: "/pay/docheckpay",
                    data: "recid=" + encodeURIComponent(recid) + "&status=-1&message="+encodeURIComponent(message),
                    dataType: "json",
                    success: function (data) {
                        if (data.code == 0) {
                            layer.closeAll();
                            loadData();
                            layer.alert("取消发放成功", {
                                title: '提示',
                                icon: 1
                            });
                            setTimeout(function () {
                                layer.closeAll();
                            }, 2000);
                        }
                        else
                            layer.open({ title: '提示', content: data.msg, icon: 5 });
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });

            }
        });

    } catch (e) {
        alert(e);
    }
}
/*
function showDisagreePayDlg(recid) {
    layer.confirm('您确定取消发放吗？', {
        btn: ['确定', '再想想'] //按钮
    }, function () {
        layer.closeAll();
        layer.load(1, {
            shade: [0.6, '#eee'] //0.1透明度的白色背景
        });
        $.ajax({
            type: "POST",
            url: "/pay/docheckpay",
            data: "recid=" + encodeURIComponent(recid) + "&status=-1",
            dataType: "json",
            success: function (data) {
                if (data.code == 0) {
                    layer.closeAll();
                    loadData();
                    layer.alert("取消发放成功", {
                        title: '提示',
                        icon: 1
                    });
                    setTimeout(function () {
                        layer.closeAll();
                    }, 2000);
                }
                else
                    layer.open({ title: '提示', content: data.msg, icon: 5 });
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }, function () {
    });
}
*/
function initSearch() {
    $("#btnSearch3").on("click", function () {
        loadPayDetailData(g_payid, $("#xm3").val());
    });

    $.ajax({
        type: "POST",
        url: "/pay/GetRelateProjects",
        dataType: "json",
        success: function (data) {
            if (data.code == 0) {
                $("#project1").empty();
                $("#project1").append("<option value=''>==不限==</option>");
                $("#company1").empty();
                $("#company1").append("<option value=''>==不限==</option>");
                $("#project2").empty();
                $("#project2").append("<option value=''>==不限==</option>");
                $("#company2").empty();
                $("#company2").append("<option value=''>==不限==</option>");
                $.each(data.records, function (i, item) {
                    $("#project1").append("<option value='" + item.gcbh + "'>" + item.gcmc + "</option>");
                    $("#project2").append("<option value='" + item.gcbh + "'>" + item.gcmc + "</option>");
                });
                layForm.render();
            }
            else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        }
    });
}

function showPayCodeDialog(payid) {
    try {

        var index = layer.open({
            type: 1,
            shadeClose: true,
            shade: 0.8,
            area: ['360px', '200px'],
            content: "<div style='padding:10px 10px 10px 10px;font-size:16px;line-height:30px;'>验证码将发送到您的手机<br/>请输入验证码：<input type='text' size='10' id='tmpVerifyCode'/><input type='button' value='获取验证码' onclick='sendVerifycode(\""+payid+"\")'/><br/><span id='tmpError' style='color:red'></span></div>",
            btn: ['提交验证'],
            yes: function (index, layero) {
                $("#tmpError").text("");
                var verifycode = $("#tmpVerifyCode").val();
                if (verifycode == "") {
                    $("#tmpError").text("请输入验证码");
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "/pay/DoSendBankInterfacePay",
                    dataType: "json",
                    data: "payid="+encodeURIComponent(payid)+"&verifycode=" + encodeURIComponent(verifycode),
                    async: false,
                    success: function (data) {
                        if (data.code == "0") {
                            loadData();
                            alert("操作成功！");
                            layer.close(index);
                        } else {
                            if (data.msg == "")
                                data.msg = "操作失败";
                            alert(data.msg);
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });

            }
        });

    } catch (e) {
        alert(e);
    }

}


function sendVerifycode(recid) {
    try {
        $.ajax({
            type: "POST",
            url: "/pay/DoSendBankInterfacePaySms",
            data:"payid="+encodeURIComponent(recid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    $("#tmpError").text("验证码已发送到您的手机");
                } else {
                    if (data.msg == "")
                        data.msg = "失败";
                    $("#tmpError").text(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
    catch (e) {
        alert(e);
    }
}

function showAttachDiv(recid) {
    try {
        setClick(recid, "2");
        var index = layer.open({
            title:"工资册附件查看",
            type: 2,
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/pay/payrollattach?id="+recid
        });

    } catch (e) {
        alert(e);
    }
}

var g_clickarray = [];
function setClick(id, itemtype) {
    var finds = g_clickarray.filter(function (item, index) {
        return item == id + "_" + itemtype;
    })
    if (finds.length == 0)
        g_clickarray.push(id + "_" + itemtype);
}
function canCheck(id,fjsum) {
    var finds = g_clickarray.filter(function (item, index) {
        return item.indexOf(id) == 0;
    })
    if ((finds.length < 2 && fjsum != "0")|| (finds.length<1 && fjsum == "0")) {
        layer.open({ title: '提示', content: "必须点击详情和附件后才能审批", icon: 5 });
        return false;
    }
    return true;
}

function getUncheckPays() {
    var gcbh = $("#project1").val();
    var qybh = $("#company1").val();
    getTable('/pay/GetUncheckPays', {
        gcbh: gcbh, qybh: qybh
    }, function (data) {
        if (data.code == 0) {
            $("#tbody1").html("");
            g_gcitems = data.records;
            $.each(g_gcitems, function (i, item) {
                var fjbutton = "<span class=\"h-btn\" onclick=\"showAttachDiv('" + item.recid + "')\">附件</span>";
                if (item.fj2 == "0")
                    fjbutton = "<span class=\"h-btn\" style='background-color:#606060'>附件</span>";
                $("#tbody1").append("<tr><td>" + item.zhye + "</td><td>" + item.ffnf + '-' + item.ffyf + "</td><td>" + item.lxmc + "</td><td title='" + item.gcmc + "'>" + item.gcmc + "</td><td title='" + item.qymc + "'>" + item.qymc + "</td><td title='" + item.bz1 + "'>" + item.bz1 + "</td><td>" + item.rysl + "</td><td>" + item.yfze + "</td><td>" + item.cjsj + "</td><td title='" + item.shsj + "'>" + item.ztms + "</td><td title='" + item.bz2 + "'>" + item.bz2 + "</td><td> <span class=\"h-btn\" onclick=\"showPayDetailDlg('" + item.recid + "','" + item.yfze + "','')\">详情</span> " + fjbutton + "<span class=\"h-btn\" onclick=\"showAgreePayDlg('" + item.recid + "','" + item.zt + "','" + item.fj2 + "')\">同意</span>  <span class=\"h-btn\" onclick=\"showDisagreePayDlg('" + item.recid + "','" + item.fj2 + "')\">拒绝</span> </td></tr>");

            });
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    });
}

function getPayHistorys() {
    var gcbh = $("#project2").val();
    var qybh = $("#company2").val();
    getTable('/pay/GetPayHistory', {
        gcmc: '', sgdw: '', ffzh: '', zt: "0,1,2,3,4,5,6,7,8", pagesize: 10, pageindex: 1, gcbh: gcbh, sgdwbh: qybh, gzzq1: "", gzzq2: "", spsj1: "", spsj2: ""
    }, function (data) {
        if (data.code == 0) {
            $("#tbody1_1").html("");
            g_gcitems = data.records;
            $.each(g_gcitems, function (i, item) {

                var ffwcbutton = "";
                var btnClass = "h-btn";
                if (item.ztms == "提现异常" || item.ztms == "异常" || item.ztms == "预提现失败")
                    btnClass = "h-btn-exception";
                var fjbutton = "<span class=\"h-btn\" onclick=\"showAttachDiv('" + item.recid + "')\">附件</span>";
                if (item.fj2 == "0")
                    fjbutton = "<span class=\"h-btn\" style='background-color:#606060'>附件</span>";
                $("#tbody1_1").append("<tr><td>" + item.zhye + "</td><td>" + item.ffnf + '-' + item.ffyf + "</td><td>" + item.lxmc + "</td><td title='" + item.gcmc + "'>" + item.gcmc + "</td><td title='" + item.qymc + "'>" + item.qymc + "</td><td title='" + item.bz1 + "'>" + item.bz1 + "</td><td>" + item.rysl + "</td><td>" + item.yfze + "</td><td>" + item.sfze + "</td><td title='" + item.shsj + "'>" + item.shsj + "</td><td title='" + item.ztms + "'>" + item.ztms + "</td><td title='" + item.bz2 + "'>" + item.bz2 + "</td><td> <span class=\"" + btnClass + "\" onclick=\"showPayDetailDlg('" + item.recid + "','" + item.yfze + "','" + item.sfze + "')\">详情</span> " + ffwcbutton + fjbutton + "  </td></tr>");

            });
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test2"));
}