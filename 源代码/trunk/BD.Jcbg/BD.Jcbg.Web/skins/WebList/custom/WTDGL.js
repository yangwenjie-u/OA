function add() {
    
    try {
        parent.layer.open({
            type: 2,
            title: '试验项目选择',
            shadeClose: false,
            shade: 0.5,
            area: ['98%', '98%'],
            content: "/jc/syxmxz"
        });
            
        
    } catch (e) {
        alert(e);
    }
}
function copy() {
    try {
        var tabledesc = "委托单";  
        var selecteds = pubselects();
        if (selecteds.length > 1) {
            alert("当前选择了多份委托单，复制第一份选择的委托单");
        }
        var selected = selecteds[0];

        $.ajax({
            type: "POST",
            url: "/jc/setdwbh?dwbh="+encodeURIComponent(selected.YTDWBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == 0) {
                    $.ajax({
                        type: "POST",
                        url: "/jc/getwtdlrbj",
                        dataType: "json",
                        data: "qybh=" + encodeURIComponent(selected.YTDWBH) + "&syxmbh=" + encodeURIComponent(selected.SYXMBH),
                        async: false,
                        success: function (data) {
                            try {
                                if (data.code == 0) {
                                    var wtdlrbj = data.msg;
                                    var syxmbh = selected.SYXMBH;
                                    var syxmmc = selected.SYXMMC;
                                    var zdzdtable = encodeURIComponent("XTZD_BY,DWZD_" + syxmbh + ",ZDZD_" + syxmbh); // zdzd名
                                    var tablename = encodeURIComponent("M_BY,M_D_" + syxmbh + ",M_" + syxmbh); 			// 表名
                                    var tablerecid = encodeURIComponent("RECID"); 	// 表主键
                                    var title = encodeURIComponent(tabledesc); 	// 标题
                                    var formdm = tablename;
                                    var buttons = encodeURIComponent("提交|TJ| "); // 按钮
                                    var js = encodeURIComponent("wtdService.js");
                                    var callback = encodeURIComponent("wtdService.setSubmit('$$RECID$$')");
                                    var rdm = Math.random();

                                    var s_tablename = encodeURIComponent("S_BY,S_D_" + syxmbh + ",S_" + syxmbh);
                                    var s_pri = encodeURIComponent("BYZBRECID,RECID");
                                    var s_title = encodeURIComponent("记录");

                                    var dllname = "BD.JC.JS.Common.dll";
                                    var dllclass = "BD.JC.JS.Common.ComSetJcyj";

                                    var fieldparam = "M_BY,SYXMBH," + syxmbh;
                                    fieldparam = encodeURIComponent(fieldparam);

                                    var sufproc = encodeURIComponent("CheckJcWtdcb|$JYDBH$");

<<<<<<< .mine
                        parent.layer.open({
                            type: 2,
                            title: tabledesc+"复制",
                            shadeClose: false,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: url,
                            end: function () {
                                searchRecord();
||||||| .r98
                        parent.layer.open({
                            type: 2,
                            title: tabledesc+"复制",
                            shadeClose: true,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: url,
                            end: function () {
                                searchRecord();
=======
                                    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                                                     "&t1_tablename=" + tablename +
                                                     "&t1_pri=" + tablerecid +
                                                     "&t1_title=" + title +
                                                     "&button=" + buttons +
                                                     "&js=" + js +
                                                     "&callback=" + callback +
                                                     "&LX=W" +
                                                     "&rownum=2" +
                                                     "&t2_tablename=" + s_tablename +
                                                     "&fieldparam=" + fieldparam +
                                                     "&t2_pri=" + s_pri +
                                                     "&t2_title=" + s_title +
                                                     "&type=" + wtdlrbj +
                                                     "&_=" + rdm +
                                                     "&dllname=" + dllname +
                                                     "&dllclass=" + dllclass +
                                                     "&redefinebh=S_BY.ZH" +
                                                     "&cf=1" +
                                                     "&companycode=" + selected.YTDWBH +
                                                     "&sylbzdzd=" + syxmbh +
                                                     "&sufproc=" + sufproc +
                                                     "&individualZdzdtable=DATAZDZD_INDIVIDUAL" +
                                                     "&syxmbh=" + syxmbh +
                                                     "&copyjydbh=" + selected.RECID;

                                    parent.layer.open({
                                        type: 2,
                                        title: tabledesc + "复制",
                                        shadeClose: true,
                                        shade: 0.8,
                                        area: ['95%', '95%'],
                                        content: url,
                                        end: function () {
                                            searchRecord();
                                        }
                                    });
                                } else {
                                    alert(data.msg);
                                }
                            } catch (e) {
                                alert(e);
>>>>>>> .r220
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                } else {
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit() {
    try {
        var tabledesc = "委托单";
        var selecteds = pubselects();
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        var selected = undefined;
        $.each(selecteds, function (index, e) {
            var curitem = e;
            if (curitem.SY_XFBS == "否") {
                selected = curitem;
                return false;
            }
        });
        if (selected == undefined) {
            alert("送样后的委托单不允许修改");
            return;
        }
        if (selecteds.length > 1) {
            alert("当前选择了多份委托单，进入第一份未送样的委托单");
        }
        //var rowindex = rowindexes[0];
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        $.ajax({
            type: "POST",
            url: "/jc/setdwbh?dwbh="+encodeURIComponent(selected.YTDWBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == 0) {
                    $.ajax({
                        type: "POST",
                        url: "/jc/getwtdlrbj",
                        dataType: "json",
                        data: "qybh=" + encodeURIComponent(selected.YTDWBH) + "&syxmbh=" + encodeURIComponent(selected.SYXMBH),
                        async: false,
                        success: function (data) {
                            try {
                                if (data.code == 0) {
                                    var wtdlrbj = data.msg;
                                    var syxmbh = selected.SYXMBH;
                                    var syxmmc = selected.SYXMMC;
                                    var zdzdtable = encodeURIComponent("XTZD_BY,DWZD_" + syxmbh + ",ZDZD_" + syxmbh); // zdzd名
                                    var tablename = encodeURIComponent("M_BY,M_D_" + syxmbh + ",M_" + syxmbh); 			// 表名
                                    var tablerecid = encodeURIComponent("RECID"); 	// 表主键
                                    var title = encodeURIComponent(tabledesc); 	// 标题
                                    var formdm = tablename;
                                    var buttons = encodeURIComponent("提交|TJ| "); // 按钮
                                    var js = encodeURIComponent("wtdService.js");
                                    var callback = encodeURIComponent("wtdService.setSubmit('$$RECID$$')");
                                    var rdm = Math.random();

                                    var s_tablename = encodeURIComponent("S_BY,S_D_" + syxmbh + ",S_" + syxmbh);
                                    var s_pri = encodeURIComponent("BYZBRECID,RECID");
                                    var s_title = encodeURIComponent("记录");

                                    var dllname = "BD.JC.JS.Common.dll";
                                    var dllclass = "BD.JC.JS.Common.ComSetJcyj";
                                    var changeWtdbh = (selected.SY_DYBS == "是" || selected.SFXF == "True") ? ",M_BY.WTDBH" : "";

                                    var fieldparam = "M_BY,SYXMBH," + syxmbh;
                                    fieldparam = encodeURIComponent(fieldparam);

                                    var sufproc = encodeURIComponent("CheckJcWtdcb|$JYDBH$");

<<<<<<< .mine
                        parent.layer.open({
                            type: 2,
                            title: tabledesc + "修改",
                            shadeClose: false,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: url,
                            end: function () {
                                searchRecord();
||||||| .r98
                        parent.layer.open({
                            type: 2,
                            title: tabledesc + "修改",
                            shadeClose: true,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: url,
                            end: function () {
                                searchRecord();
=======
                                    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                                                     "&t1_tablename=" + tablename +
                                                     "&t1_pri=" + tablerecid +
                                                     "&t1_title=" + title +
                                                     "&button=" + buttons +
                                                     "&js=" + js +
                                                     "&callback=" + callback +
                                                     "&LX=W" +
                                                     "&rownum=2" +
                                                     "&t2_tablename=" + s_tablename +
                                                     "&fieldparam=" + fieldparam +
                                                     "&t2_pri=" + s_pri +
                                                     "&t2_title=" + s_title +
                                                     "&type=" + wtdlrbj +
                                                     "&_=" + rdm +
                                                     "&dllname=" + dllname +
                                                     "&dllclass=" + dllclass +
                                                     "&redefinebh=S_BY.ZH" + changeWtdbh +
                                                     "&cf=1" +
                                                     "&companycode=" + selected.YTDWBH +
                                                     "&sylbzdzd=" + syxmbh +
                                                     "&individualZdzdtable=DATAZDZD_INDIVIDUAL" +
                                                     "&syxmbh=" + syxmbh +
                                                     "&sufproc=" + sufproc +
                                                     "&jydbh=" + selected.RECID;

                                    parent.layer.open({
                                        type: 2,
                                        title: tabledesc + "修改",
                                        shadeClose: true,
                                        shade: 0.8,
                                        area: ['95%', '95%'],
                                        content: url,
                                        end: function () {
                                            searchRecord();
                                        }
                                    });
                                } else {
                                    alert(data.msg);
                                }
                            } catch (e) {
                                alert(e);
>>>>>>> .r220
                            }
                        }
                    });
                } else {
                    alert(data.msg);
                }

            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });

        
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var tabledesc = "委托单";

        var selecteds = pubselects();

        if (selecteds.length == 0) {
            return;
        }
        

        if (!confirm("确定要删除所选" + selecteds.length + "条记录吗？")) {
            return;
        }
        var recids = "";
        $.each(selecteds, function (index, e) {
            var selected = e;
            recids += selected.RECID + ",";
        });
        $.ajax({
            type: "POST",
            url: "/delete/deletewtd?recid=" + encodeURIComponent(recids),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    if (data.msg == "")
                        data.msg = "删除成功！"
                    alert(data.msg);
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}
function printWts() {
    try {        
        var tabledesc = "委托单";
        var selecteds = pubselects();

        if (selecteds.length == 0) {
            return;
        }
        var unsubmits = "";
        var notemplates = "";
        var printids = "";
        g_prints = [];
        $.each(selecteds, function (index, e) {
            var curitem = e;
            if (curitem.SY_TJBS == "否") {
                if (unsubmits != "")
                    unsubmits += "，";
                unsubmits = "[" + curitem.SYXMMC + "]" + curitem.WTDBH;
            }
            if (curitem.WTSMB == "") {
                if (notemplates != "")
                    notemplates += "，";
                notemplates = "[" + curitem.SYXMMC + "]" + curitem.WTDBH;
            } else {
                printids += curitem.RECID + ",";
                g_prints.push(curitem);
            }
        });
        var alertMsg = "";
        if (unsubmits != "") {
            alertMsg += "委托单" + unsubmits + "未提交。";
        }
        if (notemplates != "")
            alertMsg += "委托单" + notemplates + "没有打印模板。";
        if (alertMsg != "")
            alert(alertMsg += "不能打印！");

        g_curPrintIndex = 0;
        g_dlgClose = true;
        if (g_curPrintIndex >= g_prints.length)
            return;
        $.ajax({
            type: "POST",
            url: "/jc/setwtdprint?recid=" + encodeURIComponent(printids),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    showOnePrintDlg();
                } else {
                    if (data.msg == "")
                        data.msg = "设置委托单状态为打印失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
        
    } catch (e) {
        alert(e);
    }
}

var g_curPrintIndex = 0;
var g_prints = [];
var g_dlgClose = true;
function showOnePrintDlg() {
    try {
        if (!g_dlgClose) {
            setTimeout("showOnePrintDlg()", 1000);
        }
        else {
            if (g_curPrintIndex >= g_prints.length) {
                if (g_prints.length > 0)
                    searchRecord();
            }
            else {
                g_dlgClose = false;
                var curitem = g_prints[g_curPrintIndex];
                setTimeout("showOnePrintDlg()", 1000);
                parent.layer.open({
                    type: 2,
                    title: '打印委托单',
                    shadeClose: false,
                    shade: 0.8,
                    area: ['100%', '100%'],
                    content: "/jc/printwts?syxmbh=" + encodeURIComponent(curitem.SYXMBH) + "&recid=" + encodeURIComponent(curitem.RECID)+"&wtsmb="+encodeURIComponent(curitem.WTSMB),
                    end: function () {
                        g_dlgClose = true;
                    }
                });
                g_curPrintIndex++;
                
            }
        }
    } catch (ex) {
        alert(ex);
    }
}

function viewWts() {
    try {
        var tabledesc = "委托单";
        var selecteds = pubselects();

        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        if (selecteds.length > 1) {
            alert("当前选择了多份委托单，查看第一份委托单");
        }
        
        var selected = selecteds[0];
        /*
        if (selected.WTSMB == "") {
            alert("委托书[" + selected.SYXMMC + "]" + selected.WTDBH + "没有打印模板，查看失败。");
            return;
        }*/
        if (selected.WTSMB == "")
            selected.WTSMB = selected.SYXMBH;

        parent.layer.open({
            type: 2,
            title: '查看委托单',
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/jc/viewwts?syxmbh=" + encodeURIComponent(selected.SYXMBH) + "&recid=" + encodeURIComponent(selected.RECID)+"&wtsmb="+encodeURIComponent(selected.WTSMB),
            end: function () {

            }
        });
    } catch (e) {
        alert(e);
    }
}

function printBg() {
    try {
        var tabledesc = "委托单";
        var selecteds = pubselects();

        if (selecteds.length == 0) {
            return;
        }
        var arrIds = [];
        $.each(selecteds, function (index, e) {
            var selected = e;
            if (selected.SY_BGBS == "是")
                arrIds.push(selected.RECID);
        });
        if (arrIds.length == 0) {
            alert("请选择有报告的" + tabledesc);
            return;
        }
        if (arrIds.length > 1)
            alert("当前选择了多份有报告的"+tabledesc+"，查看第一份"+tabledesc+"的报告");
        var id = arrIds[0];

        parent.layer.open({
            type: 2,
            title: '查看报告',
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/jc/viewreport?wtdwyh=" + encodeURIComponent(id),
            end: function () {

            }
        });
    } catch (e) {
        alert(e);
    }
}

function FormatZt(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function batchPrintWts() {
    try {
        var tabledesc = "委托单";
        var selecteds = pubselects();

        if (selecteds.length == 0) {
            return;
        }
        var unsubmits = "";
        var notemplates = "";
        var printids = "";
        var printinfos = "";
        $.each(selecteds, function (index, e) {
            var curitem = e;
            if (curitem.SY_TJBS == "否") {
                if (unsubmits != "")
                    unsubmits += "，";
                unsubmits = "[" + curitem.SYXMMC + "]" + curitem.WTDBH;
            }
            if (curitem.WTSMB == "") {
                if (notemplates != "")
                    notemplates += "，";
                notemplates = "[" + curitem.SYXMMC + "]" + curitem.WTDBH;
            } else {
                printids += curitem.RECID + ",";
                printinfos += curitem.SYXMBH + "|" + curitem.RECID + "|" + curitem.WTSMB + "||";
            }
        });
        var alertMsg = "";
        if (unsubmits != "") {
            alertMsg += "委托单" + unsubmits + "未提交。";
        }
        if (notemplates != "")
            alertMsg += "委托单" + notemplates + "没有打印模板。";
        if (alertMsg != "")
            alert(alertMsg += "不能打印！");

        $.ajax({
            type: "POST",
            url: "/jc/setwtdprint?recid=" + encodeURIComponent(printids),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    parent.layer.open({
                        type: 2,
                        title: '打印委托单',
                        shadeClose: true,
                        shade: 0.8,
                        area: ['100%', '100%'],
                        content: "/jc/BatchPrintWts?reqparam=" + encodeURIComponent(printinfos),
                        end: function () {
                        }
                    });
                } else {
                    if (data.msg == "")
                        data.msg = "设置委托单状态为打印失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });

    } catch (e) {
        alert(e);
    }
}