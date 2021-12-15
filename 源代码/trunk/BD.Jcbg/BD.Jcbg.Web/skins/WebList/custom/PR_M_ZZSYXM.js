function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_ZZSYXM"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("检测资质项目"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";

        var callback = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入检测资质项目',
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

        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_ZZSYXM"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("检测资质项目"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";
        var callback = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + jydbh +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
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
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("PR_M_ZZSYXM"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("检测资质项目"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";

        var callback = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback +
            "&jydbh=" + jydbh +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改检测资质项目',
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

function setZzsyxm() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var recid = selected.RECID;
    parent.layer.open({
        type: 2,
        title: '资质项目设置',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/jc/jczzsyxmsz?recid=" + recid,
        btn: ["保存", "关闭"],
        yes:function(index, layero){
            var zzxms = window.parent[layero.find('iframe')[0]['name']].getJczzsyxm();
            if (zzxms == "")
                return;
            parent.layer.closeAll();
            $.ajax({
                type: "POST",
                url: "/jc/savejczzsyxm",
                dataType: "json",
                data: "id=" + recid + "&values=" + encodeURIComponent(zzxms),
                async: false,
                success: function (data) {
                    if (data.code == "0") {
                        alert("设置成功！");
                        searchRecord();
                    } else {
                        if (data.msg == "")
                            data.msg = "设置失败";
                        alert(data.msg);
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        },
        end: function () {
        }
    });
}

function getDateString(value, row, index) {
    var ret = "";
    try {
        ttDate = value.match(/\d{4}.\d{1,2}.\d{1,2}/mg).toString();
        ttDate = ttDate.replace(/[^0-9]/mg, '-');
        if (ttDate.indexOf("1900") == -1)
            ret = ttDate;
    } catch (e) {
        ret = value;
    }
    return ret;
}
function formatYesNo(value, row, index) {
    var imgurl = "";
    try {
        if (value == "True")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
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
            url: "/delete/deletezzsyxm?id=" + encodeURIComponent(selected.RECID),
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

    } catch (ex) {
        alert(ex);
    }
}