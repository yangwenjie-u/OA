var g_accounts = [];
var g_accountmain = {};
var g_candomoneytrans = false;

$(function () {
    initEdhb();
    setAccountInfo();
    setSubAccounts();
    setListData("");
    initSearch();
});

$(".tab-title").on("click", "li", function() {
    if ($(this).attr("id") == "tab2" || $(this).attr("id") == "tab3") {
        setListData("");
    }
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
$("#btnExport").on("click", function () {
    getTable('/pay/GetSubAccounts', {
        gcbh: $("#project1").val(),
        qybh: $("#company1").val(),
        pagesize: 1000,
        pageindex: 1
    }, function (data) {
        if (data.code == 0) {
            var dataobj = {
                PayInfo: data.records
            };
            var jsonobj = {
                Template: "moneytransmain",
                Title: "工程子账户",
                Wheres: "",
                Datas: dataobj,
                OpenType: "filedown",
                SubPath: "pay"
            };
            var params = {
                key: JSON.stringify(jsonobj)
            }
            dowFile(g_printurl, params);
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    });

});
function setAccountInfo() {
    $.ajax({
        type: "POST",
        url: "/pay/GetAccountInfo",
        dataType: "json",
        success: function(data) {
            if (data.code == 0) {
                if (data.records.length > 0) {
                    g_accountmain = data.records[0];
                    if (g_accountmain.yhzh == "") {
                        //$("#money1").text("");
                        $("#zhxm").text("未创建");
                    } else {
                        $("#money1").text(g_accountmain.zhye);
                        $("#zhmc").text(g_accountmain.yhzh);
                        $("#zhxm").text(g_accountmain.yhzhmc);
                    }
                    

                }
            } else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        },
        complete: function(XMLHttpRequest, textStatus) {},
        beforeSend: function(XMLHttpRequest) {}
    });

}

function setSubAccounts() {

    getTable('/pay/GetSubAccounts', {
        gcbh: $("#project1").val(),
        qybh: $("#company1").val(),
        pagesize: 10,
        pageindex: 1
    }, function(data) {
        if (data.code == 0) {
            g_accounts = data.records;
            if (g_accounts != null && g_accounts.length > 0) {

                $("#tbody1").html("");
                var submoney = 0;
                $.each(g_accounts, function(i, item) {
                    submoney += item.zhye * 1;
                    var downbutton = "<span class=\"h-btn\" onclick=\"downDetail('" + item.zhid + "')\">导出划拨记录</span>";
                    var edhbbutton = "";
                    if (g_candomoneytrans)
                        edhbbutton = "<span class=\"h-btn\" onclick=\"addEd('" + item.zhid + "')\">增加额度</span><span class=\"h-btn\" onclick=\"minEd('" + item.zhid + "')\">额度回拨</span>";
                    $("#tbody1").append("<tr><td>" + item.zhmc + "</td><td>" + item.sgqymc + "</td><td>" + item.yhzh + "</td><td class=\"warning\">" + item.zhye + "元</td><td class=\"curd\">" + edhbbutton + " <span class=\"h-btn\" onclick=\"viewDetail('" + item.zhid + "','" + item.gcbh + "','" + item.sgqybh + "')\">划拨记录</span><span class=\"h-btn\" onclick=\"viewDetail2('" + item.zhid + "')\">充值记录</span> " + downbutton + "</td></tr>");

                });
                $("#money2").text(submoney.toFixed(2));
            }
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test1"));
}

function addEd(zhid) {
    $("#tbEd").val("");
    $("#tbEdbz").val("额度划拨");
    $("#errEd").text("");
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
        yes: function(index) {
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

            // getTable("/pay/domoneyout","tozhid="+encodeURIComponent(zhid)+"&ed="+ed+
            //     "&bz="+encodeURIComponent($("#tbEdbz").val()),function(data){
            //         if (data.code == 0) {
            //             layer.closeAll();
            //             setAccountInfo();
            //             layer.alert("划拨成功",{ 
            //                 title: '提示', 
            //                 icon: 1 
            //             });
            //         }
            //         else
            //             layer.open({ title: '提示', content: data.msg, icon: 5 });
            // },$('#test2'));

            $.ajax({
                type: "POST",
                url: "/pay/domoneyout",
                data: "tozhid=" + encodeURIComponent(zhid) + "&ed=" + ed + "&bz=" + encodeURIComponent($("#tbEdbz").val()),
                dataType: "json",
                success: function(data) {
                    if (data.code == 0) {
                        layer.closeAll();
                        setAccountInfo();
                        setSubAccounts();
                        layer.alert("划拨成功", {
                            title: '提示',
                            icon: 1
                        });
                    } else {
                        layer.closeAll();
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                    }
                },
                complete: function(XMLHttpRequest, textStatus) {},
                beforeSend: function(XMLHttpRequest) {}
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
        yes: function(index) {
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
                success: function(data) {
                    if (data.code == 0) {
                        layer.closeAll();
                        setAccountInfo();
                        setSubAccounts();
                        layer.alert("回拨成功", {
                            title: '提示',
                            icon: 1
                        });
                    } else {
                        layer.closeAll();
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                    }
                },
                complete: function(XMLHttpRequest, textStatus) {},
                beforeSend: function(XMLHttpRequest) {}
            });
        }
    });
}

function viewDetail(zhid) {
    $("#tab1").removeClass("active");
    $("#tab2").addClass("active");
    $("#tab3").removeClass("active");
    $('.tab-item').children().eq(0).hide();
    $('.tab-item').children().eq(1).show();
    $('.tab-item').children().eq(2).hide();
    // $("#table1").hide();
    // $("#table2").show();
    // $("#tab2").click();
    setListData(zhid);
}
function viewDetail2(zhid) {
    $("#tab1").removeClass("active");
    $("#tab3").addClass("active");
    $("#tab2").removeClass("active");
    $('.tab-item').children().eq(0).hide();
    $('.tab-item').children().eq(2).show();
    $('.tab-item').children().eq(1).hide();
    // $("#table1").hide();
    // $("#table2").show();
    // $("#tab2").click();
    setListData2(zhid);
}

function setListData(zhid) {
    // var params = "?hbzh=" + encodeURIComponent(zhid) + "&pagesize=1000&pageindex=1";
    getTable("/pay/GetMoneyTransList", {
        hbzh: zhid,
        pagesize: 10,
        pageindex: 1
    }, function(data) {
        if (data.code == 0) {
            $("#tbody2").html("");
            $.each(data.records, function(i, item) {
                var hblx = item.zfqyzh != g_accountmain.zhid ? "+" : "-";
                $("#tbody2").append("<tr><td>" + item.hbsj + "</td><td>" + item.gcmc + "</td><td>" + item.sgqymc + "</td><td class=\"warning\">" + hblx + item.hbje + "元</td><td>" + item.mark + " </td></tr>");

            });
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test2"));

}
function setListData2(zhid) {
    // var params = "?hbzh=" + encodeURIComponent(zhid) + "&pagesize=1000&pageindex=1";
    $("#tbody3").html("");
    getTable("/pay/GetRechargeList", {
        hbzh: zhid,
        pagesize: 100,
        pageindex: 1
    }, function (data) {
        if (data.code == 0) {
            
            $.each(data.records, function (i, item) {
                $("#tbody3").append("<tr><td>" + item.TradeTime + "</td><td>" + item.PayName + "</td><td>" + item.PayAcc + "</td><td class=\"warning\">" + item.Amount + "元</td><td class=\"warning\">" + item.AccountBalance + "元</td><td>" + item.TradeComment + " </td></tr>");

            });
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    }, $("#test3"));

}

function initSearch() {
    $.ajax({
        type: "POST",
        url: "/pay/GetRelateProjects",
        dataType: "json",
        success: function(data) {
            if (data.code == 0) {
                $("#project1").empty();
                $("#project1").append("<option value=''>==不限==</option>");
                $("#company1").empty();
                $("#company1").append("<option value=''>==不限==</option>");
                $.each(data.records, function(i, item) {
                    $("#project1").append("<option value='" + item.gcbh + "'>" + item.gcmc + "</option>");
                });
                layForm.render();
            } else
                layer.open({ title: '提示', content: data.msg, icon: 5 });
        }
    });
    $("#project1").on("change", function() {
        var gcbh = $("#project1").val();
        $("#company1").empty();
        $("#company1").append("<option value=''>==不限==</option>");
        if (gcbh != "") {
            $.ajax({
                type: "POST",
                url: "/pay/GetRelateCompanys",
                dataType: "json",
                data: "gcbh=" + encodeURIComponent(gcbh),
                success: function(data) {
                    if (data.code == 0) {
                        $.each(data.records, function(i, item) {
                            $("#company1").append("<option value='" + item.qybh + "'>" + item.qymc + "</option>");
                        });
                        layForm.render();
                        // layui.use('form', function() {
                        //     var form = layui.form;
                        //     form.render();
                        // });
                    } else
                        layer.open({ title: '提示', content: data.msg, icon: 5 });
                }
            });
        }
    });
    $("#btnSearch1").on("click", function() {
        setSubAccounts();
    });
    $("#btnSearch2").on("click", function() {
        setListData();
    });
}


function downDetail(zhid) {
    getTable("/pay/GetMoneyTransList", {
        hbzh: zhid,
        pagesize: 10,
        pageindex: 1
    }, function (data) {
        if (data.code == 0) {
            $.each(data.records, function (i, item) {
                var hblx = item.zfqyzh != g_accountmain.zhid ? "+" : "-";
            });
            var dataobj = {
                PayInfo: data.records
            };
            var jsonobj = {
                Template: "moneytransdetail",
                Title: "划拨详情",
                Wheres: "",
                Datas: dataobj,
                OpenType: "filedown",
                SubPath: "pay"
            };
            var params = {
                key: JSON.stringify(jsonobj)
            }
            dowFile(g_printurl, params);
        } else
            layer.open({ title: '提示', content: data.msg, icon: 5 });
    });
}