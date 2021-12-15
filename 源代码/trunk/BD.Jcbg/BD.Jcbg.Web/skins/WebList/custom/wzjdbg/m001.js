function dbclick(row) {
    alert(row.SZSF);
}

function addRecord() {
    var selected = dataGrid.bootstrapTable('getSelections');

    if (selected.length == 0)
        return;
    alert(selected[0].SZSF + ' ' + selected[0].SZCS + ' ' + selected[0].SZXQ);
}
