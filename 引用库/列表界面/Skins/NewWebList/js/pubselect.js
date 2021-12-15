
function JudgeKJ() {
    return '';
}

function pubselect() {
    /*
var checkStatus = table.checkStatus('idTest'); //test即为基础参数id对应的值
console.log(checkStatus.data) //获取选中行的数据
    */ 

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
