function JudgeKJ() {
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        return "easyui";
    } else {
        return "jqx";
    }
}

function pubselect() {
    try {
        if (app) {
            if (app.selected.length == 0) {
                alert('请先择需要操作的记录！');
                return;
            }
            return app.selected[0]
        }
    } catch (error) { }
    if (dataGrid && dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var selected = dataGrid.datagrid('getSelected');
        if (!selected) {
            alert('请先择需要操作的记录！');
            return;
        }
        return selected;
    } else {
        var rowindex = dataGrid.jqxGrid('getselectedrowindex');
        if (rowindex == -1) {
            alert('请先择需要操作的记录！');
            return;
        }
        var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        return selected;
    }
}


function pubselects() {
    try {
        if (app) {
            if (app.selected.length == 0) {
                alert('请先择需要操作的记录！');
                return;
            }
            return app.selected
        }
    } catch (error) { }
    var sel = [];
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var rows = dataGrid.datagrid('getSelections');
        if (rows.length == 0) {
            alert('请先择需要操作的记录！');
            return;
        }
        return rows;
    } else {
        var rowindexes = dataGrid.jqxGrid('getselectedrowindexes');
        if (rowindexes.length == 0) {
            alert('请先择需要操作的记录！');
            return;
        }
        $.each(rowindexes, function (idx, obj) {
            var selected = dataGrid.jqxGrid('getrowdata', obj);
            sel.push(selected);
        });
        return sel;
    }
}

function pubselectsV2() {
    try {
        if (app) {
            if (app.selected.length == 0) {
                // alert('请先择需要操作的记录！');
                return;
            }
            return app.selected
        }
    } catch (error) { }
    var sel = [];
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var rows = dataGrid.datagrid('getSelections');
        if (rows.length == 0) {
            //alert('请先择需要操作的记录！');
            return;
        }
        return rows;
    } else {
        var rowindexes = dataGrid.jqxGrid('getselectedrowindexes');
        if (rowindexes.length == 0) {
            //alert('请先择需要操作的记录！');
            return;
        }
        $.each(rowindexes, function (idx, obj) {
            var selected = dataGrid.jqxGrid('getrowdata', obj);
            sel.push(selected);
        });
        return sel;
    }
}

function pubselects2() {
    var sel = [];
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var rows = dataGrid.datagrid('getSelections');
        if (rows.length == 0) {
            return;
        }
        return rows;
    } else {
        var rowindexes = dataGrid.jqxGrid('getselectedrowindexes');
        if (rowindexes.length == 0) {
            return;
        }
        $.each(rowindexes, function (idx, obj) {
            var selected = dataGrid.jqxGrid('getrowdata', obj);
            sel.push(selected);
        });
        return sel;
    }
}

function commonSelect(config) {
    var str = ""
    $.ajax({
        type: "POST",
        url: config.remoteUrl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.code == "0") {
                var s = `<option value="">----请选择----</option>`;
                data.datas.forEach(function (e) {
                    s += `<option value="${e[config.selectConfig.value]}">${e[config.selectConfig.label]}</option>`
                })
                str = `<form class="layui-form" style="padding:10px;box-sizing:border-box;">
                        <div class="layui-form-item" style="">
                            <label class="layui-form-label" style="width:100px;">${config.selectLabelName}</label>
                            <div class="layui-input-block">
                            <select name="interest" lay-filter="aihao" id="J_sel">
                                ${s}
                            </select>
                            </div>
                        </div>
                    </form>
                `
            } else {
                if (data.msg == "")
                    data.msg = "获取失败";
                alert(data.msg);
            }
        },
    });
    layer.open({
        type: 1,
        title: config.layerTitle,
        content: str,
        area: ['500px', '150px'],
        btn: ["保存"],
        success: function () {
            layui.use('form', function () {
                var form = layui.form
                form.render();
                $('.layui-layer-content').css({
                    overflow: 'visible'
                })
            })
        },
        yes: function () {
            config.confirm($('#J_sel').val())
        }
    });
}