$(function () {
    loadPayHistory("","","","","","","", $("#zt1").val());
    initSearch();
});
$(".tab-title").on("click", "li", function () {
    if ($(this).attr("id") == "tab2") {
        showPayDetail("", "");
    }
});
$("#btnExport").on("click", function () {
    var gcbh = $("#project1").val();
    var sgdwbh = $("#company1").val();
    var gzzq1 = $("#paymonth1_1").val();
    var gzzq2 = $("#paymonth1_2").val();
    var spsj1 = $("#checkday1_1").val();
    var spsj2 = $("#checkday1_2").val()
    var bz1 = $("#bz1").val();
    var zt = $("#zt1").val();
    getTable('/pay/GetPayHistory', {
        pagesize: -1, pageindex: 1, gcbh: gcbh, sgdwbh: sgdwbh, gzzq1: gzzq1, gzzq2: gzzq2, spsj1: spsj1, spsj2: spsj2, bz1: bz1,zt:zt
    }, function (data) {
        if (data.code == 0) {
            var dataobj = {
                PayInfo: data.records
            };
            var jsonobj = {
                Template: "billmain",
                Title: "发放记录",
                Wheres: "",
                Datas: dataobj,
                OpenType: "filedown",
                SubPath: "pay"
            };
            var params = {
                key: JSON.stringify(jsonobj)
            }
            /*
            $("#print_frame").contents().find("#key").val(JSON.stringify(jsonobj));
            $("#print_frame").contents().find("#printform").attr("action", g_printurl);
            //$("#print_frame").contents().find("#printform").submit();
            $("#print_frame")[0].contentWindow.submitForm();*/
            /*
            var frame = document.getElementById('print_frame');
            frame.contentWindow.postMessage(JSON.stringify(jsonobj), '*');

            //iframe页面
            iframe: window.addEventListener('message', function (event) {
                //此处执行事件
                //console.log("parent.message");
            })*/
            dowFile(g_printurl, params);
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    });
});
function loadPayHistory(gcbh,sgdwbh,gzzq1,gzzq2,spsj1,spsj2,bz1,zt) {
    getTable('/pay/GetPayHistory', {
        pagesize: 10, pageindex: 1, gcbh: gcbh, sgdwbh: sgdwbh, gzzq1: gzzq1, gzzq2: gzzq2, spsj1: spsj1, spsj2: spsj2, bz1: bz1,zt:zt
    }, function (data) {
        if (data.code == 0) {
            $("#tbody1").html("");
            var items = data.records;
            $.each(items, function (i, item) {
                var btnClass = "h-btn";
                if (item.ztms == "提现异常" || item.ztms == "异常" || item.ztms == "预提现失败")
                    btnClass = "h-btn-exception";
                var fjbutton = "<span class=\"h-btn\" onclick=\"showAttachDiv('" + item.recid + "')\">附件</span>";
                var downbutton = "<span class=\"h-btn\" onclick=\"downDetail('" + item.recid + "','"+item.zt+"')\">导出详情</span></a>";
                var downbankback = "";
                if (item.zt == 3 || item.zt == 4) {
                    downbankback = "<span class=\"h-btn\" onclick=\"downBankDetail('" + item.recid + "')\">导出回单</span>";
                } else {
                    downbankback = "<span class=\"h-btn\" style='background-color:#606060'>导出回单</span>";
                }
                var subbz1 = item.bz1;
                if (subbz1.length > 6)
                    subbz1 = subbz1.substr(0, 6);
                if (item.shsj != "") {
                    item.shsj = item.shsj.substr(0, item.shsj.indexOf(" "));
                }
                //
                //<td title='" + item.qymc + "'>" + item.qymc + "</td>
                $("#tbody1").append("<tr><td>" + item.zhye + "</td><td>" + item.ffnf + '-' + item.ffyf + "</td><td title='" + item.gcmc + "'>" + item.gcmc + "</td><td title='" + item.bz1 + "'>" + subbz1 + "</td><td>"+item.rysl+"</td><td>" + item.yfze + "</td><td>" + item.sfze + "</td><td title='" + item.shsj + "'>" + item.shsj + "</td><td title='" + item.ztms + "'>" + item.ztms + "</td><td title='" + item.bz2 + "'>" + item.bz2 + "</td><td title='"+item.ffbz1+"'>"+item.ffbz1+"</td><td> <span class=\"" + btnClass + "\" onclick=\"showPayDetail('" + item.recid + "','" + item.zt + "')\">详情</span>" + fjbutton + downbutton + downbankback + " </td></tr>");

            });
            $("#tbody1").append("<tr><td colspan='4' class='layui-bg-red'>总工程数：" + data.totalcount + "</td><td colspan='4' class='layui-bg-red'>应发总额：" + data.t1.toLocaleString() + "</td><td colspan='4' class='layui-bg-red'>实发总额：" + data.t2.toLocaleString() + "</td></tr>");
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test1"));
}

var g_payid = "";
function showPayDetail(recid, zt) {
    $("#tab1").removeClass("active");
    $("#tab2").addClass("active");
    $('.tab-item').children().eq(0).hide();
    $('.tab-item').children().eq(1).show();
    g_payid = recid;
    g_payzt = zt;
    loadPayDetail(g_payid,zt);
    
}

function loadPayDetail(payid,payzt) {
    getTable('/pay/getpaydetail', {
        recid: payid
    }, function (data) {
        if (data.code == 0) {
            $("#tbody2").html("");
            $.each(data.records, function (i, item) {
                var kkjd = "";
                if (item.bkzt == "1")
                    kkjd += "绑卡完成";
                else
                    kkjd = item.kkcwxx;

                var ffjd = "";
                if (item.zt == "3")
                    ffjd = "已取消";
                else {
                    if (item.sfje * 1 < item.yfje * 1)
                        ffjd += "待发";
                    else
                        ffjd = "已发";
                }
                var sfzhm = item.sfzhm;
                var jskh = item.jskh;
                if (!(payzt == "4" || payzt == "8") || item.sfje * 1 >= item.yfje * 1) {
                    if (sfzhm != null && sfzhm.length>8)
                        sfzhm = sfzhm.substr(0, 4) + "****" + sfzhm.substr(sfzhm.length - 4, 4);
                    if (jskh != null && jskh.length>8)
                        jskh = jskh.substr(0, 4) + "****" + jskh.substr(jskh.length - 4, 4);
                }
                var infoClass = "";
                if (item.sfje * 1 < item.yfje * 1 && item.zt != "3")
                    infoClass = "info-exception";
                var btn = "";
                if (payzt == "4" || payzt == "8" || payzt == "6") {
                    if (item.zt == "0")
                        btn = "<span class=\"h-btn\" onclick=\"showModifyCardnoDlg('" + item.id + "','" + payid + "','" + payzt + "')\">修改卡号</span><span class=\"h-btn\" onclick=\"showCancelPayDlg('" + item.id + "','" + payid + "','" + payzt + "','"+item.orderid+"')\">取消发放</span>";
                    if (item.bkzt == "1" && item.zt == "0")
                        btn += "<span class=\"h-btn\" onclick=\"showResubmitDlg('" + item.id + "','" + payid + "','" + payzt + "')\">重新提交</span>";
                }
                $("#tbody2").append("<tr class='" + infoClass + "'><td>"+(i+1)+"</td><td>" + item.ryxm + "</td><td>" + item.sjhm + "</td><td title='"+sfzhm+"'>" + sfzhm + "</td><td title='"+jskh+"'>" + jskh + "</td><td>" + item.yfje + "</td><td>" + item.sfje + "</td><td title='"+item.bz+"'>" + item.bz + "</td><td title='" + kkjd + "'>" + kkjd + "</td><td>" + ffjd + " </td><td title='" + item.cwxx + "'>" + item.cwxx + "</td><td>" + btn + "</td></tr>");

            });
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test2"));
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
			    layForm.render();
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
        loadPayHistory($("#project1").val(), $("#company1").val(), $("#paymonth1_1").val(), $("#paymonth1_2").val(), $("#checkday1_1").val(), $("#checkday1_2").val(), $("#bz1").val(), $("#zt1").val());
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

function downDetail(payid,payzt) {
    getTable('/pay/getpaydetail', {
        recid: payid
    }, function (data) {
        if (data.code == 0) {
            $.each(data.records, function (i, item) {
                if (item.bkzt == "1")
                    item["kkjd"] = "绑卡完成";
                else
                    item["kkjd"] = item.kkcwxx;

                if (item.sfje * 1 < item.yfje * 1)
                    item["ffjd"] = "待发";
                else
                    item["ffjd"] = "已发";
                if (item.cwxx == "0")
                    item.cwxx = "";
                var sfzhm = item.sfzhm;
                var jskh = item.jskh;
                if (!(payzt == "4" || payzt == "8") || item.sfje * 1 >= item.yfje * 1) {
                    if (sfzhm != null && sfzhm.length>8)
                        sfzhm = sfzhm.substr(0, 4) + "****" + sfzhm.substr(sfzhm.length - 4, 4);
                    item.sfzhm = sfzhm;
                    if (jskh != null && jskh.length>8)
                        jskh = jskh.substr(0, 4) + "****" + jskh.substr(jskh.length - 4, 4);
                    item.jskh = jskh;
                }
            });

            var dataobj = {
                PayInfo: data.records
            };
            var jsonobj = {
                Template: "billdetail",
                Title: "发放详情",
                Wheres: "",
                Datas: dataobj,
                OpenType: "filedown",
                SubPath: "pay"
            };
            var params = {
                key: JSON.stringify(jsonobj)
            }
            /*
            $("#print_frame").contents().find("#key").val(JSON.stringify(jsonobj));
            $("#print_frame").contents().find("#printform").attr("action", g_printurl);
            //$("#print_frame").contents().find("#printform").submit();
            $("#print_frame")[0].contentWindow.submitForm();*/
            /*
            var frame = document.getElementById('print_frame');
            frame.contentWindow.postMessage(JSON.stringify(jsonobj), '*');

            //iframe页面
            iframe: window.addEventListener('message', function (event) {
                //此处执行事件
                //console.log("parent.message");
            })*/
            dowFile(g_printurl, params);
        }
        else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    });
}

function downBankDetail(payid) {
    
    download("/pay/GetPayBankBack", "payid=" + encodeURIComponent(payid), "post");
    /*
    $.fileDownload("/pay/GetPayBankBack", {
        httpMethod: 'POST',
        data: "payid=" + encodeURIComponent(payid),
        prepareCallback: function (url) {
            //             common.layer.msg("下载开始，请稍等！");
            layer.load(0, { shade: false });
        },
        successCallback: function (url) {
            layer.closeAll();
            
        },
        failCallback: function (html, url) {
            layer.closeAll();
        }
    });*/
}

function showModifyCardnoDlg(recid, payid,payzt) {
    $("#tbCardNo").val("");
    $("#errEd").text("");
    layer.open({
        type: 1,
        title: '修改卡号',
        skin: 'layui-layer-lan',
        shadeClose: true,
        shade: 0.8,
        area: ['320px', '200px'],
        content: $("#divModifyCard"),
        btn: ["确定"],
        anim: 4,
        yes: function (index) {
            $("#errEd").text("");
            var cardno = $("#tbCardNo").val();
            if (cardno == "") {
                $("#errEd").text("请输入卡号");
                return;
            }
            var dlgIndex = layer.load(1, { shade: [0.6, '#eee'] });
            $.ajax({
                type: "POST",
                url: "/pay/changepersoncard",
                dataType: "json",
                data: "recid=" + encodeURIComponent(recid)+"&cardno="+cardno,
                success: function (data) {
                    layer.close(dlgIndex);
                    if (data.code == 0) {
                        loadPayDetail(payid,payzt);
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                }
            });
            
        }
    });
}


function showResubmitDlg(recid, payid, payzt) {
    layer.confirm('您确定重新提交发放这条记录吗？', {
        btn: ['同意', '再想想'] //按钮
    }, function () {
        layer.closeAll();
        var dlgIndex = layer.load(1, { shade: [0.6, '#eee'] });
        $.ajax({
            type: "POST",
            //url: "/pay/doresubmitpayitem",
            url: "/pay/DoResubmitPayItem",
            dataType: "json",
            data: "recid=" + encodeURIComponent(recid),
            success: function (data) {
                layer.close(dlgIndex);
                if (data.code == 0) {
                    loadPayDetail(payid, payzt);
                }
                else
                    layer.open({ title: '提示', content: data.msg, icon: 5 });
            }
        });
    });
}

function showCancelPayDlg(recid, payid, payzt, orderid) {
    layer.confirm('您确定要取消发放吗？', {
        btn: ['同意', '再想想'] //按钮
    }, function () {
        layer.closeAll();
        var dlgIndex = layer.load(1, { shade: [0.6, '#eee'] });
        if (recid != "") {
            $.ajax({
                type: "POST",
                //url: "/pay/doresubmitpayitem",
                url: "/pay/DoCancelPayItem",
                dataType: "json",
                data: "recid=" + encodeURIComponent(recid),
                success: function (data) {
                    layer.close(dlgIndex);
                    if (data.code == 0) {
                        loadPayDetail(payid, payzt);
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                }
            });
        } else if (orderid != "") {
            $.ajax({
                type: "POST",
                //url: "/pay/doresubmitpayitem",
                url: "/pay/DoCancelBankPayItem",
                dataType: "json",
                data: "payid="+payid+"&orderid=" + encodeURIComponent(orderid),
                success: function (data) {
                    layer.close(dlgIndex);
                    if (data.code == 0) {
                        loadPayDetail(payid, payzt);
                    }
                    else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                }
            });
        }
        else {
            layer.close(dlgIndex);
            layer.open({ title: '提示', content: "无法取消，请联系管理员", icon: 5 });
        }
    });
}