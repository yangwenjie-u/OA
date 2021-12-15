function add() {
    try {
        var tabledesc = "人员信息";
        var selected = pubselect();
        if (selected == undefined)
            return;
        var tablename = getTableName(selected.QYLXBH);
        if (tablename == "") {
            alert(selected.QYLXMC+"不需要录入人员");
            return;
        }

        if (parseInt(selected.RYSL) >= 1 && tablename != "I_S_GC_TSRY") {
            alert('只能录入一个人员!');
            return;
        }

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var buttons = encodeURIComponent("保存|TJ| | ");
        var rdm = Math.random();
        var fieldparam = tablename + ",GCBH," + selected.GCBH + "|" + tablename + ",QYBH," + selected.GCQYID;
        fieldparam = encodeURIComponent(fieldparam);

        var js = encodeURIComponent("jdbgService.js");
        var callback = encodeURIComponent("jdbgService.updateUserRole('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&js=" + js +
            "&callback=" + callback +
            "&button=" + buttons +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
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

function viewGC() {
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
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位|分工程");

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
            "&view=true" +
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

function view() {
    try {
        var tabledesc = "人员信息";
        var selected = pubselect();
        if (selected == undefined)
            return;
        

        var url = "/WebList/EasyUiIndex?FormDm=QYGL_GCRYCK&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(selected.GCQYID)
        url += "|" + encodeURIComponent(selected.GCBH);

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

function getTableName(qylx) {
    var tablename = "";
    if (qylx == "13")
        tablename = "I_S_GC_JSRY";
    else if (qylx == "12")
        tablename = "I_S_GC_JLRY";
    else if (qylx == "11")
        tablename = "I_S_GC_SGRY";
    else if (qylx == "14")
        tablename = "I_S_GC_SJRY";
    else if (qylx == "15")
        tablename = "I_S_GC_KCRY";
    else if (qylx == "21")
        tablename = "I_S_GC_TSRY";
    return tablename;
}


function add2() {
    try {
        var tabledesc = "人员信息";
        var selected = pubselect();
        if (selected == undefined)
            return;
        var tablename = getTableName(selected.QYLXBH);
        if (tablename == "") {
            alert(selected.QYLXMC+"不需要录入人员");
            return;
        }
/*
        if (parseInt(selected.RYSL) >= 1 && tablename != "I_S_GC_TSRY") {
            alert('只能录入一个人员!');
            return;
        }
*/
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var buttons = encodeURIComponent("保存|TJ| | ");
        var rdm = Math.random();
        var fieldparam = tablename + ",GCBH," + selected.GCBH + "|" + tablename + ",QYBH," + selected.GCQYID;
        fieldparam = encodeURIComponent(fieldparam);

        var js = encodeURIComponent("ryService.js");
        var callback = encodeURIComponent("ryService.rydr('$$GCBH$$','" + tablename + "')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&js=" + js +
            "&callback=" + callback +
            "&button=" + buttons +
            "&rownum=2" +
            "&fieldparam=" + fieldparam +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                //searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


