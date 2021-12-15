function inputIris() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var iris = CallCSharpMethodByResult('Iris', '1.0.0.16');
        var ret = eval('(' + iris + ')');
        if (ret.success) {
            var iristext = ret.sIrisLeftBig + ret.sIrisRightBig;

            $.ajax({
                type: "POST",
                url: "/user/saveinneruseriris",
                data:"usercode=" + encodeURIComponent(selected.usercode)+"&iris="+iristext,
                dataType: "json",
                async: false,
                success: function (data) {
                    try {
                        if (data.code == "0") {
                            searchRecord();
                            alert("注册成功！");
                        }

                        else {
                            if (data.msg != "")
                                alert(data.msg);
                        }
                    } catch (e) {
                        alert(e);
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
    } catch (ex) {
        alert(ex);
    }
    
    console.log(iris);
}


function add() {
    try {

        parent.layer.open({
            type: 2,
            title: "添加考勤",
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/user/addkqjuser",
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}



function extrin() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        $.ajax({
            type: "POST",
            url: "/dwgxwz/extrin?usercode=" + encodeURIComponent(selected.usercode),
            dataType: "json",
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
    } catch (e) {
        alert(e);
    }
}


function extrout() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        $.ajax({
            type: "POST",
            url: "/dwgxwz/extrout?usercode=" + encodeURIComponent(selected.usercode),
            dataType: "json",
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
    } catch (e) {
        alert(e);
    }
}

function setCYKQ() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var needkq = selected.cykq;
        var usercode = selected.usercode;
        parent.layer.open({
            type: 2,
            title: "设置参与考勤",
            shadeClose: false,
            shade: 0.8,
            area: ['400px', '200px'],
            content: "/dwgxwz/setNeedKQ?needkq=" + encodeURIComponent(needkq) + "&usercode=" + encodeURIComponent(usercode),
            btn: ["确定", "关闭"],
            end: function () {
            },
            success: function (layero, index) {
                layerObj = layero;
            },
            yes: function (index) {
                var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                if (obj == "") {
                    alert("");
                } else {
                    parent.layer.closeAll();

                    $.ajax({
                        type: "POST",
                        url: "/dwgxwz/DoSetNeedKQ?usercode=" + encodeURIComponent(usercode) + "&needkq=" + encodeURIComponent(obj),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            
                            if (data.code == "0") {
                                parent.layer.closeAll();
                                parent.layer.alert('设置成功');
                                searchRecord();
                            } else {
                                if (data.msg == "")
                                    data.msg = "设置失败";
                                parent.layer.alert(data.msg); 
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                    
                }
                

            },
            btn2: function (index) {
                parent.layer.closeAll();
            }
        });
    } catch (e) {
        alert(e);
    }
}