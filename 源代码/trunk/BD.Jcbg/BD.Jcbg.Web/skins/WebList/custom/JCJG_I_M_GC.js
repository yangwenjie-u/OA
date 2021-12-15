function add() {
    try {
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("监理单位|施工单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("监理人员|施工人员");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
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
            shadeClose: true,
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
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("监理单位|施工单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("监理人员|施工人员");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
            "&LX=N"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
            shadeClose: true,
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
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.GCBH)   // 键值

        var s_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_SJDW|I_S_GC_JZRY|I_S_GC_SYRY|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,RECID|GCBH,RECID");
        var s_title = encodeURIComponent("监理单位|施工单位|设计单位|见证人员|送样人员|分工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("监理人员|施工人员");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
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
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var tabledesc = "工程类型";                // 表格描述

        var selected = pubselect();
        if (selected == undefined)
            return;

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
function setPos() {
    try{
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
            shadeClose: true,
            shade: 0.5,
            area: ['100%', '100%'],
            content: "/jc/map?title=" + encodeURIComponent(gcmc)+"&pos="+pos,
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
            end:function(){
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
            shadeClose: true,
            shade: 0.5,
            area: ['600px', '100%'],
            content: "/jc/wtzssz?gcbh="+encodeURIComponent(gcbh),
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
            shadeClose: true,
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

function FormatWtl(row, datafield, value) {
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

function FormatWcwtl(row, datafield, value) {
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


var g_stations = [];
function getFromPlatform() {
    $.ajax({
        type: "POST",
        url: "/ycba/GetStations",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length == 0) {
                alert("请配置正确的调用服务器");
            } else {
                g_stations = data;
                if (data.length > 0) {
                    var contents = "<center>";
                    $.each(data, function (i, item) {
                        contents += "<button style='width:380px;color: #fff;background-color: #f0ad4e;border-color: #eea236;padding:6px 12px;display:inline-block;font-size:14px;line-height:20px;margin-top:10px;border: 1px solid transparent; border-radius: 4px;' onclick=\"showPlatformDiv(" + i + ")\">" + item.StationName + "</button><br>";
                    });
                    contents += "<center>";
                    layer.open({
                        type: 1,
                        title: "请选择平台",
                        area: ['420px', '300px'], //宽高
                        content: contents
                    });
                }
                if (data.length == 1) {
                    showPlatformDiv(0);
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });

}

function showPlatformDiv(index) {
    layer.closeAll();
    var station = g_stations[index];
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/ycba/apipagelist?callid=ApiPageProjectList&version=" + encodeURIComponent(station.VersionNo) + "&url=" + encodeURIComponent(station.RootUrl),
        end: function () {
            searchRecord();
        }
    });
}