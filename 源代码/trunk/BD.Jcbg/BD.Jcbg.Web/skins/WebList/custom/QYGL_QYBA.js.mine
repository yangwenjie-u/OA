function add() {
    try{
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
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
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t2_checknum=1" +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N"+
            "&_=" + rdm;

        var content = '<iframe id="frame3d" name="frame3d" frameborder="0" width="100%" scrolling="auto" style="margin-top:-4px;" onload="this.style.height=document.body.clientHeight-84" height="100%" scr="' + url + '"><iframe>';

        parent.layer.open({
            type: 2,
            title: '录入企业信息',
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
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t2_checknum=1" +
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
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t2_checknum=1" +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改企业信息',
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
function del(){
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
            shadeClose: false,
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


        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_QY_QYZZ"); 			// 表名
        var tablerecid = encodeURIComponent("ZZBH"); 	// 表主键
        var title = encodeURIComponent("企业资质信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_QY_QYZZ,QYBH,"+selected.QYBH)
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
            shadeClose: false,
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

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
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
        shadeClose: false,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/modifyqyphone?&qybh=' + encodeURIComponent(qybh) + "&lxrxm=" + encodeURIComponent(lxrxm) + "&sjhm=" + encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}


// 直接修改企业联系人信息，不需要验证手机号码
function zjmodifyphone() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var qybh = selected.QYBH;
    var lxrxm = selected.QYFZR;
    var sjhm = selected.LXSJ;
    parent.layer.open({
        type: 2,
        title: '直接修改企业联系人信息',
        shadeClose: false,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/zjmodifyqyphone?&qybh=' + encodeURIComponent(qybh) + "&lxrxm=" + encodeURIComponent(lxrxm) + "&sjhm=" + encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}