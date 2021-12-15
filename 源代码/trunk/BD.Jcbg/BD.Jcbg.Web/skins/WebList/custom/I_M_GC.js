function add() {
    try {
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|监理单位|施工单位|勘察单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员");

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
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
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

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|监理单位|施工单位|勘察单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员");

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


        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|监理单位|施工单位|勘察单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员");

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
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
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
function editSelf() {
    try {
        var tabledesc = "工程信息";

        var selected = pubselect();
        if (selected == undefined)
            return;


        $.ajax({
            type: "POST",
            url: "/user/getcuruserinfo",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    if (data.username != selected.LRRZH) {
                        alert("只能修改自己录入的工程");
                    } else {
                        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
                        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
                        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
                        var title = encodeURIComponent(tabledesc); 	// 标题
                        var formdm = tablename;                             // 列表key名称
                        var buttons = encodeURIComponent("保存|TJ| | ");
                        var jydbh = encodeURIComponent(selected.GCBH)   // 键值

                        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
                        //  都是从表中的字段：  主表对应字段,自己主键|……
                        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
                        var s_title = encodeURIComponent("建设单位|监理单位|施工单位|勘察单位|设计单位|见证人员|送样人员|分工程");

                        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
                        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
                        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
                        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员");

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
                            "&jydbh=" + jydbh +
                            "&LX=N" +
                            "&_=" + rdm;

                        parent.layer.open({
                            type: 2,
                            title: '修改' + tabledesc,
                            shadeClose: true,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: url,
                            end: function () {
                                searchRecord();
                            }
                        });
                    }

                } else {
                    if (data.msg == "")
                        data.msg = "获取信息失败，修改失败";
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
        var tabledesc = "工程";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;
        /*
        if (IsBjwc(selected.ZT)) {
            alert("报监完成的工程不允许删除");
            return;
        }*/
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteimgc?gcbh=" + encodeURIComponent(selected.GCBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
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

function delSelf() {
    try {
        var tabledesc = "工程";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;
        
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: "/user/getcuruserinfo",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    if (data.username != selected.LRRZH) {
                        alert("只能删除自己录入的工程");
                    } else {
                        $.ajax({
                            type: "POST",
                            url: "/delete/deleteimgc?gcbh=" + encodeURIComponent(selected.GCBH),
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                if (data.code == "0") {
                                    alert("删除成功！");
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
                    }

<<<<<<< .mine
        var rdm = Math.random();

        var js = encodeURIComponent("jdbgService.js");
        var callback = encodeURIComponent("jdbgService.jdzccksbwc('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=K" +
            "&stage=1" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报窗口审批',
            shadeClose: false,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
||||||| .r98
        var rdm = Math.random();

        var js = encodeURIComponent("jdbgService.js");
        var callback = encodeURIComponent("jdbgService.jdzccksbwc('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=K" +
            "&stage=1" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报窗口审批',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
=======
                } else {
                    if (data.msg == "")
                        data.msg = "获取信息失败，删除失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
>>>>>>> .r220
            }
        });

        
    } catch (e) {
        alert(e);
    }
}
<<<<<<< .mine
||||||| .r98
//（质监审批）
function completeZJBj() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值
=======

// 窗口报监
function completeCKBj() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值
>>>>>>> .r220

<<<<<<< .mine
||||||| .r98
        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent("工程信息"); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var rdm = Math.random();

        var js = encodeURIComponent("jdbgServiceZJAJ.js");
        var callback = encodeURIComponent("jdbgService.jdzczjsbwc('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=K" +
            "&stage=1" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报窗口审批',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
//（安监审批）
function completeAJBj() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
		if(selected.AJSPYX=="True")
		{
			alert("安监已审批通过，不能再次提交");
			return ;
		}
		
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值

        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent("工程信息"); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var rdm = Math.random();

        var js = encodeURIComponent("jdbgServiceZJAJ.js");
        var callback = encodeURIComponent("jdbgService.jdzcajsbwc('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=AJ" +
            "&stage=1" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报窗口审批',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
=======
        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent("工程信息"); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var rdm = Math.random();

        var js = encodeURIComponent("jdbgService.js");
        var callback = encodeURIComponent("jdbgService.jdzccksbwc('$$GCBH$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=K" +
            "&stage=1" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报窗口审批',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

>>>>>>> .r220
// 内部报监
function completeBj() {
    try {
        /*
        var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        if (rowindex == -1) {
            alert("请选择要操作的工程");
            return;
        }

        var selected = dataGrid.jqxGrid('getrowdata', rowindex);

        if (IsBjwc(selected.ZT)) {
            alert("该工程已经完成报监，无法重复报监");
            return;
        }
        var rdm = Math.random();
        var bcode = new Base64();
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh='" + selected.GCBH + "'"));
        var extrainfo2 = bcode.encode('[' + selected.ZJDJH + ']' + selected.GCMC);
        var extrainfo3 = bcode.encode(selected.GCBH);
        var url = "/workflow/startwork?processid=1" +
            // 加密的条件信息（表单中用到）
            "&extrainfo=" + encodeURIComponent(extrainfo1) +
            // 流程中显示的主体信息
            "&extrainfo2=" + encodeURIComponent(extrainfo2) +
            // 流程中用到的跟工程关联的主键
            "&extrainfo3=" + encodeURIComponent(extrainfo3) +
            // 流程中用到的分工程主键
            "&extrainfo4=" +
            "&callbackjs="+encodeURIComponent("parent.layer.closeAll();")+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });*/
        var selected = pubselect();
        if (selected == undefined)
            return;
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值

        // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent("工程信息"); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var rdm = Math.random();

        var js = encodeURIComponent("jdbgService.js");
        var callback = encodeURIComponent("jdbgService.jdzcsbwc('$$GCBH$$')");
        

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&jydbh=" + jydbh +
            "&LX=C" +
            "&stage=1" +
            "&preproc=InputCheckZJDJH|$i_m_gc.ZJDJH|$i_m_gc.SBSP" +
            "&_=" + rdm;
        parent.layer.open({
            type: 2,
            title: '工程申报内部审批',
            shadeClose: false,
            shade: 0.8,
            area: ['800px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

// 打印监督书
function printjds() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = "质量监督书V1";
        var gcbh = selected.GCBH;

        var url = "/jdbg/jdbgreport?reportfile=" + encodeURIComponent(reportfile) + "&gcbh=" + encodeURIComponent(gcbh) + "&reporttype=ZLJDS";

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

function setPos() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var gcbh = encodeURIComponent(selected.GCBH)   // 键值
        var gcmc = selected.GCMC   // 键值
        var pos = encodeURIComponent(selected.GCZB);
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '工程标注-' + gcmc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jc/map?title=" + encodeURIComponent(gcmc) + "&pos=" + pos,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var pos = window.parent[layerObj.find('iframe')[0]['name']].getPos();
                if (pos == "") {
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "/jc/setgcbz?gcbh=" + gcbh + "&pos=" + encodeURIComponent(pos),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("标注失败，详细信息：" + data.msg);
                        else {
                            alert("标注成功");
                            parent.layer.closeAll();
                        }

                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });
            },
            success: function (layero, index) {
                layerObj = layero;
            },
            btn2: function (index) {
                parent.layer.closeAll();
            },
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function setWtzs() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        var gcbh = encodeURIComponent(selected.GCBH)   // 键值
        var gcmc = selected.GCMC   // 键值
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '必检项目设置-' + gcmc,
            shadeClose: false,
            shade: 0.5,
            area: ['600px', '100%'],
            content: "/jc/wtzssz?gcbh=" + encodeURIComponent(gcbh),
            btn: ["保存", "关闭"],
            yes: function (index) {
                var values = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                $.ajax({
                    type: "POST",
                    url: "/jc/setgcwtzs?gcbh=" + encodeURIComponent(gcbh) + "&wtzs=" + encodeURIComponent(values),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("保存失败，详细信息：" + data.msg);
                        else {
                            alert("保存成功");
                            parent.layer.closeAll();
                        }

                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });
            },
            success: function (layero, index) {
                layerObj = layero;
            },
            btn2: function (index) {
                parent.layer.closeAll();
            },
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function viewWtzs() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        var gcbh = encodeURIComponent(selected.GCBH)   // 键值
        var gcmc = selected.GCMC   // 键值
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '送检情况-' + gcmc,
            shadeClose: false,
            shade: 0.5,
            area: ['600px', '100%'],
            content: "/jc/wtzsck?gcbh=" + encodeURIComponent(gcbh),
            btn: ["关闭"],
            success: function (layero, index) {
                layerObj = layero;
            },
            btn2: function (index) {
                parent.layer.closeAll();
            },
            end: function () {
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

        //var s_tablename = encodeURIComponent("I_M_GC_JWD|I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW|I_S_GC_FGC");
        ////  都是从表中的字段：  主表对应字段,自己主键|……
        //var s_pri = encodeURIComponent("GCBH,Recid|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID");
        //var s_title = encodeURIComponent("工程经纬度|建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位|分工程");

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|监理单位|施工单位|勘察单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员");

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
function FormatWtl(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;

        if (value > 0)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已设置'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未设置'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function FormatWcwtl(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;
        var szsl = row.WTL * 1;

        if (value == 0) {
            if (szsl > 0)
                imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已完成'/></center>";
            else
                imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未设置'/></center>";
        }
        else
            imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='未完成'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function IsBjwc(zt) {
    return zt != "" && zt != "LR" && zt != 'YT';
}
function FormatBjwc(value, row, index) {
    var imgurl = "";
    try {
        var bjwc = IsBjwc(value);

        if (bjwc)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已完成'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未完成'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function FormatGczb(value, row, index) {
    var imgurl = "";
    try {


        if (value != "")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已标注'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未标注'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function viewzj() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 0;
    var zllx = 'zj';
    var params = {
        gcbh: gcbh,
        gcmc: gcmc,
        gclxbh: gclxbh,
        isedit: isedit,
        zllx: zllx
    };

    var rdm = Math.random();
    var url = "/jdbg/uploadzl?gcbh=" + gcbh +
        "&gcmc=" + gcmc +
        "&gclxbh=" + gclxbh +
        "&isedit=" + isedit +
        "&zllx=" + zllx +
        "&_=" + rdm;

    parent.layer.open({
        type: 2,
        title: '查看质监资料',
        shadeClose: false,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });
}

function viewaj() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 0;
    var zllx = 'aj';
    var params = {
        gcbh: gcbh,
        gcmc: gcmc,
        gclxbh: gclxbh,
        isedit: isedit,
        zllx: zllx
    };

    var rdm = Math.random();
    var url = "/jdbg/uploadzl?gcbh=" + gcbh +
        "&gcmc=" + gcmc +
        "&gclxbh=" + gclxbh +
        "&isedit=" + isedit +
        "&zllx=" + zllx +
        "&_=" + rdm;

    parent.layer.open({
        type: 2,
        title: '查看安监资料',
        shadeClose: false,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });
}
/**
*
*  Base64 encode / decode
*
*  @author haitao.tu
*  @date   2010-04-26
*  @email  tuhaitao@foxmail.com
*
*/

function Base64() {

    // private property
    _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    // public method for encoding
    this.encode = function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;
        input = _utf8_encode(input);
        while (i < input.length) {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output +
                _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
                _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
        }
        return output;
    }

    // public method for decoding
    this.decode = function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;
        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
        while (i < input.length) {
            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));
            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;
            output = output + String.fromCharCode(chr1);
            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = _utf8_decode(output);
        return output;
    }

    // private method for UTF-8 encoding
    _utf8_encode = function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }
        return utftext;
    }

    // private method for UTF-8 decoding
    _utf8_decode = function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;
        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
}