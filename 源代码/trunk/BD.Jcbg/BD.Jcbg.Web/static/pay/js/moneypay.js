$(function () {
    initSearch();
    loadData();
    setSubAccounts();

    $("#btnGotoStep1").on("click", function () {
        goback();
    });
    initUploadFileCtrl();
    $("#btnSaveApply").on("click", function () {
        saveApply();
    });
    $("#btnGotoStep1_1").on("click", function () {
        goback();
    });
    $("#btnExportError").on("click", function () {
        checkAndExportError();
    });
    
});

var g_zhyes = [];

function loadData() {   


    $("#selLx").empty();
    $.ajax({
        type: "POST",
        url: "/pay/getfflx",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.code == 0) {
                $.each(data.records, function (i, item) {
                    $("#selLx").append("<option value='"+item.lxbh+"'>"+item.lxmc+"</option>");
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

function setSubAccounts() {
    getTable('/pay/GetSubAccounts', {
        gcbh: $("#project1").val(),
        qybh: $("#company1").val(),
        pagesize: 10,
        pageindex: 1
    }, function (data) {
        if (data.code == 0) {
            $("#tbody_pay1").html("");
            g_zhyes = data.records;
            $.each(g_zhyes, function (i, item) {
                var ffje = item.ffje;
                if (ffje == "")
                    ffje = "0";
                $("#tbody_pay1").append("<tr><td>" + item.zhmc + "</td><td>" + item.sgqymc + "</td><td>" + item.zhid + "</td><td>" + item.zhye + "</td><td>" + item.ffcs + "</td><td>" + ffje + "</td><td>" + item.spje + "</td><td>" + item.ffsj + "</td><td> <span class=\"h-btn\" onclick=\"showPayDlg('" + item.zhid + "')\">发工资</span> <span class=\"h-btn\" onclick=\"showHistoryDlg('" + item.zhid + "')\">查看记录</span> </td></tr>");

            });
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test1"));

}
function goback() {
    g_payItem = null;
    g_ryinfos = null;
    g_payItem = null;
    $("#tbody_pay2").html("");
    $("#div_pay1").show();
    $("#div_pay2").hide();
    $("#div_pay_detail1").hide();
}

var g_payItem = null;

function showPayDlg(yhyhid) {
    try {
        $.ajax({
            type: "POST",
            url: "/pay/DoRemovePayListSession",
            dataType: "json",
            async: false
        });

        $("#file_upload_params").val(yhyhid);
        $("#div_pay1").hide();
        $("#div_pay2").show();

        var finds = g_zhyes.filter((p) => {
            return p.zhid == yhyhid;
        })
        if (finds.length == 0)
            return;
        g_payItem = finds[0];
        $("#spgc").text(g_payItem.zhmc);
        $("#spsgdw").text(g_payItem.sgqymc);
        $("#spzh").text(g_payItem.zhid);

        $("#spye").text(g_payItem.zhye);
    }
    catch (err) {
        layer.open({ title: '提示', content: err, icon: 5 });
    }
}
var g_history_yhyhid = "";
function showHistoryDlg(yhyhid) {
    try {
        $("#div_pay1").hide();
        $("#div_pay_detail1").show();
        g_history_yhyhid = yhyhid;
        loadHistoryData(g_history_yhyhid, "", "", "", "");
    }
    catch (err) {
        layer.open({ title: '提示', content: err, icon: 5 });
    }

}

function loadHistoryData(yhyhid, gzzq1, gzzq2, spsj1, spsj2) {
    try{
        getTable('/pay/GetPayHistory', {
            gcmc: '', sgdw: '', ffzh: yhyhid, zt: "", pagesize: 10, pageindex: 1, gcbh: "", sgdwbh: "", gzzq1: gzzq1, gzzq2: gzzq2, spsj1: spsj1, spsj2: spsj2
        }, function (data) {
            if (data.code == 0) {
                $("#tbody_pay_detail1").html("");
                g_gcitems = data.records;
                $.each(g_gcitems, function (i, item) {
                    
                    var ffwcbutton = "";
                    var btnClass = "h-btn";
                    if (item.ztms == "提现异常" || item.ztms == "异常" ||  item.ztms=="预提现失败")
                        btnClass = "h-btn-exception";
                    var fjbutton = "<span class=\"h-btn\" onclick=\"showAttachDiv('" + item.recid + "')\">附件</span>";
                    $("#tbody_pay_detail1").append("<tr><td>" + item.zhye + "</td><td>" + item.ffnf + '-' + item.ffyf + "</td><td>" + item.lxmc + "</td><td title='" + item.gcmc + "'>" + item.gcmc + "</td><td title='" + item.qymc + "'>" + item.qymc + "</td><td title='" + item.bz1 + "'>" + item.bz1 + "</td><td>" + item.rysl + "</td><td>" + item.yfze + "</td><td>" + item.sfze + "</td><td title='" + item.shsj + "'>" + item.shsj + "</td><td title='" + item.ztms + "'>" + item.ztms + "</td><td title='" + item.bz2 + "'>" + item.bz2 + "</td><td> <span class=\"" + btnClass + "\" onclick=\"showPayDetailDlg('" + item.recid + "')\">详情</span> " + ffwcbutton + fjbutton + "  </td></tr>");

                });
            }
            else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        }, $("#test2"));
    }
    catch (err) {
        layer.open({ title: '提示', content: err, icon: 5 });
    }
}
var g_payid = "";
function showPayDetailDlg(recid) {
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
    loadPayDetailData(g_payid, "");
    
}

function loadPayDetailData(payid, xm) {
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
                if (item.sfje * 1 < item.yfje * 1)
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
                $("#tbody3").append("<tr class='" + infoClass + "'><td>"+(i+1)+"</td><td>" + item.ryxm + "</td><td>" + item.sjhm + "</td><td>" + sfzhm + "</td><td>" + jskh + "</td><td>" + item.yfje + "</td><td>" + item.sfje + "</td><td>" + item.bz + "</td><td title='" + kkjd + "'>" + kkjd + "</td><td>" + ffjd + " </td><td title='" + item.cwxx + "'>" + item.cwxx + "</td></tr>");

            });

        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test4"));
}

var g_ryinfos = [];
function initUploadFileCtrl() {
    layui.use('upload', function () {
        var upload = layui.upload;

        //执行实例
        var uploadInst = upload.render({
            elem: '#btnUpGzc', //绑定元素
            url: '/pay/douploadpaylist', //上传接口
            before: function () {
                layer.load(1, {
                    shade: [0.6, '#eee'] //0.1透明度的白色背景
                });
            },
            done: function (res) {
                layer.closeAll();
                g_ryinfos = [];
                try {
                    if (res.code != "0") {
                        layer.open({ title: '提示', content: "文件上传错误，详细信息：" + res.msg, icon: 5 });
                    } else {
                        $("#tbody_pay2").html("");
                        g_ryinfos = res.records;
                        $.each(res.records, function (i, item) {
                            var infoClass = "";
                            if (item.sfyx == "2")
                                infoClass = "info-exception";
                            else if (item.sfyx == "3")
                                infoClass = "info-error";
                            $("#tbody_pay2").append("<tr class='" + infoClass + "'><td>" + (i + 1) + "</td><td>" + item["姓名"] + "</td><td>" + item["电话"] + "</td><td>" + item["身份证号码"] + "</td><td>" + item["银行卡号"] + "</td><td>" + item["实发工资"] + "</td><td>" + item["备注"] + "</td><td title='"+item["errmsg"]+"'>" + item["errmsg"] + "</td></tr>");
                            
                        });
                        $("#tbody_pay2").append("<tr><td colspan='5' style='text-align:right'>合计：</td><td class='warning'>" + getTotalPay() + "</td><td colspan='2'></td></tr>");

                    }
                } catch (err) {
                    layer.open({ title: '提示', content: "处理返回结果异常，详细信息：" + err, icon: 5 });
                }
            },
            error: function () {
              //请求异常回调
              layer.open({ title: '提示', content: "文件上传异常", icon: 5 });
            },
            data: {
              key: function () {
                  return $('#file_upload_params').val();
              }
            },
            accept: "file",
            acceptMime: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            exts: "xlsx",
            auto: true,
            field: "paylist",
            size: 102400,
            multiple: false
        });
    });
}

function saveApply() {
    try{
        if (g_payItem == null) {
            layer.open({ title: '提示', content: "当前没有选择支付工程", icon: 5 });
            return;
        }
        if (g_ryinfos.length == 0) {
            layer.open({ title: '提示', content: "请导入工资册", icon: 5 });
            return;
        }
        var month = $("#tbPayMonth").val();
        if (month == "") {
            layer.open({ title: '提示', content: "请选择发放日期", icon: 5 });
            return;
        }
        var fflx = $("#selLx").val();
        if (fflx == "") {
            layer.open({ title: '提示', content: "请选择发放类型", icon: 5 });
            return;
        }
        /*
        var shouldPay = getTotalPay();
        if (g_payItem.zhye < shouldPay) {
            layer.open({ title: '提示', content: "账户余额不足，请往工程账户转入&nbsp;" + (shouldPay - g_payItem.zhye) + "&nbsp;元后再试", icon: 5 });
            return;
        }*/
        layer.load(1, {
            shade: [0.6, '#eee'] //0.1透明度的白色背景
        });
        $.ajax({
            type: "POST",
            url: "/pay/DoSavePayListApply",
            dataType: "json",
            data: "yhyhid=" + encodeURIComponent(g_payItem.zhid) + "&fflx=" + encodeURIComponent(fflx) + "&ffny=" + encodeURIComponent(month) + "&paylist=" + encodeURIComponent(JSON.stringify(g_ryinfos)) + "&bz1=" + encodeURIComponent($("#tbBz1").val()) + "&bz2=" + encodeURIComponent($("#tbBz2").val()),
            async: false,
            success: function (data) {
                layer.closeAll();
                if (data.code == 0) {
                    
                    loadData();
                    goback();
                    layer.alert("提交成功，请等待审批", {
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

    } catch (err) {
        layer.open({ title: '提示', content: err, icon: 5 });
    }
}

function getTotalPay() {
    var ret = 0;
    $.each(g_ryinfos, function (i, item) {
        ret += item["实发工资"] * 1;
    });
    return ret;
}


function initSearch() {
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
                $.each(data.records, function (i, item) {
                    $("#project1").append("<option value='" + item.gcbh + "'>" + item.gcmc + "</option>");
                });
				//layForm.render();
            }
            else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        }
    });
    $("#project1").on("change", function () {
        var gcbh = $("#project1").val();
        $("#company1").empty();
        $("#company1").append("<option value=''>==不限==</option>");
        if (gcbh != "") {
            $.ajax({
                type: "POST",
                url: "/pay/GetRelateCompanys",
                dataType: "json",
                data: "gcbh=" + encodeURIComponent(gcbh),
                success: function (data) {
                    if (data.code == 0) {
                        $.each(data.records, function (i, item) {
                            $("#company1").append("<option value='" + item.qybh + "'>" + item.qymc + "</option>");
                        });
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                }
            });
        }
    });
    $("#btnSearch1").on("click", function () {
        setSubAccounts();
    });
    $("#btnSearch2").on("click", function () {
        loadHistoryData(g_history_yhyhid, $("#paymonth2_1").val(), $("#paymonth2_2").val(), $("#checkday2_1").val(), $("#checkday2_2").val());
    });
    $("#btnSearch3").on("click", function () {
        loadPayDetailData(g_payid, $("#xm3").val());
    });
}


function showAttachDiv(recid) {
    try {

        var index = layer.open({
            title: "工资册附件查看",
            type: 2,
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/pay/payrollattach?id=" + recid
        });

    } catch (e) {
        alert(e);
    }
}

function checkAndExportError() {
    $.ajax({
        type: "POST",
        url: "/pay/GetPayListSessionExists",
        dataType: "json",
        success: function (data) {
            if (data.code == 0) {
                layer.load(1, {
                    shade: [0.6, '#eee'] //0.1透明度的白色背景
                });
                $.ajax({
                    type: "POST",
                    url: "/pay/GetPayListSession",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        layer.closeAll();
                        if (data.code == 0) {
                            var dataobj = {
                                PayInfo: data.records
                            };
                            var jsonobj = {
                                Template: "billerroritems",
                                Title: "工资册错误信息",
                                Wheres: "",
                                Datas: dataobj,
                                OpenType: "filedown",
                                SubPath: "pay"
                            };
                            var params = {
                                key: JSON.stringify(jsonobj)
                            }
                            dowFile(g_printurl, params);
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        layer.closeAll();
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });
            }
            else
                layer.open({ title: '提示', content: "没有导入工资册，无法导出", icon: 5 });
        }
    });
}