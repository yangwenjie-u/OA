
function JudgeKJ() {
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        return "easyui";
    }
    else {
        return "jqx";
    }
}

function pubselect() {
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var selected = dataGrid.datagrid('getSelected');
        if (!selected) {
            alert('请先择需要操作的记录！');
            return;
        }
        return selected;
    }
    else {
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
    var sel = [];
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var rows = dataGrid.datagrid('getSelections');
        if (rows.length == 0) {
            alert('请先择需要操作的记录！');
            return;
        }
        return rows;
    }
    else {
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

function pubselects2() {
    var sel = [];
    if (dataGrid.datagrid && typeof (dataGrid.datagrid) == "function") {
        var rows = dataGrid.datagrid('getSelections');
        if (rows.length == 0) {
            return;
        }
        return rows;
    }
    else {
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
