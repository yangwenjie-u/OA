function check() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var layerObj = undefined;
    var usertype = "u";
    if (selected.LX == "企业")
        usertype = "q";
    parent.layer.open({
        type: 2,
        title: '账号申请审批',
        shadeClose: false,
        shade: 0.8,
        area: ['250px', '180px'],
        content: "/qy/qysqsp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
            var hasError = false;
            var hasCreateUser = false;
            // 审批同意，先尝试创建账号
            if (checkvalue == "1") {
                $.ajax({
                    type: "POST",
                    url: "/remoteuser/adduser?usertype=" + usertype + "&usercode=" + encodeURIComponent(selected.ID),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.msg != "") {
                            alert(data.msg);
                            hasError = true;
                        } else
                            hasCreateUser = true;
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });
            }
            // 创建账号成功后，写入审批内容
            if (!hasError) {
                if (usertype == "q") {
                    $.ajax({
                        type: "POST",
                        url: "/qy/setqysqsp?usercode=" + encodeURIComponent(selected.ID) + "&checkoption=" + encodeURIComponent(checkvalue),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.code != 0)
                                alert("审批失败，详细信息：" + data.msg);
                            else {
                                var msg = "审批成功";
                                if (hasCreateUser)
                                    msg += "。账号已创建，账号名称：" + selected.ZH + "。可在企业备案管理中查看";
                                alert(msg);
                                parent.layer.closeAll();
                            }

                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                } else {
                    $.ajax({
                        type: "POST",
                        url: "/ry/setrysqsp?usercode=" + encodeURIComponent(selected.ID) + "&checkoption=" + encodeURIComponent(checkvalue),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.code != 0)
                                alert("审批失败，详细信息：" + data.msg);
                            else {
                                var msg = "审批成功";
                                if (hasCreateUser)
                                    msg += "。账号已创建，账号名称：" + selected.ZH + "。可在人员备案管理中查看";
                                alert(msg);
                                parent.layer.closeAll();
                            }

                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                }
            }
            
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
}
function view() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var url = "";
    var title = "";
    if (selected.LX == "企业") {
        url = "/datainput/Index?zdzdtable=ZDZD_JC" +
        "&t1_tablename=I_M_QY" +
        "&t1_pri=QYBH" +
        "&t1_title=" + encodeURIComponent("企业信息") +
        "&button=" + 
        "&rownum=2" +
        "&view=true" +
        "&jydbh="+selected.ID+
        "&LX=N";
        title = "企业信息";
    } else {
        url = "/datainput/Index?zdzdtable=ZDZD_JC" +
        "&t1_tablename=I_M_RY" +
        "&t1_pri=RYBH" +
        "&t1_title=" + encodeURIComponent("人员信息") +
        "&button=" + 
        "&rownum=2" +
        "&view=true" +
        "&jydbh=" + selected.ID +
        "&LX=N";
        title = "人员信息";
    }
    if (url != "")
    {
        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    }
}