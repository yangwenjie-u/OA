function setDwbh() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("用户系统单位编号设置"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=1" +
            "&LX=A" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '用户系统单位编号设置',
            shadeClose: false,
            shade: 0.8,
            area: ['400px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function setXmzz() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;


        var qybh = encodeURIComponent(selected.QYBH)   // 键值
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '项目资质设置',
            shadeClose: false,
            shade: 0.5,
            area: ['98%', '98%'],
            content: "/jc/jcdwxmzb?dwbh=" + qybh,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var ids = window.parent[layerObj.find('iframe')[0]['name']].getTreeValue();
                if (ids == "")
                    return;
                var arrIds = ids.split("|");
                $.ajax({
                    type: "POST",
                    url: "/jc/setjcdwxmzz",
                    dataType: "json",
                    data: "dwbh=" + encodeURIComponent(qybh) + "&zbbh=" + encodeURIComponent(arrIds[0]) + "&dt1=" + encodeURIComponent(arrIds[1]) + "&dt2=" + encodeURIComponent(arrIds[2]),
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("设置项目资质失败，详细信息：" + data.msg);
                        else {
                            alert("设置项目资质成功");
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
            }
        });
    } catch (e) {
        alert(e);
    }
}
function initXm() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var confirmValue = false;
        parent.layer.confirm('初始化项目会重新生成检测机构的项目分类和试验项目，不影响单位的指标设置，确定要初始化项目吗？', {
            btn: ['确定', '取消'], //按钮
            shade: 0.5
        }, function () {
            $.ajax({
                type: "POST",
                url: "/jc/initxm?dwbh=" + selected.QYBH,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.code != 0)
                        alert("初始化失败，详细信息：" + data.msg);
                    else {
                        alert("初始化成功");
                    }

                },
                complete: function (XMLHttpRequest, textStatus) {
                    parent.layer.closeAll();
                },
                beforeSend: function (XMLHttpRequest) {
                    parent.layer.msg('正在初始化……', { icon: 16 });
                }
            });
        }, function () {
            layer.closeAll();
        });

    } catch (e) {
        alert(e);
    }
}
function syncXm() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var confirmValue = false;
        parent.layer.confirm('更新新项目会更新新加的项目分类和项目，不会影响原有项目设置，确定要更新项目吗？', {
            btn: ['确定', '取消'], //按钮
            shade: 0.5
        }, function () {
            $.ajax({
                type: "POST",
                url: "/jc/syncxm?dwbh=" + selected.QYBH,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.code != 0)
                        alert("更新新项目失败，详细信息：" + data.msg);
                    else {
                        alert("更新新项目成功");
                    }

                },
                complete: function (XMLHttpRequest, textStatus) {
                    parent.layer.closeAll();
                },
                beforeSend: function (XMLHttpRequest) {
                    parent.layer.msg('正在更新新项目……', { icon: 16 });
                }
            });
        }, function () {
            parent.layer.closeAll();
        });

    } catch (e) {
        alert(e);
    }
}

function setDwxm() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var qybh = encodeURIComponent(selected.QYBH)   // 键值
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '项目禁用启用',
            shadeClose: false,
            shade: 0.5,
            area: ['500px', '98%'],
            content: "/jc/dwxmsz?dwbh=" + qybh,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var ids = window.parent[layerObj.find('iframe')[0]['name']].getTreeValue();
                if (ids == "")
                    return;
                $.ajax({
                    type: "POST",
                    url: "/jc/setjcdwxm?ids=" + encodeURIComponent(ids),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("项目设置失败，详细信息：" + data.msg);
                        else {
                            alert("项目设置成功");
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
            }
        });

    } catch (e) {
        alert(e);
    }
}
function setCsmy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_QY"); 			// 表名
        var tablerecid = encodeURIComponent("QYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.QYBH)   // 键值
        var title = encodeURIComponent("检测机构数据传输密钥设置"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=1" +
            "&LX=B" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '检测机构数据传输密钥设置',
            shadeClose: false,
            shade: 0.8,
            area: ['500px', '300px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function viewRy() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/WebList/EasyUiIndex?FormDm=QYGL_RYBA&FormStatus=1&FormParam=PARAM--"+selected.QYBH;

        parent.layer.open({
            type: 2,
            title: '检测机构人员查看',
            shadeClose: true,
            shade: 0.8,
            area: ['98%', '98%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}