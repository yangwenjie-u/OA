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
            shadeClose: false,
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
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
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

function addaccount() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    if (selected.ZH == null || selected.ZH=='') {
        var rybh = selected.RYBH;
        var fieldparam = "I_M_RY,ZLYZ,1";
        var js = encodeURIComponent("agreement.js,customfunImry2.js,userService.js,smsService.js");
        var callback = encodeURIComponent("userService.createUserWithSmsAndDefaultRoles('u','$$RYBH$$')");
        var url = "/datainput/Index?zdzdtable=ZDZD_JC" +
        "&t1_tablename=I_M_RY" +
        "&t1_pri=RYBH" +
        "&t1_title=" + encodeURIComponent("人员基本信息") +
        "&button=" + encodeURIComponent("提交申请|TJ| | | ") +
        "&custombutton=" + encodeURIComponent("查看用户协议|AGREEMENT|showAgreement() ") +
        "&rownum=1" +
        "&fieldparam=" + fieldparam +
        "&js=" + js +
        "&callback=" + callback +
        "&jydbh=" + rybh +
        "&LX=R";

        parent.layer.open({
            type: 2,
            title: '人员账号注册申请',
            shadeClose: false,
            shade: 0.8,
            area: ['1000px', '500px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    }
    else {
        parent.layer.open({
            type: 0,
            title: '提示',
            content: '当前人员已有账号，不能重复添加！'
        });
    }

}

function modifyphone() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var rybh = selected.RYBH;
    var ryxm = selected.RYXM;
    var sjhm = selected.SJHM;
    parent.layer.open({
        type: 2,
        title: '修改手机号码',
        shadeClose: false,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/modifyuserphone?&rybh=' + encodeURIComponent(rybh) + "&ryxm=" + encodeURIComponent(ryxm) + "&sjhm="+ encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}

// 直接修改人员手机号码
function zjmodifyphone() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var rybh = selected.RYBH;
    var ryxm = selected.RYXM;
    var sjhm = selected.SJHM;
    parent.layer.open({
        type: 2,
        title: '直接修改手机号码',
        shadeClose: false,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/zjmodifyuserphone?&rybh=' + encodeURIComponent(rybh) + "&ryxm=" + encodeURIComponent(ryxm) + "&sjhm=" + encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}

function mergegc() {
    var selecteds = pubselects();
    if (selecteds == undefined || selecteds.length==0)
        return;
    var rybhlist = [];
    var ryxmlist = [];
    var sfzhlist = [];
    var gcgwlist = [];
    $.each(selecteds, function (inddex, row) {
        rybhlist.push(row.RYBH);
        ryxmlist.push(row.RYXM);
        sfzhlist.push(row.SFZHM);
        gcgwlist.push(row.GCGW);

    }); 
    if (ryxmlist.unique().length > 1) {
        parent.layer.open({
            type: 0,
            title: "提示",
            content: '选择的人员中有部分人员的姓名不一样，请重新选择！'
        });
        return;
    }

    if (sfzhlist.unique().length > 1) {
        parent.layer.open({
            type: 0,
            title: "提示",
            content: '选择的人员中有部分人员的身份证号码不一样，请重新选择！'
        });
        return;
    }
    parent.layer.open({
        type: 2,
        title: '选择需要保留的人员',
        shadeClose: false,
        shade: 0.8,
        area: ['1000px', '500px'],
        content: '/jdbg/reservedrylist?&rybhlist=' + encodeURIComponent(rybhlist.join(',')),
        end: function () {
            searchRecord();
        }
    });

}

function splitgc() {
    var selected = pubselect();
    if (selected == undefined || selected.length == 0)
        return;
    parent.layer.open({
        type: 2,
        title: '选择需要拆分的工程',
        shadeClose: false,
        shade: 0.8,
        area: ['1000px', '500px'],
        content: '/jdbg/splitedgclist?&rybh=' + encodeURIComponent(selected.RYBH) + "&ryxm=" + encodeURIComponent(selected.RYXM),
        end: function () {
            searchRecord();
        }
    });
}
Array.prototype.unique = function () {
    var newArr = [];
    for (var i = 0; i < this.length; i++) {
        if (newArr.indexOf(this[i]) == -1) {
            newArr.push(this[i]);
        }
    }
    return newArr;
}

