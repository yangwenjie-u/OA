function add() {
    try {
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH");
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位");


        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
<<<<<<< .mine
            title: '工程预填',
            shadeClose: false,
||||||| .r98
            title: tabledesc+'预填',
            shadeClose: true,
=======
            title: '工程预填',
            shadeClose: true,
>>>>>>> .r220
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function copy() {
    try {
        var tabledesc = "工程信息";
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.GCBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("保存|TJ| | ");
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID");
        var s_title = encodeURIComponent("勘察单位|设计单位|监理单位|施工单位|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("勘察人员|设计人员|监理人员|施工人员");

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&js=" + js +
            "&callback=" + callback +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit() {
    try {
        var tabledesc = "工程信息";

        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!canModify(selected.ZT)) {
            alert("已提交的信息不允许修改");
            return;
        }

        var jydbh = encodeURIComponent(selected.GCBH);
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH");
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位");


        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '预填修改',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function view() {
    try {
        var tabledesc = "工程信息";

        var selected = pubselect();
        if (selected == undefined)
            return;

        var jydbh = encodeURIComponent(selected.GCBH);
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = ""; // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位|单位工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY||I_S_GC_TSDW|I_S_GC_TSRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员|图审人员");

        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
            "&view=true"+
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var tabledesc = "工程";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;
        if (!confirm("确定要撤销申请吗？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: "/delete/deleteimgc?gcbh=" + encodeURIComponent(selected.GCBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("撤销成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "撤销失败";
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

function FormatZt(value, row, index) {
    var imgurl = "";
    try {

        if (value == "YT") {
            if (row.SPTG == "False" && row.SBSP == "True")
                imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未提交'/></center>";
            else
                imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='未通过'/></center>";
        } else if (row.ZT == "LR") {
            imgurl += "<center><img src='/skins/default/images/list/set_blue.png' title='审批中'/></center>";
        } else {
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已通过'/></center>";
        }
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function canModify(zt) {
    return zt == "YT";
}

function apply() {
    try {
        var tabledesc = "工程";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!canModify(selected.ZT)) {
            alert("不能重复提交！");
            return;
        }
        if (!confirm("确定要提交吗？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: "/qy/submitgc?gcbh=" + encodeURIComponent(selected.GCBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("提交成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "提交失败";
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

// 打印监督登记表
function printjddjb() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = "监督注册登记表V1";
        var gcbh = selected.GCBH;

        var url = "/jdbg/jdbgreport?reportfile=" + encodeURIComponent(reportfile) + "&gcbh=" + encodeURIComponent(gcbh) + "&reporttype=JDDJB";

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}