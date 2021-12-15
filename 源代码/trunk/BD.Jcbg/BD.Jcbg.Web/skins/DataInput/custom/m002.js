function dbclick(row) {
    alert(row.SZSF);
}

function addRecord() {
    //iframe层
    parent.layer.open({
        type: 2,
        title: '数据信息',
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: '/Test/TestAction?Math.random()' //iframe的url
    });
}

function addRecord1() {
    var selected = dataGrid.bootstrapTable('getSelections');

    if (selected.length == 0)
        return;
    alert(selected[0].SZSF + ' ' + selected[0].SZCS + ' ' + selected[0].SZXQ);
}
