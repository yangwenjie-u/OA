function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService.js,irisService.js");

        var dllname = "BD.JC.JS.SUser.dll";
        var dllclass = "BD.JC.JS.SUser.UserService";

        var callback = encodeURIComponent("userService.createUser('u','$$RYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&preproc=InputCheckImry|$i_m_ry.SFZHM" +
            //"&dllname=" + dllname +
            //"&dllclass=" + dllclass +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入人员信息',
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

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService.js,irisService.js");
        var callback = encodeURIComponent("userService.createUser('u','$$RYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&preproc=InputCheckImry|$i_m_ry.SFZHM" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制人员信息',
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

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("idcardService.js,irisService.js,userService.js");
	var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&js=" + js +
	    "&callback=" + callback +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改人员信息',
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
            url: "/delete/deleteiry?rybh=" + encodeURIComponent(selected.RYBH),
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
function addRyzz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;


        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_RY_RYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("人员资质信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_RY_RYZZ,RYBH," + selected.RYBH)
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
            title: '录入人员资质信息',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });
    } catch (e) {
        alert(e);
    }

}
function viewRyzz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/WebList/EasyUiIndex?FormDm=I_S_RY_RYZS&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(selected.RYBH);

        parent.layer.open({
            type: 2,
            title: '人员资质列表',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });
    } catch (e) {
        alert(e);
    }

}

function addUser() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        if (selected.YHZH != "") {
            alert("账号已创建，不能重复创建");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/remoteuser/adduser?usertype=u&usercode=" + encodeURIComponent(selected.RYBH),
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
function dowIris() {
    try {
        var tabledesc = "人员";

        var selected = pubselect();
        if (selected == undefined)
            return;


        if (selected.SY_HM == "无") {
            alert("当前人员没有登记虹膜，无法下发");
            return;
        }

        if (selected.QYMC == "") {
            alert("当前人员不属于企业，无法下发");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/kqj/downryiris?rybh=" + encodeURIComponent(selected.RYBH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("下发操作成功，等待考勤机下载！下发人员需要重启考勤机才能生效，请在考勤机界面重启考勤机。");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "下发失败";
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
function FormatZssl(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;

        if (ivalue > 0)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='" + ivalue + "'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='" + ivalue + "'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function FormatYw(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='" + value + "'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='" + value + "'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function FormatZp(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/ry/getryzp/" + row.RYBH + "' title=''/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = ""; // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        var s_pri = encodeURIComponent("RYBH,RYBH");
        var s_title = encodeURIComponent("人员证书");

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
            title: '查看人员信息',
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
        content: "/ycba/apipagelist?callid=ApiPageRyList&version=" + encodeURIComponent(station.VersionNo) + "&url=" + encodeURIComponent(station.RootUrl),
        end: function () {
            searchRecord();
        }
    });
}

function addWithZz() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService.js,irisService.js");

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("RYBH,RECID");
        var s_title = encodeURIComponent("人员证书");


        var dllname = "BD.JC.JS.SUser.dll";
        var dllclass = "BD.JC.JS.SUser.UserService";

        var callback = encodeURIComponent("userService.createUser('u','$$RYBH$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&preproc=InputCheckImry|$i_m_ry.SFZHM" +
            //"&dllname=" + dllname +
            //"&dllclass=" + dllclass +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入人员信息',
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
function copyWithZz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService.js,irisService.js");
        var callback = encodeURIComponent("userService.createUser('u','$$RYBH$$')");

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("RYBH,RECID");
        var s_title = encodeURIComponent("人员证书");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&preproc=InputCheckImry|$i_m_ry.SFZHM" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制人员信息',
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
function editWithZz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("idcardService.js,irisService.js,userService.js");
        var callback = encodeURIComponent("userService.updateUserRole('$$RYBH$$')");

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("RYBH,RECID");
        var s_title = encodeURIComponent("人员证书");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&js=" + js +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
	    "&callback=" + callback +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改人员信息',
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