function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var s_tablename = encodeURIComponent("I_M_QY_JWD");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("QYBH,Recid");
        var s_title = encodeURIComponent("企业经纬度");
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.createUser('q','$$QYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        var content = '<iframe id="frame3d" name="frame3d" frameborder="0" width="100%" scrolling="auto" style="margin-top:-4px;" onload="this.style.height=document.body.clientHeight-84" height="100%" scr="' + url + '"><iframe>';

        parent.layer.open({
            type: 2,
            title: '录入企业信息',
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
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var s_tablename = encodeURIComponent("I_M_QY_JWD");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("QYBH,Recid");
        var s_title = encodeURIComponent("企业经纬度");
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.createUser('q','$$QYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制企业信息',
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
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var s_tablename = encodeURIComponent("I_M_QY_JWD");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("QYBH,Recid");
        var s_title = encodeURIComponent("企业经纬度");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改企业信息',
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
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteiqy?qybh=" + encodeURIComponent(selected.QYBH),
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
function viewZz() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;


        var url = "/WebList/EasyUiIndex?FormDm=QYGL_ZZGL&FormStatus=0&MenuCode=QYGL_ZZGL&FormParam=PARAM--" + encodeURIComponent(selected.QYBH);

        parent.layer.open({
            type: 2,
            title: '企业资质列表',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });
    } catch (e) {
        alert(e);
    }
}
function addZz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;


        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_S_QY_QYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZZBH"); 	// 表主键
        var title = encodeURIComponent("企业资质信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_QY_QYZZ,QYBH," + selected.QYBH)
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&fieldparam=" + fieldparam +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入企业资质信息',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });
    } catch (e) {
        alert(e);
    }
}

function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC2"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("企业信息"); 	// 标题
        var buttons = ""; // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_QY_QYZZ");
        var s_pri = encodeURIComponent("QYBH,QYBH");
        var s_title = encodeURIComponent("企业资质");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&view=true" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看企业信息',
            shadeClose: true,
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


function modifyphone() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var qybh = selected.QYBH;
    var lxrxm = selected.QYFZR;
    var sjhm = selected.LXSJ;
    parent.layer.open({
        type: 2,
        title: '修改企业联系人信息',
        shadeClose: true,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/modifyqyphone?&qybh=' + encodeURIComponent(qybh) + "&lxrxm=" + encodeURIComponent(lxrxm) + "&sjhm=" + encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}

function FormatQyzz(value, row, index) {
    var imgurl = "";
    try {
        if (value != "")
            imgurl += "<a onclick='showZzDiv(\"" + row.QYBH + "\")' style='cursor:pointer;color:#ff9900;' alt='查看资质文件'>" + value + "</a>";
        else
            imgurl = value;
    } catch (e) {
    }
    return imgurl;
}


function showZzDiv(qybh) {
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/qy/qyzzck?qybh=" + qybh,
        end: function () {
            //searchRecord();
        }
    });
}

function viewZzwj() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    if (selected.SY_ZZMC == "") {
        parent.layer.alert("选择的企业还没录入资质");
    } else {
        showZzDiv(selected.QYBH);
    }
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
        content: "/ycba/apipagelist?callid=ApiPageQyList&version=" + encodeURIComponent(station.VersionNo) + "&url=" + encodeURIComponent(station.RootUrl),
        end: function () {
            searchRecord();
        }
    });
}


function addUser() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        if (selected.ZH != "") {
            alert("账号已创建，不能重复创建");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/remoteuser/adduser?usertype=q&usercode=" + encodeURIComponent(selected.QYBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                } else {
                    searchRecord();
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
